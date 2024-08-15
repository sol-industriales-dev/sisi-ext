using Core.DAO.Contabilidad.Propuesta;
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

namespace Core.Service.Contabilidad.Propuesta
{
    public class ReservaServices : IReservaDAO
    {
        #region Atributos
        private IReservaDAO p_reservaDAO;
        #endregion
        #region Propiedades
        public IReservaDAO ReservaDAO
        {
            get { return p_reservaDAO; }
            set { p_reservaDAO = value; }
        }
        #endregion
        #region Contructores
        public ReservaServices(IReservaDAO reservaDAO)
        {
            this.ReservaDAO = reservaDAO;
        }
        #endregion
        /// <summary>
        /// Guarda o actualiza las reserva
        /// </summary>
        /// <param name="lst">Reservas a guardar</param>
        /// <returns>Bandera de realizado</returns>
        public bool guardarReserva(List<ReservaDTO> lst)
        {
            return ReservaDAO.guardarReserva(lst);
        }
        /// <summary>
        /// Guarda o actualiza los detalles de ciertas reservas
        /// </summary>
        /// <param name="lst">Detalles de reservas a guardar</param>
        /// <returns>Bandera de realizado</returns>
        public bool guardarReservaDetalle(List<ConcentradoDTO> lst)
        {
            return ReservaDAO.guardarReservaDetalle(lst);
        }
        /// <summary>
        /// Guardar o actualizar reservas automáticamente desde las nóminas
        /// </summary>
        /// <param name="lst">Nominas</param>
        /// <returns>Bandera de realizado</returns>
        public bool guardarReservaNomima(List<tblC_NominaPoliza> lst)
        {
            return ReservaDAO.guardarReservaNomima(lst);
        }
        public bool guardarCatReserva(CatReservaDTO obj)
        {
            return ReservaDAO.guardarCatReserva(obj);
        }
        /// <summary>
        /// Desactiva la reservas selecionadas
        /// </summary>
        /// <param name="lst">Reservas a desactivar</param>
        /// <returns>Bandera de desactivado</returns>
        public bool ElimnarReserva(List<int> lst)
        {
            return ReservaDAO.ElimnarReserva(lst);
        }
        /// <summary>
        /// Consulta las reservas de cadena de la semana
        /// </summary>
        /// <param name="fecha">fecha de la consulta</param>
        /// <returns>Reservas de cadena</returns>
        public List<tblC_Reserva> getLstReservasCadena(DateTime fecha)
        {
            return ReservaDAO.getLstReservasCadena(fecha);
        }
        /// <summary>
        /// Consulta de reservas con los CC-R
        /// </summary>
        /// <param name="busq">Busqueda por concentrado</param>
        /// <returns>Reservas</returns>
        public List<tblC_Reserva> getLstReservasCCR(BusqConcentradoDTO busq)
        {
            return ReservaDAO.getLstReservasCCR(busq);
        }
        /// <summary>
        /// Consulta de reservas de la semana de su tipo
        /// </summary>
        /// <param name="busq">Busqueda por reserva</param>
        /// <returns>Reservas</returns>
        public List<tblC_Reserva> getLstReservas(BusqReservaDTO busq)
        {
            return ReservaDAO.getLstReservas(busq);
        }
        /// <summary>
        /// Consulta de reservas
        /// </summary>
        /// <param name="busq">Busqueda por concentrado</param>
        /// <returns>Reservas</returns>
        public List<tblC_Reserva> getLstReservas(BusqConcentradoDTO busq)
        {
            return ReservaDAO.getLstReservas(busq);
        }
        public List<tblC_Reserva> getLstReservasCC(BusqConcentradoDTO busq)
        {
            return ReservaDAO.getLstReservasCC(busq);
        }
        /// <summary>
        /// Consulta de reservas sin detalles relacionads
        /// </summary>
        /// <param name="busq">Busqueda por concentrado</param>
        /// <returns>Reservas</returns>
        public List<tblC_Reserva> getLstReservasSinDetalle(BusqConcentradoDTO busq)
        {
            return ReservaDAO.getLstReservasSinDetalle(busq);
        }
        /// <summary>
        /// Consulta de reservas de semanas anteriores
        /// </summary>
        /// <param name="busq">Busqueda de reserva</param>
        /// <returns>Lista de reservas</returns>
        public List<tblC_Reserva> getLstReservasAnteriores(BusqReservaDTO busq)
        {
            return ReservaDAO.getLstReservasAnteriores(busq);
        }
        public List<tblC_Reserva> getLstReservasAnteriores(BusqConcentradoDTO busq)
        {
            return ReservaDAO.getLstReservasAnteriores(busq);
        }
        /// <summary>
        /// Consulta de reservas de la semana, excepto las de su tipo
        /// </summary>
        /// <param name="busq">Busqueda de reserva</param>
        /// <returns>Lista de reservas</returns>
        public List<tblC_Reserva> getLstOtrasReservas(BusqReservaDTO busq)
        {
            return ReservaDAO.getLstOtrasReservas(busq);
        }
        public List<tblC_Reserva> getLstOtrasReservasAnteriores(BusqReservaDTO busq)
        {
            return ReservaDAO.getLstOtrasReservasAnteriores(busq);
        }
        public List<tblC_Reserva> getLstReservaCadenasAnteriores(DateTime min, DateTime max)
        {
            return ReservaDAO.getLstReservaCadenasAnteriores(min, max);
        }
        /// <summary>
        /// Consulta de Reservas de impuestos de iva
        /// </summary>
        /// <param name="busq">Busqueda de reservas</param>
        /// <returns>reservas de impuestos de iva</returns>
        public List<tblC_Reserva> getLstReservaImpuestoIva(BusqConcentradoDTO busq)
        {
            return ReservaDAO.getLstReservaImpuestoIva(busq);
        }
        public tblC_CatReserva getCatReservaActiva(int idCatReserva)
        {
            return ReservaDAO.getCatReservaActiva(idCatReserva);
        }
        public List<tblC_ReservaDetalle> getLstReservasDetalle(BusqConcentradoDTO busq)
        {
            return ReservaDAO.getLstReservasDetalle(busq);
        }
        public List<tblC_CatReserva> getLstCatReserva()
        {
            return ReservaDAO.getLstCatReserva();
        }
        public List<tblC_CatReserva> getLstCatReservasActivas()
        {
            return ReservaDAO.getLstCatReservasActivas();
        }
        public List<tblC_RelCatReservaCalculo> getRelCatResercaCalActiva()
        {
            return ReservaDAO.getRelCatResercaCalActiva();
        }
        public List<tblC_RelCatReservaCc> getRelCatReservaCcActivas()
        {
            return ReservaDAO.getRelCatReservaCcActivas();
        }
        public List<tblC_RelCatReservaTm> getRelCatReservaTmActivas()
        {
            return ReservaDAO.getRelCatReservaTmActivas();
        }
        public List<tblC_RelCatReservaTp> getRelCatReservaTpActivas()
        {
            return ReservaDAO.getRelCatReservaTpActivas();
        }
        public tblC_RelCatReservaCalculo getRelCatReservaCalculoActivo(int idCatReserva, int idTipoProrrateo)
        {
            return ReservaDAO.getRelCatReservaCalculoActivo(idCatReserva, idTipoProrrateo);
        }
        public List<tblC_RelCatReservaCc> getRelCatReservaCcActivas(int idCatReserva, int idTipoProrrateo)
        {
            return ReservaDAO.getRelCatReservaCcActivas(idCatReserva, idTipoProrrateo);
        }
        public List<tblC_RelCatReservaTm> getRelCatReservaTmActivas(int idCatReserva, int idTipoProrrateo)
        {
            return ReservaDAO.getRelCatReservaTmActivas(idCatReserva, idTipoProrrateo);
        }
        public List<tblC_RelCatReservaTp> getRelCatReservaTpActivas(int idCatReserva, int idTipoProrrateo)
        {
            return ReservaDAO.getRelCatReservaTpActivas(idCatReserva, idTipoProrrateo);
        }
        public List<string> getLstCCEstimacion()
        {
            return ReservaDAO.getLstCCEstimacion();
        }
        /// <summary>
        /// Consulta los úlimos porcentajes activos de los cc
        /// </summary>
        /// <returns>porcentajes de los cc</returns>
        public Dictionary<string, decimal> getLstPorcentajeImpIva()
        {
            return ReservaDAO.getLstPorcentajeImpIva();
        }
        public List<ComboDTO> cboCatReserva()
        {
            return ReservaDAO.cboCatReserva();
        }
        public List<ComboDTO> cboCatCalculoRes()
        {
            return ReservaDAO.cboCatCalculoRes();
        }
    }
}
