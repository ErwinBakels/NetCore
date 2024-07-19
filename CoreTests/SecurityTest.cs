using DSharp.NetCore.Security;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics.CodeAnalysis;

namespace DSharp.NetCore;

[ExcludeFromCodeCoverage]
[TestClass]
public class SecurityTest
{
    [TestMethod]
    public void EncryptAes()
    {
        var parentId = "0017Y00001VpugwQAB";
        var documentId = "0067Y00000BZllwQAD";

        var text = "Hallo";

        var key = Encryption.Key(parentId);
        var iv = Encryption.InitializationVector(documentId);
        var text2 = Encryption.EncryptAes(text, key, iv);

        var text3 = Encryption.DecryptAes(text2, key, iv);

        Assert.AreEqual(text, text3);

    }
    [TestMethod]
    public void EncryptAes2()
    {
        var key = new Guid("805a51e1-dcfa-4d98-b541-911290f4f9ce").ToString("N").ToUpper();
        var iv = "805a51e1dcfa4d98b541911290f4f9ce"[..16];

        var key2 = Encryption.Key(key);
        var iv2 = Encryption.InitializationVector(iv);

        var text = @"{""Username"":""launcher@enabled.com"",""Password"":""123456""}";

        var text2 = Encryption.EncryptRijndael(text, key2, iv2);

        Assert.AreEqual("uD5fsPywO0PgawWxcUNu+Koj0AjrLy7L0Ldm4QL4XB7NrHCa5+wNPeYfN6S+Ax16zPeylDAUX6HndaNqFcxA9Q==", text2);

        Console.WriteLine(text2);

        var text3 = Encryption.DecryptRijndael(text2, key2, iv2);

        Console.WriteLine(text3);

        Assert.AreEqual(text, text3);

    }

    [TestMethod]
    public void EncryptEncryptRijndael()
    {
        var parentId = "0017Y00001VpugwQAB";
        var documentId = "0067Y00000BZllwQAD";

        var text = "Hallo";

        var key = Encryption.Key(parentId);
        var iv = Encryption.InitializationVector(documentId);
        var text2 = Encryption.EncryptRijndael(text, key, iv);

        var text3 = Encryption.DecryptRijndael(text2, key, iv);

        Assert.AreEqual(text, text3);

    }
}
