namespace BookLibraryAPI.Models
{
    public class NewspaperDto
    {
        public string Title { get; set; } = string.Empty;
        public string? AuthorName { get; set; }
        public DateTime? DueDate { get; set; }
        public bool CheckedOut { get; set; }

        public string Genre { get; set; }
        public string? LibrarianId { get; set; }
    }
}
