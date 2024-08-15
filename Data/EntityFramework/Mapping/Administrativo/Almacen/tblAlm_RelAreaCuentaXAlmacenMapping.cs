using Core.Entity.Administrativo.Almacen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Almacen
{
    public class tblAlm_RelAreaCuentaXAlmacenMapping : EntityTypeConfiguration<tblAlm_RelAreaCuentaXAlmacen>
    {
        public tblAlm_RelAreaCuentaXAlmacenMapping()
        {
            
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Asignacion).HasColumnName("Asignacion");
            Property(x => x.AreaCuenta).HasColumnName("AreaCuenta");
            Property(x => x.usuarioCreacion).HasColumnName("usuarioCreacion");
            ToTable("tblAlm_RelAreaCuentaXAlmacen");

        }
    }
}
