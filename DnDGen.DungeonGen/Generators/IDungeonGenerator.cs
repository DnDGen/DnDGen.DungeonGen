using DnDGen.DungeonGen.Models;
using DnDGen.EncounterGen.Generators;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DnDGen.DungeonGen.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("DnDGen.DungeonGen.Tests.Integration")]
[assembly: InternalsVisibleTo("DnDGen.DungeonGen.Tests.Integration.IoC")]
[assembly: InternalsVisibleTo("DnDGen.DungeonGen.Tests.Integration.Tables")]
namespace DnDGen.DungeonGen.Generators
{
    public interface IDungeonGenerator
    {
        IEnumerable<Area> GenerateFromHall(int dungeonLevel, EncounterSpecifications environment);
        IEnumerable<Area> GenerateFromDoor(int dungeonLevel, EncounterSpecifications environment);
    }
}
