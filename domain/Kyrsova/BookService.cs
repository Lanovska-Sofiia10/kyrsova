namespace Kyrsova
{
    public class BookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Book[] GetAllByQuery(string query)
        {
            if (query.StartsWith("ISBN", StringComparison.OrdinalIgnoreCase))
            {
                return _bookRepository.GetAllByIsbn(query);
            }

            return _bookRepository.GetAllByTitleOrAuthor(query);
        }
    }
}
