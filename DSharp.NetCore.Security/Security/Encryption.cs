
using System.Security.Cryptography;
using System.Text;

namespace DSharp.NetCore.Security;

/// <summary>
/// 
/// </summary>
public static class Encryption
{
    /// <summary>
    /// 
    /// </summary>
    public enum EncryptionType
    {
        /// <summary>
        /// 
        /// </summary>
        Rijndael,

        /// <summary>
        /// 
        /// </summary>
        Aes
    }

    private enum Method
    {
        Encrypt,
        Decrypt
    }

    private const string Key256 = "B5B54EB11F74E1169BA437D41DC8DECA";

    private const string InitVector = "9BA437D41DC8DECA";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public static byte[] Key(Guid guid)
    {
        return Encoding.ASCII.GetBytes($"{guid:N}".ToUpper());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hex"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static byte[] Key(string hex = "", int length = 0) // if empty, use max encryption
    {
        if (length == 0)
            length = hex.Length;

        hex = $"{hex}{Key256}";

        return length switch
        {
            <= 16 => Encoding.ASCII.GetBytes(hex.Substring(0, 16)),
            <= 24 => Encoding.ASCII.GetBytes(hex.Substring(0, 24)),
            _ => Encoding.ASCII.GetBytes(hex.Substring(0, 32))
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static byte[] InitializationVector(string hex = "")
    {
        return Encoding.ASCII.GetBytes($"{hex}{InitVector}".Substring(0, 16));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="key"></param>
    /// <param name="initializationVector"></param>
    /// <returns></returns>
    public static string EncryptRijndael(string plainText, byte[]? key = null, byte[]? initializationVector = null)
    {
        return Encrypt(EncryptionType.Rijndael, plainText, key, initializationVector);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="plainBytes"></param>
    /// <param name="key"></param>
    /// <param name="initializationVector"></param>
    /// <returns></returns>
    public static byte[] EncryptRijndael(byte[] plainBytes, byte[]? key = null, byte[]? initializationVector = null)
    {
        return Encrypt(EncryptionType.Rijndael, plainBytes, key, initializationVector);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cipherText"></param>
    /// <param name="key"></param>
    /// <param name="initializationVector"></param>
    /// <returns></returns>
    public static string DecryptRijndael(string cipherText, byte[]? key = null, byte[]? initializationVector = null)
    {
        return Decrypt(EncryptionType.Rijndael, cipherText, key, initializationVector);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cipherBytes"></param>
    /// <param name="key"></param>
    /// <param name="initializationVector"></param>
    /// <returns></returns>
    public static byte[] DecryptRijndael(byte[] cipherBytes, byte[]? key = null, byte[]? initializationVector = null)
    {
        return Decrypt(EncryptionType.Rijndael, cipherBytes, key, initializationVector);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="key"></param>
    /// <param name="initializationVector"></param>
    /// <returns></returns>
    public static string EncryptAes(string plainText, byte[]? key = null, byte[]? initializationVector = null)
    {
        return Encrypt(EncryptionType.Aes, plainText, key, initializationVector);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="plainBytes"></param>
    /// <param name="key"></param>
    /// <param name="initializationVector"></param>
    /// <returns></returns>
    public static byte[] EncryptAes(byte[] plainBytes, byte[]? key = null, byte[]? initializationVector = null)
    {
        return Encrypt(EncryptionType.Aes, plainBytes, key, initializationVector);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cipherText"></param>
    /// <param name="key"></param>
    /// <param name="initializationVector"></param>
    /// <returns></returns>
    public static string DecryptAes(string cipherText, byte[]? key = null, byte[]? initializationVector = null)
    {
        return Decrypt(EncryptionType.Aes, cipherText, key, initializationVector);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cipherBytes"></param>
    /// <param name="key"></param>
    /// <param name="initializationVector"></param>
    /// <returns></returns>
    public static byte[] DecryptAes(byte[] cipherBytes, byte[]? key = null, byte[]? initializationVector = null)
    {
        return Decrypt(EncryptionType.Aes, cipherBytes, key, initializationVector);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encryptionType"></param>
    /// <param name="plainText"></param>
    /// <param name="key"></param>
    /// <param name="initializationVector"></param>
    /// <returns></returns>
    public static string Encrypt(EncryptionType encryptionType, string plainText, byte[]? key = null, byte[]? initializationVector = null)
    {
        // Check arguments.
        Guard.IsNotEmpty(plainText, "There is nothing to encrypt!");

        var cipherBytes = Encrypt(encryptionType, Encoding.UTF8.GetBytes(plainText), key, initializationVector);
        return Convert.ToBase64String(cipherBytes);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encryptionType"></param>
    /// <param name="plainBytes"></param>
    /// <param name="key"></param>
    /// <param name="initializationVector"></param>
    /// <returns></returns>
    public static byte[] Encrypt(EncryptionType encryptionType, byte[] plainBytes, byte[]? key = null, byte[]? initializationVector = null)
    {
        key ??= Key();
        initializationVector ??= InitializationVector();

        var transform = CreateCryptoTransform(encryptionType, key, initializationVector, Method.Encrypt);

        // Create the streams used for encryption.
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
        cs.Write(plainBytes, 0, plainBytes.Length);
        cs.FlushFinalBlock();

        // Return the encrypted bytes from the memory stream.
        var result = ms.ToArray();
        ms.Close();
        cs.Close();
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encryptionType"></param>
    /// <param name="cipherText"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    public static string Decrypt(EncryptionType encryptionType, string cipherText, byte[]? key = null, byte[]? iv = null)
    {
        // Check arguments.
        Guard.IsNotEmpty(cipherText, "There is nothing to Decrypt!");
        key ??= Key();
        iv ??= InitializationVector();

        var cipherBytes = Convert.FromBase64String(cipherText);

        var transform = CreateCryptoTransform(encryptionType, key, iv, Method.Decrypt);

        // Create the streams used for decryption.
        using var ms = new MemoryStream(cipherBytes);

        using var cs = new CryptoStream(ms, transform, CryptoStreamMode.Read);

        using var sr = new StreamReader(cs);
        var text = sr.ReadToEnd();

        return text;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encryptionType"></param>
    /// <param name="cipherBytes"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    public static byte[] Decrypt(EncryptionType encryptionType, byte[] cipherBytes, byte[]? key = null, byte[]? iv = null)
    {
        key ??= Key();
        iv ??= InitializationVector();

        var transform = CreateCryptoTransform(encryptionType, key, iv, Method.Decrypt);

        // Create the streams used for decryption.
        using var ms = new MemoryStream(cipherBytes);

        using var cs = new CryptoStream(ms, transform, CryptoStreamMode.Read);

        using var sr = new StreamReader(cs);
        var text = sr.ReadToEnd();

        return Encoding.UTF8.GetBytes(text);
    }

    private static ICryptoTransform CreateCryptoTransform(EncryptionType encryptionType, byte[] key, byte[] initializationVector, Method method)
    {
        if (encryptionType == EncryptionType.Aes)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = initializationVector;

            return method == Method.Encrypt
                ? aes.CreateEncryptor(aes.Key, aes.IV)
                : aes.CreateDecryptor(aes.Key, aes.IV);
        }

#pragma warning disable SYSLIB0022 // Type or member is obsolete
        using var managed = new RijndaelManaged();
        managed.Mode = CipherMode.ECB;
        managed.Key = key;
        managed.IV = initializationVector;

#pragma warning restore SYSLIB0022 // Type or member is obsolete
        return method == Method.Encrypt
            ? managed.CreateEncryptor()
            : managed.CreateDecryptor();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="base64EncodedData"></param>
    /// <returns></returns>
    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
