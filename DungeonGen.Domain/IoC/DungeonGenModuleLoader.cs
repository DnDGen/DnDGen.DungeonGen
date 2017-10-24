using DungeonGen.Domain.IoC.Modules;
using Ninject;

namespace DungeonGen.Domain.IoC
{
    public class DungeonGenModuleLoader
    {
        public void LoadModules(IKernel kernel)
        {
            kernel.Load<GeneratorsModule>();
            kernel.Load<SelectorsModule>();
        }
    }
}
