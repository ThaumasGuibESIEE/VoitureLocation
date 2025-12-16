

public class Client{

    private int id;

    private string nom;

    private int voitureLoue = 0;
    private List<int> location = new List<int>();

    private  bool premium = false;

    public Client(bool premium2, string nom2, int id2){
        premium = premium2;
        nom = nom2;
         id = id2;

    }

    public void toString(){
        Console.WriteLine("Je suis " + nom + "et mon id est " + id);
        
    }

    public void setVoitureLoue(int i)
    {
        voitureLoue = i;
    }

    public int getVoitureLoue()
    {
        return voitureLoue;
    }

    public List<int> getLocation()
    {
        return location;
    }

    public string getNom()
    {
        return nom;
    }

    public int getId()
    {
        return id;
    }

    public bool isPremium()
    {
        return premium;
    }


}
