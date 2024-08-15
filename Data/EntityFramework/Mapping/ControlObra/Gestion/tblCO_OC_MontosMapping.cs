using Core.Entity.ControlObra.GestionDeCambio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra.Gestion
{
    public class tblCO_OC_MontosMapping : EntityTypeConfiguration<tblCO_OC_Montos>
    {
        public tblCO_OC_MontosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.no).HasColumnName("no");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.unidad).HasColumnName("unidad");
            Property(x => x.cantidad).HasColumnName("cantidad");
            Property(x => x.PrecioUnitario).HasColumnName("PrecioUnitario");
            Property(x => x.importe).HasColumnName("importe");
            Property(x => x.idOrdenDeCambio).HasColumnName("idOrdenDeCambio");
            Property(x => x.tipoSoportes).HasColumnName("tipoSoportes");

            ToTable("tblCO_OC_Montos");
        }
    }
}
