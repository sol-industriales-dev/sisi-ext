using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_CapNotaCredito
    {
        public int id { get; set; }
        public string Generador { get; set; }
        public string OC { get; set; }
        public int idEconomico { get; set; }
        public string SerieComponente { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int CausaRemosion { get; set; }
        public decimal HorometroEconomico { get; set; }
        public decimal HorometroComponente { get; set; }
        public decimal MontoPesos { get; set; }
        public decimal MontoDLL { get; set; }
        public decimal AbonoDLL { get; set; }
        public string ClaveCredito { get; set; }
        public string RutaArchivo { get; set; }
        public int Estado { get; set; }
        public DateTime FechaCaptura { get; set; }
        public int idUsuarioModifico { get; set; }
        public string CadenaModifica { get; set; }
        public int EstatusModifica { get; set; }
        public Int32? TipoNC { get; set; }
        public string folio { get; set; }
        public string factura { get; set; }
        public string cc { get; set; }
        public string noAlmacen { get; set; }
        public int numInsumo { get; set; }
        public string descripcionInsumo { get; set; }
        public DateTime? fechaCasco { get; set; }
        public decimal montoTotalOC { get; set; }
    }
}
