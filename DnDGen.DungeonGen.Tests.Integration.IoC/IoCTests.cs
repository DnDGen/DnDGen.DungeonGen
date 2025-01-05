using Ninject;
using NUnit.Framework;
using System.Diagnostics;

namespace DnDGen.DungeonGen.Tests.Integration.IoC
{
    [TestFixture]
    public abstract class IoCTests : IntegrationTests
    {
        [Inject]
        public Stopwatch Stopwatch { get; set; }

        private const int TimeLimitInMilliseconds = 200;

        [TearDown]
        public void IoCTeardown()
        {
            Stopwatch.Reset();
        }

        protected void AssertSingleton<T>(int timeLimit = TimeLimitInMilliseconds)
        {
            var first = InjectAndAssertDuration<T>(timeLimit);
            var second = InjectAndAssertDuration<T>(timeLimit);
            Assert.That(first, Is.EqualTo(second));
        }

        private T InjectAndAssertDuration<T>(int timeLimit = TimeLimitInMilliseconds)
        {
            Stopwatch.Restart();

            var instance = GetNewInstanceOf<T>();
            Assert.That(Stopwatch.Elapsed.TotalMilliseconds, Is.LessThan(timeLimit));

            return instance;
        }

        private T InjectAndAssertDuration<T>(string name, int timeLimit = TimeLimitInMilliseconds)
        {
            Stopwatch.Restart();

            var instance = GetNewInstanceOf<T>(name);
            Assert.That(Stopwatch.Elapsed.TotalMilliseconds, Is.LessThan(timeLimit));

            return instance;
        }

        protected void AssertInstanceOf<I, T>(string name, int timeLimit = TimeLimitInMilliseconds)
        {
            var instance = InjectAndAssertDuration<I>(name, timeLimit);
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.InstanceOf<T>());
        }

        protected void AssertInstanceOf<I, T>(int timeLimit = TimeLimitInMilliseconds)
        {
            var instance = InjectAndAssertDuration<I>(timeLimit);
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.InstanceOf<T>());
        }

        protected void AssertInstanceOf<I>(int timeLimit = TimeLimitInMilliseconds)
        {
            var instance = InjectAndAssertDuration<I>(timeLimit);
            Assert.That(instance, Is.Not.Null);
        }
    }
}
