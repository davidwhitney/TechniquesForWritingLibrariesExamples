using System;
using NUnit.Framework;

namespace TechniquesForWritingLibrariesExamples.RegistrationApisAndContextObjects
{
    [TestFixture]
    public class Usage2
    {
        [Test]
        public void BootstrappingYourLibrariesWithFluentApis()
        {
            // To make my library, I can instantiate everything independantly
            var firstD1 = new Dependency();
            var firstD2 = new Dependency2 { SomeConfigurationProperty = "aa", SomeOptionalConfigurationProperty = "bb" };
            var firstLibraryInstantiation = new MyLibraryEntryPoint(firstD1, firstD2);

            // Or, you can offer a discoverable entry point and build a fluent interface
            var configured =
                MyFluentlyConfiguredLib.SetUp().With(
                    x =>
                    {
                        x.SomeConfigurationProperty = "my setting";
                    })
                    .AndSomeOptionalThing(x => { })
                    .AndSomeOptionalThing(x => { })
                    .AndSomeRequiredFinalThing(x => { });

            var secondInstance = configured.Create();
        }
    }

    public static class MyFluentlyConfiguredLib
    {
        private static MyLibraryBuilder _builder;

        public static MyLibraryBuilder SetUp()
        {
            _builder = new MyLibraryBuilder();
            return _builder;
        }

        public class MyLibraryBuilder : IRequireFurtherConfiguration, IAllowCreation
        {
            public string SomeConfigurationProperty { get; set; }
            public string SomeOptionalConfigurationProperty { get; set; }

            public IRequireFurtherConfiguration With(Action<MyLibraryBuilder> withConfiguration)
            {
                withConfiguration(this);
                return this;
            }

            public IRequireFurtherConfiguration AndSomeOptionalThing(Action<MyLibraryBuilder> withConfiguration)
            {
                withConfiguration(this);
                return this;
            }

            public IAllowCreation AndSomeRequiredFinalThing(Action<MyLibraryBuilder> withConfiguration)
            {
                withConfiguration(this);
                return this;
            }

            public MyLibraryEntryPoint Create()
            {
                return new MyLibraryEntryPoint(new Dependency(),
                    new Dependency2
                    {
                        SomeConfigurationProperty = SomeConfigurationProperty,
                        SomeOptionalConfigurationProperty = SomeOptionalConfigurationProperty
                    });
            }
        }

        public interface IRequireFurtherConfiguration
        {
            IRequireFurtherConfiguration AndSomeOptionalThing(Action<MyLibraryBuilder> withConfiguration);
            IAllowCreation AndSomeRequiredFinalThing(Action<MyLibraryBuilder> withConfiguration);
        }

        public interface IAllowCreation
        {
            MyLibraryEntryPoint Create();
        }

    }

    public class MyLibraryEntryPoint
    {
        private readonly Dependency _d1;
        private readonly Dependency2 _d2;

        public MyLibraryEntryPoint(Dependency d1, Dependency2 d2)
        {
            _d1 = d1;
            _d2 = d2;
        }
    }

    public class Dependency2
    {
        public string SomeConfigurationProperty { get; set; }
        public string SomeOptionalConfigurationProperty { get; set; }
    }

    public class Dependency
    {
    }
}
