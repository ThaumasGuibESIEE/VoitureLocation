using System;
using System.Runtime.InteropServices;
namespace VoitureLocations.Domain.Entities;

public class Location{
    
    private Client client;

    private Vehicule vehicule;

    private List<Options> options; 

    private int duree; // en jours

    private bool isValid = true;

    private int idLoc;

    private float prix = 0;

    public Location(Client cl, Vehicule veh, List<Options> opts, int dur , int loc){
        client= cl;
        vehicule = veh;
        options = opts;
        duree = dur;
        idLoc = loc;
    }

    public void locIsValid(){
        if (duree >=30)
            isValid = false;
        else
        {
            client.setVoitureLoue(client.getVoitureLoue()+1);
            client.getLocation().Add(idLoc);
            setPrix(getPrix() + vehicule.getPrix());
            foreach (var item in options)
            {
                setPrix(getPrix()+item.getPrix());
            }

            // Augmenter prix via prix opts et vehicule et de la durée
        }

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


}