using System;
using NUnit.Framework;

namespace RandomGenerator.Interop.Test
{
    [TestFixture]
    public class GeneratorTests
    {
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
            //TODO: Figure out how to throw this exception properly
            //Assert.Throws<NullReferenceException>(new TestDelegate(gen.Single));
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
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            var gen = new Generator(true, true, true);
            var list = gen.Multiple(100000, 10);

            Assert.AreEqual(100000, list.Count);
        }
    }
}
