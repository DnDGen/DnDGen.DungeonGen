using System.IO;

namespace DungeonGen.Tables
{
    public interface StreamLoader
    {
        Stream LoadFor(string filename);
    }
}
