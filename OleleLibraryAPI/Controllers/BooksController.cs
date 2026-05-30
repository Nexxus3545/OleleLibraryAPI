using Microsoft.AspNetCore.Mvc;
using OleleLibraryAPI.Models;
using OleleLibraryAPI.Repositories;

namespace OleleLibraryAPI.Controllers
{
    [Route("api/v1/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new
            {
                status = "success",
                data = _bookRepository.GetAll(),
                message = "books retrieved"
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound(new
                {
                    status = "error",
                    data = (object?)null,
                    message = "book not found"
                });
            }

            return Ok(new
            {
                status = "success",
                data = book,
                message = "book retrieved"
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] Book newBook)
        {
            var createdBook = _bookRepository.Add(newBook);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdBook.Id },
                new
                {
                    status = "success",
                    data = createdBook,
                    message = "book created"
                });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Book updateBook)
        {
            var updatedBook = _bookRepository.Update(id, updateBook);
            if (updatedBook == null)
            {
                return NotFound(new
                {
                    status = "error",
                    data = (object?)null,
                    message = "book not found"
                });
            }

            return Ok(new
            {
                status = "success",
                data = updatedBook,
                message = "book updated"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _bookRepository.Delete(id);
            if (!deleted)
            {
                return NotFound(new
                {
                    status = "error",
                    data = (object?)null,
                    message = "book not found"
                });
            }

            return Ok(new
            {
                status = "success",
                data = (object?)null,
                message = "book deleted"
            });
        }
    }
}

