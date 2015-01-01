using System.Collections.ObjectModel;

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
