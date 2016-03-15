using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain;
using DungeonGen.Generators.Domain.AreaGenerators;
using DungeonGen.Generators.Domain.ContentGenerators;
using DungeonGen.Generators.Domain.ExitGenerators;
using DungeonGen.Generators.Domain.RuntimeFactories;
using DungeonGen.Generators.Domain.RuntimeFactories.Domain;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Bootstrap.Modules
{
    [TestFixture]
    public class GeneratorModuleTests : BootstrapTests
    {
        [Test]
        public void DungeonGeneratorIsInjected()
        {
            AssertInstanceOf<IDungeonGenerator, DungeonGenerator>();
        }

        [Test]
        public void AreaGeneratorFactoryIsInjected()
        {
            AssertInstanceOf<IAreaGeneratorFactory, AreaGeneratorFactory>();
        }

        [Test]
        public void TrapGeneratorIsInjected()
        {
            AssertInstanceOf<ITrapGenerator, TrapGenerator>();
        }

        [Test]
        public void SpecialChamberGeneratorIsInjected()
        {
            AssertInstanceOf<AreaGenerator, SpecialAreaGenerator>(AreaTypeConstants.Special);
        }

        [Test]
        public void ChamberGeneratorIsInjected()
        {
            AssertInstanceOf<AreaGenerator, ChamberGenerator>(AreaTypeConstants.Chamber);
        }

        [Test]
        public void DoorGeneratorIsInjected()
        {
            AssertInstanceOf<AreaGenerator, DoorGenerator>(AreaTypeConstants.Door);
        }

        [Test]
        public void HallGeneratorIsInjected()
        {
            AssertInstanceOf<AreaGenerator, HallGenerator>(AreaTypeConstants.Hall);
        }

        [Test]
        public void RoomGeneratorIsInjected()
        {
            AssertInstanceOf<AreaGenerator, RoomGenerator>(AreaTypeConstants.Room);
        }

        [Test]
        public void SidePassageGeneratorIsInjected()
        {
            AssertInstanceOf<AreaGenerator, SidePassageGenerator>(AreaTypeConstants.SidePassage);
        }

        [Test]
        public void StairsGeneratorIsInjected()
        {
            AssertInstanceOf<AreaGenerator, StairsGenerator>(AreaTypeConstants.Stairs);
        }

        [Test]
        public void TurnGeneratorIsInjected()
        {
            AssertInstanceOf<AreaGenerator, TurnGenerator>(AreaTypeConstants.Turn);
        }

        [Test]
        public void RoomExitGeneratorIsInjected()
        {
            AssertInstanceOf<ExitGenerator, RoomExitGenerator>(AreaTypeConstants.Room);
        }

        [Test]
        public void ChamberExitGeneratorIsInjected()
        {
            AssertInstanceOf<ExitGenerator, ChamberExitGenerator>(AreaTypeConstants.Chamber);
        }

        [Test]
        public void ContentsGeneratorIsInjected()
        {
            AssertInstanceOf<ContentsGenerator, DomainContentsGenerator>();
        }

        [Test]
        public void PoolGeneratorIsInjected()
        {
            AssertInstanceOf<PoolGenerator, DomainPoolGenerator>();
        }

        [Test]
        public void CaveGeneratorIsInjected()
        {
            AssertInstanceOf<AreaGenerator, CaveGenerator>(AreaTypeConstants.Cave);
        }
    }
}
