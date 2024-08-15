using Core.DTO.Maquinaria._Caratulas;
using Core.DTO.Maquinaria.Caratulas;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Auth;
using Core.Entity.Maquinaria._Caratulas;
using Core.Entity.Maquinaria.Caratulas;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Maquinaria.Caratulas
{
    public interface ICaratulasDAO
    {
        List<ComboDTO> FillAreasCuentas();
        List<ComboDTO> FillCboModelo();
        List<ComboDTO> FillCboGrupo();
        List<ComboDTO> FillCaratulas();
        Dictionary<string, object> Autorizar(authDTO Autorizar, int id);
        Dictionary<string, object> Rechazar(authDTO Rechazar);
        List<CaratulaGuardadoDTO> GetCaratula();
        List<CaratulaGuardadoDTO> MostrarArchivo(HttpPostedFileBase archivo, decimal tipoCambio);
        bool GuardarModelo(tblM_Caratulas parametros);
        bool GuardarCaratula(List<CaratulaGuardadoDTO> listaCaratula, decimal tipoCambio, int idTecnico, int idSubdireccionMaquinaria);
        List<IndicadoresCaratulaDTO> GetIndicadores();
        bool GuardarIndicadores(tblM_IndicadoresCaratula parametros);
        bool ActualizarIndicadoresNuevos(List<tblM_IndicadoresCaratula> lstNuevoIndicadores);
        List<ReporteCaratulaDTO> GetReporte(int idCaratula);
        Dictionary<string, object> ListaAutorizantes(int idCaratula);
        Dictionary<string, object> EnviarCorreo(List<Byte[]> downloadPDF);
        Dictionary<string, object> EnviarCorreoGuardarCaratula(List<Byte[]> downloadPDF);
        Dictionary<string, object> CargarCaratulaActiva(List<int> lstTipoHoraDia);
        bool ObtenerAutorizante(int id);
        List<ComboDTO> obtenerComboCaratulras();
        List<ComboDTO> obtenerCC();
        Dictionary<string, object> obtenerCaratula(int idCaratula, int idCC, int status, int esHoraDia);
        Dictionary<string, object> obtenerHistorialCaratulas(int estatus);


        Dictionary<string, object> obtenerAgrupacionCaratulas();
        List<ComboDTO> obtenerGrupos();
        List<ComboDTO> obtenerModelos(int idGrupo, int Editar, int Agrupacion);
        bool EliminarAgrupacion(int id);
        bool EliminarModeloAgrupacion(int id);
        Dictionary<string, object> GuardarEditar(CaratulaEncDTO parametros);
        List<ComboDTO> ObtenerAgrupaciones();

        List<tblM_CaratulaConceptos> conceptosMoneda();
    }
}
