using static System.Reflection.Metadata.BlobBuilder;

namespace BookLibraryAPI.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Books> Books { get; set; }  // A list of books for the author
    }
}
