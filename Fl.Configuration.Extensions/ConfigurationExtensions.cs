using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Microsoft.Extensions.Configuration;
using Fl.Functional.Utils;

namespace Fl.Configuration.Extensions;

public static class ConfigurationExtensions
{
    public static T GetRequiredConfiguration<T>(this IConfiguration configuration) where T : class, new() =>
        configuration
            .GetConfiguration<T>()
            .BiMap(Prelude.identity, () => throw new KeyNotFoundException($"{typeof(T).Name} section not found in Configuration."))
            .ValueUnsafe();

    public static Option<T> GetConfiguration<T>(this IConfiguration configuration) where T : class, new() =>
        configuration
            .GetSection(typeof(T).Name)
            .Get<T>()
            .MakeOption()
            .Map(c => c!);
}
