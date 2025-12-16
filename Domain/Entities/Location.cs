using System;
using System.Runtime.InteropServices;
namespace VoitureLocations.Domain.Entities;

public class Location{
    
    private Client client;

    private Vehicule vehicule;

    private string[] options; // Options  a creer

    private int duree; // en jours

    private bool isValid = true;

    private int idLoc;

    public Location(Client cl, Vehicule veh, string[] opts, int dur){
        client= cl;
        vehicule = veh;
        options = opts;
        duree = dur;
    }

    public void locIsValid(){
        if (duree >=30)
            isValid = false;
        else
        {
            client.setVoitureLoue(client.getVoitureLoue()+1);
            client.getLocation().Add(idLoc);
        }

    }

    public int getIdLoc()
    {
        return idLoc;
    }


}