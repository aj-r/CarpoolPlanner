using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CarpoolPlanner.Model;
using CarpoolPlanner.ViewModel;
using PagedList;

namespace CarpoolPlanner.Controllers
{
    [Authorize]
    public class LogController : CarpoolControllerBase
    {
        public const int PageSize = 50;

        public ActionResult Index()
        {
            var model = new LogViewModel();
            if (!AppUtils.IsUserAdmin())
                return View(model);
            using (var context = ApplicationDbContext.Create())
            {
                var count = context.Logs.Count();
                int lastPage = (int)Math.Ceiling((double)count / (double)PageSize);
                model.Logs = context.Logs.OrderBy(log => log.Date).ToPagedList(lastPage, PageSize);
                model.Loggers = context.Logs.Select(log => log.Logger).Distinct().OrderBy(logger => logger).ToList();
                model.Users = new [] { new User { Id = 0, Name = "None" } }.Concat(context.Users).ToList();
            }
            return View(model);
        }

        public ActionResult Filter(LogFiltersViewModel filters)
        {
            IPagedList<Log> logs = null;
            if (!AppUtils.IsUserAdmin())
                return Ng(logs);
            if (filters == null)
            {
                Response.StatusCode = 400;
                return Ng(logs);
            }
            if (filters.Page < 1)
            {
                Response.StatusCode = 400;
                return Ng(logs);
            }
            using (var context = ApplicationDbContext.Create())
            {
                IQueryable<Log> query = context.Logs;
                if (filters.MinDate.HasValue)
                    query = query.Where(log => log.Date >= filters.MinDate.Value);
                if (filters.MaxDate.HasValue)
                    query = query.Where(log => log.Date <= filters.MaxDate.Value);
                if (!string.IsNullOrEmpty(filters.Level))
                    query = query.Where(log => log.Level == filters.Level);
                if (filters.UserId.HasValue)
                {
                    // 0 means only logs with no user ID
                    if (filters.UserId == 0)
                        query = query.Where(log => log.UserId == null);
                    else
                        query = query.Where(log => log.UserId == filters.UserId);
                }
                if (!string.IsNullOrEmpty(filters.Logger))
                    query = query.Where(log => log.Logger == filters.Logger);
                if (!string.IsNullOrEmpty(filters.Message))
                    query = query.Where(log => log.Message.Contains(filters.Message));

                logs = query.OrderBy(log => log.Date).Include(log => log.User).ToPagedList(filters.Page, PageSize);
            }
            return Ng(logs);
        }
    }
}
