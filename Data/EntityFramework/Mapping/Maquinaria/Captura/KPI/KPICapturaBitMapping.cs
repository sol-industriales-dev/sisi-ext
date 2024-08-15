using Core.Entity.Maquinaria.KPI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura.KPI
{
    public class KPICapturaBitMapping: EntityTypeConfiguration<tblM_KPI_KPICapturaBit>
    {
        public KPICapturaBitMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.año).HasColumnName("año");
            Property(x => x.authEstado).HasColumnName("authEstado");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.id).HasColumnName("id");
            Property(x => x.kpiSemana).HasColumnName("kpiSemana");
            Property(x => x.semana).HasColumnName("semana");
            Property(x => x.usuarioCaptura).HasColumnName("usuarioCaptura");
            Property(x => x.idAutoriza).HasColumnName("idAutoriza");
            Property(x => x.ac).HasColumnName("ac");
            Property(x => x.validado).HasColumnName("validado");

            ToTable("tblM_KPI_KPICapturaBit");
        }
    }
}
