using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using HomesEngland.UseCase.BulkCreateAsset;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.GenerateAssets.Models;

namespace HomesEngland.UseCase.GenerateAssets.Impl
{
    public class GenerateAssetsUseCase : IGenerateAssetsUseCase
    {
        private readonly IBulkCreateAssetUseCase _bulkCreateAssetUseCase;

        public GenerateAssetsUseCase(IBulkCreateAssetUseCase bulkCreateAssetUseCase)
        {
            _bulkCreateAssetUseCase = bulkCreateAssetUseCase;
        }

        public async Task<GenerateAssetsResponse> ExecuteAsync(GenerateAssetsRequest requests, CancellationToken cancellationToken)
        {
            IList<CreateAssetRequest> createAssetRequests = GenerateCreateAssetRequest(requests.Records.Value);

            var response = await _bulkCreateAssetUseCase.ExecuteAsync(createAssetRequests, cancellationToken)
                .ConfigureAwait(false);

            var generateAssetsResponse = new GenerateAssetsResponse
            {
                RecordsGenerated = response.Select(s => s.Asset).ToList()
            };
            return generateAssetsResponse;
        }

        private IList<CreateAssetRequest> GenerateCreateAssetRequest(int count)
        {
            var random = new Random(0);

            var faker = new Faker("en");
            var completionDateForHpiStart = faker.Date.Soon(random.Next(1, 15));
            var imsActualCompletionDate = faker.Date.Soon(random.Next(30, 90));
            var imsExpectedCompletionDate = faker.Date.Soon(random.Next(15, 90));
            var hopCompletionDate = faker.Date.Soon(random.Next(15, 90));
            var differenceFromImsExpectedCompletionToHopCompletionDate =
                (imsExpectedCompletionDate.Date - hopCompletionDate).Days;


            var appliedOrLimited = new List<string> { "Applied", "Limited" };
            var houseType = new List<string> { "Semi-Detached", "Detached" };
            var holdTypes = new List<string> { "Freehold", "Leasehold" };

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
                .RuleFor(asset => asset.PropertyPostcode, (fake, model) => RandomUkPostCode())
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


            var list = new List<CreateAssetRequest>();
            for (int i = 0; i < count; i++)
            {
                list.Add(generatedAsset.Generate());
            }
            return list;
        }

