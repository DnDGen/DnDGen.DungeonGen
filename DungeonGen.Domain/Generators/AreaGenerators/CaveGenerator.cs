using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using EncounterGen.Common;
using EncounterGen.Generators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class CaveGenerator : AreaGenerator
    {
        private IAreaPercentileSelector areaPercentileSelector;
        private PoolGenerator poolGenerator;
        private IPercentileSelector percentileSelector;
        private IEncounterGenerator encounterGenerator;

        public CaveGenerator(IAreaPercentileSelector areaPercentileSelector, PoolGenerator poolGenerator, IPercentileSelector percentileSelector, IEncounterGenerator encounterGenerator)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.poolGenerator = poolGenerator;
            this.percentileSelector = percentileSelector;
            this.encounterGenerator = encounterGenerator;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel)
        {
            var selectedCave = areaPercentileSelector.SelectFrom(TableNameConstants.Caves);

            var caves = new List<Area>();
            caves.Add(selectedCave);

            if (selectedCave.Descriptions.Contains(DescriptionConstants.DoubleCavern))
            {
                var secondCave = new Area();
                secondCave.Type = AreaTypeConstants.Cave;
                secondCave.Length = Convert.ToInt32(selectedCave.Contents.Miscellaneous.First());
                secondCave.Width = Convert.ToInt32(selectedCave.Contents.Miscellaneous.Skip(1).First());
                secondCave.Contents.Miscellaneous = selectedCave.Contents.Miscellaneous.Skip(2);

                caves.Add(secondCave);

                selectedCave.Contents.Miscellaneous = Enumerable.Empty<string>();
                selectedCave.Descriptions = selectedCave.Descriptions.Except(new[] { DescriptionConstants.DoubleCavern });
            }

            foreach (var cave in caves)
            {
                if (cave.Contents.Miscellaneous.Contains(ContentsTypeConstants.Pool))
                {
                    cave.Contents.Pool = poolGenerator.Generate(partyLevel);
                }

                if (cave.Contents.Miscellaneous.Contains(ContentsTypeConstants.Lake))
                {
                    var lake = percentileSelector.SelectFrom(TableNameConstants.Lakes);
                    var sections = lake.Split('/');

                    foreach (var section in sections)
                    {
                        if (string.IsNullOrEmpty(section) == false)
                            cave.Contents.Miscellaneous = cave.Contents.Miscellaneous.Union(new[] { section });

                        if (section == ContentsTypeConstants.Encounter)
                        {
                            var encounter = encounterGenerator.Generate(EnvironmentConstants.Dungeon, partyLevel);
                            cave.Contents.Encounters = cave.Contents.Encounters.Union(new[] { encounter });
                        }
                    }

                    cave.Contents.Miscellaneous = cave.Contents.Miscellaneous.Except(new[] { ContentsTypeConstants.Lake });
                }
            }

            return caves;
        }
    }
}
