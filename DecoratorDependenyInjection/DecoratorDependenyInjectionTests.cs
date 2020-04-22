using Autofac;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace DecoratorDependenyInjection
{
    public interface IReportingService
    {
        void Report(); 
    }

    public class ReportingService : IReportingService
    {
        public void Report()
        {
            Debug.WriteLine("Here is your report!");
        }
    }

    public class ReportingServiceWithLogging : IReportingService
    {
        private readonly IReportingService _decorated;

        public ReportingServiceWithLogging(IReportingService decorated)
        {
            _decorated = decorated;
        }

        public void Report()
        {
            Debug.WriteLine("Commencing log.");

            _decorated.Report();

            Debug.WriteLine("Ending log.");
        }
    }

    public class DecoratorDependenyInjectionTests
    {
        [Test]
        public void Test1()
        {
            var b = new ContainerBuilder();
            //b.RegisterType<ReportingServiceWithLogging>().As<IReportingService>(); // INFITIY LOOP!
            b.RegisterType<ReportingService>().Named<IReportingService>("reporting");
            b.RegisterDecorator<IReportingService>(
                (context, service) => new ReportingServiceWithLogging(service), "reporting");


            using (var c = b.Build())
            {
                var r = c.Resolve<IReportingService>();
                r.Report();

                Assert.Pass();
            }
        }
    }
}