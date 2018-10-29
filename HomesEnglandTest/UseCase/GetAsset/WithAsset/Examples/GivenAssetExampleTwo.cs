using NUnit.Framework;

namespace HomesEnglandTest.UseCase.GetAsset.WithAsset.Examples
{
    [TestFixture]
    public class GivenAssetExampleTwo : GivenAsset
    {
        protected sealed override int AssetId => 42;
        protected sealed override string AssetAddress => "Scout";
        protected override string AssetSchemeID => "555";
        protected override string AssetAccountingYear => "1902";
    }
}