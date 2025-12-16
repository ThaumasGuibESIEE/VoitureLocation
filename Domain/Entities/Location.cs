using System;
namespace VoitureLocations.Domain.Entities;

public class Location{
    
    private Client client;

    private Vehicule vehicule;

    private List<Options> options; 

    private int duree; // en jours
    private const int MaxLocationsParClient = 3;

    private bool isValid = true;

    private int idLoc;

    private float prix = 0;

    private Location(Client cl, Vehicule veh, List<Options> opts, int dur , int loc){
        client= cl;
        vehicule = veh;
        options = opts;
        duree = dur;
        idLoc = loc;

        if (!locIsValid())
        {
            throw new InvalidOperationException("La duree de la location ne peut pas depasser 30 jours.");
        }
    }

    public static Location Create(Client cl, Vehicule veh, List<Options> opts, int dur , int loc)
    {
        return new Location(cl, veh, opts, dur, loc);
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

        client.setVoitureLoue(client.getVoitureLoue()+1);
        client.getLocation().Add(idLoc);
        setPrix(getPrix() + vehicule.getPrix());
        foreach (var item in options)
        {
            var optionPrix = item.isPrixJournalier() ? item.getPrix() * duree : item.getPrix();
            setPrix(getPrix()+optionPrix);
        }

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

}
