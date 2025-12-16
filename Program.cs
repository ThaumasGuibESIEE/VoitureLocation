using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VoitureLocations.Domain.Entities;

// Jeu de donnees initial
var clients = new List<Client>
{
    new Client(premium2: true, nom2: "Thaumas", id2: 1)
};

var vehicules = new List<Vehicule>
{
    new Vehicule("Renault Clio", "AB-123-CD", 15_000, DateTime.UtcNow.AddMonths(-7), 5_000) { },
    new Vehicule("Peugeot 208", "BC-456-EF", 8_000, DateTime.UtcNow.AddMonths(-2), 2_000) { },
    new Vehicule("Tesla Model 3", "EV-789-GH", 20_000, DateTime.UtcNow.AddMonths(-4), 12_000) { }
};
vehicules[0].setPrix(500f);
vehicules[1].setPrix(450f);
vehicules[2].setPrix(900f);

var optionsCatalogue = new List<Options>
{
    new Options("Assurance Supplementaire", 14.99f, isPrixJournalier: true),
    new Options("Siege enfant", 20f, isPrixJournalier: false),
    new Options("GPS", 10f, isPrixJournalier: true),
    new Options("Conducteur additionnel", 9.99f, isPrixJournalier: true)
};

var promotions = new List<Promotion>
{
    new Promotion(1, "Bienvenue", "Remise premiere location", 10)
};

var locations = new Dictionary<int, Location>();

Console.WriteLine("=== Voiture Locations CLI ===");

bool running = true;
while (running)
{
    AfficherMenu();
    var choix = Console.ReadLine();
    switch (choix)
    {
        case "1":
            CreerClient(clients);
            break;
        case "2":
            CreerPromotion(promotions);
            break;
        case "3":
            CreerLocation(clients, vehicules, optionsCatalogue, promotions, locations);
            break;
        case "4":
            AfficherHistoriqueClient(clients, locations);
            break;
        case "5":
            AfficherParcVehicules(vehicules);
            break;
        case "0":
            running = false;
            break;
        default:
            Console.WriteLine("Choix invalide.");
            break;
    }

    Console.WriteLine();
}

Console.WriteLine("Au revoir !");

// --- Fonctions CLI ---

void AfficherMenu()
{
    Console.WriteLine("1) Creer un client");
    Console.WriteLine("2) Creer une promotion");
    Console.WriteLine("3) Creer une location");
    Console.WriteLine("4) Voir l'historique et la location en cours d'un client");
    Console.WriteLine("5) Voir le parc de vehicules");
    Console.WriteLine("0) Quitter");
    Console.Write("Votre choix : ");
}

void CreerClient(List<Client> clientsList)
{
    Console.Write("Nom du client : ");
    var nom = Console.ReadLine() ?? string.Empty;
    Console.Write("Client premium ? (o/n) : ");
    var premiumInput = Console.ReadLine();
    var premium = premiumInput != null && premiumInput.Trim().ToLowerInvariant().StartsWith("o");
    var id = clientsList.Count == 0 ? 1 : clientsList.Max(c => c.getId()) + 1;
    var client = new Client(premium, nom, id);
    clientsList.Add(client);
    Console.WriteLine($"Client cree avec id {id}");
}

void CreerPromotion(List<Promotion> promos)
{
    Console.Write("Nom de la promotion : ");
    var nom = Console.ReadLine() ?? string.Empty;
    Console.Write("Description : ");
    var desc = Console.ReadLine() ?? string.Empty;
    Console.Write("Reduction en % : ");
    var reductionStr = Console.ReadLine();
    if (!int.TryParse(reductionStr, out var reduction))
    {
        Console.WriteLine("Reduction invalide.");
        return;
    }

    var id = promos.Count == 0 ? 1 : promos.Max(p => p.getId()) + 1;
    try
    {
        var promo = new Promotion(id, nom, desc, reduction);
        promos.Add(promo);
        Console.WriteLine($"Promotion creee avec id {id}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de la creation : {ex.Message}");
    }
}

