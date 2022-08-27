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

// ReSharper disable once CheckNamespace

namespace Chapter.Collections;

/// <summary>
///     A list implementing <see cref="INotifyCollectionChanged" />, <see cref="INotifyPropertyChanging" /> and
///     <see cref="INotifyCollectionChanged" />.
/// </summary>
/// <typeparam name="T">The type of the items in the list.</typeparam>
/// <example>
///     <code lang="XAML">
/// <![CDATA[
/// <ListBox ItemsSource="{Binding Items}">
///     <ListBox.ItemsTemplate>
///         <DataTemplate>
///             <TextBlock Text="{Binding }" />
///         </DataTemplate>
///     </ListBox.ItemsTemplate>
/// </ListBox>
/// ]]>
/// </code>
///     <code lang="csharp">
/// <![CDATA[
/// public class ViewModel : ObservableObject
/// {
///     public ViewModel()
///     {
///         Items = new ObservableList<string>(new DispatcherInvokator());
///         Items.CatchPropertyChanged = true;
///         Items.CatchPropertyChanging = true;
///         Items.ItemPropertyChanged += OnItemPropertyChanged;
///         Items.ItemPropertyChanging += OnItemPropertyChanging;
///     }
/// 
///     public ObservableList<string> Items { get; }
/// 
///     private void OnItemPropertyChanged(string sender, PropertyChangedEventArgs e)
///     {
///         // var item = sender.ToString();
///         // e.PropertyName
///     }
/// 
///     private void OnItemPropertyChanging(string sender, PropertyChangingEventArgs e)
///     {
///         // var item = sender.ToString();
///         // e.PropertyName
///     }
/// 
///     public void Add(params string[] items)
///     {
///         Items.AddRange(items);
///         Items.Sort();
///     }
/// 
///     public void Clear()
///     {
///         using (Items.DisableNotifications())
///             Items.Clear();
///     }
/// 
///     private void RemoveAll()
///     {
///         Items.RemoveAll(x => Matches(x));
///     }
/// }
/// ]]>
/// </code>
/// </example>
public class ObservableList<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
{
    private bool _catchPropertyChanged;
    private bool _catchPropertyChanging;
    private DisableNotifications _disableNotify;
    private IInvokator _invokator;
    private const string indexerName = "Item[]";

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableList{T}" />.
    /// </summary>
    public ObservableList()
    {
        Invokator = new DirectInvokator();
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableList{T}" />.
    /// </summary>
    /// <param name="invokator">The invokator how to do actions on the list.</param>
    public ObservableList(IInvokator invokator)
    {
        Invokator = invokator;
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableList{T}" />.
    /// </summary>
    /// <param name="collection">The source collection to take over the items from.</param>
    public ObservableList(IEnumerable<T> collection)
        : base(collection.ToList())
    {
        Invokator = new DirectInvokator();
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableList{T}" />.
    /// </summary>
    /// <param name="invokator">The invokator how to do actions on the list.</param>
    /// <param name="collection">The source collection to take over the items from.</param>
    public ObservableList(IInvokator invokator, IEnumerable<T> collection)
        : base(collection.ToList())
    {
        Invokator = invokator;
    }

    /// <summary>
    ///     Gets or sets the invokator to use for actions on the collection.
    /// </summary>
    /// <exception cref="InvalidOperationException">The Invokator cannot be set to null.</exception>
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

    /// <summary>
    ///     Gets or sets the value indicating of the <see cref="INotifyPropertyChanging.PropertyChanging" /> is taken from the
    ///     elements in the collection and forwarded by <see cref="INotifyPropertyChanging" />.
    /// </summary>
    public bool CatchPropertyChanging
    {
        get => _catchPropertyChanging;
        set
        {
            if (_catchPropertyChanging == value)
                return;
            _catchPropertyChanging = value;
            if (value)
                CatchItemPropertyChanging();
            else
                IgnoreItemPropertyChanging();
        }
    }

    /// <summary>
    ///     Gets or sets the value indicating of the <see cref="INotifyPropertyChanged.PropertyChanged" /> is taken from the
    ///     elements in the collection and forwarded by <see cref="ItemPropertyChanged" />.
    /// </summary>
    public bool CatchPropertyChanged
    {
        get => _catchPropertyChanged;
        set
        {
            if (_catchPropertyChanged == value)
                return;
            _catchPropertyChanged = value;
            if (value)
                CatchItemPropertyChanged();
            else
                IgnoreItemPropertyChanged();
        }
    }

    /// <summary>
    ///     Raised of the collection has been changed.
    /// </summary>
    public event NotifyCollectionChangedEventHandler CollectionChanged;

    /// <summary>
    ///     Raised of a property has been changed.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    ///     Raised of a property is about to change.
    /// </summary>
    public event PropertyChangingEventHandler PropertyChanging;

    /// <summary>
    ///     Raised the forwarded <see cref="INotifyPropertyChanging.PropertyChanging" /> from an element of the list.
    /// </summary>
    public event EventHandler<PropertyChangingEventArgs> ItemPropertyChanging;

    /// <summary>
    ///     Raised the forwarded <see cref="INotifyPropertyChanged.PropertyChanged" /> from an element of the list.
    /// </summary>
    public event EventHandler<PropertyChangedEventArgs> ItemPropertyChanged;

    /// <summary>
    ///     Moves an item from one to another position in the list.
    /// </summary>
    /// <param name="oldIndex">The old index of the item.</param>
    /// <param name="newIndex">The new index of the item.</param>
    public virtual void Move(int oldIndex, int newIndex)
    {
        MoveItem(oldIndex, newIndex);
    }

    /// <summary>
    ///     Adds multiple items to the collection.
    /// </summary>
    /// <param name="items">The items to add.</param>
    public virtual void AddRange(IEnumerable<T> items)
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(nameof(Count));
            OnPropertyChanging(indexerName);

            var itemsToAdd = items.ToArray();
            foreach (var item in itemsToAdd)
            {
                var index = Items.Count;
                base.InsertItem(index, item);
                CatchItemPropertyChanging(item);
                CatchItemPropertyChanged(item);
            }

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        });
    }

