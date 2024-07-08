using DnDGen.DungeonGen.IoC.Modules;
using DnDGen.EncounterGen.IoC;
using DnDGen.Infrastructure.IoC;
using DnDGen.RollGen.IoC;
using DnDGen.TreasureGen.IoC;
using Ninject;
using System.Linq;

namespace DnDGen.DungeonGen.IoC
{
    public class DungeonGenModuleLoader
    {
        public void LoadModules(IKernel kernel)
        {
            //Dependencies
            var rollGenLoader = new RollGenModuleLoader();
            rollGenLoader.LoadModules(kernel);

            var infrastructureLoader = new InfrastructureModuleLoader();
            infrastructureLoader.LoadModules(kernel);

            var treasureGenLoader = new TreasureGenModuleLoader();
            treasureGenLoader.LoadModules(kernel);

            var encounterGenLoader = new EncounterGenModuleLoader();
            encounterGenLoader.LoadModules(kernel);

            //DungeonGen
            var modules = kernel.GetModules();

            if (!modules.Any(m => m is GeneratorsModule))
                kernel.Load<GeneratorsModule>();

            if (!modules.Any(m => m is SelectorsModule))
                kernel.Load<SelectorsModule>();
        }
    }
}
