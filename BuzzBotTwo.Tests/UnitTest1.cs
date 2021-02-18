using System;
using System.Linq;
using BuzzBotTwo.Domain.Seed;
using NUnit.Framework;

namespace BuzzBotTwo.Tests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var seeder = new ItemSeeder();
            var data = seeder.Data.ToList();
            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
            Assert.IsTrue(data.Count>0);

        }
    }
}