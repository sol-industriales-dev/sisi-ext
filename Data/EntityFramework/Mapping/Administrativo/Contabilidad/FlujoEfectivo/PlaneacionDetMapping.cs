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
    public class PlaneacionDetMapping : EntityTypeConfiguration<tblC_FED_PlaneacionDet>
    {
        public PlaneacionDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.ac).HasColumnName("ac");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.monto).HasColumnName("monto").HasPrecision(22,4);
            Property(x => x.semana).HasColumnName("semana");
            Property(x => x.usuarioCaptura).HasColumnName("usuarioCaptura");
            Property(x => x.año).HasColumnName("año");
            Property(x => x.sp_gastos_provID).HasColumnName("sp_gastos_provID");
            Property(x => x.factura).HasColumnName("factura");
            Property(x => x.nominaID).HasColumnName("nominaID");
            Property(x => x.cadenaProductivaID).HasColumnName("cadenaProductivaID");
            Property(x => x.numcte).HasColumnName("numcte");
            Property(x => x.numprov).HasColumnName("numprov");
            Property(x => x.fechaFactura).HasColumnName("fechaFactura");
            Property(x => x.idDetProyGemelo).HasColumnName("idDetProyGemelo");
            Property(X => X.categoriaTipo).HasColumnName("categoriaTipo");
            ToTable("tblC_FED_PlaneacionDet");

        }
    }
}
