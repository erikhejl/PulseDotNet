using System.Collections.Generic;

namespace PulseDotNet.Collection
{
    /// <summary>
    /// Transforms a collection of objects into a collection of another type.
    /// </summary>
    /// <typeparam name="TSource">Source data type</typeparam>
    /// <typeparam name="TTarget">Transformed data type</typeparam>
    public interface IEnumerableTransform<TSource, TTarget>
    {
        /// <summary>
        /// Execute transformation on a collection of data.
        /// </summary>
        /// <param name="source">Collection to be transformed</param>
        /// <returns>Transformed data</returns>
        IEnumerable<TTarget> Transform(IEnumerable<TSource> source);
    }
}