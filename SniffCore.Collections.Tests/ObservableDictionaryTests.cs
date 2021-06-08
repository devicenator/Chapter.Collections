using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using NUnit.Framework;

namespace SniffCore.Collections.Tests
{
    [TestFixture]
    public class ObservableDictionaryTests
    {
        private TestableInvokator _invokator;
        private ObservableDictionary<int, string> _target;

        [SetUp]
        public void Setup()
        {
            _invokator = new TestableInvokator();
            _target = new ObservableDictionary<int, string>(_invokator);
        }

        [Test]
        public void Add_Called_UsesInvokator()
        {
            AssertInvokatorUsed(() => _target.Add(0, "zero"));
        }

        [Test]
        public void Add_Called_RaisesPropertyChangingWithCount()
        {
            AssertPropertyChanging(nameof(_target.Count), new Dictionary<int, string>(), ToDictionary(5, "five"), () => _target.Add(5, "five"));
        }

        [Test]
        public void Add_Called_RaisesPropertyChangingWithIndexer()
        {
            AssertPropertyChanging(Binding.IndexerName, new Dictionary<int, string>(), ToDictionary(5, "five"), () => _target.Add(5, "five"));
        }

        [Test]
        public void Add_Called_AddsItem()
        {
            AssertAct(new Dictionary<int, string>(), ToDictionary(5, "five"), () => _target.Add(5, "five"));
        }

        [Test]
        public void Add_Called_RaisesPropertyChangedWithCount()
        {
            AssertPropertyChanged(nameof(_target.Count), ToDictionary(5, "five"), ToDictionary(5, "five"), () => _target.Add(5, "five"));
        }

        [Test]
        public void Add_Called_RaisesPropertyChangedWithAdd()
        {
            AssertPropertyChanged(Binding.IndexerName, ToDictionary(5, "five"), ToDictionary(5, "five"), () => _target.Add(5, "five"));
        }

        [Test]
        public void Add_Called_RaisesCollectionChangedWithAdd()
        {
            AssertCollectionChanged(NotifyCollectionChangedAction.Add, null, new []{"five"}, -1, -1, () => _target.Add(5, "five"));
        }

        [Test]
        public void Clear_Called_UsesInvokator()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertInvokatorUsed(() => _target.Clear());
        }

