using Core.Entity.Maquinaria.KPI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura.KPI
{
    public class tblM_KPI_Homologado_TurnosMapping : EntityTypeConfiguration<tblM_KPI_Homologado_Turnos>
    {
        public tblM_KPI_Homologado_TurnosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.ac).HasColumnName("ac");
            Property(x => x.turnos).HasColumnName("turnos");
            Property(x => x.horas_turno).HasColumnName("horas_turno");
            Property(x => x.horas_dia).HasColumnName("horas_dia");
            ToTable("tblM_KPI_Homologado_Turnos");
        }
    }
}
