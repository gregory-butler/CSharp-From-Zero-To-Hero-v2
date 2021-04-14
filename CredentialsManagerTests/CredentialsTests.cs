using System;
using System.IO;
using System.Linq;
using CredentialsManager;
using CredentialsManager.FilesExceptions;
using CredentialsManager.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace CredentialsManagerTests
{
  public static class CredentialsTests
  {
    public class TryParse
    {
      [Theory]
      [InlineData("MyUser,MyPassword", ",", "MyUser", "MyPassword")]
      [InlineData("OtherUser OtherPassword", " ", "OtherUser", "OtherPassword")]
      public void Should_Return_Expected_Credentials_When_Input_Is_Valid(
        string input, string delimiter, string expectedUserName, string expectedUserPassword)
      {
        // Arrange
        var expectedCredentials = new Credentials(expectedUserName, expectedUserPassword);
        var expectedIsCredentials = true;

        // Act
        var isCredentials = Credentials.TryParse(input, delimiter, out var credentials);

        // Assert
        using (new AssertionScope())
        {
          isCredentials.Should().Be(expectedIsCredentials);
          credentials.Should().Be(expectedCredentials);
        }
      }

      [Theory]
      [InlineData("", ",", "TestUser", "TestPassword")]
      [InlineData(" ", ",", "TestUser", "TestPassword")]
      [InlineData("TestUser,", ",", "TestUser", "TestPassword")]
      [InlineData(",TestPassword", ",", "TestUser", "TestPassword")]
      [InlineData("TestUser,TestPassword", " ", "TestUser", "TestPassword")]
      [InlineData("TestUser TestPassword", ",", "TestUser", "TestPassword")]
      public void Should_NOT_Return_Expected_Credentials_When_Input_Is_Erroneous(
        string input, string delimiter, string expectedUserName, string expectedUserPassword)
      {
        // Arrange
        const bool expectedIsCredentials = false;
        var expectedCredentials = new Credentials(expectedUserName, expectedUserPassword);

        // Act
        var isCredentials = Credentials.TryParse(input, delimiter, out var credentials);

        // Assert
        using (new AssertionScope())
        {
          isCredentials.Should().Be(expectedIsCredentials);
          credentials.Should().NotBe(expectedCredentials);
        }
      }
    }

    public class EqualsMethod
    {
      [Theory]
      [InlineData("Abcd", "1234", "Abcd", "1234", true)]
      [InlineData("Abcd", "1234", "aBCD", "1234", true)]
      [InlineData("Abcd", "1234", "aBCD", "4321", false)]
      public void Should_Evaluate_Equality_And_Return_Expected_Result(
        string expectedUserName, string expectedUserPassword, string actualUserName, string actualPassword, bool isExpectedEqual)
      {
        // Arrange
        var expected = new Credentials(expectedUserName, expectedUserPassword);

        // Act
        var actual = new Credentials(actualUserName, actualPassword);

        // Assert
        actual.Equals(expected).Should().Be(isExpectedEqual);
      }
    }
  }
}