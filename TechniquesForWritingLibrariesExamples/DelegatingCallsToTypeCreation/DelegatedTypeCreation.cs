using System;
using NUnit.Framework;

namespace TechniquesForWritingLibrariesExamples.DelegatingCallsToTypeCreation
{
    [TestFixture]
    public class Example
    {
        [Test]
        public void Usage_DelegatedToFunc()
        {
            var config = new MyLibrary().BindToContainer(Activator.CreateInstance);

            var instance = config.Create();

            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void Usage_ConcreteImplementation()
        {
            var config = new MyLibrary().BindToContainer(new ActivatorCreationStrategy());

            var instance = config.Create();

            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void Usage_DefaultStrategy()
        {
            var config = new MyLibrary();

            var instance = config.Create();

            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void UsageFailure_StrategyNulled()
        {
            var config = new MyLibrary {TypeCreationStrategy = null};

            Assert.Throws<NullReferenceException>(() => config.Create());
        }
    }

    public class MyLibrary
    {
        public ITypeCreationStrategy TypeCreationStrategy { get; set; }

        public MyLibrary()
        {
            TypeCreationStrategy = new TypeCreationFuncShim(Activator.CreateInstance); // Default to activator
        }

        public MyLibrary BindToContainer(ITypeCreationStrategy strategy)
        {
            TypeCreationStrategy = strategy;
            return this;
        }

        public MyLibrary BindToContainer(Func<Type, object> func)
        {
            TypeCreationStrategy = new TypeCreationFuncShim(func);
            return this;
        }

        public MyLibraryCore Create()
        {
            return (MyLibraryCore)TypeCreationStrategy.Create(typeof(MyLibraryCore));
        }
    }

    public interface ITypeCreationStrategy
    {
        object Create(Type type);
    }

    public class TypeCreationFuncShim : ITypeCreationStrategy
    {
        private readonly Func<Type, object> _create;

        public TypeCreationFuncShim(Func<Type, object> create)
        {
            _create = create;
        }

        public object Create(Type type)
        {
            return _create(type);
        }
    }

    public class ActivatorCreationStrategy : ITypeCreationStrategy
    {
        public object Create(Type type)
        {
            return Activator.CreateInstance(typeof(MyLibraryCore));
        }
    }

    public class MyLibraryCore
    {
    }
}
