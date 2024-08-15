using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.DatosDiarios
{
    public class resultDatosDiariosDTO
    {
        public int id { get; set; }
        public string economicoDescripcion { get; set; }
        public string descripcion { get; set; }
        public int grupoMaquinariaID { get; set; }
        public string modeloEquipoID { get; set; }
        public string marcaID { get; set; }
        public int anio { get; set; }
        public string placas { get; set; }
        public string noSerie { get; set; }
        public int aseguradoraID { get; set; }
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


        public int idCatMaquina { get; set; }
        public string Modelo { get; set; }
        public DateTime fechaCapturaMaquinaria { get; set; }
        public string Marca { get; set; }
        public int Economico { get; set; }
        public string NoEconomico { get; set; }
        public string Serie { get; set; }
        public decimal Horometro { get; set; }
        public DateTime? FechaPatioMaquinaria { get; set; }
        public DateTime? FechaTMC { get; set; }
        public DateTime? FechaMaquinaria { get; set; }
        public int Status { get; set; }

        public string opcionStatus { get; set; }
        public int tipoEquipoID { get; set; }
        public int ModeloEQUIPo { get; set; }

    }
}
