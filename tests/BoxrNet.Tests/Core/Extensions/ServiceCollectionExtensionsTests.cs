using BoxrNet.Core.Extensions;
using BoxrNet.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace BoxrNet.Tests.Core.Extensions
{
    public class ExtensionsTests
    {
        private readonly ServiceCollection _services;

        public ExtensionsTests()
        {
            var configuration = ConfigurationHelper.GetConfiguration();

            _services = new ServiceCollection();
            _services.AddSingleton(configuration);
        }

        [Fact]
        public void Should_AddSettings_When_ConfigurationIsValid()
        {
            // Act
            _services.AddSettings<FakeSettings>("SettingsTest");

            // Assert
            var options = _services.GetService<IOptions<FakeSettings>>();
            var settings = _services.GetService<FakeSettings>();

            Assert.NotNull(settings);
            Assert.NotNull(options?.Value);
            Assert.IsType<FakeSettings>(options.Value);
        }

        [Fact]
        public void Should_AddSettingsWithInterface_When_ConfigurationIsValid()
        {
            // Act
            _services.AddSettings<FakeFakeSettingsWithInterface>("SettingsTest");

            // Assert
            var options = _services.GetService<IOptions<FakeFakeSettingsWithInterface>>();
            var settings = _services.GetService<IFakeSettings>();

            Assert.NotNull(settings);
            Assert.NotNull(options?.Value);
            Assert.IsType<FakeFakeSettingsWithInterface>(options.Value);
        }
    }
}