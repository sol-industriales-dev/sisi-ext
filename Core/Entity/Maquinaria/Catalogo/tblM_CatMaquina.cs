using Core.Entity.Maquinaria.Reporte.ActivoFijo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Catalogo
{
    public class tblM_CatMaquina
    {

        public int id { get; set; }
        public string noEconomico { get; set; }
        public string descripcion { get; set; }
        public int grupoMaquinariaID { get; set; }
        public virtual tblM_CatGrupoMaquinaria grupoMaquinaria { get; set; }
        public int modeloEquipoID { get; set; }
        public virtual tblM_CatModeloEquipo modeloEquipo { get; set; }
        public int marcaID { get; set; }
        public virtual tblM_CatMarcaEquipo marca { get; set; }
        public int anio { get; set; }
        public string placas { get; set; }
        public string noSerie { get; set; }
        public int aseguradoraID { get; set; }
        public virtual tblM_CatAseguradora aseguradora { get; set; }
        public string noPoliza { get; set; }
        public int TipoCombustibleID { get; set; }
        public int capacidadTanque { get; set; }
        public int unidadCarga { get; set; }
        public int capacidadCarga { get; set; }
        public int horometroAdquisicion { get; set; }
        public decimal horometroActual { get; set; }
        public int estatus { get; set; }
        public bool renta { get; set; }
        public int tipoEncierro { get; set; }
        public DateTime fechaPoliza { get; set; }
        public DateTime fechaAdquisicion { get; set; }
        public string centro_costos { get; set; }
        public string proveedor { get; set; }
        public string ComentarioStandBy { get; set; }
        public int TipoCaptura { get; set; }
        //  public int trackComponenteID { get; set; }
        //  public virtual tblM_trackComponentes trackComponenteID { get; set; }

        public DateTime fechaEntregaSitio { get; set; }
        public string lugarEntregaProveedor { get; set; }
        public string ordenCompra { get; set; }
        public decimal costoEquipo { get; set; }
        public string numArreglo { get; set; }

        public string marcaMotor { get; set; }
        public string modeloMotor { get; set; }
        public string numSerieMotor { get; set; }
        public string arregloCPL { get; set; }

        public int CondicionUso { get; set; }
        public int tipoAdquisicion { get; set; }

        public int fabricacion { get; set; }
        public string numPedimento { get; set; }
        public decimal CostoRenta { get; set; }
        public decimal UtilizacionHoras { get; set; }
        public int TipoCambio { get; set; }
        public int ProveedorID { get; set; }
        public Nullable<int> TipoBajaID { get; set; }
        public Nullable<int> IdUsuarioBaja { get; set; }
        public Nullable<DateTime> fechaBaja { get; set; }
        public string LibreAbordo { get; set; }
        public decimal kmBaja { get; set; }
        public decimal HorometroBaja { get; set; }
        public string EconomicoCC { get; set; }
        public int CargoEmpresa { get; set; }

        public string Comentario { get; set; }
        public string Garantia { get; set; }

        public bool DepreciacionCapturada { get; set; }
        public bool redireccionamientoVenta { get; set; }
        public int empresa { get; set; }
        public bool? tieneSeguro { get; set; }
        public bool? ManualesOperacion { get; set; }
    }
}
