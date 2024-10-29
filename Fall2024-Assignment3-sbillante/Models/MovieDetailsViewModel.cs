namespace Fall2024_Assignment3_sbillante.Models
{
    public class MovieDetailsViewModel
    {
        public Movie Movie { get; set; }

        public IEnumerable<Actor> Actors { get; set; }

        //add reviews + sentiments table

        public MovieDetailsViewModel(Movie movie, IEnumerable<Actor> actors)
        {
            Movie = movie;
            Actors = actors;
        }
    }
}
