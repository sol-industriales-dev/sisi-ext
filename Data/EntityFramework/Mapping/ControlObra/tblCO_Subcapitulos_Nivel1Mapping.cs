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
    class tblCO_Subcapitulos_Nivel1Mapping : EntityTypeConfiguration<tblCO_Subcapitulos_Nivel1>
    {
        public tblCO_Subcapitulos_Nivel1Mapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.subcapitulo).HasColumnName("subcapitulo");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.capitulo_id).HasColumnName("capitulo_id");
            HasRequired(x => x.capitulo).WithMany(x => x.subcapitulos_N1).HasForeignKey(d => d.capitulo_id);
            ToTable("tblCO_Subcapitulos_Nivel1");
        }
    }
}
