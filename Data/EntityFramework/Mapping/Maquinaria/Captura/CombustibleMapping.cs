using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class CombustibleMapping : EntityTypeConfiguration<tblM_CapCombustible>
    {
        public CombustibleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.Economico).HasColumnName("Economico");
            Property(x => x.surtidor).HasColumnName("surtidor");
            Property(x => x.turno).HasColumnName("turno");
            Property(x => x.volumne_carga).HasColumnName("volumne_carga");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.Carga1).HasColumnName("Carga1");
            Property(x => x.Carga2).HasColumnName("Carga2");
            Property(x => x.Carga3).HasColumnName("Carga3");
            Property(x => x.Carga4).HasColumnName("Carga4");
            Property(x => x.HorometroCarga1).HasColumnName("HorometroCarga1");
            Property(x => x.HorometroCarga2).HasColumnName("HorometroCarga2");
            Property(x => x.HorometroCarga3).HasColumnName("HorometroCarga3");
            Property(x => x.HorometroCarga4).HasColumnName("HorometroCarga4");
            Property(x => x.PrecioLitro).HasColumnName("PrecioLitro");
            Property(x => x.PrecioTotal).HasColumnName("PrecioTotal");
            Property(x => x.aplicarCosto).HasColumnName("aplicarCosto");
            Property(x => x.FechaCaptura).HasColumnName("FechaCaptura");

            Property(x => x.pipa1).HasColumnName("pipa1");
            Property(x => x.pipa2).HasColumnName("pipa2");
            Property(x => x.pipa3).HasColumnName("pipa3");
            Property(x => x.pipa4).HasColumnName("pipa4");

            ToTable("tblM_CapCombustible");
        }
    }
}
