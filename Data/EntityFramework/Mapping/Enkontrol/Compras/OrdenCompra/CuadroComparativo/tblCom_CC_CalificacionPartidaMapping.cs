using Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_CC_CalificacionPartidaMapping : EntityTypeConfiguration<tblCom_CC_CalificacionPartida>
    {
        public tblCom_CC_CalificacionPartidaMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCalificacion).HasColumnName("idCalificacion");
            Property(x => x.numeroPartida).HasColumnName("numeroPartida");
            Property(x => x.idTipoCalificacionPartida).HasColumnName("idTipoCalificacionPartida");
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.precio).HasColumnName("precio");
            Property(x => x.tiempoEntrega).HasColumnName("tiempoEntrega");
            Property(x => x.condicionPago).HasColumnName("condicionPago");
            Property(x => x.LAB).HasColumnName("LAB");
            Property(x => x.confiabilidadProveedor).HasColumnName("confiabilidadProveedor");
            Property(x => x.calidad).HasColumnName("calidad");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            HasRequired(x => x.calificacionProv).WithMany().HasForeignKey(y => y.idCalificacion);

            ToTable("tblCom_CC_CalificacionPartida");
        }
    }
}
