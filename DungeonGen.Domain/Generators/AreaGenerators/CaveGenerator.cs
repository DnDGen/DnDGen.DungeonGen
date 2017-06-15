using DungeonGen.Domain.Generators.Factories;
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
        public string AreaType
        {
            get { return AreaTypeConstants.Cave; }
        }

        private readonly IAreaPercentileSelector areaPercentileSelector;
        private readonly IPercentileSelector percentileSelector;
        private readonly JustInTimeFactory justInTimeFactory;

        public CaveGenerator(IAreaPercentileSelector areaPercentileSelector, JustInTimeFactory justInTimeFactory, IPercentileSelector percentileSelector)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.justInTimeFactory = justInTimeFactory;
            this.percentileSelector = percentileSelector;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel, string temperature)
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
                    var poolGenerator = justInTimeFactory.Build<PoolGenerator>();
                    cave.Contents.Pool = poolGenerator.Generate(partyLevel, temperature);
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
                            var specifications = new EncounterSpecifications();
                            specifications.Environment = EnvironmentConstants.Underground;
                            specifications.Level = partyLevel;
                            specifications.Temperature = temperature;
                            specifications.TimeOfDay = EnvironmentConstants.TimesOfDay.Night;
                            specifications.AllowAquatic = true; //INFO: Because this is for a lake

                            var encounterGenerator = justInTimeFactory.Build<IEncounterGenerator>();
                            var encounter = encounterGenerator.Generate(specifications);
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
