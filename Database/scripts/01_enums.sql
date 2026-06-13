-- ============================================================
-- 01_enums.sql
-- Types ENUM PostgreSQL natifs correspondant aux enums C#
-- (Models/PropertyType.cs, PropertyStatus.cs, UserRole.cs, VisitStatus.cs)
-- ============================================================

CREATE TYPE property_type AS ENUM (
    'Residential',
    'Commercial',
    'Land',
    'MixedUse'
);

CREATE TYPE property_status AS ENUM (
    'Available',
    'UnderOffer',
    'Sold'
);

CREATE TYPE user_role AS ENUM (
    'Admin',
    'Agent',
    'Client'
);

CREATE TYPE visit_status AS ENUM (
    'Scheduled',
    'Completed',
    'Cancelled'
);

CREATE TYPE transaction_stage AS ENUM (
    'Interest',
    'Visit',
    'Offer',
    'Compromise',
    'Deed'
);
