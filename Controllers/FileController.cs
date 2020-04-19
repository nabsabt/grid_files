using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feladat.Models;
using Feladat.ViewModels;

namespace Feladat.Controllers
{
    public class FileController : Controller
    {

        private ApplicationDbContext _context;

        public FileController()
        {
            _context = new ApplicationDbContext();
        }

        //megcsinálni, h reloadolja az oldalt a save után 
        public ActionResult ReloadPage(string rawUrl)
        {
            rawUrl = Request.RawUrl;
            Response.Redirect(Request.RawUrl);
            return View();

        }



        [HttpPost]
        public ActionResult CreateFile(FileHandlerViewModel model)
        {
            FileModel file = new FileModel
            {
                Extension = model.Extension,
                FileName = model.FileName,
                UploadedDate = DateTime.Now
            };
            _context.Files.Add(file);
            _context.SaveChanges();
            return View("~/Views/Home/Index.cshtml");
        }
    }

}
