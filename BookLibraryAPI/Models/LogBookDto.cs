namespace BookLibraryAPI.Models
{
    public class LogBookDto
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int PageCount { get; set; }
        public bool CheckedOut { get; set; }
        public DateTime? DueDate { get; set; }
        public int AuthorId { get; set; }
        public string LibrarianId { get; set; }
    }

}
