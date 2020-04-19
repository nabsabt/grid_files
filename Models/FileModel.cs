using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Feladat.Models
{
    public class FileModel
    {
        public int ID { get; set; }
        [Required]
        [StringLength(64)]
        
        public string FileName { get; set; }
        public string Extension { get; set; }

        public DateTime UploadedDate { get; set; }
    }
}