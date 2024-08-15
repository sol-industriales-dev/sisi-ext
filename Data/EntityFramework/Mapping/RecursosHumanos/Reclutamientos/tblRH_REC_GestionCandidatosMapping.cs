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
    public class tblRH_REC_GestionCandidatosMapping : EntityTypeConfiguration<tblRH_REC_GestionCandidatos>
    {
        public tblRH_REC_GestionCandidatosMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.apePaterno).HasColumnName("apePaterno");
            Property(x => x.apeMaterno).HasColumnName("apeMaterno");
            Property(x => x.correo).HasColumnName("correo");
            Property(x => x.telefono).HasColumnName("telefono");
            Property(x => x.celular).HasColumnName("celular");
            Property(x => x.nss).HasColumnName("nss");
            Property(x => x.pais).HasColumnName("pais");
            Property(x => x.paisDesc).HasColumnName("paisDesc");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.estadoDesc).HasColumnName("estadoDesc");
            Property(x => x.municipio).HasColumnName("municipio");
            Property(x => x.municipioDesc).HasColumnName("municipioDesc");
            Property(x => x.idGestionSolicitud).HasColumnName("idGestionSolicitud");
            Property(x => x.fechaNacimiento).HasColumnName("fechaNacimiento");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.altura).HasColumnName("altura");
            Property(x => x.peso).HasColumnName("peso");
            Property(x => x.notasReclutador).HasColumnName("notasReclutador");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.puestoDesc).HasColumnName("puestoDesc");
            Property(x => x.idPuesto).HasColumnName("idPuesto");
            HasRequired(x => x.virtualLstSolicitudes).WithMany().HasForeignKey(y => y.idGestionSolicitud);

            ToTable("tblRH_REC_Candidatos");
        }
    }
}
