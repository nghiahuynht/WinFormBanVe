using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GM_DAL
{
    public class CommonHelper
    {
        #region Check Null
        public static object CheckDateNull(object readerValue)
        {
            if ((readerValue == DBNull.Value) || (readerValue == null))
            {
                return null;
            }
            else
            {
                return readerValue;
            }
        }
        public static int CheckIntNull(object readerValue)
        {
            if ((readerValue == DBNull.Value) || (readerValue == null))
            {
                //return -1;
                return 0;
            }
            else
            {
                return Convert.ToInt32(readerValue);
            }
        }
        public static long CheckLongNull(object readerValue)
        {
            if ((readerValue == DBNull.Value) || (readerValue == null))
            {
                //return -1;
                return 0;
            }
            else
            {
                return Convert.ToInt64(readerValue);
            }
        }
        public static string CheckStringNull(object readerValue)
        {
            if ((readerValue == DBNull.Value) || (string.IsNullOrEmpty(readerValue?.ToString())))
            {
                return "";
            }
            else
            {
                return Convert.ToString(readerValue);
            }
        }
        public static float CheckFloatNull(object readerValue)
        {
            if ((readerValue == DBNull.Value) || (readerValue == null))
            {
                return -1.0f;
            }
            else
            {
                return Convert.ToSingle(readerValue);
            }
        }
        public static bool CheckBooleanNull(object readerValue)
        {
            if ((readerValue == DBNull.Value) || (readerValue == null))
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(readerValue);
            }
        }

        public static string CheckObjectNull(Dictionary<string, Object> obj, string key)
        {
            if (obj.ContainsKey(key))
            {
                return obj[key].ToString();
            }
            else
                return "";
        }
        public static decimal CheckDecimalNull(object readerValue)
        {
            if ((readerValue == DBNull.Value) || (readerValue == null))
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(readerValue);
            }
        }

        // <summary>
        ///     Format số tiền hiển thị
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rightdigit"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static string FormatCurrency(object value, int rightdigit = 0, CultureInfo cultureInfo = null)
        {
            if (cultureInfo is null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("{0:");
            stringBuilder.Append("N");
            stringBuilder.Append(rightdigit);
            stringBuilder.Append("}");
            return string.Format(cultureInfo, stringBuilder.ToString(), value);
        }

        /// <summary>
        /// Lấy startdate and enddate theo interval + input date
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static (DateTime, DateTime) GetStartAndEndDate(string interval, DateTime date)
        {
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();

            switch (interval)
            {
                case "DAY":
                    StartDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    EndDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    break;

                case "WEEK":
                    DateTime _EndDateWeek = date;
                    DateTime _StartDateWeek = new DateTime();
                    GetDurationDayOfWeek(date, ref _StartDateWeek, ref _EndDateWeek);
                    StartDate = new DateTime(_StartDateWeek.Year, _StartDateWeek.Month, _StartDateWeek.Day, 0, 0, 0);
                    EndDate = new DateTime(_EndDateWeek.Year, _EndDateWeek.Month, _EndDateWeek.Day, 00, 00, 00);
                    break;

                case "MONTH":
                    DateTime _EndDateMonth = date;
                    GetDurationDayOfMonth(date, ref _EndDateMonth);
                    StartDate = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
                    EndDate = new DateTime(_EndDateMonth.Year, _EndDateMonth.Month, _EndDateMonth.Day, 23, 59, 59);
                    break;

                case "YEAR":
                    StartDate = new DateTime(date.Year, 1, 1, 0, 0, 0);
                    EndDate = new DateTime(date.Year, 12, 31, 00, 00, 00);
                    break;

                default:
                    break;
            }
            return (StartDate, EndDate);
        }
        /// <summary>
        ///  Lấy ngày trong tuần
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        public static void GetDurationDayOfWeek(DateTime Date, ref DateTime StartDate, ref DateTime EndDate)
        {
            string[] DayWeeks = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            string DayofWeek = Date.DayOfWeek.ToString();
            for (int i = 0; i < DayWeeks.Length; i++)
            {
                if (DayWeeks[i] == DayofWeek)
                {
                    int vt = i + 1;
                    EndDate = Date.AddDays(7 - vt);
                    StartDate = Date.AddDays(-1 * (vt - 1));
                    break;
                }
            }
        }

        /// <summary>
        /// Lấy ngày trong tháng
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        public static void GetDurationDayOfMonth(DateTime StartDate, ref DateTime EndDate)
        {
            int[] Month31 = { 1, 3, 5, 7, 8, 10, 12 };
            int[] Month30 = { 4, 6, 9, 11 };

            int FromMonth = StartDate.Month;

            if (Month31.Contains(FromMonth))
            {
                EndDate = new DateTime(StartDate.Year, StartDate.Month, 31, 23, 59, 59);
            }
            else if (Month30.Contains(FromMonth))
            {
                EndDate = new DateTime(StartDate.Year, StartDate.Month, 30, 23, 59, 59);
            }
            else if (((StartDate.Year % 4 == 0 && StartDate.Year % 100 != 0) || StartDate.Year % 400 == 0) && FromMonth == 2)
            {
                EndDate = new DateTime(StartDate.Year, StartDate.Month, 29, 23, 59, 59);
            }
            else
            {
                EndDate = new DateTime(StartDate.Year, StartDate.Month, 28, 23, 59, 59);
            }
        }
        #endregion
        /// <summary>
        /// Chuyển Tiếng việt sang Tieng Viet
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveUnicode(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        //    /// <summary>
        //    /// validate  > 0, true if value is a non-null number > 0, otherwise return false
        //    /// </summary>
        //    /// <param name="value"></param>
        //    /// <returns></returns>
        public static bool ValidateGreaterThanZero(object value)
        {
            // return true if value is a non-null number > 0, otherwise return false
            int i;
            return value != null && int.TryParse(value.ToString(), out i) && i > 0;
        }
        /// <summary>
        /// Định dạng lại phone (84********)
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string PhoneFormat(string phone)
        {
            string strPhone = phone;
            //Format phone
            Regex regexPhone = new Regex(@"^0\d{9}$");
            if (regexPhone.IsMatch(phone))
            {
                strPhone = $"84{phone.Substring(1, phone.Length - 1)}";
            }
            return strPhone;
        }
        /// <summary>
        /// Validate string null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ValidateStringNullOrEmpty(object value)
        {

            if ((value == DBNull.Value) || (string.IsNullOrEmpty(value?.ToString())))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Nối 2 mảng
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static List<T> ConcatLists<T>(List<List<T>> myList)
        {
            if (myList.All(list => list == null))
            {
                return new List<T>();
            }
            else
            {
                return myList.Where(list => list != null).SelectMany(list => list).ToList();
            }

        }
    }
}
