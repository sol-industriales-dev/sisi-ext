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
    public class tblM_BitacoraDetActividadesMantProyMapping : EntityTypeConfiguration<tblM_BitacoraDetActividadesMantProy>
    {
        public tblM_BitacoraDetActividadesMantProyMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.aplicar).HasColumnName("asignado");
            Property(x => x.idAct).HasColumnName("idAct");
            Property(x => x.idCompVis).HasColumnName("idComp");
            Property(x => x.idMant).HasColumnName("idMant");
            Property(x => x.modeloEquipoID).HasColumnName("modeloEquipoID");
            Property(x => x.programado).HasColumnName("programado");
            ToTable("tblM_BitacoraDetActividadesMantProy");
        }
    }
}
