using DSharp.NetCore.Security;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace DSharp.NetCore.SecurityTests
{
    [TestClass]
    public class EncryptionTests
    {
        private const string TestString = "Hello, World!";
        private static readonly Guid TestGuid = Guid.NewGuid();

        [TestMethod]
        public void EncryptRijndaelAndDecryptRijndael_ShouldReturnOriginalText()
        {
            // Arrange
            var key = Encryption.Key(TestGuid);
            var initializationVector = Encryption.InitializationVector("1234567890123456");

            // Act
            var encryptedText = Encryption.EncryptRijndael(TestString, key, initializationVector);
            var decryptedText = Encryption.DecryptRijndael(encryptedText, key, initializationVector);

            // Assert
            decryptedText.Should().Be(TestString);
        }

        [TestMethod]
        public void EncryptAesAndDecryptAes_ShouldReturnOriginalText()
        {
            // Arrange
            var key = Encryption.Key(TestGuid);
            var initializationVector = Encryption.InitializationVector("1234567890123456");

            // Act
            var encryptedText = Encryption.EncryptAes(TestString, key, initializationVector);
            var decryptedText = Encryption.DecryptAes(encryptedText, key, initializationVector);

            // Assert
            decryptedText.Should().Be(TestString);
        }

        [TestMethod]
        public void Base64EncodeAndBase64Decode_ShouldReturnOriginalText()
        {
            // Act
            var encodedText = Encryption.Base64Encode(TestString);
            var decodedText = Encryption.Base64Decode(encodedText);

            // Assert
            decodedText.Should().Be(TestString);
        }

        [TestMethod]
        public void EncryptAndDecrypt_WithDefaultValues_ShouldReturnOriginalText()
        {
            // Act
            var encryptedText = Encryption.Encrypt(Encryption.EncryptionType.Rijndael, TestString);
            var decryptedText = Encryption.Decrypt(Encryption.EncryptionType.Rijndael, encryptedText);

            // Assert
            decryptedText.Should().Be(TestString);
        }

        [TestMethod]
        public void Key_Guid_ShouldReturnByteArray()
        {
            // Act
            var key = Encryption.Key(TestGuid);

            // Assert
            key.Should().NotBeNull();
            key.Should().HaveCount(32);
        }

        [TestMethod]
        public void Key_StringAndLength_ShouldReturnByteArray()
        {
            // Arrange
            const string hex = "1234567890ABCDEF";

            // Act
            var key = Encryption.Key(hex, 24);

            // Assert
            key.Should().NotBeNull();
            key.Should().HaveCount(24);
        }

        [TestMethod]
        public void InitializationVector_String_ShouldReturnByteArray()
        {
            // Arrange
            const string hex = "1234567890ABCDEF";

            // Act
            var iv = Encryption.InitializationVector(hex);

            // Assert
            iv.Should().NotBeNull();
            iv.Should().HaveCount(16);
        }
    }
}
