namespace BookLibraryAPI.Models
{
    public class Books
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int PageCount { get; set; }
        public bool CheckedOut { get; set; }
        public DateTime? DueDate { get; set; }

        public int? AuthorId { get; set; }  // Foreign key (important)
        public Author? Author { get; set; } // Navigation property

        public string LibrarianId { get; set; } // Librarian's ID (still string)
    }
}
