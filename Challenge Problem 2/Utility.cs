using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ChallengeProblem2
{
    public static class Utility
    {
        private static IEnumerable<string> ExtractGraphemeClusters(string text) 
        {
            var enumerator = StringInfo.GetTextElementEnumerator(text);
            while(enumerator.MoveNext()) 
            {
                yield return (string)enumerator.Current;
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
    }
}