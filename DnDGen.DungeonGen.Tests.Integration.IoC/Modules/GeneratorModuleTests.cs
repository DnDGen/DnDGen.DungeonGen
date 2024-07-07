using DnDGen.DungeonGen.Generators;
using DnDGen.DungeonGen.Generators.AreaGenerators;
using DnDGen.DungeonGen.Generators.ContentGenerators;
using DnDGen.DungeonGen.Generators.ExitGenerators;
using DnDGen.DungeonGen.Generators.Factories;
using DnDGen.DungeonGen.Models;
using DnDGen.EncounterGen.Generators;
using DnDGen.RollGen;
using DnDGen.TreasureGen.Generators;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.IoC.Modules
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
            AssertInstanceOf<AreaGeneratorFactory, DomainAreaGeneratorFactory>();
        }

        [Test]
        public void TrapGeneratorIsInjected()
        {
            AssertInstanceOf<ITrapGenerator, TrapGenerator>();
        }

        [Test]
        public void SpecialAreaGeneratorIsInjected()
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

        [Test]
        public void EXTERNAL_DiceIsInjected()
        {
            AssertInstanceOf<Dice>();
        }

        [Test]
        public void EXTERNAL_TreasureGeneratorIsInjected()
        {
            AssertInstanceOf<ITreasureGenerator>();
        }

        [Test]
        public void EXTERNAL_EncounterGeneratorIsInjected()
        {
            AssertInstanceOf<IEncounterGenerator>();
        }
    }
}
