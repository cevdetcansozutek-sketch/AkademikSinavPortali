using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AkademikSinavPortali.Models;

public partial class OnlineSinavDbContext : DbContext
{
    public OnlineSinavDbContext()
    {
    }

    public OnlineSinavDbContext(DbContextOptions<OnlineSinavDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dersler> Derslers { get; set; }

    public virtual DbSet<OgrenciYanitlari> OgrenciYanitlaris { get; set; }

    public virtual DbSet<Ogrenciler> Ogrencilers { get; set; }

    public virtual DbSet<Sinavlar> Sinavlars { get; set; }

    public virtual DbSet<Sorular> Sorulars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dersler>(entity =>
        {
            entity.HasKey(e => e.DersId).HasName("PK__Dersler__E8B3DE719BEC7B30");

            entity.ToTable("Dersler");

            entity.HasIndex(e => e.DersKodu, "UQ__Dersler__9DCB30EF17ECBA6F").IsUnique();

            entity.Property(e => e.DersId).HasColumnName("DersID");
            entity.Property(e => e.DersAdi).HasMaxLength(100);
            entity.Property(e => e.DersKodu).HasMaxLength(10);
        });

        modelBuilder.Entity<OgrenciYanitlari>(entity =>
        {
            entity.HasKey(e => e.YanitId).HasName("PK__OgrenciY__279FAE1058943FB3");

            entity.ToTable("OgrenciYanitlari");

            entity.Property(e => e.YanitId).HasColumnName("YanitID");
            entity.Property(e => e.OgrenciId).HasColumnName("OgrenciID");
            entity.Property(e => e.SinavId).HasColumnName("SinavID");
            entity.Property(e => e.SoruId).HasColumnName("SoruID");
            entity.Property(e => e.VerilenCevap)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Ogrenci).WithMany(p => p.OgrenciYanitlaris)
                .HasForeignKey(d => d.OgrenciId)
                .HasConstraintName("FK__OgrenciYa__Ogren__48CFD27E");

            entity.HasOne(d => d.Sinav).WithMany(p => p.OgrenciYanitlaris)
                .HasForeignKey(d => d.SinavId)
                .HasConstraintName("FK__OgrenciYa__Sinav__47DBAE45");

            entity.HasOne(d => d.Soru).WithMany(p => p.OgrenciYanitlaris)
                .HasForeignKey(d => d.SoruId)
                .HasConstraintName("FK__OgrenciYa__SoruI__49C3F6B7");
        });

        modelBuilder.Entity<Ogrenciler>(entity =>
        {
            entity.HasKey(e => e.OgrenciId).HasName("PK__Ogrencil__E497E6D4C665B6CC");

            entity.ToTable("Ogrenciler");

            entity.HasIndex(e => e.OgrenciNo, "UQ__Ogrencil__E497FE1A6DA64F0C").IsUnique();

            entity.Property(e => e.OgrenciId).HasColumnName("OgrenciID");
            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.OgrenciNo).HasMaxLength(20);
            entity.Property(e => e.Sifre).HasMaxLength(20);
            entity.Property(e => e.Soyad).HasMaxLength(50);
        });

        modelBuilder.Entity<Sinavlar>(entity =>
        {
            entity.HasKey(e => e.SinavId).HasName("PK__Sinavlar__E089B7861AF78D2F");

            entity.ToTable("Sinavlar");

            entity.Property(e => e.SinavId).HasColumnName("SinavID");
            entity.Property(e => e.DersId).HasColumnName("DersID");
            entity.Property(e => e.SinavAdi).HasMaxLength(100);
            entity.Property(e => e.SinavTarihi).HasColumnType("datetime");

            entity.HasOne(d => d.Ders).WithMany(p => p.Sinavlars)
                .HasForeignKey(d => d.DersId)
                .HasConstraintName("FK__Sinavlar__DersID__3E52440B");

            entity.HasMany(d => d.Sorus).WithMany(p => p.Sinavs)
                .UsingEntity<Dictionary<string, object>>(
                    "SinavSorulari",
                    r => r.HasOne<Sorular>().WithMany()
                        .HasForeignKey("SoruId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SinavSoru__SoruI__4222D4EF"),
                    l => l.HasOne<Sinavlar>().WithMany()
                        .HasForeignKey("SinavId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SinavSoru__Sinav__412EB0B6"),
                    j =>
                    {
                        j.HasKey("SinavId", "SoruId").HasName("PK__SinavSor__62BA6EE6EFF1ED09");
                        j.ToTable("SinavSorulari");
                        j.IndexerProperty<int>("SinavId").HasColumnName("SinavID");
                        j.IndexerProperty<int>("SoruId").HasColumnName("SoruID");
                    });
        });

        modelBuilder.Entity<Sorular>(entity =>
        {
            entity.HasKey(e => e.SoruId).HasName("PK__Sorular__233D960925360C57");

            entity.ToTable("Sorular");

            entity.Property(e => e.SoruId).HasColumnName("SoruID");
            entity.Property(e => e.DersId).HasColumnName("DersID");
            entity.Property(e => e.DogruCevap)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Puan).HasDefaultValue(10);
            entity.Property(e => e.SecenekA).HasMaxLength(200);
            entity.Property(e => e.SecenekB).HasMaxLength(200);
            entity.Property(e => e.SecenekC).HasMaxLength(200);
            entity.Property(e => e.SecenekD).HasMaxLength(200);

            entity.HasOne(d => d.Ders).WithMany(p => p.Sorulars)
                .HasForeignKey(d => d.DersId)
                .HasConstraintName("FK__Sorular__DersID__3B75D760");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}