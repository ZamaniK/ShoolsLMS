using Microsoft.AspNet.Identity;
using ShoolsLMS.Areas.Dashboard.ViewModels;
using ShoolsLMS.Infrastructure.FileHelpers;
using ShoolsLMS.Models.Data;
using ShoolsLMS.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShoolsLMS.Areas.Dashboard.Controllers
{
    public class PapersController : Controller
    {
        // GET: Dashboard/Papers
        PapersService paperService = new PapersService();
        SubjectsService subjectsService = new SubjectsService();

        public ActionResult Index(string searchTerm, int? subjectID, int? page)
        {
            PapersListingModel model = new PapersListingModel();

            int recordSize = 5;
            page = page ?? 1;

            model.SearchTerm = searchTerm;
            model.SubjectID = subjectID;

            model.Papers = paperService.SearchPapers(searchTerm, subjectID, page.Value, recordSize);

            model.Subjects = subjectsService.GetAllSubjects();

            var totalRecords = subjectsService.SearchSubjectCount(searchTerm, subjectID);
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }

        [HttpGet]
        public ActionResult Action(int? ID)
        {
            PaperActionModel model = new PaperActionModel();

            if (ID.HasValue)  //we are trying to edit a record
            {
                var paper = paperService.GetPaperByID(ID.Value);

                paper.SubjectId = model.SubjectId;
                paper.Title = model.Title;
                paper.Date = model.Date;
                paper.FileName = model.FileName;
               

            }
            model.Subjects = subjectsService.GetAllSubjects();
            return PartialView("_Action", model);
        }


        [HttpPost]
        public JsonResult Action(PaperActionModel model, HttpPostedFileBase upload)
        {
            JsonResult json = new JsonResult();

            var result = false;

            if (model.ID > 0)
            {
                var paper = paperService.GetPaperByID(model.ID);


                paper.SubjectId = model.SubjectId;
                paper.Title = model.Title;
                paper.Date = model.Date;
                string uploadedFileName = FileUtils.UploadFile(upload);

                model.FileName = uploadedFileName;
                paper.FileName = model.FileName;

                result = paperService.UpdatePaper(paper);
            }
            else
            {
                Paper paper = new Paper();

                string getuser = User.Identity.GetUserId();

                paper.SubjectId = model.SubjectId;
                paper.Title = model.Title;
                paper.Date = model.Date;
                if (Request.Files.Count > 0)
                {
                    
                        HttpFileCollectionBase files = Request.Files;

                        HttpPostedFileBase file = files[0];
                        string fileName = file.FileName;
                        
                        //create the uploads folder if it doesn't exist
                        Directory.CreateDirectory(Server.MapPath("~/uploads/"));
                        string path = Path.Combine(Server.MapPath("~/uploads/"), fileName);

                        //save the file
                        file.SaveAs(path);
                    model.FileName = path;


                }
                paper.FileName = model.FileName;


                result = paperService.SavePaper(paper);


            }


            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Paper." };
            }
            return json;
        }

        //[HttpPost]
        /*public JsonResult UploadFile(string id)
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        //get a stream
                        var stream = fileContent.InputStream;
                        //and optionally write the file to disk
                        var fileName = Path.GetFileName(file);
                        var path = Path.Combine(Server.MapPath("~/App_Data/Papers"), fileName);
                        //using (var fileStream = File.Create(path))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("upload failed");
            }
            return Json("File Uploaded Successfully");
        }*/
        [HttpPost]
        public JsonResult uploadFile()
        {
            //check if the user selected a file to upload
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;

                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;

                    //create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath("~/uploads/"));
                    string path = Path.Combine(Server.MapPath("~/uploads/"), fileName);

                    //save the file
                    file.SaveAs(path);
                    return Json("File uploaded successfully");
                }
                catch (Exception e)
                {
                    return Json("Error"+ e.Message);
                }
                
            }
            return Json("File Uploaded Successfully");
        }

        [HttpGet]
        public ActionResult Delete(int ID)
        {
            PaperActionModel model = new PaperActionModel();

            var paper = paperService.GetPaperByID(ID);

            model.ID = paper.PaperId;


            return PartialView("_Delete", model);
        }


        [HttpPost]
        public JsonResult Delete(PaperActionModel model)
        {
            JsonResult json = new JsonResult();

            var result = false;

            var paper = paperService.GetPaperByID(model.ID);



            result = paperService.DeletePaper(paper);

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Paper." };
            }
            return json;
        }
    }
}
