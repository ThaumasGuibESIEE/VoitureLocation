public class Options{
    
    private string nom;

    private float prix;


    public Options(string n, float p){
        nom = n;
        prix = p;
    }
    
    public float getPrix()
    {
        return prix;
    }
}