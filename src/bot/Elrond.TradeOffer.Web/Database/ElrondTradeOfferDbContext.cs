﻿using Microsoft.EntityFrameworkCore;

namespace Elrond.TradeOffer.Web.Database;

public class ElrondTradeOfferDbContext: DbContext
{
    public ElrondTradeOfferDbContext(DbContextOptions<ElrondTradeOfferDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbOffer>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<DbOffer>()
            .Property(p => p.RowVersion)
            .IsRowVersion();
        
        modelBuilder.Entity<DbBid>()
            .HasKey(b => new { b.OfferId, b.CreatorUserId });

        modelBuilder.Entity<DbBid>()
            .Property(p => p.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<DbBid>()
            .HasOne(p => p.Offer)
            .WithMany(b => b.Bids)
            .HasForeignKey(p => p.OfferId);

        modelBuilder.Entity<DbOffer>()
            .HasOne(p => p.CreatorUser)
            .WithMany(b => b.Offers)
            .HasForeignKey(p => p.CreatorUserId);


        modelBuilder.Entity<DbBid>()
            .HasOne(p => p.CreatorUser)
            .WithMany(b => b.Bids)
            .HasForeignKey(p => p.CreatorUserId);

        modelBuilder.Entity<DbUser>()
            .HasKey(p => p.Id);
        
        modelBuilder.Entity<DbUser>()
            .Property(c => c.Id)
            .ValueGeneratedNever();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateCreatedOnUpdatedOn();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateCreatedOnUpdatedOn()
    {
        var addedEntities = ChangeTracker.Entries<BaseEntity>().Where(p => p.State == EntityState.Added).ToList();
        addedEntities.ForEach(p =>
        {
            var now = DateTime.UtcNow;
            p.Property(nameof(BaseEntity.CreatedOn)).CurrentValue = now;
            p.Property(nameof(BaseEntity.UpdatedOn)).CurrentValue = now;
        });

        var editedEntities = ChangeTracker.Entries<BaseEntity>().Where(p => p.State == EntityState.Modified).ToList();
        editedEntities.ForEach(p =>
        {
            p.Property(nameof(BaseEntity.UpdatedOn)).CurrentValue = DateTime.UtcNow;
        });
    }

    public virtual DbSet<DbOffer> Offers => Set<DbOffer>();

    public virtual DbSet<DbBid> Bids => Set<DbBid>();

    public virtual DbSet<DbUser> Users => Set<DbUser>();
}