using Core.Entity.Administrativo.Facultamiento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Facultamiento
{
    class AutorizacionMapping : EntityTypeConfiguration<tblFa_CatAutorizacion> 
    {
        public AutorizacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idMonto).HasColumnName("idMonto");
            Property(x => x.idTitulo).HasColumnName("idTitulo");
            Property(x => x.idTipoAutorizacion).HasColumnName("idTipoAutorizacion");
            Property(x => x.renglon).HasColumnName("renglon");
            Property(x => x.cve).HasColumnName("cve");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.descPuesto).HasColumnName("descPuesto");
            Property(x => x.Autorizado).HasColumnName("Autorizado");
            ToTable("tblFa_CatAutorizacion");
        }
    }
}
