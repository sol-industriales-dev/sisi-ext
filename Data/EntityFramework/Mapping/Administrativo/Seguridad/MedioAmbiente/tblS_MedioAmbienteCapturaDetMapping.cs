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
    public class tblS_MedioAmbienteCapturaDetMapping : EntityTypeConfiguration<tblS_MedioAmbienteCapturaDet>
    {
        public tblS_MedioAmbienteCapturaDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCaptura).HasColumnName("idCaptura");
            Property(x => x.codigoContenedor).HasColumnName("codigoContenedor");
            Property(x => x.consecutivoCodContenedor).HasColumnName("consecutivoCodContenedor");
            Property(x => x.idAspectoAmbiental).HasColumnName("idAspectoAmbiental");
            Property(x => x.cantAspectoAmbiental).HasColumnName("cantAspectoAmbiental");
            Property(x => x.estatusAspectoAmbiental).HasColumnName("estatusAspectoAmbiental");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblS_MedioAmbienteCapturaDet");
        }
    }
}