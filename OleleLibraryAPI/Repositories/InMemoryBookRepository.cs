using OleleLibraryAPI.Models;

namespace OleleLibraryAPI.Repositories
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly List<Book> _books;
        private readonly object _syncRoot = new();
        private int _nextId;

        public InMemoryBookRepository(IEnumerable<Book> seedBooks)
        {
            _books = seedBooks
                .Select(Clone)
                .OrderBy(book => book.Id)
                .ToList();

            _nextId = _books.Count == 0 ? 1 : _books.Max(book => book.Id) + 1;
        }

        public IReadOnlyList<Book> GetAll()
        {
            lock (_syncRoot)
            {
                return _books
                    .OrderBy(book => book.Id)
                    .Select(Clone)
                    .ToList();
            }
        }

        public Book? GetById(int id)
        {
            lock (_syncRoot)
            {
                var book = _books.FirstOrDefault(existingBook => existingBook.Id == id);
                return book == null ? null : Clone(book);
            }
        }

        public Book Add(Book book)
        {
            lock (_syncRoot)
            {
                var storedBook = Clone(book);
                storedBook.Id = _nextId++;
                _books.Add(storedBook);
                return Clone(storedBook);
            }
        }

        public Book? Update(int id, Book book)
        {
            lock (_syncRoot)
            {
                var existingBook = _books.FirstOrDefault(currentBook => currentBook.Id == id);
                if (existingBook == null)
                {
                    return null;
                }

                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Genre = book.Genre;
                existingBook.Available = book.Available;
                existingBook.PublishedYear = book.PublishedYear;

                return Clone(existingBook);
            }
        }

        public bool Delete(int id)
        {
            lock (_syncRoot)
            {
                var existingBook = _books.FirstOrDefault(currentBook => currentBook.Id == id);
                if (existingBook == null)
                {
                    return false;
                }

                _books.Remove(existingBook);
                return true;
            }
        }

        private static Book Clone(Book source)
        {
            return new Book
            {
                Id = source.Id,
                Title = source.Title,
                Author = source.Author,
                Genre = source.Genre,
                Available = source.Available,
                PublishedYear = source.PublishedYear
            };
        }
    }
}
