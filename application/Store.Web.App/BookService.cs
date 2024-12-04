using Kyrsova;

namespace Store.Web.App
{
    public class BookService
    {
        private readonly IBookRepository bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public BookModel GetById(int id)
        {
            var book = bookRepository.GetById(id);

            return Map(book);
        }
        
        public IReadOnlyCollection<BookModel> GetAllByQuery(string query)
        {
            var books = Book.IsIsbn(query)
                        ? bookRepository.GetAllByIsbn(query)
                        : bookRepository.GetAllByTitleOrAuthor(query);

            return books.Select(Map)
                        .ToArray();
        }

        private BookModel Map(Book arg)
        {
            return new BookModel
            {
                Id = arg.Id,
                Isbn = arg.Isbn,
                Title = arg.Title,
                Description = arg.Description,
                Price = arg.Price,
            };
        }

        public IReadOnlyCollection<BookModel> GetAllBooks()
        {
            var books = bookRepository.GetAll();
            if (books == null || books.Length == 0)
            {
                Console.WriteLine("Репозиторій повернув порожній список або null!");
            }

            var bookModels = books.Select(book => new BookModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                Description = book.Description
            }).ToList();

            Console.WriteLine($"Кількість знайдених книг: {bookModels.Count}");
            return bookModels;
        }


    }
}
