using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace RandomGenerator.Interop.Test
{
    [TestFixture]
    public class GeneratorTests
    {
        [Test]
        public void Single_Default()
        {
            var gen = new Generator().Single(5).Length;
            Assert.AreEqual(5, gen);
        }

        [Test]
        public void Single_Alpha()
        {
            var gen = new Generator(true, false, false);
            Assert.AreEqual(5, gen.Single(5).Length);
        }

        [Test]
        public void Single_Numeric()
        {
            var gen = new Generator(false, true, false);
            Assert.AreEqual(5, gen.Single(5).Length);
        }

        [Test]
        public void Single_Special()
        {
            var gen = new Generator(false, false, true);
            Assert.AreEqual(5, gen.Single(5).Length);
        }

        [Test]
        public void Single_None()
        {
            var gen = new Generator(false, false, false);
            Assert.Throws<NullReferenceException>(() => gen.Single(10));
        }

        [Test]
        public void Single_All()
        {
            var gen = new Generator(true, true, true);
            Assert.AreEqual(5, gen.Single(5).Length);
        }

        [Test]
        public void Multiple()
        {
            var gen = new Generator(true, true, true);
            var list = gen.Multiple(100000, 10);

            Assert.AreEqual(100000, list.Count);
        }

        [Test]
        public void GenerateMultipleIteratively()
        {
            var gen = new Generator();
            var stringList = new List<string>();

            for (int i = 0; i < 100000; i++)
            {
                stringList.Add(gen.Single(9));
            }

            Assert.LessOrEqual(1, stringList.GroupBy(x => x).Count());
        }
    }
}
