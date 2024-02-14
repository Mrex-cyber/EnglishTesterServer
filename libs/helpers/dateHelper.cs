using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys
{
    public interface DateFilter
    {
        DateTime? dateStart { get; set; }
        DateTime? dateEnd { get; set; }
    }

    /// <summary>
    /// Обработка дат
    /// </summary>
    public class dateHelper
    {
        /// <summary>
        /// Формат для дат на Рест методи
        /// </summary>
        public static readonly string MainDateFormat = "dd'.'MM'.'yyyy'T'HH':'mm':'ss";

        /// <summary>
        /// Нормалізувати дати для пошуку
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        public static void NormalizeDateForSearch(ref DateTime? dateFrom, ref DateTime? dateTo)
        {
            if (dateFrom != null && dateTo != null)
            {
                dateFrom = dateFrom.Value.Date;
                dateTo = dateTo.Value.Date;

                if (dateFrom > dateTo)
                {
                    var buff = dateFrom;
                    dateFrom = dateTo;
                    dateTo = buff;
                }

                dateTo = dateTo.Value.AddDays(1).AddSeconds(-1);
            }
            else
            {
                dateFrom = null;
                dateTo = null;
            }
        }

        public static void NormalizeDateForSearch(DateFilter filter)
        {
            if (filter.dateStart != null)
            {
                filter.dateStart = filter.dateStart.Value.Date;
            }

            if(filter.dateEnd != null)
            {
                filter.dateEnd = filter.dateEnd.Value.Date;
                filter.dateEnd = filter.dateEnd.Value.AddDays(1).AddSeconds(-1);
            }

            if (filter.dateStart != null && filter.dateEnd != null)
            {
                if (filter.dateStart > filter.dateEnd)
                {
                    var buff = filter.dateStart;
                    filter.dateStart = filter.dateEnd;
                    filter.dateEnd = buff;
                }
            }
        }

        /// <summary>
        /// Нормалізувати дати для пошуку
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        public static void NormalizeDateForSearch(ref DateTime dateFrom, ref DateTime dateTo)
        {
            dateFrom = dateFrom.Date;
            dateTo = dateTo.Date;

            if (dateFrom > dateTo)
            {
                var buff = dateFrom;
                dateFrom = dateTo;
                dateTo = buff;
            }

            dateTo = dateTo.AddDays(1).AddSeconds(-1);
        }

        public static void NormalizeDateForSearch_V2(ref DateTime? dateFrom, ref DateTime? dateTo)
        {
            if (dateFrom != null && dateTo != null)
            {
                dateFrom = ValidateForSql(dateFrom.Value).Date;
                dateTo = ValidateForSql(dateTo.Value).Date;

                if (dateFrom > dateTo)
                {
                    var buff = dateFrom;
                    dateFrom = dateTo;
                    dateTo = buff;
                }

                //dateTo = dateTo.Value.AddDays(1).AddSeconds(-1);
                dateTo = new DateTime(dateTo.Value.Year, dateTo.Value.Month, dateTo.Value.Day, 23, 59, 59, 990);
            }
            else if (dateFrom != null)
            {
                dateFrom = dateFrom.Value.Date;
            }
            else if (dateTo != null)
            {
                //dateTo = dateTo.Value.Date.AddDays(1).AddSeconds(-1);

                dateTo = new DateTime(dateTo.Value.Year, dateTo.Value.Month, dateTo.Value.Day, 23, 59, 59, 990);
            }
        }

        public static void NormalizeDateForSearch_V2(ref DateTime dateFrom, ref DateTime dateTo)
        {
            if (dateFrom != null && dateTo != null)
            {
                dateFrom = ValidateForSql(dateFrom).Date;
                dateTo = ValidateForSql(dateTo).Date;

                if (dateFrom > dateTo)
                {
                    var buff = dateFrom;
                    dateFrom = dateTo;
                    dateTo = buff;
                }

                //dateTo = dateTo.Value.AddDays(1).AddSeconds(-1);
                dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59, 990);
            }
            else if (dateFrom != null)
            {
                dateFrom = dateFrom.Date;
            }
            else if (dateTo != null)
            {
                //dateTo = dateTo.Value.Date.AddDays(1).AddSeconds(-1);

                dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59, 990);
            }
        }

        public static DateTime _MinSql = new DateTime(1753, 1, 1);
        public static DateTime _MaxSql = new DateTime(9999, 12, 31);
        private static DateTime ValidateForSql(DateTime date)
        {
            if (date < _MinSql)
            {
                return _MinSql;
            }
            else if (date > _MaxSql)
            {
                return _MaxSql;
            }
            return date;
        }
    }
}
