using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CarpoolPlanner.Model
{
    public class UserTripCollection : KeyedCollection<long, UserTrip>
    {
        protected override long GetKeyForItem(UserTrip item)
        {
            return item.UserId;
        }
    }
}
