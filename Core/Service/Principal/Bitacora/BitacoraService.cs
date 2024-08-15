using Core.DAO.Principal.Bitacoras;
using Core.DTO.Principal.Bitacoras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Principal.Bitacora
{
    public class BitacoraService : IBitacoraDAO
    {
        #region Atributos
        private IBitacoraDAO m_bitacoraDAO;
        #endregion Atributos

        #region Propiedades
        private IBitacoraDAO BitacoraDAO
        {
            get { return m_bitacoraDAO; }
            set { m_bitacoraDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public BitacoraService(IBitacoraDAO BitacoraDAO)
        {
            this.BitacoraDAO = BitacoraDAO;
        }
        #endregion Constructores

        public IList<BitacoraDTO> getBitacora(int Modulo, int RegistroID)
        {
            return BitacoraDAO.getBitacora(Modulo, RegistroID);
        }
    }
}
