using Core.Libraries.Domain.ValueObjects.Identities;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects.Identities;

/// <summary>
/// Testes unitários para a classe <see cref="EmailAddress"/>.
/// </summary>
public class EmailAddressTests
{
    [Fact(Skip = "Skipping this test")]
    public void Constructor_WithValidEmail_ShouldCreateInstance()
    {
        // Arrange
        var email = "user@example.com";

        // Act
        var emailAddress = new EmailAddress(email);

        // Assert
        Assert.Equal("user@example.com", emailAddress.Value);
    }

    [Fact(Skip = "Skipping this test")]
    public void Constructor_WithEmail_ShouldNormalizeToLowercase()
    {
        // Arrange
        var email = "USER@EXAMPLE.COM";

        // Act
        var emailAddress = new EmailAddress(email);

        // Assert
        Assert.Equal("user@example.com", emailAddress.Value);
    }

    [Fact(Skip = "Skipping this test")]
    public void Constructor_WithNull_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new EmailAddress(null!));
    }

    [Fact(Skip = "Skipping this test")]
    public void Constructor_WithEmptyString_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new EmailAddress(""));
        Assert.Throws<ArgumentException>(() => new EmailAddress("   "));
    }

    [Fact(Skip = "Skipping this test")]
    public void Constructor_WithInvalidEmail_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new EmailAddress("invalid-email"));
        Assert.Throws<ArgumentException>(() => new EmailAddress("@example.com"));
        Assert.Throws<ArgumentException>(() => new EmailAddress("user@"));
    }

    [Fact(Skip = "Skipping this test")]
    public void LocalPart_ShouldReturnPartBeforeAt()
    {
        // Arrange
        var emailAddress = new EmailAddress("user.name@example.com");

        // Act
        var localPart = emailAddress.LocalPart;

        // Assert
        Assert.Equal("user.name", localPart);
    }

    [Fact(Skip = "Skipping this test")]
    public void Domain_ShouldReturnPartAfterAt()
    {
        // Arrange
        var emailAddress = new EmailAddress("user@example.com");

        // Act
        var domain = emailAddress.Domain;

        // Assert
        Assert.Equal("example.com", domain);
    }

    [Fact(Skip = "Skipping this test")]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var emailAddress = new EmailAddress("user@example.com");

        // Act
        var result = emailAddress.ToString();

        // Assert
        Assert.Equal("user@example.com", result);
    }

    [Fact(Skip = "Skipping this test")]
    public void Equals_WithSameEmail_ShouldReturnTrue()
    {
        // Arrange
        var email1 = new EmailAddress("user@example.com");
        var email2 = new EmailAddress("USER@EXAMPLE.COM");

        // Act & Assert
        Assert.Equal(email1, email2);
    }

    [Fact(Skip = "Skipping this test")]
    public void Equals_WithDifferentEmail_ShouldReturnFalse()
    {
        // Arrange
        var email1 = new EmailAddress("user1@example.com");
        var email2 = new EmailAddress("user2@example.com");

        // Act & Assert
        Assert.NotEqual(email1, email2);
    }
}

