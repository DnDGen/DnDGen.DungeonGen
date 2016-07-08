using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Generators.ContentGenerators;
using DungeonGen.Domain.Generators.ExitGenerators;
using DungeonGen.Domain.Generators.RuntimeFactories;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.IoC.Modules
{
    [TestFixture]
    public class GeneratorModuleTests : IoCTests
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

        [Test]
        public void ParallelPassageGeneratorIsInjected()
        {
            AssertInstanceOf<AreaGenerator, ParallelPassageGenerator>(SidePassageConstants.ParallelPassage);
        }
    }
}
