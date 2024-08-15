using Core.Entity.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Maquinaria.Captura;

namespace Core.DAO.RecursosHumanos.Captura
{
    public interface IFormatoCambio
    {
        tblRH_FormatoCambio getFormatoByID(int id);
        tblRH_FormatoCambio getEmpleadoForId(int idEmpleado, bool activo);
        tblRH_FormatoCambio SaveChangesEmpleado(tblRH_FormatoCambio objEmpleado);
        List<tblRH_FormatoCambio> getListFormatosCambioPendientes(int id, string cc, int claveEmp, int estado,string tipo,int numero);
        void eliminarFormato(int formatoID);
        int getFormatoCambioID(int id);
        List<tblRH_CatPuestos> getCatPuestos(string term);
        List<tblRH_CatTipoNomina> getCatTipoNomina();
        List<tblRH_CatCentroCostos> getCC(string term);
        List<tblRH_CatEmpleados> getCatEmpleados(string term);
        List<tblRH_CatEmpleados> getCatEmpleadosReclutamientos(string term);
        List<tblRH_CatEmpleados> getCatEmpleadosGeneral(string term);
        List<tblRH_CatRegistroPatronales> getCatRegistroPatronales(string term);
        List<tblRH_CatCentroCostos> getCCList();
        tblRH_FormatoCambio getFormatoCambioByID(int idFormatoCambio);
        void EmpleadoEnkontrolToSigoplan();
        bool getEmpleadoExclusion(int empleadoCVE);
        bool validReporteExclusion(int id);
        List<tblRH_CatEmpleados> getCatEmpleadosTodos(string term);
        Dictionary<string, object> getDepartamentosCC(string cc);
        Dictionary<string, object> GetResponsableCC(string cc);
        Dictionary<string, object> GetRegistroPatCC(string cc);
        Dictionary<string, object> GetTabuladorByEmpleado(int puesto, int lineaNegocios, int categoria);

        #region PERMISOS
        bool CheckEsEditarPuestos();
        bool GetPermisoSueldos();
        #endregion

        List<cboDTO> cboCentroCostos();
    }
}
