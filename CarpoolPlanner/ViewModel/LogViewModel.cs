using System;
using System.Collections.Generic;
using CarpoolPlanner.Model;
using PagedList;

namespace CarpoolPlanner.ViewModel
{
    public class LogViewModel
    {
        public LogViewModel()
        {
            Filters = new LogFiltersViewModel();
        }

        public IPagedList<Log> Logs { get; set; }
        public LogFiltersViewModel Filters { get; set; }
        public List<string> Loggers { get; set; }
    }
}