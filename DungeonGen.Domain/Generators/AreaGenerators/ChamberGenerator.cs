using DungeonGen.Domain.Generators.ContentGenerators;
using DungeonGen.Domain.Generators.ExitGenerators;
using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using EncounterGen.Generators;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class ChamberGenerator : AreaGenerator
    {
        public string AreaType
        {
            get { return AreaTypeConstants.Chamber; }
        }

        private readonly IAreaPercentileSelector areaPercentileSelector;
        private readonly JustInTimeFactory justInTimeFactory;
        private readonly ContentsGenerator contentsGenerator;
        private readonly AreaGeneratorFactory areaGeneratorFactory;

        public ChamberGenerator(IAreaPercentileSelector areaPercentileSelector, AreaGeneratorFactory areaGeneratorFactory, JustInTimeFactory justInTimeFactory, ContentsGenerator contentsGenerator)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.areaGeneratorFactory = areaGeneratorFactory;
            this.justInTimeFactory = justInTimeFactory;
            this.contentsGenerator = contentsGenerator;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment)
        {
            var chamber = areaPercentileSelector.SelectFrom(TableNameConstants.Chambers);
            var chambers = new List<Area>();

            if (chamber.Type == AreaTypeConstants.Special)
            {
                var specialAreaGenerator = areaGeneratorFactory.Build(AreaTypeConstants.Special);
                var specialAreas = specialAreaGenerator.Generate(dungeonLevel, environment);
                chambers.AddRange(specialAreas);
            }
            else
            {
                chambers.Add(chamber);
            }

            for (var i = chambers.Count - 1; i >= 0; i--)
            {
                //INFO: This is for special chambers, which will have an empty type
                if (string.IsNullOrEmpty(chambers[i].Type))
                    chambers[i].Type = AreaTypeConstants.Chamber;

                var exitGenerator = justInTimeFactory.Build<ExitGenerator>(AreaTypeConstants.Chamber);
                var exits = exitGenerator.Generate(dungeonLevel, environment, chambers[i].Length, chambers[i].Width);

                if (i + 1 == chambers.Count)
                    chambers.AddRange(exits);
                else
                    chambers.InsertRange(i + 1, exits);

                var newContents = contentsGenerator.Generate(environment.Level);
                chambers[i].Contents.Encounters = chambers[i].Contents.Encounters.Union(newContents.Encounters);
                chambers[i].Contents.Miscellaneous = chambers[i].Contents.Miscellaneous.Union(newContents.Miscellaneous);
                chambers[i].Contents.Traps = chambers[i].Contents.Traps.Union(newContents.Traps);
                chambers[i].Contents.Treasures = chambers[i].Contents.Treasures.Union(newContents.Treasures);
            }

            return chambers;
        }
    }
}
