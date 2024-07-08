using DnDGen.DungeonGen.Generators;
using DnDGen.DungeonGen.Generators.AreaGenerators;
using DnDGen.DungeonGen.Generators.ContentGenerators;
using DnDGen.DungeonGen.Generators.ExitGenerators;
using DnDGen.DungeonGen.Generators.Factories;
using DnDGen.DungeonGen.Models;
using Ninject.Modules;

namespace DnDGen.DungeonGen.IoC.Modules
{
    internal class GeneratorsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDungeonGenerator>().To<DungeonGenerator>();

            Bind<AreaGeneratorFactory>().To<DomainAreaGeneratorFactory>();

            Bind<ITrapGenerator>().To<TrapGenerator>();
            Bind<ContentsGenerator>().To<DomainContentsGenerator>();
            Bind<PoolGenerator>().To<DomainPoolGenerator>();

            Bind<AreaGenerator>().To<CaveGenerator>().Named(AreaTypeConstants.Cave);
            Bind<AreaGenerator>().To<ChamberGenerator>().Named(AreaTypeConstants.Chamber);
            Bind<AreaGenerator>().To<DoorGenerator>().Named(AreaTypeConstants.Door);
            Bind<AreaGenerator>().To<HallGenerator>().Named(AreaTypeConstants.Hall);
            Bind<AreaGenerator>().To<RoomGenerator>().Named(AreaTypeConstants.Room);
            Bind<AreaGenerator>().To<ParallelPassageGenerator>().Named(SidePassageConstants.ParallelPassage);
            Bind<AreaGenerator>().To<SidePassageGenerator>().Named(AreaTypeConstants.SidePassage);
            Bind<AreaGenerator>().To<SpecialAreaGenerator>().Named(AreaTypeConstants.Special);
            Bind<AreaGenerator>().To<StairsGenerator>().Named(AreaTypeConstants.Stairs);
            Bind<AreaGenerator>().To<TurnGenerator>().Named(AreaTypeConstants.Turn);

            Bind<ExitGenerator>().To<ChamberExitGenerator>().Named(AreaTypeConstants.Chamber);
            Bind<ExitGenerator>().To<RoomExitGenerator>().Named(AreaTypeConstants.Room);
        }
    }
}
