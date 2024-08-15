using Core.Entity.Principal.Menus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Menus
{
    class AccionesVistaMapping : EntityTypeConfiguration<tblP_AccionesVista>
    {
        public AccionesVistaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.vistaID).HasColumnName("vistaID");
            //HasRequired(x => x.vista).WithMany().HasForeignKey(y => y.vistaID);
            Property(x => x.Accion).HasColumnName("Accion");
            Property(x => x.Descripcion).HasColumnName("Descripcion");
            ToTable("tblP_AccionesVista");
        }
    }
}
