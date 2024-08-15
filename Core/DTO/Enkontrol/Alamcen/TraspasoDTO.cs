using Core.DTO.Enkontrol.Requisicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class TraspasoDTO
    {
        public int numeroRequisicion { get; set; }
        public int folioInterno { get; set; }
        public string folioInternoString { get; set; }
        public string ccOrigen { get; set; }
        public string ccOrigenDesc { get; set; }
        public int almacenOrigen { get; set; }
        public string almacenOrigenDesc { get; set; }
        public string ccDestino { get; set; }
        public string ccDestinoDesc { get; set; }
        public int almacenDestino { get; set; }
        public string almacenDestinoDesc { get; set; }
        public int insumo { get; set; }
        public string insumoDesc { get; set; }
        public decimal cantidad { get; set; }
        public decimal cantidadTraspasar { get; set; }
        public bool checkBoxAutorizado { get; set; }
        public bool checkBoxRechazado { get; set; }

        public string comentariosGestion { get; set; }

        public List<UbicacionDetalleDTO> listUbicacionMovimiento { get; set; }
    }
}
