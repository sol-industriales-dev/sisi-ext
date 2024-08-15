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
    public class ObrasMapping : EntityTypeConfiguration<tblPro_Obras>
    {
        public ObrasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Area).HasColumnName("Area");
            Property(x => x.Codigo).HasColumnName("Codigo");
            Property(x => x.Descripcion).HasColumnName("Descripcion");
            Property(x => x.Probabilidad).HasColumnName("Probabilidad");
            Property(x => x.Margen).HasColumnName("Margen");
            Property(x => x.Monto).HasColumnName("Monto");
            Property(x => x.Fecha1).HasColumnName("Fecha1");
            Property(x => x.Fecha2).HasColumnName("Fecha2");
            Property(x => x.Fecha3).HasColumnName("Fecha3");
            Property(x => x.Fecha4).HasColumnName("Fecha4");
            Property(x => x.Fecha5).HasColumnName("Fecha5");
            Property(x => x.Fecha6).HasColumnName("Fecha6");
            Property(x => x.Fecha7).HasColumnName("Fecha7");
            Property(x => x.Fecha8).HasColumnName("Fecha8");
            Property(x => x.Fecha9).HasColumnName("Fecha9");
            Property(x => x.Fecha10).HasColumnName("Fecha10");
            Property(x => x.Fecha11).HasColumnName("Fecha11");
            Property(x => x.Fecha12).HasColumnName("Fecha12");
            Property(x => x.Total).HasColumnName("Total");
            Property(x => x.Tipo).HasColumnName("Tipo");
            ToTable("tblPro_Obras");
        }
    }
}
