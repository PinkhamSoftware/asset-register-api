using System;
using HomesEngland.UseCase.CreateAsset.Models;

namespace HomesEngland.Domain
{
    public class Asset:IAsset
    {
        public int Id { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        public Asset() { }
        public Asset(CreateAssetRequest request)
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
            EstimatedValuation = request.EstimatedValuation;
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
            HouseholdIncome = request.HouseholdIncome;
            HouseholdFiveKIncomeBand = request.HouseholdFiveKIncomeBand;
            HouseholdFiftyKIncomeBand = request.HouseholdFiftyKIncomeBand;
            FirstTimeBuyer = request.FirstTimeBuyer;
        }

        public Asset(IAsset request)
        {
            Id = request.Id;
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

            AssetRegisterVersionId = request.AssetRegisterVersionId;
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
