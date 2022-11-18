using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Owoify.Owoify_Net;

namespace Owoify
{
    public static class Owoifier
    {
        public enum OwoifyLevel
        {
            Owo, Uwu, Uvu
        }

        /// <summary>
        /// Owoify the given source string using specified owoness level.
        /// </summary>
        /// <param name="value">The source string to owoify.</param>
        /// <param name="level">The owoness level. Three levels are available (from lowest to highest): Owo, Uwu, Uvu</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">When the specified owoness level is invalid.</exception>
        public static string Owoify(string value, OwoifyLevel level = OwoifyLevel.Owo)
        {
            var wordRegex = new Regex(@"[^\s]+");
            var wordMatches = wordRegex.Matches(value);

            var spaceRegex = new Regex(@"\s+");
            var spaceMatches = spaceRegex.Matches(value);

            var wordsList = new List<Word>();
            for (var index = 0; index < wordMatches.Count; index++)
            {
                var match = wordMatches[index];
                wordsList.Add(new Word(match.Value));
            }

            var words = wordsList.AsEnumerable();

            var spacesList = new List<Word>();
            for (var index = 0; index < spaceMatches.Count; index++)
            {
                var match = spaceMatches[index];
                spacesList.Add(new Word(match.Value));
            }

            var spaces = spacesList.AsEnumerable();
            
            words = words.Select(word =>
            {
                word = Utility.SpecificWordMappingList.Aggregate(word, (current, func) => func.Invoke(current));

                switch (level)
                {
                    case OwoifyLevel.Owo:
                        word = Utility.OwoMappingList.Aggregate(word, (current, func) => func.Invoke(current));
                        break;
                    case OwoifyLevel.Uwu:
                        word = Utility.UwuMappingList.Aggregate(word, (current, func) => func.Invoke(current));
                        word = Utility.OwoMappingList.Aggregate(word, (current, func) => func.Invoke(current));
                        break;
                    case OwoifyLevel.Uvu:
                        word = Utility.UvuMappingList.Aggregate(word, (current, func) => func.Invoke(current));
                        word = Utility.UwuMappingList.Aggregate(word, (current, func) => func.Invoke(current));
                        word = Utility.OwoMappingList.Aggregate(word, (current, func) => func.Invoke(current));
                        break;
                    default:
                        throw new ArgumentException("Invalid owoness level.", nameof(level));
                }

                return word;
            });

            var result = Utility.InterleaveArrays(words, spaces);
            return string.Join(string.Empty, result);
        }
    }
}
