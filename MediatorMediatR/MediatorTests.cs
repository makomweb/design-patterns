using Autofac;
using JetBrains.Annotations;
using MediatR;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatorMediatR
{
    public class PingCommand : IRequest<PongResponse>
    {
    }

    public class PongResponse
    {
        public DateTime TimeStamp { get; set; }

        public PongResponse(DateTime timestamp)
        {
            TimeStamp = timestamp;
        }
    }

    [UsedImplicitly]
    public class PingCommandHandler : IRequestHandler<PingCommand, PongResponse>
    {
        public async Task<PongResponse> Handle(PingCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new PongResponse(DateTime.UtcNow))
                .ConfigureAwait(false);
        }
    }

    public class MediatorTests : IDisposable
    {
        readonly IContainer _container;

        public MediatorTests()
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            cb.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            cb.RegisterAssemblyTypes(typeof(MediatorTests).Assembly)
                .AsImplementedInterfaces();

            _container = cb.Build();
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        [Test]
        public async Task When_request_was_sent_response_should_arrive()
        {
            var mediator = _container.Resolve<IMediator>();
            var response = await mediator.Send(new PingCommand());
            Assert.NotNull(response);
        }
    }
}