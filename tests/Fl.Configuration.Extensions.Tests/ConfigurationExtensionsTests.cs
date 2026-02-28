using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Shouldly;

namespace Fl.Configuration.Extensions.Tests;

public class ConfigurationExtensionsTests
{
    internal record SomeConfiguration
    {
        public string Name { get; init; }
        public int Value { get; init; }
    }

    [Test]
    public void GetRequiredConfiguration_WhenKeysAreAvailable_ShouldReturnProperData()
    {
        var configuration = new ConfigurationBuilder()
            .Add(new MemoryConfigurationSource
            {
                InitialData =
                    new Dictionary<string, string>
                    {
                        {"SomeConfiguration:Name", "some name"},
                        {"SomeConfiguration:Value", "123"}
                    }
            })
            .Build();
        var result = configuration.GetRequiredConfiguration<SomeConfiguration>();
        result.ShouldBeEquivalentTo(new SomeConfiguration { Name = "some name", Value = 123 });
    }

    [Test]
    public void GetConfiguration_WhenKeysAreAvailable_ShouldReturnSomeWithProperData()
    {
        var configuration = new ConfigurationBuilder()
            .Add(new MemoryConfigurationSource
            {
                InitialData =
                    new Dictionary<string, string>
                    {
                        {"SomeConfiguration:Name", "some name"},
                        {"SomeConfiguration:Value", "123"}
                    }
            })
            .Build();
        var result = configuration.GetConfiguration<SomeConfiguration>();

        result
            .IsSome
            .ShouldBeTrue();

        result
            .IfSome(r => r.ShouldBeEquivalentTo(new SomeConfiguration { Name = "some name", Value = 123 }));
    }

    [Test]
    public void GetRequiredConfiguration_WhenSectionIsMissing_ShouldThrowException()
    {
        var configuration = new ConfigurationBuilder()
            .Add(new MemoryConfigurationSource
            {
                InitialData = []
            })
            .Build();

        Action action = () => configuration.GetRequiredConfiguration<SomeConfiguration>();
        action
            .ShouldThrow<KeyNotFoundException>()
            .Message
            .ShouldBe("SomeConfiguration section not found in Configuration.");
    }

    [Test]
    public void GetConfiguration_WhenSectionIsMissing_ShouldReturnNone()
    {
        var configuration = new ConfigurationBuilder()
            .Add(new MemoryConfigurationSource
            {
                InitialData = []
            })
            .Build();

        var result = configuration.GetConfiguration<SomeConfiguration>();

        result
            .IsNone
            .ShouldBeTrue();
    }
}
