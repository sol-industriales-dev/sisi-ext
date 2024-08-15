using Core.Entity.ControlObra;
using Core.Entity.Maquinaria.Reporte;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra
{
    class tblP_EncabezadoMapping : EntityTypeConfiguration<tblP_Encabezado>
    {
        public tblP_EncabezadoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.logo).HasColumnName("logo");
            Property(x => x.nombreEmpresa).HasColumnName("nombreEmpresa");
            Property(x => x.nombreReporte).HasColumnName("nombreReporte");
            Property(x => x.titulo).HasColumnName("titulo");
            ToTable("tblP_Encabezado");
        }
    }
}
