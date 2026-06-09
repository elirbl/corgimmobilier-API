using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YmmoApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    PreferredCity = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    AgencyId = table.Column<int>(type: "integer", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Bedrooms = table.Column<int>(type: "integer", nullable: false),
                    Area = table.Column<double>(type: "double precision", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    ListedDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropertyId = table.Column<int>(type: "integer", nullable: false),
                    BuyerId = table.Column<int>(type: "integer", nullable: false),
                    SellerId = table.Column<int>(type: "integer", nullable: false),
                    SalePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Clients_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales_Clients_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Agencies",
                columns: new[] { "Id", "Address", "City", "Email", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "12 Rue de la République", "Aix-en-Provence", "aix@ymmo.fr", "Ymmo Aix-en-Provence", "+33 4 42 00 00 01" },
                    { 2, "3 Avenue des Champs-Élysées", "Paris", "paris@ymmo.fr", "Ymmo Paris", "+33 1 42 00 00 02" },
                    { 3, "15 Rue de la République", "Lyon", "lyon@ymmo.fr", "Ymmo Lyon", "+33 4 72 00 00 03" },
                    { 4, "8 La Canebière", "Marseille", "marseille@ymmo.fr", "Ymmo Marseille", "+33 4 91 00 00 04" },
                    { 5, "22 Cours de l'Intendance", "Bordeaux", "bordeaux@ymmo.fr", "Ymmo Bordeaux", "+33 5 56 00 00 05" }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "AgencyId", "Area", "Bedrooms", "City", "Description", "ImageUrl", "ListedDate", "Price", "Status", "Title", "Type" },
                values: new object[,]
                {
                    { 1, 1, 73.5, 2, "Aix-en-Provence", "Superbe appartement de 73m² en plein cœur d'Aix, lumineux, double exposition. Cuisine équipée, parquet, cave. Idéal investisseur ou première acquisition.", null, new DateOnly(2026, 5, 10), 320000m, "Available", "Appartement T3 Centre Historique", "Residential" },
                    { 2, 2, 120.0, 0, "Paris", "Espace de bureaux modulable en zone commerciale premium. Accès PMR, parking, fibre. Idéal pour startup ou PME.", null, new DateOnly(2026, 4, 25), 420000m, "Available", "Bureau Open-Space 120m²", "Commercial" },
                    { 3, 3, 142.19999999999999, 4, "Lyon", "Belle maison de 4 chambres avec jardin de 300m², garage double, piscine. Quartier calme proche écoles et commerces.", null, new DateOnly(2026, 4, 15), 580000m, "UnderOffer", "Maison Familiale avec Jardin", "Residential" },
                    { 4, 4, 95.0, 2, "Marseille", "Loft de caractère 95m² en plein Vieux-Port. Hauteur sous plafond 4m, briques apparentes, cuisine ouverte. Coup de cœur garanti.", null, new DateOnly(2026, 5, 20), 485000m, "Available", "Loft Industriel Rénové", "Residential" },
                    { 5, 5, 280.0, 5, "Bordeaux", "Villa contemporaine 5 chambres, piscine à débordement, vue imprenable sur la Garonne. Prestations haut de gamme.", null, new DateOnly(2026, 3, 1), 1250000m, "Available", "Villa Prestige Vue Garonne", "Residential" },
                    { 6, 1, 28.0, 1, "Aix-en-Provence", "Studio meublé 28m², idéalement placé à 200m de l'université. Charges incluses, bail étudiant ou classique.", null, new DateOnly(2026, 5, 28), 125000m, "Available", "Studio Étudiant Centre-Ville", "Residential" },
                    { 7, 2, 800.0, 0, "Paris", "Entrepôt neuf en ZAC, hauteur 8m, 4 quais de chargement, bureau intégré 50m². Bail 3/6/9 ans.", null, new DateOnly(2026, 2, 14), 890000m, "Available", "Entrepôt Logistique 800m²", "Commercial" },
                    { 8, 3, 118.0, 3, "Lyon", "T4 de standing 118m² au 6ème étage avec terrasse 30m². Vue panoramique, gardien, cave, 2 parkings.", null, new DateOnly(2026, 1, 10), 695000m, "Sold", "Appartement T4 Prestige", "Residential" },
                    { 9, 4, 650.0, 0, "Marseille", "Terrain plat viabilisé dans lotissement sécurisé. Permis de construire accordé pour villa R+1. Environnement verdoyant.", null, new DateOnly(2026, 5, 5), 198000m, "Available", "Terrain Constructible 650m²", "Land" },
                    { 10, 5, 160.0, 3, "Bordeaux", "Duplex 160m² avec rooftop privatif 80m² face au Cours du Chapeau Rouge. Architecture contemporaine, matériaux nobles.", null, new DateOnly(2026, 4, 8), 875000m, "Available", "Duplex Rooftop Exceptceptionnel", "Residential" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_AgencyId",
                table: "Properties",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_BuyerId",
                table: "Sales",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_PropertyId",
                table: "Sales",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SellerId",
                table: "Sales",
                column: "SellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Agencies");
        }
    }
}
