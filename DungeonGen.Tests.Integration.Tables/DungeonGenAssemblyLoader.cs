using DnDGen.Core.Tables;
using DungeonGen.Domain.Tables;
using System.Reflection;

namespace DungeonGen.Tests.Integration.Tables
{
    public class DungeonGenAssemblyLoader : AssemblyLoader
    {
        public Assembly GetRunningAssembly()
        {
            var type = typeof(TableNameConstants);
            return type.Assembly;
        }
    }
}
