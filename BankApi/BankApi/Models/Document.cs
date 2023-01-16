using System.Text;

namespace BankAPI.Models;

public sealed class Document
{
    private Document(string fileName, string type, Stream content)
    {
        FileName = fileName;
        Type = type;
        Content = content;
    }

    public string FileName { get; }

    public string Type { get; }

    public Stream Content { get; }

    public static Document FromFormFile(IFormFile file)
    {
        return new(file.FileName, file.ContentType, file.OpenReadStream());
    }

    public static Document Create(string name, string content)
    {
        return new(name, "text/plain", new MemoryStream(Encoding.UTF8.GetBytes(content)));
    }
}