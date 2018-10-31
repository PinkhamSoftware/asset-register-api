using NUnit.Framework;

namespace WebApiTest.Controller.GetAsset.Examples
{
    [TestFixture]
    public class GetAssetControllerTestExampleOne:GetAssetControllerTest
    {
        protected override int AssetId => 24;
        protected override string AssetAddress => "The Cavern Club, Liverpool";
        protected override string AssetSchemeID => "3333221";
        protected override string AssetAccountingYear => "1988";
    }
}