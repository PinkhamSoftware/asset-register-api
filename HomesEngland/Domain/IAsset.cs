using System;

namespace HomesEngland.Domain
{
    public interface IAsset:IDatabaseEntity<int>
    {
        string Programme { get; set; }
        string EquityOwner { get; set; }
        int? SchemeId { get; set; }
        string LocationLaRegionName { get; set; }
        string ImsOldRegion { get; set; }
        int? NoOfBeds { get; set; }
        string Address { get; set; }
        string PropertyHouseName { get; set; }
        string PropertyStreetNumber { get; set; }
        string PropertyStreet { get; set; }
        string PropertyTown { get; set; }
        string PropertyPostcode { get; set; }
        string DevelopingRslName { get; set; }
        string LBHA { get; set; }
        DateTime? CompletionDateForHpiStart { get; set; }
        DateTime? ImsActualCompletionDate { get; set; }
        DateTime? ImsExpectedCompletionDate { get; set; }
        DateTime? ImsLegalCompletionDate { get; set; }
        DateTime? HopCompletionDate { get; set; }
        decimal? Deposit { get; set; }
        decimal? AgencyEquityLoan { get; set; }
        decimal? DeveloperEquityLoan { get; set; }
        decimal? ShareOfRestrictedEquity { get; set; }
        decimal? DeveloperDiscount { get; set; }
        decimal? Mortgage { get; set; }
        decimal? PurchasePrice { get; set; }

        decimal? Fees { get; set; }
        decimal? HistoricUnallocatedFees { get; set; }
        decimal? ActualAgencyEquityCostIncludingHomeBuyAgentFee { get; set; }

        DateTime? FullDisposalDate { get; set; }
        decimal? OriginalAgencyPercentage { get; set; }
        decimal? StaircasingPercentage { get; set; }
        decimal? NewAgencyPercentage { get; set; }
        int? Invested { get; set; }

        int? Month { get; set; }
        int? CalendarYear { get; set; }
        string MMYYYY { get; set; }
        int? Row { get; set; }
        int? Col { get; set; }

        decimal? HPIStart { get; set; }
        decimal? HPIEnd { get; set; }
        decimal? HPIPlusMinus { get; set; }
        decimal? AgencyPercentage { get; set; }
        decimal? MortgageEffect { get; set; }
        decimal? RemainingAgencyCost { get; set; }

        decimal? WAEstimatedPropertyValue { get; set; }
        decimal? AgencyFairValueDifference{ get; set; }
        decimal? ImpairmentProvision{ get; set; }
        decimal? FairValueReserve { get; set; }
        decimal? AgencyFairValue { get; set; }
        decimal? DisposalsCost { get; set; }
        decimal? DurationInMonths { get; set; }

        decimal? MonthOfCompletionSinceSchemeStart { get; set; }

        decimal? DisposalMonthSinceCompletion { get; set; }
        DateTime? IMSPaymentDate { get; set; }
        
        bool? IsPaid { get; set; }
        bool? IsAsset { get; set; }
        string PropertyType { get; set; }
        string Tenure { get; set; }
        decimal? ExpectedStaircasingRate { get; set; }
        decimal? EstimatedSalePrice { get; set; }
        decimal? EstimatedValuation { get; set; }
        decimal? RegionalSaleAdjust { get; set; }
        decimal? RegionalStairAdjust { get; set; }
        bool? NotLimitedByFirstCharge { get; set; }
        decimal? EarlyMortgageIfNeverRepay { get; set; }
        string ArrearsEffectAppliedOrLimited { get; set; }
        decimal? RelativeSalePropertyTypeAndTenureAdjustment { get; set; }
        decimal? RelativeStairPropertyTypeAndTenureAdjustment { get; set; }
        bool? IsLondon { get; set; }
        decimal? QuarterSpend { get; set; }
        string MortgageProvider { get; set; }
        string HouseType { get; set; }
        decimal? PurchasePriceBand { get; set; }
        decimal? HouseholdIncome { get; set; }
        decimal? HouseholdFiveKIncomeBand { get; set; }
        decimal? HouseholdFiftyKIncomeBand { get; set; }
        bool? FirstTimeBuyer { get; set; }

        int? AssetRegisterVersionId { get; set; }

    }
}
