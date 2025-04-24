namespace BookLibraryAPI.Models
{
    public class Magazines
    {
        public string Title { get; set; } = string.Empty;
        public string Distributor { get; set; } = string.Empty; 
        public string? AuthorName { get; set; } 
        public int SerialNumber { get; set; }
        public DateOnly DueDate { get; set; }
        public bool CheckedOut { get; set; }
        public string? LibrarianId { get; set; }
    }
}
