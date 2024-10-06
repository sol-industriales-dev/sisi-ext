using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad
{
    public class tblC_TipoMonedaMapping : EntityTypeConfiguration<tblC_TipoMoneda>
    {
        public tblC_TipoMonedaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombreCorto).HasColumnName("nombreCorto");
            Property(x => x.nombreCortoSat).HasColumnName("nombreCortoSat");
            Property(x => x.esMXN).HasColumnName("esMXN");
            Property(x => x.registroActivo).HasColumnName("registroActivo");

            ToTable("tblC_TipoMoneda");
        }
    }
}
