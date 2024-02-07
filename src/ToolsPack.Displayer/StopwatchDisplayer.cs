using System.Globalization;
using System.Diagnostics;
using System;

namespace ToolsPack.Displayer
{
    /// <summary>
    ///convert Elapse time of a stopwatch to String (used in log message)
    ///This class must not depend on anything else, 
    ///and can be move to anywhere (CommonServices for example)
    /// </summary>
    [Obsolete("Use ToolsPack.String instead. This class is moved to ToolsPack.String.StopwatchDisplayer")]
    public static class StopwatchDisplayer
    {
        public enum TimeUnit
        {
            MiliSecond,
            MicroSecond,
            Second
        }

        /// <summary>
        /// Use in log message, to display nicely the TimeSpan
        /// </summary>
        public static string Display(this Stopwatch sw)
        {
            if (sw == null) throw new ArgumentNullException(nameof(sw));
            double r = (double)sw.ElapsedTicks / Stopwatch.Frequency;
            return GetElapsedString(r);
        }

        /// <summary>
        /// Use in log message, to display nicely the TimeSpan in mili second
        /// </summary>
        public static string DisplayMili(this Stopwatch sw)
        {
            if (sw == null) throw new ArgumentNullException(nameof(sw));
            double r = (double)sw.ElapsedTicks / Stopwatch.Frequency;
            return GetElapsedString(r, TimeUnit.MiliSecond);
        }

        /// <summary>
        /// Use in log message, to display nicely the TimeSpan in micro second
        /// </summary>
        public static string DisplayMicro(this Stopwatch sw)
        {
            if (sw == null) throw new ArgumentNullException(nameof(sw));
            double r = (double)sw.ElapsedTicks / Stopwatch.Frequency;
            return GetElapsedString(r, TimeUnit.MicroSecond);
        }

        /// <summary>
        ///  ArrayFormat elapsed time to a nice format
        ///  	if (forceTimeUnit is null)
        ///  		the timeInSecond will be formated to min, ms, microSec or nanoSec base on its value
        ///  	otherwise
        ///  		it will display the timeInSecond in the forceTimeUnit
        /// </summary>
        public static string GetElapsedString(double timeInSecond, TimeUnit? forceTimeUnit = null)
        {
            if (forceTimeUnit.HasValue)
            {
                switch (forceTimeUnit)
                {
                    case TimeUnit.MicroSecond:
                        return (timeInSecond * 1000000.0).ToString("0", CultureInfo.InvariantCulture) + " mcs";
                    case TimeUnit.Second:
                        return (timeInSecond).ToString("0.##", CultureInfo.InvariantCulture) + " s";
                    default:
                        return (timeInSecond * 1000.0).ToString("0.##", CultureInfo.InvariantCulture) + " ms";
                }
            }
            if (timeInSecond >= 60)
            {
                return (timeInSecond / 60.0).ToString("0.#", CultureInfo.InvariantCulture) + " min";
            }
            if (timeInSecond >= 1)
            {
                return timeInSecond.ToString("0.#", CultureInfo.InvariantCulture) + " s";
            }
            if (timeInSecond >= 0.001)
            {
                return (timeInSecond * 1000.0).ToString("0", CultureInfo.InvariantCulture) + " ms";
            }
            if (timeInSecond >= 0.000001)
            {
                return (timeInSecond * 1000000.0).ToString("0", CultureInfo.InvariantCulture) + " mcs";
            }
            return (timeInSecond * 1000000000.0).ToString("0", CultureInfo.InvariantCulture) + " ns";
        }
    }
}
