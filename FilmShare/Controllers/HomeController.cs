using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FilmShare.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FilmShare.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FilmShare.Controllers{
    public class HomeController : Controller{
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, ApplicationDbContext context){
            _logger = logger;
            _env = env;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromBody] FilmModel file){
            if (_context.Files.FirstOrDefault(x => x.Id == file.Id) != null){
                _context.Files.Remove(file);
            }

            await _context.SaveChangesAsync();
            return Redirect("/film");
        }
        
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 20971520000)]
        public async Task<IActionResult> UploadFile(IFormFile uploadedFile){
            if (uploadedFile != null)
            {
                // путь к папке files
                string path = "/files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_env.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FilmModel file = new FilmModel { Name = uploadedFile.FileName, Path = path };
                if (_context.Files.FirstOrDefault(x => x.Path == path) == null){
                    await _context.Files.AddAsync(file);
                    await _context.SaveChangesAsync();
                }
                
            }
            
            return RedirectToAction("Film");
        }

        public IActionResult Index()
        {
            return View(_context.Files.ToList());
        }

        public IActionResult Privacy(){
            return View();
        }

        public IActionResult Film(){
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(){
            return View(new ErrorViewModel{RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}