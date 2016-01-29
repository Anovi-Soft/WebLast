using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AnoviSoftSiteFun.Helpers
{
    class QuoteComponents
    {
        private static volatile QuoteComponents instance;
        private static object syncRoot = new Object();
        private object syncSubmit = new Object();
        private DataQuoteDataContext db;

        private QuoteComponents()
        {
            db = new DataQuoteDataContext("Server=tcp:tradehistoryserver.database.windows.net,1433;Database=BtceTradeHistory;User ID=L0dom@tradehistoryserver;Password=1@3$qWeR;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public static QuoteComponents Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new QuoteComponents();
                    }
                }

                return instance;
            }
        }

        public void Submit()
        {
            lock (syncSubmit)
                db.SubmitChanges();
        }

        public void AddComment(IPrincipal user, int idQuote, string Message)
        {
            var time = DateTime.Now;
            var comment = new QuoteComment
            {
                UserId = user.Identity.GetUserId(),
                UserName = user.Identity.GetUserName(),
                CreateTime = time,
                UpdateTime = time,
                Quote = idQuote,
                Comment = Validate(Message)
            };
            lock (syncSubmit) db.QuoteComments.InsertOnSubmit(comment);
            Submit();
        }
        public void UpdateComment(int idComment, string message)
        {
            lock (syncSubmit)
                db.QuoteComments
                    .First(x => x.Id == idComment)
                    .Comment = Validate(message);
            Submit();
        }
        public void RemoveComment(int idComment)
        {
            lock (syncSubmit)
            {
                var com = db.QuoteComments.ToList().First(x => x.Id == idComment);
                db.QuoteComments.DeleteOnSubmit(com);
            }
            Submit();
        }

        public void Like(IPrincipal user, int idQuote)
        {
            var like = new QuoteLike
            {
                UserId = user.Identity.GetUserId(),
                UserName = user.Identity.GetUserName(),
                Quote = idQuote,
                UpdateTime = DateTime.Now,
            };
            lock (syncSubmit)
            {
                if (db.QuoteLikes.ToList()
                    .Any(x => x.Quote == like.Quote &&
                        x.UserId == like.UserId))
                    return;
                db.QuoteLikes.InsertOnSubmit(like);
            }
            Submit();
        }
        public void Dislike(IPrincipal user, int idQuote)
        {
            var like = new QuoteLike
            {
                UserId = user.Identity.GetUserId(),
                UserName = user.Identity.GetUserName(),
                Quote = idQuote,
            };
            lock (syncSubmit)
            {
                var other = db.QuoteLikes.ToList()
                    .FirstOrDefault(x => x.Quote == like.Quote &&
                        x.UserId == like.UserId);
                if (other == null) return;
                db.QuoteLikes.DeleteOnSubmit(other);
            }
            Submit();
        }
        private string Validate(string text)
        {
            return text;
        }

        public IEnumerable<QuoteComment> GetComments()
        {
            return db.QuoteComments.ToList();
        }

        public IEnumerable<QuoteLike> GetLikes()
        {
            return db.QuoteLikes.ToList();
        }
        public IEnumerable<QuoteComment> GetComments(int quoteId)
        {
            return GetComments().Where(x => x.Quote == quoteId);
        }

        public IEnumerable<QuoteLike> GetLikes(int quoteId)
        {
            return GetLikes().Where(x => x.Quote == quoteId);
        }
    }
}
