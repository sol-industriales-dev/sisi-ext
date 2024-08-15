using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.FlujoEfectivo
{
    public class DetProyeccionCierreMapping : EntityTypeConfiguration<tblC_FED_DetProyeccionCierre>
    {
        public DetProyeccionCierreMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idConceptoDir).HasColumnName("idConceptoDir");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.ac).HasColumnName("ac");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.semana).HasColumnName("semana");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.naturaleza).HasColumnName("naturaleza");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.numcte).HasColumnName("numcte");
            Property(x => x.numpro).HasColumnName("numpro");
            Property(x => x.factura).HasColumnName("factura");
            Property(x => x.fechaFactura).HasColumnName("fechaFactura");
            Property(x => x.grupo).HasColumnName("grupo");
            Property(x => x.idDetProyGemelo).HasColumnName("idDetProyGemelo");
            Property(x => x.grupoID).HasColumnName("grupoID");
            ToTable("tblC_FED_DetProyeccionCierre");
        }
    }
}
