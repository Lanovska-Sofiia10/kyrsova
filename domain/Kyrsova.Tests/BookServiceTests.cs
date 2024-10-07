using Moq;
using Xunit;  // Додав, якщо раптом використовується бібліотека Xunit
using System.Collections.Generic;

namespace Kyrsova.Tests
{
    public class BookServiceTests
    {
        [Fact]
        public void GetAllByQuery_WithIsbn_CallsGetAllByIsbn()
        {
            var bookRepositoryStub = new Mock<IBookRepository>();

            bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                              .Returns(new[] { new Book(1, "ISBN 12345-67890", "Author", "Title") });

            bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                              .Returns(new[] { new Book(2, "ISBN 09876-54321", "Author 2", "Title 2") });

            var bookService = new BookService(bookRepositoryStub.Object);
            var validIsbn = "ISBN 12345-67890";

            var actual = bookService.GetAllByQuery(validIsbn);

            Assert.Collection(actual, book => Assert.Equal(1, book.Id));

            bookRepositoryStub.Verify(x => x.GetAllByIsbn(validIsbn), Times.Once);
        }

        [Fact]
        public void GetAllByQuery_WithAuthor_CallsGetAllByTitleOrAuthor()
        {
            var bookRepositoryStub = new Mock<IBookRepository>();

            bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                              .Returns(new[] { new Book(1, "ISBN 12345-67890", "Author", "Title") });

            bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                              .Returns(new[] { new Book(2, "ISBN 09876-54321", "Author 2", "Title 2") });

            var bookService = new BookService(bookRepositoryStub.Object);
            var authorQuery = "Author 2";

            var actual = bookService.GetAllByQuery(authorQuery);

            Assert.Collection(actual, book => Assert.Equal(2, book.Id));

            bookRepositoryStub.Verify(x => x.GetAllByTitleOrAuthor(authorQuery), Times.Once);

            bookRepositoryStub.Verify(x => x.GetAllByIsbn(It.IsAny<string>()), Times.Never);
        }

    }
}
