// 
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 

using System;

// ReSharper disable once CheckNamespace

namespace SniffCore.Collections
{
    /// <summary>
    ///     Provides a way how to invoke an action.
    /// </summary>
    public interface IInvokator
    {
        /// <summary>
        ///     Invokes an action.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        void Invoke(Action action);
    }
}