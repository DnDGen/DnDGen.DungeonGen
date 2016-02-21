using DungeonGen.Bootstrap.Modules;
using Ninject;

namespace DungeonGen.Bootstrap
{
    public class DungeonGenModuleLoader
    {
        public void LoadModules(IKernel kernel)
        {
            kernel.Load<GeneratorsModule>();
            kernel.Load<SelectorsModule>();
            kernel.Load<MappersModule>();
            kernel.Load<TablesModule>();
        }
    }
}
