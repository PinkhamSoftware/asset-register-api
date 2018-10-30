using HomesEngland.Domain;
using NUnit.Framework;

namespace WebApiTest.Controller.GetAssets.Examples
{
    [TestFixture]
    public class GetAssetsControllerTestExampleOne:GetAssetsControllerTest
    {
        protected override Asset[] Assets => new[]{
            GetAsset("2, Horse Road, Horse Town", "1234","1998"), 
            GetAsset("2, Cow Road, Cow Town", "12455667","1234") };
        protected override int[] AssetIds  => new[]{1,2};
    }
}