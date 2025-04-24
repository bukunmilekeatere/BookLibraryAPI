namespace BookLibraryAPI.Models
{
    public class Books
    { 
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public int Id { get; set; }
        public int PageCount { get; set; }
        public DateOnly DueDate { get; set; }
        public bool CheckedOut { get; set; }
        public string? LibrarianId { get; set; }

    }
}
