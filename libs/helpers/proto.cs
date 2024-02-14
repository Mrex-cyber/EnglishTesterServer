using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class proto
{
    public readonly static System.Net.SecurityProtocolType DefSecurityProtocol = System.Net.SecurityProtocolType.Tls12;
    private readonly static Dictionary<char, string> convertedLetters = new Dictionary<char, string>
    {
        {' ', "_"},
        {'!', "_"},
        {'₴', "_"},
        {';', "_"},
        {'%', "_"},
        {':', "_"},
        {'?', "_"},
        {'*', "_"},
        {'(', "_"},
        {')', "_"},
        {'-', "_"},
        {'+', "_"},
        {'=', "_"},
        {'~', "_"},
        {'@', "_"},
        {'#', "_"},
        {'$', "_"},
        {'^', "_"},
        {'&', "_"},
        {'}', "_"},
        {'{', "_"},
        {'а', "a"},
        {'б', "b"},
        {'в', "v"},
        {'г', "g"},
        {'д', "d"},
        {'е', "e"},
        {'ё', "yo"},
        {'ж', "zh"},
        {'з', "z"},
        {'и', "i"},
        {'й', "j"},
        {'к', "k"},
        {'л', "l"},
        {'м', "m"},
        {'н', "n"},
        {'о', "o"},
        {'п', "p"},
        {'р', "r"},
        {'с', "s"},
        {'т', "t"},
        {'у', "u"},
        {'ф', "f"},
        {'х', "h"},
        {'ц', "c"},
        {'ч', "ch"},
        {'ш', "sh"},
        {'щ', "sch"},
        {'ъ', "j"},
        {'ы', "i"},
        {'ь', "j"},
        {'э', "e"},
        {'ю', "yu"},
        {'я', "ya"},
        {'А', "A"},
        {'Б', "B"},
        {'В', "V"},
        {'Г', "G"},
        {'Д', "D"},
        {'Е', "E"},
        {'Ё', "Yo"},
        {'Ж', "Zh"},
        {'З', "Z"},
        {'И', "I"},
        {'Й', "J"},
        {'К', "K"},
        {'Л', "L"},
        {'М', "M"},
        {'Н', "N"},
        {'О', "O"},
        {'П', "P"},
        {'Р', "R"},
        {'С', "S"},
        {'Т', "T"},
        {'У', "U"},
        {'Ф', "F"},
        {'Х', "H"},
        {'Ц', "C"},
        {'Ч', "Ch"},
        {'Ш', "Sh"},
        {'Щ', "Sch"},
        {'Ъ', "J"},
        {'Ы', "I"},
        {'Ь', "J"},
        {'Э', "E"},
        {'Ю', "Yu"},
        {'Я', "Ya"}
    };

    public static string ReverseStr(this string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    /// <summary>
    /// Згенерувати строку
    /// </summary>
    /// <param name="s"></param>
    /// <param name="length">Довжина</param>
    /// <param name="lower">Використовувати анг. літери нижній регістр</param>
    /// <param name="upper">Використовувати анг. літери верхній регістр</param>
    /// <param name="numbers">Використовувати числа</param>
    /// <returns></returns>
    public static String GenerateRandomString(this string s, byte length, bool lower = true, bool upper = true, bool numbers = true)
    {
        if (length == 0)
        {
            return "";
        }

        var valid = "";
        if (lower)
        {
            valid = string.Concat(valid, "abcdefghijklmnopqrstuvwxyz");
        }
        if (upper)
        {
            valid = string.Concat(valid, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        }
        if (numbers)
        {
            valid = string.Concat(valid, "1234567890");
        }

        if (string.IsNullOrEmpty(valid))
        {
            return "";
        }

        StringBuilder res = new StringBuilder();
        Random rnd = new Random();

        while (0 < length--)
        {
            res.Append(valid[rnd.Next(valid.Length)]);
        }
        return res.ToString();
    }

    /// <summary>
    /// Таймстемп від дати
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double GetTimeSpan(this DateTime value)
    {
        DateTime minDate = new DateTime(1970, 1, 9, 0, 0, 00);

        TimeSpan myDateResult;

        myDateResult = value - minDate;
        return myDateResult.TotalSeconds;
    }

    static bool IsBasicLetter(char c)
    {
        if (Char.IsDigit(c))
        {
            return true;
        }

        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }
    /// <summary>
    /// Сформує +/- валідний текстовий ключ
    /// </summary>
    /// <param name="slug"></param>
    /// <returns></returns>
    public static string ToNormalSlug(this string slug)
    {
        slug = slug.ToLower().Trim().Replace(" ", "_").Trim();

        var result = new StringBuilder();
        foreach (var letter in slug)
        {
            if (letter == '_') { result.Append(letter); }
            else
            {

                if (IsBasicLetter(letter))
                {
                    result.Append(letter);
                }
                else
                {
                    if (convertedLetters.ContainsKey(letter))
                    {
                        result.Append(convertedLetters[letter]);
                    }
                    else
                    {
                        result.Append("random".GenerateRandomString(1, true, false, false));
                    }
                }
            }
        }

        return result.ToString().ToLower();
    }
    public static String AsDBFloat(this String s)
    {
        if (s == null) return "0";
        if (s.Trim() == "") return "0";
        return s.Trim().Replace(".", ",");
    }

    /// <summary>
    /// Проверить слово на латинские символы
    /// </summary>
    /// <param name="s"></param>
    /// <returns>Вертнет true если в слове встречаются только латинские символы</returns>
    public static bool isOnlyLatin(this String s)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch(s, @"\p{IsCyrillic}"))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Перевірити чи валіжний JSON
    /// </summary>
    /// <param name="jsonString"></param>
    /// <returns></returns>
    public static bool IsValidJson(this String jsonString)
    {
        if (string.IsNullOrWhiteSpace(jsonString)) { return false; }
        jsonString = jsonString.Trim();
        if ((jsonString.StartsWith("{") && jsonString.EndsWith("}")) || //For object
            (jsonString.StartsWith("[") && jsonString.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JToken.Parse(jsonString);
                return true;
            }
            catch (JsonReaderException jex)
            {
                //Exception in parsing json
                Console.WriteLine(jex.Message);
                return false;
            }
            catch (Exception ex) //some other exception
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Список Int32 до DataTable, для структурних параметрів запитів по Іду
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static System.Data.DataTable ToDataTable(this IEnumerable<int> list, bool normilize = true)
    {
        var table = new System.Data.DataTable();

        table.Columns.Add(new System.Data.DataColumn("id", Type.GetType("System.Int32")));

        if (list == null)
        {
            list = new List<int>();
        }

        if (normilize)
        {
            if (list.Count() == 0 || list.Contains(-1))
            {
                list = new List<int> { -1 };
            }
            else
            {
                list = list.Distinct().ToList();
            }
        }

        foreach (var item in list)
        {
            var row = table.NewRow();

            row["id"] = item;

            table.Rows.Add(row);
        }

        return table;
    }

    /// <summary>
    /// Список Double до DataTable, для структурних параметрів запитів
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static System.Data.DataTable ToDataTable(this IEnumerable<double> list, bool normilize = true)
    {
        var table = new System.Data.DataTable();

        table.Columns.Add(new System.Data.DataColumn("val", Type.GetType("System.Double")));

        if (list == null || list.Count() == 0)
        {
            return table;
        }

        foreach (var item in list.Distinct())
        {
            var row = table.NewRow();

            row["val"] = item;

            table.Rows.Add(row);
        }

        return table;
    }

    public static System.Data.DataTable ToDataTable(this IEnumerable<Guid> list)
    {
        var table = new System.Data.DataTable();

        table.Columns.Add(new System.Data.DataColumn("value", typeof(Guid)));

        if (list == null)
        {
            list = new List<Guid>();
        }

        foreach (var item in list)
        {
            var row = table.NewRow();

            row["value"] = item;

            table.Rows.Add(row);
        }

        return table;
    }

    public static System.Data.DataTable ToDataTable(this IEnumerable<string> list)
    {
        var table = new System.Data.DataTable();

        table.Columns.Add(new System.Data.DataColumn("val", typeof(string)));

        if (list == null)
        {
            list = new List<string>();
        }

        foreach (var item in list)
        {
            var row = table.NewRow();

            row["val"] = item;

            table.Rows.Add(row);
        }

        return table;
    }

    public static void Swap<T>(ref T value, ref T secondValue)
    {
        var tmp = value;

        value = secondValue;
        secondValue = tmp;
    }

    /// <summary>
    /// Обрезать строку до maxLength символов
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    /// <summary>
    /// Проверить на корректность Email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool IsValidEmail(this string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Спробуйвати привести строку до числа з плаваючою комою
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string normilizeStringToDouble(this string input)
    {
        input = (input ?? "").Trim();
        if(string.IsNullOrEmpty(input))
        {
            return "";
        }

        StringBuilder sb = new StringBuilder();
        bool dotHasAppeared = false;

        for(int i = 0; i < input.Length; i++) 
        { 
            var ch = input[i];

            if (Char.IsDigit(ch))
            {
                sb.Append(ch);
            }
            else if(!dotHasAppeared && (ch == '.' || ch == ','))
            {
                dotHasAppeared = true;
                sb.Append(ch);
            }
        }

        if(sb.Length == 0 ) 
        {
            return "";
        }

        var res = sb.ToString();

        var lastChar = res[res.Length - 1];

        if (lastChar == '.' || lastChar == ',')
        {
            res = res.Substring(0, res.Length - 1);
        }

        return res;
    }

    /// <summary>
    /// Спробуйвати привести телефон до українського формату
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    public static string normalizeUAPhone(this string phone)
    {
        if (string.IsNullOrEmpty(phone))
        {
            return "";
        }

        phone = phone.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("+", "");

        int length = phone.Length;

        switch (length)
        {
            case 11:
                phone = "3" + phone;
                break;
            case 10:
                phone = "38" + phone;
                break;
            case 9:
                phone = "380" + phone;
                break;
        }

        if (phone.IndexOf("380") < 0 && phone.Length > 9)
        {
            return "error";
        }

        return phone;
    }

    public static string normalizeUAPhone_v2(this string phone)
    {
        if (string.IsNullOrEmpty(phone))
        {
            return "";
        }

        phone = phone.Trim();

        string copy = string.Copy(phone);

        copy = copy.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("+", "");

        int length = copy.Length;

        switch (length)
        {
            case 11:
                copy = "3" + copy;
                break;
            case 10:
                copy = "38" + copy;
                break;
            case 9:
                copy = "380" + copy;
                break;
        }

        if (copy.IndexOf("380") < 0 && copy.Length > 9)
        {
            return phone;
        }

        return copy;
    }


    public static bool isUaPhone(this string phone)
    {
        if (string.IsNullOrEmpty(phone))
        {
            return false;
        }

        string digits = Regex.Replace(phone, @"\D", "");
        if (digits.Length != 12 && digits.Length != 13)
        {
            return false;
        }
        return Regex.Match(digits, @"380(39|50|63|73|66|67|68|91|92|93|94|95|96|97|98|99)\d{7}$", RegexOptions.IgnoreCase).Success;
    }

    public static string TrimUAPhoneStart(this string phone)
    {
        if (phone.isUaPhone())
        {
            return phone.Substring(3);
        }
        else
        {
            return String.Copy(phone);
        }
    }

    /// <summary>
    /// Перевірка на наявність тільки чисел в строці
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsDigitsOnly(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return true;
        }

        foreach (char c in value)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }

        return true;
    }
    /// <summary>
    /// Перевірити чи строка може бути сконвертована в double|float
    /// </summary>
    /// <param name="valueToTest"></param>
    /// <returns></returns>
	public static bool IsDoubleRealNumber(this string valueToTest)
	{
		if (double.TryParse(valueToTest, out double d) && !Double.IsNaN(d) && !Double.IsInfinity(d))
		{
			return true;
		}

		return false;
	}

	public static String asHuMoneyFormat(this String s)
    {
        if (s == null) return "0";
        s = s.Trim().Replace(".", ",").Split(',')[0];

        if (s.Length <= 1) return s;

        string last = s.Substring(s.Length - 1, 1);
        string others = s.Substring(0, (s.Length - 1));

        if (last == "8")
            return (Convert.ToInt32(s) + 2).ToString();
        if (last == "9")
            return (Convert.ToInt32(s) + 1).ToString();

        if (last == "1" || last == "2")
        {
            return others + "0";
        }
        else if (last == "3" || last == "4" || last == "6" || last == "7")
        {
            return others + "5";
        }

        return s;
    }


    public static String AsJSFloat(this String s)
    {
        string res = s.Trim().Replace(",", ".");
        if (res.IndexOf('.') == -1)
            res = res + ".00";

        return res;
    }

    public static String AsCleanPhone(this String s)
    {

        string res = s.Replace(" ", "").Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Trim();
        return res;
    }

    public static String AsJSMoney(this String s)
    {
        if (s.Length == 0) return "00.00";

        string res = Math.Round(Convert.ToDouble(s.Replace(".", ",")), 2).ToString().Trim().Replace(",", ".");
        if (res.IndexOf('.') == -1)
            res = res + ".00";

        return res;

    }

    public static String AsInteger(this String s)
    {
        if (s.Length == 0) return "0";

        string res = Math.Round(Convert.ToDouble(s.Replace(".", ",")), 0).ToString().Trim().Replace(",", ".");

        return res;

    }


    public static bool isDateTime(this String date)
    {
        DateTime dateOut = new DateTime();

        return DateTime.TryParse(date, out dateOut);
    }

    /// <summary>
    /// return date from datetime string
    /// </summary>
    /// <param name="s">string datetime</param>
    /// <returns>date string</returns>
    public static String AsJustDate(this String s)
    {
        if (s.isDateTime())
            return Convert.ToDateTime(s).ToShortDateString();
        return s;
    }


    /// <summary>
    /// returt time from datetime string
    /// </summary>
    /// <param name="s">string datetime</param>
    /// <returns>time string</returns>
    public static String AsJustTime(this String s)
    {
        if (s.isDateTime())
            return Convert.ToDateTime(s).ToShortTimeString();
        return s;
    }

    public static long GetStableInt64HashCode(this String text)
    {
        Int64 hashCode = 0;
        if (!string.IsNullOrEmpty(text))
        {
            byte[] byteContents = Encoding.Unicode.GetBytes(text);
            System.Security.Cryptography.SHA256 hash =
            new System.Security.Cryptography.SHA256CryptoServiceProvider();
            byte[] hashText = hash.ComputeHash(byteContents);
            Int64 hashCodeStart = BitConverter.ToInt64(hashText, 0);
            Int64 hashCodeMedium = BitConverter.ToInt64(hashText, 8);
            Int64 hashCodeEnd = BitConverter.ToInt64(hashText, 24);
            hashCode = hashCodeStart ^ hashCodeMedium ^ hashCodeEnd;
        }
        return (hashCode);
    }

    public static string SHA256(this string rawData)
    {
        using (System.Security.Cryptography.SHA256 sha256Hash = System.Security.Cryptography.SHA256.Create())
        {
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

    }

    public static string GetDeviceIdFromInfo(this string raw)
    {
        if (string.IsNullOrEmpty(raw))
        {
            return "";
        }

        raw = (raw ?? "").Trim();

        try
        {
            var obj = JToken.Parse(raw);

            return obj.Value<string?>("IMAI") ?? raw;
        }
        catch (Exception ex)
        {
            return raw;
        }
    }


    /// <summary>
    /// Стабильное (одинаковое на всех машинах) значение int32 hash
    /// </summary>
    /// <param name="str">Строка</param>
    /// <returns>значение хеша</returns>
    public static int GetStableInt32HashCode(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return -1;

        unchecked
        {
            int hash1 = 5381;
            int hash2 = hash1;

            for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ str[i];
                if (i == str.Length - 1 || str[i + 1] == '\0')
                    break;
                hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }
    }

    /// <summary>
    /// Преобразование значение DateTime в int64 UnixTimestamp
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public static long ToUnixTimestamp(this DateTime d)
    {
        var epoch = d - new DateTime(1970, 1, 1, 0, 0, 0);

        return (long)epoch.TotalSeconds;
    }

    public static string Format(this DateTime d)
    {
        return d.ToString("yyyy-MM-ddTHH:mm:ss");
    }

    /// <summary>
    /// Преобразовываем unixTimeStamp в DateTime
    /// </summary>
    /// <param name="unixTimeStamp"></param>
    /// <returns></returns>
    public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
    {
        try
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        catch { return DateTime.Now; }
    }

    public static string NormalizeString(this string input)
    {
        return (input ?? string.Empty).Trim().ToLower();
    }

    public static List<List<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }

    public static String ToFixCharSize(this Double sum, int fixCount)
    {
        var s = sum.ToString();

        if (s.Length >= fixCount)
        {
            return s;
        }

        if (sum % 1 == 0)
        {
            s += ",0";
        }

        try
        {
            for (int i = 0; i < fixCount + 1 - s.Length; i++)
            {
                s += '0';
            }
        }
        catch { }

        return s;
    }

    public static String NumberWithSpaces(this Double sum)
    {
        var parts = sum.ToString().Replace(",", ".").Split('.');
        parts[0] = Regex.Replace(parts[0], @"\B(?=(\d{3})+(?!\d))", " ");
        return String.Join(".", parts);
    }

    /// <summary>
    /// Отримати падіж тексту по к-сті
    /// </summary>
    /// <param name="count">К-сть</param>
    /// <param name="one">1 (монітор)</param>
    /// <param name="two">2 (монітора)</param>
    /// <param name="five">5 (моніторов)</param>
    /// <returns></returns>
    public static String GetNoun(this int count, string one, string two, string five)
    {
        var n = Math.Abs(count);
        n %= 100;

        if (n >= 5 && n <= 20)
        {
            return five;
        }

        n %= 10;

        if (n == 1)
        {
            return one;
        }

        if (n >= 2 && n <= 4)
        {
            return two;
        }

        return five;
    }

    /// <summary>
    /// Розширити модель
    /// </summary>
    /// <typeparam name="T">Будь-яка модель</typeparam>
    /// <param name="model">Модель, яку потрібно розширити</param>
    /// <param name="values">Поля на які потрібно розширити</param>
    /// <returns></returns>
    public static System.Dynamic.ExpandoObject ExtendModel<T>(T model, Dictionary<string, string> values)
    {
        if (model == null
            || (model is System.Collections.IList && model.GetType().IsGenericType
                && model.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>))))
        {
            return null;
        }

        var expando = new System.Dynamic.ExpandoObject();
        var dictionary = (IDictionary<string, object>)expando;
        HashSet<string> hashset = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var property in model.GetType().GetProperties())
        {
            if (hashset.Contains(property.Name))
            {
                continue;
            }

            hashset.Add(property.Name);
            dictionary.Add(property.Name, property.GetValue(model));
        }

        foreach (var value in values)
        {
            if (hashset.Contains(value.Key))
            {
                continue;
            }

            hashset.Add(value.Key);
            dictionary.Add(value.Key, value.Value);
        }

        return expando;
    }
}

