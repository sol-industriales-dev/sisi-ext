using Core.Entity.Maquinaria._Caratulas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria._Caratulas
{
    public class tblM_CaratulaAgrupacionDetMapping: EntityTypeConfiguration<tblM_CaratulaAgrupacionDet>
    {
        public tblM_CaratulaAgrupacionDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");
            Property(x => x.idGrupo).HasColumnName("idGrupo");
            Property(x => x.idModelo).HasColumnName("idModelo");
            Property(x => x.esActivo).HasColumnName("esActivo");
      
            ToTable("tblM_CaratulaAgrupacionDet");
        }
    }
}
