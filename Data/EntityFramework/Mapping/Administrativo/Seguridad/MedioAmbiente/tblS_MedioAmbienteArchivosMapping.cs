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
    public class tblS_MedioAmbienteArchivosMapping : EntityTypeConfiguration<tblS_MedioAmbienteArchivos>
    {
        public tblS_MedioAmbienteArchivosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCaptura).HasColumnName("idCaptura");
            Property(x => x.tipoArchivo).HasColumnName("tipoArchivo");
            Property(x => x.nombreArchivo).HasColumnName("nombreArchivo");
            Property(x => x.rutaArchivo).HasColumnName("rutaArchivo");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");

            ToTable("tblS_MedioAmbienteArchivos");
        }
    }
}