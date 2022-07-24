using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkerService1.Models
{
    public class FtpFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public string Id { get; set; }
        public string FileName { get; set; }
        public bool FileDownloaded { get; set; }
        public bool FileInProgress { get; set; }
    }
}
