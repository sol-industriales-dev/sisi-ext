using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura.Conciliacion
{
    public class excelCaratulaMapping : EntityTypeConfiguration<excelCaratula>
    {
        public excelCaratulaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.grupo).HasColumnName("grupo");
            Property(x => x.modelo).HasColumnName("modelo");
            Property(x => x.unidad).HasColumnName("unidad");
            Property(x => x.costo).HasColumnName("costo");
            Property(x => x.cargoFijo).HasColumnName("cargoFijo");
            Property(x => x.overhaul).HasColumnName("overhaul");
            Property(x => x.mtoCorrectivo).HasColumnName("mtoCorrectivo");
            Property(x => x.combustible).HasColumnName("combustible");
            Property(x => x.aceites).HasColumnName("aceites");
            Property(x => x.filtros).HasColumnName("filtros");
            Property(x => x.ansul).HasColumnName("ansul");
            Property(x => x.llantas).HasColumnName("llantas");
            Property(x => x.carrileria).HasColumnName("carrileria");
            Property(x => x.desgasteHerramientas).HasColumnName("desgasteHerramientas");
            Property(x => x.cargoOperador).HasColumnName("cargoOperador");
            Property(x => x.personalMto).HasColumnName("personalMto");
            Property(x => x.manoObra).HasColumnName("manoObra");
            ToTable("excelCaratula");
        }
    }
}
