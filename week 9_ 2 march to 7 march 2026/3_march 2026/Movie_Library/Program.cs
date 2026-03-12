using System;
using System.Collections.Generic;
using System.Linq;

public interface IMovie
{
    int Id { get; set; }
    string Title { get; set; }
    string Director { get; set; }
    string Genre { get; set; }
    int Year { get; set; }
}

public interface IMovieLibrary
{
    void AddMovie(IMovie movie, int quantity);
    void RemoveMovie(IMovie movie, int quantity);
    List<IMovie> GetMovies();
    List<IMovie> SearchMovies(string keyword);
}

public class Movie : IMovie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Director { get; set; }
    public string Genre { get; set; }
    public int Year { get; set; }
}

public class MovieLibrary : IMovieLibrary
{
    private Dictionary<IMovie, int> movies = new Dictionary<IMovie, int>();

    public void AddMovie(IMovie movie, int quantity)
    {
        var existing = movies.Keys.FirstOrDefault(m => m.Id == movie.Id);

        if (existing != null)
        {
            movies[existing] += quantity;
        }
        else
        {
            movies.Add(movie, quantity);
        }
    }

    public void RemoveMovie(IMovie movie, int quantity)
    {
        var existing = movies.Keys.FirstOrDefault(m => m.Id == movie.Id);

        if (existing != null)
        {
            movies[existing] -= quantity;

            if (movies[existing] <= 0)
                movies.Remove(existing);
        }
    }

    public List<IMovie> GetMovies()
    {
        return movies.Keys.ToList();
    }

    public List<IMovie> SearchMovies(string keyword)
    {
        return movies.Keys
            .Where(m => m.Title.ToLower().Contains(keyword.ToLower()))
            .ToList();
    }
}

class Solution
{
    static void Main()
    {
        IMovieLibrary movieLibrary = new MovieLibrary();

        Console.Write("Enter number of movies: ");
        int movieCount = Convert.ToInt32(Console.ReadLine());

        for (int i = 1; i <= movieCount; i++)
        {
            Console.WriteLine("\nEnter movie details:");
            Console.Write("Id: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Director: ");
            string director = Console.ReadLine();

            Console.Write("Genre: ");
            string genre = Console.ReadLine();

            Console.Write("Year: ");
            int year = Convert.ToInt32(Console.ReadLine());

            Console.Write("Quantity: ");
            int quantity = Convert.ToInt32(Console.ReadLine());

            IMovie movie = new Movie()
            {
                Id = id,
                Title = title,
                Director = director,
                Genre = genre,
                Year = year
            };

            movieLibrary.AddMovie(movie, quantity);
        }

        Console.WriteLine("\nMovies in Library:");

        var movies = movieLibrary.GetMovies();

        foreach (var movie in movies.OrderBy(x => x.Title))
        {
            Console.WriteLine($"Title:{movie.Title}, Director:{movie.Director}, Genre:{movie.Genre}, Year:{movie.Year}");
        }

        Console.Write("\nEnter keyword to search movie: ");
        string keyword = Console.ReadLine();

        Console.WriteLine("\nSearch Result:");

        var result = movieLibrary.SearchMovies(keyword);

        foreach (var movie in result.OrderBy(x => x.Title))
        {
            Console.WriteLine($"Title:{movie.Title}, Director:{movie.Director}, Genre:{movie.Genre}, Year:{movie.Year}");
        }
    }
}