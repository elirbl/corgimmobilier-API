-- ============================================================
-- 06_seed.sql
-- Données de test : complète le seed EF Core (HasData) pour
-- atteindre 12 agences, 5 utilisateurs et 20 biens au total.
--
-- Pré-requis : migrations EF appliquées (InitialCreate seed déjà
-- 5 agences [Id 1-5] et 10 biens [Id 1-10] via HasData).
-- Ce script ajoute 7 agences, 5 utilisateurs et 10 biens
-- supplémentaires (Ids générés par les séquences SERIAL).
-- ============================================================

-- 7 agences supplémentaires (5 existantes + 7 = 12)
INSERT INTO "Agencies" ("Name", "City", "Address", "Phone", "Email") VALUES
    ('Ymmo Nantes',      'Nantes',      '5 Quai de la Fosse',          '+33 2 40 00 00 06', 'nantes@ymmo.fr'),
    ('Ymmo Toulouse',    'Toulouse',    '18 Rue Alsace-Lorraine',      '+33 5 61 00 00 07', 'toulouse@ymmo.fr'),
    ('Ymmo Lille',       'Lille',       '7 Place du Général de Gaulle', '+33 3 20 00 00 08', 'lille@ymmo.fr'),
    ('Ymmo Strasbourg',  'Strasbourg',  '2 Place Kléber',              '+33 3 88 00 00 09', 'strasbourg@ymmo.fr'),
    ('Ymmo Nice',        'Nice',        '10 Promenade des Anglais',    '+33 4 93 00 00 10', 'nice@ymmo.fr'),
    ('Ymmo Rennes',      'Rennes',      '4 Place de la Mairie',        '+33 2 99 00 00 11', 'rennes@ymmo.fr'),
    ('Ymmo Montpellier', 'Montpellier', '9 Place de la Comédie',       '+33 4 67 00 00 12', 'montpellier@ymmo.fr');

-- 5 utilisateurs (1 admin, 2 agents rattachés à une agence, 2 clients)
INSERT INTO "Users" ("FirstName", "LastName", "Email", "PasswordHash", "Phone", "Role", "AgencyId") VALUES
    ('Camille', 'Durand',  'camille.durand@ymmo.fr',  '$2a$11$placeholderhash000000000000000000000000000000000001', '+33 6 00 00 00 01', 'Admin',  NULL),
    ('Lucas',   'Bernard', 'lucas.bernard@ymmo.fr',   '$2a$11$placeholderhash000000000000000000000000000000000002', '+33 6 00 00 00 02', 'Agent',  1),
    ('Manon',   'Petit',   'manon.petit@ymmo.fr',     '$2a$11$placeholderhash000000000000000000000000000000000003', '+33 6 00 00 00 03', 'Agent',  3),
    ('Hugo',    'Lefevre', 'hugo.lefevre@example.com', '$2a$11$placeholderhash000000000000000000000000000000000004', '+33 6 00 00 00 04', 'Client', NULL),
    ('Chloe',   'Moreau',  'chloe.moreau@example.com', '$2a$11$placeholderhash000000000000000000000000000000000005', '+33 6 00 00 00 05', 'Client', NULL);

-- 10 biens supplémentaires (10 existants + 10 = 20)
INSERT INTO "Properties"
    ("Title", "Description", "Price", "Type", "Status", "AgencyId", "City", "Bedrooms", "Area", "ImageUrl", "ListedDate") VALUES
    ('Appartement T2 Quartier Bouffay',      'T2 rénové de 45m² au cœur du centre historique, proche tramway.',            185000, 'Residential', 'Available',  6,  'Nantes',      1,  45,  NULL, '2026-05-12'),
    ('Local Commercial Centre-Ville',         'Local de 60m² en pied d''immeuble, vitrine sur rue passante.',               210000, 'Commercial',  'Available',  7,  'Toulouse',    0,  60,  NULL, '2026-04-02'),
    ('Maison de Ville avec Terrasse',         'Maison 3 chambres, terrasse 20m², proche métro, quartier calme.',            340000, 'Residential', 'UnderOffer', 8,  'Lille',       3,  90,  NULL, '2026-03-18'),
    ('Appartement T3 Petite France',          'T3 typique à colombages, vue canal, entièrement rénové.',                    295000, 'Residential', 'Available',  9,  'Strasbourg',  2,  68,  NULL, '2026-05-30'),
    ('Villa avec Piscine Vue Mer',            'Villa contemporaine 4 chambres, piscine, vue mer panoramique.',              1450000, 'Residential', 'Available', 10, 'Nice',        4,  210, NULL, '2026-02-22'),
    ('Plateau de Bureaux Modulable',          'Plateau 200m² divisible, climatisation, parking sécurisé.',                  520000, 'Commercial',  'Available', 11, 'Rennes',      0,  200, NULL, '2026-01-15'),
    ('Studio Étudiant Proche Fac',            'Studio 22m² meublé, idéal investissement locatif étudiant.',                 98000,  'Residential', 'Sold',       12, 'Montpellier', 1,  22,  NULL, '2025-12-05'),
    ('Terrain Constructible Périphérie',      'Terrain de 800m² viabilisé, zone pavillonnaire en développement.',           165000, 'Land',        'Available',  6,  'Nantes',      0,  800, NULL, '2026-04-20'),
    ('Immeuble Mixte Rénové',                  'Immeuble 4 lots (2 commerces + 2 appartements), rénové en 2024.',           780000, 'MixedUse',    'Available',  7,  'Toulouse',    4,  260, NULL, '2026-03-01'),
    ('Penthouse Vue Panoramique',             'Penthouse 5 pièces, terrasse 50m², vue dégagée sur la ville.',               980000, 'Residential', 'UnderOffer', 10, 'Nice',        4,  150, NULL, '2026-05-08');
