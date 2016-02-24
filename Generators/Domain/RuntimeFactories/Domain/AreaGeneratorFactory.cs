using System;

namespace DungeonGen.Generators.Domain.RuntimeFactories.Domain
{
    public class AreaGeneratorFactory : IAreaGeneratorFactory
    {
        public IAreaGenerator Build(string areaType)
        {
            throw new NotImplementedException();
        }

        public bool HasSpecificGenerator(string areaType)
        {
            throw new NotImplementedException();
        }
    }
}
