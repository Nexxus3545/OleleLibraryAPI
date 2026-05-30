using OleleLibraryAPI.Models;

namespace OleleLibraryAPI.Repositories
{
    public interface IBookRepository
    {
        IReadOnlyList<Book> GetAll();
        Book? GetById(int id);
        Book Add(Book book);
        Book? Update(int id, Book book);
        bool Delete(int id);
    }
}

