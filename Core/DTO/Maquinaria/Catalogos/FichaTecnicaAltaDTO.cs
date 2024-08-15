using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Catalogos
{
    public class FichaTecnicaAltaDTO
    {
        public int ProveedorID { get; set; }
        public string Proveedor { get; set; }
        public string EntregaSitio { get; set; }
        public string LugarEntrega { get; set; }
        public string OrdenCompra { get; set; }
        public string CostoEquipo { get; set; }
        public string TipoEquipo { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string NoSerie { get; set; }
        public string Arreglo { get; set; }
        public string MarcaMotor { get; set; }
        public string ModeloMotor { get; set; }
        public string SerieMotor { get; set; }
        public string ArregloMotor { get; set; }
        public string CodicionesUso { get; set; }
        public string Adquisicion { get; set; }
        public string añoEquipo { get; set; }
        public string LugarFabricacion { get; set; }
        public string Pedimento { get; set; }
        public string horometro { get; set; }
        public string Economico { get; set; }

        public decimal CostoRenta { get; set; }
        public decimal UtilizacionHoras { get; set; }
        public int TipoCambio { get; set; }
        public string fechaAdquisicion { get; set; }
        public string LibreAbordo { get; set; }
        public string EconomicoCC { get; set; }
        public int CargoEmpresa { get; set; }
        public string Garantia { get; set; }
        public string Comentario { get; set; }
        public int empresa { get; set; }
        public bool tieneSeguro { get; set; }
        public bool ManualesOperacion { get; set; }
    }
}
