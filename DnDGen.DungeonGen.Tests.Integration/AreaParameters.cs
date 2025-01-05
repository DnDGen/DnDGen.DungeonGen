using DnDGen.EncounterGen.Models;
using System.Collections.Generic;

namespace DnDGen.DungeonGen.Tests.Integration
{
    public static class AreaParameters
    {
        public static IEnumerable<string> AllEnvironments =>
        [
            EnvironmentConstants.Aquatic,
            EnvironmentConstants.Civilized,
            EnvironmentConstants.Desert,
            EnvironmentConstants.Forest,
            EnvironmentConstants.Hills,
            EnvironmentConstants.Marsh,
            EnvironmentConstants.Mountain,
            EnvironmentConstants.Plains,
            EnvironmentConstants.Underground,
        ];

        public static IEnumerable<string> AllTemperatures =>
        [
            EnvironmentConstants.Temperatures.Cold,
            EnvironmentConstants.Temperatures.Temperate,
            EnvironmentConstants.Temperatures.Warm
        ];

        public static IEnumerable<string> AllTimesOfDay =>
        [
            EnvironmentConstants.TimesOfDay.Day,
            EnvironmentConstants.TimesOfDay.Night,
        ];
    }
}