        [Test]
        public void Clear_Called_RaisesPropertyChangingWithCount()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertPropertyChanging(nameof(_target.Count), ToDictionary(0, "zero", 5, "five"), new Dictionary<int, string>(), () => _target.Clear());
        }

        [Test]
        public void Clear_Called_RaisesPropertyChangingWithIndexer()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertPropertyChanging(Binding.IndexerName, ToDictionary(0, "zero", 5, "five"), new Dictionary<int, string>(), () => _target.Clear());
        }

        [Test]
        public void Clear_Called_RemovesAllItems()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertAct(ToDictionary(0, "zero", 5, "five"), new Dictionary<int, string>(), () => _target.Clear());
        }

        [Test]
        public void Clear_Called_RaisesPropertyChangedWithCount()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertPropertyChanged(nameof(_target.Count), new Dictionary<int, string>(), new Dictionary<int, string>(), () => _target.Clear());
        }

        [Test]
        public void Clear_Called_RaisesPropertyChangedWithReset()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertPropertyChanged(Binding.IndexerName, new Dictionary<int, string>(), new Dictionary<int, string>(), () => _target.Clear());
        }

        [Test]
        public void Clear_Called_RaisesCollectionChangedWithReset()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertCollectionChanged(NotifyCollectionChangedAction.Reset, null, null, -1, -1, () => _target.Clear());
        }

        [Test]
        public void Indexer_Set_UsesInvokator()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertInvokatorUsed(() => _target[3] = "three");
        }

        [Test]
        public void Indexer_Set_RaisesPropertyChangingWithIndexer()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertPropertyChanging(Binding.IndexerName, ToDictionary(0, "zero", 5, "five"), ToDictionary(0, "zero", 3, "three", 5, "five"), () => _target[3] = "three");
        }

        [Test]
        public void Indexer_Set_RaisesPropertyChangedWithIndexer()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertPropertyChanged(Binding.IndexerName, ToDictionary(0, "zero", 3, "three", 5, "five"), ToDictionary(0, "zero", 3, "three", 5, "five"), () => _target[3] = "three");
        }

        [Test]
        public void Indexer_SetToKnownKey_SetsTheItem()
        {
            _target[0] = "zero";
            _target[3] = "three";

            AssertAct(ToDictionary(0, "zero", 3, "three"), ToDictionary(0, "zero", 3, "other three"), () => _target[3] = "other three");
        }

        [Test]
        public void Indexer_SetToUnknownKey_AddsTheItem()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertAct(ToDictionary(0, "zero", 5, "five"), ToDictionary(0, "zero", 3, "three", 5, "five"), () => _target[3] = "three");
        }

        [Test]
        public void Indexer_SetToKnownKey_RaisesCollectionChangedWithReplace()
        {
            _target[0] = "zero";
            _target[3] = "three";

            AssertCollectionChanged(NotifyCollectionChangedAction.Replace, new []{"three"}, new []{"not three"}, -1, -1, () => _target[3] = "not three");
        }

        [Test]
        public void Indexer_SetWithUnknownKey_RaisesPropertyChangingWithCount()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertPropertyChanging(nameof(_target.Count), ToDictionary(0, "zero", 5, "five"), ToDictionary(0, "zero", 3, "three", 5, "five"), () => _target[3] = "three");
        }

        [Test]
        public void Indexer_SetWithUnknownKey_RaisesCollectionChangedWithAdd()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertCollectionChanged(NotifyCollectionChangedAction.Add, null, new []{"three"}, -1, -1, () => _target[3] = "three");
        }

        [Test]
        public void Indexer_SetWithUnknownKey_RaisesPropertyChangedWithCount()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertPropertyChanged(nameof(_target.Count), ToDictionary(0, "zero", 3, "three", 5, "five"), ToDictionary(0, "zero", 3, "three", 5, "five"), () => _target[3] = "three");
        }

        [Test]
        public void Remove_CalledWithKeyWithUnknownKey_RaisesNoEvent()
        {
            _target[0] = "zero";
            _target[5] = "five";

            var result = true;
            AssertNoEvents(() => result = _target.Remove(3));

            Assert.That(result, Is.False);
        }

        [Test]
        public void Remove_CalledWithKey_UsesInvokator()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertInvokatorUsed(() => _target.Remove(5));
        }

        [Test]
        public void Remove_CalledWithKey_RaisesPropertyChangingWithCount()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertPropertyChanging(nameof(_target.Count), ToDictionary(0, "zero", 5, "five"), ToDictionary(0, "zero"), () => _target.Remove(5));
        }

        [Test]
        public void Remove_CalledWithKey_RaisesPropertyChangingWithIndexer()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertPropertyChanging(Binding.IndexerName, ToDictionary(0, "zero", 5, "five"), ToDictionary(0, "zero"), () => _target.Remove(5));
        }

        [Test]
        public void Remove_CalledWithKey_RemovesTheItem()
        {
            _target[0] = "zero";
            _target[5] = "five";

            var result = false;
            AssertAct(ToDictionary(0, "zero", 5, "five"), ToDictionary(0, "zero"), () => result = _target.Remove(5));

            Assert.That(result, Is.True);
        }

        [Test]
        public void Remove_CalledWithKey_RaisesPropertyChangedWithCount()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertPropertyChanged(nameof(_target.Count), ToDictionary(0, "zero"), ToDictionary(0, "zero"), () => _target.Remove(5));
        }

        [Test]
        public void Remove_CalledWithKey_RaisesPropertyChangedWithIndexer()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertPropertyChanged(nameof(_target.Count), ToDictionary(0, "zero"), ToDictionary(0, "zero"), () => _target.Remove(5));
        }

        [Test]
        public void Remove_CalledWithKey_RaisesCollectionChangedWithRemove()
        {
            _target[0] = "zero";
            _target[5] = "five";

            AssertCollectionChanged(NotifyCollectionChangedAction.Remove, new []{"five"}, null, -1, -1, () => _target.Remove(5));
        }

        [Test]
        public void Remove_CalledWithKeyAndOutWithUnknownKey_RaisesNoEvent()
        {
            _target[0] = "zero";
            _target[5] = "five";

            var result = true;
            var value = string.Empty;
            AssertNoEvents(() => result = _target.Remove(3, out value));

            Assert.That(result, Is.False);
            Assert.That(value, Is.Null);
        }
        
        [Test]
        public void DisableNotifications_Disposed_CallsResetEvents()
        {
            var countTriggered = false;
            var indexerTriggered = false;
            var collectionChangedTriggered = false;

            void TargetOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == nameof(_target.Count))
                    countTriggered = true;
                if (e.PropertyName == Binding.IndexerName)
                    indexerTriggered = true;
            }

            _target.PropertyChanged += TargetOnPropertyChanged;

            void TargetOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                    collectionChangedTriggered = true;
            }

            _target.CollectionChanged += TargetOnCollectionChanged;
            var token = _target.DisableNotifications();

            token.Dispose();

            Assert.That(countTriggered, Is.True);
            Assert.That(indexerTriggered, Is.True);
            Assert.That(collectionChangedTriggered, Is.True);
            _target.PropertyChanged -= TargetOnPropertyChanged;
            _target.CollectionChanged -= TargetOnCollectionChanged;
        }

        [Test]
        public void DisableNotifications_IndexerSet_DoesNotRaiseEvents()
        {
            _target[0] = "zero";
            _target[5] = "five";
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target[3] = "three");
            }
        }

        [Test]
        public void DisableNotifications_Add_DoesNotRaiseEvents()
        {
            _target[0] = "zero";
            _target[5] = "five";
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Add(3, "three"));
            }
        }

        [Test]
        public void DisableNotifications_Clear_DoesNotRaiseEvents()
        {
            _target[0] = "zero";
            _target[5] = "five";
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Clear());
            }
        }

        [Test]
        public void DisableNotifications_RemoveWithKey_DoesNotRaiseEvents()
        {
            _target[0] = "zero";
            _target[5] = "five";
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Remove(5));
            }
        }

        [Test]
        public void DisableNotifications_RemoveWithKeyAndOut_DoesNotRaiseEvents()
        {
            _target[0] = "zero";
            _target[5] = "five";
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Remove(5, out _));
            }
        }

        private Dictionary<int, string> ToDictionary(int keyOne, string valueOne)
        {
            return new Dictionary<int, string>
            {
                {keyOne, valueOne}
            };
        }

        private Dictionary<int, string> ToDictionary(int keyOne, string valueOne, int keyTwo, string valueTwo)
        {
            return new Dictionary<int, string>
            {
                {keyOne, valueOne},
                {keyTwo, valueTwo}
            };
        }

        private Dictionary<int, string> ToDictionary(int keyOne, string valueOne, int keyTwo, string valueTwo, int keyThree, string valueThree)
        {
            return new Dictionary<int, string>
            {
                {keyOne, valueOne},
                {keyTwo, valueTwo},
                {keyThree, valueThree}
            };
        }

        private void AssertNoEvents(Action action)
        {
            var triggered = false;

            void TargetOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                triggered = true;
            }

            void TargetOnPropertyChanging(object sender, PropertyChangingEventArgs e)
            {
                triggered = true;
            }

            void TargetOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                triggered = true;
            }

            _target.CollectionChanged += TargetOnCollectionChanged;
            _target.PropertyChanging += TargetOnPropertyChanging;
            _target.PropertyChanged += TargetOnPropertyChanged;

            action();

            _target.CollectionChanged -= TargetOnCollectionChanged;
            _target.PropertyChanging -= TargetOnPropertyChanging;
            _target.PropertyChanged -= TargetOnPropertyChanged;
            Assert.That(triggered, Is.False);
        }

        private void AssertAct(Dictionary<int, string> before, Dictionary<int, string> after, Action act)
        {
            Assert.That(_target, Is.EqualTo(before));

            act();

            Assert.That(_target, Is.EqualTo(after));
        }

        private void AssertInvokatorUsed(Action action)
        {
            _invokator.Reset();

            action();

            Assert.That(_invokator.Triggered, Is.True);
        }

        private void AssertPropertyChanging(string property, Dictionary<int, string> before, Dictionary<int, string> after, Action act)
        {
            var triggered = false;

            void TargetOnPropertyChanging(object sender, PropertyChangingEventArgs e)
            {
                if (e.PropertyName == property)
                    triggered = true;
                Assert.That(_target, Is.EqualTo(before));
            }

            _target.PropertyChanging += TargetOnPropertyChanging;

            act();

            _target.PropertyChanging -= TargetOnPropertyChanging;
            Assert.That(triggered, Is.True);
            Assert.That(_target, Is.EqualTo(after));
        }

        private void AssertPropertyChanged(string property, Dictionary<int, string> before, Dictionary<int, string> after, Action act)
        {
            var triggered = false;

            void TargetOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == property)
                    triggered = true;
                Assert.That(_target, Is.EqualTo(before));
            }

            _target.PropertyChanged += TargetOnPropertyChanged;

            act();

            _target.PropertyChanged -= TargetOnPropertyChanged;
            Assert.That(triggered, Is.True);
            Assert.That(_target, Is.EqualTo(after));
        }

        private void AssertCollectionChanged(NotifyCollectionChangedAction change, string[] oldItems, string[] newItems, int oldIndex, int newIndex, Action act)
        {
            var triggered = false;

            void TargetOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == change &&
                    Equals(e.OldItems, oldItems) &&
                    Equals(e.NewItems, newItems) &&
                    e.OldStartingIndex == oldIndex &&
                    e.NewStartingIndex == newIndex)
                    triggered = true;
            }

            _target.CollectionChanged += TargetOnCollectionChanged;

            act();

            _target.CollectionChanged -= TargetOnCollectionChanged;
            Assert.That(triggered, Is.True);
        }

        private bool Equals(IList left, string[] right)
        {
            if (left == null || right == null)
                return left == right;
            return left.Cast<string>().SequenceEqual(right);
        }

        private class TestableInvokator : IInvokator
        {
            public bool Triggered { get; private set; }

            public void Invoke(Action action)
            {
                Triggered = true;
                action();
            }

            public void Reset()
            {
                Triggered = false;
            }
        }
    }
}