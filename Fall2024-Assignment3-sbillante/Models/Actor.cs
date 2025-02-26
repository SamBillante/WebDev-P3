﻿using System.ComponentModel.DataAnnotations;

namespace Fall2024_Assignment3_sbillante.Models
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public int Age { get; set; }

        public byte[]? Photo { get; set; }

        public string[]? Tweets { get; set; }

        public double[]? TweetsSentiment { get; set; }
    }
}
