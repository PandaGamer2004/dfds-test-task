using System.Security.Cryptography;
using System.Text;
using DfdsTestTask.Features.Encryption.Interfaces;
using DfdsTestTask.Features.Encryption.Models;

namespace DfdsTestTask.Features.Encryption.Implementations;

public class AesStringDataEncryptor: ISymmetricStringDataEncryptor
{
        public string Encrypt(string input, SymmetricEncryptionContext symmetricEncryptionContext)
        {
            using var aes = Aes.Create();
            aes.GenerateIV();
            aes.Key = GetKeyFromContext(symmetricEncryptionContext);
            ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream();
            memoryStream.Write(aes.IV, 0, aes.IV.Length);
            using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
            {
                using (var sw = new StreamWriter(cryptoStream))
                {
                    sw.Write(input);
                } 
            }

            return Convert.ToBase64String(memoryStream.ToArray());

        }

    public string Decrypt(string output, SymmetricEncryptionContext context)
    {
        byte[] fullCypher = Convert.FromBase64String(output);
        using var aes = Aes.Create();
        byte[] iv = new byte[aes.BlockSize / 8];
        byte[] cypher = new byte[fullCypher.Length - iv.Length];
        Array.Copy(fullCypher, iv, iv.Length);
        Array.Copy(fullCypher, iv.Length, cypher,0, cypher.Length);

        aes.Key = GetKeyFromContext(context);
        aes.IV = iv;

        ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream(cypher);
        using var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        return streamReader.ReadToEnd();
    }

    private byte[] GetKeyFromContext(SymmetricEncryptionContext context)
        => Encoding.UTF8.GetBytes(context.EncryptionKey);
    
}