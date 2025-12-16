
using VoitureLocations.Domain.Entities;

// Démo : affichage des informations d'un véhicule
var vehicule = new Vehicule(
    modele: "Renault Clio",
    plaque: "AB-123-CD",
    kilometrage: 15_000,
    derniereMaintenance: DateTime.UtcNow.AddMonths(-7),
    kilometrageDerniereMaintenance: 5_000);

vehicule.setPrix(500.00f);

var opt1 = new Options("Assurance Supplémentaire", 149.99f);
var opt2 = new Options("Siege enfant", 20f);
var opt3 = new Options("GPS", 120f);


List<Options> opts = new List<Options>();
opts.Add(opt1);
opts.Add(opt2);
opts.Add(opt3);

Console.WriteLine("=== Fiche véhicule ===");
Console.WriteLine($"Modèle : {vehicule.Modele}");
Console.WriteLine($"Plaque : {vehicule.Plaque}");
Console.WriteLine($"Kilométrage actuel : {vehicule.Kilometrage} km");
Console.WriteLine($"Dernière maintenance : {vehicule.DerniereMaintenance:yyyy-MM-dd}");
Console.WriteLine($"Kilométrage à la dernière maintenance : {vehicule.KilometrageDerniereMaintenance} km");
Console.WriteLine($"Kilomètres depuis maintenance : {vehicule.Kilometrage - vehicule.KilometrageDerniereMaintenance} km");
Console.WriteLine($"Jours depuis maintenance : {(DateTime.UtcNow - vehicule.DerniereMaintenance).TotalDays:F0} jours");
Console.WriteLine($"Maintenance due : {(vehicule.MaintenanceDue() ? "Oui" : "Non")}");

Console.WriteLine("Hello, World!");

Client thaumas = new Client(true,"Thaumas",1);

thaumas.toString();



Location loc1 = new Location(thaumas, vehicule, opts, 29, 1 ) ;


loc1.locIsValid();

Console.WriteLine(loc1.getPrix());


