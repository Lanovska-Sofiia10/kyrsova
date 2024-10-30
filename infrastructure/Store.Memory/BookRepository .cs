using Kyrsova;
using System;
using System.Linq;

namespace Store.Memory
{
    public class BookRepository : IBookRepository
	{
		private readonly Book[] books = new Book[]
		{
			new Book(1, "ISBN 12312-31231", "D. Knuth", "The Art of Computer Programming",
				"<p>Countless readers have spoken about the profound personal influence of Knuth's work.</p>" +
				"<p>Scientists have marveled at the beauty and elegance of his analysis, while ordinary programmers have successfully applied his \"cookbook\" solutions to their day-to-day problems.</p>" +
				"<p>Primarily written as a reference, some people have nevertheless found it possible and interesting to read each volume from beginning to end.</p>" +
				"<p>These five books comprise what easily could be the most important set of information on any serious programmer's bookshelf.</p>" +
				"<p>This box set includes the following volumes:</p>" +
				"<ul>" +
				"<li>The Art of Computer Programming: Volume 1: Fundamental Algorithms, 3rd Edition</li>" +
				"<li>The Art of Computer Programming, Volume 2: Seminumerical Algorithms, 3rd Edition</li>" +
				"<li>The Art of Computer Programming: Volume 3: Sorting and Searching, 2nd Edition</li>" +
				"<li>The Art of Computer Programming: Volume 4A: Combinatorial Algorithms, Part 1</li>" +
				"<li>The Art of Computer Programming: Volume 4B: Combinatorial Algorithms, Part 2</li>" +
				"</ul>",
				290.09m),

			new Book(2, "ISBN 12312-31232", "M. Fowler", "Refactoring",
				"<p>For more than twenty years, experienced programmers worldwide have relied on Martin Fowler’s Refactoring " +
				"to improve the design of existing code and to enhance software maintainability.</p>" +
				"<p>This eagerly awaited new edition has been fully updated to reflect crucial changes in the programming landscape.</p>" +
				"<p>Like the original, this edition explains what refactoring is; why you should refactor; how to recognize " +
				"code that needs refactoring; and how to actually do it successfully.</p>" +
				"<p>Recognize “bad smells” in code that signal opportunities to refactor.</p>" +
				"<p>Explore the refactorings, each with explanations, motivation, mechanics, and simple examples.</p>" +
				"<p>Includes free access to the canonical web edition.</p>",
				43.49m),

			new Book(3, "ISBN 12312-31233", "B. Kernighan, D. Ritchie", "C Programming Language",
				"<p>The authors present the complete guide to ANSI standard C language programming.</p>" +
				"<p>Written by the developers of C, this new version helps readers keep up with the finalized ANSI standard for C.</p>" +
				"<p>The 2/E has been completely rewritten with additional examples and problem sets to clarify the implementation " +
				"of difficult language constructs.</p>" +
				"<p>Includes detailed coverage of the C language plus the official C language reference manual for at-a-glance " +
				"help with syntax notation, declarations, ANSI changes, scope rules, and the list goes on and on.</p>",
				52.95m),
		};

        public Book[] GetAllByIds(IEnumerable<int> bookIds)
        {
            var foundBooks = from book in books
							 join bookId in bookIds on book.Id equals bookId
							 select book;

			return foundBooks.ToArray();
        }

        public Book[] GetAllByIsbn(string isbn)
		{
			return books.Where(book => book.Isbn == isbn)
						.ToArray();
		}

		public Book[] GetAllByTitle(string titlePart)
		{
			if (string.IsNullOrEmpty(titlePart))
			{
				return Array.Empty<Book>();
			}

			return books.Where(book => book.Title.Contains(titlePart, StringComparison.OrdinalIgnoreCase)).ToArray();
		}

		public Book[] GetAllByTitleOrAuthor(string query)
		{
			return books.Where(book => book.Author.Contains(query)
									|| book.Title.Contains(query))
						.ToArray();
		}

		public Book GetById(int id)
		{
            return books.Single(book => book.Id == id);
		}
	}
}




