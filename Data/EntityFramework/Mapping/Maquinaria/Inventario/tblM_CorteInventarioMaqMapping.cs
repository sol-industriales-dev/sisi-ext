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
    public class tblM_CorteInventarioMaqMapping : EntityTypeConfiguration<tblM_CorteInventarioMaq>
    {
        public tblM_CorteInventarioMaqMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.FechaCorte).HasColumnName("FechaCorte");
            Property(x => x.IdUsuarioCorte).HasColumnName("IdUsuarioCorte");
            Property(x => x.IdTipoMaquina).HasColumnName("IdTipoMaquina");
            Property(x => x.Estatus).HasColumnName("Estatus");
            Property(x => x.Bloqueado).HasColumnName("Bloqueado");
            Property(x => x.BloqueadoConstruplan).HasColumnName("BloqueadoConstruplan");
            Property(x => x.FechaCreacion).HasColumnName("FechaCreacion");
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(y => y.IdUsuarioCorte);
            HasOptional(x => x.TipoMaquina).WithMany().HasForeignKey(y => y.IdTipoMaquina);
            ToTable("tblM_CorteInventarioMaq");
        }
    }
}