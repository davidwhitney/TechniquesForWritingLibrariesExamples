using System;
using NUnit.Framework;

namespace TechniquesForWritingLibrariesExamples.RegistrationApisAndContextObjects
{
    [TestFixture]
    public class Example
    {
        [Test]
        public void Usage()
        {
            MyLibrary.Configuration.SomeConfigPropery = "some value";
            MyLibrary.Configuration.SomethingElse = "something else";

            var instance = MyLibrary.Create();

            Assert.That(instance.PrintString(), Is.EqualTo(MyLibrary.Configuration.SomeConfigPropery));
        }
    }

    public class MyLibrary : ILibraryConfiguration
    {
        private MyLibrary() { }

        private static readonly Lazy<MyLibrary> Singleton = new Lazy<MyLibrary>(() => new MyLibrary());
        public static MyLibrary Configuration { get { return Singleton.Value; } }

        public string SomeConfigPropery { get; set; }
        public string SomethingElse { get; set; }

        public static MyLibraryCore Create()
        {
            return new MyLibraryCore(Configuration);
        }
    }

    public interface ILibraryConfiguration
    {
        string SomeConfigPropery { get; set; }
        string SomethingElse { get; set; }
    }

    public class MyLibraryCore
    {
        private readonly ILibraryConfiguration _config;

        public MyLibraryCore(ILibraryConfiguration config)
        {
            _config = config;
        }

        public string PrintString()
        {
            return _config.SomeConfigPropery;
        }
    }
}
