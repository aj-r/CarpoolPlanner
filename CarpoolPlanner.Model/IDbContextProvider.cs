using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpoolPlanner.Model
{
    /// <summary>
    /// An interface used to acquire a database context.
    /// </summary>
    public interface IDbContextProvider
    {
        /// <summary>
        /// Gets a database context instance.
        /// </summary>
        IApplicationDbContext GetContext();
    }
}
