using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_sbillante.Data;
using Fall2024_Assignment3_sbillante.Models;
using System.Numerics;
using Newtonsoft.Json;
using System.Text;

namespace Fall2024_Assignment3_sbillante.Controllers
{
    public class MovieController : AIController
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public MovieController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> CallChatGPT(string prompt)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("api-key", _configuration["OpenAI-Key"]);

                var requestBody = new
                {
                    messages = new[]
                    {
                        new { role = "system", content = "you generate realistic fake reviews for movies. delimit each review with a | character to signify a new review is starting. Do NOT use numbers."},
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 950
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{_configuration["OpenAI-Endpoint"]}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    return result.choices[0].message.content;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Request to Azure OpenAI failed: {errorContent}");
                }
            }
        }

        public async Task<IActionResult> GetMoviePoster(int?  id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null || movie.Poster == null)
            {
                return NotFound();
            }

            var data = movie.Poster;
            return File(data, "image/jpg");
        }

        // GET: Movie
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movie.ToListAsync());
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Link,Genre,Year")] Movie movie, IFormFile? poster)
        {
            if (ModelState.IsValid)
            {
                if (poster != null && poster.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    poster.CopyTo(memoryStream);
                    movie.Poster = memoryStream.ToArray();
                }

                string prompt = $"Generate twenty 30-70 word reviews of the movie {movie.Title} ({movie.Year})";
                string rawReviews = await CallChatGPT(prompt, _configuration);
                movie.Reviews = rawReviews.Split('|', 20);

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Link,Genre,Year")] Movie movie, IFormFile? Poster)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (Poster != null)
            {
                using var memoryStream = new MemoryStream();
                await Poster.CopyToAsync(memoryStream);
                movie.Poster = memoryStream.ToArray(); // Save the new image
            }
            else
            {
                var existingMovie = await _context.Movie.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
                movie.Poster = existingMovie.Poster; // Keep the existing photo
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
