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
    public class CalendarioPlaneacionOverhaulMapping : EntityTypeConfiguration<tblM_CalendarioPlaneacionOverhaul>
    {
        CalendarioPlaneacionOverhaulMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.grupoMaquinaID).HasColumnName("grupoMaquinaID");
            Property(x => x.modeloMaquinaID).HasColumnName("modeloMaquinaID");
            Property(x => x.obraID).HasColumnName("obraID");
            Property(x => x.subConjuntoID).HasColumnName("subconjuntoID");
            Property(x => x.anio).HasColumnName("anio");
            ToTable("tblM_CalendarioPlaneacionOverhaul");
        }
    }
}