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
    }
}
