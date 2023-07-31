using Infrastructure.FileStorage.OwnerDirectoryNameProviders;

namespace Infrastructure.UnitTests.FileStorageTests.Sha256OwnerDirectoryNameProviderTests;

public class GetForTests
{
    [Theory]
    [InlineData("test.email@test.dev", "746573742E656D61696C40746573742E646576")]
    [InlineData("test.email@test.dev", "746573742E656D61696C40746573742E646576")]
    [InlineData("aaa@bbb.ccc", "616161406262622E636363")]
    [InlineData("aaa@bbb.ccc", "616161406262622E636363")]
    [InlineData("test.email@test.dev", "746573742E656D61696C40746573742E646576")]
    [InlineData("aaa@bbb.ccc", "616161406262622E636363")]
    public void GetFor_ReturnsTheSameString_GivenRepeatedOwner(string email, string expectedName)
    {
        Sha256OwnerDirectoryNameProvider tested = new();
        var result = tested.GetFor(email);
        result.Should()
            .Be(expectedName);
    }
}