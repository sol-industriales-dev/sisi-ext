using Core.Entity.Maquinaria.Rentabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Rentabilidad
{
    public class KBCorteMapping : EntityTypeConfiguration<tblM_KBCorte>
    {
        KBCorteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.fechaCorte).HasColumnName("fechaCorte");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.guardadoConstruplan).HasColumnName("guardadoConstruplan");
            Property(x => x.guardadoArrendadora).HasColumnName("guardadoArrendadora");
            Property(x => x.costoEstCerrado).HasColumnName("costoEstCerrado");
            Property(x => x.anio).HasColumnName("anio");
            ToTable("tblM_KBCorte");
        }
    }
}