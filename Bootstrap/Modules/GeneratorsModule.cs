using DungeonGen.Generators;
using DungeonGen.Generators.Domain;
using Ninject.Modules;

namespace DungeonGen.Bootstrap.Modules
{
    public class GeneratorsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDungeonGenerator>().To<DungeonGenerator>();
        }
    }
}
