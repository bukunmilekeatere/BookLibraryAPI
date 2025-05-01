namespace BookLibraryAPI.ModelDto
{
    public class MagazineDto
    {
        public string Title { get; set; } = string.Empty;
        public string Distributor { get; set; } = string.Empty;
        public string? AuthorName { get; set; }
        public int SerialNumber { get; set; }
        public string Issue { get; set; }
        public DateTime? DueDate { get; set; }
        public string? LibrarianId { get; set; }
    }
}
