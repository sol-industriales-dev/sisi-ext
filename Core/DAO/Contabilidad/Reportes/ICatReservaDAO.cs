using Core.DTO.Contabilidad.Propuesta;
using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Reportes
{
    public interface ICatReservaDAO
    {
        tblC_CatReserva Guardar(tblC_CatReserva obj);
        List<tblC_CatReserva> GetReservaSemana(int semana);
        List<tblC_CatReserva> GetReserva();
        List<tblC_CatReserva> GetReserva(BusqConcentradoDTO busq);
    }
}
