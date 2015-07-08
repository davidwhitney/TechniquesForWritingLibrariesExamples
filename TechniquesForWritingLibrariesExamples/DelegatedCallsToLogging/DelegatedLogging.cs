using System;
using NUnit.Framework;

namespace TechniquesForWritingLibrariesExamples.DelegatedCallsToLogging
{
    [TestFixture]
    public class Example
    {
        [Test]
        public void Usage_DelegatedToFunc()
        {
            var lib = new MyLibrary()
                .OnLogMessage(m => System.Diagnostics.Debug.WriteLine(m))
                .Create();

            lib.DoStuff();
        }
    }

    public class MyLibrary
    {
        public ILogProvider LogProvider { get; set; }

        public MyLibrary()
        {
            LogProvider = new DefaultLoggingStrategy();
        }

        public MyLibrary OnLogMessage(ILogProvider strategy)
        {
            LogProvider = strategy;
            return this;
        }

        public MyLibrary OnLogMessage(Action<string> func)
        {
            LogProvider = new FuncWrapper(func);
            return this;
        }

        public MyLibraryCore Create()
        {
            return new MyLibraryCore(this);
        }
    }

    public interface ILogProvider
    {
        void Log(string message);
    }

    public class FuncWrapper : ILogProvider
    {
        private readonly Action<string> _log;

        public FuncWrapper(Action<string> log)
        {
            _log = log;
        }

        public void Log(string message)
        {
            _log(message);
        }
    }

    public class DefaultLoggingStrategy : ILogProvider
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class MyLibraryCore
    {
        private readonly MyLibrary _cfg;

        public MyLibraryCore(MyLibrary cfg)
        {
            _cfg = cfg;
        }

        public void DoStuff()
        {
            _cfg.LogProvider.Log("hi");
        }
    }
}
