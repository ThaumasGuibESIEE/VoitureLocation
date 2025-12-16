using VoitureLocations.Domain.Entities;

// Démo : affichage des informations d'un véhicule
var vehicule = new Vehicule(
    modele: "Renault Clio",
    plaque: "AB-123-CD",
    kilometrage: 15_000,
    derniereMaintenance: DateTime.UtcNow.AddMonths(-7),
    kilometrageDerniereMaintenance: 5_000);

Console.WriteLine("=== Fiche véhicule ===");
Console.WriteLine($"Modèle : {vehicule.Modele}");
Console.WriteLine($"Plaque : {vehicule.Plaque}");
Console.WriteLine($"Kilométrage actuel : {vehicule.Kilometrage} km");
Console.WriteLine($"Dernière maintenance : {vehicule.DerniereMaintenance:yyyy-MM-dd}");
Console.WriteLine($"Kilométrage à la dernière maintenance : {vehicule.KilometrageDerniereMaintenance} km");
Console.WriteLine($"Kilomètres depuis maintenance : {vehicule.Kilometrage - vehicule.KilometrageDerniereMaintenance} km");
Console.WriteLine($"Jours depuis maintenance : {(DateTime.UtcNow - vehicule.DerniereMaintenance).TotalDays:F0} jours");
Console.WriteLine($"Maintenance due : {(vehicule.MaintenanceDue() ? "Oui" : "Non")}");
