using System;
using System.Collections.Generic;
using System.Text;

namespace ToolsPack.String
{
    public static class UriCombine
    {
        /// <summary>
        /// Example:
        /// Join("http://www.my.domain/", "relative/path") returns "http://www.my.domain/relative/path"
        /// Join("http://www.my.domain/something/other", "/absolute/path") returns "http://www.my.domain/absolute/path"
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="relativeOrAbsoluteUri"></param>
        /// <returns></returns>
        public static Uri Join(string baseUri, string relativeOrAbsoluteUri)
        {
            return new Uri(new Uri(baseUri), relativeOrAbsoluteUri);
        }
    }
}
