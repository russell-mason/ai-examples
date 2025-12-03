namespace AIExamples.Data.Services;

public partial class NewsHeadlinesJsonReader
{
    public static async Task<List<NewsHeadline>> ReadAsync(int count = int.MaxValue) =>
        await DeserializeJsonLinesAsync(@".\SourceData\NewsHeadlines.json", count).ToListAsync();

    [GeneratedRegex(@"\/([a-z0-9]+(?:-[a-z0-9]+)+)([\/_]|$)")]
    private static partial Regex UrlExtractRegex();

    private static string SlugOf(string value)
    {
        var match = UrlExtractRegex().Match(value);

        return match.Success ? match.Groups[1].Value : value;
    }

    private static async IAsyncEnumerable<NewsHeadline> DeserializeJsonLinesAsync(string filePath, int count)
    {
        using var stream = new StreamReader(filePath);

        // Read each line as a separate JSON object
        while (await stream.ReadLineAsync() is { } line && count-- > 0)
        {
            var headline = JsonSerializer.Deserialize<NewsHeadline>(line)!;
            headline.Slug = SlugOf(headline.Link);

            yield return headline;
        }
    }
}
