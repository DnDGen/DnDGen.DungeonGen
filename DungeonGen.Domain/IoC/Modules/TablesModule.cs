using DungeonGen.Domain.Tables;
using Ninject.Modules;

namespace DungeonGen.Domain.IoC.Modules
{
    internal class TablesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<StreamLoader>().To<EmbeddedResourceStreamLoader>();
        }
    }
}
