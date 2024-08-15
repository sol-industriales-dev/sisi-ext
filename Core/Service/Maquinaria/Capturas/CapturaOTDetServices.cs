using Core.DAO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Reporte;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class CapturaOTDetServices : ICapturaOTDetDAO
    {
        #region Atributos
        private ICapturaOTDetDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private ICapturaOTDetDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public CapturaOTDetServices(ICapturaOTDetDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores
        public void Guardar(List<tblM_DetOrdenTrabajo> obj, int idOT)
        {
            interfazDAO.Guardar(obj, idOT);
        }

        public List<tblM_DetOrdenTrabajo> getListaOTDet(int id)
        {
            return interfazDAO.getListaOTDet(id);
        }

        public tblRH_CatEmpleados getCatEmpleados(string term)
        {
            return interfazDAO.getCatEmpleados(term);
        }

        public void delete(int id)
        {
            interfazDAO.delete(id);
        }

        public List<RepGastosMaquinariaGrid> GetCostosHoraHombre(int EconomicoID, DateTime FI, DateTime FF)
        {
            return interfazDAO.GetCostosHoraHombre(EconomicoID, FI, FF);
        }

        public List<RepGastosMaquinariaGrid> FillMotivosParo(RepGastosFiltrosDTO obj)
        {
            return interfazDAO.FillMotivosParo(obj);
        }
        public List<RepGastosMaquinariaGrid> FillUsuario(RepGastosFiltrosDTO obj)
        {
            return interfazDAO.FillUsuario(obj);
        }
        public byte[] obtenerImagen(int id, int tipoEvidencia)
        {
            return interfazDAO.obtenerImagen(id, tipoEvidencia);
        }
        public List<byte[]> obtenerImagenLista(int id, int tipoEvidencia)
        {
            return interfazDAO.obtenerImagenLista(id, tipoEvidencia);
        }
    }
}
