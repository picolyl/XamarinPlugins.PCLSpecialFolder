﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

//-----------------------------------------------------------------------
// <copyright file="Requires.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
// This file is a derivation of:
// https://github.com/dsplaisted/PCLStorage/blob/master/src/PCLStorage.WinRT/AwaitExtensions.WinRT.cs
// Which is released under the Ms-PL license.
//-----------------------------------------------------------------------

namespace PCLStorage
{
    /// <summary>
    /// Extensions for use internally by PCLStorage for awaiting.
    /// </summary>
    internal static partial class AwaitExtensions
    {
        /// <summary>
        /// Returns a task that completes when the async operation completes,
        /// but never throws to the person awaiting it.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the async operation.</typeparam>
        /// <param name="operation">The async operation.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task whose result is the completed task.</returns>
        internal static Task<Task<T>> AsTaskNoThrow<T>(this IAsyncOperation<T> operation, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<Task<T>>();
            var task = operation.AsTask(cancellationToken);
            task.ContinueWith((t, state) => ((TaskCompletionSource<Task<T>>)state).SetResult(t), tcs);
            return tcs.Task;
        }
    }
}
