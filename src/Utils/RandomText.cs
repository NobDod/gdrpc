using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.Utils
{
    class RandomText
    {
        /// <summary>
        /// Random text with chars
        /// </summary>
        /// <param name="len">Length for randoming char</param>
        /// <returns>Random text</returns>
        public static string Run(int len = 16)
        {
            Random random = new Random();
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, len).Select(s => s[random.Next(0, s.Length)]).ToArray());
        }
    }
}
