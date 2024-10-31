using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace Fall2024_Assignment3_sbillante.Models
{
    public class Movie
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Link {  get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public int Year { get; set; }

        public byte[]? Poster { get; set; }

        public string[]? Reviews { get; set; }

        public double[]? ReviewsSentiment { get; set; }
    }
}
