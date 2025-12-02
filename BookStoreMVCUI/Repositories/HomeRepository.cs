using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;

namespace BookStoreMVCUI.Repositories
{
    public class HomeRepository:IHomeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0)
        {

            var bookQuery = _dbContext.Books.AsNoTracking().Include(x => x.Stock).Include(x => x.genre).AsQueryable();

            if(!string.IsNullOrWhiteSpace(sTerm))
            {
                bookQuery = bookQuery.Where(b => b.BookName.StartsWith(sTerm));
            }
            if(genreId>0)
            {
                bookQuery = bookQuery.Where(b => b.GenreId == genreId);
            }

            var books = await bookQuery.AsNoTracking().Select(book => new Book
            {
                Id = book.Id,
                Image = book.Image,
                AuthorName = book.AuthorName,
                BookName = book.BookName,
                GenreId = genreId,
                Price = book.Price,
                GenreName = book.genre.GenreName,
                Quantity = book.Stock == null ? 0 : book.Stock.Quantity
                
            }).ToListAsync();   

            return books;
        }

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            return await _dbContext.Genres.ToListAsync();
        }

        
    }
}
