using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpoolPlanner.Model
{
    /// <summary>
    /// A provider used to access the Carpool Planner database context.
    /// </summary>
    public class CarpoolPlannerDbContextProvider : IDbContextProvider
    {
        /// <summary>
        /// Gets a database context instance.
        /// </summary>
        public IApplicationDbContext GetContext()
        {
            return new ApplicationDbContext();
        }
    }
}
