using Core.Entity.Maquinaria.KPI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura.KPI
{
    public class AuthHomologadoMapping : EntityTypeConfiguration<tblM_KPI_AuthHomologado>
    {
        public AuthHomologadoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Año).HasColumnName("Año");
            Property(x => x.AC).HasColumnName("AC");
            Property(x => x.AuthEstado).HasColumnName("AuthEstado");
            Property(x => x.Comentario).HasColumnName("Comentario");
            Property(x => x.UsuarioElaboraID).HasColumnName("UsuarioElaboraID");
            Property(x => x.UsuarioVobo1).HasColumnName("UsuarioVobo1");
            Property(x => x.UsuarioVobo2).HasColumnName("UsuarioVobo2");
            Property(x => x.UsuarioAutoriza).HasColumnName("UsuarioAutoriza");
            Property(x => x.CadenaElabora).HasColumnName("CadenaElabora");
            Property(x => x.CadenaVobo1).HasColumnName("CadenaVobo1");
            Property(x => x.CadenaVobo2).HasColumnName("CadenaVobo2");
            Property(x => x.CadenaAutoriza).HasColumnName("CadenaAutoriza");
            Property(x => x.FirmaElabora).HasColumnName("FirmaElabora");
            Property(x => x.FirmaVobo1).HasColumnName("FirmaVobo1");
            Property(x => x.FirmaVobo2).HasColumnName("FirmaVobo2");
            Property(x => x.FirmaAutoriza).HasColumnName("FirmaAutoriza");
            Property(x => x.FechaElaboracion).HasColumnName("FechaElaboracion");
            Property(x => x.FechaVobo1).HasColumnName("FechaVobo1");
            Property(x => x.FechaVobo2).HasColumnName("FechaVobo2");
            Property(x => x.FechaAutoriza).HasColumnName("FechaAutoriza");
            Property(x => x.Activo).HasColumnName("Activo");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            ToTable("tblM_KPI_AuthHomologado");
        }
    }
}
