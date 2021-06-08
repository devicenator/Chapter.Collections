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
    public class ObservableListTests
    {
        [SetUp]
        public void Setup()
        {
            _invokator = new TestableInvokator();
            _target = new ObservableList<string>(_invokator);

            _target.Clear();
        }

        private TestableInvokator _invokator;
        private ObservableList<string> _target;

        [Test]
        public void Move_Called_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("B");

            AssertInvokatorUsed(() => _target.Move(0, 1));
        }

        [Test]
        public void Move_Called_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("B");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "B"}, new[] {"B", "A"}, () => _target.Move(0, 1));
        }

        [Test]
        public void Move_Called_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("B");

            AssertPropertyChanged(Binding.IndexerName, new[] {"B", "A"}, new[] {"B", "A"}, () => _target.Move(0, 1));
        }

        [Test]
        public void Move_Called_CallsCollectionChangedMove()
        {
            _target.Add("A");
            _target.Add("B");

            AssertCollectionChanged(NotifyCollectionChangedAction.Move, new[] {"A"}, new[] {"A"}, 0, 1, () => _target.Move(0, 1));
        }

        [Test]
        public void Move_Called_MovesTheItems()
        {
            _target.Add("A");
            _target.Add("B");

            AssertAct(new[] {"A", "B"}, new[] {"B", "A"}, () => _target.Move(0, 1));
        }

        [Test]
        public void AddRange_Called_GoesOverInvokator()
        {
            AssertInvokatorUsed(() => _target.AddRange(new[] {"A", "B"}));
        }

        [Test]
        public void AddRange_Called_CallsPropertyChangingCount()
        {
            AssertPropertyChanging(nameof(_target.Count), Array.Empty<string>(), new[] {"A", "B"}, () => _target.AddRange(new[] {"A", "B"}));
        }

        [Test]
        public void AddRange_Called_CallsPropertyChangingIndexerName()
        {
            AssertPropertyChanging(Binding.IndexerName, Array.Empty<string>(), new[] {"A", "B"}, () => _target.AddRange(new[] {"A", "B"}));
        }

        [Test]
        public void AddRange_Called_CallsPropertyChangedCount()
        {
            AssertPropertyChanged(nameof(_target.Count), new[] {"A", "B"}, new[] {"A", "B"}, () => _target.AddRange(new[] {"A", "B"}));
        }

        [Test]
        public void AddRange_Called_CallsPropertyChangedIndexerName()
        {
            AssertPropertyChanged(Binding.IndexerName, new[] {"A", "B"}, new[] {"A", "B"}, () => _target.AddRange(new[] {"A", "B"}));
        }

        [Test]
        public void AddRange_Called_CallsCollectionChangedReset()
        {
            AssertCollectionChanged(NotifyCollectionChangedAction.Reset, null, null, -1, -1, () => _target.AddRange(new[] {"A", "B"}));
        }

        [Test]
        public void AddRange_Called_AddsTheItems()
        {
            AssertAct(Array.Empty<string>(), new[] {"A", "B"}, () => _target.AddRange(new[] {"A", "B"}));
        }

        [Test]
        public void Add_Called_GoesOverInvokator()
        {
            AssertInvokatorUsed(() => _target.Add("A"));
        }

        [Test]
        public void Add_Called_CallsPropertyChangingCount()
        {
            AssertPropertyChanging(nameof(_target.Count), Array.Empty<string>(), new[] {"A"}, () => _target.Add("A"));
        }

        [Test]
        public void Add_Called_CallsPropertyChangingIndexerName()
        {
            AssertPropertyChanging(Binding.IndexerName, Array.Empty<string>(), new[] {"A"}, () => _target.Add("A"));
        }

        [Test]
        public void Add_Called_CallsPropertyChangedCount()
        {
            AssertPropertyChanged(nameof(_target.Count), new[] {"A"}, new[] {"A"}, () => _target.Add("A"));
        }

        [Test]
        public void Add_Called_CallsPropertyChangedIndexerName()
        {
            AssertPropertyChanged(Binding.IndexerName, new[] {"A"}, new[] {"A"}, () => _target.Add("A"));
        }

        [Test]
        public void Add_Called_CallsCollectionChangedAdd()
        {
            AssertCollectionChanged(NotifyCollectionChangedAction.Add, null, new[] {"A"}, -1, 0, () => _target.Add("A"));
        }

        [Test]
        public void Add_Called_AddsTheItem()
        {
            AssertAct(Array.Empty<string>(), new[] {"A"}, () => _target.Add("A"));
        }

        [Test]
        public void Swap_CalledWithIndexer_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertInvokatorUsed(() => _target.Swap(0, 2));
        }

        [Test]
        public void Swap_CalledWithIndexer_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "B", "C"}, new[] {"C", "B", "A"}, () => _target.Swap(0, 2));
        }

        [Test]
        public void Swap_CalledWithIndexer_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertPropertyChanged(Binding.IndexerName, new[] {"C", "B", "A"}, new[] {"C", "B", "A"}, () => _target.Swap(0, 2));
        }

        [Test]
        public void Swap_CalledWithIndexer_CallsCollectionChangedReplaceOneDirection()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertCollectionChanged(NotifyCollectionChangedAction.Replace, new[] {"A"}, new[] {"C"}, -1, -1, () => _target.Swap(0, 2));
        }

        [Test]
        public void Swap_CalledWithIndexer_CallsCollectionChangedReplaceOtherDirection()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertCollectionChanged(NotifyCollectionChangedAction.Replace, new[] {"C"}, new[] {"A"}, -1, -1, () => _target.Swap(0, 2));
        }

        [Test]
        public void Swap_CalledWithIndexer_SwapsTheItems()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertAct(new[] {"A", "B", "C"}, new[] {"C", "B", "A"}, () => _target.Swap(0, 2));
        }

        [Test]
        public void Swap_CalledWithItems_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertInvokatorUsed(() => _target.Swap("A", "C"));
        }

        [Test]
        public void Swap_CalledWithItems_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "B", "C"}, new[] {"C", "B", "A"}, () => _target.Swap("A", "C"));
        }

        [Test]
        public void Swap_CalledWithItems_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertPropertyChanged(Binding.IndexerName, new[] {"C", "B", "A"}, new[] {"C", "B", "A"}, () => _target.Swap("A", "C"));
        }

        [Test]
        public void Swap_CalledWithItems_CallsCollectionChangedReplaceOneDirection()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertCollectionChanged(NotifyCollectionChangedAction.Replace, new[] {"A"}, new[] {"C"}, -1, -1, () => _target.Swap("A", "C"));
        }

        [Test]
        public void Swap_CalledWithItems_CallsCollectionChangedReplaceOtherDirection()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertCollectionChanged(NotifyCollectionChangedAction.Replace, new[] {"C"}, new[] {"A"}, -1, -1, () => _target.Swap("A", "C"));
        }

        [Test]
        public void Swap_CalledWithItems_SwapsTheItems()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertAct(new[] {"A", "B", "C"}, new[] {"C", "B", "A"}, () => _target.Swap("A", "C"));
        }

        [Test]
        public void Clear_Called_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("B");

            AssertInvokatorUsed(() => _target.Clear());
        }

        [Test]
        public void Clear_Called_CallsPropertyChangingCount()
        {
            _target.Add("A");
            _target.Add("B");

            AssertPropertyChanging(nameof(_target.Count), new[] {"A", "B"}, Array.Empty<string>(), () => _target.Clear());
        }

        [Test]
        public void Clear_Called_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("B");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "B"}, Array.Empty<string>(), () => _target.Clear());
        }

        [Test]
        public void Clear_Called_CallsPropertyChangedCount()
        {
            _target.Add("A");
            _target.Add("B");

            AssertPropertyChanged(nameof(_target.Count), Array.Empty<string>(), Array.Empty<string>(), () => _target.Clear());
        }

        [Test]
        public void Clear_Called_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("B");

            AssertPropertyChanged(Binding.IndexerName, Array.Empty<string>(), Array.Empty<string>(), () => _target.Clear());
        }

        [Test]
        public void Clear_Called_CallsCollectionChangedReset()
        {
            _target.Add("A");
            _target.Add("B");

            AssertCollectionChanged(NotifyCollectionChangedAction.Reset, null, null, -1, -1, () => _target.Clear());
        }

        [Test]
        public void Clear_Called_RemovesAllItems()
        {
            _target.Add("A");
            _target.Add("B");

            AssertAct(new[] {"A", "B"}, Array.Empty<string>(), () => _target.Clear());
        }

        [Test]
        public void Insert_Called_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("C");

            AssertInvokatorUsed(() => _target.Insert(1, "B"));
        }

        [Test]
        public void Insert_Called_CallsPropertyChangingCount()
        {
            _target.Add("A");
            _target.Add("C");

            AssertPropertyChanging(nameof(_target.Count), new[] {"A", "C"}, new[] {"A", "B", "C"}, () => _target.Insert(1, "B"));
        }

        [Test]
        public void Insert_Called_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("C");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "C"}, new[] {"A", "B", "C"}, () => _target.Insert(1, "B"));
        }

        [Test]
        public void Insert_Called_CallsPropertyChangedCount()
        {
            _target.Add("A");
            _target.Add("C");

            AssertPropertyChanged(nameof(_target.Count), new[] {"A", "B", "C"}, new[] {"A", "B", "C"}, () => _target.Insert(1, "B"));
        }

        [Test]
        public void Insert_Called_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("C");

            AssertPropertyChanged(Binding.IndexerName, new[] {"A", "B", "C"}, new[] {"A", "B", "C"}, () => _target.Insert(1, "B"));
        }

        [Test]
        public void Insert_Called_CallsCollectionChangedAdd()
        {
            _target.Add("A");
            _target.Add("C");

            AssertCollectionChanged(NotifyCollectionChangedAction.Add, null, new[] {"B"}, -1, 1, () => _target.Insert(1, "B"));
        }

        [Test]
        public void Insert_Called_InsertsTheItem()
        {
            _target.Add("A");
            _target.Add("C");

            AssertAct(new[] {"A", "C"}, new[] {"A", "B", "C"}, () => _target.Insert(1, "B"));
        }

        [Test]
        public void Remove_CalledWithItem_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertInvokatorUsed(() => _target.Remove("A"));
        }

        [Test]
        public void Remove_CalledWithItem_CallsPropertyChangingCount()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanging(nameof(_target.Count), new[] {"A", "B", "C", "A"}, new[] {"B", "C", "A"}, () => _target.Remove("A"));
        }

        [Test]
        public void Remove_CalledWithItem_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "B", "C", "A"}, new[] {"B", "C", "A"}, () => _target.Remove("A"));
        }

        [Test]
        public void Remove_CalledWithItem_CallsPropertyChangedCount()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanged(nameof(_target.Count), new[] {"B", "C", "A"}, new[] {"B", "C", "A"}, () => _target.Remove("A"));
        }

        [Test]
        public void Remove_CalledWithItem_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanged(Binding.IndexerName, new[] {"B", "C", "A"}, new[] {"B", "C", "A"}, () => _target.Remove("A"));
        }

        [Test]
        public void Remove_CalledWithItem_CallsCollectionChangedRemove()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertCollectionChanged(NotifyCollectionChangedAction.Remove, new[] {"A"}, null, 0, -1, () => _target.Remove("A"));
        }

        [Test]
        public void Remove_CalledWithItem_RemovesTheItem()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            var removed = false;
            AssertAct(new[] {"A", "B", "C", "A"}, new[] {"B", "C", "A"}, () => removed = _target.Remove("A"));

            Assert.That(removed, Is.True);
        }

        [Test]
        public void Remove_CalledWithUnknownItem_DoesNotChangeTheCollection()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            var removed = false;
            AssertAct(new[] {"A", "B", "C", "A"}, new[] {"A", "B", "C", "A"}, () => removed = _target.Remove("D"));

            Assert.That(removed, Is.False);
        }

        [Test]
        public void Remove_CalledWithCondition_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertInvokatorUsed(() => _target.Remove(x => x == "A"));
        }

        [Test]
        public void Remove_CalledWithCondition_CallsPropertyChangingCount()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanging(nameof(_target.Count), new[] {"A", "B", "C", "A"}, new[] {"B", "C", "A"}, () => _target.Remove(x => x == "A"));
        }

        [Test]
        public void Remove_CalledWithCondition_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "B", "C", "A"}, new[] {"B", "C", "A"}, () => _target.Remove(x => x == "A"));
        }

        [Test]
        public void Remove_CalledWithCondition_CallsPropertyChangedCount()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanged(nameof(_target.Count), new[] {"B", "C", "A"}, new[] {"B", "C", "A"}, () => _target.Remove(x => x == "A"));
        }

        [Test]
        public void Remove_CalledWithCondition_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanged(Binding.IndexerName, new[] {"B", "C", "A"}, new[] {"B", "C", "A"}, () => _target.Remove(x => x == "A"));
        }

        [Test]
        public void Remove_CalledWithCondition_CallsCollectionChangedRemove()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertCollectionChanged(NotifyCollectionChangedAction.Remove, new[] {"A"}, null, 0, -1, () => _target.Remove(x => x == "A"));
        }

        [Test]
        public void Remove_CalledWithCondition_RemovesTheFirstItem()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            var removed = false;
            AssertAct(new[] {"A", "B", "C", "A"}, new[] {"B", "C", "A"}, () => removed = _target.Remove(x => x == "A"));

            Assert.That(removed, Is.True);
        }

        [Test]
        public void Remove_CalledWithConditionOfUnknownItem_DoesNotChangeTheCollection()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            var removed = false;
            AssertAct(new[] {"A", "B", "C", "A"}, new[] {"A", "B", "C", "A"}, () => removed = _target.Remove(x => x == "D"));

            Assert.That(removed, Is.False);
        }

        [Test]
        public void RemoveAt_Called_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertInvokatorUsed(() => _target.RemoveAt(1));
        }

        [Test]
        public void RemoveAt_Called_CallsPropertyChangingCount()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertPropertyChanging(nameof(_target.Count), new[] {"A", "B", "C"}, new[] {"A", "C"}, () => _target.RemoveAt(1));
        }

        [Test]
        public void RemoveAt_Called_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "B", "C"}, new[] {"A", "C"}, () => _target.RemoveAt(1));
        }

        [Test]
        public void RemoveAt_Called_CallsPropertyChangedCount()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertPropertyChanged(nameof(_target.Count), new[] {"A", "C"}, new[] {"A", "C"}, () => _target.RemoveAt(1));
        }

        [Test]
        public void RemoveAt_Called_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertPropertyChanged(Binding.IndexerName, new[] {"A", "C"}, new[] {"A", "C"}, () => _target.RemoveAt(1));
        }

        [Test]
        public void RemoveAt_Called_CallsCollectionChangedRemove()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertCollectionChanged(NotifyCollectionChangedAction.Remove, new[] {"B"}, null, 1, -1, () => _target.RemoveAt(1));
        }

        [Test]
        public void RemoveAt_Called_RemovesTheFirstItem()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");

            AssertAct(new[] {"A", "B", "C"}, new[] {"A", "C"}, () => _target.RemoveAt(1));
        }

        [Test]
        public void RemoveLast_CalledWithCondition_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertInvokatorUsed(() => _target.RemoveLast(x => x == "A"));
        }

        [Test]
        public void RemoveLast_CalledWithCondition_CallsPropertyChangingCount()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanging(nameof(_target.Count), new[] {"A", "B", "C", "A"}, new[] {"A", "B", "C"}, () => _target.RemoveLast(x => x == "A"));
        }

        [Test]
        public void RemoveLast_CalledWithCondition_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "B", "C", "A"}, new[] {"A", "B", "C"}, () => _target.RemoveLast(x => x == "A"));
        }

        [Test]
        public void RemoveLast_CalledWithCondition_CallsPropertyChangedCount()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanged(nameof(_target.Count), new[] {"A", "B", "C"}, new[] {"A", "B", "C"}, () => _target.RemoveLast(x => x == "A"));
        }

        [Test]
        public void RemoveLast_CalledWithCondition_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanged(Binding.IndexerName, new[] {"A", "B", "C"}, new[] {"A", "B", "C"}, () => _target.RemoveLast(x => x == "A"));
        }

        [Test]
        public void RemoveLast_CalledWithCondition_CallsCollectionChangedRemove()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertCollectionChanged(NotifyCollectionChangedAction.Remove, new[] {"A"}, null, 3, -1, () => _target.RemoveLast(x => x == "A"));
        }

        [Test]
        public void RemoveLast_CalledWithCondition_RemovesTheLastItem()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            var removed = false;
            AssertAct(new[] {"A", "B", "C", "A"}, new[] {"A", "B", "C"}, () => removed = _target.RemoveLast(x => x == "A"));

            Assert.That(removed, Is.True);
        }

        [Test]
        public void RemoveLast_CalledWithConditionAndUnknownItem_DoesNotChangeTheCollection()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            var removed = false;
            AssertAct(new[] {"A", "B", "C", "A"}, new[] {"A", "B", "C", "A"}, () => removed = _target.RemoveLast(x => x == "D"));

            Assert.That(removed, Is.False);
        }

        [Test]
        public void RemoveAll_Called_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertInvokatorUsed(() => _target.RemoveAll(x => x == "A"));
        }

        [Test]
        public void RemoveAll_Called_CallsPropertyChangingCount()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanging(nameof(_target.Count), new[] {"A", "B", "C", "A"}, new[] {"B", "C"}, () => _target.RemoveAll(x => x == "A"));
        }

        [Test]
        public void RemoveAll_Called_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "B", "C", "A"}, new[] {"B", "C"}, () => _target.RemoveAll(x => x == "A"));
        }

        [Test]
        public void RemoveAll_Called_CallsPropertyChangedCount()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanged(nameof(_target.Count), new[] {"B", "C"}, new[] {"B", "C"}, () => _target.RemoveAll(x => x == "A"));
        }

        [Test]
        public void RemoveAll_Called_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanged(Binding.IndexerName, new[] {"B", "C"}, new[] {"B", "C"}, () => _target.RemoveAll(x => x == "A"));
        }

        [Test]
        public void RemoveAll_Called_CallsCollectionChangedRemove()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertCollectionChanged(NotifyCollectionChangedAction.Remove, new[] {"A", "A"}, null, -1, -1, () => _target.RemoveAll(x => x == "A"));
        }

        [Test]
        public void RemoveAll_Called_RemovesAllItems()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertAct(new[] {"A", "B", "C", "A"}, new[] {"B", "C"}, () => _target.RemoveAll(x => x == "A"));
        }

        [Test]
        public void RemoveRange_Called_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertInvokatorUsed(() => _target.RemoveRange(1, 2));
        }

        [Test]
        public void RemoveRange_Called_CallsPropertyChangingCount()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanging(nameof(_target.Count), new[] {"A", "B", "C", "A"}, new[] {"A", "A"}, () => _target.RemoveRange(1, 2));
        }

        [Test]
        public void RemoveRange_Called_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "B", "C", "A"}, new[] {"A", "A"}, () => _target.RemoveRange(1, 2));
        }

        [Test]
        public void RemoveRange_Called_CallsPropertyChangedCount()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanged(nameof(_target.Count), new[] {"A", "A"}, new[] {"A", "A"}, () => _target.RemoveRange(1, 2));
        }

        [Test]
        public void RemoveRange_Called_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertPropertyChanged(Binding.IndexerName, new[] {"A", "A"}, new[] {"A", "A"}, () => _target.RemoveRange(1, 2));
        }

        [Test]
        public void RemoveRange_Called_CallsCollectionChangedRemove()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertCollectionChanged(NotifyCollectionChangedAction.Remove, new[] {"B", "C"}, null, -1, -1, () => _target.RemoveRange(1, 2));
        }

        [Test]
        public void RemoveRange_Called_RemovesSpecificItems()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("A");

            AssertAct(new[] {"A", "B", "C", "A"}, new[] {"A", "A"}, () => _target.RemoveRange(1, 2));
        }

        [Test]
        public void Sort_Called_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertInvokatorUsed(() => _target.Sort());
        }

        [Test]
        public void Sort_Called_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "C", "B", "A"}, new[] {"A", "A", "B", "C"}, () => _target.Sort());
        }

        [Test]
        public void Sort_Called_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertPropertyChanged(Binding.IndexerName, new[] {"A", "A", "B", "C"}, new[] {"A", "A", "B", "C"}, () => _target.Sort());
        }

        [Test]
        public void Sort_Called_CallsCollectionChangedReset()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertCollectionChanged(NotifyCollectionChangedAction.Reset, null, null, -1, -1, () => _target.Sort());
        }

        [Test]
        public void Sort_Called_SortsTheItems()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertAct(new[] {"A", "C", "B", "A"}, new[] {"A", "A", "B", "C"}, () => _target.Sort());
        }

        [Test]
        public void Sort_CalledWithComparer_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertInvokatorUsed(() => _target.Sort(new StringComparer()));
        }

        [Test]
        public void Sort_CalledWithComparer_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "C", "B", "A"}, new[] {"A", "A", "B", "C"}, () => _target.Sort(new StringComparer()));
        }

        [Test]
        public void Sort_CalledWithComparer_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertPropertyChanged(Binding.IndexerName, new[] {"A", "A", "B", "C"}, new[] {"A", "A", "B", "C"}, () => _target.Sort(new StringComparer()));
        }

        [Test]
        public void Sort_CalledWithComparer_CallsCollectionChangedReset()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertCollectionChanged(NotifyCollectionChangedAction.Reset, null, null, -1, -1, () => _target.Sort(new StringComparer()));
        }

        [Test]
        public void Sort_CalledWithComparer_SortsTheItems()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertAct(new[] {"A", "C", "B", "A"}, new[] {"A", "A", "B", "C"}, () => _target.Sort(new StringComparer()));
        }

        [Test]
        public void Sort_CalledWithComparison_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertInvokatorUsed(() => _target.Sort(Comparison));
        }

        [Test]
        public void Sort_CalledWithComparison_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "C", "B", "A"}, new[] {"A", "A", "B", "C"}, () => _target.Sort(Comparison));
        }

        [Test]
        public void Sort_CalledWithComparison_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertPropertyChanged(Binding.IndexerName, new[] {"A", "A", "B", "C"}, new[] {"A", "A", "B", "C"}, () => _target.Sort(Comparison));
        }

        [Test]
        public void Sort_CalledWithComparison_CallsCollectionChangedReset()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertCollectionChanged(NotifyCollectionChangedAction.Reset, null, null, -1, -1, () => _target.Sort(Comparison));
        }

        [Test]
        public void Sort_CalledWithComparison_SortsTheItems()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertAct(new[] {"A", "C", "B", "A"}, new[] {"A", "A", "B", "C"}, () => _target.Sort(Comparison));
        }

        [Test]
        public void Sort_CalledWithIndexCountAndComparer_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertInvokatorUsed(() => _target.Sort(2, 2, new StringComparer()));
        }

        [Test]
        public void Sort_CalledWithIndexCountAndComparer_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "C", "B", "A"}, new[] {"A", "C", "A", "B"}, () => _target.Sort(2, 2, new StringComparer()));
        }

        [Test]
        public void Sort_CalledWithIndexCountAndComparer_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertPropertyChanged(Binding.IndexerName, new[] {"A", "C", "A", "B"}, new[] {"A", "C", "A", "B"}, () => _target.Sort(2, 2, new StringComparer()));
        }

        [Test]
        public void Sort_CalledWithIndexCountAndComparer_CallsCollectionChangedReset()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertCollectionChanged(NotifyCollectionChangedAction.Reset, null, null, -1, -1, () => _target.Sort(2, 2, new StringComparer()));
        }

        [Test]
        public void Sort_CalledWithIndexCountAndComparer_SortsTheItems()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertAct(new[] {"A", "C", "B", "A"}, new[] {"A", "C", "A", "B"}, () => _target.Sort(2, 2, new StringComparer()));
        }

        [Test]
        public void Sort_CalledWithSorterFunc_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertInvokatorUsed(() => _target.Sort(x => x));
        }

        [Test]
        public void Sort_CalledWithSorterFunc_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "C", "B", "A"}, new[] {"A", "A", "B", "C"}, () => _target.Sort(x => x));
        }

        [Test]
        public void Sort_CalledWithSorterFunc_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertPropertyChanged(Binding.IndexerName, new[] {"A", "A", "B", "C"}, new[] {"A", "A", "B", "C"}, () => _target.Sort(x => x));
        }

        [Test]
        public void Sort_CalledWithSorterFunc_CallsCollectionChangedReset()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertCollectionChanged(NotifyCollectionChangedAction.Reset, null, null, -1, -1, () => _target.Sort(x => x));
        }

        [Test]
        public void Sort_CalledWithSorterFunc_SortsTheItems()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            _target.Add("A");

            AssertAct(new[] {"A", "C", "B", "A"}, new[] {"A", "A", "B", "C"}, () => _target.Sort(x => x));
        }

        [Test]
        public void Reverse_Called_GoesOverInvokator()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("D");

            AssertInvokatorUsed(() => _target.Reverse());
        }

        [Test]
        public void Reverse_Called_CallsPropertyChangingIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("D");

            AssertPropertyChanging(Binding.IndexerName, new[] {"A", "B", "C", "D"}, new[] {"D", "C", "B", "A"}, () => _target.Reverse());
        }

        [Test]
        public void Reverse_Called_CallsPropertyChangedIndexerName()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("D");

            AssertPropertyChanged(Binding.IndexerName, new[] {"D", "C", "B", "A"}, new[] {"D", "C", "B", "A"}, () => _target.Reverse());
        }

        [Test]
        public void Reverse_Called_CallsCollectionChangedReset()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("D");

            AssertCollectionChanged(NotifyCollectionChangedAction.Reset, null, null, -1, -1, () => _target.Reverse());
        }

        [Test]
        public void Reverse_Called_ReversesTheItems()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("D");

            AssertAct(new[] {"A", "B", "C", "D"}, new[] {"D", "C", "B", "A"}, () => _target.Reverse());
        }

        [Test]
        public void CatchPropertyChanged_SetToOn_ForwardsForExistingItems()
        {
            var target = new ObservableList<Item>
            {
                new Item(),
                new Item()
            };
            target.CatchPropertyChanged = true;
            var count = 0;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                ++count;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target[0].OnPropertyChanged();
            target[1].OnPropertyChanged();

            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void CatchPropertyChanged_SetToOn_ForwardsForAddItem()
        {
            var target = new ObservableList<Item> {CatchPropertyChanged = true};
            var triggered = false;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                triggered = true;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target.Add(new Item());

            target[0].OnPropertyChanged();
            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(triggered, Is.True);
        }

        [Test]
        public void CatchPropertyChanged_SetToOn_ForwardsForAddRangeItems()
        {
            var target = new ObservableList<Item> {CatchPropertyChanged = true};
            var count = 0;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                ++count;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target.AddRange(new[] {new Item(), new Item()});

            target[0].OnPropertyChanged();
            target[1].OnPropertyChanged();
            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void CatchPropertyChanged_SetToOn_ForwardsForInsertItem()
        {
            var target = new ObservableList<Item> {CatchPropertyChanged = true};
            var triggered = false;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                triggered = true;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target.Insert(0, new Item());

            target[0].OnPropertyChanged();
            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(triggered, Is.True);
        }

        [Test]
        public void CatchPropertyChanged_SetToOn_DoesNotForwardForRemoveItem()
        {
            var target = new ObservableList<Item> {CatchPropertyChanged = true};
            var item1 = new Item();
            var item2 = new Item();
            target.AddRange(new[] {item1, item2});
            var triggered = false;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                triggered = true;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target.Remove(item2);

            item2.OnPropertyChanged();
            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(triggered, Is.False);
        }

        [Test]
        public void CatchPropertyChanged_SetToOn_DoesNotForwardForRemoveConditionItem()
        {
            var target = new ObservableList<Item> {CatchPropertyChanged = true};
            var item1 = new Item();
            var item2 = new Item();
            target.AddRange(new[] {item1, item2});
            var triggered = false;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                triggered = true;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target.Remove(x => true);

            item1.OnPropertyChanged();
            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(triggered, Is.False);
        }

        [Test]
        public void CatchPropertyChanged_SetToOn_DoesNotForwardForRemoveAllItems()
        {
            var target = new ObservableList<Item> {CatchPropertyChanged = true};
            var item1 = new Item();
            var item2 = new Item();
            target.AddRange(new[] {item1, item2});
            var triggered = false;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                triggered = true;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target.RemoveAll(x => true);

            item1.OnPropertyChanged();
            item2.OnPropertyChanged();
            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(triggered, Is.False);
        }

        [Test]
        public void CatchPropertyChanged_SetToOn_DoesNotForwardForRemoveLastItem()
        {
            var target = new ObservableList<Item> {CatchPropertyChanged = true};
            var item1 = new Item();
            var item2 = new Item();
            target.AddRange(new[] {item1, item2});
            var triggered = false;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                triggered = true;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target.RemoveLast(x => true);

            item2.OnPropertyChanged();
            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(triggered, Is.False);
        }

        [Test]
        public void CatchPropertyChanged_SetToOn_DoesNotForwardForRemoveRangeItems()
        {
            var target = new ObservableList<Item> {CatchPropertyChanged = true};
            var item1 = new Item();
            var item2 = new Item();
            var item3 = new Item();
            var item4 = new Item();
            target.AddRange(new[] {item1, item2, item3, item4});
            var triggered = false;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                triggered = true;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target.RemoveRange(1, 2);

            item2.OnPropertyChanged();
            item3.OnPropertyChanged();
            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(triggered, Is.False);
        }

        [Test]
        public void CatchPropertyChanged_SetToOn_DoesNotForwardForClearItems()
        {
            var target = new ObservableList<Item> {CatchPropertyChanged = true};
            var item = new Item();
            target.Add(item);
            var triggered = false;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                triggered = true;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target.Clear();

            item.OnPropertyChanged();
            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(triggered, Is.False);
        }

        [Test]
        public void CatchPropertyChanged_SetToOff_DoesNotForwardForExistingItems()
        {
            var target = new ObservableList<Item> {CatchPropertyChanged = true};
            target.Add(new Item());
            var triggered = false;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                triggered = true;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target.CatchPropertyChanged = false;

            target[0].OnPropertyChanged();
            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(triggered, Is.False);
        }

        [Test]
        public void CatchPropertyChanged_SetToOff_DoesNotForwardForAddItem()
        {
            var target = new ObservableList<Item> {CatchPropertyChanged = true};
            var triggered = false;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                triggered = true;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target.CatchPropertyChanged = false;
            target.Add(new Item());

            target[0].OnPropertyChanged();
            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(triggered, Is.False);
        }

        [Test]
        public void CatchPropertyChanged_SetToOff_DoesNotForwardForAddRangeItems()
        {
            var target = new ObservableList<Item> {CatchPropertyChanged = true};
            var triggered = false;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                triggered = true;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target.CatchPropertyChanged = false;
            target.AddRange(new[] {new Item(), new Item()});

            target[0].OnPropertyChanged();
            target[1].OnPropertyChanged();
            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(triggered, Is.False);
        }

        [Test]
        public void CatchPropertyChanged_SetToOff_DoesNotForwardForInsertItem()
        {
            var target = new ObservableList<Item> {CatchPropertyChanged = true};
            var triggered = false;

            void TargetOnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                triggered = true;
            }

            target.ItemPropertyChanged += TargetOnItemPropertyChanged;

            target.CatchPropertyChanged = false;
            target.Insert(0, new Item());

            target[0].OnPropertyChanged();
            target.ItemPropertyChanged -= TargetOnItemPropertyChanged;
            Assert.That(triggered, Is.False);
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
        public void DisableNotifications_Add_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Add("D"));
            }
        }

        [Test]
        public void DisableNotifications_AddRange_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.AddRange(new[] {"D", "E"}));
            }
        }

        [Test]
        public void DisableNotifications_Clear_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Clear());
            }
        }

        [Test]
        public void DisableNotifications_Insert_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Insert(1, "D"));
            }
        }

        [Test]
        public void DisableNotifications_Move_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Move(1, 2));
            }
        }

        [Test]
        public void DisableNotifications_RemoveByItem_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Remove("B"));
            }
        }

        [Test]
        public void DisableNotifications_RemoveByCondition_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Remove(x => x == "B"));
            }
        }

        [Test]
        public void DisableNotifications_RemoveAll_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.RemoveAll(x => x == "B"));
            }
        }

        [Test]
        public void DisableNotifications_RemoveLast_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.RemoveLast(x => x == "B"));
            }
        }

        [Test]
        public void DisableNotifications_RemoveRange_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.RemoveRange(0, 1));
            }
        }

        [Test]
        public void DisableNotifications_Reverse_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Reverse());
            }
        }

        [Test]
        public void DisableNotifications_Sort_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Sort());
            }
        }

        [Test]
        public void DisableNotifications_SortWithComparison_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Sort(Comparison));
            }
        }

        [Test]
        public void DisableNotifications_SortWithComparer_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Sort(new StringComparer()));
            }
        }

        [Test]
        public void DisableNotifications_SortWithFunc_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Sort(x => x));
            }
        }

        [Test]
        public void DisableNotifications_SortWithRangeComparer_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("C");
            _target.Add("B");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Sort(0, 2, new StringComparer()));
            }
        }

        [Test]
        public void DisableNotifications_SwapByIndex_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Swap(1, 2));
            }
        }

        [Test]
        public void DisableNotifications_SwapByItems_DoesNotRaiseEvents()
        {
            _target.Add("A");
            _target.Add("B");
            _target.Add("C");
            using (_target.DisableNotifications())
            {
                AssertNoEvents(() => _target.Swap("B", "C"));
            }
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

        private void AssertAct(string[] before, string[] after, Action act)
        {
            Assert.That(_target.ToArray(), Is.EqualTo(before));

            act();

            Assert.That(_target.ToArray(), Is.EqualTo(after));
        }

        private void AssertInvokatorUsed(Action action)
        {
            _invokator.Reset();

            action();

            Assert.That(_invokator.Triggered, Is.True);
        }

        private void AssertPropertyChanging(string property, string[] before, string[] after, Action act)
        {
            var triggered = false;

            void TargetOnPropertyChanging(object sender, PropertyChangingEventArgs e)
            {
                if (e.PropertyName == property)
                    triggered = true;
                Assert.That(_target.ToArray(), Is.EqualTo(before));
            }

            _target.PropertyChanging += TargetOnPropertyChanging;

            act();

            _target.PropertyChanging -= TargetOnPropertyChanging;
            Assert.That(triggered, Is.True);
            Assert.That(_target.ToArray(), Is.EqualTo(after));
        }

        private void AssertPropertyChanged(string property, string[] before, string[] after, Action act)
        {
            var triggered = false;

            void TargetOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == property)
                    triggered = true;
                Assert.That(_target.ToArray(), Is.EqualTo(before));
            }

            _target.PropertyChanged += TargetOnPropertyChanged;

            act();

            _target.PropertyChanged -= TargetOnPropertyChanged;
            Assert.That(triggered, Is.True);
            Assert.That(_target.ToArray(), Is.EqualTo(after));
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

        private class Item : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public void OnPropertyChanged()
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Property"));
            }
        }

        private class StringComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return string.Compare(x, y, StringComparison.InvariantCulture);
            }
        }

        private static int Comparison(string s1, string s2)
        {
            return string.Compare(s1, s2, StringComparison.InvariantCulture);
        }
    }
}