using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    class tblM_ComparativoAdiquisicionyRentaMapping : EntityTypeConfiguration<tblM_ComparativoAdquisicionyRenta>
    {
        public tblM_ComparativoAdiquisicionyRentaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idAsignacion).HasColumnName("idAsignacion");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.idMegusta).HasColumnName("idMegusta");
            Property(x => x.estatusFinanciera).HasColumnName("estatusFinanciera");
            Property(x => x.folioAdquisicion).HasColumnName("folioAdquisicion");
            Property(x => x.folioFinanciera).HasColumnName("folioFinanciera");
            Property(x => x.ComentarioGeneral).HasColumnName("ComentarioGeneral");
            Property(x => x.fechaDeElaboracion).HasColumnName("fechaDeElaboracion");
            Property(x => x.fechaDeElaboracionFinanciero).HasColumnName("fechaDeElaboracionFinanciero");
            Property(x => x.obra).HasColumnName("obra");
            Property(x => x.nombreDelEquipo).HasColumnName("nombreDelEquipo");
            Property(x => x.compra).HasColumnName("compra");
            Property(x => x.renta).HasColumnName("renta");
            Property(x => x.roc).HasColumnName("roc");
            Property(x => x.tipoMoneda).HasColumnName("tipoMoneda");
            
           
            ToTable("tblM_ComparativoAdquisicionyRenta");
        }
    }
}
