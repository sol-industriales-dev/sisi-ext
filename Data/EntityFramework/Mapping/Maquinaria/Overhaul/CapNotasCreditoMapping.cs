using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Overhaul
{
    public class CapNotasCreditoMapping : EntityTypeConfiguration<tblM_CapNotaCredito>
    {
        public CapNotasCreditoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Generador).HasColumnName("Generador");
            Property(x => x.OC).HasColumnName("OC");
            Property(x => x.idEconomico).HasColumnName("idEconomico");
            Property(x => x.SerieComponente).HasColumnName("SerieComponente");
            Property(x => x.Descripcion).HasColumnName("Descripcion");
            Property(x => x.Fecha).HasColumnName("Fecha");
            Property(x => x.CausaRemosion).HasColumnName("CausaRemosion");
            Property(x => x.HorometroEconomico).HasColumnName("HorometroEconomico");
            Property(x => x.HorometroComponente).HasColumnName("HorometroComponente");
            Property(x => x.MontoPesos).HasColumnName("MontoPesos");
            Property(x => x.MontoDLL).HasColumnName("MontoDLL");
            Property(x => x.AbonoDLL).HasColumnName("AbonoDLL");
            Property(x => x.ClaveCredito).HasColumnName("ClaveCredito");
            Property(x => x.RutaArchivo).HasColumnName("RutaArchivo");
            Property(x => x.TipoNC).HasColumnName("TipoNC");

            Property(x => x.CadenaModifica).HasColumnName("CadenaModifica");
            Property(x => x.EstatusModifica).HasColumnName("EstatusModifica");

            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.factura).HasColumnName("factura");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.noAlmacen).HasColumnName("noAlmacen");
            Property(x => x.numInsumo).HasColumnName("numInsumo");
            Property(x => x.descripcionInsumo).HasColumnName("descripcionInsumo");
            Property(x => x.fechaCasco).HasColumnName("fechaCasco");
            Property(x => x.montoTotalOC).HasColumnName("montoTotalOC");
            ToTable("tblM_CapNotaCredito");
        }
    }
}
