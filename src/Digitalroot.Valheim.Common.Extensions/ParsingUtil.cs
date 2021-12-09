using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Digitalroot.Valheim.Common.Extensions
{
  public static class ParsingUtil
  {
    private const string StandardTimeFomrat = "hh:mm ampm";
    private const string Am = "am";
    private const string Pm = "pm";

    public static string ReplaceTheOldCharacterWithNew(this string originString, char oldChar, char newChar)
    {
      var newString = new StringBuilder();

      if (originString.IsEmpty()) return originString;
      Array charArray = originString.ToCharArray();
      foreach (char chr in charArray)
      {
        newString.Append(chr == oldChar ? newChar : chr);
      }

      return newString.ToString().Trim();
    }

    public static string ToStringOrEmptyStringIfNull<TStruct>(this TStruct? nullable)
      where TStruct : struct => nullable.HasValue ? nullable.ToString() : string.Empty;

    public static DateTime TruncateTime(this DateTime dateTime) => DateTime.Parse(dateTime.ToShortDateString());

    /// <summary>
    ///   Trims the specified string value. If the value
    ///   is null, then null is returned.
    /// </summary>
    /// <param name = "stringValue">The string value to trim.</param>
    public static string Trim(this string stringValue) => stringValue?.Trim();

    /// <summary>
    ///   Replaces any multiple spaces with a single space.
    /// </summary>
    /// <param name = "stringValue">The string value.</param>
    public static string ReplaceMultipleSpacesWithSingleSpace(this string stringValue)
    {
      var returnValue = stringValue;
      if (stringValue.IsEmpty()) return returnValue;

      while (returnValue.Contains("  "))
      {
        returnValue = returnValue.Replace("  ", " ");
      }

      return returnValue;
    }

    public static bool StringContainsNumber(this string stringValue) => !stringValue.IsEmpty() && stringValue.IndexOfAny("0123456789".ToCharArray()) > -1;

    public static bool IsNullOrEmpty(this string stringValue) => string.IsNullOrEmpty(stringValue);

    public static string ToDecimalString(this decimal? nullableDecimal) => nullableDecimal?.ToString();

    public static string ToIntegerString(this int? nullableInt) => nullableInt?.ToString();

    public static string ToCurrencyString(this decimal? nullableDecimal, bool includeCurrencySymbol = false, bool includeGroupSeparators = false)
    {
      if (nullableDecimal == null) return null;

      var nfi = new CultureInfo("en-US", false).NumberFormat;
      nfi.CurrencySymbol = includeCurrencySymbol ? nfi.CurrencySymbol : "";
      nfi.CurrencyGroupSeparator = includeGroupSeparators ? nfi.CurrencyGroupSeparator : "";
      return string.Format(nfi, "{0:C}", (decimal)nullableDecimal);
    }

    public static string ReplaceNonAlphaNumericWithSpace(this string value)
    {
      if (value.IsEmpty())
      {
        return value;
      }

      var newString = new StringBuilder();
      Array charArray = value.ToCharArray();
      foreach (char chr in charArray)
      {
        newString.Append(char.IsLetterOrDigit(chr) ? chr : ' ');
      }

      return newString.ToString().Trim();
    }

    public static string ReplaceNonAlphaNumericWithSpaceExceptSlash(this string value)
    {
      if (value.IsEmpty())
      {
        return value;
      }

      var newString = new StringBuilder();
      Array charArray = value.ToCharArray();
      foreach (char chr in charArray)
      {
        if (chr.Equals('/'))
        {
          newString.Append(chr);
        }
        else
        {
          newString.Append(char.IsLetterOrDigit(chr) ? chr : ' ');
        }
      }

      var removeSpecialCharacter = newString.ToString().Trim();
      if (!string.IsNullOrEmpty(removeSpecialCharacter))
      {
        removeSpecialCharacter = Regex.Replace(removeSpecialCharacter, @"[^\u0000-\u007F]+", string.Empty);
      }

      return removeSpecialCharacter;
    }

    public static string ReplaceAllSpecialCharacterWithSpace(this string value)
    {
      var removeSpecialCharacter = ReplaceNonAlphaNumericWithSpace(value);
      if (!string.IsNullOrEmpty(removeSpecialCharacter))
      {
        removeSpecialCharacter = Regex.Replace(removeSpecialCharacter, @"[^\u0000-\u007F]+", string.Empty);
      }

      return removeSpecialCharacter;
    }

    public static string ToStandardTimeString(this TimeSpan time) => $"{(time.Hours <= 12 ? time.Hours : time.Hours - 12)}:{time.Minutes.ToString().PadLeft(2, '0')} {(time.Hours <= 12 ? Am : Pm)}";

    public static string ToClientFileDateString(this DateTime? date) => date?.ToString("yyyyMMdd");
    public static int GetPrimaryKey(this string id) => int.Parse(id.IndexOf("@", StringComparison.InvariantCulture) != -1 ? id.Substring(0, id.IndexOf("@", 1, StringComparison.InvariantCulture)) : id);

    public static bool TryGetPrimaryKey(this string id, out int primaryKey)
    {
      if (!id.IsEmpty())
      {
        return id.IndexOf("@", StringComparison.InvariantCulture) != -1 ? int.TryParse(id.Substring(0, id.IndexOf("@", 1, StringComparison.InvariantCulture)), out primaryKey) : int.TryParse(id, out primaryKey);
      }

      primaryKey = 0;
      return false;
    }

    /// <summary>
    ///   Converts a two digit year into a four digit year.
    ///   If the given year is greater that the pivotYear then
    ///   the century is '19', else it is '20'.
    /// </summary>
    public static string GetFourDigitYear(this string year, int pivotYear) => year?.Length switch
    {
      2 when int.Parse(year) > pivotYear => "19" + year
      , 2 => "20" + year
      , _ => year
    };

    /// <summary>
    ///   Converts a four digit year into a two digit year.
    /// </summary>
    public static string GetTwoDigitYear(this string year) => year?.Length == 4 ? year.Substring(2, 2) : year;

    /// <summary>
    ///   Format a Date or NullableDate as a string
    /// </summary>
    /// <param name = "dateData"></param>
    /// <returns></returns>
    public static string FormatDateAsString(this object dateData) => dateData switch
    {
      null => string.Empty
      , DateTime time => time.ToShortDateString()
      , _ => dateData.ToString()
    };

    /// <summary>
    ///   Format a Date or NullableDate as a string
    /// </summary>
    /// <param name = "dateData"></param>
    /// <returns></returns>
    public static string FormatDateTimeAsString(this object dateData) => dateData switch
    {
      null => string.Empty
      , DateTime time => time.ToLongTimeString()
      , _ => dateData.ToString()
    };

    /// <summary>
    ///   Re-formats the given dateTime string to the given format.
    /// </summary>
    public static string ReformatDate(this string date, string format) => TryParseDate(date)?.ToString(format) ?? date;

    /// <summary>
    ///   Re-formats the given dateTime string to the given format.
    /// </summary>
    public static string ReformatTime(this string time, string format) => time == null ? null : ParseTime(time)?.ToString(format);

    public static DateTime ParseDate(this string dateTime) => DateTime.ParseExact(dateTime, new[] { "MMddyyyy", "yyMMdd", "yyyyMMdd", "yyyy-MM-dd", "MM/dd/yy", "MMM dd,yyyy", "d", "D", "F", "f", "g", "G" }, null, DateTimeStyles.AllowWhiteSpaces);

    public static bool? ParseToNullableBoolean(this string text)
    {
      if (!text.IsEmpty())
      {
        return ParseBoolean(text);
      }

      return null;
    }

    public static DateTime? ParseToNullableDate(this string dateTime)
    {
      DateTime? dt = null;

      if (!dateTime.IsEmpty())
      {
        dt = DateTime.ParseExact(dateTime, new[] { "MMddyyyy", "yyMMdd", "yyyyMMdd", "MM-dd-yyyy", "yyyy-MM-dd", "MM/dd/yy", "MM/dd/yyyy", "MMM dd,yyyy", "d", "D", "F", "f", "g", "G" }, null, DateTimeStyles.AllowWhiteSpaces);
      }

      return dt;
    }

    public static DateTime? TryParseDate(this string dateTime)
    {
      DateTime? dt = null;
      if (dateTime.IsEmpty()) return null;
      try
      {
        var valid = DateTime.TryParseExact(dateTime, new[] { "MMddyyyy", "yyMMdd", "yyyyMMdd", "M-d-yyyy", "MM-d-yyyy", "M-dd-yyyy", "MM-dd-yyyy", "yyyy-MM-dd", "MM/dd/yy", "MM/dd/yyyy", "MMM dd,yyyy", "d", "D", "F", "f", "g", "G" }, null, DateTimeStyles.AllowWhiteSpaces, out var result);
        if (valid)
        {
          dt = result;
        }
      }
      catch (FormatException) { }

      return dt;
    }

    public static DateTime? TryParseDateExtended(this string dateTime)
    {
      DateTime? dt = null;
      if (dateTime.IsEmpty()) return null;
      try
      {
        var valid = DateTime.TryParseExact(dateTime, new[] { "MMddyyyy", "yyyyMMdd", "M-d-yy", "MM-d-yy", "M-dd-yy", "M-d-yyyy", "MM-d-yyyy", "M-dd-yyyy", "MM-dd-yyyy", "yyyy-MM-dd", "MM/dd/yy", "M/d/yy", "MM/d/yy", "M/dd/yy", "MM/dd/yyyy", "MMM dd,yyyy", "d", "D", "F", "f", "g", "G" }, null, DateTimeStyles.AllowWhiteSpaces, out var result);
        if (valid)
        {
          dt = result;
        }
      }
      catch (FormatException) { }

      return dt;
    }

    public static DateTime? ParseTime(this string dateTime)
    {
      if (dateTime.IsEmpty()) return null;
      var dateTimeToStringFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern;
      return DateTime.ParseExact(dateTime, new[] { "HHmmss", dateTimeToStringFormat }, null, DateTimeStyles.AllowWhiteSpaces);
    }

    public static TimeSpan? ParseTimeSpan(this string time, string format = StandardTimeFomrat)
    {
      TimeSpan? ts;

      if (time.IsEmpty()) return null;
      switch (format)
      {
        case StandardTimeFomrat:
          var tempHour = time.Substring(0, time.IndexOf(':'));
          var tempMinute = time.Substring(time.IndexOf(':') + 1, 2);
          var ampm = time.Substring(time.Length - 2);

          var hour = System.Convert.ToInt32(tempHour);
          var minute = System.Convert.ToInt32(tempMinute);
          hour += string.Compare(ampm, Am, StringComparison.InvariantCulture) == 0 ? 0 : 12;

          ts = new TimeSpan(hour, minute, 0);
          break;

        default:
          return null;
      }

      return ts;
    }

    public static short ParseInt16(this string fromValue) => short.Parse(fromValue, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint);

    public static int ParseInt32(this string str, int defaultValue) => str.IsEmpty() ? defaultValue : int.Parse(str);

    public static int ParseInt32(this string str) => int.Parse(str, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint);

    public static long ParseInt64(this string fromValue) => long.Parse(fromValue, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint);

    public static string EmptyToNull(this string str) => str.IsEmpty() ? null : str.Trim();

    public static string EmptyToNull(this string str, bool allowTrim) => str.IsEmpty() ? null : allowTrim ? str.Trim() : str;

    public static string EmptyToEnglish(this string str) => str.IsEmpty() ? "EN" : str.Trim();

    public static decimal ParseDecimal(this string str, bool hasImpliedDecimal, int decPosition = 2)
    {
      var dec = decimal.MinValue; // Considered null
      if (str.IsEmpty() || str.Length <= decPosition) return dec;
      // Handle implied decimal
      if (hasImpliedDecimal)
      {
        if (str.Substring(str.Length - decPosition, 1) != ".")
        {
          str = str.Insert(str.Length - decPosition, ".");
        }
      }

      dec = decimal.Parse(str);
      return dec;
    }

    public static decimal ParseDecimalForFee(this string str, bool hasImpliedDecimal = false, int decPosition = 2)
    {
      var dec = decimal.MinValue;
      if (str.IsEmpty()) return dec;
      // Handle implied decimal
      if (hasImpliedDecimal)
      {
        if (str.Length == 1)
        {
          str = str.Insert(str.Length - 1, ".");
        }
        else if (str.Substring(str.Length - decPosition, 1) != ".")
        {
          str = str.Insert(str.Length - decPosition, ".");
        }
      }

      dec = decimal.Parse(str);
      return dec;
    }

    public static decimal ParseDecimal(this string str) => ParseDecimal(str, false, 0);

    public static decimal? ParseToNullableDecimal(this string str, bool hasImpliedDecimal, int decPosition = 2)
    {
      if (str.IsEmpty() || str.Length <= decPosition) return null;
      // Handle implied decimal
      if (hasImpliedDecimal)
      {
        if (str.Substring(str.Length - decPosition, 1) != ".")
        {
          str = str.Insert(str.Length - decPosition, ".");
        }
      }

      decimal? dec = decimal.Parse(str);
      return dec;
    }

    public static decimal? TryParseDecimal(this string decimalValue) => decimalValue.IsEmpty() ? null : decimal.TryParse(decimalValue, out var dec) ? dec : null;

    public static decimal? TryParseDecimal(this string decimalValue, decimal maxValue) => decimalValue.IsEmpty() ? null : decimal.TryParse(decimalValue, out var dec) && dec < maxValue ? dec : null;

    public static decimal? ParseToNullableDecimal(this string str) => ParseToNullableDecimal(str, false, 0);

    public static int? ParseToNullableInt(this string text) => !text.IsEmpty() && int.TryParse(text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, null, out var value) ? value : null;

    public static uint? ParseToNullableUInt(this string text) => !text.IsEmpty() && uint.TryParse(text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, null, out var value) ? value : null;

    public static short? ParseToNullableShort(this string text) => !text.IsEmpty() && short.TryParse(text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, null, out var value) ? value : null;

    public static bool ParseBoolean(this string fromValue) => !fromValue.IsEmpty() && fromValue switch
    {
      "Y" => true
      , "N" => false
      , _ => bool.Parse(fromValue)
    };

    public static decimal? ParseCurrency(this string currencyString, bool insertDecimalAtEnd = true)
    {
      // Only operate on strings that are not empty.
      if (currencyString.IsEmpty()) return null;
      // Set decimal pos.
      var posToInsertDecimal = insertDecimalAtEnd ? 0 : 2;

      switch (currencyString.Contains("."))
      {
        // Insert a decimal point "." if one does not exist.
        case false:
          currencyString = currencyString.Insert(currencyString.Length - posToInsertDecimal, ".");
          currencyString = currencyString.PadRight(currencyString.Length + (2 - posToInsertDecimal), '0');
          break;
      }

      // Ensure decimal point has exactly two digits after it.
      return currencyString.LastIndexOf('.') != currencyString.Length - 3 ? null : decimal.TryParse(currencyString, out var tempConvertedCurrency) ? tempConvertedCurrency : null;
    }

    /// <summary>
    ///   Parses the given string into an enum of the
    ///   given <typeparam name = "TEnumType" />.
    /// </summary>
    /// <typeparam name = "TEnumType">The type of enum.</typeparam>
    /// <param name = "value">The value.</param>
    /// <returns></returns>
    public static TEnumType ParseEnum<TEnumType>(this string value) => (TEnumType)Enum.Parse(typeof(TEnumType), value, true);

    /// <summary>
    ///   Parses the given string into an enum of the
    ///   given <paramref name = "enumType" />.
    /// </summary>
    /// <param name = "enumType">Type of the enum.</param>
    /// <param name = "value">The value.</param>
    /// <returns></returns>
    public static Enum ParseEnum(Type enumType, string value) => (Enum)Enum.Parse(enumType, value, true);

    public static string ToNullableString(this Enum value) => value?.ToString();

    /// <summary>
    ///   Convenience method for replacing null values without repeating null check by caller
    /// </summary>
    /// <param name = "value"></param>
    /// <returns></returns>
    public static string ToNonNullString(this string value) => value ?? string.Empty;

    /// <summary>
    ///   Parses the given string into a nullable enum of the
    ///   given <typeparam name = "TEnumType" />.
    /// </summary>
    /// <param name = "value">The value to parse.</param>
    public static TEnumType? ParseToNullableEnum<TEnumType>(this string value)
      where TEnumType : struct, IComparable, IFormattable, IConvertible => Enum.TryParse(value, out TEnumType enumValue) ? enumValue : null;

    public static bool CanParseEnum<TEnumType>(this string value) => !value.IsEmpty() && Enum.IsDefined(typeof(TEnumType), value);

    public static TReturnEnumType ParseEnum<TReturnEnumType, TMappingEnumType>(string value)
      where TMappingEnumType : struct, IComparable, IFormattable, IConvertible =>
      // Can't cast directly from MappingEnumType to ReturnEnumType since the compiler can't
      //   say they are both enums with the same underlying type.
      (TReturnEnumType)(object)ParseEnum<TMappingEnumType>(value);

    /// <summary>
    ///   Converts the specified fromEnum to an enums of type <typeparamref name = "TOEnumType" />.
    /// </summary>
    /// <typeparam name = "TOEnumType">The type of enum to convert to.</typeparam>
    /// <param name = "fromEnum">The enum to convert</param>
    public static TOEnumType Convert<TOEnumType>(this Enum fromEnum)
      where TOEnumType : struct, IComparable, IFormattable, IConvertible => ParseEnum<TOEnumType>(fromEnum.ToString());

    /// <summary>
    ///   Converts the specified fromEnum to an enums of type <typeparamref name = "TOEnumType" />.
    /// </summary>
    /// <typeparam name = "TFromEnumType">The type of enum to convert from.</typeparam>
    /// <typeparam name = "TOEnumType">The type of enum to convert to.</typeparam>
    /// <param name = "fromEnum">The enum to convert</param>
    /// <returns></returns>
    public static TOEnumType Convert<TFromEnumType, TOEnumType>(this TFromEnumType fromEnum)
      where TFromEnumType : struct, IComparable, IFormattable, IConvertible
      where TOEnumType : struct, IComparable, IFormattable, IConvertible => ParseEnum<TOEnumType>(fromEnum.ToString());

    /// <summary>
    ///   Converts the specified sequence of enums to a sequence of enums of type <typeparamref name = "TOEnumType" />.
    /// </summary>
    /// <typeparam name = "TFromEnumType">The type of enum to convert from.</typeparam>
    /// <typeparam name = "TOEnumType">The type of enum to convert to.</typeparam>
    /// <param name = "fromEnumSequence">The sequence of enums to convert.</param>
    /// <returns></returns>
    public static IEnumerable<TOEnumType> Convert<TFromEnumType, TOEnumType>(this IEnumerable<TFromEnumType> fromEnumSequence)
      where TFromEnumType : struct, IComparable, IFormattable, IConvertible
      where TOEnumType : struct, IComparable, IFormattable, IConvertible
    {
      return fromEnumSequence.Select(fromEnum => fromEnum.Convert<TFromEnumType, TOEnumType>());
    }

    public static string UppercaseString(this string str) => str.IsEmpty() ? null : str.ToUpper();

    public static string Capitalize(this string text) => UpperCaseFirstCharacter(text);

    public static string UpperCaseFirstCharacter(this string text) => !string.IsNullOrEmpty(text) ? $"{text.Substring(0, 1).ToUpper()}{text.Substring(1).ToLower()}" : text;

    public static bool CanParseToDate(this string text)
    {
      try
      {
        ParseDate(text);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool CanParseToInt(this string text)
    {
      try
      {
        ParseInt32(text);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool CanParseToDecimal(this string text)
    {
      try
      {
        ParseDecimal(text);
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
