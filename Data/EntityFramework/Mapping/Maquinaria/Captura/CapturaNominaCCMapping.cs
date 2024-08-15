using Core.Entity.Maquinaria.Captura;
using Core.Service.Maquinaria.Capturas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    class CapturaNominaCCMapping : EntityTypeConfiguration<tblM_CapNominaCC>
    {
        public CapturaNominaCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.periodoInicial).HasColumnName("periodoInicial");
            Property(x => x.periodoFinal).HasColumnName("periodoFinal");
            Property(x => x.nominaSemanal).HasColumnName("nominaSemanal");
            Property(x => x.archivo).HasColumnName("archivo");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.isVerificado).HasColumnName("isVerificado");
            Property(x => x.ac).HasColumnName("ac");
            Property(x => x.areaCuentaDescripcion).HasColumnName("areaCuentaDescripcion");
            ToTable("tblM_CapNominaCC");
        }
    }
}
