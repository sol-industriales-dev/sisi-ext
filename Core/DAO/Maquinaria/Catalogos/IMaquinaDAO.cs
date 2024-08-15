using Core.DTO;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Reporte;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface IMaquinaDAO
    {
        bool CorteInventarioEnviado(int? tipo);
        bool guardarCorteInventario(tblM_CorteInventarioMaq corte);
        tblM_CatMaquina getEconomicoIDNo(string noEconomico);
        List<string> getListaCorreosInventario(int id);
        string GetNumeroEconomico(int idGrupo, bool renta);
        void Guardar(tblM_CatMaquina obj);
        void NotificarAltaFichaTecnica(tblM_CatMaquina objMaquina, FichaTecnicaAltaDTO setImprimible);

        List<tblM_CatTipoMaquinaria> FillCboTipoMaquinaria(bool estatus);
        List<tblM_CatGrupoMaquinaria> FillCboGrupoMaquinaria(int idTipo);
        List<AutocompletadoDTO> EconomicoDesripcion(string term);
        List<tblM_CatMaquina> FillGridMaquina(MaquinaFiltrosDTO obj);
        List<tblM_CatMaquina> FillGridMaquina(DateTime inicio, DateTime fin, int tipo, int estatus);
        List<tblM_CatAseguradora> FillCboAseguradora(bool estatus);
        List<tblM_CatMarcaEquipo> FillCboMarcasEquipo(int idGrupo);
        List<tblM_CatModeloEquipo> FillCboModeloEquipo(int idMarca);
        List<tblM_CatGrupoMaquinaria> FillCboFiltroGrupoMaquinaria(bool estatus);
        List<tblM_CatMaquina> FillInventarioMaquinaria(MaquinaFiltrosDTO obj);
        tblM_CatGrupoMaquinaria getGrupoMaquina(int p);
        tblM_CatMaquina GetMaquina(int obj, List<tblM_CatMaquina> _lstCatMaquinasDapperDTO = null);
        List<tblM_CatMaquina> getCboMaquinaria(string obj);
        tblM_CatMaquina GetMaquinaByNoEconomico(string obj);
        List<tblM_CatMaquina> FillCboEconomicos(int grupo);
        List<tblM_CatMaquina> getListaMaquinaria(MaquinaFiltrosDTO obj);
        List<tblM_CatMaquina> GetMaquinaByID(int id);
        tblM_CatMaquina EconomicoNotNull(int obj);
        string GetParcialEconomico(int obj);
        List<tblM_CatMaquina> ListaEquiposSolicitud(int grupo);
        List<tblM_CatMaquina> GetAllMaquinas();
        List<tblM_CatPipas> GetAllPipas();
        List<tblM_CatMaquina> getCboMaquinariaFiltro(int obj);
        List<inventarioGeneralDTO> GetInventarioMaquinaria(MaquinaFiltrosDTO obj);
        List<tblM_CatMaquina> ListaEquiposGrupo(List<int> grupo);
        List<tblM_CatModeloEquipo> FillCboModeloEquipoGrupo(int idGrupo);
        List<ListaAnexosDTO> GetListaAnexos(List<string> CCs, int grupo, int Economico, int tipo);
        List<tblM_CatGrupoMaquinaria> GetGrupoMaquinarias();
        List<ComboDTO> fillCboNoEconomicos(string cc);
        List<ComboDTO> fillCboNoEconomicosCC(List<string> cc);
        List<RepCargoNominaCCArreDTO> GetEconomicos(List<string> arrProyectos, string periodoInicial, string periodoFinal);
        string GetProyectosString(List<string> arrProyectos);
        List<NominaCCDTO> GetNominaCCGuardados(string fechaCaptura, string proyecto, int estatus);
        bool existPeriodoNomina(string areaCuenta, DateTime ini, DateTime fin);
        tblM_CapNominaCC getNominaCC(string ac, DateTime ini, DateTime fin);
        tblM_CapNominaCC getNominaCC(int id);
        string getNominaCCProyectos(int id);
        List<string> getNominaCCArrProyectos(int id);
        List<tblM_CapNominaCC_Proyectos> getNominaCCLstProyectos(int id);
        List<tblM_CapNominaCC_Detalles> getNominaCCDet(int id);
        void GuardarCargoNominaCC(List<byte[]> downloadPDF);
        void ActualizarCargoNominaCC(List<byte[]> downloadPDF, tblM_CapNominaCC nomina, List<tblM_CapNominaCC_Detalles> lstDet);
        string GetCCNomina(List<string> arrProyectos);
        bool checkPeriodoCapturado(string ac, DateTime ini, DateTime fin);

        /// <summary>
        /// Obtiene la nómina mensual del area cuenta seleccionada del mes y el año indicado.
        /// </summary>
        /// <param name="areaCuentaArray">Areas cuenta a consultar.</param>
        /// <param name="mes">Mes a consultar.</param>
        /// <param name="año">Año a consultar.</param>
        /// <param name="estatus">Indica si la nómina mensual ya ha sido capturada.</param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerNominaMensualCC(string[] areaCuentaArray, int mes, int año, int estatus);

        /// <summary>
        /// Guarda los cambios hehchos a las nominas de los proyectos seleccionados.
        /// </summary>
        /// <param name="nominasProyectos">Lista de nominas.</param>
        /// <param name="mes">Mes seleeccionado.</param>
        /// <param name="año">Año seleccionado.</param>
        /// <returns></returns>
        Dictionary<string, object> GuardarNominaMensualCC(List<NominaMensualCCDTO> nominasProyectos, int mes, int año);

        /// <summary>
        /// Obtiene todos los datos necesarios para generar un reporte sobre la nomina mensual de alguna AC.
        /// </summary>
        /// <param name="nominaMensualID">Identificador de la nomina mensual.</param>
        /// <returns></returns>
        ReporteCargoMensualNominaCCDTO ObtenerNominaMensualCCReporte(int nominaMensualID);

        bool verificarPermisoEliminarDocumentoEconomico();

        #region CONSULTAS CON DAPPER
        List<tblM_CatMaquina> _lstCatMaquinasDapperDTO(MainContextEnum idEmpresa);
        #endregion
    }
}
