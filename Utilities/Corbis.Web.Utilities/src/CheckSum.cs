using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Corbis.Web.Utilities
{
    public static class CheckSum
    {
        private static byte[] saltBytes = new byte[] { 243, 80, 22, 127, 93, 162, 175, 180, 192, 71, 96, 43, 37, 212, 160, 101 };
        private static Encoding latin = Encoding.GetEncoding("ISO-8859-1");

        /// <summary>
        /// Computes the check sum of a string to make sure it hasn't been tampered with.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The Base64 encoded Checksum
        /// </returns>
        public static string ComputeCheckSum(string value)
        {
            byte[] valueBuff = latin.GetBytes(value);
            byte[] buffToHash = new byte[saltBytes.Length + valueBuff.Length];
            Array.Copy(saltBytes, buffToHash, saltBytes.Length);
            Array.Copy(valueBuff, 0, buffToHash, saltBytes.Length, valueBuff.Length);
            MD5 md5 = MD5.Create();
            byte[] hashedBuff = md5.ComputeHash(buffToHash);
            string bas64HashedBuff = Convert.ToBase64String(hashedBuff);

            return bas64HashedBuff;
        }

        /// <summary>
        /// Validates the check sum.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="checkSum">
        /// The checksum computed by calling ComputeCheckSum with the value
        /// </param>
        /// <returns></returns>
        public static bool ValidateCheckSum(string value, string checkSum)
        {
            return ComputeCheckSum(value) == checkSum;
        }
    }
}
