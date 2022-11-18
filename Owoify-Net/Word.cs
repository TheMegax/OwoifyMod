﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Owoify.Owoify_Net
{
    internal class Word
    {
        private string _word;
        private readonly HashSet<string> _replacedWords = new();

        public override string ToString()
            => _word;

        internal Word(string word)
        {
            _word = word;
        }

        internal Word Replace(Regex searchValue, string replaceValue, bool replaceReplacedWords = false)
        {
            if (!replaceReplacedWords &&
                SearchValueContainsReplacedWords(searchValue, replaceValue, _replacedWords))
                return this;

            var replacingWord = _word;
            if (searchValue.IsMatch(_word))
                replacingWord = Regex.Replace(_word, searchValue.ToString(), replaceValue);

            var matchCollection = searchValue.Matches(_word);
            
            var replacedWords = new List<string>();
            if (matchCollection.Count > 0)
            {
                for (var i = 0; i < matchCollection.Count; i++)
                {
                    var x = matchCollection[i];
                    replacedWords.Add(x.Value.Replace(x.Value, replaceValue));
                }
            }

            if (replacingWord == _word) return this;
            
            foreach (var word in replacedWords)
            {
                _replacedWords.Add(word);
            }
            _word = replacingWord;
            return this;
        }

        internal Word Replace(Regex searchValue, Func<string> func, bool replaceReplacedWords = false)
        {
            var replaceValue = func.Invoke();

            if (!replaceReplacedWords &&
                SearchValueContainsReplacedWords(searchValue, replaceValue, _replacedWords))
                return this;

            var replacingWord = _word;
            if (searchValue.IsMatch(_word))
            {
                var match = searchValue.Match(_word).Value;
                replacingWord = _word.Replace(match, replaceValue).Trim();
            }

            var matchCollection = searchValue.Matches(_word);
            var replacedWords = new List<string>();
            if (matchCollection.Count > 0)
            {
                for (var i = 0; i < matchCollection.Count; i++)
                {
                    var x = matchCollection[i];
                    replacedWords.Add(x.Value.Replace(x.Value, replaceValue));
                }
            }

            if (replacingWord != _word)
            {
                foreach (var word in replacedWords)
                {
                    _replacedWords.Add(word);
                }
                _word = replacingWord;
            }
            return this;
        }

        internal Word Replace(Regex searchValue, Func<string, string, string> func, bool replaceReplacedWords = false)
        {
            if (!searchValue.IsMatch(_word)) return this;
            
            var match = searchValue.Match(_word);
            var replaceValue = func.Invoke(match.Groups[1].Value, match.Groups[2].Value);

            if (!replaceReplacedWords &&
                SearchValueContainsReplacedWords(searchValue, replaceValue, _replacedWords))
                return this;
            
            var replacingWord = _word.Replace(match.Value, replaceValue).Trim();

            var matchCollection = searchValue.Matches(_word);
            var replacedWords = new List<string>();
            if (matchCollection.Count > 0)
            {
                for (var i = 0; i < matchCollection.Count; i++)
                {
                    var x = matchCollection[i];
                    replacedWords.Add(x.Value.Replace(x.Value, replaceValue));
                }
            }

            if (replacingWord == _word) return this;
            
            foreach (var word in replacedWords)
            {
                _replacedWords.Add(word);
            }
            _word = replacingWord;
            return this;
        }

        private static bool SearchValueContainsReplacedWords(Regex searchValue, string replaceValue,
            IEnumerable<string> replacedWords)
        {
            return replacedWords.Any(word =>
            {
                if (!searchValue.IsMatch(word)) return false;
                
                var match = searchValue.Match(word).Groups[0];
                return word.Replace(match.Value, replaceValue) == word;
            });
        }
    }
}
