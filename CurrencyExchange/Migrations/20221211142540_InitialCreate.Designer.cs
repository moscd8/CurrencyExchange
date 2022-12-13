﻿// <auto-generated />
using System;
using CurrencyExchange.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CurrencyExchange.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20221211142540_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CurrencyExchange.Model.CurrencyExchangeDBModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExchangeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromCurrency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Result")
                        .HasColumnType("real");

                    b.Property<bool>("Success")
                        .HasColumnType("bit");

                    b.Property<string>("ToCurrency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CurrencyExchanges");

                    b.HasDiscriminator<string>("Discriminator").HasValue("CurrencyExchangeDBModel");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("CurrencyExchange.Model.CurrencyExchangeDBModelUSD_ILS", b =>
                {
                    b.HasBaseType("CurrencyExchange.Model.CurrencyExchangeDBModel");

                    b.HasDiscriminator().HasValue("CurrencyExchangeDBModelUSD_ILS");
                });
#pragma warning restore 612, 618
        }
    }
}
