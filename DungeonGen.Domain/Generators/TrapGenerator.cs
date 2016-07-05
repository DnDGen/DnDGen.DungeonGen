using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using System;
using System.Linq;

namespace DungeonGen.Domain.Generators
{
    internal class TrapGenerator : ITrapGenerator
    {
        private IAreaPercentileSelector areaPercentileSelector;

        public TrapGenerator(IAreaPercentileSelector areaPercentileSelector)
        {
            this.areaPercentileSelector = areaPercentileSelector;
        }

        public Trap Generate(int partyLevel)
        {
            var selectedTrap = SelectTrap(partyLevel);

            var trap = new Trap();
            trap.Description = selectedTrap.Type;
            trap.SearchDC = selectedTrap.Length;
            trap.DisableDeviceDC = selectedTrap.Width;
            trap.ChallengeRating = Convert.ToInt32(selectedTrap.Descriptions.Single());

            return trap;
        }

        private Area SelectTrap(int partyLevel)
        {
            if (partyLevel <= 6)
                return areaPercentileSelector.SelectFrom(TableNameConstants.LowLevelTraps);

            if (partyLevel <= 12)
                return areaPercentileSelector.SelectFrom(TableNameConstants.MidLevelTraps);

            return areaPercentileSelector.SelectFrom(TableNameConstants.HighLevelTraps);
        }
    }
}
