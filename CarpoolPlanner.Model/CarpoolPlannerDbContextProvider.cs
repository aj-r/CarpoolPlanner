using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpoolPlanner.Model
{
    public class CarpoolPlannerDbContextProvider : IDbContextProvider
    {
        #region IDbContextProvider Members

        public IApplicationDbContext GetContext()
        {
            return new ApplicationDbContext();
        }

        #endregion
    }
}
