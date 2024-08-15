using Core.DAO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura.DatosDiarios;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class DatosDiariosService : IDatosDiariosDAO
    {
        private IDatosDiariosDAO m_IDatosDiariosDAO;
        public IDatosDiariosDAO DatosDiariosDAO
        {
            get { return m_IDatosDiariosDAO; }
            set { m_IDatosDiariosDAO = value; }
        }
        public DatosDiariosService(IDatosDiariosDAO datosDiariosDAO)
        {
            this.DatosDiariosDAO = datosDiariosDAO;
        }


        public List<ComboDTO> ObtenerAreaCuenta()
        {
            return DatosDiariosDAO.ObtenerAreaCuenta();
        }
        public List<resultDatosDiariosDTO> ObtenerCatMaquinas(datosDiariosDTO parametros, int idEmpresa)
        {
            return DatosDiariosDAO.ObtenerCatMaquinas(parametros, idEmpresa);
        }
        public List<tblM_CapturaDatosDiariosMaquinaria> ObtenerCapturaDeDatosDiario(datosDiariosDTO parametros, int idEmpresa)
        {
            return DatosDiariosDAO.ObtenerCapturaDeDatosDiario(parametros, idEmpresa);
        }
        public bool CapturarDatosDiaros(List<tblM_CapturaDatosDiariosMaquinaria> parametros)
        {
            return DatosDiariosDAO.CapturarDatosDiaros(parametros);
        }
        public MemoryStream GenerarExcelDatosDiarios(datosDiariosDTO parametros, int idEmpresa)
        {
            return DatosDiariosDAO.GenerarExcelDatosDiarios(parametros, idEmpresa);
        }
        public MemoryStream GenerarExcelDatosDiariosEnviandocorreo(datosDiariosDTO parametros, int idEmpresa)
        {
            return DatosDiariosDAO.GenerarExcelDatosDiariosEnviandocorreo(parametros, idEmpresa);
        }
        public int ObtenerBotonEnviarExcel(DateTime Fecha)
        {
            return DatosDiariosDAO.ObtenerBotonEnviarExcel(Fecha);
        }
        public bool PermisoBoton(int idUsuario)
        {
            return DatosDiariosDAO.PermisoBoton(idUsuario);
        }
        public List<ComboDTO> ObtenerGrupo()
        {
            return DatosDiariosDAO.ObtenerGrupo();
        }
        public List<ComboDTO> ObtenerModelo(int idGrupo)
        {
            return DatosDiariosDAO.ObtenerModelo(idGrupo);
        }
        public bool guardar_Estatus_Diario(tblM_CatMaquina_EstatusDiario obj, List<tblM_CatMaquina_EstatusDiario_Det> det)
        {
            return DatosDiariosDAO.guardar_Estatus_Diario(obj,det);
        }
        public EstatusDiarioDTO getEstatus_Diario(DateTime fecha, string cc)
        {
            return DatosDiariosDAO.getEstatus_Diario(fecha,cc);
        }
        public void saveCapturarDatosDiaros(tblM_CatMaquina_EstatusDiario obj, List<tblM_CatMaquina_EstatusDiario_Det> det)
        {
            DatosDiariosDAO.saveCapturarDatosDiaros(obj, det);
        }
        public void sendCapturarDatosDiaros(string cc, DateTime fecha, List<byte[]> archivos)
        {
            DatosDiariosDAO.sendCapturarDatosDiaros(cc, fecha, archivos);
        }

        public Dictionary<string, object> CargarGraficasDashboard(List<string> listaAreaCuenta)
        {
            return DatosDiariosDAO.CargarGraficasDashboard(listaAreaCuenta);
        }
    }
}
