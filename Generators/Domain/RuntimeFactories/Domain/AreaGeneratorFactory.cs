using DungeonGen.Common;
using DungeonGen.Generators.Domain.AreaGenerators;
using DungeonGen.Selectors;

namespace DungeonGen.Generators.Domain.RuntimeFactories.Domain
{
    public class AreaGeneratorFactory : IAreaGeneratorFactory
    {
        private IAreaPercentileSelector areaPercentileSelector;
        private AreaGenerator specialChamberGenerator;
        private ExitGenerator chamberExitGenerator;
        private ContentsGenerator contentsGenerator;
        private AreaGenerator hallGenerator;
        private IPercentileSelector percentileSelector;

        public AreaGeneratorFactory(IAreaPercentileSelector areaPercentileSelector, AreaGenerator specialChamberGenerator, ExitGenerator chamberExitGenerator, ContentsGenerator contentsGenerator,
            AreaGenerator hallGenerator, IPercentileSelector percentileSelector)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.specialChamberGenerator = specialChamberGenerator;
            this.chamberExitGenerator = chamberExitGenerator;
            this.contentsGenerator = contentsGenerator;
            this.hallGenerator = hallGenerator;
            this.percentileSelector = percentileSelector;
        }

        public AreaGenerator Build(string areaType)
        {
            switch (areaType)
            {
                case AreaTypeConstants.Chamber: return new ChamberGenerator(areaPercentileSelector, specialChamberGenerator, chamberExitGenerator, contentsGenerator);
                case AreaTypeConstants.Door: return new DoorGenerator();
                case AreaTypeConstants.Hall: return hallGenerator;
                case AreaTypeConstants.SidePassage: return new SidePassageGenerator(percentileSelector, hallGenerator);
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
