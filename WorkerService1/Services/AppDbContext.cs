using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WorkerService1.Models;

namespace WorkerService1.Services
{
    public class AppDbContext:DbContext
    {
        public DbSet<FtpFile> FtpFiles { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)  
        {

                
        }
    }
}
