using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace Sys
{
    public class HtmlCleaner
    {
        private static string ReplaceFirst(string haystack, string needle, string replacement)
        {
            int pos = haystack.IndexOf(needle);
            if (pos < 0) return haystack;
            return haystack.Substring(0, pos) + replacement + haystack.Substring(pos + needle.Length);
        }

        private static string ReplaceAll(string haystack, string needle, string replacement)
        {
            int pos;
            // Avoid a possible infinite loop
            if (needle == replacement) return haystack;
            while ((pos = haystack.IndexOf(needle)) > 0)
                haystack = haystack.Substring(0, pos) + replacement + haystack.Substring(pos + needle.Length);
            return haystack;
        }

        public static string StripTags(string Input, string[] AllowedTags)
        {
            Regex StripHTMLExp = new Regex(@"(<\/?[^>]+>)");
            string Output = Input;

            foreach (Match Tag in StripHTMLExp.Matches(Input))
            {
                string HTMLTag = Tag.Value.ToLower();
                bool IsAllowed = false;

                foreach (string AllowedTag in AllowedTags)
                {
                    int offset = -1;

                    // Determine if it is an allowed tag
                    // "<tag>" , "<tag " and "</tag"
                    if (offset != 0) offset = HTMLTag.IndexOf('<' + AllowedTag + '>');
                    if (offset != 0) offset = HTMLTag.IndexOf('<' + AllowedTag + ' ');
                    if (offset != 0) offset = HTMLTag.IndexOf("</" + AllowedTag);

                    // If it matched any of the above the tag is allowed
                    if (offset == 0)
                    {
                        IsAllowed = true;
                        break;
                    }
                }

                // Remove tags that are not allowed
                if (!IsAllowed) Output = ReplaceFirst(Output, Tag.Value, "");
            }

            return Output;
        }

        public static string StripTagsAndAttributes(string Input, string[] AllowedTags)
        {
            /* Remove all unwanted tags first */
            string Output = StripTags(Input, AllowedTags);

            /* Lambda functions */
            MatchEvaluator HrefMatch = m => m.Groups[1].Value + "href..;,;.." + m.Groups[2].Value;
            MatchEvaluator ClassMatch = m => m.Groups[1].Value + "class..;,;.." + m.Groups[2].Value;
            MatchEvaluator UnsafeMatch = m => m.Groups[1].Value + m.Groups[4].Value;

            /* Allow the "href" attribute */
            Output = new Regex("(<a.*)href=(.*>)").Replace(Output, HrefMatch);

            /* Allow the "class" attribute */
            Output = new Regex("(<a.*)class=(.*>)").Replace(Output, ClassMatch);

            /* Remove unsafe attributes in any of the remaining tags */
            Output = new Regex(@"(<.*) .*=(\'|\""|\w)[\w|.|(|)]*(\'|\""|\w)(.*>)").Replace(Output, UnsafeMatch);

            /* Return the allowed tags to their proper form */
            Output = ReplaceAll(Output, "..;,;..", "=");

            return Output;
        }

        public static string CleanWordHtml(string html)
        {
            StringCollection sc = new StringCollection();
            // get rid of unnecessary tag spans (comments and title)
            sc.Add(@"<!--(\w|\W)+?-->");
            sc.Add(@"<title>(\w|\W)+?</title>");
            // Get rid of classes and styles
            sc.Add(@"\s?class=\w+");
            sc.Add(@"\s+style='[^']+'");
            // Get rid of unnecessary tags
            sc.Add(@"<(meta|link|/?o:|/?style|/?div|/?st\d|/?head|/?html|body|/?body|/?span|!\[)[^>]*?>");
            // Get rid of empty paragraph tags
            sc.Add(@"(<[^>]+>)+&nbsp;(</\w+>)+");
            // remove bizarre v: element attached to <img> tag
            sc.Add(@"\s+v:\w+=""[^""]+""");
            // remove extra lines
            sc.Add(@"(\n\r){2,}");
            sc.Add(@"class=""\w+""");
            sc.Add(@"style=""\w+""");
            sc.Add(@"<script.*?</script>");
            sc.Add(@"<a\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>");
            foreach (string s in sc)
            {
                html = Regex.Replace(html, s, "", RegexOptions.IgnoreCase).Replace(" >", ">");
            }
            return html;
        }

        private static string FixEntities(string html)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("\"", "&ldquo;");
            nvc.Add("\"", "&rdquo;");
            nvc.Add("–", "&mdash;");
            foreach (string key in nvc.Keys)
            {
                html = html.Replace(key, nvc[key]);
            }
            return html;
        }

        /// <summary>
        /// чистимо штмл
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string CleanAll(string html)
        {
            html = CleanWordHtml(html);

            string tags = "a|b|em|strong|i|div|p|ul|li|ol|span|font";

            html = StripTagsAndAttributes(html, tags.Split('|'));
            html = StripTagsAndAttributes(html, tags.ToUpper().Split('|'));
            return html;
        }

        /// чистимо матюки
        public static string CleanObsceneWords(string text)
        {

            List<string> words = new List<string>() {
            "сука",
            "бля",
            "пизд",
            "пізд",
            "хуй",
            "xyй",
            "хyй",
            "xуй",
            "ганд",
            "курв",
            "уойб",
            "йоб",
            "ебав",
            "єбав",
            "ебаь",
            "єбав",
            };

            foreach (string word in words)
            {
                try
                {
                    if (word != "")
                        if (text.ToLower().IndexOf(word) != -1)
                        {
                            int indexFrom = text.ToLower().IndexOf(word);
                            int indexTo = indexFrom + word.Length;
                            string badW = text.Substring(indexFrom, indexTo);
                            text = text.Replace(badW, "🙈🙊🙉");
                        }
                }
                catch { }
            }


            return text;
        }
    }
}
