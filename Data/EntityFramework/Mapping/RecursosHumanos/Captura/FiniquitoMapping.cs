using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    public class FiniquitoMapping : EntityTypeConfiguration<tblRH_Finiquito>
    {
        public FiniquitoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.claveEmpleado).HasColumnName("claveEmpleado");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.ape_paterno).HasColumnName("ape_paterno");
            Property(x => x.ape_materno).HasColumnName("ape_materno");
            Property(x => x.fechaAlta).HasColumnName("fechaAlta");
            Property(x => x.fechaBaja).HasColumnName("fechaBaja");
            Property(x => x.puestoID).HasColumnName("puestoID");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.tipoNominaID).HasColumnName("tipoNominaID");
            Property(x => x.tipoNomina).HasColumnName("tipoNomina");
            Property(x => x.ccID).HasColumnName("ccID");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.salarioBase).HasColumnName("salarioBase");
            Property(x => x.complemento).HasColumnName("complemento");
            Property(x => x.bono).HasColumnName("bono");
            Property(x => x.formuloID).HasColumnName("formuloID");
            Property(x => x.formuloNombre).HasColumnName("formuloNombre");
            Property(x => x.voboID).HasColumnName("voboID");
            Property(x => x.voboNombre).HasColumnName("voboNombre");
            Property(x => x.autorizoID).HasColumnName("autorizoID");
            Property(x => x.autorizoNombre).HasColumnName("autorizoNombre");
            Property(x => x.total).HasColumnName("total");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.autorizado).HasColumnName("autorizado");
            ToTable("tblRH_Finiquito");
        }
    }
}
