using FluentAssertions;
using System.Linq;
using Xunit;

namespace ChristianSchulz.ObjectInspection.Shared.Tests;

public class SearchQueryParserTests
{
    [Fact]
    public void Parse_ReturnsWord_IfSearchQueryHasWord()
    {
        // Arrange
        var searchQuery = "word";

        // Act
        var terms = SearchQueryParser.Parse(searchQuery)
            .ToList();

        // Assert
        terms.Should().HaveCount(1);
        terms.Should().Contain(x => x.Key == string.Empty && x.Value.Length == 1);
        terms.Should().Contain(x => x.Key == string.Empty && x.Value.Contains("word"));
    }

    [Fact]
    public void Parse_ReturnsWords_IfSearchQueryHasWords()
    {
        // Arrange
        var searchQuery = "word1 word2";

        // Act
        var terms = SearchQueryParser.Parse(searchQuery)
            .ToList();

        // Assert
        terms.Should().HaveCount(1);
        terms.Should().Contain(x => x.Key == string.Empty && x.Value.Length == 2);
        terms.Should().Contain(x => x.Key == string.Empty && x.Value.Contains("word1"));
        terms.Should().Contain(x => x.Key == string.Empty && x.Value.Contains("word2"));
    }

    [Fact]
    public void Parse_ReturnsPhrase_IfSearchQueryHasPhrase()
    {
        // Arrange
        var searchQuery = "\"the phrase\"";

        // Act
        var terms = SearchQueryParser.Parse(searchQuery)
            .ToList();

        // Assert
        terms.Should().HaveCount(1);
        terms.Should().Contain(x => x.Key == string.Empty && x.Value.Length == 1);
        terms.Should().Contain(x => x.Key == string.Empty && x.Value.Contains("the phrase"));
    }

    [Fact]
    public void Parse_ReturnsPhrases_IfSearchQueryHasPhrases()
    {
        // Arrange
        var searchQuery = "\"the phrase1\" \"the phrase2\"";

        // Act
        var terms = SearchQueryParser.Parse(searchQuery)
            .ToList();

        // Assert
        terms.Should().HaveCount(1);
        terms.Should().Contain(x => x.Key == string.Empty && x.Value.Length == 2);
        terms.Should().Contain(x => x.Key == string.Empty && x.Value.Contains("the phrase1"));
        terms.Should().Contain(x => x.Key == string.Empty && x.Value.Contains("the phrase2"));
    }

    [Fact]
    public void Parse_ReturnsParameterWithWord_IfSearchQueryHasParameterWithWord()
    {
        // Arrange
        var searchQuery = "parameter:word";

        // Act
        var terms = SearchQueryParser.Parse(searchQuery)
            .ToList();

        // Assert
        terms.Should().HaveCount(1);
        terms.Should().Contain(x => x.Key == "parameter" && x.Value.Length == 1);
        terms.Should().Contain(x => x.Key == "parameter" && x.Value.Contains("word"));
    }

    [Fact]
    public void Parse_ReturnsParametersWithWords_IfSearchQueryHasParametersWithWords()
    {
        // Arrange
        var searchQuery = "parameter1:word1 parameter2:word2";

        // Act
        var terms = SearchQueryParser.Parse(searchQuery)
            .ToList();

        // Assert
        terms.Should().HaveCount(2);
        terms.Should().Contain(x => x.Key == "parameter1" && x.Value.Length == 1);
        terms.Should().Contain(x => x.Key == "parameter1" && x.Value.Contains("word1"));
        terms.Should().Contain(x => x.Key == "parameter2" && x.Value.Length == 1);
        terms.Should().Contain(x => x.Key == "parameter2" && x.Value.Contains("word2"));
    }

    [Fact]
    public void Parse_ReturnsParameterWithPhrase_IfSearchQueryHasParameterWithPhrase()
    {
        // Arrange
        var searchQuery = "parameter:\"the phrase\"";

        // Act
        var terms = SearchQueryParser.Parse(searchQuery)
            .ToList();

        // Assert
        terms.Should().HaveCount(1);
        terms.Should().Contain(x => x.Key == "parameter" && x.Value.Length == 1);
        terms.Should().Contain(x => x.Key == "parameter" && x.Value.Contains("the phrase"));
    }

    [Fact]
    public void Parse_ReturnsParameterWithMultipePhrases_IfSearchQueryHasParameterWithPhrasesMultipleTimes()
    {
        // Arrange
        var searchQuery = "parameter:\"the phrase1\" parameter:\"the phrase2\"";

        // Act
        var terms = SearchQueryParser.Parse(searchQuery)
            .ToList();

        // Assert
        terms.Should().HaveCount(1);
        terms.Should().Contain(x => x.Key == "parameter" && x.Value.Length == 2);
        terms.Should().Contain(x => x.Key == "parameter" && x.Value.Contains("the phrase1"));
        terms.Should().Contain(x => x.Key == "parameter" && x.Value.Contains("the phrase2"));
    }

    [Fact]
    public void Parse_ReturnsParametersWithPhrases_IfSearchQueryHasParametersWithPhrases()
    {
        // Arrange
        var searchQuery = "parameter1:\"the phrase1\" parameter2:\"the phrase2\"";

        // Act
        var terms = SearchQueryParser.Parse(searchQuery)
            .ToList();

        // Assert
        terms.Should().HaveCount(2);
        terms.Should().Contain(x => x.Key == "parameter1" && x.Value.Length == 1);
        terms.Should().Contain(x => x.Key == "parameter1" && x.Value.Contains("the phrase1"));
        terms.Should().Contain(x => x.Key == "parameter2" && x.Value.Length == 1);
        terms.Should().Contain(x => x.Key == "parameter2" && x.Value.Contains("the phrase2"));
    }

    [Fact]
    public void Parse_ReturnsTerms_IfSearchQueryHasTerms()
    {
        // Arrange
        var searchQuery = "word \"the phrase1\" parameter:\"the phrase2\"";

        // Act
        var terms = SearchQueryParser.Parse(searchQuery)
.ToList();

        // Assert
        terms.Should().HaveCount(2);
        terms.Should().Contain(x => x.Key == string.Empty && x.Value.Length == 2);
        terms.Should().Contain(x => x.Key == string.Empty && x.Value.Contains("word"));
        terms.Should().Contain(x => x.Key == string.Empty && x.Value.Contains("the phrase1"));
        terms.Should().Contain(x => x.Key == "parameter" && x.Value.Length == 1);
        terms.Should().Contain(x => x.Key == "parameter" && x.Value.Contains("the phrase2"));
    }
}