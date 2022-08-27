// 
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

// ReSharper disable once CheckNamespace

namespace Chapter.Collections;

/// <summary>
///     A dictionary implementing <see cref="INotifyCollectionChanged" />, <see cref="INotifyPropertyChanging" /> and
///     <see cref="INotifyCollectionChanged" />.
/// </summary>
/// <typeparam name="TKey">The type of the dictionary key.</typeparam>
/// <typeparam name="TValue">The type of the dictionary value.</typeparam>
/// <example>
///     <code lang="XAML">
/// <![CDATA[
/// <ListBox ItemsSource="{Binding Items}">
///     <ListBox.ItemsTemplate>
///         <DataTemplate>
///             <TextBlock Text="{Binding Value}" />
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
///         Items = new ObservableDictionary<int, string>(new DispatcherInvokator());
///         Items.CatchPropertyChanged = true;
///         Items.CatchPropertyChanging = true;
///         Items.ItemPropertyChanged += OnItemPropertyChanged;
///         Items.ItemPropertyChanging += OnItemPropertyChanging;
///     }
/// 
///     public ObservableDictionary<int, string> Items { get; }
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
///     public void Add(int no, string value)
///     {
///         Items[no] = value;
///     }
/// 
///     public void Remove(int no)
///     {
///         Items.Remove(no);
///     }
/// 
///     public void Clear()
///     {
///         using (Items.DisableNotifications())
///             Items.Clear();
///     }
/// }
/// ]]>
/// </code>
/// </example>
public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
{
    private bool _catchPropertyChanged;
    private bool _catchPropertyChanging;
    private DisableNotifications _disableNotify;
    private IInvokator _invokator;
    private const string indexerName = "Item[]";

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableDictionary{TKey,TValue}" />.
    /// </summary>
    public ObservableDictionary()
    {
        _invokator = new DirectInvokator();
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableDictionary{TKey,TValue}" />.
    /// </summary>
    /// <param name="invokator">The invokator how to do actions on the list.</param>
    public ObservableDictionary(IInvokator invokator)
    {
        _invokator = invokator;
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableDictionary{TKey,TValue}" />.
    /// </summary>
    /// <param name="dictionary">The source dictionary to take over the items from.</param>
    public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        : base(dictionary)
    {
        _invokator = new DirectInvokator();
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableDictionary{TKey,TValue}" />.
    /// </summary>
    /// <param name="invokator">The invokator how to do actions on the list.</param>
    /// <param name="dictionary">The source dictionary to take over the items from.</param>
    public ObservableDictionary(IInvokator invokator, IDictionary<TKey, TValue> dictionary)
        : base(dictionary)
    {
        _invokator = invokator;
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableDictionary{TKey,TValue}" />.
    /// </summary>
    /// <param name="comparer">The comparer to compare the keys in the dictionary when working.</param>
    public ObservableDictionary(IEqualityComparer<TKey> comparer)
        : base(comparer)
    {
        _invokator = new DirectInvokator();
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableDictionary{TKey,TValue}" />.
    /// </summary>
    /// <param name="invokator">The invokator how to do actions on the list.</param>
    /// <param name="comparer">The comparer to compare the keys in the dictionary when working.</param>
    public ObservableDictionary(IInvokator invokator, IEqualityComparer<TKey> comparer)
        : base(comparer)
    {
        _invokator = invokator;
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableDictionary{TKey,TValue}" />.
    /// </summary>
    /// <param name="capacity">The initial number of elements the dictionary can contain.</param>
    public ObservableDictionary(int capacity)
        : base(capacity)
    {
        _invokator = new DirectInvokator();
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableDictionary{TKey,TValue}" />.
    /// </summary>
    /// <param name="invokator">The invokator how to do actions on the list.</param>
    /// <param name="capacity">The initial number of elements the dictionary can contain.</param>
    public ObservableDictionary(IInvokator invokator, int capacity)
        : base(capacity)
    {
        _invokator = invokator;
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableDictionary{TKey,TValue}" />.
    /// </summary>
    /// <param name="dictionary">The source dictionary to take over the items from.</param>
    /// <param name="comparer">The comparer to compare the keys in the dictionary when working.</param>
    public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        : base(dictionary, comparer)
    {
        _invokator = new DirectInvokator();
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableDictionary{TKey,TValue}" />.
    /// </summary>
    /// <param name="invokator">The invokator how to do actions on the list.</param>
    /// <param name="dictionary">The source dictionary to take over the items from.</param>
    /// <param name="comparer">The comparer to compare the keys in the dictionary when working.</param>
    public ObservableDictionary(IInvokator invokator, IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        : base(dictionary, comparer)
    {
        _invokator = invokator;
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableDictionary{TKey,TValue}" />.
    /// </summary>
    /// <param name="capacity">The initial number of elements the dictionary can contain.</param>
    /// <param name="comparer">The comparer to compare the keys in the dictionary when working.</param>
    public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        : base(capacity, comparer)
    {
        _invokator = new DirectInvokator();
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ObservableDictionary{TKey,TValue}" />.
    /// </summary>
    /// <param name="invokator">The invokator how to do actions on the list.</param>
    /// <param name="capacity">The initial number of elements the dictionary can contain.</param>
    /// <param name="comparer">The comparer to compare the keys in the dictionary when working.</param>
    public ObservableDictionary(IInvokator invokator, int capacity, IEqualityComparer<TKey> comparer)
        : base(capacity, comparer)
    {
        _invokator = invokator;
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
                throw new InvalidOperationException("The ObservableDictionary<T>.Invokator cannot be set to null.");
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
    ///     Gets or sets the value for the given key.
    /// </summary>
    /// <param name="key">The key to associate with the value.</param>
    /// <returns>The value associated with the value.</returns>
    public new TValue this[TKey key]
    {
        get => base[key];
        set
        {
            _invokator.Invoke(() =>
            {
                OnPropertyChanging(indexerName);

                if (ContainsKey(key))
                {
                    var oldItem = base[key];
                    base[key] = value;

                    IgnoreItemPropertyChanging(oldItem);
                    IgnoreItemPropertyChanged(oldItem);
                    CatchItemPropertyChanging(value);
                    CatchItemPropertyChanged(value);
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem));
                }
                else
                {
                    OnPropertyChanging(nameof(Count));

                    base[key] = value;
                    CatchItemPropertyChanging(key);
                    CatchItemPropertyChanged(key);
                    CatchItemPropertyChanging(value);
                    CatchItemPropertyChanged(value);

                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
                    OnPropertyChanged(nameof(Count));
                }

                OnPropertyChanged(indexerName);
            });
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
    ///     Removes the element from the dictionary by its key.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns>True if the element got removed; otherwise false.</returns>
    public new bool Remove(TKey key)
    {
        if (!ContainsKey(key))
            return false;

        _invokator.Invoke(() =>
        {
            OnPropertyChanging(nameof(Count));
            OnPropertyChanging(indexerName);

            var oldItem = base[key];
            base.Remove(key);
            IgnoreItemPropertyChanging(key);
            IgnoreItemPropertyChanged(key);
            IgnoreItemPropertyChanging(oldItem);
            IgnoreItemPropertyChanged(oldItem);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem));
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(indexerName);
        });

        return true;
    }

    /// <summary>
    ///     Adds a new element to the dictionary by the key.
    /// </summary>
    /// <param name="key">The key to associate with the value.</param>
    /// <param name="value">The value to add into the dictionary.</param>
    public new void Add(TKey key, TValue value)
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(nameof(Count));
            OnPropertyChanging(indexerName);

            base.Add(key, value);
            CatchItemPropertyChanging(key);
            CatchItemPropertyChanged(key);
            CatchItemPropertyChanging(value);
            CatchItemPropertyChanged(value);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(indexerName);
        });
    }

    /// <summary>
    ///     Clears the dictionary.
    /// </summary>
    public new void Clear()
    {
        _invokator.Invoke(() =>
        {
            OnPropertyChanging(nameof(Count));
            OnPropertyChanging(indexerName);

            IgnoreItemPropertyChanging();
            IgnoreItemPropertyChanged();
            base.Clear();

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(indexerName);
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
        _disableNotify.Disposed -= OnDisableNotifyDisposed;
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
            foreach (var (key, value) in this)
            {
                CatchItemPropertyChanging(key);
                CatchItemPropertyChanging(value);
            }
    }

    private void CatchItemPropertyChanging(TKey item)
    {
        if (!CatchPropertyChanging)
            return;

        if (item is INotifyPropertyChanging notifyItem)
            notifyItem.PropertyChanging += NotifyItemPropertyChanging;
    }

    private void CatchItemPropertyChanging(TValue item)
    {
        if (!CatchPropertyChanging)
            return;

        if (item is INotifyPropertyChanging notifyItem)
            notifyItem.PropertyChanging += NotifyItemPropertyChanging;
    }

    private void CatchItemPropertyChanged()
    {
        if (CatchPropertyChanged)
            foreach (var (key, value) in this)
            {
                CatchItemPropertyChanged(key);
                CatchItemPropertyChanged(value);
            }
    }

    private void CatchItemPropertyChanged(TKey item)
    {
        if (!CatchPropertyChanged)
            return;

        if (item is INotifyPropertyChanged notifyItem)
            notifyItem.PropertyChanged += NotifyItemPropertyChanged;
    }

    private void CatchItemPropertyChanged(TValue item)
    {
        if (!CatchPropertyChanged)
            return;

        if (item is INotifyPropertyChanged notifyItem)
            notifyItem.PropertyChanged += NotifyItemPropertyChanged;
    }

    private void IgnoreItemPropertyChanging()
    {
        for (var i = 0; i < Count; ++i)
            foreach (var (key, value) in this)
            {
                IgnoreItemPropertyChanging(key);
                IgnoreItemPropertyChanging(value);
            }
    }

    private void IgnoreItemPropertyChanging(TKey item)
    {
        if (!CatchPropertyChanging)
            return;

        if (item is INotifyPropertyChanging notifyItem)
            notifyItem.PropertyChanging -= NotifyItemPropertyChanging;
    }

    private void IgnoreItemPropertyChanging(TValue item)
    {
        if (!CatchPropertyChanging)
            return;

        if (item is INotifyPropertyChanging notifyItem)
            notifyItem.PropertyChanging -= NotifyItemPropertyChanging;
    }

    private void IgnoreItemPropertyChanged()
    {
        for (var i = 0; i < Count; ++i)
            foreach (var (key, value) in this)
            {
                IgnoreItemPropertyChanged(key);
                IgnoreItemPropertyChanged(value);
            }
    }

    private void IgnoreItemPropertyChanged(TKey item)
    {
        if (!CatchPropertyChanged)
            return;

        if (item is INotifyPropertyChanged notifyItem)
            notifyItem.PropertyChanged -= NotifyItemPropertyChanged;
    }

    private void IgnoreItemPropertyChanged(TValue item)
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