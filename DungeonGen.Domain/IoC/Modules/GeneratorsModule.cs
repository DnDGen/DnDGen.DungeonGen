using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Generators.ContentGenerators;
using DungeonGen.Domain.Generators.RuntimeFactories;
using DungeonGen.Domain.IoC.Providers;
using Ninject.Modules;

namespace DungeonGen.Domain.IoC.Modules
{
    internal class GeneratorsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDungeonGenerator>().To<DungeonGenerator>();
            Bind<IAreaGeneratorFactory>().ToProvider<AreaGeneratorFactoryProvider>();
            Bind<ITrapGenerator>().To<TrapGenerator>();
            Bind<ContentsGenerator>().To<DomainContentsGenerator>();
            Bind<PoolGenerator>().To<DomainPoolGenerator>();

            Bind<AreaGenerator>().To<CaveGenerator>().Named(AreaTypeConstants.Cave);
            Bind<AreaGenerator>().ToProvider<ChamberGeneratorProvider>().Named(AreaTypeConstants.Chamber);
            Bind<AreaGenerator>().To<DoorGenerator>().Named(AreaTypeConstants.Door);
            Bind<AreaGenerator>().To<HallGenerator>().Named(AreaTypeConstants.Hall);
            Bind<AreaGenerator>().ToProvider<RoomGeneratorProvider>().Named(AreaTypeConstants.Room);
            Bind<AreaGenerator>().ToProvider<SidePassageGeneratorProvider>().Named(AreaTypeConstants.SidePassage);
            Bind<AreaGenerator>().ToProvider<SpecialAreaGeneratorProvider>().Named(AreaTypeConstants.Special);
            Bind<AreaGenerator>().ToProvider<StairsGeneratorProvider>().Named(AreaTypeConstants.Stairs);
            Bind<AreaGenerator>().To<TurnGenerator>().Named(AreaTypeConstants.Turn);

            Bind<ExitGenerator>().ToProvider<ChamberExitGeneratorProvider>().Named(AreaTypeConstants.Chamber);
            Bind<ExitGenerator>().ToProvider<RoomExitGeneratorProvider>().Named(AreaTypeConstants.Room);
        }
    }
}
