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
    class tblCO_CapitulosMapping : EntityTypeConfiguration<tblCO_Capitulos>
    {
        public tblCO_CapitulosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.capitulo).HasColumnName("capitulo");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.periodoFacturacion).HasColumnName("periodoFacturacion");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.cc_id).HasColumnName("cc_id");
            HasRequired(x => x.cc).WithMany(x => x.capitulo).HasForeignKey(d => d.cc_id);
            Property(x => x.autorizante_id).HasColumnName("autorizante_id");
            HasRequired(x => x.usuario).WithMany(x => x.capitulo).HasForeignKey(d => d.autorizante_id);
            ToTable("tblCO_Capitulos");
        }
    }
}

