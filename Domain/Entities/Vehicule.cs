using System;

namespace VoitureLocations.Domain.Entities;

/// <summary>
/// Représente un véhicule de la flotte.
/// Une maintenance est requise tous les 10 000 km ou tous les 6 mois.
/// </summary>
public sealed class Vehicule
{
    private const int IntervalleKilometresMaintenance = 10_000;
    private static readonly TimeSpan IntervalleTempsMaintenance = TimeSpan.FromDays(182.5); // ≈ 6 mois
    private float prix;
    public Vehicule(string modele, string plaque, int kilometrage, DateTime? derniereMaintenance = null, int? kilometrageDerniereMaintenance = null)
    {
        Modele = string.IsNullOrWhiteSpace(modele)
            ? throw new ArgumentException("Le modèle est requis.", nameof(modele))
            : modele.Trim();

        Plaque = string.IsNullOrWhiteSpace(plaque)
            ? throw new ArgumentException("La plaque est requise.", nameof(plaque))
            : plaque.Trim().ToUpperInvariant();

        if (kilometrage < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(kilometrage), "Le kilométrage ne peut pas être négatif.");
        }

        Kilometrage = kilometrage;
        DerniereMaintenance = derniereMaintenance?.ToUniversalTime() ?? DateTime.UtcNow;
        KilometrageDerniereMaintenance = kilometrageDerniereMaintenance ?? kilometrage;
    }

    public string Modele { get; }

    public string Plaque { get; }

    /// <summary>
    /// Kilométrage actuel du véhicule.
    /// </summary>
    public int Kilometrage { get; private set; }

    /// <summary>
    /// Date de la dernière maintenance effectuée (UTC).
    /// </summary>
    public DateTime DerniereMaintenance { get; private set; }

    /// <summary>
    /// Kilométrage lors de la dernière maintenance.
    /// </summary>
    public int KilometrageDerniereMaintenance { get; private set; }

    /// <summary>
    /// Met à jour le kilométrage courant.
    /// </summary>
    public void MettreAJourKilometrage(int nouveauKilometrage)
    {
        if (nouveauKilometrage < Kilometrage)
        {
            throw new ArgumentOutOfRangeException(nameof(nouveauKilometrage), "Le kilométrage ne peut pas diminuer.");
        }

        Kilometrage = nouveauKilometrage;
    }

    /// <summary>
    /// Indique si une maintenance est due selon les seuils kilométrique ou temporel.
    /// </summary>
    public bool MaintenanceDue(DateTime? dateReference = null)
    {
        var reference = (dateReference ?? DateTime.UtcNow).ToUniversalTime();

        var dueParKilometrage = Kilometrage - KilometrageDerniereMaintenance >= IntervalleKilometresMaintenance;
        var dueParTemps = reference - DerniereMaintenance >= IntervalleTempsMaintenance;

        return dueParKilometrage || dueParTemps;
    }

    /// <summary>
    /// Marque la maintenance comme effectuée à la date courante et kilométrage actuel.
    /// </summary>
    public void MarquerMaintenanceEffectuee(DateTime? dateMaintenance = null)
    {
        DerniereMaintenance = (dateMaintenance ?? DateTime.UtcNow).ToUniversalTime();
        KilometrageDerniereMaintenance = Kilometrage;
    }

    public float getPrix()
    {
        return prix;
    }

    public void setPrix(float a)
    {
        prix = a;
    }
}
