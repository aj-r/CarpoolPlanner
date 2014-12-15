using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CarpoolPlanner.Model
{
    public class UserTripInstanceCollection : KeyedCollection<string, UserTripInstance>
    {
        protected override string GetKeyForItem(UserTripInstance item)
        {
            return item.UserId;
        }
    }
}
