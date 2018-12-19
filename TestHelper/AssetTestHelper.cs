using System;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.GetAsset.Models;

namespace TestHelper
{

    public static class AssetTestHelper
    {
        /// <summary>
        /// Some database store Datetime Seconds fields to 6 decimal places instead of 7
        /// this helps compare the 2 entities in that case
        /// </summary>
        /// <param name="readAsset"></param>
        /// <param name="entity"></param>
        public static void AssetIsEqual(this IAsset readAsset, IAsset entity)
        {
            readAsset.Id.Should().Be(entity.Id);
            readAsset.ModifiedDateTime.Should().BeCloseTo(entity.ModifiedDateTime, TimeSpan.FromMilliseconds(1.0));

            readAsset.Programme.Should().BeEquivalentTo(entity.Programme);
            readAsset.EquityOwner.Should().BeEquivalentTo(entity.EquityOwner);
            readAsset.SchemeId.Should().Be(entity.SchemeId);
            readAsset.LocationLaRegionName.Should().BeEquivalentTo(entity.LocationLaRegionName);
            readAsset.ImsOldRegion.Should().BeEquivalentTo(entity.ImsOldRegion);
            readAsset.NoOfBeds.Should().Be(entity.NoOfBeds);
            readAsset.Address.Should().BeEquivalentTo(entity.Address);
            readAsset.PropertyHouseName.Should().BeEquivalentTo(entity.PropertyHouseName);
            readAsset.PropertyStreetNumber.Should().BeEquivalentTo(entity.PropertyStreetNumber);
            readAsset.PropertyStreet.Should().BeEquivalentTo(entity.PropertyStreet);
            readAsset.PropertyTown.Should().BeEquivalentTo(entity.PropertyTown);
            readAsset.PropertyPostcode.Should().BeEquivalentTo(entity.PropertyPostcode);
            readAsset.DevelopingRslName.Should().BeEquivalentTo(entity.DevelopingRslName);
            readAsset.LBHA.Should().BeEquivalentTo(entity.LBHA);
            readAsset.CompletionDateForHpiStart.Should().BeCloseTo(entity.CompletionDateForHpiStart.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.ImsActualCompletionDate.Should().BeCloseTo(entity.ImsActualCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.ImsExpectedCompletionDate.Should().BeCloseTo(entity.ImsExpectedCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.ImsLegalCompletionDate.Should().BeCloseTo(entity.ImsLegalCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.HopCompletionDate.Should().BeCloseTo(entity.HopCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.Deposit.Should().Be(entity.Deposit);
            readAsset.AgencyEquityLoan.Should().Be(entity.AgencyEquityLoan);
            readAsset.DeveloperEquityLoan.Should().Be(entity.DeveloperEquityLoan);
            readAsset.ShareOfRestrictedEquity.Should().Be(entity.ShareOfRestrictedEquity);
            readAsset.DeveloperDiscount.Should().Be(entity.DeveloperDiscount);
            readAsset.Mortgage.Should().Be(entity.Mortgage);
            readAsset.PurchasePrice.Should().Be(entity.PurchasePrice);
            readAsset.Fees.Should().Be(entity.Fees);
            readAsset.HistoricUnallocatedFees.Should().Be(entity.HistoricUnallocatedFees);
            readAsset.ActualAgencyEquityCostIncludingHomeBuyAgentFee.Should().Be(entity.ActualAgencyEquityCostIncludingHomeBuyAgentFee);
            readAsset.FullDisposalDate.Should().BeCloseTo(entity.FullDisposalDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.OriginalAgencyPercentage.Should().Be(entity.OriginalAgencyPercentage);
            readAsset.StaircasingPercentage.Should().Be(entity.StaircasingPercentage);
            readAsset.NewAgencyPercentage.Should().Be(entity.NewAgencyPercentage);
            readAsset.Invested.Should().Be(entity.Invested);
            readAsset.Month.Should().Be(entity.Month);
            readAsset.CalendarYear.Should().Be(entity.CalendarYear);
            readAsset.MMYYYY.Should().BeEquivalentTo(entity.MMYYYY);
            readAsset.Row.Should().Be(entity.Row);
            readAsset.Col.Should().Be(entity.Col);
            readAsset.HPIStart.Should().Be(entity.HPIStart);
            readAsset.HPIEnd.Should().Be(entity.HPIEnd);
            readAsset.HPIPlusMinus.Should().Be(entity.HPIPlusMinus);
            readAsset.AgencyPercentage.Should().Be(entity.AgencyPercentage);
            readAsset.MortgageEffect.Should().Be(entity.MortgageEffect);
            readAsset.RemainingAgencyCost.Should().Be(entity.RemainingAgencyCost);
            readAsset.WAEstimatedPropertyValue.Should().Be(entity.WAEstimatedPropertyValue);
            readAsset.AgencyFairValueDifference.Should().Be(entity.AgencyFairValueDifference);
            readAsset.ImpairmentProvision.Should().Be(entity.ImpairmentProvision);
            readAsset.FairValueReserve.Should().Be(entity.FairValueReserve);
            readAsset.AgencyFairValue.Should().Be(entity.AgencyFairValue);
            readAsset.DisposalsCost.Should().Be(entity.DisposalsCost);
            readAsset.DurationInMonths.Should().Be(entity.DurationInMonths);
            readAsset.MonthOfCompletionSinceSchemeStart.Should().Be(entity.MonthOfCompletionSinceSchemeStart);
            readAsset.DisposalMonthSinceCompletion.Should().Be(entity.DisposalMonthSinceCompletion);
            readAsset.IMSPaymentDate.Should().BeCloseTo(entity.IMSPaymentDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.IsPaid.Should().Be(entity.IsPaid);
            readAsset.IsAsset.Should().Be(entity.IsAsset);
            readAsset.PropertyType.Should().BeEquivalentTo(entity.PropertyType);
            readAsset.Tenure.Should().BeEquivalentTo(entity.Tenure);
            readAsset.ExpectedStaircasingRate.Should().Be(entity.ExpectedStaircasingRate);
            readAsset.EstimatedSalePrice.Should().Be(entity.EstimatedSalePrice);
            readAsset.RegionalSaleAdjust.Should().Be(entity.RegionalSaleAdjust);
            readAsset.RegionalStairAdjust.Should().Be(entity.RegionalStairAdjust);
            readAsset.NotLimitedByFirstCharge.Should().Be(entity.NotLimitedByFirstCharge);
            readAsset.EarlyMortgageIfNeverRepay.Should().Be(entity.EarlyMortgageIfNeverRepay);
            readAsset.ArrearsEffectAppliedOrLimited.Should().Be(entity.ArrearsEffectAppliedOrLimited);
            readAsset.RelativeSalePropertyTypeAndTenureAdjustment.Should().Be(entity.RelativeSalePropertyTypeAndTenureAdjustment);
            readAsset.RelativeStairPropertyTypeAndTenureAdjustment.Should().Be(entity.RelativeStairPropertyTypeAndTenureAdjustment);
            readAsset.IsLondon.Should().Be(entity.IsLondon);
            readAsset.QuarterSpend.Should().Be(entity.QuarterSpend);
            readAsset.MortgageProvider.Should().Be(entity.MortgageProvider);
            readAsset.HouseType.Should().Be(entity.HouseType);
            readAsset.PurchasePriceBand.Should().Be(entity.PurchasePriceBand);
            readAsset.HouseholdFiveKIncomeBand.Should().Be(entity.HouseholdFiveKIncomeBand);
            readAsset.HouseholdFiftyKIncomeBand.Should().Be(entity.HouseholdFiftyKIncomeBand);
            readAsset.FirstTimeBuyer.Should().Be(entity.FirstTimeBuyer);

            readAsset.HouseholdIncome.Should().Be(entity.HouseholdIncome);
            readAsset.EstimatedValuation.Should().Be(entity.EstimatedValuation);
        }

        /// <summary>
        /// Some database store Datetime Seconds fields to 6 decimal places instead of 7
        /// this helps compare the 2 entities in that case
        /// </summary>
        /// <param name="readAsset"></param>
        /// <param name="entity"></param>
        public static bool AssetIsEqual(this IAsset readAsset, CreateAssetRequest entity)
        {
            return
                readAsset.Programme.Equals(entity.Programme) &&
                readAsset.EquityOwner.Equals(entity.EquityOwner) &&
                readAsset.SchemeId.Equals(entity.SchemeId) &&
                readAsset.LocationLaRegionName.Equals(entity.LocationLaRegionName) &&
                readAsset.ImsOldRegion.Equals(entity.ImsOldRegion) &&
                readAsset.NoOfBeds.Equals(entity.NoOfBeds) &&
                readAsset.Address.Equals(entity.Address) &&
                readAsset.PropertyHouseName.Equals(entity.PropertyHouseName) &&
                readAsset.PropertyStreetNumber.Equals(entity.PropertyStreetNumber) &&
                readAsset.PropertyStreet.Equals(entity.PropertyStreet) &&
                readAsset.PropertyTown.Equals(entity.PropertyTown) &&
                readAsset.PropertyPostcode.Equals(entity.PropertyPostcode) &&
                readAsset.DevelopingRslName.Equals(entity.DevelopingRslName) &&
                readAsset.LBHA.Equals(entity.LBHA) &&
                readAsset.CompletionDateForHpiStart.Equals(entity.CompletionDateForHpiStart.Value) &&
                readAsset.ImsActualCompletionDate.Equals(entity.ImsActualCompletionDate.Value) &&
                readAsset.ImsExpectedCompletionDate.Equals(entity.ImsExpectedCompletionDate.Value) &&
                readAsset.ImsLegalCompletionDate.Equals(entity.ImsLegalCompletionDate.Value) &&
                readAsset.HopCompletionDate.Equals(entity.HopCompletionDate.Value) &&
                readAsset.Deposit.Equals(entity.Deposit) &&
                readAsset.AgencyEquityLoan.Equals(entity.AgencyEquityLoan) &&
                readAsset.DeveloperEquityLoan.Equals(entity.DeveloperEquityLoan) &&
                readAsset.ShareOfRestrictedEquity.Equals(entity.ShareOfRestrictedEquity) &&
                readAsset.DeveloperDiscount.Equals(entity.DeveloperDiscount) &&
                readAsset.Mortgage.Equals(entity.Mortgage) &&
                readAsset.PurchasePrice.Equals(entity.PurchasePrice) &&
                readAsset.Fees.Equals(entity.Fees) &&
                readAsset.HistoricUnallocatedFees.Equals(entity.HistoricUnallocatedFees) &&
                readAsset.ActualAgencyEquityCostIncludingHomeBuyAgentFee.Equals(entity
                    .ActualAgencyEquityCostIncludingHomeBuyAgentFee) &&
                readAsset.FullDisposalDate.Equals(entity.FullDisposalDate.Value) &&
                readAsset.OriginalAgencyPercentage.Equals(entity.OriginalAgencyPercentage) &&
                readAsset.StaircasingPercentage.Equals(entity.StaircasingPercentage) &&
                readAsset.NewAgencyPercentage.Equals(entity.NewAgencyPercentage) &&
                readAsset.Invested.Equals(entity.Invested) &&
                readAsset.Month.Equals(entity.Month) &&
                readAsset.CalendarYear.Equals(entity.CalendarYear) &&
                readAsset.MMYYYY.Equals(entity.MMYYYY) &&
                readAsset.Row.Equals(entity.Row) &&
                readAsset.Col.Equals(entity.Col) &&
                readAsset.HPIStart.Equals(entity.HPIStart) &&
                readAsset.HPIEnd.Equals(entity.HPIEnd) &&
                readAsset.HPIPlusMinus.Equals(entity.HPIPlusMinus) &&
                readAsset.AgencyPercentage.Equals(entity.AgencyPercentage) &&
                readAsset.MortgageEffect.Equals(entity.MortgageEffect) &&
                readAsset.RemainingAgencyCost.Equals(entity.RemainingAgencyCost) &&
                readAsset.WAEstimatedPropertyValue.Equals(entity.WAEstimatedPropertyValue) &&
                readAsset.AgencyFairValueDifference.Equals(entity.AgencyFairValueDifference) &&
                readAsset.ImpairmentProvision.Equals(entity.ImpairmentProvision) &&
                readAsset.FairValueReserve.Equals(entity.FairValueReserve) &&
                readAsset.AgencyFairValue.Equals(entity.AgencyFairValue) &&
                readAsset.DisposalsCost.Equals(entity.DisposalsCost) &&
                readAsset.DurationInMonths.Equals(entity.DurationInMonths) &&
                readAsset.MonthOfCompletionSinceSchemeStart.Equals(entity.MonthOfCompletionSinceSchemeStart) &&
                readAsset.DisposalMonthSinceCompletion.Equals(entity.DisposalMonthSinceCompletion) &&
                readAsset.IMSPaymentDate.Equals(entity.IMSPaymentDate.Value) &&
                readAsset.IsPaid.Equals(entity.IsPaid) &&
                readAsset.IsAsset.Equals(entity.IsAsset) &&
                readAsset.PropertyType.Equals(entity.PropertyType) &&
                readAsset.Tenure.Equals(entity.Tenure) &&
                readAsset.ExpectedStaircasingRate.Equals(entity.ExpectedStaircasingRate) &&
                readAsset.EstimatedSalePrice.Equals(entity.EstimatedSalePrice) &&
                readAsset.RegionalSaleAdjust.Equals(entity.RegionalSaleAdjust) &&
                readAsset.RegionalStairAdjust.Equals(entity.RegionalStairAdjust) &&
                readAsset.NotLimitedByFirstCharge.Equals(entity.NotLimitedByFirstCharge) &&
                readAsset.EarlyMortgageIfNeverRepay.Equals(entity.EarlyMortgageIfNeverRepay) &&
                readAsset.ArrearsEffectAppliedOrLimited.Equals(entity.ArrearsEffectAppliedOrLimited) &&
                readAsset.RelativeSalePropertyTypeAndTenureAdjustment.Equals(entity
                    .RelativeSalePropertyTypeAndTenureAdjustment) &&
                readAsset.RelativeStairPropertyTypeAndTenureAdjustment.Equals(entity
                    .RelativeStairPropertyTypeAndTenureAdjustment) &&
                readAsset.IsLondon.Equals(entity.IsLondon) &&
                readAsset.QuarterSpend.Equals(entity.QuarterSpend) &&
                readAsset.MortgageProvider.Equals(entity.MortgageProvider) &&
                readAsset.HouseType.Equals(entity.HouseType) &&
                readAsset.PurchasePriceBand.Equals(entity.PurchasePriceBand) &&
                readAsset.HouseholdFiveKIncomeBand.Equals(entity.HouseholdFiveKIncomeBand) &&
                readAsset.HouseholdFiftyKIncomeBand.Equals(entity.HouseholdFiftyKIncomeBand) &&
                readAsset.FirstTimeBuyer.Equals(entity.FirstTimeBuyer) &&

                readAsset.HouseholdIncome.Equals(entity.HouseholdIncome) &&
                readAsset.EstimatedValuation.Equals(entity.EstimatedValuation);
        }

        /// <summary>
        /// Some database store Datetime Seconds fields to 6 decimal places instead of 7
        /// this helps compare the 2 entities in that case
        /// </summary>
        /// <param name="readAsset"></param>
        /// <param name="entity"></param>
        public static void AssetOutputModelIsEqual(this AssetOutputModel readAsset, CreateAssetRequest entity)
        {
            readAsset.Programme.Should().BeEquivalentTo(entity.Programme);
            readAsset.EquityOwner.Should().BeEquivalentTo(entity.EquityOwner);
            readAsset.SchemeId.Should().Be(entity.SchemeId);
            readAsset.LocationLaRegionName.Should().BeEquivalentTo(entity.LocationLaRegionName);
            readAsset.ImsOldRegion.Should().BeEquivalentTo(entity.ImsOldRegion);
            readAsset.NoOfBeds.Should().Be(entity.NoOfBeds);
            readAsset.Address.Should().BeEquivalentTo(entity.Address);
            readAsset.PropertyHouseName.Should().BeEquivalentTo(entity.PropertyHouseName);
            readAsset.PropertyStreetNumber.Should().BeEquivalentTo(entity.PropertyStreetNumber);
            readAsset.PropertyStreet.Should().BeEquivalentTo(entity.PropertyStreet);
            readAsset.PropertyTown.Should().BeEquivalentTo(entity.PropertyTown);
            readAsset.PropertyPostcode.Should().BeEquivalentTo(entity.PropertyPostcode);
            readAsset.DevelopingRslName.Should().BeEquivalentTo(entity.DevelopingRslName);
            readAsset.LBHA.Should().BeEquivalentTo(entity.LBHA);
            readAsset.CompletionDateForHpiStart.Should().BeCloseTo(entity.CompletionDateForHpiStart.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.ImsActualCompletionDate.Should().BeCloseTo(entity.ImsActualCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.ImsExpectedCompletionDate.Should().BeCloseTo(entity.ImsExpectedCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.ImsLegalCompletionDate.Should().BeCloseTo(entity.ImsLegalCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.HopCompletionDate.Should().BeCloseTo(entity.HopCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.Deposit.Should().Be(entity.Deposit);
            readAsset.AgencyEquityLoan.Should().Be(entity.AgencyEquityLoan);
            readAsset.DeveloperEquityLoan.Should().Be(entity.DeveloperEquityLoan);
            readAsset.ShareOfRestrictedEquity.Should().Be(entity.ShareOfRestrictedEquity);
            readAsset.DeveloperDiscount.Should().Be(entity.DeveloperDiscount);
            readAsset.Mortgage.Should().Be(entity.Mortgage);
            readAsset.PurchasePrice.Should().Be(entity.PurchasePrice);
            readAsset.Fees.Should().Be(entity.Fees);
            readAsset.HistoricUnallocatedFees.Should().Be(entity.HistoricUnallocatedFees);
            readAsset.ActualAgencyEquityCostIncludingHomeBuyAgentFee.Should().Be(entity.ActualAgencyEquityCostIncludingHomeBuyAgentFee);
            readAsset.FullDisposalDate.Should().BeCloseTo(entity.FullDisposalDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.OriginalAgencyPercentage.Should().Be(entity.OriginalAgencyPercentage);
            readAsset.StaircasingPercentage.Should().Be(entity.StaircasingPercentage);
            readAsset.NewAgencyPercentage.Should().Be(entity.NewAgencyPercentage);
            readAsset.Invested.Should().Be(entity.Invested);
            readAsset.Month.Should().Be(entity.Month);
            readAsset.CalendarYear.Should().Be(entity.CalendarYear);
            readAsset.MMYYYY.Should().BeEquivalentTo(entity.MMYYYY);
            readAsset.Row.Should().Be(entity.Row);
            readAsset.Col.Should().Be(entity.Col);
            readAsset.HPIStart.Should().Be(entity.HPIStart);
            readAsset.HPIEnd.Should().Be(entity.HPIEnd);
            readAsset.HPIPlusMinus.Should().Be(entity.HPIPlusMinus);
            readAsset.AgencyPercentage.Should().Be(entity.AgencyPercentage);
            readAsset.MortgageEffect.Should().Be(entity.MortgageEffect);
            readAsset.RemainingAgencyCost.Should().Be(entity.RemainingAgencyCost);
            readAsset.WAEstimatedPropertyValue.Should().Be(entity.WAEstimatedPropertyValue);
            readAsset.AgencyFairValueDifference.Should().Be(entity.AgencyFairValueDifference);
            readAsset.ImpairmentProvision.Should().Be(entity.ImpairmentProvision);
            readAsset.FairValueReserve.Should().Be(entity.FairValueReserve);
            readAsset.AgencyFairValue.Should().Be(entity.AgencyFairValue);
            readAsset.DisposalsCost.Should().Be(entity.DisposalsCost);
            readAsset.DurationInMonths.Should().Be(entity.DurationInMonths);
            readAsset.MonthOfCompletionSinceSchemeStart.Should().Be(entity.MonthOfCompletionSinceSchemeStart);
            readAsset.DisposalMonthSinceCompletion.Should().Be(entity.DisposalMonthSinceCompletion);
            readAsset.IMSPaymentDate.Should().BeCloseTo(entity.IMSPaymentDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.IsPaid.Should().Be(entity.IsPaid);
            readAsset.IsAsset.Should().Be(entity.IsAsset);
            readAsset.PropertyType.Should().BeEquivalentTo(entity.PropertyType);
            readAsset.Tenure.Should().BeEquivalentTo(entity.Tenure);
            readAsset.ExpectedStaircasingRate.Should().Be(entity.ExpectedStaircasingRate);
            readAsset.EstimatedSalePrice.Should().Be(entity.EstimatedSalePrice);
            readAsset.RegionalSaleAdjust.Should().Be(entity.RegionalSaleAdjust);
            readAsset.RegionalStairAdjust.Should().Be(entity.RegionalStairAdjust);
            readAsset.NotLimitedByFirstCharge.Should().Be(entity.NotLimitedByFirstCharge);
            readAsset.EarlyMortgageIfNeverRepay.Should().Be(entity.EarlyMortgageIfNeverRepay);
            readAsset.ArrearsEffectAppliedOrLimited.Should().Be(entity.ArrearsEffectAppliedOrLimited);
            readAsset.RelativeSalePropertyTypeAndTenureAdjustment.Should().Be(entity.RelativeSalePropertyTypeAndTenureAdjustment);
            readAsset.RelativeStairPropertyTypeAndTenureAdjustment.Should().Be(entity.RelativeStairPropertyTypeAndTenureAdjustment);
            readAsset.IsLondon.Should().Be(entity.IsLondon);
            readAsset.QuarterSpend.Should().Be(entity.QuarterSpend);
            readAsset.MortgageProvider.Should().Be(entity.MortgageProvider);
            readAsset.HouseType.Should().Be(entity.HouseType);
            readAsset.PurchasePriceBand.Should().Be(entity.PurchasePriceBand);
            readAsset.HouseholdFiveKIncomeBand.Should().Be(entity.HouseholdFiveKIncomeBand);
            readAsset.HouseholdFiftyKIncomeBand.Should().Be(entity.HouseholdFiftyKIncomeBand);
            readAsset.FirstTimeBuyer.Should().Be(entity.FirstTimeBuyer);

            readAsset.HouseholdIncome.Should().Be(entity.HouseholdIncome);
            readAsset.EstimatedValuation.Should().Be(entity.EstimatedValuation);
        }

        /// <summary>
        /// Some database store Datetime Seconds fields to 6 decimal places instead of 7
        /// this helps compare the 2 entities in that case
        /// </summary>
        /// <param name="readAsset"></param>
        /// <param name="entity"></param>
        public static void AssetOutputModelIsEqual(this AssetOutputModel readAsset, AssetOutputModel entity)
        {
            readAsset.Id.Should().Be(entity.Id);
            readAsset.ModifiedDateTime.Should().BeCloseTo(entity.ModifiedDateTime, TimeSpan.FromMilliseconds(1.0));

            readAsset.Programme.Should().BeEquivalentTo(entity.Programme);
            readAsset.EquityOwner.Should().BeEquivalentTo(entity.EquityOwner);
            readAsset.SchemeId.Should().Be(entity.SchemeId);
            readAsset.LocationLaRegionName.Should().BeEquivalentTo(entity.LocationLaRegionName);
            readAsset.ImsOldRegion.Should().BeEquivalentTo(entity.ImsOldRegion);
            readAsset.NoOfBeds.Should().Be(entity.NoOfBeds);
            readAsset.Address.Should().BeEquivalentTo(entity.Address);
            readAsset.PropertyHouseName.Should().BeEquivalentTo(entity.PropertyHouseName);
            readAsset.PropertyStreetNumber.Should().BeEquivalentTo(entity.PropertyStreetNumber);
            readAsset.PropertyStreet.Should().BeEquivalentTo(entity.PropertyStreet);
            readAsset.PropertyTown.Should().BeEquivalentTo(entity.PropertyTown);
            readAsset.PropertyPostcode.Should().BeEquivalentTo(entity.PropertyPostcode);
            readAsset.DevelopingRslName.Should().BeEquivalentTo(entity.DevelopingRslName);
            readAsset.LBHA.Should().BeEquivalentTo(entity.LBHA);
            readAsset.CompletionDateForHpiStart.Should().BeCloseTo(entity.CompletionDateForHpiStart.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.ImsActualCompletionDate.Should().BeCloseTo(entity.ImsActualCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.ImsExpectedCompletionDate.Should().BeCloseTo(entity.ImsExpectedCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.ImsLegalCompletionDate.Should().BeCloseTo(entity.ImsLegalCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.HopCompletionDate.Should().BeCloseTo(entity.HopCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.Deposit.Should().Be(entity.Deposit);
            readAsset.AgencyEquityLoan.Should().Be(entity.AgencyEquityLoan);
            readAsset.DeveloperEquityLoan.Should().Be(entity.DeveloperEquityLoan);
            readAsset.ShareOfRestrictedEquity.Should().Be(entity.ShareOfRestrictedEquity);
            readAsset.DeveloperDiscount.Should().Be(entity.DeveloperDiscount);
            readAsset.Mortgage.Should().Be(entity.Mortgage);
            readAsset.PurchasePrice.Should().Be(entity.PurchasePrice);
            readAsset.Fees.Should().Be(entity.Fees);
            readAsset.HistoricUnallocatedFees.Should().Be(entity.HistoricUnallocatedFees);
            readAsset.ActualAgencyEquityCostIncludingHomeBuyAgentFee.Should().Be(entity.ActualAgencyEquityCostIncludingHomeBuyAgentFee);
            readAsset.FullDisposalDate.Should().BeCloseTo(entity.FullDisposalDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.OriginalAgencyPercentage.Should().Be(entity.OriginalAgencyPercentage);
            readAsset.StaircasingPercentage.Should().Be(entity.StaircasingPercentage);
            readAsset.NewAgencyPercentage.Should().Be(entity.NewAgencyPercentage);
            readAsset.Invested.Should().Be(entity.Invested);
            readAsset.Month.Should().Be(entity.Month);
            readAsset.CalendarYear.Should().Be(entity.CalendarYear);
            readAsset.MMYYYY.Should().BeEquivalentTo(entity.MMYYYY);
            readAsset.Row.Should().Be(entity.Row);
            readAsset.Col.Should().Be(entity.Col);
            readAsset.HPIStart.Should().Be(entity.HPIStart);
            readAsset.HPIEnd.Should().Be(entity.HPIEnd);
            readAsset.HPIPlusMinus.Should().Be(entity.HPIPlusMinus);
            readAsset.AgencyPercentage.Should().Be(entity.AgencyPercentage);
            readAsset.MortgageEffect.Should().Be(entity.MortgageEffect);
            readAsset.RemainingAgencyCost.Should().Be(entity.RemainingAgencyCost);
            readAsset.WAEstimatedPropertyValue.Should().Be(entity.WAEstimatedPropertyValue);
            readAsset.AgencyFairValueDifference.Should().Be(entity.AgencyFairValueDifference);
            readAsset.ImpairmentProvision.Should().Be(entity.ImpairmentProvision);
            readAsset.FairValueReserve.Should().Be(entity.FairValueReserve);
            readAsset.AgencyFairValue.Should().Be(entity.AgencyFairValue);
            readAsset.DisposalsCost.Should().Be(entity.DisposalsCost);
            readAsset.DurationInMonths.Should().Be(entity.DurationInMonths);
            readAsset.MonthOfCompletionSinceSchemeStart.Should().Be(entity.MonthOfCompletionSinceSchemeStart);
            readAsset.DisposalMonthSinceCompletion.Should().Be(entity.DisposalMonthSinceCompletion);
            readAsset.IMSPaymentDate.Should().BeCloseTo(entity.IMSPaymentDate.Value, TimeSpan.FromMilliseconds(1.0));
            readAsset.IsPaid.Should().Be(entity.IsPaid);
            readAsset.IsAsset.Should().Be(entity.IsAsset);
            readAsset.PropertyType.Should().BeEquivalentTo(entity.PropertyType);
            readAsset.Tenure.Should().BeEquivalentTo(entity.Tenure);
            readAsset.ExpectedStaircasingRate.Should().Be(entity.ExpectedStaircasingRate);
            readAsset.EstimatedSalePrice.Should().Be(entity.EstimatedSalePrice);
            readAsset.RegionalSaleAdjust.Should().Be(entity.RegionalSaleAdjust);
            readAsset.RegionalStairAdjust.Should().Be(entity.RegionalStairAdjust);
            readAsset.NotLimitedByFirstCharge.Should().Be(entity.NotLimitedByFirstCharge);
            readAsset.EarlyMortgageIfNeverRepay.Should().Be(entity.EarlyMortgageIfNeverRepay);
            readAsset.ArrearsEffectAppliedOrLimited.Should().Be(entity.ArrearsEffectAppliedOrLimited);
            readAsset.RelativeSalePropertyTypeAndTenureAdjustment.Should().Be(entity.RelativeSalePropertyTypeAndTenureAdjustment);
            readAsset.RelativeStairPropertyTypeAndTenureAdjustment.Should().Be(entity.RelativeStairPropertyTypeAndTenureAdjustment);
            readAsset.IsLondon.Should().Be(entity.IsLondon);
            readAsset.QuarterSpend.Should().Be(entity.QuarterSpend);
            readAsset.MortgageProvider.Should().Be(entity.MortgageProvider);
            readAsset.HouseType.Should().Be(entity.HouseType);
            readAsset.PurchasePriceBand.Should().Be(entity.PurchasePriceBand);
            readAsset.HouseholdFiveKIncomeBand.Should().Be(entity.HouseholdFiveKIncomeBand);
            readAsset.HouseholdFiftyKIncomeBand.Should().Be(entity.HouseholdFiftyKIncomeBand);
            readAsset.FirstTimeBuyer.Should().Be(entity.FirstTimeBuyer);

            readAsset.HouseholdIncome.Should().Be(entity.HouseholdIncome);
            readAsset.EstimatedValuation.Should().Be(entity.EstimatedValuation);
        }
    }
}
