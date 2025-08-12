using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUtility.Extensions
{
    /// <summary>
    /// Helper class for Vietnamese text search without accents
    /// Uses existing ConvertToUnSign_V2 utility from StringExtension
    /// </summary>
    public static class VietnameseSearchHelper
    {
        /// <summary>
        /// Normalizes text by removing Vietnamese accents and converting to lowercase
        /// </summary>
        /// <param name="text">Text to normalize</param>
        /// <returns>Normalized text without accents</returns>
        public static string NormalizeText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // Use existing ConvertToUnSign_V2 from StringExtension as static method
            return StringExtension.ConvertToUnSign_V2(text).ToLower().Trim();
        }

        /// <summary>
        /// Checks if text contains keyword (both normalized for Vietnamese search)
        /// </summary>
        /// <param name="text">Text to search in</param>
        /// <param name="keyword">Keyword to search for</param>
        /// <returns>True if text contains keyword</returns>
        public static bool ContainsKeyword(string text, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return true;

            if (string.IsNullOrEmpty(text))
                return false;

            var normalizedText = NormalizeText(text);
            var normalizedKeyword = NormalizeText(keyword);

            return normalizedText.Contains(normalizedKeyword);
        }

        /// <summary>
        /// Filters a collection by keyword search on multiple text properties
        /// </summary>
        /// <typeparam name="T">Type of items to filter</typeparam>
        /// <param name="items">Collection to filter</param>
        /// <param name="keyword">Keyword to search for</param>
        /// <param name="textSelectors">Functions to extract text properties from items</param>
        /// <returns>Filtered collection</returns>
        public static IEnumerable<T> FilterByKeyword<T>(
            IEnumerable<T> items,
            string keyword,
            params Func<T, string>[] textSelectors)
        {
            if (string.IsNullOrEmpty(keyword) || textSelectors == null || textSelectors.Length == 0)
                return items;

            return items.Where(item =>
                textSelectors.Any(selector =>
                {
                    var text = selector(item);
                    return ContainsKeyword(text, keyword);
                })
            );
        }

        /// <summary>
        /// Filters a collection by keyword search on a single text property
        /// </summary>
        /// <typeparam name="T">Type of items to filter</typeparam>
        /// <param name="items">Collection to filter</param>
        /// <param name="keyword">Keyword to search for</param>
        /// <param name="textSelector">Function to extract text property from item</param>
        /// <returns>Filtered collection</returns>
        public static IEnumerable<T> FilterByKeyword<T>(
            IEnumerable<T> items,
            string keyword,
            Func<T, string> textSelector)
        {
            return FilterByKeyword(items, keyword, new[] { textSelector });
        }

        /// <summary>
        /// Splits keyword into individual words and searches for all words
        /// </summary>
        /// <typeparam name="T">Type of items to filter</typeparam>
        /// <param name="items">Collection to filter</param>
        /// <param name="keyword">Keywords to search for (space-separated)</param>
        /// <param name="textSelectors">Functions to extract text properties from items</param>
        /// <returns>Filtered collection where ALL keywords are found</returns>
        public static IEnumerable<T> FilterByKeywordWords<T>(
            IEnumerable<T> items,
            string keyword,
            params Func<T, string>[] textSelectors)
        {
            if (string.IsNullOrEmpty(keyword) || textSelectors == null || textSelectors.Length == 0)
                return items;

            var keywords = keyword.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (keywords.Length == 0)
                return items;

            return items.Where(item =>
                keywords.All(kw =>
                    textSelectors.Any(selector =>
                    {
                        var text = selector(item);
                        return ContainsKeyword(text, kw);
                    })
                )
            );
        }
    }
} 