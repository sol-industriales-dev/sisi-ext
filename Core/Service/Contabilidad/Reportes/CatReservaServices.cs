using Core.DAO.Contabilidad.Reportes;
using Core.DTO.Contabilidad.Propuesta;
using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Reportes
{
    public class CatReservaServices : ICatReservaDAO
    {
        #region Atributo
        public ICatReservaDAO c_reserva { get; set; }
        #endregion
        #region Propiedad
        public ICatReservaDAO Reserva
        {
            get { return c_reserva; }
            set { c_reserva = value; }
        }
        #endregion
        #region Constructor
        public CatReservaServices(ICatReservaDAO reserva)
        {
            this.Reserva = reserva;
        }
        #endregion
        public tblC_CatReserva Guardar(tblC_CatReserva obj)
        {
            return this.Reserva.Guardar(obj);
        }
        public List<tblC_CatReserva> GetReservaSemana(int semana)
        {
            return this.Reserva.GetReservaSemana(semana);
        }
        public List<tblC_CatReserva> GetReserva()
        {
            return this.Reserva.GetReserva();
        }
        public List<tblC_CatReserva> GetReserva(BusqConcentradoDTO busq)
        {
            return Reserva.GetReserva(busq);
        }
    }
}
