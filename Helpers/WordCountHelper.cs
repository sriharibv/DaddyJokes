using System;
namespace DaddyJokes.Helpers
{
    public static class WordCountHelper
    {
        public static int Count(string x)
        {
            //Trim whitespace from beginning and end of string
            x = x.Trim();

            if (string.IsNullOrEmpty(x))
                return 0;

            //Ensure there is only one space between each word in the passed string
            while (x.Contains("  "))
                x = x.Replace("  ", " ");

            //Count the words
            return x.Split(' ').Length;
        }
    }
}

