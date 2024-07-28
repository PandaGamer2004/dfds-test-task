namespace DfdsTestTask.Features.Encryption.Interfaces;

public interface IEncryptionConfigurationLoader<TConfiguration>
{
    ValueTask<TConfiguration> LoadConfiguration(CancellationToken ct = default);
}