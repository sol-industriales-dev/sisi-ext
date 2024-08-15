using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    public class ControlInternoMovimientos_PermisosMapping : EntityTypeConfiguration<tblM_ControMovimientoInterno_Permisos>
    {
        public ControlInternoMovimientos_PermisosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.usuarioID).HasColumnName("usuarioID");

            ToTable("tblM_ControMovimientoInterno_Permisos");
        }
    }
}
