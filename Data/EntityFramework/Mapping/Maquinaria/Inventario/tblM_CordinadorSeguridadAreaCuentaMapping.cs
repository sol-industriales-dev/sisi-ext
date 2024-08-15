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
    public class tblM_CordinadorSeguridadAreaCuentaMapping : EntityTypeConfiguration<tblM_CordinadorSeguridadAreaCuenta>
    {
        public tblM_CordinadorSeguridadAreaCuentaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(x => x.usuarioId);
            ToTable("tblM_CordinadorSeguridadAreaCuenta");
        }
    }
}
