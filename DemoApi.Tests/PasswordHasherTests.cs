using DemoApi.Auth;

namespace DemoApi.Tests;

public class PasswordHasherTests
{
    [Fact]
    public void Hash_DoesNotStoreThePasswordAsPlainText()
    {
        var hasher = new PasswordHasher();

        var hash = hasher.Hash("secret123");

        Assert.NotEqual("secret123", hash);
        Assert.StartsWith("$2", hash);
    }

    [Fact]
    public void Verify_AcceptsTheRightPasswordAndRejectsAWrongOne()
    {
        var hasher = new PasswordHasher();
        var hash = hasher.Hash("secret123");

        Assert.True(hasher.Verify("secret123", hash));
        Assert.False(hasher.Verify("wrong-password", hash));
    }
}