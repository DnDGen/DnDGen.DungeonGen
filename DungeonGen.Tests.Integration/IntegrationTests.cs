using CharacterGen.Domain.IoC;
using DungeonGen.Domain.IoC;
using EncounterGen.Domain.IoC;
using EventGen.IoC;
using Ninject;
using NUnit.Framework;
using RollGen.Domain.IoC;
using TreasureGen.Domain.IoC;

namespace DungeonGen.Tests.Integration
{
    [TestFixture]
    public class IntegrationTests
    {
        private IKernel kernel;

        [OneTimeSetUp]
        public void IntegrationTestsFixtureSetup()
        {
            kernel = new StandardKernel(new NinjectSettings() { InjectNonPublic = true });

            var diceLoader = new RollGenModuleLoader();
            diceLoader.LoadModules(kernel);

            var eventGenLoader = new EventGenModuleLoader();
            eventGenLoader.LoadModules(kernel);

            var treasureGenLoader = new TreasureGenModuleLoader();
            treasureGenLoader.LoadModules(kernel);

            var characterGenLoader = new CharacterGenModuleLoader();
            characterGenLoader.LoadModules(kernel);

            var encounterGenLoader = new EncounterGenModuleLoader();
            encounterGenLoader.LoadModules(kernel);

            var dungeonGenLoader = new DungeonGenModuleLoader();
            dungeonGenLoader.LoadModules(kernel);
        }

        [SetUp]
        public void IntegrationTestsSetup()
        {
            kernel.Inject(this);
        }

        protected T GetNewInstanceOf<T>()
        {
            return kernel.Get<T>();
        }

        protected T GetNewInstanceOf<T>(string name)
        {
            return kernel.Get<T>(name);
        }
    }
}
