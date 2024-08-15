using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario.ControlCalidad
{
    class ControlCalidadMapping : EntityTypeConfiguration<tblM_CatControlCalidad>
    {
        public ControlCalidadMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.IdAsignacion).HasColumnName("IdAsignacion");
            Property(x => x.TipoControl).HasColumnName("TipoControl");
            Property(x => x.Folio).HasColumnName("Folio");
            Property(x => x.IdEconomico).HasColumnName("IdEconomico");
            Property(x => x.NoEconomico).HasColumnName("NoEconomico");
            Property(x => x.FechaCaptura).HasColumnName("FechaCaptura");
            Property(x => x.Horometro).HasColumnName("Horometro");
            Property(x => x.Obra).HasColumnName("Obra");
            Property(x => x.CcOrigen).HasColumnName("CcOrigen");
            Property(x => x.CcDestino).HasColumnName("CcDestino");
            Property(x => x.MarcaMotor).HasColumnName("MarcaMotor");
            Property(x => x.ModeloMotor).HasColumnName("ModeloMotor");
            Property(x => x.SerieMotor).HasColumnName("SerieMotor");
            Property(x => x.CompañiaTraslado).HasColumnName("CompañiaTraslado");
            Property(x => x.VehiculoTraslado).HasColumnName("VehiculoTraslado");
            Property(x => x.OperadorTraslado).HasColumnName("OperadorTraslado");

            Property(x => x.Observaciones).HasColumnName("Observaciones");
            Property(x => x.archivoSetFotografico).HasColumnName("archivoSetFotografico");
            Property(x => x.archivoRehabilitacion).HasColumnName("archivoRehabilitacion");
            Property(x => x.archivoDN).HasColumnName("archivoDN");
            Property(x => x.archivoSOS).HasColumnName("archivoSOS");
            Property(x => x.archivoBitacora).HasColumnName("archivoBitacora");
            Property(x => x.archivoCheckList).HasColumnName("archivoCheckList");
            Property(x => x.archivoVidaAceites).HasColumnName("archivoVidaAceites");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            ToTable("tblM_CatControlCalidad");
        }
    }
}
