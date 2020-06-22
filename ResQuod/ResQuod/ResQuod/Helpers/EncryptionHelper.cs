using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ResQuod.Helpers
{
    static class EncryptionHelper
    {
        private const string DefaultKeyText = "okres godowy chomika trwa 3 miesiace";
        private const string DefaultSaltText = "super secure salt";

        public static string EncryptWithAes(string plainText)
        {
            byte[] encryptedData;
            using (var aes = CreateDefaultKey())
            {
                encryptedData = EncryptWithAes(plainText, aes.Key, aes.IV);
            }
            return BytesToString(encryptedData);
        }

        public static byte[] EncryptWithAes(string plainText, byte[] key, byte[] IV)
        {
            byte[] encryptedData;

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = IV;
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        encryptedData = ms.ToArray();
                    }
                }
            }

            return encryptedData;
        }

        public static string DecryptFromAes(string cipherText)
        {
            string plainText;
            using (var aes = CreateDefaultKey())
            {
                plainText = DecryptFromAes(StringToBytes(cipherText), aes.Key, aes.IV);
            }
            return plainText;
        }

        public static string DecryptFromAes(byte[] encryptedData, byte[] key, byte[] IV)
        {
            string plainText = null;

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = IV;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(encryptedData))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            plainText = sr.ReadToEnd();
                        }
                    }
                }
            }

            return plainText;
        }

        private static Aes CreateDefaultKey()
        {
            var aes = Aes.Create();
            var salt = Encoding.UTF8.GetBytes(DefaultSaltText);
            var key = new Rfc2898DeriveBytes(DefaultKeyText, salt);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            return aes;
        }

        private static string BytesToString(byte[] data)
        {
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private static byte[] StringToBytes(string data)
        {
            var outputLength = data.Length / 2;
            var output = new byte[outputLength];
            for (var i = 0; i < outputLength; i++)
                output[i] = Convert.ToByte(data.Substring(i * 2, 2), 16);
            return output;
        }
    }
}