namespace Prototypes
{
    /// <summary>
    /// Списки об'єктів
    /// </summary>
    public class ItemsList
    {
        /// <summary>
        /// Загальна кількість
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// Закальна кількість сторінок
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// Сторінка з результатом
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// Кільскість елементів на сторінці
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Список з об'єктами
        /// </summary>
        public object Items { get; set; }

        public object Totals { get; set; }

        public ItemsList(int _pageIndex, int _pageSize, object _items, int _totalPages = 0, int _totalItems = 0, object totals = null)
        {
            PageIndex = _pageIndex;
            PageSize = _pageSize;
            Items = _items;
            TotalPages = _totalPages;
            TotalItems = _totalItems;
            Totals = totals;
        }
    }

    public class ItemsList<T>
    {
        /// <summary>
        /// Загальна кількість
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// Закальна кількість сторінок
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// Сторінка з результатом
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// Кільскість елементів на сторінці
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Список з об'єктами
        /// </summary>
        public List<T> Items { get; set; }

        public object Totals { get; set; }

        public ItemsList(int _pageIndex, int _pageSize, List<T> _items, int _totalPages = 0, int _totalItems = 0, object totals = null)
        {
            PageIndex = _pageIndex;
            PageSize = _pageSize;
            Items = _items;
            TotalPages = _totalPages;
            TotalItems = _totalItems;
            Totals = totals;
        }
    }

