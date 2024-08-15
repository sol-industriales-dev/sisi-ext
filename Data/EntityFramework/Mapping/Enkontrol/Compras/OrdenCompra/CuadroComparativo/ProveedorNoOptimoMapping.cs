using Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.OrdenCompra.CuadroComparativo
{
    public class ProveedorNoOptimoMapping : EntityTypeConfiguration<tblCom_CC_ProveedorNoOptimo>
    {
        public ProveedorNoOptimoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.CalificacionId).HasColumnName("CalificacionId");
            Property(x => x.NumeroCompra).HasColumnName("NumeroCompra");
            Property(x => x.Estatus).HasColumnName("Estatus");
            Property(x => x.FechaCreacion).HasColumnName("FechaCreacion");
            Property(x => x.VoBo).HasColumnName("VoBo");
            Property(x => x.IdTipoCalificacion).HasColumnName("IdTipoCalificacion");
            Property(x => x.Fecha).HasColumnName("Fecha");
            Property(x => x.IdPartida).HasColumnName("IdPartida");
            Property(x => x.UsuarioIdVoBo).HasColumnName("UsuarioIdVoBo");
            HasRequired(x => x.Calificacion).WithMany().HasForeignKey(y => y.CalificacionId);
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(y => y.UsuarioIdVoBo);
            HasOptional(x => x.partida).WithMany().HasForeignKey(y => y.IdPartida);
            ToTable("tblCom_CC_ProveedorNoOptimo");
        }
    }
}
