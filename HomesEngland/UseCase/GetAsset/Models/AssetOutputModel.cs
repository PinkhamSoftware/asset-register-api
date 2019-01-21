using System;
using HomesEngland.Domain;

namespace HomesEngland.UseCase.GetAsset.Models
{
    public class AssetOutputModel
    {
        public int Id { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        public AssetOutputModel(){}

        public AssetOutputModel(IAsset asset)
        {
            Id = asset.Id;
            ModifiedDateTime = asset.ModifiedDateTime;

            Programme = asset.Programme;
            EquityOwner = asset.EquityOwner;
            SchemeId = asset.SchemeId;
            LocationLaRegionName = asset.LocationLaRegionName;
            ImsOldRegion = asset.ImsOldRegion;
            NoOfBeds = asset.NoOfBeds;
            Address = asset.Address;
            PropertyHouseName = asset.PropertyHouseName;
            PropertyStreetNumber = asset.PropertyStreetNumber;
            PropertyStreet = asset.PropertyStreet;
            PropertyTown = asset.PropertyTown;
            PropertyPostcode = asset.PropertyPostcode;
            DevelopingRslName = asset.DevelopingRslName;
            LBHA = asset.LBHA;
            CompletionDateForHpiStart = asset.CompletionDateForHpiStart;
            ImsActualCompletionDate = asset.ImsActualCompletionDate;
            ImsExpectedCompletionDate = asset.ImsExpectedCompletionDate;
            ImsLegalCompletionDate = asset.ImsLegalCompletionDate;
            HopCompletionDate = asset.HopCompletionDate;
            Deposit = asset.Deposit;
            AgencyEquityLoan = asset.AgencyEquityLoan;
            DeveloperEquityLoan = asset.DeveloperEquityLoan;
            ShareOfRestrictedEquity = asset.ShareOfRestrictedEquity;
            DeveloperDiscount = asset.DeveloperDiscount;
            Mortgage = asset.Mortgage;
            PurchasePrice = asset.PurchasePrice;
            Fees = asset.Fees;
            HistoricUnallocatedFees = asset.HistoricUnallocatedFees;
            ActualAgencyEquityCostIncludingHomeBuyAgentFee = asset.ActualAgencyEquityCostIncludingHomeBuyAgentFee;
            FullDisposalDate = asset.FullDisposalDate;
            OriginalAgencyPercentage = asset.OriginalAgencyPercentage;
            StaircasingPercentage = asset.StaircasingPercentage;
            NewAgencyPercentage = asset.NewAgencyPercentage;
            Invested = asset.Invested;
            Month = asset.Month;
            CalendarYear = asset.CalendarYear;
            MMYYYY = asset.MMYYYY;
            Row = asset.Row;
            Col = asset.Col;
            HPIStart = asset.HPIStart;
            HPIEnd = asset.HPIEnd;
            HPIPlusMinus = asset.HPIPlusMinus;
            AgencyPercentage = asset.AgencyPercentage;
            MortgageEffect = asset.MortgageEffect;
            RemainingAgencyCost = asset.RemainingAgencyCost;
            WAEstimatedPropertyValue = asset.WAEstimatedPropertyValue;
            AgencyFairValueDifference = asset.AgencyFairValueDifference;
            ImpairmentProvision = asset.ImpairmentProvision;
            FairValueReserve = asset.FairValueReserve;
            AgencyFairValue = asset.AgencyFairValue;
            DisposalsCost = asset.DisposalsCost;
            DurationInMonths = asset.DurationInMonths;
            MonthOfCompletionSinceSchemeStart = asset.MonthOfCompletionSinceSchemeStart;
            DisposalMonthSinceCompletion = asset.DisposalMonthSinceCompletion;
            IMSPaymentDate = asset.IMSPaymentDate;
            IsPaid = asset.IsPaid;
            IsAsset = asset.IsAsset;
            PropertyType = asset.PropertyType;
            Tenure = asset.Tenure;
            ExpectedStaircasingRate = asset.ExpectedStaircasingRate;
            EstimatedSalePrice = asset.EstimatedSalePrice;
            RegionalSaleAdjust = asset.RegionalSaleAdjust;
            RegionalStairAdjust = asset.RegionalStairAdjust;
            NotLimitedByFirstCharge = asset.NotLimitedByFirstCharge;
            EarlyMortgageIfNeverRepay = asset.EarlyMortgageIfNeverRepay;
            ArrearsEffectAppliedOrLimited = asset.ArrearsEffectAppliedOrLimited;
            RelativeSalePropertyTypeAndTenureAdjustment = asset.RelativeSalePropertyTypeAndTenureAdjustment;
            RelativeStairPropertyTypeAndTenureAdjustment = asset.RelativeStairPropertyTypeAndTenureAdjustment;
            IsLondon = asset.IsLondon;
            QuarterSpend = asset.QuarterSpend;
            MortgageProvider = asset.MortgageProvider;
            HouseType = asset.HouseType;
            PurchasePriceBand = asset.PurchasePriceBand;
            HouseholdFiveKIncomeBand = asset.HouseholdFiveKIncomeBand;
            HouseholdFiftyKIncomeBand = asset.HouseholdFiftyKIncomeBand;
            FirstTimeBuyer = asset.FirstTimeBuyer;

            HouseholdIncome = asset.HouseholdIncome;
            EstimatedValuation = asset.EstimatedValuation;

            AssetRegisterVersionId = asset.AssetRegisterVersionId;
        }

        public string Programme { get; set; }
        public string EquityOwner { get; set; }
        public int? SchemeId { get; set; }
        public string LocationLaRegionName { get; set; }
        public string ImsOldRegion { get; set; }
        public int? NoOfBeds { get; set; }
        public string Address { get; set; }
        public string PropertyHouseName { get; set; }
        public string PropertyStreetNumber { get; set; }
        public string PropertyStreet { get; set; }
        public string PropertyTown { get; set; }
        public string PropertyPostcode { get; set; }
        public string DevelopingRslName { get; set; }
        public string LBHA { get; set; }
        public DateTime? CompletionDateForHpiStart { get; set; }
        public DateTime? ImsActualCompletionDate { get; set; }
        public DateTime? ImsExpectedCompletionDate { get; set; }
        public DateTime? ImsLegalCompletionDate { get; set; }
        public DateTime? HopCompletionDate { get; set; }
        public decimal? Deposit { get; set; }
        public decimal? AgencyEquityLoan { get; set; }
        public decimal? DeveloperEquityLoan { get; set; }
        public decimal? ShareOfRestrictedEquity { get; set; }
        public decimal? DeveloperDiscount { get; set; }
        public decimal? Mortgage { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? Fees { get; set; }
        public decimal? HistoricUnallocatedFees { get; set; }
        public decimal? ActualAgencyEquityCostIncludingHomeBuyAgentFee { get; set; }
        public DateTime? FullDisposalDate { get; set; }
        public decimal? OriginalAgencyPercentage { get; set; }
        public decimal? StaircasingPercentage { get; set; }
        public decimal? NewAgencyPercentage { get; set; }
        public int? Invested { get; set; }
        public int? Month { get; set; }
        public int? CalendarYear { get; set; }
        public string MMYYYY { get; set; }
        public int? Row { get; set; }
        public int? Col { get; set; }
        public decimal? HPIStart { get; set; }
        public decimal? HPIEnd { get; set; }
        public decimal? HPIPlusMinus { get; set; }
        public decimal? AgencyPercentage { get; set; }
        public decimal? MortgageEffect { get; set; }
        public decimal? RemainingAgencyCost { get; set; }
        public decimal? WAEstimatedPropertyValue { get; set; }
        public decimal? AgencyFairValueDifference { get; set; }
        public decimal? ImpairmentProvision { get; set; }
        public decimal? FairValueReserve { get; set; }
        public decimal? AgencyFairValue { get; set; }
        public decimal? DisposalsCost { get; set; }
        public decimal? DurationInMonths { get; set; }
        public decimal? MonthOfCompletionSinceSchemeStart { get; set; }
        public decimal? DisposalMonthSinceCompletion { get; set; }
        public DateTime? IMSPaymentDate { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsAsset { get; set; }
        public string PropertyType { get; set; }
        public string Tenure { get; set; }
        public decimal? ExpectedStaircasingRate { get; set; }
        public decimal? EstimatedSalePrice { get; set; }
        public decimal? EstimatedValuation { get; set; }
        public decimal? RegionalSaleAdjust { get; set; }
        public decimal? RegionalStairAdjust { get; set; }
        public bool? NotLimitedByFirstCharge { get; set; }
        public decimal? EarlyMortgageIfNeverRepay { get; set; }
        public string ArrearsEffectAppliedOrLimited { get; set; }
        public decimal? RelativeSalePropertyTypeAndTenureAdjustment { get; set; }
        public decimal? RelativeStairPropertyTypeAndTenureAdjustment { get; set; }
        public bool? IsLondon { get; set; }
        public decimal? QuarterSpend { get; set; }
        public string MortgageProvider { get; set; }
        public string HouseType { get; set; }
        public decimal? PurchasePriceBand { get; set; }
        public decimal? HouseholdIncome { get; set; }
        public decimal? HouseholdFiveKIncomeBand { get; set; }
        public decimal? HouseholdFiftyKIncomeBand { get; set; }
        public bool? FirstTimeBuyer { get; set; }
        public int? AssetRegisterVersionId { get; set; }
    }
}
