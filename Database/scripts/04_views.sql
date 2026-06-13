-- ============================================================
-- 04_views.sql
-- Vues métier : biens disponibles, transactions par agence, KPI mensuel
-- ============================================================

-- ------------------------------------------------------------
-- vw_biens_disponibles
-- Biens actuellement disponibles, avec les infos de l'agence
-- ------------------------------------------------------------
CREATE VIEW vw_biens_disponibles AS
SELECT
    p."Id"          AS "PropertyId",
    p."Title",
    p."Type",
    p."Price",
    p."City",
    p."Bedrooms",
    p."Area",
    p."DpeRating",
    p."ListedDate",
    a."Id"          AS "AgencyId",
    a."Name"        AS "AgencyName",
    a."City"        AS "AgencyCity"
FROM "Properties" p
JOIN "Agencies" a ON a."Id" = p."AgencyId"
WHERE p."Status" = 'Available';

-- ------------------------------------------------------------
-- vw_transactions_agence
-- Chiffre d'affaires et nombre de ventes par agence (CTE)
-- ------------------------------------------------------------
CREATE VIEW vw_transactions_agence AS
WITH ventes_agence AS (
    SELECT
        p."AgencyId",
        s."Id"        AS "SaleId",
        s."SalePrice",
        s."Date"
    FROM "Sales" s
    JOIN "Properties" p ON p."Id" = s."PropertyId"
)
SELECT
    a."Id"                       AS "AgencyId",
    a."Name"                     AS "AgencyName",
    COUNT(v."SaleId")            AS "SalesCount",
    COALESCE(SUM(v."SalePrice"), 0)  AS "TotalRevenue",
    COALESCE(AVG(v."SalePrice"), 0)  AS "AverageSalePrice",
    MAX(v."Date")                AS "LastSaleDate"
FROM "Agencies" a
LEFT JOIN ventes_agence v ON v."AgencyId" = a."Id"
GROUP BY a."Id", a."Name";

-- ------------------------------------------------------------
-- vw_kpi_mensuel
-- KPI mensuel des ventes : CA du mois, nombre de ventes,
-- cumul (running total) et part du CA total (fenêtrage OVER/PARTITION BY)
-- ------------------------------------------------------------
CREATE VIEW vw_kpi_mensuel AS
WITH ventes_mensuelles AS (
    SELECT
        date_trunc('month', s."Date")::date AS "Month",
        s."Id"                               AS "SaleId",
        s."SalePrice"
    FROM "Sales" s
)
SELECT
    "Month",
    COUNT("SaleId")                                              AS "SalesCount",
    SUM("SalePrice")                                             AS "MonthlyRevenue",
    SUM(SUM("SalePrice")) OVER (ORDER BY "Month")                AS "CumulativeRevenue",
    ROUND(
        100.0 * SUM("SalePrice") / SUM(SUM("SalePrice")) OVER (), 2
    )                                                             AS "PercentOfTotalRevenue"
FROM ventes_mensuelles
GROUP BY "Month"
ORDER BY "Month";
