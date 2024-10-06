using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Multiempresa
{
    public class CCMapping : EntityTypeConfiguration<tblP_CC>
    {
        public CCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.ccRH).HasColumnName("ccRH");
            Property(x => x.descripcionRH).HasColumnName("descripcionRH");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.departamentoID).HasColumnName("departamentoID");
            Property(x => x.abreviacion).HasColumnName("abreviacion");
            Property(x => x.esQuincenaNormal).HasColumnName("esQuincenaNormal");
            Property(x => x.isBajio).HasColumnName("isBajio");
            Property(x => x.fechaArranque).HasColumnName("fechaArranque");
            Property(x => x.ordernFlujoEfectivo).HasColumnName("ordernFlujoEfectivo");
            HasRequired(x => x.departamento).WithMany(x => x.listaCC).HasForeignKey(d => d.departamentoID);
            Property(x => x.grupoID).HasColumnName("grupoID");
            HasRequired(x => x.grupo).WithMany(x => x.listaCC).HasForeignKey(d => d.grupoID);
            Property(x => x.st_ppto).HasColumnName("st_ppto");
            Property(x => x.valida_anio).HasColumnName("valida_anio");
            ToTable("tblP_CC");
        }
    }
}
