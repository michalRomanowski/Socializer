using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Socializer.Database.Models;
using Socializer.Services.Services;

namespace Socializer.Services.UnitTests.Services
{
    public class ReadPreferencesServiceTests
    {
        private readonly ReadPreferencesService sut;

        public ReadPreferencesServiceTests()
        {
            sut = new ReadPreferencesService(NullLogger<ReadPreferencesService>.Instance);
        }

        [Theory]
        [MemberData(nameof(PreferencesData))]
        public void ReadPreferences_ReturnsExpectedPreferences(string preferencesFromLLM, IEnumerable<Preference> expectedPreferences)
        {
            // Act
            var result = sut.ReadPreferences(preferencesFromLLM);

            // Assert
            expectedPreferences.Select(x => x.DBPediaResource).Should().BeEquivalentTo(result.Select(r => r.DBPediaResource));
        }

        public static IEnumerable<object[]> PreferencesData =>
            [
                [
                    "resource1", // string parameter
                    new Preference[] { new() { DBPediaResource = "resource1" } } // array parameter
                ]
            ];
    }
}