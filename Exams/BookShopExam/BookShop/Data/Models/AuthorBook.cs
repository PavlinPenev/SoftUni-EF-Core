namespace BookShop.Data.Models
{
    public class AuthorBook
    {
        public int AuthorId { get; set; }

        public int BookId { get; set; }

        #region Navigation Properties
        public Author Author { get; set; }

        public Book Book { get; set; }
        #endregion
    }
}
