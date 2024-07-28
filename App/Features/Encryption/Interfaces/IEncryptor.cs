namespace DfdsTestTask.Features.Encryption.Interfaces;

public interface IEncryptor<TInput, TOutput, TContext>
{
    TOutput Encrypt(TInput input, TContext context);

    TInput Decrypt(TOutput output, TContext context);
}