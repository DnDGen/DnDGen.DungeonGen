using EventGen;
using Ninject;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DungeonGen.Tests.Integration.Stress
{
    [TestFixture]
    public abstract class StressTests : IntegrationTests
    {
        [Inject]
        public Stopwatch Stopwatch { get; set; }
        [Inject]
        public ClientIDManager ClientIdManager { get; set; }
        [Inject]
        public GenEventQueue EventQueue { get; set; }

        private const int ConfidentIterations = 1000000;
        private const int TravisJobOutputTimeLimit = 60 * 10;
        private const int TravisJobBuildTimeLimit = 60 * 50;

        private readonly int timeLimitInSeconds;

        private int iterations;
        private Guid clientId;
        private DateTime eventCheckpoint;

        public StressTests()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            var methods = types.SelectMany(t => t.GetMethods());
            var unignoredMethods = methods.Where(m => !m.GetCustomAttributes<IgnoreAttribute>(true).Any());
            var stressTestsCount = unignoredMethods.Sum(m => m.GetCustomAttributes<TestAttribute>(true).Count());
            var stressTestCasesCount = unignoredMethods.Sum(m => m.GetCustomAttributes<TestCaseAttribute>().Count());
            var stressTestsTotal = stressTestsCount + stressTestCasesCount;

            var perTestTimeLimit = TravisJobBuildTimeLimit / stressTestsTotal;
            Assert.That(perTestTimeLimit, Is.AtLeast(20));

#if STRESS
            timeLimitInSeconds = Math.Min(perTestTimeLimit, TravisJobOutputTimeLimit - 10);
#else
            timeLimitInSeconds = 1;
#endif
        }

        [SetUp]
        public void StressSetup()
        {
            WriteStressIntroduction();

            clientId = Guid.NewGuid();
            ClientIdManager.SetClientID(clientId);

            iterations = 0;
            eventCheckpoint = new DateTime();

            Stopwatch.Start();
        }

        [TearDown]
        public void StressTearDown()
        {
            WriteStressSummary();
            WriteEventSummary();

            Stopwatch.Reset();
        }

        private void WriteStressIntroduction()
        {
            var duration = new TimeSpan(0, 0, timeLimitInSeconds);
            Console.WriteLine($"Stress test duration is {duration}");
        }

        private void WriteStressSummary()
        {
            var message = BuildMessage("Stress test complete");
            Console.WriteLine(message);
        }

        private void WriteEventSummary()
        {
            var events = EventQueue.DequeueAll(clientId);

            //INFO: Get the 10 most recent events for DungeonGen.  We assume the events are ordered chronologically already
            events = events.Where(e => e.Source == "DungeonGen");
            events = events.Reverse();
            events = events.Take(10);
            events = events.Reverse();

            foreach (var genEvent in events)
                Console.WriteLine(GetEventMessage(genEvent));
        }

        private string GetEventMessage(GenEvent genEvent)
        {
            return $"[{genEvent.When.ToLongTimeString()}] {genEvent.Source}: {genEvent.Message}";
        }

        protected void Stress(Action stressedAction)
        {
            do
            {
                stressedAction();
                AssertEventSpacing();
            }
            while (TestShouldKeepRunning());
        }

        private void AssertEventSpacing()
        {
            var events = EventQueue.DequeueAll(clientId);

            //INFO: Have to put the events back in the queue for the summary at the end of the test
            foreach (var genEvent in events)
                EventQueue.Enqueue(genEvent);

            Assert.That(events, Is.Ordered.By("When"));

            var newEvents = events.Where(e => e.When > eventCheckpoint).ToArray();

            Assert.That(newEvents, Is.Ordered.By("When"));

            for (var i = 1; i < newEvents.Length; i++)
            {
                var failureMessage = $"{GetEventMessage(newEvents[i - 1])}\n{GetEventMessage(newEvents[i])}";
                Assert.That(newEvents[i].When, Is.EqualTo(newEvents[i - 1].When).Within(1).Seconds, failureMessage);
            }

            if (newEvents.Any())
                eventCheckpoint = newEvents.Last().When;
        }

        private string BuildMessage(string baseMessage, bool includeIterations = true)
        {
            var message = $"{baseMessage} after {Stopwatch.Elapsed}";

            if (!includeIterations)
                return message;

            var iterationsPerSecond = Math.Round(iterations / Stopwatch.Elapsed.TotalSeconds, 2);
            return $"{message} and {iterations} iterations, or {iterationsPerSecond} iterations/second";
        }

        private bool TestShouldKeepRunning()
        {
            iterations++;
            return Stopwatch.Elapsed.TotalSeconds < timeLimitInSeconds && iterations < ConfidentIterations;
        }

        protected T Generate<T>(Func<T> generate, Func<T, bool> isValid)
        {
            T generatedObject;

            do
            {
                generatedObject = generate();
                AssertEventSpacing();
            }
            while (!isValid(generatedObject) && Stopwatch.Elapsed.TotalSeconds < timeLimitInSeconds + 10);

            if (!isValid(generatedObject))
            {
                var message = BuildMessage("Failed to generate", false);
                Assert.Fail(message);
            }

            return generatedObject;
        }

        protected T GenerateOrFail<T>(Func<T> generate, Func<T, bool> isValid)
        {
            T generatedObject;

            do
            {
                generatedObject = generate();
                AssertEventSpacing();
            }
            while (TestShouldKeepRunning() && isValid(generatedObject) == false);

            if (!isValid(generatedObject))
            {
                var message = BuildMessage("Failed to generate");
                Assert.Fail(message);
            }

            return generatedObject;
        }
    }
}
