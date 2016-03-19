using DungeonGen.Mappers;
using DungeonGen.Selectors;
using DungeonGen.Selectors.Domain;
using Moq;
using NUnit.Framework;
using RollGen;
using System;
using System.Collections.Generic;

namespace DungeonGen.Tests.Unit.Selectors
{
    [TestFixture]
    public class PercentileSelectorTests
    {
        private const string tableName = "table name";

        private IPercentileSelector selector;
        private Dictionary<int, string> table;
        private Mock<PercentileMapper> mockPercentileMapper;
        private Mock<Dice> mockDice;

        [SetUp]
        public void Setup()
        {
            mockPercentileMapper = new Mock<PercentileMapper>();
            mockDice = new Mock<Dice>();
            selector = new PercentileSelector(mockPercentileMapper.Object, mockDice.Object);
            table = new Dictionary<int, string>();

            for (var i = 1; i <= 5; i++)
                table.Add(i, "content");

            for (var i = 6; i <= 10; i++)
                table.Add(i, i.ToString());

            mockPercentileMapper.Setup(p => p.Map(tableName)).Returns(table);
            mockDice.Setup(d => d.ReplaceExpressionWithTotal(It.IsAny<string>())).Returns((string s) => s);
        }

        [TestCase(1, "content")]
        [TestCase(2, "content")]
        [TestCase(3, "content")]
        [TestCase(4, "content")]
        [TestCase(5, "content")]
        [TestCase(6, "6")]
        [TestCase(7, "7")]
        [TestCase(8, "8")]
        [TestCase(9, "9")]
        [TestCase(10, "10")]
        public void GetPercentile(int roll, string content)
        {
            mockDice.Setup(d => d.Roll(1).IndividualRolls(100)).Returns(new[] { roll });
            var result = selector.SelectFrom(tableName);
            Assert.That(result, Is.EqualTo(content));
        }

        [Test]
        public void IfRollNotPresentInTable_ThrowException()
        {
            mockDice.Setup(d => d.Roll(1).IndividualRolls(100)).Returns(new[] { 11 });
            Assert.That(() => selector.SelectFrom(tableName), Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("11 is not a valid entry in the table table name"));
        }

        [Test]
        public void ReplaceRollsInResult()
        {
            table[3] = "1d6+4 things";
            mockDice.Setup(d => d.ContainsRoll(table[3])).Returns(true);
            mockDice.Setup(d => d.Roll(1).IndividualRolls(100)).Returns(new[] { 3 });
            mockDice.Setup(d => d.ReplaceExpressionWithTotal("1d6+4 things")).Returns("rolls replaced");

            var result = selector.SelectFrom(tableName);
            Assert.That(result, Is.EqualTo("rolls replaced"));
        }

        [Test]
        public void DoNotReplaceNonRollsInResult()
        {
            table[3] = "[20/30/contents]";
            mockDice.Setup(d => d.ContainsRoll(table[3])).Returns(false);
            mockDice.Setup(d => d.Roll(1).IndividualRolls(100)).Returns(new[] { 3 });
            mockDice.Setup(d => d.ReplaceExpressionWithTotal("20/30/contents")).Returns("rolls replaced");

            var result = selector.SelectFrom(tableName);
            Assert.That(result, Is.EqualTo("[20/30/contents]"));
        }
    }
}
