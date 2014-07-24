//-----------------------------------------------------------------------
// <copyright file="ScxStatistics.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>5/13/2009 11:09:42 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;

    /// <summary>
    /// Wrapper class to hold static methods and delegates
    /// </summary>
    public class ScxStatistics
    {
        /// <summary>
        /// Return the square of the value
        /// </summary>
        /// <param name="value">A value to be squared</param>
        /// <returns>The square of the value</returns>
        public static double Square(double value)
        {
            return value * value;
        }

        /// <summary>
        /// Return the mean (average) of the samples
        /// </summary>
        /// <param name="data">An array of samples</param>
        /// <returns>The statistical mean (average)</returns>
        public static double Mean(double[] data)
        {
            if (data.Length <= 0)
            {
                return 0;
            }

            double sum = 0;

            foreach (double d in data)
            {
                sum += d;
            }

            return sum / data.Length;
        }

        /// <summary>
        /// Return the statistical variance of the samples
        /// </summary>
        /// <param name="data">An array of samples</param>
        /// <returns>The variance</returns>
        public static double Variance(double[] data)
        {
            if (data.Length <= 0)
            {
                return 0;
            }

            double mean = ScxStatistics.Mean(data);
            double sumSquares = 0;

            foreach (double d in data)
            {
                sumSquares += ScxStatistics.Square(d - mean);
            }

            return sumSquares / data.Length;
        }

        /// <summary>
        /// Return the statisitcal standard deviation
        /// </summary>
        /// <param name="data">An array of samples</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation(double[] data)
        {
            double variance = ScxStatistics.Variance(data);

            return Math.Sqrt(variance);
        }
    }
}