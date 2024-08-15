using Core.DAO.Maquinaria.Captura.HorasHombre;
using Core.DTO.Maquinaria.Captura.OT;
using Core.DTO.Maquinaria.Captura.OT.rptConcentradoHH;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas.HorasHombre
{
    public class CapHorasHombreService : ICapHorasHombreDAO
    {
        #region Atributos
        private ICapHorasHombreDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private ICapHorasHombreDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public CapHorasHombreService(ICapHorasHombreDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public List<CapDistribucionHHDTO> loadDataCapHorasHombre(string cc, DateTime fecha, int turno)
        {
            return interfazDAO.loadDataCapHorasHombre(cc, fecha, turno);
        }

        public List<tblM_CatSubCategoriasHH> getSubCategorias(List<int> listCategorias)
        {
            return interfazDAO.getSubCategorias(listCategorias);
        }
        public Dictionary<string, object> getSubCategorias()
        {
            return interfazDAO.getSubCategorias();
        }
        public void guardarInformacion(List<tblM_CapHorasHombre> array)
        {
            interfazDAO.guardarInformacion(array);
        }

        public List<GeneralConcentradoHHDTO> getConcentradoGeneral(List<string> cc, DateTime fechaInicio, DateTime fechaFin, List<int> listaCategorias, List<int> subCategoriasHH)
        {
            return interfazDAO.getConcentradoGeneral(cc, fechaInicio, fechaFin, listaCategorias, subCategoriasHH);
        }

        public List<DistribucionHHDTO> getDistribucionGeneral(List<string> cc, DateTime fechaInicio, DateTime fechaFin, List<int> listaCategorias, List<int> subCategoriasHH)
        {
            return interfazDAO.getDistribucionGeneral(cc, fechaInicio, fechaFin, listaCategorias, subCategoriasHH);
        }
        public List<ComboDTO> fillCboPuestos()
        {
            return interfazDAO.fillCboPuestos();

        }
        public List<ComboDTO> getNombreEmpleado(string term, int puesto)
        {

            return interfazDAO.getNombreEmpleado(term, puesto);
        }
        public ComboDTO searchNumEmpleado(string term, int puesto,int numEmpleado)
        {
            return interfazDAO.searchNumEmpleado(term, puesto, numEmpleado);
        }
        
        public List<DistribucionHHPersonalDTO> DetalleDistribucionGeneral(List<string> cc, DateTime fechaInicio, DateTime fechaFin, int puestoID)
        {
            return interfazDAO.DetalleDistribucionGeneral(cc, fechaInicio, fechaFin, puestoID);
        }

        public List<EmpleadoConcentradoGeneralDTO> getInfoByEmpleado(List<string> cc, DateTime fechaInicio, DateTime fechaFin, int numEmpleado)
        {
            return interfazDAO.getInfoByEmpleado(cc, fechaInicio, fechaFin, numEmpleado);
        }
        public PuestosDTO getinfoEmpleadoGeneral(int numEmpleado)
        {
            return interfazDAO.getinfoEmpleadoGeneral(numEmpleado);
        }
        public List<tblM_CatCategoriasHH> getCategorias()
        {
            return interfazDAO.getCategorias();
        }

        public List<rptUtilizacionDTO> getDetalleConcentradoGeneral(List<string> cc, DateTime fechaInicio, DateTime fechaFin, int puestoID)
        {
            return interfazDAO.getDetalleConcentradoGeneral(cc, fechaInicio, fechaFin, puestoID);

        }

        public GraficaParetoDTO getParetoCategorias(List<string> ccs, DateTime fechaInicio, DateTime fechaFin, List<int> listaCategorias, List<int> listaSubCategoria)
        {
            return interfazDAO.getParetoCategorias(ccs, fechaInicio, fechaFin, listaCategorias, listaSubCategoria);
        }
        public List<ComboDTO> getListaPuestos()
        {
            return interfazDAO.getListaPuestos();//(ccs, fechaInicio, fechaFin, listaCategorias, listaSubCategoria);
        }

        public List<ComboDTO> getListaPersonalPuestos(List<int> puestos, List<string> ccs)
        {
            return interfazDAO.getListaPersonalPuestos(puestos, ccs);
        }

        public List<DistribucionHHPersonalDTO> rptGeneralDistribucion(List<string> cc, DateTime fechaInicio, DateTime fechaFin, List<int> puestos, List<int> empleados)
        {
            return interfazDAO.rptGeneralDistribucion(cc, fechaInicio, fechaFin, puestos, empleados);

        }
        public void setRptMaquinariaHorasHombre(List<int> economicos, DateTime fechaInicio, DateTime fechaFin, List<int> Puestos, List<int> Empleados, string cc)
        {
            interfazDAO.setRptMaquinariaHorasHombre(economicos, fechaInicio, fechaFin, Puestos, Empleados, cc);
        }

        public List<DistribucionHHPersonalDTO> rptGeneralCCPorPuesto(List<string> cc, DateTime fechaInicio, DateTime fechaFin, List<int> puestoID)
        {
            return interfazDAO.rptGeneralCCPorPuesto(cc, fechaInicio, fechaFin, puestoID);
        }

        public List<newCapHorasHombreDTO> loadTblHorasHombre(string cc, int clave_depto, DateTime fecha, int turno, int usuarioActual, int puesto, int empleado)
        {
            return interfazDAO.loadTblHorasHombre(cc, clave_depto, fecha, turno, usuarioActual, puesto, empleado);
        }

        public List<ComboDTO> fillCboCC()
        {
            return interfazDAO.fillCboCC();
        }

        public Dictionary<string, object> FillComboDepartamentos(string cc)
        {
            return interfazDAO.FillComboDepartamentos(cc);
        }
    }
}
