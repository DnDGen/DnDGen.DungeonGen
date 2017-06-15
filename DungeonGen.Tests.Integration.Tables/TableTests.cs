using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables
{
    [TestFixture]
    public abstract class TableTests : IntegrationTests
    {
        protected abstract string tableName { get; }
    }
}
