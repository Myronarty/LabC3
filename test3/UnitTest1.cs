using NUnit.Framework;
using System.Threading;
using test3;

namespace test3
{
    public class Tests
    {
        private ComputationController engine;
        private int stepCount;

        [SetUp]
        public void Setup()
        {
            stepCount = 0;
            engine = new ComputationController();
            engine.OnStep += (_) => stepCount++;
        }

        [Test]
        public void Computation_Runs_When_NotPaused()
        {
            engine.Start();
            Thread.Sleep(300);
            engine.Stop();

            Assert.That(stepCount, Is.GreaterThan(0));
        }

        [Test]
        public void Computation_Pauses_And_Resumes()
        {
            engine.Start();
            Thread.Sleep(200);

            engine.TogglePause();
            int pausedCount = stepCount;

            Thread.Sleep(300);
            Assert.That(stepCount, Is.EqualTo(pausedCount));

            engine.TogglePause();
            Thread.Sleep(200);
            engine.Stop();

            Assert.That(stepCount, Is.GreaterThan(pausedCount));
        }
    }
}
