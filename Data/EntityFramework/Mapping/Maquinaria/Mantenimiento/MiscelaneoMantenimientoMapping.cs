using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Mantenimiento
{
    public class MiscelaneoMantenimientoMapping : EntityTypeConfiguration<tblM_MiscelaneoMantenimiento>
    {
        public MiscelaneoMantenimientoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idAct).HasColumnName("idAct");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.idCompVis).HasColumnName("idCompVis");
            Property(x => x.idTipo).HasColumnName("idTipo");
            Property(x => x.modeloEquipoID).HasColumnName("modeloEquipoID");
            Property(x => x.idMis).HasColumnName("idMis");
            Property(x => x.vida).HasColumnName("vida");
            Property(x => x.cantidad).HasColumnName("cantidad");
            Property(x => x.UsuarioCap).HasColumnName("UsuarioCap");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            ToTable("tblM_MiscelaneoMantenimiento");
        }
    }
}
