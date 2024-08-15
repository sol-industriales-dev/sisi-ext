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
    public class tblRH_REC_ArchivosMapping : EntityTypeConfiguration<tblRH_REC_Archivos>
    {
        public tblRH_REC_ArchivosMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCandidato).HasColumnName("idCandidato");
            Property(x => x.claveEmpleado).HasColumnName("claveEmpleado");
            Property(x => x.idFase).HasColumnName("idFase");
            Property(x => x.idActividad).HasColumnName("idActividad");
            Property(x => x.tipoArchivo).HasColumnName("tipoArchivo");
            Property(x => x.nombreArchivo).HasColumnName("nombreArchivo");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.ubicacionArchivo).HasColumnName("ubicacionArchivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            HasRequired(x => x.virtualLstGestionCandidatos).WithMany().HasForeignKey(y => y.idCandidato);
            HasRequired(x => x.virtualLstActividades).WithMany().HasForeignKey(y => y.idActividad);

            ToTable("tblRH_REC_Archivos");
        }
    }
}
