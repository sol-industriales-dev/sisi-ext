using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class tblM_CatMaquina_EstatusDiarioMapping : EntityTypeConfiguration<tblM_CatMaquina_EstatusDiario>
    {
        public tblM_CatMaquina_EstatusDiarioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.usuario).HasColumnName("usuario");
            Property(x => x.cantActivos).HasColumnName("cantActivos");
            Property(x => x.cantInactivos).HasColumnName("cantInactivos");
            Property(x => x.porActivos).HasColumnName("porActivos");
            Property(x => x.porInactivos).HasColumnName("porInactivos");
            
            ToTable("tblM_CatMaquina_EstatusDiario");
        }
    }
}
