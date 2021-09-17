using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

// ReSharper disable once CheckNamespace

namespace SniffCore.Collections.Tests
{
    [TestFixture]
    public class EnumerableExTests
    {
        [Test]
        public void Repeat_CalledWithNull_ThrowsException()
        {
            var count = 5u;
            Func<int> func = null;

            var action = new TestDelegate(() => _ = EnumerableEx.Repeat(func, count).ToList());

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void Repeat_Called_RepeatsTheActionGivenTimes()
        {
            var count = 0;
            var func = new Func<int>(() => count++);
            var expected = Enumerable.Range(0, 5);

            var sequence = EnumerableEx.Repeat(func, 5);

            Assert.That(sequence, Is.EquivalentTo(expected));
        }

        [Test]
        public void ForEach_CalledWithNullList_ThrowsException()
        {
            IEnumerable<int> input = null;

            var action = new TestDelegate(() => input.ForEach(i => { }));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void ForEach_CalledWithNullCondition_ThrowsException()
        {
            var input = Enumerable.Range(0, 5);

            var action = new TestDelegate(() => input.ForEach(null));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void ForEach_Called_ExecutesAnActionForEachItem()
        {
            var input = Enumerable.Range(0, 5);
            var no = 0;
            var triggered = false;

            input.ForEach(x =>
            {
                triggered = true;
                Assert.That(x, Is.EqualTo(no++));
            });

            Assert.That(triggered, Is.True);
        }

        [Test]
        public void Shuffle_CalledWithNull_ThrowsException()
        {
            IEnumerable<int> input = null;

            var action = new TestDelegate(() => _ = input.Shuffle());

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void Shuffle_Called_ShufflesTheItems()
        {
            var input = Enumerable.Range(0, 5).ToArray();

            var result1 = input.Shuffle().ToArray();
            var result2 = input.Shuffle().ToArray();
            var result3 = input.Shuffle().ToArray();

            Assert.That(result1, Is.Not.EqualTo(input));
            Assert.That(result1.Length, Is.EqualTo(5));
            Assert.That(result2.Length, Is.EqualTo(5));
            Assert.That(result3.Length, Is.EqualTo(5));
            Assert.That(result1, Is.Not.EqualTo(result2));
            Assert.That(result2, Is.Not.EqualTo(result3));
            Assert.That(result1, Is.Not.EqualTo(result3));
        }
    }
}