using HomesEngland.UseCase.GenerateAssets;
using Main;

namespace HomesEngland.Gateway.DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var assetRegister = new AssetRegister();
            IConsoleGenerator generator = assetRegister.Get<IConsoleGenerator>();
            generator.ProcessAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
