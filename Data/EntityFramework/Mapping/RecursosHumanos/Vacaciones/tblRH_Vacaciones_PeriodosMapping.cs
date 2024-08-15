using Core.Entity.RecursosHumanos.Vacaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Vacaciones
{
    public class tblRH_Vacaciones_PeriodosMapping : EntityTypeConfiguration<tblRH_Vacaciones_Periodos>
    {
        public tblRH_Vacaciones_PeriodosMapping(){

            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.periodoDesc).HasColumnName("periodoDesc");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFinal).HasColumnName("fechaFinal");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");

            ToTable("tblRH_Vacaciones_Periodos");
        }
    }
}
