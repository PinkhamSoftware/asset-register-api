using System;
using FluentAssertions;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.GetAsset.Models;

namespace TestHelper
{
    public static class AssetOutputModelTestHelper
    {
        /// <summary>
        /// Some database store Datetime Seconds fields to 6 decimal places instead of 7
        /// this helps compare the 2 entities in that case
        /// </summary>
        /// <param name="assetOutputModel"></param>
        /// <param name="entity"></param>
        public static void AssetOutputModelIsEqual(this CreateAssetRequest assetOutputModel, AssetOutputModel entity)
        {
            assetOutputModel.Programme.Should().BeEquivalentTo(entity.Programme);
            assetOutputModel.EquityOwner.Should().BeEquivalentTo(entity.EquityOwner);
            assetOutputModel.SchemeId.Should().Be(entity.SchemeId);
            assetOutputModel.LocationLaRegionName.Should().BeEquivalentTo(entity.LocationLaRegionName);
            assetOutputModel.ImsOldRegion.Should().BeEquivalentTo(entity.ImsOldRegion);
            assetOutputModel.NoOfBeds.Should().Be(entity.NoOfBeds);
            assetOutputModel.Address.Should().BeEquivalentTo(entity.Address);
            assetOutputModel.PropertyHouseName.Should().BeEquivalentTo(entity.PropertyHouseName);
            assetOutputModel.PropertyStreetNumber.Should().BeEquivalentTo(entity.PropertyStreetNumber);
            assetOutputModel.PropertyStreet.Should().BeEquivalentTo(entity.PropertyStreet);
            assetOutputModel.PropertyTown.Should().BeEquivalentTo(entity.PropertyTown);
            assetOutputModel.PropertyPostcode.Should().BeEquivalentTo(entity.PropertyPostcode);
            assetOutputModel.DevelopingRslName.Should().BeEquivalentTo(entity.DevelopingRslName);
            assetOutputModel.LBHA.Should().BeEquivalentTo(entity.LBHA);
            assetOutputModel.CompletionDateForHpiStart.Should().BeCloseTo(entity.CompletionDateForHpiStart.Value, TimeSpan.FromMilliseconds(1.0));
            assetOutputModel.ImsActualCompletionDate.Should().BeCloseTo(entity.ImsActualCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            assetOutputModel.ImsExpectedCompletionDate.Should().BeCloseTo(entity.ImsExpectedCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            assetOutputModel.ImsLegalCompletionDate.Should().BeCloseTo(entity.ImsLegalCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            assetOutputModel.HopCompletionDate.Should().BeCloseTo(entity.HopCompletionDate.Value, TimeSpan.FromMilliseconds(1.0));
            assetOutputModel.Deposit.Should().Be(entity.Deposit);
            assetOutputModel.AgencyEquityLoan.Should().Be(entity.AgencyEquityLoan);
            assetOutputModel.DeveloperEquityLoan.Should().Be(entity.DeveloperEquityLoan);
            assetOutputModel.ShareOfRestrictedEquity.Should().Be(entity.ShareOfRestrictedEquity);
            assetOutputModel.DeveloperDiscount.Should().Be(entity.DeveloperDiscount);
            assetOutputModel.Mortgage.Should().Be(entity.Mortgage);
            assetOutputModel.PurchasePrice.Should().Be(entity.PurchasePrice);
            assetOutputModel.Fees.Should().Be(entity.Fees);
            assetOutputModel.HistoricUnallocatedFees.Should().Be(entity.HistoricUnallocatedFees);
            assetOutputModel.ActualAgencyEquityCostIncludingHomeBuyAgentFee.Should().Be(entity.ActualAgencyEquityCostIncludingHomeBuyAgentFee);
            assetOutputModel.FullDisposalDate.Should().BeCloseTo(entity.FullDisposalDate.Value, TimeSpan.FromMilliseconds(1.0));
            assetOutputModel.OriginalAgencyPercentage.Should().Be(entity.OriginalAgencyPercentage);
            assetOutputModel.StaircasingPercentage.Should().Be(entity.StaircasingPercentage);
            assetOutputModel.NewAgencyPercentage.Should().Be(entity.NewAgencyPercentage);
            assetOutputModel.Invested.Should().Be(entity.Invested);
            assetOutputModel.Month.Should().Be(entity.Month);
            assetOutputModel.CalendarYear.Should().Be(entity.CalendarYear);
            assetOutputModel.MMYYYY.Should().BeEquivalentTo(entity.MMYYYY);
            assetOutputModel.Row.Should().Be(entity.Row);
            assetOutputModel.Col.Should().Be(entity.Col);
            assetOutputModel.HPIStart.Should().Be(entity.HPIStart);
            assetOutputModel.HPIEnd.Should().Be(entity.HPIEnd);
            assetOutputModel.HPIPlusMinus.Should().Be(entity.HPIPlusMinus);
            assetOutputModel.AgencyPercentage.Should().Be(entity.AgencyPercentage);
            assetOutputModel.MortgageEffect.Should().Be(entity.MortgageEffect);
            assetOutputModel.RemainingAgencyCost.Should().Be(entity.RemainingAgencyCost);
            assetOutputModel.WAEstimatedPropertyValue.Should().Be(entity.WAEstimatedPropertyValue);
            assetOutputModel.AgencyFairValueDifference.Should().Be(entity.AgencyFairValueDifference);
            assetOutputModel.ImpairmentProvision.Should().Be(entity.ImpairmentProvision);
            assetOutputModel.FairValueReserve.Should().Be(entity.FairValueReserve);
            assetOutputModel.AgencyFairValue.Should().Be(entity.AgencyFairValue);
            assetOutputModel.DisposalsCost.Should().Be(entity.DisposalsCost);
            assetOutputModel.DurationInMonths.Should().Be(entity.DurationInMonths);
            assetOutputModel.MonthOfCompletionSinceSchemeStart.Should().Be(entity.MonthOfCompletionSinceSchemeStart);
            assetOutputModel.DisposalMonthSinceCompletion.Should().Be(entity.DisposalMonthSinceCompletion);
            assetOutputModel.IMSPaymentDate.Should().BeCloseTo(entity.IMSPaymentDate.Value, TimeSpan.FromMilliseconds(1.0));
            assetOutputModel.IsPaid.Should().Be(entity.IsPaid);
            assetOutputModel.IsAsset.Should().Be(entity.IsAsset);
            assetOutputModel.PropertyType.Should().BeEquivalentTo(entity.PropertyType);
            assetOutputModel.Tenure.Should().BeEquivalentTo(entity.Tenure);
            assetOutputModel.ExpectedStaircasingRate.Should().Be(entity.ExpectedStaircasingRate);
            assetOutputModel.EstimatedSalePrice.Should().Be(entity.EstimatedSalePrice);
            assetOutputModel.RegionalSaleAdjust.Should().Be(entity.RegionalSaleAdjust);
            assetOutputModel.RegionalStairAdjust.Should().Be(entity.RegionalStairAdjust);
            assetOutputModel.NotLimitedByFirstCharge.Should().Be(entity.NotLimitedByFirstCharge);
            assetOutputModel.EarlyMortgageIfNeverRepay.Should().Be(entity.EarlyMortgageIfNeverRepay);
            assetOutputModel.ArrearsEffectAppliedOrLimited.Should().Be(entity.ArrearsEffectAppliedOrLimited);
            assetOutputModel.RelativeSalePropertyTypeAndTenureAdjustment.Should().Be(entity.RelativeSalePropertyTypeAndTenureAdjustment);
            assetOutputModel.RelativeStairPropertyTypeAndTenureAdjustment.Should().Be(entity.RelativeStairPropertyTypeAndTenureAdjustment);
            assetOutputModel.IsLondon.Should().Be(entity.IsLondon);
            assetOutputModel.QuarterSpend.Should().Be(entity.QuarterSpend);
            assetOutputModel.MortgageProvider.Should().Be(entity.MortgageProvider);
            assetOutputModel.HouseType.Should().Be(entity.HouseType);
            assetOutputModel.PurchasePriceBand.Should().Be(entity.PurchasePriceBand);
            assetOutputModel.HouseholdFiveKIncomeBand.Should().Be(entity.HouseholdFiveKIncomeBand);
            assetOutputModel.HouseholdFiftyKIncomeBand.Should().Be(entity.HouseholdFiftyKIncomeBand);
            assetOutputModel.FirstTimeBuyer.Should().Be(entity.FirstTimeBuyer);

            assetOutputModel.HouseholdIncome.Should().Be(entity.HouseholdIncome);
            assetOutputModel.EstimatedValuation.Should().Be(entity.EstimatedValuation);
        }
    }
}
