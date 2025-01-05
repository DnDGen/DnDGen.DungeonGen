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

        private const int TimeLimitInMilliseconds = 350;

        [TearDown]
        public void IoCTeardown()
        {
            Stopwatch.Reset();
        }

        protected void AssertSingleton<T>()
        {
            var first = InjectAndAssertDuration<T>();
            var second = InjectAndAssertDuration<T>();
            Assert.That(first, Is.EqualTo(second));
        }

        private T InjectAndAssertDuration<T>()
        {
            Stopwatch.Restart();

            var instance = GetNewInstanceOf<T>();
            Assert.That(Stopwatch.Elapsed.TotalMilliseconds, Is.LessThan(TimeLimitInMilliseconds));

            return instance;
        }

        private T InjectAndAssertDuration<T>(string name)
        {
            Stopwatch.Restart();

            var instance = GetNewInstanceOf<T>(name);
            Assert.That(Stopwatch.Elapsed.TotalMilliseconds, Is.LessThan(TimeLimitInMilliseconds));

            return instance;
        }

        protected void AssertInstanceOf<I, T>(string name)
        {
            var instance = InjectAndAssertDuration<I>(name);
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.InstanceOf<T>());
        }

        protected void AssertInstanceOf<I, T>()
        {
            var instance = InjectAndAssertDuration<I>();
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.InstanceOf<T>());
        }

        protected void AssertInstanceOf<I>()
        {
            var instance = InjectAndAssertDuration<I>();
            Assert.That(instance, Is.Not.Null);
        }
    }
}
