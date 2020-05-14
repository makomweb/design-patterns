using NUnit.Framework;
using Stateless;

namespace StateStateless
{
    public enum Health
    {
        NonReproductive,
        Pregnant,
        Reproductive
    }

    public enum Activity
    {
        GiveBirth,
        ReachPurberty,
        HaveAbortion,
        HaveUnprotectedSex,
        Historectomy
    }

    public class StateTests
    {
        [Test]
        public void Test_activity_path_for_reaching_puberty()
        {
            var stateMachine = new StateMachine<Health, Activity>(Health.NonReproductive);

            stateMachine.Configure(Health.NonReproductive)
                .Permit(Activity.ReachPurberty, Health.Reproductive);

            stateMachine.Configure(Health.Reproductive)
                .Permit(Activity.Historectomy, Health.NonReproductive)
                .PermitIf(Activity.HaveUnprotectedSex, Health.Pregnant,
                  () => ParentsNotWatching);

            stateMachine.Configure(Health.Pregnant)
                .Permit(Activity.GiveBirth, Health.Reproductive)
                .Permit(Activity.HaveAbortion, Health.Reproductive);

            stateMachine.Fire(Activity.ReachPurberty);

            Assert.That(stateMachine.State, Is.EqualTo(Health.Reproductive));
        }

        public static bool ParentsNotWatching
        {
            get; set;
        }
    }
}