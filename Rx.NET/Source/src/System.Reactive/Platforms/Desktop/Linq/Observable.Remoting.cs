﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if HAS_REMOTING
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Provides a set of static methods for exposing observable sequences through .NET Remoting.
    /// </summary>
    public static partial class RemotingObservable
    {
        #region Remotable

        /// <summary>
        /// Makes an observable sequence remotable, using an infinite lease for the <see cref="MarshalByRefObject"/> wrapping the source.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The observable sequence that supports remote subscriptions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Remotable", Justification = "In honor of the .NET Remoting heroes.")]
        public static IObservable<TSource> Remotable<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Remotable_(source);
        }

        /// <summary>
        /// Makes an observable sequence remotable, using a controllable lease for the <see cref="MarshalByRefObject"/> wrapping the source.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="lease">Lease object to control lifetime of the remotable sequence. Notice null is a supported value.</param>
        /// <returns>The observable sequence that supports remote subscriptions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Remotable", Justification = "In honor of the .NET Remoting heroes.")]
        public static IObservable<TSource> Remotable<TSource>(this IObservable<TSource> source, ILease lease)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Remotable_(source, lease);
        }

        /// <summary>
        /// Makes an observable sequence remotable, using an infinite lease for the <see cref="MarshalByRefObject"/> wrapping the source.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The observable sequence that supports remote subscriptions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Remotable", Justification = "In honor of the .NET Remoting heroes.")]
        public static IQbservable<TSource> Remotable<TSource>(this IQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
#if CRIPPLED_REFLECTION
                    InfoOf(() => RemotingObservable.Remotable<TSource>(default(IQbservable<TSource>))),
#else
#pragma warning disable IL2060 // ('System.Reflection.MethodInfo.MakeGenericMethod' can not be statically analyzed.) This gets the MethodInfo for the method running right now, so it can't have been trimmed
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
#pragma warning restore IL2060
#endif
                    source.Expression
                )
            );
        }

        /// <summary>
        /// Makes an observable sequence remotable, using a controllable lease for the <see cref="MarshalByRefObject"/> wrapping the source.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="lease">Lease object to control lifetime of the remotable sequence. Notice null is a supported value.</param>
        /// <returns>The observable sequence that supports remote subscriptions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Remotable", Justification = "In honor of the .NET Remoting heroes.")]
        public static IQbservable<TSource> Remotable<TSource>(this IQbservable<TSource> source, ILease lease)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
#if CRIPPLED_REFLECTION
                    InfoOf(() => RemotingObservable.Remotable<TSource>(default(IQbservable<TSource>), default(ILease))),
#else
#pragma warning disable IL2060 // ('System.Reflection.MethodInfo.MakeGenericMethod' can not be statically analyzed.) This gets the MethodInfo for the method running right now, so it can't have been trimmed
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
#pragma warning restore IL2060
#endif
                    source.Expression,
                    Expression.Constant(lease, typeof(ILease))
                )
            );
        }

#if CRIPPLED_REFLECTION
        internal static MethodInfo InfoOf<R>(Expression<Func<R>> f)
        {
            return ((MethodCallExpression)f.Body).Method;
        }
#endif

        #endregion
    }
}
#endif
