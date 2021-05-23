//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace SniffCore.Collections
{
    public class ObservableList<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
    {
        private bool _catchPropertyChanged;
        private DisableNotifications _disableNotify;
        private IInvokator _invokator;

        public ObservableList()
        {
            Invokator = new DirectInvokator();
        }

        public ObservableList(IInvokator invokator)
        {
            Invokator = invokator;
        }

        public ObservableList(IEnumerable<T> collection)
            : base(collection.ToList())
        {
            Invokator = new DirectInvokator();
        }

        public ObservableList(IInvokator invokator, IEnumerable<T> collection)
            : base(collection.ToList())
        {
            Invokator = invokator;
        }

        public IInvokator Invokator
        {
            get => _invokator;
            set
            {
                _invokator = value;
                if (_invokator == null)
                    throw new InvalidOperationException("The ObservableList<T>.Invokator cannot be set to null.");
            }
        }

        public bool CatchPropertyChanged
        {
            get => _catchPropertyChanged;
            set
            {
                if (_catchPropertyChanged == value)
                    return;
                if (value)
                    CatchItemPropertyChanged();
                else if (_catchPropertyChanged)
                    IgnoreItemPropertyChanged();
                _catchPropertyChanged = value;
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public event EventHandler<PropertyChangedEventArgs> ItemPropertyChanged;

        public virtual void Move(int oldIndex, int newIndex)
        {
            MoveItem(oldIndex, newIndex);
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            _invokator.Invoke(() =>
            {
                OnPropertyChanging(nameof(Count));
                OnPropertyChanging(Binding.IndexerName);

                var itemsToAdd = items.ToArray();
                foreach (var item in itemsToAdd)
                {
                    var index = Items.Count;
                    base.InsertItem(index, item);
                    CatchItemPropertyChanged(item);
                }

                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        public virtual void Swap(T item1, T item2)
        {
            Swap(IndexOf(item1), IndexOf(item2));
        }

        public virtual void Swap(int index1, int index2)
        {
            _invokator.Invoke(() =>
            {
                OnPropertyChanging(Binding.IndexerName);

                var tmp = Items[index1];
                Items[index1] = Items[index2];
                Items[index2] = tmp;

                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, Items[index1], Items[index2]));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, Items[index2], Items[index1]));
            });
        }

        protected override void ClearItems()
        {
            _invokator.Invoke(() =>
            {
                OnPropertyChanging(nameof(Count));
                OnPropertyChanging(Binding.IndexerName);

                base.ClearItems();
                IgnoreItemPropertyChanged();

                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        protected override void InsertItem(int index, T item)
        {
            _invokator.Invoke(() =>
            {
                OnPropertyChanging(nameof(Count));
                OnPropertyChanging(Binding.IndexerName);

                base.InsertItem(index, item);
                CatchItemPropertyChanged(item);

                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            });
        }

        public virtual bool Remove(Func<T, bool> condition)
        {
            var itemToRemove = this.FirstOrDefault(condition);
            return itemToRemove != null && Remove(itemToRemove);
        }

        public virtual bool RemoveLast(Func<T, bool> condition)
        {
            var itemToRemove = this.LastOrDefault(condition);
            return itemToRemove != null && Remove(itemToRemove);
        }

        public virtual void RemoveAll(Func<T, bool> condition)
        {
            _invokator.Invoke(() =>
            {
                var removedItems = this.Where(condition).ToArray();
                if (!removedItems.Any())
                    return;

                OnPropertyChanging(nameof(Count));
                OnPropertyChanging(Binding.IndexerName);

                removedItems.ForEach(x => Remove(x));

                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems));
            });
        }

        public virtual void RemoveRange(int index, int count)
        {
            _invokator.Invoke(() =>
            {
                OnPropertyChanging(nameof(Count));
                OnPropertyChanging(Binding.IndexerName);

                var removedItems = new List<T>();
                for (var i = 0; i < count; ++i)
                {
                    var item = Items[index];
                    removedItems.Add(item);
                    IgnoreItemPropertyChanged(item);
                    Items.RemoveAt(index);
                }

                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems));
            });
        }

        public virtual void Sort()
        {
            _invokator.Invoke(() =>
            {
                OnPropertyChanging(Binding.IndexerName);

                var items = Items.ToList();
                items.Sort();
                Items.Clear();
                foreach (var item in items)
                    Items.Add(item);

                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        public virtual void Sort(IComparer<T> comparer)
        {
            _invokator.Invoke(() =>
            {
                OnPropertyChanging(Binding.IndexerName);

                var items = Items.ToList();
                items.Sort(comparer);
                Items.Clear();
                foreach (var item in items)
                    Items.Add(item);

                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        public virtual void Sort(Comparison<T> comparison)
        {
            _invokator.Invoke(() =>
            {
                OnPropertyChanging(Binding.IndexerName);

                var items = Items.ToList();
                items.Sort(comparison);
                Items.Clear();
                foreach (var item in items)
                    Items.Add(item);

                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        public virtual void Sort(int index, int count, IComparer<T> comparer)
        {
            _invokator.Invoke(() =>
            {
                OnPropertyChanging(Binding.IndexerName);

                var items = Items.ToList();
                items.Sort(index, count, comparer);
                Items.Clear();
                foreach (var item in items)
                    Items.Add(item);

                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        public virtual void Sort<TKey>(Func<T, TKey> sorter)
        {
            _invokator.Invoke(() =>
            {
                OnPropertyChanging(Binding.IndexerName);

                var items = Items.OrderBy(sorter);
                Items.Clear();
                foreach (var item in items)
                    Items.Add(item);

                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        public virtual void Reverse()
        {
            _invokator.Invoke(() =>
            {
                OnPropertyChanging(Binding.IndexerName);

                var items = Items.Reverse();
                Items.Clear();
                foreach (var item in items)
                    Items.Add(item);

                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        protected override void RemoveItem(int index)
        {
            _invokator.Invoke(() =>
            {
                var removedItem = this[index];

                OnPropertyChanging(nameof(Count));
                OnPropertyChanging(Binding.IndexerName);

                base.RemoveItem(index);
                IgnoreItemPropertyChanged(removedItem);

                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, index));
            });
        }

        protected override void SetItem(int index, T item)
        {
            _invokator.Invoke(() =>
            {
                var originalItem = this[index];

                OnPropertyChanging(Binding.IndexerName);

                base.SetItem(index, item);
                IgnoreItemPropertyChanged(originalItem);
                CatchItemPropertyChanged(item);

                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, originalItem, item, index));
            });
        }

        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            _invokator.Invoke(() =>
            {
                var removedItem = this[oldIndex];

                OnPropertyChanging(Binding.IndexerName);

                base.RemoveItem(oldIndex);
                base.InsertItem(newIndex, removedItem);

                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex));
            });
        }

        public IDisposable DisableNotifications()
        {
            _disableNotify = new DisableNotifications();
            _disableNotify.Disposed += OnDisableNotifyDisposed;
            return _disableNotify;
        }

        private void OnDisableNotifyDisposed(object sender, EventArgs e)
        {
            var notify = (DisableNotifications) sender;
            notify.Disposed -= OnDisableNotifyDisposed;
            _disableNotify = null;
            _invokator.Invoke(() =>
            {
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        protected virtual void OnPropertyChanging(string propertyName)
        {
            if (_disableNotify == null)
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (_disableNotify == null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (_disableNotify == null)
                CollectionChanged?.Invoke(this, e);
        }

        private void CatchItemPropertyChanged()
        {
            if (CatchPropertyChanged)
                this.ForEach(CatchItemPropertyChanged);
        }

        private void CatchItemPropertyChanged(T item)
        {
            if (!CatchPropertyChanged)
                return;

            if (item is INotifyPropertyChanged notifyItem)
                notifyItem.PropertyChanged += NotifyItemPropertyChanged;
        }

        private void IgnoreItemPropertyChanged()
        {
            if (CatchPropertyChanged)
                this.ForEach(IgnoreItemPropertyChanged);
        }

        private void IgnoreItemPropertyChanged(T item)
        {
            if (!CatchPropertyChanged)
                return;

            if (item is INotifyPropertyChanged notifyItem)
                notifyItem.PropertyChanged -= NotifyItemPropertyChanged;
        }

        private void NotifyItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (CatchPropertyChanged)
                ItemPropertyChanged?.Invoke(sender, e);
        }
    }
}