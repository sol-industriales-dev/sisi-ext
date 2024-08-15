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
    class tblCO_Subcapitulos_Nivel3Mapping : EntityTypeConfiguration<tblCO_Subcapitulos_Nivel3>
    {
        public tblCO_Subcapitulos_Nivel3Mapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.subcapitulo).HasColumnName("subcapitulo");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.subcapituloN2_id).HasColumnName("subcapituloN2_id");
            HasRequired(x => x.subcapitulo_N2).WithMany(x => x.subcapitulos_N3).HasForeignKey(d => d.subcapituloN2_id);
            ToTable("tblCO_Subcapitulos_Nivel3");
        }
    }
}
