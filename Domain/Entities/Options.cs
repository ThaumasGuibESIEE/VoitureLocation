namespace VoitureLocations.Domain.Entities;

public class Options{
    
    private string nom;

    private float prix;
    private bool prixJournalier;


    public Options(string n, float p, bool isPrixJournalier = false){
        nom = n;
        prix = p;
        prixJournalier = isPrixJournalier;
    }

    public string getNom()
    {
        return nom;
    }
    
    public float getPrix()
    {
        return prix;
    }

    public bool isPrixJournalier()
    {
        return prixJournalier;
    }
}
