using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace WorkerService1
{
    public class AppSettings
    {
        public static IConfiguration  Configuration { get; set; }
        public static string ConnectionString { get; set; }
    }
}
