using Core.Entity.Enkontrol.Compras.OrdenCompra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.OrdenCompra
{
    class InsumosExcepcionUsuarioUso : EntityTypeConfiguration<tblCom_InsumosExcepcionUsuarioUso>
    {
        public InsumosExcepcionUsuarioUso()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.insumo).HasColumnName("insumo");
            Property(x => x.familia).HasColumnName("familia");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.grupo).HasColumnName("grupo");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblCom_InsumosExcepcionUsuarioUso");
        }
    }
}
