using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Kyrsova
{
	public interface IBookRepository
	{
		Book[] GetAllByIsbn(string isbn);

		Book[] GetAllByTitleOrAuthor(string titleOrAuthor);
	}
}