using DfdsTestTask.Features.Encryption.Models;

namespace DfdsTestTask.Features.Encryption.Interfaces;

public interface ISymmetricEncryptor<TInput, TOutput>
    : IEncryptor<TInput, TOutput, SymmetricEncryptionContext>
{
    
}