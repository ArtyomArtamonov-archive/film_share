using System.Diagnostics;
using System.Threading.Tasks;
using FilmShare.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmShare.Controllers{
    public class StreamController : Controller{
        public async Task<FileStreamResult> FilmStream(){
            var stream = System.IO.File.OpenRead("wwwroot/files/film.mp4");
            return new FileStreamResult(stream, "video/mp4");
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(){
            return View(new ErrorViewModel{RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}