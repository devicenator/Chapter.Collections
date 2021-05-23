//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace SniffCore.Collections
{
    /// <summary>
    ///     A dictionary implementing <see cref="INotifyCollectionChanged" />, <see cref="INotifyPropertyChanging" /> and
    ///     <see cref="INotifyCollectionChanged" />.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionary key.</typeparam>
    /// <typeparam name="TValue">The type of the dictionary value.</typeparam>
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
    {
        private DisableNotifications _disableNotify;
        private IInvokator _invokator;

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
                    OnPropertyChanging(Binding.IndexerName);

                    if (ContainsKey(key))
                    {
                        var oldItem = base[key];
                        base[key] = value;

                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem));
                    }
                    else
                    {
                        OnPropertyChanging(nameof(Count));

                        base[key] = value;

                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
                        OnPropertyChanged(nameof(Count));
                    }

                    OnPropertyChanged(Binding.IndexerName);
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
                OnPropertyChanging(Binding.IndexerName);

                var oldItem = base[key];
                base.Remove(key);

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem));
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(Binding.IndexerName);
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
                OnPropertyChanging(Binding.IndexerName);

                base.Add(key, value);

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(Binding.IndexerName);
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
                OnPropertyChanging(Binding.IndexerName);

                base.Clear();

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(Binding.IndexerName);
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
                OnPropertyChanged(Binding.IndexerName);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        /// <summary>
        ///     Raises <see cref="PropertyChanging" />.
        /// </summary>
        /// <param name="propertyName">The name of the property to change.</param>
        protected virtual void OnPropertyChanging(string propertyName)
        {
            if (_disableNotify != null)
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        ///     Raises <see cref="PropertyChanged" />.
        /// </summary>
        /// <param name="propertyName">The name of the property which is about to change.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (_disableNotify != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Raises <see cref="CollectionChanged" />.
        /// </summary>
        /// <param name="e">The event args to raise with.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (_disableNotify != null)
                CollectionChanged?.Invoke(this, e);
        }
    }
}