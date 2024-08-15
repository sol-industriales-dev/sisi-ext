using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Propuesta
{
    public class CatCalculoCatReservaMapping : EntityTypeConfiguration<tblC_CatCalculoCatReserva>
    {
        public CatCalculoCatReservaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblC_CatCalculoCatReserva");
        }
    }
}