    /// <summary>
    /// Модель для відповіді
    /// </summary>
    public class ProviderResult
    {
        /// <summary>
        /// Код відповіді
        /// </summary>
        public int statusCode { get; set; }
        /// <summary>
        /// Повідомлення відповіді
        /// </summary>
        public string statusMessage { get; set; }
        /// <summary>
        /// Вміст відповіді
        /// </summary>
        public object data { get; set; }
    }

    public class ProviderResult_v2
    {
        /// <summary>
        /// Код відповіді
        /// </summary>
        public int statusCode { get; set; }
        /// <summary>
        /// Повідомлення відповіді
        /// </summary>
        public string statusMessage { get; set; }
        /// <summary>
        /// Вміст відповіді
        /// </summary>
        public object result { get; set; }
    }

    public class ProviderResult<T> : ProviderResult
    {
        /// <summary>
        /// Вміст відповіді
        /// </summary>
        public new T data { get; set; }
    }

    public class EntityById<T>
    {
        public int id { get; set; }
        public T data { get; set; }

        public EntityById(int id, T data)
        {
            this.id = id;
            this.data = data;
        }

        public EntityById(int id)
        {
            this.id = id;
        }
    }


    /// <summary>
    /// Дефолтна таблиця
    /// </summary>
    public class defaultTableClass
    {
        /// <summary>
        /// Ідентифікатор запису
        /// </summary>
        [JsonIgnore]
        public int id { get; set; } = -1;
        /// <summary>
        /// Статус запису
        /// </summary>
        [JsonIgnore]
        public bool status { get; set; } = false;
        /// <summary>
        /// дача час створення запису
        /// </summary>
        public DateTime date { get; set; } = DateTime.Now;
    }

