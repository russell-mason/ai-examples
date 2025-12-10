namespace AIExamples.Data.Services;

public static partial class TextMetricCalculator
{
    public static int GetLineCount(string text) => LineCountRegex().Count(text) + 1;

    public static int GetBlankLineCount(string text) => BlankLineCountRegex().Count(text);

    public static int GetParagraphCount(string text) => ParagraphCountRegex().Count(text);

    public static int GetWordCount(string text) => WordCountRegex().Count(text);

    [GeneratedRegex(@"\n+")]
    private static partial Regex LineCountRegex();

    [GeneratedRegex(@"^\s*$", RegexOptions.Multiline)]
    private static partial Regex BlankLineCountRegex();

    [GeneratedRegex(@"(?:[^\r\n]+(?:(?:\r\n|\n)(?!(?:\r\n|\n)).*)*)")]
    private static partial Regex ParagraphCountRegex();

    [GeneratedRegex(@"\S+")]
    private static partial Regex WordCountRegex();
}
