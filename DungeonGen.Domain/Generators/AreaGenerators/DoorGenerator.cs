using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class DoorGenerator : AreaGenerator
    {
        private IAreaPercentileSelector areaPercentileSelector;

        public DoorGenerator(IAreaPercentileSelector areaPercentileSelector)
        {
            this.areaPercentileSelector = areaPercentileSelector;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel)
        {
            var door = areaPercentileSelector.SelectFrom(TableNameConstants.DoorTypes);

            if (door.Type == AreaTypeConstants.Special)
            {
                door = GetSpecialDoor(door);
            }

            if (door.Length > 0)
            {
                var stuck = $"Stuck (Break DC {door.Length})";
                door.Descriptions = door.Descriptions.Union(new[] { stuck });
                door.Length = 0;
            }

            if (door.Width > 0)
            {
                var locked = $"Locked (Break DC {door.Width})";
                door.Descriptions = door.Descriptions.Union(new[] { locked });
                door.Width = 0;
            }

            return new[] { door };
        }

        private Area GetSpecialDoor(Area specialDoor)
        {
            var newDoor = areaPercentileSelector.SelectFrom(TableNameConstants.DoorTypes);

            while (newDoor.Type == AreaTypeConstants.Special)
            {
                newDoor = areaPercentileSelector.SelectFrom(TableNameConstants.DoorTypes);
            }

            var specialBonus = specialDoor.Length;

            if (specialDoor.Descriptions.Contains(DescriptionConstants.MagicallyReinforced) && newDoor.Descriptions.Contains(DescriptionConstants.Wooden) == false)
                specialBonus = specialDoor.Width;

            if (newDoor.Length > 0)
            {
                newDoor.Length += specialBonus;

                if (specialDoor.Descriptions.Contains(DescriptionConstants.MagicallyReinforced))
                    newDoor.Length = specialBonus;
            }

            if (newDoor.Width > 0)
            {
                newDoor.Width += specialBonus;

                if (specialDoor.Descriptions.Contains(DescriptionConstants.MagicallyReinforced))
                    newDoor.Width = specialBonus;
            }

            newDoor.Descriptions = newDoor.Descriptions.Union(specialDoor.Descriptions);

            return newDoor;
        }
    }
}
