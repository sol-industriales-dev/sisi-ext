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
    public class tblAlm_RelAreaCuentaXAlmacenDetMapping : EntityTypeConfiguration<tblAlm_RelAreaCuentaXAlmacenDet>
    {
        public tblAlm_RelAreaCuentaXAlmacenDetMapping()
        {
            
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idRelacion).HasColumnName("idRelacion");
            Property(x => x.Almacen).HasColumnName("Almacen");
            Property(x => x.Prioridad).HasColumnName("Prioridad");
            Property(x => x.TipoAlmacen).HasColumnName("TipoAlmacen");
            
            ToTable("tblAlm_RelAreaCuentaXAlmacenDet");

        }
    }
}
