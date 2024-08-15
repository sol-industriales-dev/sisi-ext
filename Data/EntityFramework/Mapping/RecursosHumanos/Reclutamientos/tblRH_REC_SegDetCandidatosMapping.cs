using Core.Entity.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_SegDetCandidatosMapping : EntityTypeConfiguration<tblRH_REC_SegDetCandidatos>
    {
        public tblRH_REC_SegDetCandidatosMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idSeg).HasColumnName("idSeg");
            Property(x => x.idActividad).HasColumnName("idActividad");
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.esAprobada).HasColumnName("esAprobada");
            Property(x => x.esOmitida).HasColumnName("esOmitida");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.fechaActividad).HasColumnName("fechaActividad");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblRH_REC_SegDetCandidatos");
        }
    }
}
