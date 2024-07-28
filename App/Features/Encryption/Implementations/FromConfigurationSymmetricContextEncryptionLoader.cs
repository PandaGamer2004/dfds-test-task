using DfdsTestTask.Exceptions;
using DfdsTestTask.Features.Encryption.Interfaces;
using DfdsTestTask.Features.Encryption.Models;

namespace DfdsTestTask.Features.Encryption.Implementations;

public class FromConfigurationSymmetricContextEncryptionLoader(IConfiguration configuration)
    : IEncryptionConfigurationLoader<SymmetricEncryptionContext>
{
    private const string SecretKeyConfigurationSection = "Encryption:SecretKey";

    public ValueTask<SymmetricEncryptionContext> LoadConfiguration(CancellationToken ct)
    {
        string? targetConfigurationSection = configuration[SecretKeyConfigurationSection];

        if (targetConfigurationSection is null)
        {
            throw new IncompleteAppConfigurationException(
                configurationSection: SecretKeyConfigurationSection
                );
        }

        return ValueTask.FromResult(new SymmetricEncryptionContext
        {
            EncryptionKey = targetConfigurationSection
        });
    }
}