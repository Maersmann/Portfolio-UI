﻿// <auto-generated />
using System;
using Aktien.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Aktien.Data.Infrastructure.Migrations
{
    [DbContext(typeof(Repository))]
    [Migration("20210217105517_VW6")]
    partial class VW6
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Aktien.Data.Model.DepotEntitys.Ausgabe", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Art")
                        .HasColumnType("integer");

                    b.Property<string>("Beschreibung")
                        .HasColumnType("text");

                    b.Property<double>("Betrag")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Datum")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("DepotID")
                        .HasColumnType("integer");

                    b.Property<int?>("HerkunftID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("DepotID");

                    b.ToTable("Ausgabe");
                });

            modelBuilder.Entity("Aktien.Data.Model.DepotEntitys.Depot", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Bezeichnung")
                        .HasColumnType("text");

                    b.Property<double?>("GesamtAusgaben")
                        .HasColumnType("double precision");

                    b.Property<double?>("GesamtEinahmen")
                        .HasColumnType("double precision");

                    b.HasKey("ID");

                    b.ToTable("Depot");
                });

            modelBuilder.Entity("Aktien.Data.Model.DepotEntitys.DepotWertpapier", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Anzahl")
                        .HasColumnType("double precision");

                    b.Property<double>("BuyIn")
                        .HasColumnType("double precision");

                    b.Property<int>("DepotID")
                        .HasColumnType("integer");

                    b.Property<int>("WertpapierID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("DepotID");

                    b.HasIndex("WertpapierID");

                    b.ToTable("DepotWertpapier");
                });

            modelBuilder.Entity("Aktien.Data.Model.DepotEntitys.Einnahme", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Art")
                        .HasColumnType("integer");

                    b.Property<string>("Beschreibung")
                        .HasColumnType("text");

                    b.Property<double>("Betrag")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Datum")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("DepotID")
                        .HasColumnType("integer");

                    b.Property<int?>("HerkunftID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("DepotID");

                    b.ToTable("Einnahme");
                });

            modelBuilder.Entity("Aktien.Data.Model.OptionEntitys.Konvertierung", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Typ")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.ToTable("Konvertierung");
                });

            modelBuilder.Entity("Aktien.Data.Model.WertpapierEntitys.Dividende", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Betrag")
                        .HasColumnType("double precision");

                    b.Property<double?>("BetragUmgerechnet")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Datum")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Waehrung")
                        .HasColumnType("integer");

                    b.Property<int>("WertpapierID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("WertpapierID");

                    b.ToTable("Dividende");
                });

            modelBuilder.Entity("Aktien.Data.Model.WertpapierEntitys.DividendeErhalten", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Bestand")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Datum")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("DividendeID")
                        .HasColumnType("integer");

                    b.Property<double>("GesamtBrutto")
                        .HasColumnType("double precision");

                    b.Property<double>("GesamtNetto")
                        .HasColumnType("double precision");

                    b.Property<double?>("Quellensteuer")
                        .HasColumnType("double precision");

                    b.Property<double?>("Umrechnungskurs")
                        .HasColumnType("double precision");

                    b.Property<int>("WertpapierID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("DividendeID");

                    b.HasIndex("WertpapierID");

                    b.ToTable("DividendeErhalten");
                });

            modelBuilder.Entity("Aktien.Data.Model.WertpapierEntitys.ETFInfo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Emittent")
                        .HasColumnType("integer");

                    b.Property<int>("ProfitTyp")
                        .HasColumnType("integer");

                    b.Property<int>("WertpapierID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("WertpapierID")
                        .IsUnique();

                    b.ToTable("ETFInfo");
                });

            modelBuilder.Entity("Aktien.Data.Model.WertpapierEntitys.OrderHistory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Anzahl")
                        .HasColumnType("double precision");

                    b.Property<int>("BuySell")
                        .HasColumnType("integer");

                    b.Property<double?>("Fremdkostenzuschlag")
                        .HasColumnType("double precision");

                    b.Property<int>("KaufartTyp")
                        .HasColumnType("integer");

                    b.Property<int>("OrderartTyp")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Orderdatum")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("Preis")
                        .HasColumnType("double precision");

                    b.Property<int>("WertpapierID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("WertpapierID");

                    b.ToTable("OrderHistory");
                });

            modelBuilder.Entity("Aktien.Data.Model.WertpapierEntitys.Wertpapier", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ISIN")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("WKN")
                        .HasColumnType("text");

                    b.Property<int>("WertpapierTyp")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.ToTable("Wertpapier");
                });

            modelBuilder.Entity("Aktien.Data.Model.DepotEntitys.Ausgabe", b =>
                {
                    b.HasOne("Aktien.Data.Model.DepotEntitys.Depot", "Depot")
                        .WithMany("Ausgaben")
                        .HasForeignKey("DepotID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Aktien.Data.Model.DepotEntitys.DepotWertpapier", b =>
                {
                    b.HasOne("Aktien.Data.Model.DepotEntitys.Depot", "Depot")
                        .WithMany("DepotWertpapier")
                        .HasForeignKey("DepotID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Aktien.Data.Model.WertpapierEntitys.Wertpapier", "Wertpapier")
                        .WithMany()
                        .HasForeignKey("WertpapierID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Aktien.Data.Model.DepotEntitys.Einnahme", b =>
                {
                    b.HasOne("Aktien.Data.Model.DepotEntitys.Depot", "Depot")
                        .WithMany("Einnahmen")
                        .HasForeignKey("DepotID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Aktien.Data.Model.WertpapierEntitys.Dividende", b =>
                {
                    b.HasOne("Aktien.Data.Model.WertpapierEntitys.Wertpapier", "Wertpapier")
                        .WithMany("Dividenden")
                        .HasForeignKey("WertpapierID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Aktien.Data.Model.WertpapierEntitys.DividendeErhalten", b =>
                {
                    b.HasOne("Aktien.Data.Model.WertpapierEntitys.Dividende", "Dividende")
                        .WithMany()
                        .HasForeignKey("DividendeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Aktien.Data.Model.WertpapierEntitys.Wertpapier", "Wertpapier")
                        .WithMany("ErhalteneDividenden")
                        .HasForeignKey("WertpapierID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Aktien.Data.Model.WertpapierEntitys.ETFInfo", b =>
                {
                    b.HasOne("Aktien.Data.Model.WertpapierEntitys.Wertpapier", "Wertpapier")
                        .WithOne("ETFInfo")
                        .HasForeignKey("Aktien.Data.Model.WertpapierEntitys.ETFInfo", "WertpapierID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Aktien.Data.Model.WertpapierEntitys.OrderHistory", b =>
                {
                    b.HasOne("Aktien.Data.Model.WertpapierEntitys.Wertpapier", "Wertpapier")
                        .WithMany("OrderHistories")
                        .HasForeignKey("WertpapierID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
