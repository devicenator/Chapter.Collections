using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace SniffCore.Collections
{
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
    {
        private DisableNotifications _disableNotify;
        private IInvokator _invokator;

        public ObservableDictionary()
        {
            _invokator = new DirectInvokator();
        }

        public ObservableDictionary(IInvokator invokator)
        {
            _invokator = invokator;
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
            _invokator = new DirectInvokator();
        }

        public ObservableDictionary(IInvokator invokator, IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
            _invokator = invokator;
        }

        public ObservableDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
            _invokator = new DirectInvokator();
        }

        public ObservableDictionary(IInvokator invokator, IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
            _invokator = invokator;
        }

        public ObservableDictionary(int capacity)
            : base(capacity)
        {
            _invokator = new DirectInvokator();
        }

        public ObservableDictionary(IInvokator invokator, int capacity)
            : base(capacity)
        {
            _invokator = invokator;
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
            _invokator = new DirectInvokator();
        }

        public ObservableDictionary(IInvokator invokator, IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
            _invokator = invokator;
        }

        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
            _invokator = new DirectInvokator();
        }

        public ObservableDictionary(IInvokator invokator, int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
            _invokator = invokator;
        }

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

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public new bool Remove(TKey key)
        {
            if (ContainsKey(key))
            {
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

            return false;
        }

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
            _invokator.Invoke(() => OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)));
        }

        protected virtual void OnPropertyChanging(string propertyName)
        {
            if (_disableNotify != null)
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (_disableNotify != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (_disableNotify != null)
                CollectionChanged?.Invoke(this, e);
        }
    }
}