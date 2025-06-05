using System.Net.Sockets;
using System.Net;
using M9Studio.SecureStream;
using M9Studio.ShadowTalk.Core;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using M9Studio.ShadowTalk.Client.Packet;

namespace M9Studio.ShadowTalk.Client
{
    partial class Core
    {
        public static string DecryptWithRSA(string base64EncryptedText, string privateKeyXml)
        {
            using var rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
            rsa.FromXmlString(privateKeyXml);

            byte[] encryptedBytes = Convert.FromBase64String(base64EncryptedText);
            byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, false);

            return System.Text.Encoding.UTF8.GetString(decryptedBytes);
        }
        public static string DecryptAesBase64(string base64CipherText, string base64Key)
        {
            byte[] cipherBytes = Convert.FromBase64String(base64CipherText);
            byte[] keyBytes = Convert.FromBase64String(base64Key);

            // IV берём из начала сообщения (если он туда добавлен), либо задаём явно:
            byte[] iv = new byte[16]; // Можно использовать фиксированный IV или получать отдельно
            Array.Copy(cipherBytes, 0, iv, 0, 16);

            byte[] actualCipher = new byte[cipherBytes.Length - 16];
            Array.Copy(cipherBytes, 16, actualCipher, 0, actualCipher.Length);

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            byte[] decryptedBytes = decryptor.TransformFinalBlock(actualCipher, 0, actualCipher.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
        public static string EncryptWithRSA(string plainText, string publicKeyXml)
        {
            using var rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
            rsa.FromXmlString(publicKeyXml);

            byte[] dataBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = rsa.Encrypt(dataBytes, false); // false = PKCS#1 v1.5 padding

            return Convert.ToBase64String(encryptedBytes);
        }
        public static string EncryptAesBase64(string plainText, string base64Key)
        {
            byte[] keyBytes = Convert.FromBase64String(base64Key);
            byte[] iv = RandomNumberGenerator.GetBytes(16); // 16 байт IV

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // Префикс IV к шифртексту (как ты предполагаешь в DecryptAesBase64)
            byte[] result = new byte[iv.Length + cipherBytes.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(cipherBytes, 0, result, iv.Length, cipherBytes.Length);

            return Convert.ToBase64String(result);
        }


        public static (string publicKey, string privateKey) GenRSA()
        {
            using var rsa = new System.Security.Cryptography.RSACryptoServiceProvider(2048);
            string publicKey = rsa.ToXmlString(false);  // только публичный ключ
            string privateKey = rsa.ToXmlString(true);  // полный ключ (приватный + публичный)
            return (publicKey, privateKey);
        }
        public static string GenAesKey(int keySizeBits = 256)
        {
            int keySizeBytes = keySizeBits / 8;
            byte[] key = RandomNumberGenerator.GetBytes(keySizeBytes);
            return Convert.ToBase64String(key);
        }

    }
}
