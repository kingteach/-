using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ams.Utils
{
    public static class StringUtil
    {
        public static string GetExtension(string str)
        {
            if (IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            int startIndex = str.LastIndexOf('.');
            if (startIndex == -1)
            {
                return string.Empty;
            }
            return str.Substring(startIndex);
        }

        /// <summary>
        /// 返回 GUID 用于数据库操作，特定的时间代码可以提高检索效率
        /// </summary>
        /// <returns></returns>
        public static string Guid()
        {
            byte[] guidArray = System.Guid.NewGuid().ToByteArray();
            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            // Get the days and milliseconds which will be used to build the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));
            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));
            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);
            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new System.Guid(guidArray).ToString();
        }

        /// <summary>
        /// 从 SQL SERVER 返回的 GUID 中生成时间信息
        /// </summary>
        /// <param name="guid">包含时间信息的 COMB </param>
        /// <returns>时间</returns>
        public static DateTime GetDateTime(Guid guid)
        {
            DateTime baseDate = new DateTime(1900, 1, 1);
            byte[] daysArray = new byte[4];
            byte[] msecsArray = new byte[4];
            byte[] guidArray = guid.ToByteArray();
            // Copy the date parts of the guid to the respective byte arrays. 
            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);
            // Reverse the arrays to put them into the appropriate order 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);
            // Convert the bytes to ints 
            int days = BitConverter.ToInt32(daysArray, 0);
            int msecs = BitConverter.ToInt32(msecsArray, 0);
            DateTime date = baseDate.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);
            return date;
        }

        public static bool IsNullOrEmpty(string str)
        {
            return !HasText(str);
        }

        public static bool HasText(string str)
        {
            if (str == null)
            {
                return false;
            }
            return str.Trim().Length > 0;
        }

        public static string GetUrlFileName(string url)
        {
            //http://localhost:9796/Content/temp/Cert/18-ac3f8def-6d2a-44ad-a166-a4110111bdbe.png
            if (!StringUtil.HasText(url)) return null;
            return url.Substring(url.LastIndexOf("/") + 1);
        }

        public static string CombinUrl(string url1, string url2)
        {
            url1 = url1 ?? string.Empty;
            url2 = url2 ?? string.Empty;
            return string.Format("{0}/{1}", url1.TrimEnd(new char[] { '/' }), url2.TrimEnd(new char[] { '/' }));
        }

        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="length">随机数长度</param>
        /// <returns></returns>
        public static string GeneralRandomCode(int length)
        {
            char[] myCodeChar = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            List<string> checkCodes = new List<string>();
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int number = random.Next(myCodeChar.Length);
                string code = ((char)(myCodeChar[number])).ToString();

                // 不生成重复字母
                if (checkCodes.Contains(code.ToUpper())
                    || checkCodes.Contains(code.ToLower()))
                {
                    i -= 1;
                    continue;
                }
                checkCodes.Add(code);
            }
            return string.Join("", checkCodes);
        }

        public static bool HasEmpty(params string[] strs)
        {
            if (strs.Length == 0) return true;
            return strs.Where(x => !HasText(x)).FirstOrDefault() != null;
        }
    }
}
