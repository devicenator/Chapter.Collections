// 
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 

using System;
using System.Windows.Threading;

// ReSharper disable once CheckNamespace

namespace SniffCore.Collections;

/// <summary>
///     Invokes an action using a <see cref="Dispatcher.BeginInvoke(System.Delegate, object[])" />.
/// </summary>
public sealed class DispatcherBeginInvokator : IInvokator
{
    private readonly Dispatcher _dispatcher;
    private readonly DispatcherPriority _priority;

    /// <summary>
    ///     Creates a new instance of <see cref="DispatcherBeginInvokator" />.
    /// </summary>
    public DispatcherBeginInvokator()
        : this(Dispatcher.CurrentDispatcher, DispatcherPriority.Normal)
    {
    }

    /// <summary>
    ///     Creates a new instance of <see cref="DispatcherBeginInvokator" />.
    /// </summary>
    /// <param name="dispatcher">The dispatcher to invoke the action on.</param>
    public DispatcherBeginInvokator(Dispatcher dispatcher)
        : this(dispatcher, DispatcherPriority.Normal)
    {
    }

    /// <summary>
    ///     Creates a new instance of <see cref="DispatcherBeginInvokator" />.
    /// </summary>
    /// <param name="priority">The priority of the dispatcher invoke.</param>
    public DispatcherBeginInvokator(DispatcherPriority priority)
        : this(Dispatcher.CurrentDispatcher, priority)
    {
    }

    /// <summary>
    ///     Creates a new instance of <see cref="DispatcherBeginInvokator" />.
    /// </summary>
    /// <param name="dispatcher">The dispatcher to invoke the action on.</param>
    /// <param name="priority">The priority of the dispatcher invoke.</param>
    public DispatcherBeginInvokator(Dispatcher dispatcher, DispatcherPriority priority)
    {
        _dispatcher = dispatcher;
        _priority = priority;
    }

    /// <summary>
    ///     Invokes an action.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    public void Invoke(Action action)
    {
        _dispatcher.BeginInvoke(action, _priority);
    }
}