namespace BookShop
{
    public static class Constants
    {
        public const string PHONE_PATTERN = @"[0-9]{3}-[0-9]{3}-[0-9]{4}";

        public const int MIN_LENGTH_NAME = 3;

        public const int MAX_LENGTH_NAME = 30;

        public const decimal PRICE_MIN_VALUE = 0.01M;

        public const int PAGE_MIN_COUNT = 50;

        public const int PAGE_MAX_COUNT = 5000;

        public const string BOOKS_XML_ROOT = "Books";

        public const string BOOK = "Book";

        public const string PAGES = "Pages";

        public const string NAME = "Name";

        public const string DATE = "Date";

        public const int MIN_GENRE_VALUE = 1;

        public const int MAX_GENRE_VALUE = 3;
    }
}
