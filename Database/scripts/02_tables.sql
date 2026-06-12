-- ============================================================
-- 02_tables.sql
-- Tables principales avec PK, FK, UNIQUE et CHECK
-- Correspond au schéma EF Core (Models/, Data/YmmoDbContext.cs)
-- ============================================================

CREATE TABLE "Agencies" (
    "Id"      SERIAL PRIMARY KEY,
    "Name"    TEXT NOT NULL,
    "City"    TEXT NOT NULL,
    "Address" TEXT NOT NULL,
    "Phone"   TEXT NOT NULL,
    "Email"   TEXT NOT NULL UNIQUE
);

CREATE TABLE "Clients" (
    "Id"            SERIAL PRIMARY KEY,
    "FirstName"     TEXT NOT NULL,
    "LastName"      TEXT NOT NULL,
    "Email"         TEXT NOT NULL UNIQUE,
    "Phone"         TEXT NOT NULL,
    "Role"          TEXT NOT NULL,
    "PreferredCity" TEXT NOT NULL
);

CREATE TABLE "Users" (
    "Id"           SERIAL PRIMARY KEY,
    "FirstName"    TEXT NOT NULL,
    "LastName"     TEXT NOT NULL,
    "Email"        TEXT NOT NULL UNIQUE,
    "PasswordHash" TEXT NOT NULL,
    "Phone"        TEXT NOT NULL,
    "Role"         user_role NOT NULL,
    "AgencyId"     INTEGER NULL
        REFERENCES "Agencies" ("Id") ON DELETE SET NULL
);

CREATE TABLE "Properties" (
    "Id"          SERIAL PRIMARY KEY,
    "Title"       TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "Price"       NUMERIC(18,2) NOT NULL CHECK ("Price" >= 0),
    "Type"        property_type NOT NULL,
    "Status"      property_status NOT NULL,
    "AgencyId"    INTEGER NOT NULL
        REFERENCES "Agencies" ("Id") ON DELETE RESTRICT,
    "City"        TEXT NOT NULL,
    "Bedrooms"    INTEGER NOT NULL CHECK ("Bedrooms" >= 0),
    "Area"        DOUBLE PRECISION NOT NULL CHECK ("Area" > 0),
    "ImageUrl"    TEXT NULL,
    "ListedDate"  DATE NOT NULL,
    "CreatedAt"   TIMESTAMPTZ NOT NULL DEFAULT now(),
    "UpdatedAt"   TIMESTAMPTZ NULL
);

CREATE TABLE "Sales" (
    "Id"        SERIAL PRIMARY KEY,
    "PropertyId" INTEGER NOT NULL
        REFERENCES "Properties" ("Id") ON DELETE CASCADE,
    "BuyerId"   INTEGER NOT NULL
        REFERENCES "Clients" ("Id") ON DELETE RESTRICT,
    "SellerId"  INTEGER NOT NULL
        REFERENCES "Clients" ("Id") ON DELETE RESTRICT,
    "SalePrice" NUMERIC(18,2) NOT NULL CHECK ("SalePrice" >= 0),
    "Date"      DATE NOT NULL,
    "Comment"   TEXT NOT NULL,
    CHECK ("BuyerId" <> "SellerId")
);

CREATE TABLE "Visits" (
    "Id"          SERIAL PRIMARY KEY,
    "PropertyId"  INTEGER NOT NULL
        REFERENCES "Properties" ("Id") ON DELETE CASCADE,
    "ClientId"    INTEGER NOT NULL
        REFERENCES "Clients" ("Id") ON DELETE CASCADE,
    "AgentId"     INTEGER NULL
        REFERENCES "Users" ("Id") ON DELETE SET NULL,
    "ScheduledAt" TIMESTAMPTZ NOT NULL,
    "Status"      visit_status NOT NULL DEFAULT 'Scheduled',
    "Notes"       TEXT NOT NULL
);

CREATE TABLE "Photos" (
    "Id"         SERIAL PRIMARY KEY,
    "PropertyId" INTEGER NOT NULL
        REFERENCES "Properties" ("Id") ON DELETE CASCADE,
    "Url"        TEXT NOT NULL,
    "IsMain"     BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE "Messages" (
    "Id"          SERIAL PRIMARY KEY,
    "SenderId"    INTEGER NOT NULL
        REFERENCES "Users" ("Id") ON DELETE RESTRICT,
    "RecipientId" INTEGER NOT NULL
        REFERENCES "Users" ("Id") ON DELETE RESTRICT,
    "Content"     TEXT NOT NULL,
    "SentAt"      TIMESTAMPTZ NOT NULL DEFAULT now(),
    "IsRead"      BOOLEAN NOT NULL DEFAULT FALSE,
    CHECK ("SenderId" <> "RecipientId")
);
