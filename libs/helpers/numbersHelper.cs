using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys
{
    /// <summary>
    /// Правки для числових типів данних
    /// </summary>
    public class numbersHelper
    {
        /// <summary>
        /// Культура для конвертації
        /// </summary>
        public static readonly CultureInfo LocalCulture = new CultureInfo("uk-UA");

        /// <summary>
        /// Максимальне число знаків після коми для встановлення курсів
        /// </su
        public static int MaxRatePrecision
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        /// Нормалізація Double змінної
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defPrecision">Базовая точність заокруглення</param>
        /// <returns></returns>
        public static double DoubleNormalize(double value, int defPrecision = 10)
        {
            double res;
            if (double.TryParse(Convert.ToString(value, LocalCulture).Replace(".", ","), NumberStyles.Any, LocalCulture, out res))
            {
                return Math.Round(res, defPrecision);
            }
            return 0;
        }

        /// <summary>
        /// Нормалізація Double змінної для суми
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double DoubleNormalizeSum(double value)
        {
            return DoubleNormalize(value, 2);
        }

        /// <summary>
        /// Нормалізація Double змінної
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double DoubleNormalize(object value)
        {
            if (value == null)
            {
                throw new Exception("numbersHelper.DoubleNormalize: object is null");
            }
            double res;
            if (double.TryParse(Convert.ToString(value, LocalCulture).Replace(".", ","), NumberStyles.Any, LocalCulture, out res))
            {
                return res;
            }
            return 0;
        }

        /// <summary>
        /// Отримати суму операції
        /// </summary>
        /// <param name="sum">Сума операції</param>
        /// <param name="rate">Курс операції</param>
        /// <param name="defPrecision">Базовая точність заокруглення</param>
        /// <param name="invertDivMul">Інвертувати Множення/Ділення<para>Якщо значення averageRateForIncome = 0</para></param>
        /// <param name="averageRateForIncome">Середньозважений курс<para>Для отримання суми дохіду</para></param>
        /// <returns></returns>
        public static double GetOperationSum(double sum, double rate, int defPrecision = 10, bool invertDivMul = false, double? averageRateForIncome = null)
        {
            if (averageRateForIncome != null && averageRateForIncome.Value - rate == 0)
            {
                return 0;
            }

            if (averageRateForIncome == null)
            {
                return DoubleNormalize(!invertDivMul ? (sum * rate) : (sum / rate), defPrecision);
            }
            return DoubleNormalize(sum * (rate - averageRateForIncome.Value), defPrecision);
        }

        /// <summary>
        /// Отримати суму операції
        /// </summary>
        /// <param name="sum">Сума операції</param>
        /// <param name="sumInBaseCurr">Сума операцыъ в базовый валюты</param>
        /// <param name="defPrecision">Базовая точність заокруглення</param>
        /// <returns></returns>
        public static double GetAverageRate(double sum, double sumInBaseCurr, int defPrecision = 10)
        {
            if (sum == 0 || sumInBaseCurr == 0)
            {
                return 0;
            }

            return DoubleNormalize(sumInBaseCurr / sum, defPrecision);
        }
    }
}