    /// <summary>
    ///     Swaps two items in the collection.
    /// </summary>
    /// <param name="item1">The first item.</param>
    /// <param name="item2">The second item.</param>
    public virtual void Swap(T item1, T item2)
    {
        Swap(IndexOf(item1), IndexOf(item2));
    }

    /// <summary>
    ///     Swaps two items in the collection by their index.
    /// </summary>
    /// <param name="index1">The index of the first item.</param>
    /// <param name="index2">The index of the second item.</param>
    public virtual void Swap(int index1, int index2)
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(indexerName);

            var tmp = Items[index1];
            Items[index1] = Items[index2];
            Items[index2] = tmp;

            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, Items[index1], Items[index2]));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, Items[index2], Items[index1]));
        });
    }

    /// <summary>
    ///     Removes all items from the collection.
    /// </summary>
    protected override void ClearItems()
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(nameof(Count));
            OnPropertyChanging(indexerName);

            IgnoreItemPropertyChanging();
            IgnoreItemPropertyChanged();
            base.ClearItems();

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        });
    }

    /// <summary>
    ///     Inserts a new item into the collection on a specific index.
    /// </summary>
    /// <param name="index">The index where to add the item.</param>
    /// <param name="item">The new item to add.</param>
    protected override void InsertItem(int index, T item)
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(nameof(Count));
            OnPropertyChanging(indexerName);

            base.InsertItem(index, item);
            CatchItemPropertyChanging(item);
            CatchItemPropertyChanged(item);

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        });
    }

    /// <summary>
    ///     Removes the first items from the collection the condition matches.
    /// </summary>
    /// <param name="condition">The match condition.</param>
    /// <returns>True if an item got removed; otherwise false.</returns>
    public virtual bool Remove(Func<T, bool> condition)
    {
        var itemToRemove = this.FirstOrDefault(condition);
        return itemToRemove != null && Remove(itemToRemove);
    }

    /// <summary>
    ///     Removes the last items from the collection the condition matches.
    /// </summary>
    /// <param name="condition">The match condition.</param>
    /// <returns>True if an item got removed; otherwise false.</returns>
    public virtual bool RemoveLast(Func<T, bool> condition)
    {
        var index = this.LastIndexOf(condition);
        if (index == -1)
            return false;
        RemoveAt(index);
        return true;
    }

    /// <summary>
    ///     Removes all items from the collection the condition matches.
    /// </summary>
    /// <param name="condition">The match condition.</param>
    public virtual void RemoveAll(Func<T, bool> condition)
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(nameof(Count));
            OnPropertyChanging(indexerName);

            var removed = new List<T>();
            for (var i = 0; i < Count; i++)
            {
                var item = Items[i];
                if (condition(item))
                {
                    removed.Add(item);
                    IgnoreItemPropertyChanging(item);
                    IgnoreItemPropertyChanged(item);
                    base.RemoveItem(i--);
                }
            }

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed));
        });
    }

    /// <summary>
    ///     Removes given amount of items starting at the given position.
    /// </summary>
    /// <param name="index">The position where to start removal.</param>
    /// <param name="count">The amount of items to remove.</param>
    public virtual void RemoveRange(int index, int count)
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(nameof(Count));
            OnPropertyChanging(indexerName);

            var removedItems = new List<T>();
            for (var i = 0; i < count; ++i)
            {
                var item = Items[index];
                removedItems.Add(item);
                IgnoreItemPropertyChanging(item);
                IgnoreItemPropertyChanged(item);
                base.RemoveItem(index);
            }

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems));
        });
    }

    /// <summary>
    ///     Sorts the items in the collection.
    /// </summary>
    public virtual void Sort()
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(indexerName);

            var items = Items.ToList();
            items.Sort();
            base.ClearItems();
            foreach (var item in items)
            {
                var index = Items.Count;
                base.InsertItem(index, item);
            }

            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        });
    }

    /// <summary>
    ///     Sorts the collection by the given <see cref="IComparer{T}" />.
    /// </summary>
    /// <param name="comparer">The <see cref="IComparer{T}" /> to be used when sort.</param>
    public virtual void Sort(IComparer<T> comparer)
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(indexerName);

            var items = Items.ToList();
            items.Sort(comparer);
            base.ClearItems();
            foreach (var item in items)
            {
                var index = Items.Count;
                base.InsertItem(index, item);
            }

            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        });
    }

    /// <summary>
    ///     Sorts the collection by the given <see cref="Comparison{T}" />.
    /// </summary>
    /// <param name="comparison">The <see cref="Comparison{T}" /> to be used when sort.</param>
    public virtual void Sort(Comparison<T> comparison)
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(indexerName);

            var items = Items.ToList();
            items.Sort(comparison);
            base.ClearItems();
            foreach (var item in items)
            {
                var index = Items.Count;
                base.InsertItem(index, item);
            }

            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        });
    }

    /// <summary>
    ///     Sorts the elements in the collection for the given amount in the given position by a <see cref="IComparer{T}" />.
    /// </summary>
    /// <param name="index">The index where to start the sorting from.</param>
    /// <param name="count">The amount of elements to sort.</param>
    /// <param name="comparer">The <see cref="IComparer{T}" /> to be used when sort.</param>
    public virtual void Sort(int index, int count, IComparer<T> comparer)
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(indexerName);

            var items = Items.ToList();
            items.Sort(index, count, comparer);
            base.ClearItems();
            foreach (var item in items)
            {
                var position = Items.Count;
                base.InsertItem(position, item);
            }

            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        });
    }

    /// <summary>
    ///     Sorts the collection by the given sorting func.
    /// </summary>
    /// <typeparam name="TKey">The key of the items in the collection.</typeparam>
    /// <param name="sorter">The sorter func to use.</param>
    public virtual void Sort<TKey>(Func<T, TKey> sorter)
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(indexerName);

            var items = Items.OrderBy(sorter).ToList();
            base.ClearItems();
            foreach (var item in items)
            {
                var index = Items.Count;
                base.InsertItem(index, item);
            }

            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        });
    }

    /// <summary>
    ///     Reverses the items in the collection.
    /// </summary>
    public virtual void Reverse()
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(indexerName);

            var items = Items.Reverse().ToList();
            base.ClearItems();
            foreach (var item in items)
            {
                var index = Items.Count;
                base.InsertItem(index, item);
            }

            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        });
    }

    /// <summary>
    ///     Removes the element from the given position in the collection.
    /// </summary>
    /// <param name="index"></param>
    protected override void RemoveItem(int index)
    {
        _invokator.Invoke(() =>
        {
            var removedItem = this[index];

            OnPropertyChanging(nameof(Count));
            OnPropertyChanging(indexerName);

            IgnoreItemPropertyChanging(removedItem);
            IgnoreItemPropertyChanged(removedItem);
            base.RemoveItem(index);

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, index));
        });
    }

    /// <summary>
    ///     Replaces the item in the collection on the given index.
    /// </summary>
    /// <param name="index">The index which item to replace.</param>
    /// <param name="item">The new item to set.</param>
    protected override void SetItem(int index, T item)
    {
        _invokator.Invoke(() =>
        {
            var originalItem = this[index];

            OnPropertyChanging(indexerName);

            base.SetItem(index, item);
            IgnoreItemPropertyChanging(originalItem);
            IgnoreItemPropertyChanged(originalItem);
            CatchItemPropertyChanging(item);
            CatchItemPropertyChanged(item);

            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, originalItem, item, index));
        });
    }

    /// <summary>
    ///     Moves an item in the collection from one to another position.
    /// </summary>
    /// <param name="oldIndex">The old position of the item in the collection.</param>
    /// <param name="newIndex">The new position of the item in the collection.</param>
    protected virtual void MoveItem(int oldIndex, int newIndex)
    {
        _invokator.Invoke(() =>
        {
            var removedItem = this[oldIndex];

            OnPropertyChanging(indexerName);

            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, removedItem);

            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex));
        });
    }

    /// <summary>
    ///     Stops raising of <see cref="CollectionChanged" />, <see cref="PropertyChanging" /> and
    ///     <see cref="PropertyChanged" /> till the return value is disposed.
    ///     After the return object got disposed the dictionary raises <see cref="PropertyChanged" />,
    ///     <see cref="PropertyChanging" /> and <see cref="CollectionChanged" /> with
    ///     <see cref="NotifyCollectionChangedAction.Reset" />.
    /// </summary>
    /// <returns></returns>
    public IDisposable DisableNotifications()
    {
        _disableNotify = new DisableNotifications();
        _disableNotify.Disposed += OnDisableNotifyDisposed;
        return _disableNotify;
    }

    private void OnDisableNotifyDisposed(object sender, EventArgs e)
    {
        var notify = (DisableNotifications)sender;
        notify.Disposed -= OnDisableNotifyDisposed;
        _disableNotify = null;
        _invokator.Invoke(() =>
        {
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        });
    }

    /// <summary>
    ///     Raises <see cref="PropertyChanging" />.
    /// </summary>
    /// <param name="propertyName">The name of the property to change.</param>
    protected virtual void OnPropertyChanging(string propertyName)
    {
        if (_disableNotify == null)
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }

    /// <summary>
    ///     Raises <see cref="PropertyChanged" />.
    /// </summary>
    /// <param name="propertyName">The name of the property which is about to change.</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (_disableNotify == null)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    ///     Raises <see cref="CollectionChanged" />.
    /// </summary>
    /// <param name="e">The event args to raise with.</param>
    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (_disableNotify == null)
            CollectionChanged?.Invoke(this, e);
    }

    private void CatchItemPropertyChanging()
    {
        if (CatchPropertyChanging)
            for (var i = 0; i < Count; ++i)
                CatchItemPropertyChanging(Items[i]);
    }

    private void CatchItemPropertyChanging(T item)
    {
        if (!CatchPropertyChanging)
            return;

        if (item is INotifyPropertyChanging notifyItem)
            notifyItem.PropertyChanging += NotifyItemPropertyChanging;
    }

    private void CatchItemPropertyChanged()
    {
        if (CatchPropertyChanged)
            for (var i = 0; i < Count; ++i)
                CatchItemPropertyChanged(Items[i]);
    }

    private void CatchItemPropertyChanged(T item)
    {
        if (!CatchPropertyChanged)
            return;

        if (item is INotifyPropertyChanged notifyItem)
            notifyItem.PropertyChanged += NotifyItemPropertyChanged;
    }

    private void IgnoreItemPropertyChanging()
    {
        for (var i = 0; i < Count; ++i)
            IgnoreItemPropertyChanging(Items[i]);
    }

    private void IgnoreItemPropertyChanging(T item)
    {
        if (!CatchPropertyChanging)
            return;

        if (item is INotifyPropertyChanging notifyItem)
            notifyItem.PropertyChanging -= NotifyItemPropertyChanging;
    }

    private void IgnoreItemPropertyChanged()
    {
        for (var i = 0; i < Count; ++i)
            IgnoreItemPropertyChanged(Items[i]);
    }

    private void IgnoreItemPropertyChanged(T item)
    {
        if (!CatchPropertyChanged)
            return;

        if (item is INotifyPropertyChanged notifyItem)
            notifyItem.PropertyChanged -= NotifyItemPropertyChanged;
    }

    private void NotifyItemPropertyChanging(object sender, PropertyChangingEventArgs e)
    {
        if (CatchPropertyChanging)
            ItemPropertyChanging?.Invoke(sender, e);
    }

    private void NotifyItemPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (CatchPropertyChanged)
            ItemPropertyChanged?.Invoke(sender, e);
    }
}