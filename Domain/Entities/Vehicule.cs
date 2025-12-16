using System;

namespace VoitureLocations.Domain.Entities;

/// <summary>
/// Represente un vehicule de la flotte.
/// Une maintenance est requise tous les 10 000 km ou tous les 6 mois.
/// </summary>
public sealed class Vehicule
{
    private const int IntervalleKilometresMaintenance = 10_000;
    private static readonly TimeSpan IntervalleTempsMaintenance = TimeSpan.FromDays(182.5); // ~ 6 mois
    private float prix;
    private bool estLouee;

    public Vehicule(string modele, string plaque, int kilometrage, DateTime? derniereMaintenance = null, int? kilometrageDerniereMaintenance = null)
    {
        Modele = string.IsNullOrWhiteSpace(modele)
            ? throw new ArgumentException("Le modele est requis.", nameof(modele))
            : modele.Trim();

        Plaque = string.IsNullOrWhiteSpace(plaque)
            ? throw new ArgumentException("La plaque est requise.", nameof(plaque))
            : plaque.Trim().ToUpperInvariant();

        if (kilometrage < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(kilometrage), "Le kilometrage ne peut pas etre negatif.");
        }

        Kilometrage = kilometrage;
        DerniereMaintenance = derniereMaintenance?.ToUniversalTime() ?? DateTime.UtcNow;
        KilometrageDerniereMaintenance = kilometrageDerniereMaintenance ?? kilometrage;
    }

    public string Modele { get; }

    public string Plaque { get; }

    /// <summary>
    /// Kilometrage actuel du vehicule.
    /// </summary>
    public int Kilometrage { get; private set; }

    /// <summary>
    /// Date de la derniere maintenance effectuee (UTC).
    /// </summary>
    public DateTime DerniereMaintenance { get; private set; }

    /// <summary>
    /// Kilometrage lors de la derniere maintenance.
    /// </summary>
    public int KilometrageDerniereMaintenance { get; private set; }

    /// <summary>
    /// Met a jour le kilometrage courant.
    /// </summary>
    public void MettreAJourKilometrage(int nouveauKilometrage)
    {
        if (nouveauKilometrage < Kilometrage)
        {
            throw new ArgumentOutOfRangeException(nameof(nouveauKilometrage), "Le kilometrage ne peut pas diminuer.");
        }

        Kilometrage = nouveauKilometrage;
    }

    /// <summary>
    /// Indique si une maintenance est due selon les seuils kilometrique ou temporel.
    /// </summary>
    public bool MaintenanceDue(DateTime? dateReference = null)
    {
        var reference = (dateReference ?? DateTime.UtcNow).ToUniversalTime();

        var dueParKilometrage = Kilometrage - KilometrageDerniereMaintenance >= IntervalleKilometresMaintenance;
        var dueParTemps = reference - DerniereMaintenance >= IntervalleTempsMaintenance;

        return dueParKilometrage || dueParTemps;
    }

    /// <summary>
    /// Marque la maintenance comme effectuee a la date courante et kilometrage actuel.
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

    public bool EstLouee()
    {
        return estLouee;
    }

    public void MarquerLouee()
    {
        estLouee = true;
    }

    public void MarquerDisponible()
    {
        estLouee = false;
    }
}
