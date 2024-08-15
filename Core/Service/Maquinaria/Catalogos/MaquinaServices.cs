using Core.DAO.Maquinaria.Catalogos;
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

namespace Core.Service.Maquinaria.Catalogos
{
    public class MaquinaServices : IMaquinaDAO
    {
        #region Atributos
        private IMaquinaDAO m_maquinaDAO;
        #endregion
        #region Propiedades
        public IMaquinaDAO MaquinaDAO
        {
            get { return m_maquinaDAO; }
            set { m_maquinaDAO = value; }
        }
        #endregion
        #region Constructores
        public MaquinaServices(IMaquinaDAO maquinaDAO)
        {
            this.MaquinaDAO = maquinaDAO;
        }
        #endregion

        public void Guardar(tblM_CatMaquina obj)
        {
            MaquinaDAO.Guardar(obj);
        }

        public void NotificarAltaFichaTecnica(tblM_CatMaquina objMaquina, FichaTecnicaAltaDTO setImprimible)
        {
            MaquinaDAO.NotificarAltaFichaTecnica(objMaquina, setImprimible);
        }

        public bool CorteInventarioEnviado(int? tipo)
        {
            return MaquinaDAO.CorteInventarioEnviado(tipo);
        }

        public bool guardarCorteInventario(tblM_CorteInventarioMaq corte)
        {
            return MaquinaDAO.guardarCorteInventario(corte);
        }

        public List<string> getListaCorreosInventario(int obj)
        {
            return MaquinaDAO.getListaCorreosInventario(obj);
        }

        public List<tblM_CatTipoMaquinaria> FillCboTipoMaquinaria(bool estatus)
        {
            return MaquinaDAO.FillCboTipoMaquinaria(estatus);
        }
        public List<tblM_CatGrupoMaquinaria> FillCboGrupoMaquinaria(int idTipo)
        {
            return MaquinaDAO.FillCboGrupoMaquinaria(idTipo);
        }

        public List<tblM_CatMaquina> FillGridMaquina(MaquinaFiltrosDTO obj)
        {
            return MaquinaDAO.FillGridMaquina(obj);
        }

        public List<tblM_CatMaquina> FillGridMaquina(DateTime inicio, DateTime fin, int tipo, int estatus)
        {
            return MaquinaDAO.FillGridMaquina(inicio, fin, tipo, estatus);
        }
        public List<tblM_CatMaquina> FillInventarioMaquinaria(MaquinaFiltrosDTO obj)
        {
            return MaquinaDAO.FillInventarioMaquinaria(obj);
        }
        public List<AutocompletadoDTO> EconomicoDesripcion(string term)
        {
            return MaquinaDAO.EconomicoDesripcion(term);
        }

        public List<tblM_CatAseguradora> FillCboAseguradora(bool estatus)
        {
            return MaquinaDAO.FillCboAseguradora(estatus);
        }
        public List<tblM_CatMarcaEquipo> FillCboMarcasEquipo(int idGrupo)
        {
            return MaquinaDAO.FillCboMarcasEquipo(idGrupo);
        }
        public List<tblM_CatModeloEquipo> FillCboModeloEquipo(int idMarca)
        {
            return MaquinaDAO.FillCboModeloEquipo(idMarca);
        }
        public List<tblM_CatGrupoMaquinaria> FillCboFiltroGrupoMaquinaria(bool estatus)
        {
            return MaquinaDAO.FillCboFiltroGrupoMaquinaria(estatus);
        }

        public tblM_CatGrupoMaquinaria getGrupoMaquina(int idGrupo)
        {
            return MaquinaDAO.getGrupoMaquina(idGrupo);
        }

        public tblM_CatMaquina GetMaquina(int obj, List<tblM_CatMaquina> _lstCatMaquinasDapperDTO = null)
        {
            return MaquinaDAO.GetMaquina(obj, _lstCatMaquinasDapperDTO);
        }

        public List<tblM_CatMaquina> getCboMaquinaria(string obj)
        {
            return MaquinaDAO.getCboMaquinaria(obj);
        }

        public tblM_CatMaquina GetMaquinaByNoEconomico(string obj)
        {
            return MaquinaDAO.GetMaquinaByNoEconomico(obj);
        }

        public List<tblM_CatMaquina> FillCboEconomicos(int grupo)
        {
            return MaquinaDAO.FillCboEconomicos(grupo);
        }

        public List<tblM_CatMaquina> getListaMaquinaria(MaquinaFiltrosDTO obj)
        {
            return MaquinaDAO.getListaMaquinaria(obj);
        }

        public List<tblM_CatMaquina> GetMaquinaByID(int id)
        {
            return MaquinaDAO.GetMaquinaByID(id);
        }

        public tblM_CatMaquina EconomicoNotNull(int obj)
        {
            return MaquinaDAO.EconomicoNotNull(obj);
        }

        public string GetParcialEconomico(int obj)
        {
            return MaquinaDAO.GetParcialEconomico(obj);
        }

        public string GetNumeroEconomico(int idGrupo, bool renta)
        {
            return MaquinaDAO.GetNumeroEconomico(idGrupo, renta);
        }

        public List<tblM_CatMaquina> ListaEquiposSolicitud(int grupo)
        {
            return MaquinaDAO.ListaEquiposSolicitud(grupo);
        }

