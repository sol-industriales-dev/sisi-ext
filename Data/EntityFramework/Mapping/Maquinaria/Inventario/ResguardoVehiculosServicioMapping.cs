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
    public class ResguardoVehiculosServicioMapping : EntityTypeConfiguration<tblM_ResguardoVehiculosServicio>
    {
        ResguardoVehiculosServicioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CheckLiberacionArchivo).HasColumnName("CheckLiberacionArchivo");
            Property(x => x.CheckLiberacionRuta).HasColumnName("CheckLiberacionRuta");
            Property(x => x.Fecha).HasColumnName("Fecha");
            Property(x => x.FormatoResguardo).HasColumnName("FormatoResguardo");

            Property(x => x.FormatoResguardoArchivo).HasColumnName("FormatoResguardoArchivo");
            Property(x => x.FormatoResguardoRuta).HasColumnName("FormatoResguardoRuta");
            Property(x => x.Kilometraje).HasColumnName("Kilometraje");
            Property(x => x.LicenciaArchivo).HasColumnName("LicenciaArchivo");
            Property(x => x.LicenciaRuta).HasColumnName("LicenciaRuta");
            Property(x => x.ManualMMTOPArchivo).HasColumnName("ManualMMTOPArchivo");
            Property(x => x.ManualMMTOPRuta).HasColumnName("ManualMMTOPRuta");
            Property(x => x.MaquinariaID).HasColumnName("MaquinariaID");
            Property(x => x.noEmpleado).HasColumnName("noEmpleado");
            Property(x => x.nombEmpleado).HasColumnName("nombEmpleado");
            Property(x => x.Obra).HasColumnName("Obra");
            Property(x => x.PermisoCargaArchivo).HasColumnName("PermisoCargaArchivo");
            Property(x => x.PermisoCargaRuta).HasColumnName("PermisoCargaRuta");
            Property(x => x.PolizaSegurosArchivo).HasColumnName("PolizaSegurosArchivo");
            Property(x => x.PolizaSegurosRuta).HasColumnName("PolizaSegurosRuta");
            Property(x => x.Puesto).HasColumnName("Puesto");
            Property(x => x.TarjetaCirculacionArchivo).HasColumnName("TarjetaCirculacionArchivo");
            Property(x => x.TarjetaCirculacionRuta).HasColumnName("TarjetaCirculacionRuta");
            Property(x => x.TipoEncierro).HasColumnName("TipoEncierro");
            Property(x => x.fechaVencimiento).HasColumnName("fechaVencimiento");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.Comentario).HasColumnName("Comentario");
            Property(x => x.Placas).HasColumnName("Placas");
            Property(x => x.DocumentoFirmadoRuta).HasColumnName("DocumentoFirmadoRuta");
            Property(x => x.DocumentoFirmado).HasColumnName("DocumentoFirmado");
            Property(x => x.DocumentoAnexo).HasColumnName("DocumentoAnexo");
            Property(x => x.DocumentoAnexoRuta).HasColumnName("DocumentoAnexoRuta");
            Property(x => x.fechaVigenciaCurso).HasColumnName("fechaVigenciaCurso");
            ToTable("tblM_ResguardoVehiculosServicio");
        }
    }
}
