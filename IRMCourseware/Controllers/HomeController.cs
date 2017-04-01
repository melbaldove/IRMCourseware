using IRMCourseware.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRMCourseware.Models;
namespace IRMCourseware.Controllers
{
    public class HomeController : Controller
    {
        IRMContext context = new IRMContext();
        public ActionResult Index()
        {

            var listChapters = context.Groups.Select(x => x).ToList();
            return View(listChapters);
        }
        public PartialViewResult LoadGroupsListPartial(int? selectedID)
        {
            var listChapters = context.Groups.Select(x => x).ToList();
            ViewBag.SelectedID = selectedID;
            return PartialView(listChapters);

        }
        public ActionResult RenderReport(int? id, int? groupID)
        {
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            var report = context.Documents.SingleOrDefault(x => x.ID == id);
            ViewBag.Header = report.FileName;
            ViewBag.SelectedID = groupID;
            if (report.FileType.Equals(".mp4") || report.FileType.Equals(".avi"))
            {
                ViewBag.Partial = "RenderVideoPartial";
            }
            else if(report.FileType.Equals(".doc") || report.FileType.Equals(".docx") || report.FileType.Equals(".pdf"))
            {
                ViewBag.Partial = "RenderDocumentPartial";
            }
            else if(report.FileType.Equals(".ppt") || report.FileType.Equals(".pptx"))
            {
                ViewBag.SlideCount = Directory.GetFiles(Server.MapPath(report.Directory), "*.jpg").Count();
                ViewBag.Partial = "RenderPowerPointPartial";
            }
            return View("RenderReport", report);
        }
        public ActionResult GroupDocuments(int? id)
        {
            if(id == null)
            {
                var documentsList = context.Documents.ToList();
                ViewBag.ReportTitle = "All BSIT IV-1 Reports";
                return View(documentsList);
            }
            using(var context = new IRMContext())
            {
                var documentsList = context.Documents.Where(x => x.GroupID == id).ToList();
                ViewBag.SelectedID = id;
                ViewBag.ReportTitle = context.Groups.SingleOrDefault(x => x.ID == id).Title;
                return View(documentsList);
            }
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}