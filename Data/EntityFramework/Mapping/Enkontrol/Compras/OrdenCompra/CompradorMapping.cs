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
    class CompradorMapping : EntityTypeConfiguration<tblCom_Comprador>
    {
        public CompradorMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.empleado).HasColumnName("empleado");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.usuarioSIGOPLAN).HasColumnName("usuarioSIGOPLAN");
            Property(x => x.cveEmpleado).HasColumnName("cveEmpleado");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblCom_Comprador");
        }
    }
}
