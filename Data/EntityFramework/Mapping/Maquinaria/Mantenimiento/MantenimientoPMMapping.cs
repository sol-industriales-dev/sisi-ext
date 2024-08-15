using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Mantenimiento;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EntityFramework.Mapping.Maquinaria.Mantenimiento
{
    public class MantenimientoPMMapping : EntityTypeConfiguration<tblM_MatenimientoPm>
    {
        public MantenimientoPMMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.economicoID).HasColumnName("economicoID");
            Property(x => x.horometroUltCapturado).HasColumnName("horometroUltCaptura");
            Property(x => x.fechaUltCapturado).HasColumnName("fechaUltCaptura");
            Property(x => x.tipoPM).HasColumnName("tipoPM");
            Property(x => x.fechaPM).HasColumnName("fechaPM");
            Property(x => x.horometroPM).HasColumnName("horometroPM");
            Property(x => x.personalRealizo).HasColumnName("personalRealizo");
            Property(x => x.horometroProy).HasColumnName("horometroProy");
            Property(x => x.fechaProy).HasColumnName("fechaProy");
            Property(x => x.tipoMantenimientoProy).HasColumnName("tipoMantenimientoProy");
            Property(x => x.fechaProyFin).HasColumnName("fechaProyFin");
            Property(x => x.actual).HasColumnName("actual");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.idMaquina).HasColumnName("idMaquina");
            Property(x => x.observaciones).HasColumnName("observaciones");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.planeador).HasColumnName("planeador");
            Property(x => x.UsuarioCap).HasColumnName("UsuarioCap");
            Property(x => x.horometroPMEjecutado).HasColumnName("horometroPMEjecutado");

            Property(x => x.estadoMantenimiento).HasColumnName("estadoMantenimiento");

            ToTable("tblM_MatenimientoPm");
        }
    }
}
