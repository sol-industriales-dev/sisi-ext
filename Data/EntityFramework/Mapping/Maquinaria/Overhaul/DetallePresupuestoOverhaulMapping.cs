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
    public class DetallePresupuestoOverhaulMapping : EntityTypeConfiguration<tblM_DetallePresupuestoOverhaul>
    {
        DetallePresupuestoOverhaulMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.componenteID).HasColumnName("componenteID");
            Property(x => x.maquinaID).HasColumnName("maquinaID");
            Property(x => x.costoSugerido).HasColumnName("costoSugerido");
            Property(x => x.costoPresupuesto).HasColumnName("costoPresupuesto");
            Property(x => x.costoReal).HasColumnName("costoReal");
            Property(x => x.horasCiclo).HasColumnName("horasCiclo");
            Property(x => x.horasAcumuladas).HasColumnName("horasAcumuladas");
            Property(x => x.presupuestoID).HasColumnName("presupuestoID");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.subconjuntoID).HasColumnName("subconjuntoID");
            Property(x => x.obra).HasColumnName("obra");
            Property(x => x.vida).HasColumnName("vida");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.programado).HasColumnName("programado");
            Property(x => x.esServicio).HasColumnName("esServicio");
            Property(x => x.comentarioAumento).HasColumnName("comentarioAumento");
            HasRequired(x => x.componente).WithMany().HasForeignKey(x => x.componenteID);
            HasRequired(x => x.maquina).WithMany().HasForeignKey(x => x.maquinaID);
            HasRequired(x => x.presupuesto).WithMany().HasForeignKey(x => x.presupuestoID);
            ToTable("tblM_DetallePresupuestoOverhaul");
        }
    }
}

