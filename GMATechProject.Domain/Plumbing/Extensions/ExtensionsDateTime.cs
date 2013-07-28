namespace GMATechProject.Domain
{
    #region Using Directives

    using System;
    using System.Globalization;

    #endregion

    /// <summary>
    /// Extension for DateTime
    /// </summary>
    public static class ExtensionsDateTime
    {
        #region Elapsed

        /// <summary>
        /// Get the duration between today and the specified date.
        /// </summary>
        /// <param name="datetime">The datetime to substract from today.</param>
        /// <returns>The duration between today and <paramref name="datetime"/>.</returns>
        public static TimeSpan Elapsed(this DateTime datetime)
        {
            return DateTime.UtcNow - datetime;
        }

        #endregion

        #region Week of year

        /// <summary>
        /// Get the number of the week from the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="weekRule">The week rule.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns>The number of the week.</returns>
        public static int WeekOfYear(this DateTime dateTime, CalendarWeekRule weekRule, DayOfWeek firstDayOfWeek)
        {
            var currentCulture = CultureInfo.CurrentCulture;

            return currentCulture.Calendar.GetWeekOfYear(dateTime, weekRule, firstDayOfWeek);
        }

        /// <summary>
        /// Get the number of the week from the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns>The number of the week.</returns>
        public static int WeekOfYear(this DateTime dateTime, DayOfWeek firstDayOfWeek)
        {
            var dateinf = CultureInfo.CurrentCulture.DateTimeFormat;
            var weekRule = dateinf.CalendarWeekRule;

            return WeekOfYear(dateTime, weekRule, firstDayOfWeek);
        }

        /// <summary>
        /// Get the number of the week from the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="weekRule">The week rule.</param>
        /// <returns>The number of the week.</returns>
        public static int WeekOfYear(this DateTime dateTime, CalendarWeekRule weekRule)
        {
            var dateinf = CultureInfo.CurrentCulture.DateTimeFormat;
            var firstDayOfWeek = dateinf.FirstDayOfWeek;

            return WeekOfYear(dateTime, weekRule, firstDayOfWeek);
        }

        /// <summary>
        /// Get the number of the week from the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The number of the week.</returns>
        public static int WeekOfYear(this DateTime dateTime)
        {
            var dateinf = CultureInfo.CurrentCulture.DateTimeFormat;
            var weekrule = dateinf.CalendarWeekRule;
            var firstDayOfWeek = dateinf.FirstDayOfWeek;
            
            return WeekOfYear(dateTime, weekrule, firstDayOfWeek);
        }
        
        #endregion

        #region Get Datetime for Day of Week
        
        /// <summary>
        /// Gets the date time for the specified day of week.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="day">The day.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns>The date time for the specified day of week</returns>
        public static DateTime GetDateTimeForDayOfWeek(this DateTime dateTime, DayOfWeek day, DayOfWeek firstDayOfWeek)
        {
            int current = DaysFromFirstDayOfWeek(dateTime.DayOfWeek, firstDayOfWeek);
            int resultday = DaysFromFirstDayOfWeek(day, firstDayOfWeek);
            return dateTime.AddDays(resultday - current);
        }
        
        /// <summary>
        /// Gets the date time for the specified day of week.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="day">The day.</param>
        /// <returns>The date time for the specified day of week</returns>
        public static DateTime GetDateTimeForDayOfWeek(this DateTime dateTime, DayOfWeek day)
        {
            var dateinf = CultureInfo.CurrentCulture.DateTimeFormat;
            var firstDayOfWeek = dateinf.FirstDayOfWeek;
            return GetDateTimeForDayOfWeek(dateTime, day, firstDayOfWeek);
        }
        
        /// <summary>
        /// Get the first date time for the week.
        /// </summary>
        /// <param name="dateTime">The datetime.</param>
        /// <returns>The first date time for the week.</returns>
        public static DateTime FirstDateTimeOfWeek(this DateTime dateTime)
        {
            var dateinf = CultureInfo.CurrentCulture.DateTimeFormat;
            var firstDayOfWeek = dateinf.FirstDayOfWeek;
            return FirstDateTimeOfWeek(dateTime, firstDayOfWeek);
        }
        
        /// <summary>
        /// Firsts the date time of week.
        /// </summary>
        /// <param name="dateTime">The datetime.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns>The first date time for the week.</returns>
        public static DateTime FirstDateTimeOfWeek(this DateTime dateTime, DayOfWeek firstDayOfWeek)
        {
            return dateTime.AddDays(-DaysFromFirstDayOfWeek(dateTime.DayOfWeek, firstDayOfWeek));
        }

        /// <summary>
        /// Get the number of days between the first day of the week and the day of the specified date.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        private static int DaysFromFirstDayOfWeek(DayOfWeek current, DayOfWeek firstDayOfWeek)
        {
            //Sunday = 0,Monday = 1,...,Saturday = 6
            int daysbetween = current - firstDayOfWeek;
            if (daysbetween < 0) daysbetween = 7 + daysbetween;
            return daysbetween;
        }

        #endregion

        #region GetValueOrDefaultToString

        /// <summary>
        /// Gets the value or default to string.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static string GetValueOrDefaultToString(this DateTime? datetime, string defaultvalue)
        {
            // By default, the result is the specified default value
            var result = defaultvalue;

            // If the datetime is not null
            if (datetime != null)
            {
                // The resut become this date string representation
                result = datetime.Value.ToString();
            }

            return result;
        }

        /// <summary>
        /// Gets the value or default to string.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="format">The format.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static string GetValueOrDefaultToString(this DateTime? datetime, string format, string defaultvalue)
        {
            // By default, the result is the specified default value
            var result = defaultvalue;

            // If the datetime is not null
            if (datetime != null)
            {
                // The resut become this date string representation in the specified format
                result = datetime.Value.ToString(format);
            }
            return result;
        }

        #endregion
    }
}
