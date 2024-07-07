using DnDGen.DungeonGen.Selectors;
using Ninject.Modules;

namespace DnDGen.DungeonGen.IoC.Modules
{
    internal class SelectorsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAreaPercentileSelector>().To<AreaPercentileSelector>();
        }
    }
}
