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
    public class tblRH_BN_ListaBlancaMapping : EntityTypeConfiguration<tblRH_BN_ListaBlanca>
    {
        public tblRH_BN_ListaBlancaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cve_Emp).HasColumnName("cve_Emp");
            Property(x => x.nombre_Emp).HasColumnName("nombre_Emp");
            Property(x => x.puesto_Emp).HasColumnName("puesto_Emp");
            Property(x => x.puestoCve_Emp).HasColumnName("puestoCve_Emp");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.usuarioID);

            ToTable("tblRH_BN_ListaBlanca");
        }
    }
}
