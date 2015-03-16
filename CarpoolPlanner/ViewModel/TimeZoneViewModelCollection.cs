using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace CarpoolPlanner.ViewModel
{
    public class TimeZoneViewModelCollection : KeyedCollection<string, TimeZoneViewModel>
    {
        public TimeZoneViewModelCollection()
        { }

        public TimeZoneViewModelCollection(IEnumerable<TimeZoneViewModel> items)
        {
            foreach (var item in items)
                Add(item);
        }

        protected override string GetKeyForItem(TimeZoneViewModel item)
        {
            return item.Name;
        }
    }
}