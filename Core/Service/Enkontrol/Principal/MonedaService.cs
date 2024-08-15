using Core.DAO.Enkontrol.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Enkontrol.Principal
{
    public class MonedaService : IMonedaDAO
    {
        #region Atributos
        public IMonedaDAO e_mDAO;
        #endregion
        #region Propiedades
        public IMonedaDAO MDAO
        {
            get { return e_mDAO; }
            set { e_mDAO = value; }
        }
        #endregion
        #region Constructor
        public MonedaService(IMonedaDAO mDAO)
        {
            this.MDAO = mDAO;
        }
        #endregion
        public void guardarTC(int moneda, decimal tc)
        {
            MDAO.guardarTC(moneda, tc);
        }
        public bool isUsuarioCambiarTC()
        {
            return MDAO.isUsuarioCambiarTC();
        }
        public decimal getTcHoy(int moneda)
        {
            return MDAO.getTcHoy(moneda);
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboMonedaHoy()
        {
            return MDAO.FillComboMonedaHoy();
        }
    }
}
