using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra
{
    class tblCO_Subcapitulos_Nivel2Mapping : EntityTypeConfiguration<tblCO_Subcapitulos_Nivel2>
    {
        public tblCO_Subcapitulos_Nivel2Mapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.subcapitulo).HasColumnName("subcapitulo");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.subcapituloN1_id).HasColumnName("subcapituloN1_id");
            HasRequired(x => x.subcapitulos_N1).WithMany(x => x.subcapitulos_N2).HasForeignKey(d => d.subcapituloN1_id);
            ToTable("tblCO_Subcapitulos_Nivel2");
        }
    }
}
