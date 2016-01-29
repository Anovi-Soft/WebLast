using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.HtmlControls;
using Microsoft.AspNet.Identity;
namespace AnoviSoftSiteFun.Helpers
{
    public class VisitorCounter
    {
        private static volatile VisitorCounter instance;
        private static object syncRoot = new Object();
        private object syncSubmit = new Object();
        private VisitorsDataDataContext db;
        public int UsersCount { get; private set; } = 0;

        private VisitorCounter()
        {
            lock (syncSubmit)
            {
                db = new VisitorsDataDataContext("Server=tcp:tradehistoryserver.database.windows.net,1433;Database=BtceTradeHistory;User ID=L0dom@tradehistoryserver;Password=1@3$qWeR;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                var loading = true;
                while (loading)
                        try
                        {
                            var tmp = db.SessionLogs.ToList();
                            loading = false;
                        }
                        catch (Exception)
                        {

                        }
            }
            
            
        }

        public List<SessionLog> SessionLogs
        {
            get
            {
                lock (syncSubmit)
                    return db.SessionLogs.ToList();
            }
        }
        public List<PageLoadLog> PageLoadLogs
        {
            get
            {
                lock (syncSubmit)
                    return db.PageLoadLogs.ToList();
            }
        }
        

        public string TextInfo =>
            $"USERS\n" +
            $"LastDay:{SessionLogs.Count(x => x.OpenDateTime.AddDays(1) > DateTime.Now)}\n" +
            $"LastMonth:{SessionLogs.Count(x => x.OpenDateTime.AddMonths(1) > DateTime.Now)}\n" +
            $"AllTime:{SessionLogs.Count()}\n" +
            $"                         ";
        public static VisitorCounter Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new VisitorCounter();
                    }
                }

                return instance;
            }
        }
        
        public Task PageVisit(HttpRequest request, IPrincipal user)
        {
            var log = new PageLoadLog
            {
                UserId = user.Identity.GetUserId(),
                Ip4 = request.UserHostAddress,
                Url = request.Url.AbsolutePath,
                LoadDateTime = DateTime.Now
            };
            return Task.Run(() =>
            {
                lock (syncSubmit) db.PageLoadLogs.InsertOnSubmit(log);
                Submit();
            });
        }

        public Task OpenSession(HttpRequest request, IPrincipal user)
        {
            //КОСТЫЛЬ
            lock (syncSubmit)
            {

                var log = new SessionLog
                {
                    UserId = user.Identity.GetUserId(),
                    Ip4 = request.UserHostAddress,
                    OpenDateTime = DateTime.Now
                };
                var tmp = db.SessionLogs.ToList();
                var other = tmp.LastOrDefault(x => x.UserId == log.UserId && x.Ip4 == log.Ip4);
                if (other != null && (log.UserId == other.UserId || log.OpenDateTime < other.OpenDateTime.AddMinutes(30)))
                    return null;
                return Task.Run(() =>
                {
                    lock (syncSubmit) db.SessionLogs.InsertOnSubmit(log);
                    Submit();
                });
            }
        }
        
        public void Submit()
        {
            lock (syncSubmit)
                db.SubmitChanges();
        }
    }
}
