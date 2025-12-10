namespace AIExamples.Data.Services;

public class TextFileInfo(string filePath, string text)
{
    public string FilePath { get; } = Path.GetFullPath(filePath);

    public string Text { get; } = text;

    public int LineCount { get; } = TextMetricCalculator.GetLineCount(text);

    public int BlankLineCount { get; } = TextMetricCalculator.GetBlankLineCount(text);

    public int ParagraphCount { get; } = TextMetricCalculator.GetParagraphCount(text);

    public int WordCount { get; } = TextMetricCalculator.GetWordCount(text);

    public int CharacterCount { get; } = text.Length;

    public IEnumerable<string> GetTextAsLines() => Text.Replace("\r\n", "\n").Split('\n');
}
