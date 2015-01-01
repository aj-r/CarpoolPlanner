using System.Collections.ObjectModel;

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
