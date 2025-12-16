using System;

namespace VoitureLocations.Domain.Entities;

public class Facture
{
    private int id;
    private int clientId;
    private int locationId;
    private string clientNom;
    private string vehiculeModele;
    private float montantTotal;
    private DateTime dateEmission;
    private List<string> lignes;

    public Facture(int idFacture, int clientIdFacture, int locationIdFacture, string nomClient, string modeleVehicule, float total, DateTime date, List<string> lignesFacture)
    {
        id = idFacture;
        clientId = clientIdFacture;
        locationId = locationIdFacture;
        clientNom = nomClient;
        vehiculeModele = modeleVehicule;
        montantTotal = total;
        dateEmission = date;
        lignes = lignesFacture;
    }

    public int getId() => id;
    public int getClientId() => clientId;
    public int getLocationId() => locationId;
    public string getClientNom() => clientNom;
    public string getVehiculeModele() => vehiculeModele;
    public float getMontantTotal() => montantTotal;
    public DateTime getDateEmission() => dateEmission;
    public IReadOnlyList<string> getLignes() => lignes;
}
