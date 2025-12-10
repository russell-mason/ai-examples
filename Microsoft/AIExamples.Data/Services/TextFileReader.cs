using static System.Net.Mime.MediaTypeNames;

namespace AIExamples.Data.Services;

public static class TextFileReader
{
    public static async Task<TextFileInfo> ReadAsync(string filePath)
    {
        var text = await File.ReadAllTextAsync(filePath);

        return new TextFileInfo(filePath, text);
    }
}