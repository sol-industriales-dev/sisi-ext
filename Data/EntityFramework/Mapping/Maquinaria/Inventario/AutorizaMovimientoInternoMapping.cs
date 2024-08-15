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
    class AutorizaMovimientoInternoMapping : EntityTypeConfiguration<tblM_AutorizaMovimientoInterno>
    {
        public AutorizaMovimientoInternoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cadenafirmaEnterado).HasColumnName("cadenafirmaEnterado");
            Property(x => x.cadenafirmaEnvia).HasColumnName("cadenafirmaEnvia");
            Property(x => x.cadenafirmaRecibe).HasColumnName("cadenafirmaRecibe");
            Property(x => x.ControMovimientoInternoID).HasColumnName("ControMovimientoInternoID");

            Property(x => x.FechaCaptura).HasColumnName("FechaCaptura");
            Property(x => x.firmaEnterado).HasColumnName("firmaEnterado");
            Property(x => x.firmaEnvio).HasColumnName("firmaEnvio");
            Property(x => x.firmaRecibe).HasColumnName("firmaRecibe");
            Property(x => x.usuarioEnvio).HasColumnName("usuarioEnvio");
            Property(x => x.usuarioRecibe).HasColumnName("usuarioRecibe");
            Property(x => x.usuarioValida).HasColumnName("usuarioValida");

            Property(x => x.Autoriza1).HasColumnName("Autoriza1");
            Property(x => x.Autoriza2).HasColumnName("Autoriza2");
            Property(x => x.Autoriza3).HasColumnName("Autoriza3");

            HasRequired(x => x.ControMovimientoInterno).WithMany().HasForeignKey(y => y.ControMovimientoInternoID);
            ToTable("tblM_AutorizaMovimientoInterno");
        }
    }
}
