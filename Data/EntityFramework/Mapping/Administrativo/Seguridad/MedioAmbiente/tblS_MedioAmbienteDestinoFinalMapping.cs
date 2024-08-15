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
    public class tblS_MedioAmbienteDestinoFinalMapping : EntityTypeConfiguration<tblS_MedioAmbienteDestinoFinal>
    {
        public tblS_MedioAmbienteDestinoFinalMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");
            Property(x => x.idAspectoAmbiental).HasColumnName("idAspectoAmbiental");
            Property(x => x.fechaDestinoFinal).HasColumnName("fechaDestinoFinal");
            Property(x => x.idTransportistaDestinoFinal).HasColumnName("idTransportistaDestinoFinal");
            Property(x => x.idArchivoTrayecto).HasColumnName("idArchivoTrayecto");
            Property(x => x.cantidad).HasColumnName("cantidad");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");
            Property(x => x.aaID).HasColumnName("aaID");

            ToTable("tblS_MedioAmbienteDestinoFinal");
        }
    }
}
