using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.RuntimeFactories;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Bootstrap.Modules
{
    [TestFixture]
    public class GeneratorModuleTests : BootstrapTests
    {
        [Test]
        public void DungeonGeneratorIsInjected()
        {
            AssertInjected<IDungeonGenerator>();
        }

        [Test]
        public void AreaGeneratorFactoryIsInjected()
        {
            AssertInjected<IAreaGeneratorFactory>();
        }

        [Test]
        public void TrapGeneratorIsInjected()
        {
            AssertInjected<ITrapGenerator>();
        }

        [Test]
        public void SpecialChamberGeneratorIsInjected()
        {
            AssertInjected<AreaGenerator>(AreaTypeConstants.Special);
        }

        [Test]
        public void ChamberGeneratorIsInjected()
        {
            AssertInjected<AreaGenerator>(AreaTypeConstants.Chamber);
        }

        [Test]
        public void DoorGeneratorIsInjected()
        {
            AssertInjected<AreaGenerator>(AreaTypeConstants.Door);
        }

        [Test]
        public void HallGeneratorIsInjected()
        {
            AssertInjected<AreaGenerator>(AreaTypeConstants.Hall);
        }

        [Test]
        public void RoomGeneratorIsInjected()
        {
            AssertInjected<AreaGenerator>(AreaTypeConstants.Room);
        }

        [Test]
        public void SidePassageGeneratorIsInjected()
        {
            AssertInjected<AreaGenerator>(AreaTypeConstants.SidePassage);
        }

        [Test]
        public void StairsGeneratorIsInjected()
        {
            AssertInjected<AreaGenerator>(AreaTypeConstants.Stairs);
        }

        [Test]
        public void TurnGeneratorIsInjected()
        {
            AssertInjected<AreaGenerator>(AreaTypeConstants.Turn);
        }

        [Test]
        public void RoomExitGeneratorIsInjected()
        {
            AssertInjected<ExitGenerator>(AreaTypeConstants.Room);
        }

        [Test]
        public void ChamberExitGeneratorIsInjected()
        {
            AssertInjected<ExitGenerator>(AreaTypeConstants.Chamber);
        }

        [Test]
        public void ContentsGeneratorIsInjected()
        {
            AssertInjected<ContentsGenerator>();
        }
    }
}
