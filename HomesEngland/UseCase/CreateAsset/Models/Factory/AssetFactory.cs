using System;
using System.Linq;
using HomesEngland.Domain.Factory;
using HomesEngland.UseCase.ImportAssets.Models.ParserExtensions;

namespace HomesEngland.UseCase.CreateAsset.Models.Factory
{
    public class CreateAssetRequestFactory : IFactory<CreateAssetRequest, CsvAsset>
    {
        public CreateAssetRequest Create(CsvAsset csvAsset)
        {
            if (CsvAssetInvalid(csvAsset))
            {
                return null;
            }

            var fields = csvAsset?.CsvLine?.Split(csvAsset.Delimiter);
            int.TryParse(fields.ElementAtOrDefault(2), out var schemeId);
            int.TryParse(fields.ElementAtOrDefault(5), out var noOfBeds);
            DateTime? completionDateForHpiStart = fields.ElementAtOrDefault(14).TryParseDateTimeNullable();
            DateTime? imsActualCompletionDate = fields.ElementAtOrDefault(15).TryParseDateTimeNullable();
            DateTime? imsExpectedCompletionDate = fields.ElementAtOrDefault(16).TryParseDateTimeNullable();
            DateTime? imsLegalCompletionDate = fields.ElementAtOrDefault(17).TryParseDateTimeNullable();
            DateTime? hopCompletionDate = fields.ElementAtOrDefault(18).TryParseDateTimeNullable();

            decimal? deposit = fields.ElementAtOrDefault(19).TryParseDecimalNullable();
            decimal? agencyEquityLoan = fields.ElementAtOrDefault(20).TryParseDecimalNullable();
            decimal? developerEquityLoan = fields.ElementAtOrDefault(21).TryParseDecimalNullable();
            decimal? shareOfRestrictedEquity = fields.ElementAtOrDefault(22).TryParseDecimalNullable("%");
            decimal? developerDiscount = fields.ElementAtOrDefault(23).TryParseDecimalNullable();
            decimal? mortgage = fields.ElementAtOrDefault(24).TryParseDecimalNullable();
            decimal? purchasePrice = fields.ElementAtOrDefault(25).TryParseDecimalNullable();

            decimal? fees = fields.ElementAtOrDefault(26).TryParseDecimalNullable();
            decimal? historicUnallocatedFees = fields.ElementAtOrDefault(27).TryParseDecimalNullable();

            decimal? actualAgencyEquityCostIncludingHomeBuyAgentFee = fields.ElementAtOrDefault(28).TryParseDecimalNullable();
            DateTime? fullDisposalDate = fields.ElementAtOrDefault(29).TryParseDateTimeNullable();
            decimal? originalAgencyPercentage = fields.ElementAtOrDefault(30).TryParseDecimalNullable("%");

            decimal? staircasingPercentage = fields.ElementAtOrDefault(31).TryParseDecimalNullable("%");

            decimal? newAgencyPercentage = fields.ElementAtOrDefault(32).TryParseDecimalNullable("%");
            int? invested = fields.ElementAtOrDefault(33).TryParseIntNullable();
            int? month = fields.ElementAtOrDefault(34).TryParseIntNullable();
            int? calendarYear = fields.ElementAtOrDefault(35).TryParseIntNullable();
            int? row = fields.ElementAtOrDefault(37).TryParseIntNullable();
            int? col = fields.ElementAtOrDefault(38).TryParseIntNullable();

            decimal? hpiStart = fields.ElementAtOrDefault(39).TryParseDecimalNullable();
            decimal? hpiEnd = fields.ElementAtOrDefault(40).TryParseDecimalNullable();
            decimal? hpiPlusMinus = fields.ElementAtOrDefault(41).TryParseDecimalNullable("%");
            decimal? agencyPercentage = fields.ElementAtOrDefault(42).TryParseDecimalNullable("%");
            decimal? mortgageEffect = fields.ElementAtOrDefault(43).TryParseDecimalNullable("%");
            decimal? remainingAgencyCost = fields.ElementAtOrDefault(44).TryParseDecimalNullable();
            decimal? waEstimatedPropertyValue = fields.ElementAtOrDefault(45).TryParseDecimalNullable();
            decimal? agencyFairValueDifference = fields.ElementAtOrDefault(46).TryParseDecimalNullable();
            decimal? impairmentProvision = fields.ElementAtOrDefault(47).TryParseDecimalNullable();
            decimal? fairValueReserve = fields.ElementAtOrDefault(48).TryParseDecimalNullable();
            decimal? agencyFairValue = fields.ElementAtOrDefault(49).TryParseDecimalNullable();
            decimal? disposalsCost = fields.ElementAtOrDefault(50).TryParseDecimalNullable();
            decimal? durationInMonths = fields.ElementAtOrDefault(51).TryParseDecimalNullable();
            decimal? monthOfCompletionSinceSchemeStart = fields.ElementAtOrDefault(52).TryParseDecimalNullable();
            decimal? disposalMonthSinceCompletion = fields.ElementAtOrDefault(53).TryParseDecimalNullable();

            DateTime? imsPaymentDate = fields.ElementAtOrDefault(54).TryParseDateTimeNullable();
            bool? isPaid = ParseIsPaid(fields.ElementAtOrDefault(55));
            bool? isAsset = ParseIsAsset(fields.ElementAtOrDefault(56));
            decimal? expectedStaircasingRate = fields.ElementAtOrDefault(59).TryParseDecimalNullable();
            decimal? estimatedSalePrice = fields.ElementAtOrDefault(60).TryParseDecimalNullable();
            decimal? estimatedValuation = fields.ElementAtOrDefault(61).TryParseDecimalNullable();
            decimal? regionalSaleAdjust = fields.ElementAtOrDefault(62).TryParseDecimalNullable();
            decimal? regionalStairAdjust = fields.ElementAtOrDefault(63).TryParseDecimalNullable();
            bool? notLimitedByFirstCharge = ParseNotLimitedByFirstCharge(fields.ElementAtOrDefault(64));
            decimal? earlyMortgageIfNeverRepay = fields.ElementAtOrDefault(65).TryParseDecimalNullable();
            decimal? relativeSalePropertyTypeAndTenureAdjustment =
                fields.ElementAtOrDefault(67).TryParseDecimalNullable();
            decimal? relativeStairPropertyTypeAndTenureAdjustment =
                fields.ElementAtOrDefault(68).TryParseDecimalNullable();
            bool? isLondon = ParseIsLondon(fields.ElementAtOrDefault(69));
            decimal? quarterSpend = fields.ElementAtOrDefault(70).TryParseDecimalNullable();
            decimal? purchasePriceBand = fields.ElementAtOrDefault(73).TryParseDecimalNullable();
            // household income
            decimal? householdIncome = fields.ElementAtOrDefault(74).TryParseDecimalNullable();
            decimal? householdFiveKIncomeBand = fields.ElementAtOrDefault(75).TryParseDecimalNullable();
            decimal? householdFiftyKIncomeBand = fields.ElementAtOrDefault(76).TryParseDecimalNullable();
            bool? firstTimeBuyer = ParseFirstTimeBuyer(fields.ElementAtOrDefault(77));

            var createAssetRequest = new CreateAssetRequest
            {
                Programme = fields.ElementAtOrDefault(0),
                EquityOwner = fields.ElementAtOrDefault(1),
                SchemeId = schemeId,
                LocationLaRegionName = fields.ElementAtOrDefault(3),
                ImsOldRegion = fields.ElementAtOrDefault(4),
                NoOfBeds = noOfBeds,
                Address = fields.ElementAtOrDefault(6),
                PropertyHouseName = fields.ElementAtOrDefault(7),
                PropertyStreetNumber = fields.ElementAtOrDefault(8),
                PropertyStreet = fields.ElementAtOrDefault(9),
                PropertyTown = fields.ElementAtOrDefault(10),
                PropertyPostcode = fields.ElementAtOrDefault(11),
                DevelopingRslName = fields.ElementAtOrDefault(12),
                LBHA = fields.ElementAtOrDefault(13),
                CompletionDateForHpiStart = completionDateForHpiStart,
                ImsActualCompletionDate = imsActualCompletionDate,
                ImsExpectedCompletionDate = imsExpectedCompletionDate,
                ImsLegalCompletionDate = imsLegalCompletionDate,
                HopCompletionDate = hopCompletionDate,
                Deposit = deposit,
                AgencyEquityLoan = agencyEquityLoan,
                DeveloperEquityLoan = developerEquityLoan,
                ShareOfRestrictedEquity = shareOfRestrictedEquity,
                DeveloperDiscount = developerDiscount,
                Mortgage = mortgage,
                PurchasePrice = purchasePrice,
                Fees = fees,
                HistoricUnallocatedFees = historicUnallocatedFees,
                ActualAgencyEquityCostIncludingHomeBuyAgentFee = actualAgencyEquityCostIncludingHomeBuyAgentFee,
                FullDisposalDate = fullDisposalDate,
                OriginalAgencyPercentage = originalAgencyPercentage,
                StaircasingPercentage = staircasingPercentage,
                NewAgencyPercentage = newAgencyPercentage,
                Invested = invested,
                Month = month,
                CalendarYear = calendarYear,
                MMYYYY = fields.ElementAtOrDefault(36),
                Row = row,
                Col = col,
                HPIStart = hpiStart,
                HPIEnd = hpiEnd,
                HPIPlusMinus = hpiPlusMinus,
                AgencyPercentage = agencyPercentage,
                MortgageEffect = mortgageEffect,
                RemainingAgencyCost = remainingAgencyCost,
                WAEstimatedPropertyValue = waEstimatedPropertyValue,
                AgencyFairValueDifference = agencyFairValueDifference,
                ImpairmentProvision = impairmentProvision,
                FairValueReserve = fairValueReserve,
                AgencyFairValue = agencyFairValue,
                DisposalsCost = disposalsCost,
                DurationInMonths = durationInMonths,
                MonthOfCompletionSinceSchemeStart = monthOfCompletionSinceSchemeStart,
                DisposalMonthSinceCompletion = disposalMonthSinceCompletion,
                IMSPaymentDate = imsPaymentDate,
                IsPaid = isPaid,
                IsAsset = isAsset,
                PropertyType = fields.ElementAtOrDefault(57),
                Tenure = fields.ElementAtOrDefault(58),
                ExpectedStaircasingRate = expectedStaircasingRate,
                EstimatedSalePrice = estimatedSalePrice,
                EstimatedValuation = estimatedValuation,
                RegionalSaleAdjust = regionalSaleAdjust,
                RegionalStairAdjust = regionalStairAdjust,
                NotLimitedByFirstCharge = notLimitedByFirstCharge,
                EarlyMortgageIfNeverRepay = earlyMortgageIfNeverRepay,
                ArrearsEffectAppliedOrLimited = fields.ElementAtOrDefault(66),
                RelativeSalePropertyTypeAndTenureAdjustment = relativeSalePropertyTypeAndTenureAdjustment,
                RelativeStairPropertyTypeAndTenureAdjustment = relativeStairPropertyTypeAndTenureAdjustment,
                IsLondon = isLondon,
                QuarterSpend = quarterSpend,
                MortgageProvider = fields.ElementAtOrDefault(71),
                HouseType = fields.ElementAtOrDefault(72),
                PurchasePriceBand = purchasePriceBand,
                HouseholdIncome = householdIncome,
                HouseholdFiveKIncomeBand = householdFiveKIncomeBand,
                HouseholdFiftyKIncomeBand = householdFiftyKIncomeBand,
                FirstTimeBuyer = firstTimeBuyer
            };
            return createAssetRequest;
        }

