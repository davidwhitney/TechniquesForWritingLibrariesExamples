using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TechniquesForWritingLibrariesExamples.ReflectionBasedDiscovery
{
    [TestFixture]
    public class Example
    {
        [Test]
        public void BuildMyThing()
        {
            var instance = BuilderClassThatScansForDependencies.MakeAThing();

            Assert.That(instance.Things, Is.Not.Empty);
        }
    }

    public class BuilderClassThatScansForDependencies
    {
        public static ClassThatRequires MakeAThing()
        {
            var discovered = new List<IMyThings>();
            foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                var publicTypes = ass.GetExportedTypes();
                foreach (var type in publicTypes)
                {
                    if (type.GetInterfaces().Contains(typeof (IMyThings)))
                    {
                        discovered.Add((IMyThings)Activator.CreateInstance(type));
                    }
                }
            }

            return new ClassThatRequires(discovered);
        }
    }

    public class ClassThatRequires
    {
        public List<IMyThings> Things { get; set; }

        public ClassThatRequires(List<IMyThings> things)
        {
            Things = things;
        }

        public void DoStuff()
        {
            
        }
    }

    public interface IMyThings
    {
        void DoMoreStuff();
    }

    public class TotallyOneOfMyThings : IMyThings
    {
        public void DoMoreStuff()
        {
            
        }
    }
}
