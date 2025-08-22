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
                    "Single_resource",
                    new Preference[] { new() { DBPediaResource = "Single_resource" } }
                ],
                [
                    " should_trim ",
                    new Preference[] { new() { DBPediaResource = "should_trim" } }
                ],
                [
                    "\nres1\n\nres2\n", // Few resources with empty lines
                    new Preference[] { 
                        new() { DBPediaResource = "res1" },
                        new() { DBPediaResource = "res2" }}
                ],
                [
                    "res1\nres1", // Duplicated resource
                    new Preference[] {
                        new() { DBPediaResource = "res1" } }
                ]
            ];
    }
}