using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WorkerService1.Models;

namespace WorkerService1.Services
{
    public class DbHelper
    {
        private AppDbContext dbContext;

        private DbContextOptions<AppDbContext> GetAllOptions()
        {
            var optionBuilder= new DbContextOptionsBuilder<AppDbContext>();
            optionBuilder.UseSqlServer(AppSettings.ConnectionString);
            return optionBuilder.Options;
        }

        //GetAllFiles
        public List<FtpFile> GetAllFiles()
        {
            
            using (dbContext = new AppDbContext(GetAllOptions()))
            {
                try
                {
                    var files = dbContext.FtpFiles.ToList();
                    if (files != null)
                        return files;
                    else
                        return new List<FtpFile>();
                }
                catch (Exception)
                {
                    throw; 
                }
            }
        }

        //Seed Data
        //Used when no data is present, we want some default data to fill in the Database
        public void SeedData()
        {
            using (dbContext = new AppDbContext(GetAllOptions()))
            {
                dbContext.FtpFiles.Add(new FtpFile
                    {
                    FileName = "file01.txt",
                    FileDownloaded = false, 
                    FileInProgress = true
                    });
                dbContext.SaveChanges();
            }
        }


    }
}
