using System.Collections.ObjectModel;

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
