using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace GoogleAuth
{
    public static class Utitity
    {
        private const string ReservedChars = @"`!@#$%^&*()_-+=.~,:;'?/|\[] ";
        private const string UnReservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        /// <summary>
        /// Url Encoding
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in value)
            {
                if (UnReservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
        }
    }
}