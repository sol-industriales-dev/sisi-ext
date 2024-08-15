using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class InspeccionesObraExcelDTO
    {
        public string descripcion { get; set; }
        public string sistemaReparar { get; set; }
        public string NoRequisicion { get; set; }
        public string NoOrdenCompra { get; set; }
        public string NoFolio { get; set; }
        public string fechaRefacciones { get; set; }
        public string fechaPromRefacciones { get; set; }
        public string fechaRealRefacciones { get; set; }
        public string avance { get; set; }
        public string fechaTerminacion { get; set; }
        public string fechaPromTerminacion { get; set; }
        public string fechaRealTerminacion { get; set; }
        public string costoUSD { get; set; }
        public string costoMN { get; set; }
        public string costoManoObra { get; set; }
        public string granTotalUSD { get; set; }
    }
}
