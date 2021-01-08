using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FilmShare.Classes.Attributes;
using FilmShare.Classes.Helpers;
using FilmShare.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FilmShare.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

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
            return Redirect("/Home/Film");
        }
        
        public IActionResult Index()
        {
            return View(_context.Files.ToList());
        }

        public IActionResult Privacy(){
            return View();
        }
        
        public async Task<IActionResult> Film(){
            return View();
        }
        
        
    }
}