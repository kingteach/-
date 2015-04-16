using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ams.Utils
{
    /// <summary>
    /// 密码加解密方法
    /// </summary>
    public class EncryptUtil
    {
        #region DES 对称加解密的类，用私钥和密钥加解密

        /// <summary>
        /// DES加密函数
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Encrypt(string pToEncrypt)
        {
            if (string.IsNullOrEmpty(pToEncrypt))
                return string.Empty;
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.UTF8.GetBytes("87654321");//指定8位加密密钥
            provider.IV = Encoding.UTF8.GetBytes("12345678");//指定8位加密向量
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] buffer1 = Encoding.UTF8.GetBytes(pToEncrypt);
            cs.Write(buffer1, 0, buffer1.Length);
            cs.FlushFinalBlock();

            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

        }

        /// <summary>
        ///  解密字符串
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <returns></returns>
        public static string Decrypt(string pToDecrypt)
        {
            if (string.IsNullOrEmpty(pToDecrypt))
                return string.Empty;
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            try
            {
                //密钥和向量必须是8位的
                provider.Key = Encoding.UTF8.GetBytes("87654321");
                provider.IV = Encoding.UTF8.GetBytes("12345678");

                byte[] buffer = Convert.FromBase64String(pToDecrypt);
                MemoryStream ms = new MemoryStream(buffer);
                CryptoStream cs = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs, Encoding.UTF8);
                return sr.ReadToEnd();
            }
            catch (FormatException fe)
            {
                return string.Format("解密失败:\"{0}\"", fe.Message);
            }
        }
        #endregion

        #region MD5

        /// 获得32位的MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Encrypt32(string input)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获得16位的MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5_16(string input)
        {
            return Encrypt32(input).Substring(8, 16);
        }
        /// <summary>
        /// 获得8位的MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5_8(string input)
        {
            return Encrypt32(input).Substring(8, 8);
        }
        /// <summary>
        /// 获得4位的MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5_4(string input)
        {
            return Encrypt32(input).Substring(8, 4);
        }

        public static string MD5EncryptHash(String input)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //the GetBytes method returns byte array equavalent of a string
            byte[] res = md5.ComputeHash(Encoding.Default.GetBytes(input), 0, input.Length);
            char[] temp = new char[res.Length];
            //copy to a char array which can be passed to a String constructor
            Array.Copy(res, temp, res.Length);
            //return the result as a string
            return new String(temp);
        }

        #endregion
    }
}
