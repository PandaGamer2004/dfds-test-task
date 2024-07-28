using System.Security.Cryptography;
using System.Text;
using DfdsTestTask.Features.Encryption.Implementations;
using DfdsTestTask.Features.Encryption.Models;

namespace App.Tests.UnitTests;

public class AesEncryptorTests
{

    private const string TestKey = "fY2gN6lqP0u7Vw8xT4aSd3mRkQ5zZxK9";
    
    [Theory]
    [InlineData("012345678")]
    [InlineData("sample paload2")]
    [InlineData("PFAXA123")]
    public void DecryptAfterDecrypt_ShouldReturnSameValueAsInitial(string payload)
    {
        var encryptor = new AesStringDataEncryptor();
        var encryptionContext = new SymmetricEncryptionContext
        {
            EncryptionKey = TestKey
        };
        var encryptedData = encryptor.Encrypt(payload, encryptionContext);

        var decryptedData = encryptor.Decrypt(encryptedData, encryptionContext);
        
        Assert.Equal(payload, decryptedData);
    }

    
    
}