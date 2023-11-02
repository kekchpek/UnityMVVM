using AsyncReactAwait.Promises;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using UnityEngine;

namespace CCG.Tests.Editor.Core
{
    public static class AutoSub
    {
        private static readonly Fixture CoreFixture;

        static AutoSub()
        {
            CoreFixture = new Fixture();
            var customization = new AutoNSubstituteCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true,
            };
            customization.Customize(CoreFixture);
            CoreFixture.Inject(PromiseTool.GetSuccessful());
            CoreFixture.Inject(Texture2D.blackTexture);
        }
        
        public static T For<T>() => CoreFixture.Create<T>();
    }
}