using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario.ControlCalidad
{
    public class tblM_CatControlCalidad
    {
        public int id {get;set;}
        public int IdAsignacion {get;set;}
        public int TipoControl {get;set;}
        public string Folio {get;set;}
        public int IdEconomico {get;set;}
        public string NoEconomico {get;set;}
        public DateTime FechaCaptura {get;set;}
        public decimal Horometro {get;set;}
        public string Obra {get;set;}
        public string CcOrigen {get;set;}
        public string CcDestino {get;set;}
        public string MarcaMotor {get;set;}
        public string ModeloMotor {get;set;}
        public string SerieMotor {get;set;}
        public string CompañiaTraslado {get;set;}
        public string VehiculoTraslado {get;set;}
        public string OperadorTraslado { get; set; }
        public string Observaciones { get; set; }
        public string archivoSetFotografico { get; set; }
        public string archivoRehabilitacion { get; set; }
        public string archivoDN { get; set; }
        public string archivoSOS { get; set; }
        public string archivoBitacora { get; set; }
        public string  archivoCheckList { get; set; }
        public string archivoVidaAceites { get; set; }
        public int? usuarioID { get; set; }
    }
}