    public class defaultSysTableClass: defaultTableClass
	{
        public Guid hash { get; set; }
		public int sys_userId { get; set; }
	}


		public class defaultTableClassWithKey<T> {
        /// <summary>
        /// Ідентифікатор запису
        /// </summary>
        public T id { get; set; } = default;
        /// <summary>
        /// Статус запису
        /// </summary>
        public bool status { get; set; } = false;
        /// <summary>
        /// дача час створення запису
        /// </summary>
        public DateTime date { get; set; } = DateTime.Now;
    }

    public class ListResult<T> {

        /// <summary>
        /// сторінка
        /// </summary>
        public int pageIndex { get; set; } = 1;
        /// <summary>
        /// розмір сторінки
        /// </summary>
        public int pageSize { get; set; } = 100;
        /// <summary>
        /// к-сть елементів, у загальному датасеті
        /// </summary>
        public int totalCount { get; set; } = 0;
        /// <summary>
        /// елемнти сторінки pageIndex
        /// </summary>
        public List<T> List { get; set; } = new List<T>();

    }

    public interface iDefaultTableClass<T>
    {
        T Add(T value);
        T Get(int id);
        T Remove(int id);
        T Update(T value);
        List<T> FillTable(DataTable table);
        T FillRow(DataRow row);
    }
}