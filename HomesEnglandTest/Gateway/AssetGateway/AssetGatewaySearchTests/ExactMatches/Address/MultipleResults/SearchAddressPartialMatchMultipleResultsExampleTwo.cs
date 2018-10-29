using HomesEngland.Domain;
using HomesEnglandTest.Gateway.AssetGateway.InMemoryGateway;

namespace HomesEnglandTest.Gateway.AssetGateway.AssetGatewaySearchTests.ExactMatches.Address.MultipleResults
{
    public class SearchAddressPartialMatchMultipleResultsExampleTwo:InMemoryAssetGatewaySearchTest
    {
        protected override string SearchQuery => "Clang";

        protected override Asset[] AssetsInGateway => new[]
        {
            new Asset()
            {
                Address = "Cow", 
                SchemeID = "22",
                AccountingYear = "1982"
            },
            new Asset()
            {
                Address = "Clanger", 
                SchemeID = "12355",
                AccountingYear = "665"
            },
            new Asset()
            {
                Address = "Clanger", 
                SchemeID = "67",
                AccountingYear = "0"
            },
            new Asset()
            {
                Address = "Clanger", 
                SchemeID = "12345",
                AccountingYear = "61165"
            },
            new Asset()
            {
                Address = "Clanger", 
                SchemeID = "2345",
                AccountingYear = "1234"
            } 
        };
        
        protected override Asset[] ExpectedGatewaySearchResults => new[]
        {
            new Asset()
            {
                Address = "Clanger", 
                SchemeID = "12355",
                AccountingYear = "665"
            },
            new Asset()
            {
                Address = "Clanger", 
                SchemeID = "67",
                AccountingYear = "0"
            },
            new Asset()
            {
                Address = "Clanger", 
                SchemeID = "12345",
                AccountingYear = "61165"
            },
            new Asset()
            {
                Address = "Clanger", 
                SchemeID = "2345",
                AccountingYear = "1234"
            }  
        };
    }
}