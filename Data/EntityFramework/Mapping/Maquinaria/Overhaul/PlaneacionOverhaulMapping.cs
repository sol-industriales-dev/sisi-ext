using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Overhaul
{
    public class PlaneacionOverhaulMapping : EntityTypeConfiguration<tblM_CapPlaneacionOverhaul>
    {
        public PlaneacionOverhaulMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.maquinaID).HasColumnName("maquinaID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.idComponentes).HasColumnName("idComponente");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.indexCal).HasColumnName("indexCal");
            Property(x => x.actividades).HasColumnName("actividades");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.diasDuracionP).HasColumnName("diasDuracionP");
            Property(x => x.diasTrabajados).HasColumnName("diasTrabajados");
            Property(x => x.fechaFinP).HasColumnName("fechaFinP");
            Property(x => x.ritmo).HasColumnName("ritmo");
            Property(x => x.terminado).HasColumnName("terminado");
            Property(x => x.indexCalOriginal).HasColumnName("indexCalOriginal");

            HasRequired(x => x.calendario).WithMany().HasForeignKey(y => y.calendarioID);
            //HasRequired(x => x.componente).WithMany().HasForeignKey(y => y.idComponente);

            ToTable("tblM_CapPlaneacionOverhaul");
        }
    }
}


