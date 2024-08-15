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

namespace Core.DAO.Maquinaria.Captura
{
    public interface IDatosDiariosDAO
    {

        List<ComboDTO> ObtenerAreaCuenta();
        List<resultDatosDiariosDTO> ObtenerCatMaquinas(datosDiariosDTO parametros,int idEmpresa);
        List<tblM_CapturaDatosDiariosMaquinaria> ObtenerCapturaDeDatosDiario(datosDiariosDTO parametros, int idEmpresa);
        bool CapturarDatosDiaros(List<tblM_CapturaDatosDiariosMaquinaria> parametros);
        MemoryStream GenerarExcelDatosDiarios(datosDiariosDTO parametros, int idEmpresa);
        MemoryStream GenerarExcelDatosDiariosEnviandocorreo(datosDiariosDTO parametros, int idEmpresa);
        int ObtenerBotonEnviarExcel(DateTime Fecha);
        bool PermisoBoton(int idUsuario);
        List<ComboDTO> ObtenerGrupo();
        List<ComboDTO> ObtenerModelo(int idGrupo);
        bool guardar_Estatus_Diario(tblM_CatMaquina_EstatusDiario obj, List<tblM_CatMaquina_EstatusDiario_Det> det);
        EstatusDiarioDTO getEstatus_Diario(DateTime fecha,string cc);
        void saveCapturarDatosDiaros(tblM_CatMaquina_EstatusDiario obj, List<tblM_CatMaquina_EstatusDiario_Det> det);
        void sendCapturarDatosDiaros(string cc, DateTime fecha,List<byte[]> archivos);
        Dictionary<string, object> CargarGraficasDashboard(List<string> listaAreaCuenta);
    }
}
