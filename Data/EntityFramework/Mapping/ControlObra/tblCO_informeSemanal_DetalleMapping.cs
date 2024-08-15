using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.EntityFramework.Mapping.ControlObra
{
    class tblCO_informeSemanal_DetalleMapping : EntityTypeConfiguration<tblCO_informeSemanal_detalle>
    {
        public tblCO_informeSemanal_DetalleMapping()
         {
             HasKey(x => x.id);
             Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
             Property(x => x.ordenDiapositiva).HasColumnName("ordenDiapositiva");
             Property(x => x.tituloDiapositiva).HasColumnName("tituloDiapositiva");
             Property(x => x.contenido).HasColumnName("contenido");
             Property(x => x.pdf).HasColumnName("pdf");     

             Property(x => x.informe_id).HasColumnName("informe_id");
             HasRequired(x => x.informe).WithMany(x => x.informeDetalles).HasForeignKey(d => d.informe_id);

             ToTable("tblCO_informeSemanal_Detalle");
         }
    }
}
