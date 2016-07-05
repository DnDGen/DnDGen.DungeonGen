using System.IO;

namespace DungeonGen.Domain.Tables
{
    internal interface StreamLoader
    {
        Stream LoadFor(string filename);
    }
}
