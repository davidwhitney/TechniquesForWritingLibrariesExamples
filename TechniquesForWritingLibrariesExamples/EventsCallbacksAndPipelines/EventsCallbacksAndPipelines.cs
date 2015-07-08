using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace TechniquesForWritingLibrariesExamples.EventsCallbacksAndPipelines
{
    [TestFixture]
    public class Example
    {
        [Test]
        public void ClassWithExtensibileNotifiers_NoNotifiersInvoked()
        {
            var impl = new MyImplementation();

            impl.DoSomething();

            // Doesn't write summary
        }

        [Test]
        public void ClassWithExtensibileNotifiers_InvokesSuppliedOnes()
        {
            var impl = new MyImplementation(new[] {new WriteItOut(), new WriteItOut()});

            impl.DoSomething();

            // writes to debug at the end.
        }
    }

    public class MyImplementation
    {
        private readonly List<IGetCalledOnCompletion> _onComplete;

        public MyImplementation(IEnumerable<IGetCalledOnCompletion> onComplete = null)
        {
            _onComplete = new List<IGetCalledOnCompletion>(onComplete ?? new List<IGetCalledOnCompletion>());
        }

        public void DoSomething()
        {
            int i;
            for (i = 0; i < 100; i++)
            {
                Debug.WriteLine("Counting " + i);
            }

            _onComplete.ForEach(x => x.DoSomethingWithCount(i));
        }
    }

    public interface IGetCalledOnCompletion
    {
        void DoSomethingWithCount(int i);
    }

    public class WriteItOut : IGetCalledOnCompletion
    {
        public void DoSomethingWithCount(int i)
        {
            Debug.WriteLine(i);
        }
    }
}
