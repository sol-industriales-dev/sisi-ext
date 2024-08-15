using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    public class ControlInternoMovimientosMapping : EntityTypeConfiguration<tblM_ControMovimientoInterno>
    {
        public ControlInternoMovimientosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Bateria).HasColumnName("Bateria");
            Property(x => x.Combustible).HasColumnName("Combustible");
            Property(x => x.Comentario).HasColumnName("Comentario");
            Property(x => x.EconomicoID).HasColumnName("EconomicoID");
            Property(x => x.Envio).HasColumnName("Envio");
            Property(x => x.EstadoRegistro).HasColumnName("EstadoRegistro");
            Property(x => x.Estatus).HasColumnName("Estatus");
            Property(x => x.Folio).HasColumnName("Folio");
            Property(x => x.Horometro).HasColumnName("Horometro");
            Property(x => x.Marca2).HasColumnName("Marca2");
            Property(x => x.Serie2).HasColumnName("Serie2");
            Property(x => x.Registro).HasColumnName("Registro");
            Property(x => x.FechaCaptura).HasColumnName("FechaCaptura");
            ToTable("tblM_ControMovimientoInterno");
        }
    }
}
