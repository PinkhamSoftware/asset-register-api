using System;
using HomesEngland.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomesEngland.Gateway
{
    /// <summary>
    /// Has all of the database annotations to interact with the database
    /// </summary>
    [Table("assets")]
    public class AssetEntity : IAsset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("modifieddatetime")]
        public DateTime ModifiedDateTime { get; set; }

        [Column("programme")]
        public string Programme { get; set; }
        [Column("equityowner")]
        public string EquityOwner { get; set; }

        [Column("schemeid")]
        public int? SchemeId { get; set; }

        //Identifying Information
        [Column("locationlaregionname")]
        public string LocationLaRegionName { get; set; }
        [Column("imsoldregion")]
        public string ImsOldRegion { get; set; }
        [Column("noofbeds")]
        public int? NoOfBeds { get; set; }
        //IMS Adress
        [Column("address")]
        public string Address { get; set; }
        [Column("propertyhousename")]
        public string PropertyHouseName { get; set; }
        [Column("propertystreetnumber")]
        public string PropertyStreetNumber { get; set; }
        [Column("propertystreet")]
        public string PropertyStreet { get; set; }
        [Column("propertytown")]
        public string PropertyTown { get; set; }
        [Column("propertypostcode")]
        public string PropertyPostcode { get; set; }

        [Column("developingrslname")]
        public string DevelopingRslName { get; set; }
        [Column("lbha")]
        public string LBHA { get; set; }

        //Completion Dates
        [Column("completiondateforhpistart")]
        public DateTime? CompletionDateForHpiStart { get; set; }
        [Column("imsactualcompletiondate")]
        public DateTime? ImsActualCompletionDate { get; set; }
        [Column("imsexpectedcompletiondate")]
        public DateTime? ImsExpectedCompletionDate { get; set; }
        [Column("imslegalcompletiondate")]
        public DateTime? ImsLegalCompletionDate { get; set; }
        [Column("hopcompletiondate")]
        public DateTime? HopCompletionDate { get; set; }
        //Financial Information
        [Column("deposit")]
        public decimal? Deposit { get; set; }
        [Column("agencyequityloan")]
        public decimal? AgencyEquityLoan { get; set; }
        [Column("developerequityloan")]
        public decimal? DeveloperEquityLoan { get; set; }
        [Column("shareofrestrictedequity")]
        public decimal? ShareOfRestrictedEquity { get; set; }
        [Column("developerdiscount")]
        public decimal? DeveloperDiscount { get; set; }
        [Column("mortgage")]
        public decimal? Mortgage { get; set; }
        [Column("purchaseprice")]
        public decimal? PurchasePrice { get; set; }
        [Column("fees")]
        public decimal? Fees { get; set; }
        [Column("historicunallocatedfees")]
        public decimal? HistoricUnallocatedFees { get; set; }
        [Column("actualagencyequitycostincludinghomebuyagentfee")]
        public decimal? ActualAgencyEquityCostIncludingHomeBuyAgentFee { get; set; }
        [Column("fulldisposaldate")]
        public DateTime? FullDisposalDate { get; set; }
        [Column("originalagencypercentage")]
        public decimal? OriginalAgencyPercentage { get; set; }
        [Column("staircasingpercentage")]
        public decimal? StaircasingPercentage { get; set; }
        [Column("newagencypercentage")]
        public decimal? NewAgencyPercentage { get; set; }
        [Column("invested")]
        public int? Invested { get; set; }
        [Column("month")]
        public int? Month { get; set; }
        [Column("calendaryear")]
        public int? CalendarYear { get; set; }
        [Column("mmyyyy")]
        public string MMYYYY { get; set; }
        [Column("row")]
        public int? Row { get; set; }
        [Column("col")]
        public int? Col { get; set; }
        [Column("hpistart")]
        public decimal? HPIStart { get; set; }
        [Column("hpiend")]
        public decimal? HPIEnd { get; set; }
        [Column("hpiplusminus")]
        public decimal? HPIPlusMinus { get; set; }
        [Column("agencypercentage")]
        public decimal? AgencyPercentage { get; set; }
        [Column("mortgageeffect")]
        public decimal? MortgageEffect { get; set; }
        [Column("remainingagencycost")]
        public decimal? RemainingAgencyCost { get; set; }
        [Column("waestimatedpropertyvalue")]
        public decimal? WAEstimatedPropertyValue { get; set; }
        [Column("agencyfairvaluedifference")]
        public decimal? AgencyFairValueDifference { get; set; }
        [Column("impairmentprovision")]
        public decimal? ImpairmentProvision { get; set; }
        [Column("fairvaluereserve")]
        public decimal? FairValueReserve { get; set; }
        [Column("agencyfairvalue")]
        public decimal? AgencyFairValue { get; set; }
        [Column("disposalscost")]
        public decimal? DisposalsCost { get; set; }
        [Column("durationinmonths")]
        public decimal? DurationInMonths { get; set; }
        [Column("monthofcompletionsinceschemestart")]
        public decimal? MonthOfCompletionSinceSchemeStart { get; set; }
        [Column("disposalmonthsincecompletion")]
        public decimal? DisposalMonthSinceCompletion { get; set; }
        [Column("imspaymentdate")]
        public DateTime? IMSPaymentDate { get; set; }
        [Column("ispaid")]
        public bool? IsPaid { get; set; }
        [Column("isasset")]
        public bool? IsAsset { get; set; }
        [Column("propertytype")]
        public string PropertyType { get; set; }
        [Column("tenure")]
        public string Tenure { get; set; }
        [Column("expectedstaircasingrate")]
        public decimal? ExpectedStaircasingRate { get; set; }
        [Column("estimatedsaleprice")]
        public decimal? EstimatedSalePrice { get; set; }
        [Column("estimatedvaluation")]
        public decimal? EstimatedValuation { get; set; }

        [Column("regionalsaleadjust")]
        public decimal? RegionalSaleAdjust { get; set; }
        [Column("regionalstairadjust")]
        public decimal? RegionalStairAdjust { get; set; }
        [Column("notlimitedbyfirstcharge")]
        public bool? NotLimitedByFirstCharge { get; set; }
        [Column("earlymortgageifneverrepay")]
        public decimal? EarlyMortgageIfNeverRepay { get; set; }
        [Column("arrearseffectappliedorlimited")]
        public string ArrearsEffectAppliedOrLimited { get; set; }
        [Column("relativesalepropertytypeandtenureadjustment")]
        public decimal? RelativeSalePropertyTypeAndTenureAdjustment { get; set; }
        [Column("relativestairpropertytypeandtenureadjustment")]
        public decimal? RelativeStairPropertyTypeAndTenureAdjustment { get; set; }
        [Column("islondon")]
        public bool? IsLondon { get; set; }
        [Column("quarterspend")]
        public decimal? QuarterSpend { get; set; }
        [Column("mortgageprovider")]
        public string MortgageProvider { get; set; }
        [Column("housetype")]
        public string HouseType { get; set; }
        [Column("purchasepriceband")]
        public decimal? PurchasePriceBand { get; set; }
        [Column("householdincome")]
        public decimal? HouseholdIncome { get; set; }

        [Column("householdfivekincomeband")]
        public decimal? HouseholdFiveKIncomeBand { get; set; }
        [Column("householdfiftykincomeband")]
        public decimal? HouseholdFiftyKIncomeBand { get; set; }
        [Column("firsttimebuyer")]
        public bool? FirstTimeBuyer { get; set; }

        [Column("assetregisterversionid")]
        public int? AssetRegisterVersionId { get; set; }
        [ForeignKey("assetregisterversionid")]
        public virtual AssetRegisterVersion AssetRegisterVersion { get; set; }

        public AssetEntity() { }
        public AssetEntity(IAsset request)
        {
            Programme = request.Programme;
            EquityOwner = request.EquityOwner;
            SchemeId = request.SchemeId;
            LocationLaRegionName = request.LocationLaRegionName;
            ImsOldRegion = request.ImsOldRegion;
            NoOfBeds = request.NoOfBeds;
            Address = request.Address;
            PropertyHouseName = request.PropertyHouseName;
            PropertyStreetNumber = request.PropertyStreetNumber;
            PropertyStreet = request.PropertyStreet;
            PropertyTown = request.PropertyTown;
            PropertyPostcode = request.PropertyPostcode;
            DevelopingRslName = request.DevelopingRslName;
            LBHA = request.LBHA;
            CompletionDateForHpiStart = request.CompletionDateForHpiStart;
            ImsActualCompletionDate = request.ImsActualCompletionDate;
            ImsExpectedCompletionDate = request.ImsExpectedCompletionDate;
            ImsLegalCompletionDate = request.ImsLegalCompletionDate;
            HopCompletionDate = request.HopCompletionDate;
            Deposit = request.Deposit;
            AgencyEquityLoan = request.AgencyEquityLoan;
            DeveloperEquityLoan = request.DeveloperEquityLoan;
            ShareOfRestrictedEquity = request.ShareOfRestrictedEquity;
            DeveloperDiscount = request.DeveloperDiscount;
            Mortgage = request.Mortgage;
            PurchasePrice = request.PurchasePrice;
            Fees = request.Fees;
            HistoricUnallocatedFees = request.HistoricUnallocatedFees;
            ActualAgencyEquityCostIncludingHomeBuyAgentFee = request.ActualAgencyEquityCostIncludingHomeBuyAgentFee;
            FullDisposalDate = request.FullDisposalDate;
            OriginalAgencyPercentage = request.OriginalAgencyPercentage;
            StaircasingPercentage = request.StaircasingPercentage;
            NewAgencyPercentage = request.NewAgencyPercentage;
            Invested = request.Invested;
            Month = request.Month;
            CalendarYear = request.CalendarYear;
            MMYYYY = request.MMYYYY;
            Row = request.Row;
            Col = request.Col;
            HPIStart = request.HPIStart;
            HPIEnd = request.HPIEnd;
            HPIPlusMinus = request.HPIPlusMinus;
            AgencyPercentage = request.AgencyPercentage;
            MortgageEffect = request.MortgageEffect;
            RemainingAgencyCost = request.RemainingAgencyCost;
            WAEstimatedPropertyValue = request.WAEstimatedPropertyValue;
            AgencyFairValueDifference = request.AgencyFairValueDifference;
            ImpairmentProvision = request.ImpairmentProvision;
            FairValueReserve = request.FairValueReserve;
            AgencyFairValue = request.AgencyFairValue;
            DisposalsCost = request.DisposalsCost;
            DurationInMonths = request.DurationInMonths;
            MonthOfCompletionSinceSchemeStart = request.MonthOfCompletionSinceSchemeStart;
            DisposalMonthSinceCompletion = request.DisposalMonthSinceCompletion;
            IMSPaymentDate = request.IMSPaymentDate;
            IsPaid = request.IsPaid;
            IsAsset = request.IsAsset;
            PropertyType = request.PropertyType;
            Tenure = request.Tenure;
            ExpectedStaircasingRate = request.ExpectedStaircasingRate;
            EstimatedSalePrice = request.EstimatedSalePrice;
            RegionalSaleAdjust = request.RegionalSaleAdjust;
            RegionalStairAdjust = request.RegionalStairAdjust;
            NotLimitedByFirstCharge = request.NotLimitedByFirstCharge;
            EarlyMortgageIfNeverRepay = request.EarlyMortgageIfNeverRepay;
            ArrearsEffectAppliedOrLimited = request.ArrearsEffectAppliedOrLimited;
            RelativeSalePropertyTypeAndTenureAdjustment = request.RelativeSalePropertyTypeAndTenureAdjustment;
            RelativeStairPropertyTypeAndTenureAdjustment = request.RelativeStairPropertyTypeAndTenureAdjustment;
            IsLondon = request.IsLondon;
            QuarterSpend = request.QuarterSpend;
            MortgageProvider = request.MortgageProvider;
            HouseType = request.HouseType;
            PurchasePriceBand = request.PurchasePriceBand;
            HouseholdFiveKIncomeBand = request.HouseholdFiveKIncomeBand;
            HouseholdFiftyKIncomeBand = request.HouseholdFiftyKIncomeBand;
            FirstTimeBuyer = request.FirstTimeBuyer;

            HouseholdIncome = request.HouseholdIncome;
            EstimatedValuation = request.EstimatedValuation;
        }
    }
}
