using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SniffCore.Collections.Tests
{
    [TestFixture]
    public class ListExTests
    {
        [Test]
        public void IndexOf_CalledWithNullList_ThrowsException()
        {
            List<string> items = null;

            var action = new TestDelegate(() => items.IndexOf(x => true));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void IndexOf_CalledWithNullCondition_ThrowsException()
        {
            var items = new List<string>();
            Func<string, bool> condition = null;

            var action = new TestDelegate(() => items.IndexOf(condition));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void IndexOf_CalledWithNothingMatching_ReturnsMinusOne()
        {
            var items = new List<string> {"A", "B", "C", "A"};

            var result = items.IndexOf(x => x == "D");

            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void IndexOf_CalledWithMatch_ReturnsFirstIndexOfMatch()
        {
            var items = new List<string> {"A", "B", "C", "A"};

            var result = items.IndexOf(x => x == "A");

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void LastIndexOf_CalledWithNullList_ThrowsException()
        {
            List<string> items = null;

            var action = new TestDelegate(() => items.LastIndexOf(x => true));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void LastIndexOf_CalledWithNullCondition_ThrowsException()
        {
            var items = new List<string>();
            Func<string, bool> condition = null;

            var action = new TestDelegate(() => items.LastIndexOf(condition));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void LastIndexOf_CalledWithNothingMatching_ReturnsMinusOne()
        {
            var items = new List<string> {"A", "B", "C", "A"};

            var result = items.LastIndexOf(x => x == "D");

            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void LastIndexOf_CalledWithMatch_ReturnsLastIndexOfMatch()
        {
            var items = new List<string> {"A", "B", "C", "A"};

            var result = items.LastIndexOf(x => x == "A");

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void Split_CalledWithNull_ThrowsException()
        {
            List<string> items = null;

            var action = new TestDelegate(() => items.Split(10));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void Split_CalledWithZeroAmount_ThrowsException()
        {
            var items = new List<string>();

            var action = new TestDelegate(() => items.Split(0));

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void Split_Called_SplitsTheList()
        {
            var items = Enumerable.Range(1, 44).ToList();

            var splitted = items.Split(10);

            Assert.That(splitted.Count, Is.EqualTo(5));
            Assert.That(splitted[0], Is.EqualTo(Enumerable.Range(1, 10).ToList()));
            Assert.That(splitted[1], Is.EqualTo(Enumerable.Range(11, 10).ToList()));
            Assert.That(splitted[2], Is.EqualTo(Enumerable.Range(21, 10).ToList()));
            Assert.That(splitted[3], Is.EqualTo(Enumerable.Range(31, 10).ToList()));
            Assert.That(splitted[4], Is.EqualTo(Enumerable.Range(41, 4).ToList()));
        }

        [Test]
        public void Shuffle_CalledWithNullList_ThrowsException()
        {
            List<string> items = null;

            var action = new TestDelegate(() => items.Shuffle());

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void Shuffle_CalledWithList_ShufflesTheItems()
        {
            var items = Enumerable.Range(1, 10).ToList();

            var shuffledItems = items.Shuffle();

            Assert.That(shuffledItems, Is.EquivalentTo(items));
            Assert.That(shuffledItems, Is.Not.EqualTo(items));
        }

        [Test]
        public void Shuffle_CalledWithListThreeTimes_ShufflesTheItems()
        {
            var items = Enumerable.Range(1, 10).ToList();

            var shuffledItems1 = items.Shuffle();
            var shuffledItems2 = items.Shuffle();
            var shuffledItems3 = items.Shuffle();

            Assert.That(shuffledItems1, Is.EquivalentTo(items));
            Assert.That(shuffledItems2, Is.EquivalentTo(items));
            Assert.That(shuffledItems3, Is.EquivalentTo(items));
            Assert.That(shuffledItems1, Is.Not.EqualTo(items));
            Assert.That(shuffledItems2, Is.Not.EqualTo(items));
            Assert.That(shuffledItems3, Is.Not.EqualTo(items));
            Assert.That(shuffledItems1, Is.Not.EqualTo(shuffledItems2));
            Assert.That(shuffledItems2, Is.Not.EqualTo(shuffledItems3));
            Assert.That(shuffledItems1, Is.Not.EqualTo(shuffledItems3));
        }

        [Test]
        public void Shuffle_CalledWithNullIList_ThrowsException()
        {
            IList<string> items = null;

            var action = new TestDelegate(() => items.Shuffle());

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void Shuffle_CalledWithIList_ShufflesTheItems()
        {
            IList<int> items = Enumerable.Range(1, 10).ToList();

            var shuffledItems = items.Shuffle();

            Assert.That(shuffledItems, Is.EquivalentTo(items));
            Assert.That(shuffledItems, Is.Not.EqualTo(items));
        }

        [Test]
        public void Shuffle_CalledWithIListThreeTimes_ShufflesTheItems()
        {
            IList<int> items = Enumerable.Range(1, 10).ToList();

            var shuffledItems1 = items.Shuffle();
            var shuffledItems2 = items.Shuffle();
            var shuffledItems3 = items.Shuffle();

            Assert.That(shuffledItems1, Is.EquivalentTo(items));
            Assert.That(shuffledItems2, Is.EquivalentTo(items));
            Assert.That(shuffledItems3, Is.EquivalentTo(items));
            Assert.That(shuffledItems1, Is.Not.EqualTo(items));
            Assert.That(shuffledItems2, Is.Not.EqualTo(items));
            Assert.That(shuffledItems3, Is.Not.EqualTo(items));
            Assert.That(shuffledItems1, Is.Not.EqualTo(shuffledItems2));
            Assert.That(shuffledItems2, Is.Not.EqualTo(shuffledItems3));
            Assert.That(shuffledItems1, Is.Not.EqualTo(shuffledItems3));
        }
    }
}