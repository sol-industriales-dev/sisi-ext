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
    class tblCO_Actividades_FacturadoMapping : EntityTypeConfiguration<tblCO_Actividades_Facturado>
    {
        public tblCO_Actividades_FacturadoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");          
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.autorizado).HasColumnName("autorizado");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.capitulo_id).HasColumnName("capitulo_id");
            HasRequired(x => x.capitulo).WithMany(x => x.actividad_facturado).HasForeignKey(d => d.capitulo_id);

            ToTable("tblCO_Actividades_Facturado");
        }
    }
}
