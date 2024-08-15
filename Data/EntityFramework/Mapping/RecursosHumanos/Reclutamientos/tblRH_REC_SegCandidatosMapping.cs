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
    public class tblRH_REC_SegCandidatosMapping : EntityTypeConfiguration<tblRH_REC_SegCandidatos>
    {
        public tblRH_REC_SegCandidatosMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCandidato).HasColumnName("idCandidato");
            Property(x => x.idFase).HasColumnName("idFase");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            HasRequired(x => x.virtualLstCandidatos).WithMany().HasForeignKey(y => y.idCandidato);
            HasRequired(x => x.virtualLstFases).WithMany().HasForeignKey(y => y.idFase);

            ToTable("tblRH_REC_SegCandidatos");
        }
    }
}
