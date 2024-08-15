using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SubContratistas
{
    public class RelacionSubContratistaDocumentacionMapping : EntityTypeConfiguration<tblX_RelacionSubContratistaDocumentacion>
    {
        public RelacionSubContratistaDocumentacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.subContratistaID).HasColumnName("subContratistaID");
            Property(x => x.documentacionID).HasColumnName("documentacionID");
            Property(x => x.rutaArchivo).HasColumnName("rutaArchivo");
            Property(x => x.fechaVencimiento).HasColumnName("fechaVencimiento");
            Property(x => x.fechaSolicitud).HasColumnName("fechaSolicitud");
            Property(x => x.aplica).HasColumnName("aplica");
            Property(x => x.justificacionOpcional).HasColumnName("justificacionOpcional");
            Property(x => x.justificacionValidacion).HasColumnName("justificacionValidacion");
            Property(x => x.validacion).HasColumnName("validacion");
            Property(x => x.rutaArchivoValidacion).HasColumnName("rutaArchivoValidacion");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.fechaValidacion).HasColumnName("fechaValidacion");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblX_RelacionSubContratistaDocumentacion");
        }
    }
}
