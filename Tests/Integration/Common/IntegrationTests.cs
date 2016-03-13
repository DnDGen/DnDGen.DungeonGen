using CharacterGen.Bootstrap;
using DungeonGen.Bootstrap;
using EncounterGen.Bootstrap;
using Ninject;
using NUnit.Framework;
using RollGen.Bootstrap;
using TreasureGen.Bootstrap;

namespace DungeonGen.Tests.Integration.Common
{
    [TestFixture]
    public class IntegrationTests
    {
        private IKernel kernel;

        [OneTimeSetUp]
        public void IntegrationTestsFixtureSetup()
        {
            kernel = new StandardKernel();

            var diceLoader = new RollGenModuleLoader();
            diceLoader.LoadModules(kernel);

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
