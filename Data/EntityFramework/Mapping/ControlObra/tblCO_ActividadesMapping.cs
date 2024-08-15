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
    class tblCO_ActividadesMapping : EntityTypeConfiguration<tblCO_Actividades>
    {
        public tblCO_ActividadesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.actividad).HasColumnName("actividad");
            Property(x => x.cantidad).HasColumnName("cantidad").HasPrecision(20, 4);
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.observaciones).HasColumnName("observaciones");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.actividadTerminada).HasColumnName("actividadTerminada");
            Property(x => x.actividadPadreRequerida).HasColumnName("actividadPadreRequerida");
            Property(x => x.costo).HasColumnName("costo").HasPrecision(20,4);
            Property(x => x.precioUnitario).HasColumnName("precioUnitario").HasPrecision(20, 4);
            Property(x => x.importeContratado).HasColumnName("importeContratado").HasPrecision(20, 4);
            Property(x => x.tipoPeriodoAvance).HasColumnName("tipoPeriodoAvance");        

            Property(x => x.subcapituloN1_id).HasColumnName("subcapituloN1_id");
            HasRequired(x => x.subcapitulos_N1).WithMany(x => x.actividades).HasForeignKey(d => d.subcapituloN1_id);

            Property(x => x.subcapituloN2_id).HasColumnName("subcapituloN2_id");
            HasRequired(x => x.subcapitulos_N2).WithMany(x => x.actividades).HasForeignKey(d => d.subcapituloN2_id);

            Property(x => x.subcapituloN3_id).HasColumnName("subcapituloN3_id");
            HasRequired(x => x.subcapitulos_N3).WithMany(x => x.actividades).HasForeignKey(d => d.subcapituloN3_id);
        
            Property(x => x.actividadPadre_id).HasColumnName("actividadPadre_id");
            HasRequired(x => x.actividadPadre).WithMany(x => x.actividades).HasForeignKey(d => d.actividadPadre_id);

            ToTable("tblCO_Actividades");
        }
    }
}
