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
    public class tblM_CorteInventarioMaq_DetalleMapping : EntityTypeConfiguration<tblM_CorteInventarioMaq_Detalle>
    {
        public tblM_CorteInventarioMaq_DetalleMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.IdCorteInvMaq).HasColumnName("IdCorteInvMaq");
            Property(x => x.Economico).HasColumnName("Economico");
            Property(x => x.Descripcion).HasColumnName("Descripcion");
            Property(x => x.Marca).HasColumnName("Marca");
            Property(x => x.Modelo).HasColumnName("Modelo");
            Property(x => x.NumeroSerie).HasColumnName("NumeroSerie");
            Property(x => x.Año).HasColumnName("Año");
            Property(x => x.Ubicacion).HasColumnName("Ubicacion");
            Property(x => x.Redireccion).HasColumnName("Redireccion");
            Property(x => x.CargoObra).HasColumnName("CargoObra");
            Property(x => x.Resguardante).HasColumnName("Resguardante");
            Property(x => x.HorometroAcumulado).HasColumnName("HorometroAcumulado");
            Property(x => x.Tipo).HasColumnName("Tipo");
            Property(x => x.IdEconomico).HasColumnName("IdEconomico");
            Property(x => x.Estatus).HasColumnName("Estatus");
            Property(x => x.Empresa).HasColumnName("Empresa");
            HasRequired(x => x.Corte).WithMany().HasForeignKey(y => y.IdCorteInvMaq);
            HasRequired(x => x.Maquina).WithMany().HasForeignKey(y => y.IdEconomico);
            ToTable("tblM_CorteInventarioMaq_Detalle");
        }
    }
}