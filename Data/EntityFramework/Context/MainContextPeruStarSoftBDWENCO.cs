using Core.Entity.StarSoft.Requisiciones;
//using Core.Entity.StarSoft.Usuario;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Context
{
    public class MainContextPeruStarSoftBDWENCO : DbContext
    {
        #region REQUISISIONES
        //public DbSet<AUDITORIA_SISTEMAS> AUDITORIA_SISTEMAS { get; set; }
        //public DbSet<USUARIO_INV> USUARIO_INV { get; set; }
        //public DbSet<USUARIO_COMP> USUARIO_COMP { get; set; }
        #endregion

        public MainContextPeruStarSoftBDWENCO()
            : base("MainContextPeruStarSoftBDWENCO")
        {
            //Disable initializer
            Database.SetInitializer<MainContextPeruStarSoftBDWENCO>(null);
        }
    }
}
