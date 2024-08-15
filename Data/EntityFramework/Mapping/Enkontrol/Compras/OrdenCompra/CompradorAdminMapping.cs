using Core.Entity.Enkontrol.Compras.OrdenCompra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.OrdenCompra
{
    class CompradorAdminMapping : EntityTypeConfiguration<tblCom_Comprador_Admin>
    {
        public CompradorAdminMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idUsuario).HasColumnName("idUsuario");
            Property(x => x.empleado).HasColumnName("empleado");
            Property(x => x.sn_empleado).HasColumnName("sn_empleado");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblCom_Comprador_Admin");
        }
    }
}