        public List<tblM_CatMaquina> getCboMaquinariaFiltro(int obj)
        {
            return MaquinaDAO.getCboMaquinariaFiltro(obj);
        }
        public List<tblM_CatMaquina> GetAllMaquinas()
        {
            return MaquinaDAO.GetAllMaquinas();
        }
        public List<tblM_CatPipas> GetAllPipas()
        {
            return MaquinaDAO.GetAllPipas();
        }
        public List<inventarioGeneralDTO> GetInventarioMaquinaria(MaquinaFiltrosDTO obj)
        {
            return MaquinaDAO.GetInventarioMaquinaria(obj);
        }

        public List<tblM_CatMaquina> ListaEquiposGrupo(List<int> grupo)
        {
            return MaquinaDAO.ListaEquiposGrupo(grupo);
        }

        public List<tblM_CatModeloEquipo> FillCboModeloEquipoGrupo(int idGrupo)
        {
            return MaquinaDAO.FillCboModeloEquipoGrupo(idGrupo);
        }
        public List<ListaAnexosDTO> GetListaAnexos(List<string> CCs, int grupo, int Economico, int tipo)
        {
            return MaquinaDAO.GetListaAnexos(CCs, grupo, Economico, tipo);
        }

        public List<tblM_CatGrupoMaquinaria> GetGrupoMaquinarias()
        {
            return MaquinaDAO.GetGrupoMaquinarias();
        }

        public List<ComboDTO> fillCboNoEconomicos(string cc)
        {
            return MaquinaDAO.fillCboNoEconomicos(cc);
        }

        public List<ComboDTO> fillCboNoEconomicosCC(List<string> cc)
        {
            return MaquinaDAO.fillCboNoEconomicosCC(cc);
        }

        public List<RepCargoNominaCCArreDTO> GetEconomicos(List<string> arrProyectos, string periodoInicial, string periodoFinal)
        {
            return MaquinaDAO.GetEconomicos(arrProyectos, periodoInicial, periodoFinal);
        }

        public tblM_CatMaquina getEconomicoIDNo(string noEconomico)
        {
            return MaquinaDAO.getEconomicoIDNo(noEconomico);

        }

        public string GetProyectosString(List<string> arrProyectos)
        {
            return MaquinaDAO.GetProyectosString(arrProyectos);
        }

        public List<NominaCCDTO> GetNominaCCGuardados(string fechaCaptura, string proyecto, int estatus)
        {
            return MaquinaDAO.GetNominaCCGuardados(fechaCaptura, proyecto, estatus);
        }
        public bool existPeriodoNomina(string ac, DateTime ini, DateTime fin)
        {
            return MaquinaDAO.existPeriodoNomina(ac, ini, fin);
        }
        public tblM_CapNominaCC getNominaCC(string ac, DateTime ini, DateTime fin)
        {
            return MaquinaDAO.getNominaCC(ac, ini, fin);
        }
        public tblM_CapNominaCC getNominaCC(int id)
        {
            return MaquinaDAO.getNominaCC(id);
        }
        public string getNominaCCProyectos(int id)
        {
            return MaquinaDAO.getNominaCCProyectos(id);
        }
        public List<string> getNominaCCArrProyectos(int id)
        {
            return MaquinaDAO.getNominaCCArrProyectos(id);
        }
        public List<tblM_CapNominaCC_Proyectos> getNominaCCLstProyectos(int id)
        {
            return MaquinaDAO.getNominaCCLstProyectos(id);
        }
        public List<tblM_CapNominaCC_Detalles> getNominaCCDet(int id)
        {
            return MaquinaDAO.getNominaCCDet(id);
        }
        public void GuardarCargoNominaCC(List<byte[]> downloadPDF)
        {
            MaquinaDAO.GuardarCargoNominaCC(downloadPDF);
        }
        public void ActualizarCargoNominaCC(List<byte[]> downloadPDF, tblM_CapNominaCC nomina, List<tblM_CapNominaCC_Detalles> lstDet)
        {
            MaquinaDAO.ActualizarCargoNominaCC(downloadPDF, nomina, lstDet);
        }
        public string GetCCNomina(List<string> arrProyectos)
        {
            return MaquinaDAO.GetCCNomina(arrProyectos);
        }
        public bool checkPeriodoCapturado(string ac, DateTime ini, DateTime fin)
        {
            return MaquinaDAO.checkPeriodoCapturado(ac, ini, fin);
        }

        public Dictionary<string, object> ObtenerNominaMensualCC(string[] areaCuentaArray, int mes, int año, int estatus)
        {
            return MaquinaDAO.ObtenerNominaMensualCC(areaCuentaArray, mes, año, estatus);
        }

        public Dictionary<string, object> GuardarNominaMensualCC(List<NominaMensualCCDTO> nominasProyectos, int mes, int año)
        {
            return MaquinaDAO.GuardarNominaMensualCC(nominasProyectos, mes, año);
        }


        public ReporteCargoMensualNominaCCDTO ObtenerNominaMensualCCReporte(int nominaMensualID)
        {
            return MaquinaDAO.ObtenerNominaMensualCCReporte(nominaMensualID);
        }

        public bool verificarPermisoEliminarDocumentoEconomico()
        {
            return MaquinaDAO.verificarPermisoEliminarDocumentoEconomico();
        }

        #region CONSULTAS CON DAPPER
        public List<tblM_CatMaquina> _lstCatMaquinasDapperDTO(MainContextEnum idEmpresa)
        {
            return MaquinaDAO._lstCatMaquinasDapperDTO(idEmpresa);
        }
        #endregion
    }
}
