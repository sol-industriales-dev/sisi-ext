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
    public class tblM_KB_CorteMapping : EntityTypeConfiguration<tblM_KB_Corte>
    {
        public tblM_KB_CorteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.fechaCorte).HasColumnName("fechaCorte");
            Property(x => x.usuarioCaptura).HasColumnName("usuarioCaptura");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.tipoCorte).HasColumnName("tipoCorte");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.existeCorteOtraEmpresa).HasColumnName("existeCorteOtraEmpresa");
            Property(x => x.estimadosCerrados).HasColumnName("estimadosCerrados");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.corteAnteriorID).HasColumnName("corteAnteriorID");

            ToTable("tblM_KB_Corte");
        }
    }
}