        private List<string> _postCodes = new List<string>
            {
                "AB1 0UD",
                "AB1 0UE",
                "AB1 0UJ",
                "AB1 0UL",
                "AB1 0UN",
                "AB1 0UP",
                "AB1 0UQ",
                "AB1 0UR",
                "AB1 0US",
                "AB1 0UT",
                "AB1 0UX",
                "AB1 0WA",
                "AB1 0WB",
                "AB1 0WD",
                "AB1 0WE",
                "AB1 0WF",
                "AB1 0WG",
                "AB1 0WH",
                "AB1 0WN",
                "AB1 0XA",
                "AB1 0XB",
                "AB1 0XD",
                "AB1 0XE",
                "AB1 0XJ",
                "AB1 0XL",
                "AB1 0XN",
                "AB1 0XP",
                "AB1 0XQ",
                "AB1 0XR",
                "AB1 0XU",
                "AB1 0YA",
                "CV8 1LL",
                "CV8 1LN",
                "CV8 1LP",
                "CV8 1LQ",
                "CV8 1LR",
                "CV8 1LS",
                "CV8 1LT",
                "CV8 1LU",
                "CV8 1LW",
                "CV8 1LX",
                "CV8 1LY",
                "CV8 1LZ",
                "CV8 1NA",
                "CV8 1NB",
                "CV8 1ND",
                "CV8 1NE",
                "CV8 1NF",
                "CV8 1NG",
                "CV8 1NH",
                "CV8 1NJ",
                "CV8 1NL",
                "CV8 1NN",
                "CV8 1NP",
                "CV8 1NQ",
                "CV8 1NR",
                "CV8 1NS",
                "CV8 1NT",
                "CV8 1NU",
                "CV8 1NW",
                "CV8 1NX",
                "CW1 3DE",
                "CW1 3DF",
                "CW1 3DG",
                "CW1 3DH",
                "CW1 3DJ",
                "CW1 3DL",
                "CW1 3DN",
                "CW1 3DP",
                "CW1 3DQ",
                "CW1 3DR",
                "CW1 3DS",
                "CW1 3DT",
                "E6 1RX",
                "E6 1RY",
                "E6 1RZ",
                "E6 1SA",
                "E6 1SB",
                "E6 1SD",
                "E6 1SE",
                "E6 1SF",
                "E6 1SG",
                "E6 1SH",
                "E6 1SJ",
                "E6 1SL",
                "E6 1SN",
                "G72 7XE",
                "G72 7XF",
                "G72 7XG",
                "G72 7XH",
                "G72 7XJ",
                "G72 7XL",
                "G72 7XN",
                "G72 7XP",
                "G72 7XQ",
                "GL12 8TX",
                "GL12 8TY",
                "GL12 8TZ",
                "GL12 8UA",
                "GL12 8UB",
                "GL12 8UD",
                "GL12 8UE",
                "GL12 8UF",
                "GL12 8UG",
                "GL12 8UH",
                "GL12 8UJ",
                "GL12 8UL",
                "GL12 8UN",
                "GL12 8UP",
                "GL12 8UQ",
                "GL12 8UR",
                "GL12 8US",
                "GL12 8UT",
                "GL12 8UU",
                "GL4 6BD",
                "GL4 6BE",
                "GL4 6BG",
                "GL4 6BH",
                "GL4 6BJ",
                "GL4 6BL",
                "GL4 6BN",
                "GL4 6BP",
                "GL4 6BQ",
                "GL4 6BS",
                "GL4 6BT",
                "GL4 6BU",
                "GL4 6BW",
                "GL4 6BX",
                "GL4 6DA",
                "GL4 6DG",
                "GL4 6DT",
                "HX1 4QN",
                "HX1 4QP",
                "HX1 4QQ",
                "HX1 4QR",
                "HX1 4QS",
                "HX1 4QT",
                "HX1 4QU",
                "HX1 4QW",
                "HX1 4QX",
                "HX1 4QY",
                "HX1 4QZ",
                "HX1 4RA",
                "HX1 4RB",
                "HX1 4RD",
                "HX1 4RE",
                "HX1 4RF",
                "HX1 4RG",
                "ME5 9FB",
                "ME5 9FD",
                "ME5 9FE",
                "ME5 9FF",
                "ME5 9FZ",
                "PO4 8FW",
                "PO4 8GA",
                "PO4 8GB",
                "PO4 8GD",
                "PO4 8GE",
                "PO4 8GR",
                "PO4 8GS",
                "SP1 2PH",
                "SP1 2PJ",
                "SP1 2PL",
                "SP1 2PN",
                "SP1 2PP",
                "SP1 2PQ",
                "SP1 2PR",
                "SP1 2PS",
                "SP1 2PT",
                "SP1 2PU",
                "SP1 2PW",
                "SP1 2PX",
                "TD5 7TZ",
                "TD5 7UA",
                "TD5 7UB",
                "TD5 7UD",
                "TD5 7UE",
                "TD5 7UF",
                "TF3 2JU",
                "TF3 2JW",
                "TF3 2JX",
                "TF3 2JY",
                "TF3 2JZ",
                "TF3 2LA",
                "TF3 2LB",
                "TF3 2LD",
                "TF3 2LE",
                "TF3 2LF",
                "TF3 2LG",
                "TF3 2LH",
                "W1U 7NA",
                "W1U 7NB",
                "W1U 7ND",
                "W1U 7NE",
                "W1U 7NF",
                "W1U 7NG",
                "W1U 7NH",
                "W1U 7NN",
                "W1U 7NP",
                "WS13 6JH",
                "WS13 6JJ",
                "WS13 6JL",
                "WS13 6JN",
                "WS13 6JP",
                "WS13 6JQ",
                "WS13 6JR",
                "WS13 6JS",
                "WS13 6JT",
                "WS13 6JU",
                "WS13 6JW", 
            };

        public string RandomUkPostCode()
        {
            var seed = new Random(DateTime.UtcNow.Millisecond);
            var index = seed.Next(0, _postCodes.Count - 1);
            var postCode = _postCodes.ElementAt(index);
            return postCode;
        }
    }
}
