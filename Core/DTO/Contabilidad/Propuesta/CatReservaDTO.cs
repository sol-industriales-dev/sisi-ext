using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class CatReservaDTO
    {
        public tblC_CatReserva catReserva { get; set; }
        public List<tblC_RelCatReservaCalculo> lstCalc { get; set; }
        public List<tblC_RelCatReservaCc> lstCc { get; set; }
        public List<tblC_RelCatReservaTm> lstTm { get; set; }
        public List<tblC_RelCatReservaTp> lstTp { get; set; }
    }
}
