// 
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 

using System;
using System.Windows.Threading;

// ReSharper disable once CheckNamespace

namespace SniffCore.Collections;

/// <summary>
///     Invokes an action using a <see cref="Dispatcher.Invoke(System.Action, DispatcherPriority)" />.
/// </summary>
public sealed class DispatcherInvokator : IInvokator
{
    private readonly Dispatcher _dispatcher;
    private readonly DispatcherPriority _priority;

    /// <summary>
    ///     Creates a new instance of <see cref="DispatcherInvokator" />.
    /// </summary>
    public DispatcherInvokator()
        : this(Dispatcher.CurrentDispatcher, DispatcherPriority.Normal)
    {
    }

    /// <summary>
    ///     Creates a new instance of <see cref="DispatcherInvokator" />.
    /// </summary>
    /// <param name="dispatcher">The dispatcher to invoke the action on.</param>
    public DispatcherInvokator(Dispatcher dispatcher)
        : this(dispatcher, DispatcherPriority.Normal)
    {
    }

    /// <summary>
    ///     Creates a new instance of <see cref="DispatcherInvokator" />.
    /// </summary>
    /// <param name="priority">The priority of the dispatcher invoke.</param>
    public DispatcherInvokator(DispatcherPriority priority)
        : this(Dispatcher.CurrentDispatcher, priority)
    {
    }

    /// <summary>
    ///     Creates a new instance of <see cref="DispatcherInvokator" />.
    /// </summary>
    /// <param name="dispatcher">The dispatcher to invoke the action on.</param>
    /// <param name="priority">The priority of the dispatcher invoke.</param>
    public DispatcherInvokator(Dispatcher dispatcher, DispatcherPriority priority)
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
        _dispatcher.Invoke(action, _priority);
    }
}