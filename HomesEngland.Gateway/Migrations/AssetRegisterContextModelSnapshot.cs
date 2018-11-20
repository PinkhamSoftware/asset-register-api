﻿// <auto-generated />
using System;
using HomesEngland.Gateway.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HomesEngland.Gateway.Migrations
{
    [DbContext(typeof(AssetRegisterContext))]
    partial class AssetRegisterContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("HomesEngland.Gateway.DapperAsset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountingYear")
                        .HasColumnName("accountingyear");

                    b.Property<string>("Address")
                        .HasColumnName("address");

                    b.Property<decimal?>("AgencyEquityLoan")
                        .HasColumnName("agencyequityloan");

                    b.Property<DateTime?>("CompletionDateForHpiStart")
                        .HasColumnName("completiondateforhpistart");

                    b.Property<decimal?>("Deposit")
                        .HasColumnName("deposit");

                    b.Property<decimal?>("DeveloperEquityLoan")
                        .HasColumnName("developerequityloan");

                    b.Property<string>("DevelopingRslName")
                        .HasColumnName("developingrslname");

                    b.Property<int?>("DifferenceFromImsExpectedCompletionToHopCompletionDate")
                        .HasColumnName("differencefromimsexpectedcompletiontohopcompletiondate");

                    b.Property<DateTime?>("HopCompletionDate")
                        .HasColumnName("hopcompletiondate");

                    b.Property<DateTime?>("ImsActualCompletionDate")
                        .HasColumnName("imsactualcompletiondate");

                    b.Property<DateTime?>("ImsExpectedCompletionDate")
                        .HasColumnName("imsexpectedcompletiondate");

                    b.Property<DateTime?>("ImsLegalCompletionDate")
                        .HasColumnName("imslegalcompletiondate");

                    b.Property<string>("ImsOldRegion")
                        .HasColumnName("imsoldregion");

                    b.Property<string>("LocationLaRegionName")
                        .HasColumnName("locationlaregionname");

                    b.Property<DateTime>("ModifiedDateTime")
                        .HasColumnName("modifieddatetime");

                    b.Property<string>("MonthPaid")
                        .HasColumnName("monthpaid");

                    b.Property<string>("NoOfBeds")
                        .HasColumnName("noofbeds");

                    b.Property<string>("SchemeId")
                        .HasColumnName("schemeid");

                    b.Property<decimal?>("ShareOfRestrictedEquity")
                        .HasColumnName("shareofrestrictedequity");

                    b.HasKey("Id");

                    b.ToTable("assets");
                });
#pragma warning restore 612, 618
        }
    }
}