        private static bool CsvAssetInvalid(CsvAsset csvAsset)
        {
            return csvAsset == null || string.IsNullOrEmpty(csvAsset.CsvLine) ||
                   string.IsNullOrWhiteSpace(csvAsset.CsvLine) || string.IsNullOrWhiteSpace(csvAsset.Delimiter) ||
                   string.IsNullOrEmpty(csvAsset.Delimiter);
        }

        private bool ParseIsPaid(string isPaid)
        {
            if (string.IsNullOrEmpty(isPaid) || string.IsNullOrWhiteSpace(isPaid))
                return false;
            return isPaid.Trim().ToLower().Equals("paid");
        }

        private bool ParseIsAsset(string isAsset)
        {
            if (string.IsNullOrEmpty(isAsset) || string.IsNullOrWhiteSpace(isAsset))
                return false;
            return isAsset.Trim().ToLower().Equals("asset");
        }

        private bool ParseNotLimitedByFirstCharge(string notLimitedByFirstCharge)
        {
            // Hardcoded until we have more clarity
            return false;
        }

        private bool ParseIsLondon(string isLondon)
        {
            if (string.IsNullOrEmpty(isLondon) || string.IsNullOrWhiteSpace(isLondon))
                return false;
            return !isLondon.Trim().ToLower().Equals("non-london");
        }

        private bool ParseFirstTimeBuyer(string isFirstTimeBuyer)
        {
            if (string.IsNullOrEmpty(isFirstTimeBuyer) || string.IsNullOrWhiteSpace(isFirstTimeBuyer))
                return false;
            return isFirstTimeBuyer.Trim().ToLower().Equals("y");
        }
    }
}
