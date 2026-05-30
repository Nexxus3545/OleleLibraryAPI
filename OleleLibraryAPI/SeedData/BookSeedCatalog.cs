using OleleLibraryAPI.Models;

namespace OleleLibraryAPI.SeedData;

internal static class BookSeedCatalog
{
    public static IReadOnlyList<Book> GetBooks(bool renderMode)
    {
        return renderMode ? GetRenderBooks() : GetLocalBooks();
    }

    private static IReadOnlyList<Book> GetLocalBooks()
    {
        return new List<Book>
        {
            new()
            {
                Id = 1,
                Title = "The Gentle Reminder",
                Author = "Bianca Sparacino",
                Genre = "Poetic Self help",
                Available = true,
                PublishedYear = 2021
            },
            new()
            {
                Id = 2,
                Title = "let Go and Let God",
                Author = "Albert E. Cliffe",
                Genre = "Spritual Self Help",
                Available = true,
                PublishedYear = 1954
            }
        };
    }

    private static IReadOnlyList<Book> GetRenderBooks()
    {
        return new List<Book>
        {
            new()
            {
                Id = 1,
                Title = "Crime and Punishment",
                Author = "Fyodor Dostoevsky",
                Genre = "Drama",
                Available = true,
                PublishedYear = 1866
            },
            new()
            {
                Id = 2,
                Title = "Lord Of The Rings",
                Author = "J.R.R.Tolkien",
                Genre = "Fantasy",
                Available = true,
                PublishedYear = 1954
            },
            new()
            {
                Id = 3,
                Title = "Clean Code",
                Author = "Robert C. Martin",
                Genre = "Programming",
                Available = false,
                PublishedYear = 2008
            },
            new()
            {
                Id = 4,
                Title = "The Pragmatic Programmer",
                Author = "Adrew Hunt",
                Genre = "Programming",
                Available = true,
                PublishedYear = 1999
            },
            new()
            {
                Id = 5,
                Title = "Design Pattern",
                Author = "Gang Of Four",
                Genre = "Architecture",
                Available = false,
                PublishedYear = 1994
            },
            new()
            {
                Id = 6,
                Title = "You Dont Know Js",
                Author = "Kyle Simpson",
                Genre = "Javascript",
                Available = true,
                PublishedYear = 2015
            },
            new()
            {
                Id = 7,
                Title = "Introduction To Algorithms",
                Author = "Clrs",
                Genre = "General",
                Available = true,
                PublishedYear = 2009
            }
        };
    }
}
