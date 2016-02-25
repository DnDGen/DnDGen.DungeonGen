using DungeonGen.Common;
using DungeonGen.Generators.Domain.AreaGenerators;

namespace DungeonGen.Generators.Domain.RuntimeFactories.Domain
{
    public class AreaGeneratorFactory : IAreaGeneratorFactory
    {
        public AreaGenerator Build(string areaType)
        {
            switch (areaType)
            {
                case AreaTypeConstants.Chamber: return new ChamberGenerator();
                case AreaTypeConstants.Door: return new DoorGenerator();
                case AreaTypeConstants.Hall: return new HallGenerator();
                case AreaTypeConstants.SidePassage: return new SidePassageGenerator();
                case AreaTypeConstants.Stairs: return new StairsGenerator();
                case AreaTypeConstants.Turn: return new TurnGenerator();
                default: return null;
            }
        }

        public bool HasSpecificGenerator(string areaType)
        {
            var generator = Build(areaType);
            return generator != null;
        }
    }
}
