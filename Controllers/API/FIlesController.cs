using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Feladat.Models;
using DevExtreme.AspNet.Data;
using Newtonsoft.Json;
using System.Linq.Dynamic;
using System.Web.Http;
using Feladat.ViewModels;

namespace Feladat.Controllers
{
    /// <summary>
    /// this is the API
    /// </summary>
    public class FilesController : ApiController
    {
        private ApplicationDbContext _context;

        public FilesController()
        {
            _context = new ApplicationDbContext();
        }

      
        
               //////ez a függvény küldi a frontendre az adatokat sort nélkül
             
        public DevExpressIsABitch GetFileModels()
        {

            return new DevExpressIsABitch {
                Data = _context.Files.ToList(),
                TotalCount = _context.Files.Count()
                
            };



        }
        



        public DevExpressIsABitch Get(int skip, int take, string sort=null, string filter = null)
        {


            IEnumerable<FileModel> totalList = _context.Files;



            if (!string.IsNullOrEmpty(filter))
            {
                totalList = totalList.FilterByOptions(filter);
            }


            if (!string.IsNullOrEmpty(sort))
            {
               List<SortModel> sortModels = JsonConvert.DeserializeObject<List<SortModel>>(sort);  //DeserializeObject json stringből objektumot csinál
                string sortColumn = sortModels[0].selector;
                if (sortModels[0].desc)
                {
                    sortColumn += " DESC";
                }
                totalList = totalList.OrderBy(sortColumn);
            }
            else
            {
                totalList = totalList.OrderBy(c => c.ID);
            }

            //totalList = FileModel[] totalList;

            var result = new DevExpressIsABitch
            {
                TotalCount = totalList.Count(),
                Data = totalList.Skip(skip).Take(take),
                //Filter = filter

            };


            return result;

        }





        public FileModel GetFileModel(int id)
        {
            var file = _context.Files.SingleOrDefault(c => c.ID == id);
            return file;
        }

        [HttpPost]
        public FileModel CreateFile(FileHandlerViewModel fileViewModel)
        {
            FileModel file = new FileModel
            {
                Extension = fileViewModel.Extension,
                FileName = fileViewModel.FileName,
                UploadedDate = DateTime.Now
            };
            _context.Files.Add(file);
            _context.SaveChanges();
            return file;
        }
        [HttpPut]
        public void UpdateFile(int id, FileModel file)
        {
            var fileInDb = _context.Files.SingleOrDefault(c => c.ID == id);
            fileInDb.FileName = file.FileName;
            fileInDb.Extension = file.Extension;
            fileInDb.UploadedDate = file.UploadedDate;

            _context.SaveChanges();
        }

        [HttpDelete]
        public void DeleteFile(int id)
        {
            var fileInDb = _context.Files.SingleOrDefault(c => c.ID == id);
            _context.Files.Remove(fileInDb);
            _context.SaveChanges();
        }
    }

}
