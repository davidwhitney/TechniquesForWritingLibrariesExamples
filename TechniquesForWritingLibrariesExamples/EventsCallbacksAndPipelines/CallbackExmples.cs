using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace TechniquesForWritingLibrariesExamples.EventsCallbacksAndPipelines
{
    [TestFixture]
    public class CallbackExmples
    {
        [Test]
        public void ClassThatCallsBack_NoCallbacks_AllOk()
        {
            var cb = new CallerBacker();

            cb.DoStuff();
        }

        [Test]
        public void ClassThatCallsBack_Callbacks_Called()
        {
            var invokedPre = false;
            var invokedPost = false;

            var cb = new CallerBacker(() => { invokedPre = true; }, () => { invokedPost = true; });

            cb.DoStuff();

            Assert.That(invokedPre, Is.True);
            Assert.That(invokedPost, Is.True);
        }
    }

    public class CallerBacker
    {
        private readonly Action _pre;
        private readonly Action _post;

        public CallerBacker(Action pre = null, Action post = null)
        {
            _pre = pre ?? (() => { });
            _post = post ?? (() => { });
        }

        public void DoStuff()
        {
            _pre();

            new[] {1, 2, 3, 4, 5}.ToList().ForEach(i => Debug.Write(i));

            _post();
        }
    }
}
