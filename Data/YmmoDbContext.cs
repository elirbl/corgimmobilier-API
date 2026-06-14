using Microsoft.EntityFrameworkCore;
using YmmoApi.Models;

namespace YmmoApi.Data;

public class YmmoDbContext(DbContextOptions<YmmoDbContext> options) : DbContext(options)
{
    public DbSet<Agency> Agencies => Set<Agency>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Visit> Visits => Set<Visit>();
    public DbSet<Photo> Photos => Set<Photo>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<AvailablePropertyListing> AvailablePropertyListings => Set<AvailablePropertyListing>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<TransactionStageHistory> TransactionStageHistories => Set<TransactionStageHistory>();
    public DbSet<TransactionDocument> TransactionDocuments => Set<TransactionDocument>();
    public DbSet<AgencyMonthlyRevenue> AgencyMonthlyRevenues => Set<AgencyMonthlyRevenue>();
    public DbSet<Favorite> Favorites => Set<Favorite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Property>()
            .Property(p => p.Price)
            .HasColumnType("numeric(18,2)");

        modelBuilder.Entity<Property>()
            .Property(p => p.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Property>()
            .Property(p => p.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Property>()
            .Property(p => p.DpeRating)
            .HasConversion<string>();

        modelBuilder.Entity<Property>()
            .HasOne(p => p.Agent)
            .WithMany()
            .HasForeignKey(p => p.AgentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AvailablePropertyListing>(b =>
        {
            b.HasNoKey();
            b.ToView("vw_biens_disponibles");
            b.Property(x => x.Type).HasConversion<string>();
            b.Property(x => x.DpeRating).HasConversion<string>();
        });

        modelBuilder.Entity<AgencyMonthlyRevenue>().HasNoKey().ToTable((string?)null);

        modelBuilder.Entity<Sale>()
            .Property(s => s.SalePrice)
            .HasColumnType("numeric(18,2)");

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Visit>()
            .Property(v => v.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Visit>()
            .HasOne(v => v.Agent)
            .WithMany()
            .HasForeignKey(v => v.AgentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Visit>()
            .HasOne(v => v.Client)
            .WithMany()
            .HasForeignKey(v => v.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.CurrentStage)
            .HasConversion<string>();

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Property)
            .WithMany()
            .HasForeignKey(t => t.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Client)
            .WithMany()
            .HasForeignKey(t => t.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Agent)
            .WithMany()
            .HasForeignKey(t => t.AgentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<TransactionStageHistory>()
            .Property(h => h.Stage)
            .HasConversion<string>();

        modelBuilder.Entity<TransactionStageHistory>()
            .HasOne(h => h.Transaction)
            .WithMany(t => t.StageHistory)
            .HasForeignKey(h => h.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TransactionDocument>()
            .HasOne(d => d.Transaction)
            .WithMany(t => t.Documents)
            .HasForeignKey(d => d.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Photo>()
            .HasOne(ph => ph.Property)
            .WithMany(p => p.Photos)
            .HasForeignKey(ph => ph.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Recipient)
            .WithMany()
            .HasForeignKey(m => m.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshToken>()
            .HasIndex(rt => rt.Token)
            .IsUnique();

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.Property)
            .WithMany()
            .HasForeignKey(f => f.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Favorite>()
            .HasIndex(f => new { f.UserId, f.PropertyId })
            .IsUnique();

        // Seed agencies
        modelBuilder.Entity<Agency>().HasData(
            new Agency { Id = 1, Name = "Ymmo Aix-en-Provence", City = "Aix-en-Provence", Address = "12 Rue de la République", Phone = "+33 4 42 00 00 01", Email = "aix@ymmo.fr" },
            new Agency { Id = 2, Name = "Ymmo Paris", City = "Paris", Address = "3 Avenue des Champs-Élysées", Phone = "+33 1 42 00 00 02", Email = "paris@ymmo.fr" },
            new Agency { Id = 3, Name = "Ymmo Lyon", City = "Lyon", Address = "15 Rue de la République", Phone = "+33 4 72 00 00 03", Email = "lyon@ymmo.fr" },
            new Agency { Id = 4, Name = "Ymmo Marseille", City = "Marseille", Address = "8 La Canebière", Phone = "+33 4 91 00 00 04", Email = "marseille@ymmo.fr" },
            new Agency { Id = 5, Name = "Ymmo Bordeaux", City = "Bordeaux", Address = "22 Cours de l'Intendance", Phone = "+33 5 56 00 00 05", Email = "bordeaux@ymmo.fr" }
        );

        // Seed properties
        modelBuilder.Entity<Property>().HasData(
            new Property { Id = 1, Title = "Appartement T3 Centre Historique", Description = "Superbe appartement de 73m² en plein cœur d'Aix, lumineux, double exposition. Cuisine équipée, parquet, cave. Idéal investisseur ou première acquisition.", Price = 320000m, Type = PropertyType.Residential, Status = PropertyStatus.Available, AgencyId = 1, City = "Aix-en-Provence", Bedrooms = 2, Area = 73.5, ListedDate = new DateOnly(2026, 5, 10) },
            new Property { Id = 2, Title = "Bureau Open-Space 120m²", Description = "Espace de bureaux modulable en zone commerciale premium. Accès PMR, parking, fibre. Idéal pour startup ou PME.", Price = 420000m, Type = PropertyType.Commercial, Status = PropertyStatus.Available, AgencyId = 2, City = "Paris", Bedrooms = 0, Area = 120, ListedDate = new DateOnly(2026, 4, 25) },
            new Property { Id = 3, Title = "Maison Familiale avec Jardin", Description = "Belle maison de 4 chambres avec jardin de 300m², garage double, piscine. Quartier calme proche écoles et commerces.", Price = 580000m, Type = PropertyType.Residential, Status = PropertyStatus.UnderOffer, AgencyId = 3, City = "Lyon", Bedrooms = 4, Area = 142.2, ListedDate = new DateOnly(2026, 4, 15) },
            new Property { Id = 4, Title = "Loft Industriel Rénové", Description = "Loft de caractère 95m² en plein Vieux-Port. Hauteur sous plafond 4m, briques apparentes, cuisine ouverte. Coup de cœur garanti.", Price = 485000m, Type = PropertyType.Residential, Status = PropertyStatus.Available, AgencyId = 4, City = "Marseille", Bedrooms = 2, Area = 95, ListedDate = new DateOnly(2026, 5, 20) },
            new Property { Id = 5, Title = "Villa Prestige Vue Garonne", Description = "Villa contemporaine 5 chambres, piscine à débordement, vue imprenable sur la Garonne. Prestations haut de gamme.", Price = 1250000m, Type = PropertyType.Residential, Status = PropertyStatus.Available, AgencyId = 5, City = "Bordeaux", Bedrooms = 5, Area = 280, ListedDate = new DateOnly(2026, 3, 1) },
            new Property { Id = 6, Title = "Studio Étudiant Centre-Ville", Description = "Studio meublé 28m², idéalement placé à 200m de l'université. Charges incluses, bail étudiant ou classique.", Price = 125000m, Type = PropertyType.Residential, Status = PropertyStatus.Available, AgencyId = 1, City = "Aix-en-Provence", Bedrooms = 1, Area = 28, ListedDate = new DateOnly(2026, 5, 28) },
            new Property { Id = 7, Title = "Entrepôt Logistique 800m²", Description = "Entrepôt neuf en ZAC, hauteur 8m, 4 quais de chargement, bureau intégré 50m². Bail 3/6/9 ans.", Price = 890000m, Type = PropertyType.Commercial, Status = PropertyStatus.Available, AgencyId = 2, City = "Paris", Bedrooms = 0, Area = 800, ListedDate = new DateOnly(2026, 2, 14) },
            new Property { Id = 8, Title = "Appartement T4 Prestige", Description = "T4 de standing 118m² au 6ème étage avec terrasse 30m². Vue panoramique, gardien, cave, 2 parkings.", Price = 695000m, Type = PropertyType.Residential, Status = PropertyStatus.Sold, AgencyId = 3, City = "Lyon", Bedrooms = 3, Area = 118, ListedDate = new DateOnly(2026, 1, 10) },
            new Property { Id = 9, Title = "Terrain Constructible 650m²", Description = "Terrain plat viabilisé dans lotissement sécurisé. Permis de construire accordé pour villa R+1. Environnement verdoyant.", Price = 198000m, Type = PropertyType.Land, Status = PropertyStatus.Available, AgencyId = 4, City = "Marseille", Bedrooms = 0, Area = 650, ListedDate = new DateOnly(2026, 5, 5) },
            new Property { Id = 10, Title = "Duplex Rooftop Exceptceptionnel", Description = "Duplex 160m² avec rooftop privatif 80m² face au Cours du Chapeau Rouge. Architecture contemporaine, matériaux nobles.", Price = 875000m, Type = PropertyType.Residential, Status = PropertyStatus.Available, AgencyId = 5, City = "Bordeaux", Bedrooms = 3, Area = 160, ListedDate = new DateOnly(2026, 4, 8) }
        );
    }
}
