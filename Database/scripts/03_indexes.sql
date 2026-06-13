-- ============================================================
-- 03_indexes.sql
-- Index de performance pour les filtres les plus fréquents
-- (recherche de biens : statut, prix, agence, ville)
-- ============================================================

CREATE INDEX "IX_Properties_Status"   ON "Properties" ("Status");
CREATE INDEX "IX_Properties_Price"    ON "Properties" ("Price");
CREATE INDEX "IX_Properties_AgencyId" ON "Properties" ("AgencyId");
CREATE INDEX "IX_Properties_City"     ON "Properties" ("City");

-- Index composé pour le cas fréquent "biens disponibles par ville triés par prix"
CREATE INDEX "IX_Properties_Status_City_Price"
    ON "Properties" ("Status", "City", "Price");

-- Index sur les clés étrangères des tables liées
CREATE INDEX "IX_Visits_PropertyId"  ON "Visits" ("PropertyId");
CREATE INDEX "IX_Visits_AgentId"     ON "Visits" ("AgentId");
CREATE INDEX "IX_Visits_ClientId"    ON "Visits" ("ClientId");
CREATE INDEX "IX_Sales_PropertyId"   ON "Sales" ("PropertyId");
CREATE INDEX "IX_Photos_PropertyId"  ON "Photos" ("PropertyId");

-- Index sur les clés étrangères des transactions
CREATE INDEX "IX_Transactions_PropertyId"            ON "Transactions" ("PropertyId");
CREATE INDEX "IX_Transactions_ClientId"              ON "Transactions" ("ClientId");
CREATE INDEX "IX_Transactions_AgentId"               ON "Transactions" ("AgentId");
CREATE INDEX "IX_TransactionStageHistories_TransactionId" ON "TransactionStageHistories" ("TransactionId");
CREATE INDEX "IX_TransactionDocuments_TransactionId" ON "TransactionDocuments" ("TransactionId");
