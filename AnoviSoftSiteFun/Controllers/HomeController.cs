using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using AnoviSoftSiteFun.Helpers;
using AnoviSoftSiteFun.Models;
using Microsoft.AspNet.Identity;
using MyBusinessCardSite.Models;

namespace AnoviSoftSiteFun.Controllers
{
    public class HomeController : Controller
    {
        const string connectionString = "Server=tcp:tradehistoryserver.database.windows.net,1433;Database=BtceTradeHistory;User ID=L0dom@tradehistoryserver;Password=1@3$qWeR;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        QuoteDataContext db = new QuoteDataContext(connectionString);
        QuoteComponents quoteComponents = QuoteComponents.Instance;
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ViewResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Contact(ContactModel contactModel)
        {
            if (ModelState.IsValid)
                return View("Thanks", contactModel);
            else
                return View();
        }
        [HttpGet]
        public ActionResult Quote(int id = -1)
        {
            var userId = User.Identity.GetUserId();
            if (id < -1 || id > 99)
                return RedirectToAction("Oops", "ErrorPage");
            if (id == -1) return Redirect($"/home/Quote/{new Random().Next(0, 100)}");
            var model = db.ItQuotes.Where(x => x.id == id+1)
                .Select(x=> new QuoteModel(x.id, x.Author, x.Quote))
                .First();
            return View("Quote", model);
        }

        [HttpGet]
        public ViewResult Album()
        {
            return View();
        }
        
        public PartialViewResult GetComments(int quoteId)
        {
            return PartialView(quoteComponents.GetComments(quoteId));
        }
        [HttpPost]
        public PartialViewResult AddAndGetComments(int quoteId, string message)
        {
            if (message.Trim() != "")
                quoteComponents.AddComment(User, quoteId, message);
            return PartialView("GetComments", quoteComponents.GetComments(quoteId));
        }
        public PartialViewResult RemoveComments(int quoteId, int commentId)
        {
            var result = quoteComponents.GetComments(quoteId);
            var item = result.First(x => x.Id == commentId);
            if (item.UserId == User.Identity.GetUserId())
            {
                quoteComponents.RemoveComment(commentId);
                result = result.Where(x => x.Id != commentId);
            }
            return PartialView("GetComments", result);
        }
        public PartialViewResult GetLikes(int quoteId)
        {
            return PartialView(new LikesModel {Likes=quoteComponents.GetLikes(quoteId), QuoteId = quoteId});
        }

        public PartialViewResult Like(int quoteId)
        {
            quoteComponents.Like(User, quoteId);
            return PartialView("GetLikes", new LikesModel { Likes = quoteComponents.GetLikes(quoteId), QuoteId = quoteId });
        }
        public PartialViewResult Dislike(int quoteId)
        {
            quoteComponents.Dislike(User, quoteId);
            return PartialView("GetLikes", new LikesModel { Likes = quoteComponents.GetLikes(quoteId), QuoteId = quoteId });
        }

        public ActionResult ImageInfo(string url)
        {
            var text = $"{VisitorCounter.Instance.TextInfo}\n";
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var lastSeen = VisitorCounter.Instance
                    .PageLoadLogs
                    .LastOrDefault(x => x.UserId == userId && x.Url == url && x.LoadDateTime.AddSeconds(5) < DateTime.Now);
                text += lastSeen == null ? "You first on this page\n" : $"You last visit this page at:\n {lastSeen.LoadDateTime:g}\n";
            }
            text += $"Browser:{Request.Browser.Browser}\n";
            var img = ImgRender.DrawText(text, new Font("Microsoft Sans Serif", 14f), Color.Black, Color.White);
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return File(stream.ToArray(), "image/png");
            }
        }

        public ActionResult GetAllQuotes()
        {
            var xml = QuotesXml.GetQuotes(db.ItQuotes.ToList(),
                quoteComponents.GetComments(),
                quoteComponents.GetLikes());
            XmlSerializer ser = new XmlSerializer(typeof(QuotesXml));
            using (var stream = new MemoryStream())
            {
                ser.Serialize(stream, xml);
                return File(stream.ToArray(), "text/xml");
            }
        }
        //public PartialViewResult PartialInfo()
        //{
        //    var text = VisitorCounter.Instance.TextInfo;
        //    var img = ImgRender.DrawText(text, new Font("Microsoft Sans Serif", 14f), Color.Black, Color.AliceBlue);
        //    var path = Server.MapPath(Path.Combine("Content","Informer", $"{DateTime.Now:g}.jpg".Replace("/", "_").Replace(":", "_")));
        //    img.Save(path, ImageFormat.Jpeg);
        //    return PartialView("PartialInfo", path);
        //}

    }
}