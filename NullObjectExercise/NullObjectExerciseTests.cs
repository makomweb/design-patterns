using NUnit.Framework;
using System;

namespace NullObjectExercise
{
    public interface ILog
    {
        // maximum # of elements in the log
        int RecordLimit { get; }

        // number of elements already in the log
        int RecordCount { get; set; }

        // expected to increment RecordCount
        void LogInfo(string message);
    }

    public class Account
    {
        private readonly ILog _log;

        public Account(ILog log)
        {
            this._log = log;
        }

        public void SomeOperation()
        {
            int c = _log.RecordCount;
            _log.LogInfo("Performing an operation");
            if (c + 1 != _log.RecordCount)
                throw new Exception($"c+1 is {c+1} - RecordCount is {_log.RecordCount}");
            if (_log.RecordCount >= _log.RecordLimit)
                throw new Exception($"Record count is {_log.RecordCount} - limit is {_log.RecordLimit}");
        }
    }

    public class NullLog : ILog
    {
        public int RecordLimit { get; set; } = 100;
        public int RecordCount { get; set; } = 0;

        public void LogInfo(string message)
        {
            RecordCount++;
            RecordLimit = RecordCount + 1;
        }
    }

    public class NullObjectExerciseTests
    {
        [Test]
        public void Test_that_it_does_not_throw()
        {
            var log = new NullLog();
            var acc = new Account(log);

            for( var i = 0; i < 1000; ++i)
            {
                acc.SomeOperation();
            }
        }
    }
}