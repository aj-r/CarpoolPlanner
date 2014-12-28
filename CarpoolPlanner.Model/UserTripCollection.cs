using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CarpoolPlanner.Model
{
    public class UserTripCollection : KeyedCollection<string, UserTrip>
    {
        protected override string GetKeyForItem(UserTrip item)
        {
            return item.UserId;
        }
    }
}
