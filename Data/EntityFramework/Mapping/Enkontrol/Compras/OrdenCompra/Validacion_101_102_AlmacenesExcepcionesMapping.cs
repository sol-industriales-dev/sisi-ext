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
    class Validacion_101_102_AlmacenesExcepcionesMapping : EntityTypeConfiguration<tblAlm_Validacion_101_102_AlmacenesExcepciones>
    {
        public Validacion_101_102_AlmacenesExcepcionesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.almacen).HasColumnName("almacen");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblAlm_Validacion_101_102_AlmacenesExcepciones");
        }
    }
}
