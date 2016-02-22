using DungeonGen.Tables;
using DungeonGen.Tables.Domain;
using Ninject.Modules;

namespace DungeonGen.Bootstrap.Modules
{
    public class TablesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<StreamLoader>().To<EmbeddedResourceStreamLoader>();
        }
    }
}
