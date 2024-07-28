namespace DfdsTestTask.Exceptions;

public class IncompleteAppConfigurationException(string configurationSection)
    : Exception($"Missing configuration section: {configurationSection}");