using DnDGen.Stress;
using DnDGen.Stress.Events;
using EventGen;
using NUnit.Framework;
using System.Reflection;

namespace DungeonGen.Tests.Integration.Stress
{
    [TestFixture]
    public abstract class StressTests : IntegrationTests
    {
        protected Stressor stressor;

        [OneTimeSetUp]
        public void StressSetup()
        {
            var options = new StressorWithEventsOptions();
            options.RunningAssembly = Assembly.GetExecutingAssembly();

#if STRESS
            options.IsFullStress = true;
#else
            options.IsFullStress = false;
#endif

            options.ClientIdManager = GetNewInstanceOf<ClientIDManager>();
            options.EventQueue = GetNewInstanceOf<GenEventQueue>();
            options.Source = "DungeonGen";

            stressor = new StressorWithEvents(options);
        }
    }
}
