using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
// ReSharper disable MemberCanBePrivate.Global

namespace Digitalroot.Valheim.Common.Extensions
{
  /// <summary>
  ///   Provides useful methods to check assertions.
  /// </summary>
  public static class ValidationUtil
  {
    /// <summary>
    ///   Return null value if string is empty.
    /// </summary>
    /// <param name = "value">the string to test</param>
    public static string UseNullIfEmpty(this string value)
    {
      if (value == null)
      {
        return null;
      }
      return value.Trim().Length == 0
               ? null
               : value;
    }

    /// <summary>
    ///   Determine if the given string is empty; i.e., is null or 
    ///   a string containing only white space.
    /// </summary>
    /// <param name = "value">the string to test</param>
    public static bool IsEmpty(this string value)
    {
      return value == null || (value.Trim().Length == 0);
    }

    /// <summary>
    ///   Determine if the given string is non-empty; i.e., is not null and 
    ///   does not contain only white space.
    /// </summary>
    /// <param name = "value">the string to test</param>
    public static bool HasValue(this string value)
    {
      return !IsEmpty(value);
    }

    public static string RemoveMultipleSpacesWithSingleSpace(this string value)
    {
      const RegexOptions options = RegexOptions.None;
      var regex = new Regex(@"[ ]{2,}", options);
      return regex.Replace(value, @" ");
    }

    /// <summary>
    ///   Returns the first "n" characters of a string.
    /// </summary>
    /// <param name = "s"></param>
    /// <param name = "charCount"></param>
    /// <returns></returns>
    public static string First(this string s, int charCount)
    {
      return s.IsEmpty()
               ? String.Empty
               : s.Substring(0, Math.Min(s.Length, charCount));
    }

    public static string ToTitleCase(this string value)
    {
      var cultureInfo = Thread.CurrentThread.CurrentCulture;
      var textInfo = cultureInfo.TextInfo;
      return value.IsEmpty()
               ? string.Empty
               : textInfo.ToTitleCase(textInfo.ToLower(value));
    }


      /// <summary>
    ///   Determine if the given StringBuilder is empty; i.e., is null or 
    ///   a string containing only white space.
    /// </summary>
    /// <param name = "value">the string to test</param>
    public static bool IsEmpty(this StringBuilder value)
    {
      return (value == null) || (value.ToString().Trim().Length == 0);
    }

    public static bool IsEmpty<TItemType>(this IEnumerable<TItemType> collection)
    {
      return (collection == null) || (!collection.Any());
    }

    public static bool HaveValue<TItemType>(this IEnumerable<TItemType> collection)
    {
      return !collection.IsEmpty();
    }

    public static bool IsEmptyArray(this Array array)
    {
      return (array == null) || (array.Length == 0);
    }

    public static bool IsStringValueOfEnumType<TEnumType>(this string value)
    {
      try
      {
        return (!IsEmpty(value) && Enum.IsDefined(typeof (TEnumType), value));
      }
      catch
      {
        return false;
      }
    }

    public static bool IsDateStringValid(this string dateString)
    {
      var tempDate = dateString.TryParseDate();
      return tempDate?.Year > 1899 && tempDate.Value.Year < 2100;
    }

    public static bool IsNumeric(this string value)
    {
      return decimal.TryParse(value, out _);
    }

    public static bool IsAlphaNumeric(this string value)
    {
      var ret = !IsEmpty(value);
      if (ret)
      {
        if (value.Trim().Any(c => !char.IsNumber(c) && !char.IsLetter(c)))
        {
          ret = false;
        }
      }
      return ret;
    }
		
		public static bool HasNumbers(this string value)
		{
			var ret = !value.IsEmpty();
			if (ret)
			{
				if (value.Trim().Any(char.IsNumber))
				{
					return true;
				}
			}
			return false;
		}

    /// <summary>
    ///   Determines if the length of the given string is
    ///   less than or equal to the given length.
    /// </summary>
    public static bool LengthIsLessThanOrEqualTo(this string valueToCheck, int length)
    {
      return IsEmpty(valueToCheck) || valueToCheck.Length <= length;
    }

    /// <summary>
    ///   Compares two strings after removing whitespace to determine if different
    /// </summary>
    /// <param name = "valOne">First value</param>
    /// <param name = "valTwo">Second value</param>
    /// <returns>True if they are different</returns>
    public static bool StringIsDifferent(this string valOne, string valTwo)
    {
      var one = valOne ?? String.Empty;
      var two = valTwo ?? String.Empty;
      return one.Trim() != two.Trim();
    }

    public static bool IsNull(object valueToValidate)
    {
      if (valueToValidate == null)
      {
        return true;
      }

      // Adding this logic so we don't need a different
      // method for string value types.
      var stringToValidate = valueToValidate as String;
      return (stringToValidate != null) && String.IsNullOrEmpty(stringToValidate);
    }
    
  }
}
