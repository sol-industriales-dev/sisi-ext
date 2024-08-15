using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Catalogo
{

    public class CorreosEnvioInventarioMapping : EntityTypeConfiguration<tblM_CorreosEnvioInventario>
    {
        public CorreosEnvioInventarioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.correo).HasColumnName("correo");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.TipoEnvio).HasColumnName("TipoEnvio");
            ToTable("tblM_CorreosEnvioInventario");
        }
    }
}
