using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CarpoolPlanner.Model
{
    public class UserTripInstanceCollection : KeyedCollection<long, UserTripInstance>
    {
        protected override long GetKeyForItem(UserTripInstance item)
        {
            return item.UserId;
        }
    }
}
