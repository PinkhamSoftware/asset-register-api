﻿// <auto-generated />
using System;
using HomesEngland.Gateway.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HomesEngland.Gateway.Migrations
{
    [DbContext(typeof(AssetRegisterContext))]
    [Migration("20190118154146_asset_register_version_relations")]
    partial class asset_register_version_relations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("HomesEngland.Gateway.AssetEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<decimal?>("ActualAgencyEquityCostIncludingHomeBuyAgentFee")
                        .HasColumnName("actualagencyequitycostincludinghomebuyagentfee");

                    b.Property<string>("Address")
                        .HasColumnName("address");

                    b.Property<decimal?>("AgencyEquityLoan")
                        .HasColumnName("agencyequityloan");

                    b.Property<decimal?>("AgencyFairValue")
                        .HasColumnName("agencyfairvalue");

                    b.Property<decimal?>("AgencyFairValueDifference")
                        .HasColumnName("agencyfairvaluedifference");

                    b.Property<decimal?>("AgencyPercentage")
                        .HasColumnName("agencypercentage");

                    b.Property<string>("ArrearsEffectAppliedOrLimited")
                        .HasColumnName("arrearseffectappliedorlimited");

                    b.Property<int?>("AssetRegisterVersionEntityId");

                    b.Property<int?>("AssetRegisterVersionId")
                        .HasColumnName("assetregisterversionid");

                    b.Property<int?>("CalendarYear")
                        .HasColumnName("calendaryear");

                    b.Property<int?>("Col")
                        .HasColumnName("col");

                    b.Property<DateTime?>("CompletionDateForHpiStart")
                        .HasColumnName("completiondateforhpistart");

                    b.Property<decimal?>("Deposit")
                        .HasColumnName("deposit");

                    b.Property<decimal?>("DeveloperDiscount")
                        .HasColumnName("developerdiscount");

                    b.Property<decimal?>("DeveloperEquityLoan")
                        .HasColumnName("developerequityloan");

                    b.Property<string>("DevelopingRslName")
                        .HasColumnName("developingrslname");

                    b.Property<decimal?>("DisposalMonthSinceCompletion")
                        .HasColumnName("disposalmonthsincecompletion");

                    b.Property<decimal?>("DisposalsCost")
                        .HasColumnName("disposalscost");

                    b.Property<decimal?>("DurationInMonths")
                        .HasColumnName("durationinmonths");

                    b.Property<decimal?>("EarlyMortgageIfNeverRepay")
                        .HasColumnName("earlymortgageifneverrepay");

                    b.Property<string>("EquityOwner")
                        .HasColumnName("equityowner");

                    b.Property<decimal?>("EstimatedSalePrice")
                        .HasColumnName("estimatedsaleprice");

                    b.Property<decimal?>("EstimatedValuation")
                        .HasColumnName("estimatedvaluation");

                    b.Property<decimal?>("ExpectedStaircasingRate")
                        .HasColumnName("expectedstaircasingrate");

                    b.Property<decimal?>("FairValueReserve")
                        .HasColumnName("fairvaluereserve");

                    b.Property<decimal?>("Fees")
                        .HasColumnName("fees");

                    b.Property<bool?>("FirstTimeBuyer")
                        .HasColumnName("firsttimebuyer");

                    b.Property<DateTime?>("FullDisposalDate")
                        .HasColumnName("fulldisposaldate");

                    b.Property<decimal?>("HPIEnd")
                        .HasColumnName("hpiend");

                    b.Property<decimal?>("HPIPlusMinus")
                        .HasColumnName("hpiplusminus");

                    b.Property<decimal?>("HPIStart")
                        .HasColumnName("hpistart");

                    b.Property<decimal?>("HistoricUnallocatedFees")
                        .HasColumnName("historicunallocatedfees");

                    b.Property<DateTime?>("HopCompletionDate")
                        .HasColumnName("hopcompletiondate");

                    b.Property<string>("HouseType")
                        .HasColumnName("housetype");

                    b.Property<decimal?>("HouseholdFiftyKIncomeBand")
                        .HasColumnName("householdfiftykincomeband");

                    b.Property<decimal?>("HouseholdFiveKIncomeBand")
                        .HasColumnName("householdfivekincomeband");

                    b.Property<decimal?>("HouseholdIncome")
                        .HasColumnName("householdincome");

                    b.Property<DateTime?>("IMSPaymentDate")
                        .HasColumnName("imspaymentdate");

                    b.Property<decimal?>("ImpairmentProvision")
                        .HasColumnName("impairmentprovision");

                    b.Property<DateTime?>("ImsActualCompletionDate")
                        .HasColumnName("imsactualcompletiondate");

                    b.Property<DateTime?>("ImsExpectedCompletionDate")
                        .HasColumnName("imsexpectedcompletiondate");

                    b.Property<DateTime?>("ImsLegalCompletionDate")
                        .HasColumnName("imslegalcompletiondate");

                    b.Property<string>("ImsOldRegion")
                        .HasColumnName("imsoldregion");

                    b.Property<int?>("Invested")
                        .HasColumnName("invested");

                    b.Property<bool?>("IsAsset")
                        .HasColumnName("isasset");

                    b.Property<bool?>("IsLondon")
                        .HasColumnName("islondon");

                    b.Property<bool?>("IsPaid")
                        .HasColumnName("ispaid");

                    b.Property<string>("LBHA")
                        .HasColumnName("lbha");

                    b.Property<string>("LocationLaRegionName")
                        .HasColumnName("locationlaregionname");

                    b.Property<string>("MMYYYY")
                        .HasColumnName("mmyyyy");

                    b.Property<DateTime>("ModifiedDateTime")
                        .HasColumnName("modifieddatetime");

                    b.Property<int?>("Month")
                        .HasColumnName("month");

                    b.Property<decimal?>("MonthOfCompletionSinceSchemeStart")
                        .HasColumnName("monthofcompletionsinceschemestart");

                    b.Property<decimal?>("Mortgage")
                        .HasColumnName("mortgage");

                    b.Property<decimal?>("MortgageEffect")
                        .HasColumnName("mortgageeffect");

                    b.Property<string>("MortgageProvider")
                        .HasColumnName("mortgageprovider");

                    b.Property<decimal?>("NewAgencyPercentage")
                        .HasColumnName("newagencypercentage");

                    b.Property<int?>("NoOfBeds")
                        .HasColumnName("noofbeds");

                    b.Property<bool?>("NotLimitedByFirstCharge")
                        .HasColumnName("notlimitedbyfirstcharge");

                    b.Property<decimal?>("OriginalAgencyPercentage")
                        .HasColumnName("originalagencypercentage");

                    b.Property<string>("Programme")
                        .HasColumnName("programme");

                    b.Property<string>("PropertyHouseName")
                        .HasColumnName("propertyhousename");

                    b.Property<string>("PropertyPostcode")
                        .HasColumnName("propertypostcode");

                    b.Property<string>("PropertyStreet")
                        .HasColumnName("propertystreet");

                    b.Property<string>("PropertyStreetNumber")
                        .HasColumnName("propertystreetnumber");

                    b.Property<string>("PropertyTown")
                        .HasColumnName("propertytown");

                    b.Property<string>("PropertyType")
                        .HasColumnName("propertytype");

                    b.Property<decimal?>("PurchasePrice")
                        .HasColumnName("purchaseprice");

                    b.Property<decimal?>("PurchasePriceBand")
                        .HasColumnName("purchasepriceband");

                    b.Property<decimal?>("QuarterSpend")
                        .HasColumnName("quarterspend");

                    b.Property<decimal?>("RegionalSaleAdjust")
                        .HasColumnName("regionalsaleadjust");

                    b.Property<decimal?>("RegionalStairAdjust")
                        .HasColumnName("regionalstairadjust");

                    b.Property<decimal?>("RelativeSalePropertyTypeAndTenureAdjustment")
                        .HasColumnName("relativesalepropertytypeandtenureadjustment");

                    b.Property<decimal?>("RelativeStairPropertyTypeAndTenureAdjustment")
                        .HasColumnName("relativestairpropertytypeandtenureadjustment");

                    b.Property<decimal?>("RemainingAgencyCost")
                        .HasColumnName("remainingagencycost");

                    b.Property<int?>("Row")
                        .HasColumnName("row");

                    b.Property<int?>("SchemeId")
                        .HasColumnName("schemeid");

                    b.Property<decimal?>("ShareOfRestrictedEquity")
                        .HasColumnName("shareofrestrictedequity");

                    b.Property<decimal?>("StaircasingPercentage")
                        .HasColumnName("staircasingpercentage");

                    b.Property<string>("Tenure")
                        .HasColumnName("tenure");

                    b.Property<decimal?>("WAEstimatedPropertyValue")
                        .HasColumnName("waestimatedpropertyvalue");

                    b.HasKey("Id");

                    b.HasIndex("AssetRegisterVersionEntityId");

                    b.HasIndex("AssetRegisterVersionId");

                    b.ToTable("assets");
                });

            modelBuilder.Entity("HomesEngland.Gateway.AssetRegisterVersionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("ModifiedDateTime")
                        .HasColumnName("modifieddatetime");

                    b.HasKey("Id");

                    b.ToTable("assetregisterversions");
                });

            modelBuilder.Entity("HomesEngland.Gateway.AuthenticationTokenEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("Expiry")
                        .HasColumnName("expiry");

                    b.Property<DateTime>("ModifiedDateTime")
                        .HasColumnName("modifieddatetime");

                    b.Property<string>("ReferenceNumber")
                        .HasColumnName("referencenumber");

                    b.Property<string>("Token")
                        .HasColumnName("token");

                    b.HasKey("Id");

                    b.ToTable("authenticationtokens");
                });

            modelBuilder.Entity("HomesEngland.Gateway.AssetEntity", b =>
                {
                    b.HasOne("HomesEngland.Gateway.AssetRegisterVersionEntity")
                        .WithMany("Assets")
                        .HasForeignKey("AssetRegisterVersionEntityId");

                    b.HasOne("HomesEngland.Gateway.AssetRegisterVersionEntity", "AssetRegisterVersion")
                        .WithMany()
                        .HasForeignKey("AssetRegisterVersionId");
                });
#pragma warning restore 612, 618
        }
    }
}
