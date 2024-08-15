using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class RespuestaResguardoVehiculosServices : IRespuestaResguardoVehiculosDAO
    {

        #region Atributos
        private IRespuestaResguardoVehiculosDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IRespuestaResguardoVehiculosDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }

        public RespuestaResguardoVehiculosServices(IRespuestaResguardoVehiculosDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        #region Constructores
        #endregion
        public void Guardar(List<tblM_RespuestaResguardoVehiculos> obj)
        {
            interfazDAO.Guardar(obj);

        }
        public List<RespuestasDTO> GetResguardoRespuestas(int id)
        {
            return interfazDAO.GetResguardoRespuestas(id);
        }

        public List<DocumentoGridDTO> getDocumentos(int id)
        {
            return interfazDAO.getDocumentos(id);
        }

        public List<RespuestasDTO> GetResguardoRespuestasLiberado(int id)
        {
            return interfazDAO.GetResguardoRespuestasLiberado(id);
        }
    }
}
