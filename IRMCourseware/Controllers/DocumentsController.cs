using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IRMCourseware.DAL;
using System.IO;
using ByteSizeLib;
using Aspose.Slides;
using System.Drawing;
using Aspose.Words.Saving;
using Aspose.Pdf.Devices;
using IRMCourseware.Models;
namespace IRMCourseware.Controllers
{
    public class DocumentsController : Controller
    {
        private IRMContext db = new IRMContext();

        // GET: Documents
        public ActionResult Index()
        {
            var documents = db.Documents.Include(d => d.Group);
            return View(documents.ToList());
        }

       

        // GET: Documents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Create
        public ActionResult Create(int id)
        {
            ViewBag.GroupID = id;
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Document document, HttpPostedFileBase UploadedFile)
        {
           
            if (ModelState.IsValid)
            {
                var groups = db.Groups.SingleOrDefault(g => g.ID == document.GroupID);
                document.FileName = Path.GetFileNameWithoutExtension(UploadedFile.FileName);
                document.FileType = Path.GetExtension(UploadedFile.FileName).ToLower();
                document.Directory = "~/DocumentsPath/Group" + groups.GroupNo + "/" + document.FileType.Substring(1) + "/" + document.FileName + "/";
                document.FileSize = ByteSize.FromBytes(UploadedFile.ContentLength).MegaBytes.ToString("0.00") + " MB";
                
                string absoluteDirPath = Server.MapPath(document.Directory);
                
                string completeFilePath = Path.Combine(absoluteDirPath, document.FileName + document.FileType);
                if (!Directory.Exists(absoluteDirPath))
                {
                    Directory.CreateDirectory(absoluteDirPath);
                }
                
                UploadedFile.SaveAs(completeFilePath);
                if (document.FileType.Equals(".ppt") || document.FileType.Equals(".pptx"))
                {
                    using (Presentation presentation = new Presentation(completeFilePath))
                    {
                        int totalSlides = presentation.Slides.Count;
                        for (int i = 0; i < totalSlides; i++)
                        {
                            Bitmap bmp = presentation.Slides[i].GetThumbnail(1f, 1f);
                            bmp.Save(absoluteDirPath + "slide" + i.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }
                }
                else if(document.FileType.Equals(".doc") || document.FileType.Equals(".docx"))
                {
                    Aspose.Words.Document wordDocument = new Aspose.Words.Document(completeFilePath);
                    
                    ImageSaveOptions options = new ImageSaveOptions(Aspose.Words.SaveFormat.Jpeg);
                    options.PageIndex = 0;
                    
                    wordDocument.Save(absoluteDirPath + document.FileName + ".jpg", options);
                    wordDocument.Save(absoluteDirPath + document.FileName + ".pdf");
                }
                else if (document.FileType.Equals(".pdf"))
                {
                    Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(completeFilePath);
                    using (FileStream imageStream = new FileStream(absoluteDirPath + document.FileName + ".jpg", FileMode.Create))
                    {
                        Resolution resolution = new Resolution(300);
                        JpegDevice jpegDevice = new JpegDevice(resolution, 100);
                        jpegDevice.Process(pdfDocument.Pages[1], imageStream);
                        imageStream.Close();

                    }
                }


                db.Documents.Add(document);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Index", "Groups");
        }

        // GET: Documents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupID = new SelectList(db.Groups, "ID", "GroupNo", document.GroupID);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Location,FileType,FileSize,GroupID")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupID = new SelectList(db.Groups, "ID", "GroupNo", document.GroupID);
            return View(document);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
