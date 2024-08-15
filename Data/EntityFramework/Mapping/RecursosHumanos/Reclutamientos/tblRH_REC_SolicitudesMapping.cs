using Core.Entity.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_SolicitudesMapping : EntityTypeConfiguration<tblRH_REC_Solicitudes>
    {
        public tblRH_REC_SolicitudesMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.idPuesto).HasColumnName("idPuesto");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.clave_depto).HasColumnName("clave_depto");
            Property(x => x.esGeneral).HasColumnName("esGeneral");
            Property(x => x.idMotivo).HasColumnName("idMotivo");
            Property(x => x.sexo).HasColumnName("sexo");
            Property(x => x.rangoInicioEdad).HasColumnName("rangoInicioEdad");
            Property(x => x.rangoFinEdad).HasColumnName("rangoFinEdad");
            Property(x => x.idEscolaridad).HasColumnName("idEscolaridad");
            Property(x => x.clave_pais_nac).HasColumnName("clave_pais_nac");
            Property(x => x.clave_estado_nac).HasColumnName("clave_estado_nac");
            Property(x => x.clave_ciudad_nac).HasColumnName("clave_ciudad_nac");
            Property(x => x.aniosExp).HasColumnName("aniosExp");
            Property(x => x.conocimientoGen).HasColumnName("conocimientoGen");
            Property(x => x.expEspecializada).HasColumnName("expEspecializada");
            Property(x => x.cantVacantes).HasColumnName("cantVacantes");
            Property(x => x.cantVacantesCubiertas).HasColumnName("cantVacantesCubiertas");
            Property(x => x.esPuestoNuevo).HasColumnName("esPuestoNuevo");
            Property(x => x.terminado).HasColumnName("terminado");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            HasRequired(x => x.virtualLstMotivos).WithMany().HasForeignKey(y => y.idMotivo);
            HasRequired(x => x.virtualLstEscolaridades).WithMany().HasForeignKey(y => y.idEscolaridad);

            ToTable("tblRH_REC_Solicitudes");
        }
    }
}
