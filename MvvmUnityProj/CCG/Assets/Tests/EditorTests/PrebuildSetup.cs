using CCG.Tests.Editor.Core.SubstituteValueProviders;
using NSubstitute.Core;
using NSubstitute.Core.DependencyInjection;
using NSubstitute.Routing.AutoValues;
using NUnit.Framework;

namespace CCG.Tests.Editor
{
    [SetUpFixture]
    public class PrebuildSetup
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var customizedContainer = NSubstituteDefaultFactory.DefaultContainer.Customize();
            customizedContainer
                .Decorate<IAutoValueProvidersFactory>((factory, _) => new UnityObjectsValueProviderFactory(factory))
                .Decorate<IAutoValueProvidersFactory>((factory, _) => new PromiseValueProviderFactory(factory));
            SubstitutionContext.Current = customizedContainer.Resolve<ISubstitutionContext>();
        }
    }
}