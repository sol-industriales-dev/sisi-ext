using Core.Entity.Administrativo.Contratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contratistas
{
    public class tblP_MunicipioMapping : EntityTypeConfiguration<tblP_Municipio>
    {
        public tblP_MunicipioMapping()
        {
            HasKey(x => x.idMunicipio);
            Property(x => x.idMunicipio).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("idMunicipio");
            Property(x => x.idEstado).HasColumnName("idEstado");
            Property(x => x.Municipio).HasColumnName("Municipio");

            ToTable("tblP_Municipios");
        }
    }
}
