using Core.DTO.Contabilidad.Propuesta;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Propuesta
{
    public interface IReservaDAO
    {
        bool guardarReserva(List<ReservaDTO> lst);
        bool guardarReservaDetalle(List<ConcentradoDTO> lst);
        bool guardarReservaNomima(List<tblC_NominaPoliza> lst);
        bool guardarCatReserva(CatReservaDTO obj);
        bool ElimnarReserva(List<int> lst);
        List<tblC_Reserva> getLstReservasCadena(DateTime fecha);
        List<tblC_Reserva> getLstReservasCCR(BusqConcentradoDTO busq);
        List<tblC_Reserva> getLstReservas(BusqReservaDTO busq);
        List<tblC_Reserva> getLstReservas(BusqConcentradoDTO busq);
        List<tblC_Reserva> getLstReservasCC(BusqConcentradoDTO busq);
        List<tblC_Reserva> getLstReservasSinDetalle(BusqConcentradoDTO busq);
        List<tblC_Reserva> getLstOtrasReservas(BusqReservaDTO busq);
        List<tblC_Reserva> getLstReservasAnteriores(BusqReservaDTO busq);
        List<tblC_Reserva> getLstReservasAnteriores(BusqConcentradoDTO busq);
        List<tblC_Reserva> getLstOtrasReservasAnteriores(BusqReservaDTO busq);
        List<tblC_Reserva> getLstReservaCadenasAnteriores(DateTime min, DateTime max);
        List<tblC_Reserva> getLstReservaImpuestoIva(BusqConcentradoDTO busq);
        List<tblC_ReservaDetalle> getLstReservasDetalle(BusqConcentradoDTO busq);
        tblC_CatReserva getCatReservaActiva(int idCatReserva);
        List<tblC_CatReserva> getLstCatReserva();
        List<tblC_CatReserva> getLstCatReservasActivas();
        List<tblC_RelCatReservaCalculo> getRelCatResercaCalActiva();
        List<tblC_RelCatReservaCc> getRelCatReservaCcActivas();
        List<tblC_RelCatReservaTm> getRelCatReservaTmActivas();
        List<tblC_RelCatReservaTp> getRelCatReservaTpActivas();
        tblC_RelCatReservaCalculo getRelCatReservaCalculoActivo(int idCatReserva, int idTipoProrrateo);
        List<tblC_RelCatReservaCc> getRelCatReservaCcActivas(int idCatReserva, int idTipoProrrateo);
        List<tblC_RelCatReservaTm> getRelCatReservaTmActivas(int idCatReserva, int idTipoProrrateo);
        List<tblC_RelCatReservaTp> getRelCatReservaTpActivas(int idCatReserva, int idTipoProrrateo);
        List<string> getLstCCEstimacion();
        Dictionary<string, decimal> getLstPorcentajeImpIva();
        List<ComboDTO> cboCatReserva();
        List<ComboDTO> cboCatCalculoRes();
    }
}
