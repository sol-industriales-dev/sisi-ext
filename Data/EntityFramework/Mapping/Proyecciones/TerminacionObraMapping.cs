using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Proyecciones
{
    public class TerminacionObraMapping : EntityTypeConfiguration<tblPro_CierreObra>
    {
        public TerminacionObraMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.capturadeObrasID).HasColumnName("capturadeObrasID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.AnticipoMonto).HasColumnName("AnticipoMonto");
            Property(x => x.Bien).HasColumnName("Bien");
            Property(x => x.CantidadPersonal).HasColumnName("CantidadPersonal");
            Property(x => x.capturadeObrasID).HasColumnName("capturadeObrasID");
            Property(x => x.Comentarios).HasColumnName("Comentarios");
            Property(x => x.Contactos).HasColumnName("Contactos");
            Property(x => x.CuantoCotizo).HasColumnName("CuantoCotizo");
            Property(x => x.DatosEconomicos).HasColumnName("DatosEconomicos");
            Property(x => x.Mal).HasColumnName("Mal");
            Property(x => x.Margen).HasColumnName("Margen");
            Property(x => x.MontoUtilidad).HasColumnName("MontoUtilidad");
            Property(x => x.Porcentaje).HasColumnName("Porcentaje");
            Property(x => x.registroID).HasColumnName("registroID");
            Property(x => x.Retenciones).HasColumnName("Retenciones");

            ToTable("tblPro_CierreObra");
        }
    }
}
