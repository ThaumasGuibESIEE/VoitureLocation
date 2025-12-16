using System;
using System.Collections.Generic;
namespace VoitureLocations.Domain.Entities;

public class Location{
    
    private Client client;

    private Vehicule vehicule;

    private List<Options> options; 

    private int duree; // en jours
    private const int MaxLocationsParClient = 3;
    private const float DepotMinimum = 300f;

    private bool isValid = true;

    private int idLoc;

    private float prix = 0;

    private int reduction = 0 ; // en pourcentage
    private int reductionPromotion = 0; // en pourcentage

    private float priceToPay ;

    private float depotMontant;
    private bool depotVerse;
    private bool inspectionRetourEffectuee;

    private Promotion? promotion;

    public Location(Client cl, Vehicule veh, List<Options> opts, int dur , int loc, float depot, Promotion? promo = null){
        client= cl;
        vehicule = veh;
        options = opts;
        duree = dur;
        idLoc = loc;
        depotMontant = depot;
        promotion = promo;

        if (!locIsValid())
        {
            throw new InvalidOperationException("La duree de la location ne peut pas depasser 30 jours.");
        }
    }

    public static Location Create(Client cl, Vehicule veh, List<Options> opts, int dur , int loc, float depot, Promotion? promo = null)
    {
        return new Location(cl, veh, opts, dur, loc, depot, promo);
    }

    private bool locIsValid(){
        if (duree >=30)
        {
            isValid = false;
            return false;
        }

        if (client.getVoitureLoue() >= MaxLocationsParClient)
        {           
            isValid = false;
            throw new InvalidOperationException($"Le client ne peut pas avoir plus de {MaxLocationsParClient} locations simultanées.");
        }

        if (depotMontant < DepotMinimum)
        {
            isValid = false;
            throw new InvalidOperationException($"Depot de garantie insuffisant (minimum {DepotMinimum:0.00} EUR).");
        }

        depotVerse = true;

        if (vehicule.EstLouee())
        {
            isValid = false;
            throw new InvalidOperationException("Le vehicule est deja loue.");
        }
        vehicule.MarquerLouee();

        client.setVoitureLoue(client.getVoitureLoue()+1);
        client.getLocation().Add(idLoc);
        var lignesFacture = new List<string>();
        setPrix(getPrix() + vehicule.getPrix());
        lignesFacture.Add($"Vehicule {vehicule.Modele} ({vehicule.Plaque}) : {vehicule.getPrix():0.00} EUR");
        foreach (var item in options)
        {
            var optionPrix = item.isPrixJournalier() ? item.getPrix() * duree : item.getPrix();
            setPrix(getPrix()+optionPrix);
            var libelleType = item.isPrixJournalier() ? "par jour" : "forfait";
            lignesFacture.Add($"{item.getNom()} ({libelleType}) : {optionPrix:0.00} EUR");
        }
        if (duree>= 7)
        {
            reduction = 15;
            lignesFacture.Add($"Reduction long sejour ({reduction}%): -{(getPrix() * (reduction/100f)):0.00} EUR");
        }
        if (promotion != null)
        {
            reductionPromotion = promotion.getReductionPourcentage();
            if (reductionPromotion > 0)
            {
                lignesFacture.Add($"Promotion \"{promotion.getNom()}\" (-{reductionPromotion}%): -{(getPrix() * (reductionPromotion/100f)):0.00} EUR");
            }
        }
        calcPriceToPay();
        lignesFacture.Add($"Depot de garantie versé : {depotMontant:0.00} EUR (non inclus dans le total)");
        lignesFacture.Add($"Total : {getPriceToPay():0.00} EUR");

        var facture = new Facture(
            idFacture: client.getFactures().Count + 1,
            clientIdFacture: client.getId(),
            locationIdFacture: idLoc,
            nomClient: client.getNom(),
            modeleVehicule: vehicule.Modele,
            total: getPriceToPay(),
            date: DateTime.UtcNow,
            lignesFacture: lignesFacture
        );
        client.addFacture(facture);

        isValid = true;
        return true;
    }

    public int getIdLoc()
    {
        return idLoc;
    }


    public float getPrix()
    {
        return prix;
    }

    public void setPrix(float f)
    {
        prix = f;
    }
    public void setReduction(int x)
    {
        reduction = x;
    }

    public void setPriceToPay(float f)
    {
        priceToPay = f;
    }
    public float getPriceToPay()
    {
        return priceToPay;
    }
    public int getReduction()
    {
        return reduction;
    }
    public int getReductionPromotion()
    {
        return reductionPromotion;
    }
    public void calcPriceToPay()
    {
        var totalReduction = getReduction() + getReductionPromotion();
        var reductionDecimal = totalReduction / 100f;
        reductionDecimal = Math.Min(reductionDecimal, 1f);
        float var = getPrix() * reductionDecimal ;
        setPriceToPay(getPrix()- var);
    }
    public bool IsValid()
    {
        return isValid;
    }

    public Client getClient()
    {
        return client;
    }

    public Vehicule getVehicule()
    {
        return vehicule;
    }

    public IReadOnlyList<Options> getOptions()
    {
        return options;
    }

    public int getDuree()
    {
        return duree;
    }

    public float getDepotMontant()
    {
        return depotMontant;
    }

    public bool isDepotVerse()
    {
        return depotVerse;
    }

    public void marquerInspectionRetourEffectuee()
    {
        inspectionRetourEffectuee = true;
        vehicule.MarquerDisponible();
    }

    public bool isInspectionRetourEffectuee()
    {
        return inspectionRetourEffectuee;
    }

}
