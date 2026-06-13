namespace YmmoApi.Models;

/// <summary>Entité sans clé pour le résultat de la requête CTE ventes_agence (CA mensuel glissant par agence).</summary>
public class AgencyMonthlyRevenue
{
    public int AgencyId { get; set; }
    public string AgencyName { get; set; } = string.Empty;
    public DateOnly Month { get; set; }
    public decimal Revenue { get; set; }
    public long RankInAgency { get; set; }
}
