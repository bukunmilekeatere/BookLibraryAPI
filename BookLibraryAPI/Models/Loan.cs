﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibraryAPI.Models
{
    public class Loan
    {
        public int LoanId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int BookId { get; set; }

        public int MagazineId { get; set; }

        public int NewspaperId { get; set; }    

        public DateTime LoanDue {  get; set; }

        [ForeignKey("BookId")]

        public Books? Book {  get; set; }

        [ForeignKey("MagazineId")]

        public Magazines? Magazine { get; set; }

        [ForeignKey("NewspaperId")]

        public Newspapers? Newspaper { get; set; }
    }
}
