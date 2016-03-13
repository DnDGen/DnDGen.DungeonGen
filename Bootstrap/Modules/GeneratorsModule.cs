using DungeonGen.Bootstrap.Factories;
using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain;
using DungeonGen.Generators.Domain.AreaGenerators;
using DungeonGen.Generators.Domain.ExitGenerators;
using DungeonGen.Generators.Domain.RuntimeFactories;
using Ninject.Modules;

namespace DungeonGen.Bootstrap.Modules
{
    public class GeneratorsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDungeonGenerator>().To<DungeonGenerator>();
            Bind<IAreaGeneratorFactory>().ToMethod(c => AreaGeneratorFactoryFactory.Build(c.Kernel));
            Bind<ITrapGenerator>().To<TrapGenerator>();
            Bind<AreaGenerator>().To<SpecialAreaGenerator>().Named(AreaTypeConstants.Special);
            Bind<AreaGenerator>().To<HallGenerator>().Named(AreaTypeConstants.Hall);
            Bind<ExitGenerator>().To<ChamberExitGenerator>().Named(AreaTypeConstants.Chamber);
            Bind<ExitGenerator>().To<RoomExitGenerator>().Named(AreaTypeConstants.Room);
            Bind<ContentsGenerator>().To<DomainContentsGenerator>();
        }
    }
}
