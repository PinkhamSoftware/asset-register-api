using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using HomesEngland.UseCase.CreateAsset;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.GenerateAssets.Models;
using HomesEngland.UseCase.GetAsset.Models;

namespace HomesEngland.UseCase.GenerateAssets.Impl
{
    public class GenerateAssetsUseCase : IGenerateAssetsUseCase
    {
        private readonly ICreateAssetUseCase _createAssetUseCase;

        public GenerateAssetsUseCase(ICreateAssetUseCase createAssetUseCase)
        {
            _createAssetUseCase = createAssetUseCase;
        }

        public async Task<GenerateAssetsResponse> ExecuteAsync(GenerateAssetsRequest request,
            CancellationToken cancellationToken)
        {
            IList<AssetOutputModel> createdList = new List<AssetOutputModel>();

            for (int index = 0; index < request.Records; index++)
            {
                var createAssetRequest = GenerateCreateAssetRequest();
                var response = await _createAssetUseCase.ExecuteAsync(createAssetRequest, cancellationToken)
                    .ConfigureAwait(false);
                createdList.Add(response?.Asset);
            }

            var generateAssetsResponse = new GenerateAssetsResponse
            {
                RecordsGenerated = createdList
            };
            return generateAssetsResponse;
        }

        private CreateAssetRequest GenerateCreateAssetRequest()
        {
            var random = new Random(0);

            var faker = new Faker("en");
            var completionDateForHpiStart = faker.Date.Soon(random.Next(1, 15));
            var imsActualCompletionDate = faker.Date.Soon(random.Next(30, 90));
            var imsExpectedCompletionDate = faker.Date.Soon(random.Next(15, 90));
            var hopCompletionDate = faker.Date.Soon(random.Next(15, 90));
            var differenceFromImsExpectedCompletionToHopCompletionDate =
                (imsExpectedCompletionDate.Date - hopCompletionDate).Days;


            var appliedOrLimited = new List<string> {"Applied", "Limited"};
            var houseType = new List<string> {"Semi-Detached", "Detached"};
            var holdTypes = new List<string> {"Freehold", "Leasehold"};

            var generatedAsset = new Faker<CreateAssetRequest>("en")
                .RuleFor(asset => asset.Programme, (fake, model) => fake.Company.CompanyName())
                .RuleFor(asset => asset.EquityOwner, (fake, model) => fake.Company.CompanyName())
                .RuleFor(property => property.SchemeId, (fake, model) => fake.IndexGlobal + 1)
                .RuleFor(property => property.LocationLaRegionName, (fake, model) => fake.Address.County())
                .RuleFor(property => property.ImsOldRegion, (fake, model) => fake.Address.County())
                .RuleFor(property => property.NoOfBeds, (fake, model) => fake.Random.Int(1, 4))
                .RuleFor(property => property.Address, (fake, model) => fake.Address.FullAddress())
                .RuleFor(asset => asset.PropertyHouseName, (fake, model) => fake.Address.StreetName())
                .RuleFor(asset => asset.PropertyStreetNumber, (fake, model) => fake.Address.BuildingNumber())
                .RuleFor(asset => asset.PropertyStreet, (fake, model) => fake.Address.StreetName())
                .RuleFor(asset => asset.PropertyTown, (fake, model) => fake.Address.City())
                .RuleFor(asset => asset.PropertyPostcode, (fake, model) => fake.Address.ZipCode())
                .RuleFor(property => property.DevelopingRslName, (fake, model) => fake.Company.CompanyName())
                .RuleFor(asset => asset.LBHA, (fake, model) => fake.Company.CompanyName())
                .RuleFor(property => property.CompletionDateForHpiStart, (fake, model) => completionDateForHpiStart)
                .RuleFor(property => property.ImsActualCompletionDate, (fake, model) => imsActualCompletionDate)
                .RuleFor(property => property.ImsExpectedCompletionDate, (fake, model) => imsExpectedCompletionDate)
                .RuleFor(property => property.ImsLegalCompletionDate,
                    (fake, model) => fake.Date.Soon(random.Next(15, 90)))
                .RuleFor(property => property.HopCompletionDate, (fake, model) => hopCompletionDate)
                .RuleFor(property => property.AgencyEquityLoan, (fake, model) => fake.Finance.Amount(5000m, 100000m))
                .RuleFor(property => property.DeveloperEquityLoan, (fake, model) => fake.Finance.Amount(5000m, 100000m))
                .RuleFor(property => property.ShareOfRestrictedEquity, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.DeveloperDiscount, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.Mortgage, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.PurchasePrice, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.Fees, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.HistoricUnallocatedFees, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.ActualAgencyEquityCostIncludingHomeBuyAgentFee,
                    (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.FullDisposalDate, (fake, model) => fake.Date.Soon(random.Next(15, 90)))
                .RuleFor(asset => asset.OriginalAgencyPercentage, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.StaircasingPercentage, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.NewAgencyPercentage, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.Invested, (fake, model) => fake.Random.Int(0, 1))
                .RuleFor(asset => asset.Month, (fake, model) => fake.Random.Int(1, 12))
                .RuleFor(asset => asset.CalendarYear, (fake, model) => fake.Random.Int(1985, 2018))
                .RuleFor(asset => asset.MMYYYY, (fake, model) => fake.Date.Soon(1).ToString("MMYYYY"))
                .RuleFor(asset => asset.Row, (fake, model) => fake.Random.Int())
                .RuleFor(asset => asset.Col, (fake, model) => fake.Random.Int())
                .RuleFor(asset => asset.HPIStart, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.HPIEnd, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.HPIPlusMinus, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.AgencyPercentage, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.MortgageEffect, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.RemainingAgencyCost, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.WAEstimatedPropertyValue, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.AgencyFairValueDifference, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.ImpairmentProvision, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.FairValueReserve, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.AgencyFairValue, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.DisposalsCost, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.DurationInMonths, (fake, model) => fake.Random.Int(1, 12))
                .RuleFor(asset => asset.MonthOfCompletionSinceSchemeStart,
                    (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.DisposalMonthSinceCompletion, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.IMSPaymentDate, (fake, model) => fake.Date.Soon(1, DateTime.Now))
                .RuleFor(asset => asset.IsPaid, (fake, model) => fake.Random.Bool())
                .RuleFor(asset => asset.IsAsset, (fake, model) => fake.Random.Bool())
                .RuleFor(asset => asset.PropertyType, (fake, model) => fake.PickRandom(houseType))
                .RuleFor(asset => asset.Tenure, (fake, model) => fake.PickRandom(holdTypes))
                .RuleFor(asset => asset.ExpectedStaircasingRate, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.EstimatedSalePrice, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.RegionalSaleAdjust, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.RegionalStairAdjust, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.NotLimitedByFirstCharge, (fake, model) => fake.Random.Bool())
                .RuleFor(asset => asset.EarlyMortgageIfNeverRepay, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.ArrearsEffectAppliedOrLimited,
                    (fake, model) => fake.PickRandom(appliedOrLimited))
                .RuleFor(asset => asset.RelativeSalePropertyTypeAndTenureAdjustment,
                    (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.RelativeStairPropertyTypeAndTenureAdjustment,
                    (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.IsLondon, (fake, model) => fake.Random.Bool())
                .RuleFor(asset => asset.QuarterSpend, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.MortgageProvider, (fake, model) => fake.Company.CompanyName())
                .RuleFor(asset => asset.HouseType, (fake, model) => fake.PickRandom(houseType))
                .RuleFor(asset => asset.PurchasePriceBand, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.HouseholdFiveKIncomeBand, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.HouseholdFiftyKIncomeBand, (fake, model) => fake.Finance.Amount(50, 100))
                .RuleFor(asset => asset.FirstTimeBuyer, (fake, model) => fake.Random.Bool());

            return generatedAsset;
        }
    }
}
