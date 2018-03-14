using Android.App;
using Android.Widget;
using Android.OS;
using System.Net.Http;
using ModernHttpClient;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics.Contracts;
using System.Text;

namespace androidtest
{
    [Activity (Label = "androidtest", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        System.Net.CookieContainer cookieContainer = new System.Net.CookieContainer();
        public Cookie Cookie { get; set; }
        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button> (Resource.Id.myButton);

            button.Click += delegate { button.Text = $"{count++} clicks!"; Run (); };

            Run();
        }

        private async void Run()
        {
            var cert = "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";
            
            HttpClient client = new HttpClient (new NativeMessageHandler(pfxData: Convert.FromBase64String(cert), pfxPassword: "badssl.com"));

            string [] urls = new string[]{"https://badssl.com", "https://client.badssl.com", "https://client-cert-missing.badssl.com"};

            foreach (string url in urls)
            {
                try
                {
                    var message = new HttpRequestMessage(HttpMethod.Get, url);
                    message.Headers.Add("User-Agent", "test -client");
                    HttpResponseMessage response = await client.SendAsync(message);
                    var statusCode = response.StatusCode;
                    string content = await response.Content.ReadAsStringAsync ();

                    System.Diagnostics.Debug.WriteLine ($"{statusCode} : {content.Length} : {url}");

                    var headerKey = "Set-Cookie";
                    if (response.Headers.Contains(headerKey))
                    {
                        var cookies = response.Headers.GetValues(headerKey).ToList();
                        this.ApplyCookiesHeaderValues(cookies);

                        foreach (var cookie in Cookies)
                        {
                            this.cookieContainer.Add(new Uri(url), cookie);
                            System.Diagnostics.Debug.WriteLine($"{cookie}");
                        }
                    }
                }
                catch(Java.Net.SocketTimeoutException tex)
                {
                    System.Diagnostics.Debug.WriteLine($"Timeout: {tex.Message}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        public void ApplyCookiesHeaderValues(List<string> setCookieHeaderValues)
        {
            var combinedValue = setCookieHeaderValues.Join("`");
            this.Cookie = new Cookie("Cookie", combinedValue);
        }

        public IEnumerable<Cookie> Cookies
        {
            get
            {
                if (this.Cookie == null)
                {
                    return Enumerable.Empty<Cookie>();
                }

                return from setCookieHeader in this.Cookie.Value.Split('`')
                       let name = setCookieHeader.SubstringToFirst("=")
                       let cookieValue = WebUtility.UrlDecode(setCookieHeader.SubstringFromFirst("=").SubstringToFirst(";"))
                       let expires = setCookieHeader.SubstringFromLast("expires=",StringComparison.OrdinalIgnoreCase).SubstringToFirst(";")
                       let path = setCookieHeader.SubstringFromLast("path=", StringComparison.OrdinalIgnoreCase).SubstringToFirst(";")
                       select new Cookie(name, cookieValue, path);
            }
        }
    }


    /// <summary>
    /// Common string parsing and manipulation functions
    /// </summary>
    public static class StringExtensions
    {
        #region Static Fields

        /// <summary>
        /// A list of punctuation marks to remove from the start and end of words.
        /// </summary>
        public static readonly char[] CleanWordsOfCharacters = { ' ', ',', '.', '\t', '\r', '\n', '-', '!', '?', '*', '&' };

        /// <summary>
        /// A list of characters and string combinations that separate lines in a body of text.
        /// </summary>
        public static readonly string[] LineSeparators = { System.Environment.NewLine, "\r", "\n" };

        /// <summary>
        /// A non-definitive list of words that should not have their first letter capitalized in a title.
        /// </summary>
        public static readonly string[] NonTitleCaseWords = { "a", "in", "the", "if", "be", "of", "on", "is", "but", "and", "my", "as", "are", "has", "was", "de", "or", "it" };

        /// <summary>
        /// A list of strings that are used to determine the start or end of words.
        /// </summary>
        public static readonly string[] WordSeparators = { " ", ",", ".", "\t", "\r", "\n", "!", "?" };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Enumerates all the lines in a string.
        /// </summary>
        /// <param name="text">
        /// The text to split into separate lines
        /// </param>
        /// <returns>
        /// Returns an enumerable sequence of lines if the text is not null, otherwise an empty sequence.
        /// </returns>
        [Pure]
        public static IEnumerable<string> Lines(this string text)
        {
            if (text == null)
            {
                return Enumerable.Empty<string>();
            }

            return text.Split(LineSeparators, StringSplitOptions.None);
        }

        /// <summary>
        /// Enumerates all the words from a string, cleaning the words of any
        /// trailing or leading whitespace or punctuation as it goes.
        /// NOTE: This is not safe to be used with cultures other than English.
        /// </summary>
        /// <param name="text">
        /// The text to extract the words from.
        /// </param>
        /// <returns>
        /// Returns an enumerable sequence of strings, one per word.
        /// </returns>
        [Pure]
        public static IEnumerable<string> Words(this string text)
        {
            return text.Words(true);
        }

        /// <summary>
        /// Enumerates all the words from a string, optionally cleaning the words
        /// of any trailing or leading whitespace or punctuation as it goes.
        /// NOTE: This is not safe to be used with cultures other than English.
        /// </summary>
        /// <param name="text">
        /// The text to extract the words from.
        /// </param>
        /// <param name="cleanWords">
        /// A value indicating whether each words should be cleaned of any trailing or leading whitespace or punctuation.
        /// </param>
        /// <returns>
        /// Returns an enumerable sequence of strings, one per word.
        /// </returns>
        [Pure]
        public static IEnumerable<string> Words(this string text, bool cleanWords)
        {
            if (text == null)
            {
                return Enumerable.Empty<string>();
            }

            IEnumerable<string> result = text.SplitAfter(WordSeparators);
            if (cleanWords)
            {
                result = result.Select(w => w.Trim(CleanWordsOfCharacters)).Where(s => !s.IsNullOrWhitespace());
            }

            return result;
        }

        /// <summary>
        /// Converts the single Pascal cased word into a sentence phrase.
        /// </summary>
        /// <param name="pascalCasedPhrase">
        /// The phrase pascal cased as a single work e.g. TheQuickBrownFox
        /// </param>
        /// <param name="convertToLower">
        /// Whether to convert to all lower case
        /// </param>
        /// <param name="capitalizedWords">
        /// The words that must be capitalized
        /// </param>
        /// <returns>
        /// Returns the new sentence if not null, otherwise returns an empty string
        /// </returns>
        [Pure]
        public static string ToPhrase(this string pascalCasedPhrase, bool convertToLower, IList<string> capitalizedWords)
        {
            if (string.IsNullOrEmpty(pascalCasedPhrase))
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            builder.Append(pascalCasedPhrase[0]);

            bool lastWasDigit = false;
            for (int i = 1; i < pascalCasedPhrase.Length; i++)
            {
                if (char.IsUpper(pascalCasedPhrase[i]))
                {
                    lastWasDigit = false;
                    builder.Append(" ");
                }
                else if (char.IsDigit(pascalCasedPhrase[i]))
                {
                    if (!lastWasDigit)
                    {
                        lastWasDigit = true;
                        builder.Append(" ");
                    }
                }
                else
                {
                    lastWasDigit = false;
                }

                builder.Append(pascalCasedPhrase[i]);
            }

            if (convertToLower)
            {
                builder = new StringBuilder(builder.ToString().ToLowerInvariant());

                // Make special words capitalized
                if (capitalizedWords != null)
                {
                    builder = capitalizedWords.Aggregate(builder, (current, word) => current.Replace(word.ToLower() + " ", word + " "));
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="String"/> occurs within this string.
        /// </summary>
        /// <param name="text">
        /// The string to search in.
        /// </param>
        /// <param name="value">
        /// The string to seek.
        /// </param>
        /// <param name="comparisonType">
        /// A parameter that specifies the type of search to perform for the specified string.
        /// </param>
        /// <returns>
        /// Returns a value indicating whether the specified <see cref="String"/> occurs within this string.
        /// </returns>
        [Pure]
        public static bool Contains(this string text, string value, StringComparison comparisonType)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return text.IndexOf(value, comparisonType) != -1;
        }

        /// <summary>
        /// Enumerates all the sentences from a string, cleaning the sentences of any
        /// trailing or leading whitespace or punctuation (other than the trailing period) as it goes.
        /// Excludes any null or whitespace sentences.
        /// NOTE: This is not safe to be used with cultures other than English.
        /// </summary>
        /// <param name="text">
        /// The text to extract the sentences from.
        /// </param>
        /// <returns>
        /// Returns an enumerable sequence of strings, one per sentence.
        /// </returns>
        [Pure]
        public static IEnumerable<string> Sentences(this string text)
        {
            if (text == null)
            {
                return Enumerable.Empty<string>();
            }

            return from rawSentence in text.SplitAfter(".") let cleanedSentence = rawSentence.Trim(' ', '\t', '\r', '\n') where !cleanedSentence.IsNullOrWhitespace() select cleanedSentence;
        }

        /// <summary>
        /// Performs a non-consuming split of a string after each occurrence of
        /// any of the specified delimiters. Each string will keep it's delimiter
        /// at the end of it. This overload performs a case-sensitive search.
        /// </summary>
        /// <param name="text">
        /// The text to be split.
        /// </param>
        /// <param name="delimiters">
        /// The list of delimiters to split by.
        /// </param>
        /// <returns>
        /// Returns an enumerable sequence of strings.
        /// </returns>
        [Pure]
        public static IEnumerable<string> SplitAfter(this string text, params string[] delimiters)
        {
            return text.SplitAfter(StringComparison.Ordinal, delimiters);
        }

        /// <summary>
        /// Performs a non-consuming split of a string after each occurrence of
        /// any of the specified delimiters. Each string will keep it's delimiter
        /// at the end of it.
        /// </summary>
        /// <param name="text">
        /// The text to split.
        /// </param>
        /// <param name="comparisonType">
        /// The string comparison to perform on the delimiters,
        /// this allows for case insensitive splits etc.
        /// </param>
        /// <param name="delimiters">
        /// The list of delimiters to split by.
        /// </param>
        /// <returns>
        /// Returns an enumerable sequence of strings.
        /// </returns>
        [Pure]
        public static IEnumerable<string> SplitAfter(this string text, StringComparison comparisonType, params string[] delimiters)
        {
            int lastIndex = 0;
            while (lastIndex != -1)
            {
                var index = lastIndex;
                var nextIndex = (from delim in delimiters let delimIndex = text.IndexOf(delim, index, comparisonType) where delimIndex != -1 select delimIndex + delim.Length).DefaultIfEmpty(-1).Min();
                if (nextIndex != -1)
                {
                    yield return text.Substring(lastIndex, nextIndex - lastIndex);
                }
                else
                {
                    yield return text.Substring(lastIndex);
                }

                lastIndex = nextIndex;
            }
        }

        /// <summary>
        /// Performs a non-consuming split of a string before each occurrence of
        /// any of the specified delimiters. Each string will keep it's delimiter
        /// at the end of it.  This overload performs a case-sensitive search.
        /// </summary>
        /// <param name="text">
        /// The text to split.
        /// </param>
        /// <param name="delimiters">
        /// The list of delimiters to split by.
        /// </param>
        /// <returns>
        /// Returns an enumerable sequence of strings.
        /// </returns>
        [Pure]
        public static IEnumerable<string> SplitBefore(this string text, params string[] delimiters)
        {
            return text.SplitBefore(StringComparison.Ordinal, delimiters);
        }

        /// <summary>
        /// Performs a non-consuming split of a string before each occurrence of
        /// any of the specified delimiters. Each string will keep it's delimiter
        /// at the end of it.
        /// </summary>
        /// <param name="text">
        /// The text to split.
        /// </param>
        /// <param name="comparisonType">
        /// The string comparison to perform on the delimiters,
        /// this allows for case insensitive splits etc.
        /// </param>
        /// <param name="delimiters">
        /// The list of delimiters to split by.
        /// </param>
        /// <returns>
        /// Returns an enumerable sequence of strings.
        /// </returns>
        [Pure]
        public static IEnumerable<string> SplitBefore(this string text, StringComparison comparisonType, params string[] delimiters)
        {
            int lastIndex = 0;
            int lastSearchIndex = 0;
            while (lastIndex != -1)
            {
                var index = lastSearchIndex;
                var nextIndex =
                    (from delim in delimiters let delimIndex = text.IndexOf(delim, index, comparisonType) where delimIndex != -1 orderby delimIndex select Tuple.Create(delimIndex, delim.Length)).DefaultIfEmpty(
                        Tuple.Create(-1, 0)).First();

                if (nextIndex.Item1 != -1)
                {
                    yield return text.Substring(lastIndex, nextIndex.Item1 - lastIndex);
                    lastIndex = nextIndex.Item1;
                    lastSearchIndex = nextIndex.Item1 + nextIndex.Item2;
                }
                else
                {
                    yield return text.Substring(lastIndex);
                    yield break;
                }
            }
        }

        /// <summary>
        /// Performs a non-consuming split of a string before each occurrence of
        /// any of the specified delimiters. Each string will keep it's delimiter
        /// at the end of it.  This overload performs a case-sensitive search.
        /// </summary>
        /// <param name="text">
        /// The text to split.
        /// </param>
        /// <param name="delimiters">
        /// The list of delimiters to split by.
        /// </param>
        /// <returns>
        /// Returns an enumerable sequence of strings.
        /// </returns>
        [Pure]
        public static IEnumerable<string> SplitAfter(this IEnumerable<string> text, params string[] delimiters)
        {
            return text.SelectMany(t => t.SplitAfter(delimiters));
        }

        /// <summary>
        /// Performs a non-consuming split of a string before each occurrence of
        /// any of the specified delimiters. Each string will keep it's delimiter
        /// at the end of it.  This overload performs a case-sensitive search.
        /// </summary>
        /// <param name="text">
        /// The text to split.
        /// </param>
        /// <param name="delimiters">
        /// The list of delimiters to split by.
        /// </param>
        /// <returns>
        /// Returns an enumerable sequence of strings.
        /// </returns>
        [Pure]
        public static IEnumerable<string> SplitBefore(this IEnumerable<string> text, params string[] delimiters)
        {
            return text.SelectMany(t => t.SplitBefore(delimiters));
        }

        /// <summary>
        /// Splits a string after a maximum length has been reached
        /// </summary>
        /// <param name="text">
        /// The sequence of texts to be split
        /// </param>
        /// <param name="maxLength">
        /// The maximum length to split after
        /// </param>
        /// <returns>
        /// Returns a sequence of strings no longer than max length
        /// </returns>
        [Pure]
        public static IEnumerable<string> SplitAfterMaxLength(this IEnumerable<string> text, int maxLength)
        {
            foreach (var word in text)
            {
                for (int i = 0; i < word.Length;)
                {
                    int take = Math.Min(word.Length - i, maxLength);
                    yield return word.Substring(i, take);
                    i += take;
                }
            }
        }

        /// <summary>
        /// Concatenates strings while the concatenated result is acceptable.
        /// </summary>
        /// <param name="text">
        /// The sequence of texts to concatenate
        /// </param>
        /// <param name="shouldConcat">
        /// Predicate function that determines if the supplied concatenated result is valid
        /// </param>
        /// <returns>
        /// Returns the list of concatenated strings
        /// </returns>
        public static IEnumerable<string> ConcatWhile(this IEnumerable<string> text, Func<string, bool> shouldConcat)
        {
            var current = new StringBuilder();
            foreach (var word in text)
            {
                if (shouldConcat(current + word))
                {
                    current.Append(word);
                }
                else
                {
                    yield return current.ToString();
                    current = new StringBuilder(word);
                }
            }

            yield return current.ToString();
        }

        /// <summary>
        /// Extension shorthand for static method <see cref="string.IsNullOrWhiteSpace"/>.
        /// </summary>
        /// <param name="text">
        /// The text to check for null or whitespace.
        /// </param>
        /// <returns>
        /// Returns true if null, empty or whitespace, otherwise false.
        /// </returns>
        [Pure]
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        /// <summary>
        /// Extension shorthand for static method <see cref="string.IsNullOrWhiteSpace"/>.
        /// </summary>
        /// <param name="text">
        /// The text to check for null or whitespace.
        /// </param>
        /// <returns>
        /// Returns true if null, empty or whitespace, otherwise false.
        /// </returns>
        [Pure]
        public static bool IsNullOrWhitespace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        /// <summary>
        /// Counts the number of spaces in the given text, ignoring double spaces.
        /// </summary>
        /// <param name="text">
        /// The text to count the number of words in.
        /// </param>
        /// <returns>
        /// If the text is not null returns the number of non-empty words, otherwise returns zero.
        /// </returns>
        [Obsolete("Use Words() and Count() extension method instead, which allows you to exclude small words etc.", false)]
        public static int WordCount(this string text)
        {
            if (text == null)
            {
                return 0;
            }

            return text.Words().Count();
        }

        /// <summary>
        /// Counts the number sentences by counting the number of full-stops (.) in the given text. Does not count empty sentences.
        /// </summary>
        /// <param name="text">
        /// The text to count the number of sentences in.
        /// </param>
        /// <returns>
        /// If the text is not null, returns the number of full-stops. Otherwise returns zero.
        /// </returns>
        [Obsolete("Use Sentences() and Count() extension method, which allows you to exclude small sentences etc.", false)]
        public static int SentenceCount(this string text)
        {
            if (text == null)
            {
                return 0;
            }

            return text.Sentences().Count();
        }

        /// <summary>
        /// Gets the camel case conversion of some text (lower cases the first letter).
        /// </summary>
        /// <param name="text">
        /// The already title or pascal cased text.
        /// </param>
        /// <returns>
        /// Returns the text with the first letter converted to lower case.
        /// </returns>
        [Pure]
        public static string CamelCase(this string text)
        {
            if (text == null)
            {
                return null;
            }

            if (text.Length == 0)
            {
                return string.Empty;
            }

            if (text.Length == 1)
            {
                return text.ToLower();
            }

            return text.Substring(0, 1).ToLower() + text.Substring(1);
        }

        /// <summary>
        /// Converts text to sentence case (uppercases the first letter of the first word).
        /// NOTE: This is not safe to be used with cultures other than English.
        /// </summary>
        /// <param name="text">
        /// Text to be sentence cased.
        /// </param>
        /// <returns>
        /// If text is not null, returns the input text with the first letter uppercased, otherwise returns null.
        /// </returns>
        [Pure]
        public static string SentenceCase(this string text)
        {
            if (text == null)
            {
                return null;
            }

            if (text.Length == 0)
            {
                return string.Empty;
            }

            var sentences = from sentence in text.Sentences() let casedSentence = sentence.Substring(0, 1).ToUpper() + sentence.Substring(1, sentence.Length - 1) select casedSentence;

            return sentences.Join(". ");
        }

        /// <summary>
        /// Converts the casing of lower case text into a title case.
        /// Does not capitalize small reserved words such as 'in', 'if', 'is' etc.
        /// NOTE: This is not safe to be used with cultures other than English.
        /// </summary>
        /// <param name="text">
        /// The lower case text to convert to title case.
        /// </param>
        /// <returns>
        /// Returns a string with most words capitalized.
        /// </returns>
        [Pure]
        public static string TitleCase(this string text)
        {
            if (text == null)
            {
                return null;
            }

            if (text.Length == 0)
            {
                return string.Empty;
            }

            var words = text.Words(false);
            var resultWords = (from word in words.Take(1) select word.Substring(0, 1).ToUpper() + word.Substring(1, word.Length - 1)).Concat(
                from word in words.Skip(1)
                let cleanedWord = word.Trim(CleanWordsOfCharacters)
                let casedWord = NonTitleCaseWords.Contains(cleanedWord) ? word.ToLower() : word.Length == 0 ? string.Empty : word.Substring(0, 1).ToUpper() + word.Substring(1, word.Length - 1)
                select casedWord);
            return resultWords.Concat();
        }

        /// <summary>
        /// Concatenates all of the strings in a enumerable sequence into a single string. Excludes any null strings from the resulting string.
        /// </summary>
        /// <param name="items">
        /// The enumerable sequence of strings to concatenate,
        /// </param>
        /// <returns>
        /// Returns a non null string if the enumerable sequence itself is not null, otherwise returns null.
        /// </returns>
        [Pure]
        public static string Concat(this IEnumerable<string> items)
        {
            if (items == null)
            {
                return null;
            }

            var sb = new StringBuilder();
            foreach (var item in items)
            {
                if (item != null)
                {
                    sb.Append(item);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Joins a sequence of strings with a separator (Enumerable equivalent of a call to string.Join() )
        /// </summary>
        /// <param name="items">
        /// The strings to join together with a separator between each
        /// </param>
        /// <param name="separator">
        /// The separator to be placed between them
        /// </param>
        /// <returns>
        /// A single string with all the passed in strings joined, separated by the separator
        /// </returns>
        [Pure]
        public static string Join(this IEnumerable<string> items, string separator)
        {
            if (separator == null)
            {
                throw new ArgumentNullException("separator");
            }

            if (items == null)
            {
                return null;
            }

            bool first = true;
            var sb = new StringBuilder();
            foreach (var item in items)
            {
                if (item != null)
                {
                    if (!first)
                    {
                        sb.Append(separator);
                    }
                    else
                    {
                        first = false;
                    }

                    sb.Append(item);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Joins a sequence of strings with a separator (Enumerable equivalent of a call to string.Join() )
        /// </summary>
        /// <typeparam name="T">
        /// The type of items to join as a list of strings
        /// </typeparam>
        /// <param name="items">
        /// The strings to join together with a separator between each
        /// </param>
        /// <param name="selector">
        /// The function to use to extract the string to be joined.
        /// </param>
        /// <param name="separator">
        /// The separator to be placed between them
        /// </param>
        /// <returns>
        /// A single string with all the passed in strings joined, separated by the separator
        /// </returns>
        [Pure]
        public static string Join<T>(this IEnumerable<T> items, Func<T, string> selector, string separator)
        {
            if (separator == null)
            {
                throw new ArgumentNullException("separator");
            }

            if (items == null)
            {
                return null;
            }

            return items.Select(selector).Join(separator);
        }

        /// <summary>
        /// Removes the start of the text if it equals the specified text to be trimmed
        /// </summary>
        /// <param name="text">
        /// The text to be trimmed
        /// </param>
        /// <param name="trimText">
        /// The text to remove from the start
        /// </param>
        /// <returns>
        /// If the text is not null then returns the text without the
        /// trimmed text at the start, otherwise returns null
        /// </returns>
        [Pure]
        public static string TrimStart(this string text, string trimText)
        {
            if (text == null)
            {
                return null;
            }

            return text.StartsWith(trimText) ? text.Substring(trimText.Length) : text;
        }

        /// <summary>
        /// Removes the start of the text if it equals the specified text to be trimmed
        /// </summary>
        /// <param name="text">
        /// The text to be trimmed
        /// </param>
        /// <param name="trimText">
        /// The text to remove from the start
        /// </param>
        /// <param name="comparisonType">
        /// The type of string comparison to perform.
        /// </param>
        /// <returns>
        /// If the text is not null then returns the text without the
        /// trimmed text at the start, otherwise returns null
        /// </returns>
        [Pure]
        public static string TrimStart(this string text, string trimText, StringComparison comparisonType)
        {
            if (text == null)
            {
                return null;
            }

            return text.StartsWith(trimText, comparisonType) ? text.Substring(trimText.Length) : text;
        }

        /// <summary>
        /// Removes the end of the text if it equals the specified text to be trimmed
        /// </summary>
        /// <param name="text">
        /// The text to be trimmed
        /// </param>
        /// <param name="trimText">
        /// The text to remove from the end
        /// </param>
        /// <returns>
        /// If the text is not null then returns the text without the
        /// trimmed text at the end, otherwise returns null
        /// </returns>
        [Pure]
        public static string TrimEnd(this string text, string trimText)
        {
            if (text == null)
            {
                return null;
            }

            return text.EndsWith(trimText) ? text.Remove(text.Length - trimText.Length) : text;
        }

        /// <summary>
        /// Removes the end of the text if it equals the specified text to be trimmed
        /// </summary>
        /// <param name="text">
        /// The text to be trimmed
        /// </param>
        /// <param name="trimText">
        /// The text to remove from the end
        /// </param>
        /// <param name="comparisonType">
        /// The type of string comparison to perform.
        /// </param>
        /// <returns>
        /// If the text is not null then returns the text without the
        /// trimmed text at the end, otherwise returns null
        /// </returns>
        [Pure]
        public static string TrimEnd(this string text, string trimText, StringComparison comparisonType)
        {
            if (text == null)
            {
                return null;
            }

            return text.EndsWith(trimText, comparisonType) ? text.Remove(text.Length - trimText.Length) : text;
        }

        /// <summary>
        /// Gets the part of the string leading up to the last occurrence of the specified delimiter
        /// (does not include the last delimiter in the result).
        /// </summary>
        /// <param name="text">
        /// The text to get the substring of.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter to search for the last occurrence of.
        /// </param>
        /// <returns>
        /// Returns the part of the text before the last occurrence of the delimiter if present,
        /// otherwise returns all of the text,
        /// if text is null then returns null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if delimiter is null.
        /// </exception>
        [Pure]
        public static string SubstringToLast(this string text, string delimiter)
        {
            return SubstringToLast(text, delimiter, StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets the part of the string leading up to the last occurrence of the specified delimiter
        /// (does not include the last delimiter in the result).
        /// </summary>
        /// <param name="text">
        /// The text to get the substring of.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter to search for the last occurrence of.
        /// </param>
        /// <param name="comparison">
        /// The kind of string comparison to perform on the delimiter against the text
        /// </param>
        /// <returns>
        /// Returns the part of the text before the last occurrence of the delimiter if present,
        /// otherwise returns all of the text, if text is null then returns null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if delimiter is null.
        /// </exception>
        [Pure]
        public static string SubstringToLast(this string text, string delimiter, StringComparison comparison)
        {
            if (text == null)
            {
                return null;
            }

            if (delimiter == null)
            {
                throw new ArgumentNullException("delimiter");
            }

            var lastIndex = text.LastIndexOf(delimiter, comparison);
            return lastIndex == -1 ? text : text.Substring(0, lastIndex);
        }

        /// <summary>
        /// Gets the part of the string starting from the last occurrence of the
        /// specified delimiter excluding the delimiter from the result.
        /// This overload performs a case-sensitive match.
        /// </summary>
        /// <param name="text">
        /// The text to get the substring of.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter to search for the last occurrence of.
        /// </param>
        /// <returns>
        /// Returns the part of the text after the last occurrence of the delimiter if present,
        /// otherwise returns all of the text, if text is null then returns null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if delimiter is null.
        /// </exception>
        [Pure]
        public static string SubstringFromLast(this string text, string delimiter)
        {
            return SubstringFromLast(text, delimiter, StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets the part of the string starting from the last occurrence of the
        /// specified delimiter excluding the delimiter from the result.
        /// </summary>
        /// <param name="text">
        /// The text to get the substring of.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter to search for the last occurrence of.
        /// </param>
        /// <param name="comparison">
        /// The kind of string comparison to perform on the delimiter against the text
        /// </param>
        /// <returns>
        /// Returns the part of the text after the last occurrence of the delimiter if present,
        /// otherwise returns all of the text, if text is null then returns null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if delimiter is null.
        /// </exception>
        [Pure]
        public static string SubstringFromLast(this string text, string delimiter, StringComparison comparison)
        {
            if (text == null)
            {
                return null;
            }

            if (delimiter == null)
            {
                throw new ArgumentNullException("delimiter");
            }

            var lastIndex = text.LastIndexOf(delimiter, comparison);
            return lastIndex == -1 ? text : lastIndex >= text.Length ? text : text.Substring(lastIndex + delimiter.Length);
        }

        /// <summary>
        /// Gets the part of the string leading up to the first occurrence of the
        /// specified delimiter excluding the delimiter from the result.
        /// This overload performs a case sensitive match.
        /// </summary>
        /// <param name="text">
        /// The text to get the substring of.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter to search for the last occurrence of.
        /// </param>
        /// <returns>
        /// Returns the part of the text before the first occurrence of the delimiter if present,
        /// otherwise returns all of the text, if text is null then returns null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if delimiter is null.
        /// </exception>
        [Pure]
        public static string SubstringToFirst(this string text, string delimiter)
        {
            return SubstringToFirst(text, delimiter, StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets the part of the string leading up to the first occurrence of the
        /// specified delimiter excluding the delimiter from the result.
        /// </summary>
        /// <param name="text">
        /// The text to get the substring of.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter to search for the last occurrence of.
        /// </param>
        /// <param name="comparison">
        /// The kind of string comparison to perform on the delimiter against the text
        /// </param>
        /// <returns>
        /// Returns the part of the text before the first occurrence of the delimiter if present,
        /// otherwise returns all of the text, if text is null then returns null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if delimiter is null.
        /// </exception>
        [Pure]
        public static string SubstringToFirst(this string text, string delimiter, StringComparison comparison)
        {
            if (text == null)
            {
                return null;
            }

            if (delimiter == null)
            {
                throw new ArgumentNullException("delimiter");
            }

            var firstIndex = text.IndexOf(delimiter, comparison);
            return firstIndex == -1 ? text : text.Substring(0, firstIndex);
        }

        /// <summary>
        /// Gets the part of the string starting from the first occurrence of
        /// the specified delimiter excluding the delimiter from the result. Will return all of the text if the delimiter is not present
        /// This overload performs a case sensitive match.
        /// </summary>
        /// <param name="text">
        /// The text to get the substring of.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter to search for the last occurrence of.
        /// </param>
        /// <returns>
        /// Returns the part of the text after the first occurrence of the delimiter if present,
        /// otherwise returns all of the text, if text is null then returns null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if delimiter is null.
        /// </exception>
        [Pure]
        public static string SubstringFromFirst(this string text, string delimiter)
        {
            return SubstringFromFirst(text, delimiter, StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets the part of the string starting from the first occurrence of
        /// the specified delimiter excluding the delimiter from the result. Will return all of the text if the delimiter is not present
        /// </summary>
        /// <param name="text">
        /// The text to get the substring of.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter to search for the last occurrence of.
        /// </param>
        /// <param name="comparison">
        /// The kind of string comparison to perform on the delimiter against the text
        /// </param>
        /// <returns>
        /// Returns the part of the text after the first occurrence of the delimiter if present,
        /// otherwise returns all of the text, if text is null then returns null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if delimiter is null.
        /// </exception>
        [Pure]
        public static string SubstringFromFirst(this string text, string delimiter, StringComparison comparison)
        {
            if (text == null)
            {
                return null;
            }

            if (delimiter == null)
            {
                throw new ArgumentNullException("delimiter");
            }

            var firstIndex = text.IndexOf(delimiter, comparison);
            return firstIndex == -1 || firstIndex >= text.Length ? text : text.Substring(firstIndex + delimiter.Length);
        }

        /// <summary>
        /// Marks text to be localised (TODO: Replace with culture get)
        /// </summary>
        /// <param name="text">
        /// The text that needs to be localised
        /// </param>
        /// <returns>
        /// The original text
        /// </returns>
        public static string ToBeLocalised(this string text)
        {
            return text;
        }

        /// <summary>
        /// Gets the left most number of characters
        /// </summary>
        /// <param name="text">
        /// </param>
        /// <param name="count">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Left(this string text, int count)
        {
            if (text == null)
            {
                return null;
            }

            return text.Substring(0, Math.Min(count, text.Length));
        }

        /// <summary>
        /// Joins strings that aren't empty
        /// </summary>
        /// <param name="items">
        /// The list of items to join
        /// </param>
        /// <param name="separator">
        /// The string separator
        /// </param>
        /// <returns>
        /// The joined string
        /// </returns>
        public static string JoinNonEmpty(this IEnumerable<string> items, string separator)
        {
            return string.Join(separator, items.Where(i => !string.IsNullOrEmpty(i)));
        }

        #endregion
    }
}

