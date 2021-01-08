using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FilmShare.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FilmShare.Data{
    public class ApplicationDbContext : IdentityDbContext{
        
        public DbSet<FilmModel> Files { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options){
            Database.EnsureCreated();
        }
    }
}