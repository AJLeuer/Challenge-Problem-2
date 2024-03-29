using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ChallengeProblem2
{
    public static class Utility
    {
        private static IEnumerable<string> ExtractGraphemeClusters(string text)
        {
            var enumerator = StringInfo.GetTextElementEnumerator(text);
            while (enumerator.MoveNext())
            {
                yield return (string) enumerator.Current;
            }
        }

        /* Code credit: https://stackoverflow.com/questions/228038/best-way-to-reverse-a-string/15111719#15111719 */
        public static string Reverse(this string text)
        {
            var graphemeClusters = ExtractGraphemeClusters(text);
            return string.Join("", graphemeClusters.Reverse().ToArray());
        }

        public static void Reverse(this StringBuilder text)
        {
            var reversed = text.ToString().Reverse();
            text.Clear();
            text.Append(reversed);
        }

        /// <summary>
        /// Removes any disallowed characters found at the beginning or end of text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="disallowedCharacters"></param>
        /// <returns></returns>
        public static String CleanEnds(this String text, params char[] disallowedCharacters)
        {
            string cleanedForward = cleanStringEnds(text, disallowedCharacters);
            string cleanedInReverse = cleanStringEnds(cleanedForward.Reverse(), disallowedCharacters);
            string cleaned = cleanedInReverse.Reverse();
            return cleaned;
        }

        private static String cleanStringEnds(String text, params char[] disallowedCharacters)
        {
            String cleaned = new String(text);

            for (int i = 0; i < cleaned.Length; i++)
            {
                char currentChar = cleaned[i];

                if (disallowedCharacters.Contains(currentChar))
                {
                    cleaned = cleaned.Remove(i, 1);
                    i--;
                }
                else
                {
                    break;
                }
            }

            return cleaned;
        }

        /* Code credit: https://stackoverflow.com/questions/4718965/c-sharp-string-comparison-ignoring-spaces-carriage-return-or-line-breaks/4719009 */
        public static bool AreEquivalent(string source, string target)
        {
            if (source == null) return target == null;
            if (target == null) return false;
            var normForm1 = Normalize(source);
            var normForm2 = Normalize(target);
            return string.Equals(normForm1, normForm2);
        }

        private static string Normalize(string value)
        {
            value = value.Normalize(NormalizationForm.FormC);
            value = value.Replace("\r\n", "\n").Replace("\r", "\n");
            value = Regex.Replace(value, @"\s", string.Empty);
            return value.ToLowerInvariant();
        }

        public static T FindMedian<T>(List<T> items) where T : IComparable<T>, IEquatable<T>, new()
        {
            items.Sort();

            if (items.Count <= 2)
            {
                if (items.Count == 0)
                {
                    return default(T);
                }
                else if (items.Count == 1)
                {
                    return items[0];
                }
                else /* if (items.Count == 2) */
                {
                    dynamic firstItem = items[0];
                    dynamic secondItem = items[1];
                    dynamic average = (firstItem + secondItem) / 2;
                    return (T) average;
                }
            }
            else
            {
                items.RemoveAt(items.Count - 1);
                items.RemoveAt(0);
                return FindMedian<T>(items);
            }
        }
        
        /* Code and idea credit: https://stackoverflow.com/questions/479410/enum-tostring-with-user-friendly-strings */
        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }

        public static string ReadFromBeginning(this StreamReader reader)
        {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            return reader.ReadToEnd();
        }
    }
}