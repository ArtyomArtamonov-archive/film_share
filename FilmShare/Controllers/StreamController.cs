using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FilmShare.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmShare.Controllers{
    public class StreamController : Controller{
        public async Task<IActionResult> FilmStream(){
            string path = @"wwwroot/files/film.mp4";
            var memory = new MemoryStream();
            
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 65536, FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/octet-stream", Path.GetFileName(path),true); 
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(){
            return View(new ErrorViewModel{RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}