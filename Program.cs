using System;
using VoitureLocations.Domain.Entities;

// Demo : affichage des informations d'un vehicule
var vehicule = new Vehicule(
    modele: "Renault Clio",
    plaque: "AB-123-CD",
    kilometrage: 15_000,
    derniereMaintenance: DateTime.UtcNow.AddMonths(-7),
    kilometrageDerniereMaintenance: 5_000);

vehicule.setPrix(500.00f);

var opt1 = new Options("Assurance Supplementaire", 14.99f, isPrixJournalier: true);
var opt2 = new Options("Siege enfant", 20f, isPrixJournalier: false);
var opt3 = new Options("GPS", 10f, isPrixJournalier: true);

List<Options> opts = new List<Options>();
opts.Add(opt1);
opts.Add(opt2);
opts.Add(opt3);

Console.WriteLine("=== Fiche vehicule ===");
Console.WriteLine($"Modele : {vehicule.Modele}");
Console.WriteLine($"Plaque : {vehicule.Plaque}");
Console.WriteLine($"Kilometrage actuel : {vehicule.Kilometrage} km");
Console.WriteLine($"Derniere maintenance : {vehicule.DerniereMaintenance:yyyy-MM-dd}");
Console.WriteLine($"Kilometrage a la derniere maintenance : {vehicule.KilometrageDerniereMaintenance} km");
Console.WriteLine($"Kilometres depuis maintenance : {vehicule.Kilometrage - vehicule.KilometrageDerniereMaintenance} km");
Console.WriteLine($"Jours depuis maintenance : {(DateTime.UtcNow - vehicule.DerniereMaintenance).TotalDays:F0} jours");
Console.WriteLine($"Maintenance due : {(vehicule.MaintenanceDue() ? "Oui" : "Non")}");

Client thaumas = new Client(true,"Thaumas",1);
thaumas.toString();








Location loc1 = Location.Create(thaumas, vehicule, opts, 29, 1 ) ;

Console.WriteLine();
Console.WriteLine("=== Location en cours ===");
Console.WriteLine($"Client : {loc1.getClient().getNom()} (id {loc1.getClient().getId()}) {(loc1.getClient().isPremium() ? "[Premium]" : string.Empty)}");
Console.WriteLine($"Vehicule loue : {loc1.getVehicule().Modele} - {loc1.getVehicule().Plaque}");
Console.WriteLine($"Duree : {loc1.getDuree()} jours");
Console.WriteLine("Options :");
foreach (var opt in loc1.getOptions())
{
    var optionCost = opt.isPrixJournalier() ? opt.getPrix() * loc1.getDuree() : opt.getPrix();
    var typeLibelle = opt.isPrixJournalier() ? "par jour" : "forfait";
    Console.WriteLine($" - {opt.getNom()} ({typeLibelle}) : {optionCost:0.00} EUR");
}
Console.WriteLine($"Prix total location : {loc1.getPriceToPay():0.00} EUR");
