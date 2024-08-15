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
    public class tblRH_Vacaciones_FechasMapping : EntityTypeConfiguration<tblRH_Vacaciones_Fechas>
    {

        public tblRH_Vacaciones_FechasMapping()
        {

            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.vacacionID).HasColumnName("vacacionID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");
            Property(x => x.incidenciaAplicada).HasColumnName("incidenciaAplicada");

            ToTable("tblRH_Vacaciones_Fechas");
        }

    }
}