void CreerLocation(
    List<Client> clientsList,
    List<Vehicule> vehiculesList,
    List<Options> optionsList,
    List<Promotion> promos,
    Dictionary<int, Location> locs)
{
    if (clientsList.Count == 0 || vehiculesList.Count == 0)
    {
        Console.WriteLine("Aucun client ou aucun vehicule disponible.");
        return;
    }

    Console.WriteLine("Clients disponibles :");
    foreach (var c in clientsList)
    {
        Console.WriteLine($" {c.getId()}) {c.getNom()} {(c.isPremium() ? "[Premium]" : string.Empty)}");
    }
    Console.Write("Choisissez l'id du client : ");
    if (!int.TryParse(Console.ReadLine(), out var clientId))
    {
        Console.WriteLine("Id invalide.");
        return;
    }
    var client = clientsList.FirstOrDefault(c => c.getId() == clientId);
    if (client == null)
    {
        Console.WriteLine("Client introuvable.");
        return;
    }

    Console.WriteLine("Vehicules disponibles :");
    for (int i = 0; i < vehiculesList.Count; i++)
    {
        Console.WriteLine($" {i+1}) {vehiculesList[i].Modele} ({vehiculesList[i].Plaque}) - {vehiculesList[i].getPrix():0.00} EUR");
    }
    Console.Write("Choisissez le vehicule (numero) : ");
    if (!int.TryParse(Console.ReadLine(), out var vehIndex) || vehIndex < 1 || vehIndex > vehiculesList.Count)
    {
        Console.WriteLine("Choix invalide.");
        return;
    }
    var vehicule = vehiculesList[vehIndex - 1];

    Console.Write("Duree (jours) : ");
    if (!int.TryParse(Console.ReadLine(), out var duree))
    {
        Console.WriteLine("Duree invalide.");
        return;
    }

    Console.WriteLine("Options disponibles (separer par des virgules, ex: 1,3) ou laisser vide :");
    for (int i = 0; i < optionsList.Count; i++)
    {
        var opt = optionsList[i];
        var type = opt.isPrixJournalier() ? "jour" : "forfait";
        Console.WriteLine($" {i+1}) {opt.getNom()} - {opt.getPrix():0.00} EUR ({type})");
    }
    Console.Write("Choix : ");
    var choixOpts = Console.ReadLine() ?? string.Empty;
    var selectedOptions = new List<Options>();
    if (!string.IsNullOrWhiteSpace(choixOpts))
    {
        var tokens = choixOpts.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var token in tokens)
        {
            if (int.TryParse(token.Trim(), out var optIndex) && optIndex >=1 && optIndex <= optionsList.Count)
            {
                selectedOptions.Add(optionsList[optIndex-1]);
            }
        }
    }

    Promotion? promoChoisie = null;
    if (promos.Count > 0)
    {
        Console.WriteLine("Promotions disponibles (laisser vide pour aucune) :");
        foreach (var p in promos)
        {
            Console.WriteLine($" {p.getId()}) {p.getNom()} - {p.getReductionPourcentage()}% ({p.getDescription()})");
        }
        Console.Write("Id de la promotion : ");
        var promoInput = Console.ReadLine();
        if (int.TryParse(promoInput, out var promoId))
        {
            promoChoisie = promos.FirstOrDefault(p => p.getId() == promoId);
        }
    }

    Console.Write("Depot de garantie verse (EUR, min 300) : ");
    if (!float.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out var depot))
    {
        Console.WriteLine("Montant invalide.");
        return;
    }

    var locId = locs.Count == 0 ? 1 : locs.Keys.Max() + 1;
    try
    {
        var location = Location.Create(client, vehicule, selectedOptions, duree, locId, depot, promoChoisie);
        locs[locId] = location;
        Console.WriteLine($"Location creee avec id {locId}. Prix a payer : {location.getPriceToPay():0.00} EUR");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Echec de creation : {ex.Message}");
    }
}

void AfficherHistoriqueClient(List<Client> clientsList, Dictionary<int, Location> locs)
{
    if (clientsList.Count == 0)
    {
        Console.WriteLine("Aucun client.");
        return;
    }

    Console.WriteLine("Clients :");
    foreach (var c in clientsList)
    {
        Console.WriteLine($" {c.getId()}) {c.getNom()}");
    }
    Console.Write("Id du client : ");
    if (!int.TryParse(Console.ReadLine(), out var clientId))
    {
        Console.WriteLine("Id invalide.");
        return;
    }
    var client = clientsList.FirstOrDefault(c => c.getId() == clientId);
    if (client == null)
    {
        Console.WriteLine("Client introuvable.");
        return;
    }

    Console.WriteLine("Factures :");
    foreach (var facture in client.getFactures())
    {
        Console.WriteLine($"- Facture #{facture.getId()} (Location {facture.getLocationId()}) {facture.getDateEmission():yyyy-MM-dd} : {facture.getMontantTotal():0.00} EUR");
    }
    if (client.getFactures().Count == 0)
    {
        Console.WriteLine("Aucune facture.");
    }

    Console.WriteLine("Location en cours (ids) :");
    foreach (var locId in client.getLocation())
    {
        if (locs.TryGetValue(locId, out var loc))
        {
            Console.WriteLine($"- Location {locId} : {loc.getVehicule().Modele} ({loc.getVehicule().Plaque}), duree {loc.getDuree()} jours, prix {loc.getPriceToPay():0.00} EUR, depot {loc.getDepotMontant():0.00} EUR");
        }
        else
        {
            Console.WriteLine($"- Location {locId} (details indisponibles)");
        }
    }
    if (client.getLocation().Count == 0)
    {
        Console.WriteLine("Aucune location enregistree.");
    }
}

void AfficherParcVehicules(List<Vehicule> vehiculesList)
{
    Console.WriteLine("Parc de vehicules :");
    foreach (var v in vehiculesList)
    {
        var statut = v.EstLouee() ? "Loue" : "Disponible";
        Console.WriteLine($"- {v.Modele} ({v.Plaque}) | Km: {v.Kilometrage} | Prix: {v.getPrix():0.00} EUR | Statut: {statut} | Maintenance due: {(v.MaintenanceDue() ? "Oui" : "Non")}");
    }
}
