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
    public class tblS_MedioAmbienteTrayectosMapping : EntityTypeConfiguration<tblS_MedioAmbienteTrayectos>
    {
        public tblS_MedioAmbienteTrayectosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");
            Property(x => x.idAspectoAmbiental).HasColumnName("idAspectoAmbiental");
            Property(x => x.tratamiento).HasColumnName("tratamiento");
            Property(x => x.manifiesto).HasColumnName("manifiesto");
            Property(x => x.fechaEmbarque).HasColumnName("fechaEmbarque");
            Property(x => x.tipoTransporte).HasColumnName("tipoTransporte");
            Property(x => x.idTransportistaTrayecto).HasColumnName("idTransportistaTrayecto");
            Property(x => x.idArchivoTrayecto).HasColumnName("idArchivoTrayecto");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");

            ToTable("tblS_MedioAmbienteTrayectos");
        }
    }
}
