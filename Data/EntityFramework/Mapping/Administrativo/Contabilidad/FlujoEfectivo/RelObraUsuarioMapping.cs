using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.FlujoEfectivo
{
    public class RelObraUsuarioMapping : EntityTypeConfiguration<tblC_FED_RelObraUsuario>
    {
        public RelObraUsuarioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idUsuario).HasColumnName("idUsuario");
            Property(x => x.obra).HasColumnName("obra");
            Property(x => x.tipo).HasColumnName("tipo");
            ToTable("tblC_FED_RelObraUsuario");
        }
    }
}
