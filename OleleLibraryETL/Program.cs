using System.Globalization;
using System.Net.Http.Json;

var apiBaseUrl = Environment.GetEnvironmentVariable("OLELE_LIBRARY_API_URL") ?? "http://localhost:5095/";
var csvPath = Path.Combine(AppContext.BaseDirectory, "LegacyBooks.csv");

if (!File.Exists(csvPath))
{
    Console.WriteLine($"Legacy CSV file not found: {csvPath}");
    return;
}

var lines = await File.ReadAllLinesAsync(csvPath);
var records = lines
    .Skip(1)
    .Where(line => !string.IsNullOrWhiteSpace(line))
    .Select(ParseLegacyRow)
    .ToList();

Console.WriteLine($"Extracted {records.Count} records");

foreach (var book in records)
{
    Console.WriteLine($"{book.Id} | {book.Title} | {book.Author} | {book.Genre} | {book.Available} | {book.PublishedYear}");
}

using var client = new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl, UriKind.Absolute)
};

client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

foreach (var book in records)
{
    var response = await client.PostAsJsonAsync("api/v1/books", book);
    Console.WriteLine($"Loaded: {book.Title} -> {response.StatusCode}");
}

Console.WriteLine($"Loaded {records.Count} books into OleleLibraryAPI.");

static TransformedBook ParseLegacyRow(string row)
{
    var columns = row.Split(',', StringSplitOptions.TrimEntries);
    if (columns.Length < 6)
    {
        throw new InvalidDataException($"Invalid legacy row: {row}");
    }

    return new TransformedBook
    {
        Id = int.Parse(columns[0], CultureInfo.InvariantCulture),
        Title = NormalizeTitle(columns[1]),
        Author = NormalizeAuthor(columns[2]),
        Genre = NormalizeGenre(columns[3]),
        Available = ParseAvailability(columns[4]),
        PublishedYear = int.Parse(columns[5], CultureInfo.InvariantCulture)
    };
}

static bool ParseAvailability(string value)
{
    return value.Trim().ToLowerInvariant() switch
    {
        "yes" => true,
        "true" => true,
        "available" => true,
        "1" => true,
        _ => false
    };
}

static string NormalizeTitle(string value)
{
    return value.Trim().ToLowerInvariant() switch
    {
        "crime and punishment" => "Crime and Punishment",
        "lord of the rings" => "Lord Of The Rings",
        "clean code" => "Clean Code",
        "the pragmatic programmer" => "The Pragmatic Programmer",
        "design patterns" => "Design Patterns",
        "you don't know js" => "You Dont Know Js",
        "introduction to algorithms" => "Introduction To Algorithms",
        "atomic habits" => "Atomic Habits",
        _ => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.Trim().ToLowerInvariant())
    };
}

static string NormalizeAuthor(string value)
{
    return value.Trim().ToLowerInvariant() switch
    {
        "fyodor dostoevsky" => "Fyodor Dostoevsky",
        "j.r.r. tolkien" => "J.R.R. Tolkien",
        "robert c. martin" => "Robert C. Martin",
        "andrew hunt" => "Andrew Hunt",
        "gang of four" => "Gang Of Four",
        "kyle simpson" => "Kyle Simpson",
        "clrs" => "Clrs",
        "james clear" => "James Clear",
        _ => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.Trim().ToLowerInvariant())
    };
}

static string NormalizeGenre(string value)
{
    return value.Trim().ToLowerInvariant() switch
    {
        "drama" => "Drama",
        "fantasy" => "Fantasy",
        "programming" => "Programming",
        "architecture" => "Architecture",
        "javascript" => "Javascript",
        "general" => "General",
        "self help" => "Self Help",
        _ => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.Trim().ToLowerInvariant())
    };
}

internal class TransformedBook
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Genre { get; set; }
    public bool Available { get; set; }
    public int PublishedYear { get; set; }
}
