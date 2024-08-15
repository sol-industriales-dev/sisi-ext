using Core.DAO.Maquinaria.SOS;
using Core.DTO.Maquinaria.SOS;
using Core.Entity.Maquinaria.SOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.SOS
{
    public class MinadoServices : IMinadoDAO
    {
        #region Atributos
        private IMinadoDAO m_MinadoDAO;
        #endregion
        #region Propiedades
        public IMinadoDAO MinadoDAO
        {
            get { return m_MinadoDAO; }
            set { m_MinadoDAO = value; }
        }
        #endregion
        #region Constructores
        public MinadoServices(IMinadoDAO minadoDAO)
        {
            this.MinadoDAO = minadoDAO;
        }
        #endregion

        public void Guardar(List<MinadoEntity> obj)
        {
            MinadoDAO.Guardar(obj);
        }
        public List<CCMuestrasDTO> cboFiltroLugar()
        {
            return MinadoDAO.cboFiltroLugar();
        }
        public List<CCMuestrasDTO> cboComponente()
        {
            return MinadoDAO.cboComponente();
        }
        public List<CCMuestrasDTO> cboFiltroModelo(string lugar)
        {
            return MinadoDAO.cboFiltroModelo(lugar);
        }

        public List<MuestrasGeneralesDTO> muestrasGenerales(string lugar, DateTime fechaini, DateTime fechafin)
        {
            return MinadoDAO.muestrasGenerales(lugar, fechaini, fechafin);
        }
        public List<MaquinaDTO> cboFiltroMaquinaria(string lugar)
        {
            return MinadoDAO.cboFiltroMaquinaria(lugar);
        }
        //
        public List<indicaroresDTO> detallesMuestras(List<MuestrasElementosDTO> resultado, string elemento)
        {
            return MinadoDAO.detallesMuestras(resultado, elemento);
        }
        public List<MuestrasElementosDTO> detalleCompleto(string lugar, string componente, string unitid, string modelo, string elemento, DateTime fechaini, DateTime fechafin)
        {
            return MinadoDAO.detalleCompleto(lugar, componente, unitid, modelo, elemento, fechaini, fechafin);
        }

        public List<MuestrasElementosDTO> detalleCompletoLista(List<string> lugar, string componente, string unitid, string modelo, string elemento, DateTime fechaini, DateTime fechafin)
        {
            return MinadoDAO.detalleCompletoLista(lugar, componente, unitid, modelo, elemento, fechaini, fechafin);
        }


        //detalleCompletoLista

        public List<MuestrasElementosDTO> detalleGeneralMuestras(string lugar, DateTime fechaini, DateTime fechafin, string indicador)
        {
            return MinadoDAO.detalleGeneralMuestras(lugar, fechaini, fechafin, indicador);
        }

        public List<MuestrasElementosDTO> detalleGeneralMuestrasList(List<string> lugar, DateTime fechaini, DateTime fechafin, string indicador)
        {
            return MinadoDAO.detalleGeneralMuestrasList(lugar, fechaini, fechafin, indicador);
        }

        public List<MuestrasGeneralesDTO> muestrasGeneralesLists(List<string> lugar, DateTime fechaini, DateTime fechafin)
        {
            return MinadoDAO.muestrasGeneralesLists(lugar, fechaini, fechafin);
        }

        public List<MaquinaDTO> cboFiltroMaquinariaXlista(List<string> lugar)
        {
            return MinadoDAO.cboFiltroMaquinariaXlista(lugar);
        }
    }
}
