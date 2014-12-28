using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CarpoolPlanner.Model
{
    public class UserTripRecurrenceCollection : KeyedCollection<long, UserTripRecurrence>
    {
        protected override long GetKeyForItem(UserTripRecurrence item)
        {
            return item.UserId;
        }
    }
}
