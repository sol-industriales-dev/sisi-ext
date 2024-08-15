using Core.Entity.Administrativo.Seguridad.MedioAmbiente;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.MedioAmbiente
{
    public class tblS_MedioAmbienteClasificacionesTransportistasMapping : EntityTypeConfiguration<tblS_MedioAmbienteClasificacionesTransportistas>
    {
        public tblS_MedioAmbienteClasificacionesTransportistasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.clasificacion).HasColumnName("clasificacion");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblS_MedioAmbienteClasificacionesTransportistas");
        }
    }
}
