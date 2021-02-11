namespace Birder.Helpers
{
    public static class XenoCantoServiceHelpers
    {
        /// <summary>
        /// Builds the recording url from various feilds of the response object 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string BuildRecordingUrl(string baseUrl, string fileName)
        {
            var substring = baseUrl.Substring(0, IndexOfNth(baseUrl, '/', 6) + 1);
            return string.Concat(substring, fileName);
        }

        /// <summary>
        /// Returns index of the nth instance of char c in string str
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static int IndexOfNth(string str, char c, int n)
        {
            int remaining = n;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == c)
                {
                    remaining--;
                    if (remaining == 0)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Formats serach term by replacing spaces between words with '+'
        /// </summary>
        /// <param name="species"></param>
        /// <returns></returns>
        public static string FormatSearchTerm(string species)
        {
            return species.Replace(" ", "+");
        }

        /// <summary>
        /// Builds the Api url adding formatted search term & recording length to base url
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public static string BuildXenoCantoApiUrl(string searchTerm)
        {
            return $"https://www.xeno-canto.org/api/2/recordings?query=" +
                   $"{searchTerm}" +
                   $"+len_gt:40";
        }
    }
}
