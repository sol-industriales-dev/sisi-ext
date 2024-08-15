using Core.Entity.Portal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Context
{
    public class MainContextPortal : DbContext
    {
        #region TABLAS
        public DbSet<RequirementPurchaseOrder> RequirementPurchaseOrder { get; set; }
        //public DbSet<Settings> Settings { get; set; }
        #endregion

        public MainContextPortal()
            : base("MainContextPortal")
        {
            //Disable initializer
            Database.SetInitializer<MainContextPortal>(null);
        }
    }
}
