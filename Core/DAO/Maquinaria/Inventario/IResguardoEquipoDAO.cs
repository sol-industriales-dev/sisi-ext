using Core.Entity.Maquinaria.Inventario;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IResguardoEquipoDAO
    {
        void GuardarResguardoVehiculos(tblM_ResguardoVehiculosServicio obj);
        List<tblM_CatPreguntaResguardoVehiculo> GetListaPreguntas();
        List<tblRH_CatEmpleados> getCatEmpleados(string term, List<string> CentroCostos);
        List<tblM_ResguardoVehiculosServicio> GetListaAutorizacionesPendientes(string cc, int obj);
        string GetFechaVigenciaResguardo(int id);
        tblM_ResguardoVehiculosServicio getResguardoBYID(int obj);
        tblRH_CatEmpleados getCatEmpleado(string id);
        List<int> GetEmpleadosResguardo();

        List<int> GetMaquinariaAsignada();

        List<tblM_ResguardoVehiculosServicio> getListaResguardosPendientesAutorizacion(string cc, int economicoID);

        List<tblM_ResguardoVehiculosServicio> getListaResguardosPendientesLicencia(List<tblP_CC_Usuario> listObj);

        List<tblM_ResguardoVehiculosServicio> getListaResguardosPendientesPoliza(List<tblP_CC_Usuario> listObj);

        string getCCByArea(string area);

        string getMaquinaByID(int id);

        string getNoEconomicoMaquinaByID(int id);

        string getModeloByID(int id);
        List<tblM_CatMaquina> getEquipoSinResguardo(string ac);
        List<dynamic> GetDocumentosResguardos();

        List<tblM_ResguardoVehiculosServicio> GetCursosManejoVencidos();
        List<tblP_CC> GetCentrosCostos();

        void NotificarCoordinadorSSOMA(string cc, int resguardoId, string economico);
        void QuitarNotificacionSSOMA(int resguardoId);
    }
}
