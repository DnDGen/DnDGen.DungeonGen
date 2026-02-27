using DnDGen.DungeonGen.Models;
using DnDGen.DungeonGen.Selectors;
using DnDGen.DungeonGen.Tables;
using DnDGen.EncounterGen.Generators;
using DnDGen.Infrastructure.Factories;
using DnDGen.Infrastructure.Selectors.Percentiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.DungeonGen.Generators.AreaGenerators
{
    internal class CaveGenerator(IAreaPercentileSelector areaPercentileSelector, JustInTimeFactory justInTimeFactory, IPercentileSelector percentileSelector) : AreaGenerator
    {
        public string AreaType => AreaTypeConstants.Cave;

        public IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment)
        {
            var selectedCave = areaPercentileSelector.SelectFrom(TableNameConstants.Caves);

            var caves = new List<Area>
            {
                selectedCave
            };

            if (selectedCave.Descriptions.Contains(DescriptionConstants.DoubleCavern))
            {
                var secondCave = new Area
                {
                    Type = AreaTypeConstants.Cave,
                    Length = Convert.ToInt32(selectedCave.Contents.Miscellaneous.First()),
                    Width = Convert.ToInt32(selectedCave.Contents.Miscellaneous.Skip(1).First())
                };
                secondCave.Contents.Miscellaneous = selectedCave.Contents.Miscellaneous.Skip(2);

                caves.Add(secondCave);

                selectedCave.Contents.Miscellaneous = [];
                selectedCave.Descriptions = selectedCave.Descriptions.Except([DescriptionConstants.DoubleCavern]);
            }

            foreach (var cave in caves)
            {
                if (cave.Contents.Miscellaneous.Contains(ContentsTypeConstants.Pool))
                {
                    var poolGenerator = justInTimeFactory.Build<PoolGenerator>();
                    cave.Contents.Pool = poolGenerator.Generate(environment);
                }

                if (cave.Contents.Miscellaneous.Contains(ContentsTypeConstants.Lake))
                {
                    var lake = percentileSelector.SelectFrom(Config.Name, TableNameConstants.Lakes);
                    var sections = lake.Split('/');

                    foreach (var section in sections)
                    {
                        if (string.IsNullOrEmpty(section) == false)
                            cave.Contents.Miscellaneous = cave.Contents.Miscellaneous.Union([section]);

                        if (section == ContentsTypeConstants.Encounter)
                        {
                            var specifications = environment.Clone();
                            //INFO: Because this is for a lake
                            specifications.AllowAquatic = true;

                            var encounterGenerator = justInTimeFactory.Build<IEncounterGenerator>();
                            var encounter = encounterGenerator.Generate(specifications);
                            cave.Contents.Encounters = cave.Contents.Encounters.Union([encounter]);
                        }
                    }

                    cave.Contents.Miscellaneous = cave.Contents.Miscellaneous.Except([ContentsTypeConstants.Lake]);
                }
            }

            return caves;
        }
    }
}
