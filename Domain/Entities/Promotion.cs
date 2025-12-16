using System;

namespace VoitureLocations.Domain.Entities;

public class Promotion
{
    private readonly int id;
    private readonly string nom;
    private readonly string description;
    private readonly int pourcentageReduction;

    public Promotion(int idPromo, string nomPromo, string desc, int reductionPourcent)
    {
        if (reductionPourcent < 0 || reductionPourcent > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(reductionPourcent), "La reduction doit etre comprise entre 0 et 100%.");
        }

        id = idPromo;
        nom = string.IsNullOrWhiteSpace(nomPromo) ? $"Promo-{idPromo}" : nomPromo.Trim();
        description = desc ?? string.Empty;
        pourcentageReduction = reductionPourcent;
    }

    public int getId() => id;
    public string getNom() => nom;
    public string getDescription() => description;
    public int getReductionPourcentage() => pourcentageReduction;
}
