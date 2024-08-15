using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Overhaul;
using Core.Entity.Principal.Multiempresa;
using Core.DTO.Principal.Generales;
using Core.DTO.Maquinaria.Overhaul;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface IComponenteDAO
    {
        void Guardar(tblM_CatComponente obj);
        List<ComponenteDTO> FillGrid_Componente(ComponenteDTO obj);
        List<tblM_CatConjunto> FillCboConjuntos(bool estatus);
        List<tblM_CatSubConjunto> FillCboSubConjuntos(int idConjunto);
        List<ComboDTO> FillCboGrupoMaquinaria();
        List<cboPrefijoModeloDTO> FillCboPrefijoModelo(int idModelo);
        List<tblM_CatModeloEquipo> FillCboModeloEquipo(int idGrupo);
        List<tblM_CatModeloEquipo> FillCboFiltroModeloEquipo();
        List<ComboDTO> FillCboConjunto(int idModelo);
        //List<int> getSubConjuntoPosiciones(int idSubConjunto);
        List<tblM_CatLocacionesComponentes> FillCboLocaciones(int tipoBusqueda);
        List<tblM_CatMaquina> FillCboLocacionesMaquina(int idModelo);
        int GuardarTrackingComponente(tblM_CatComponente obj, int locacion, DateTime fecha, int tipoLocacion, bool reciclado, string ordenCompra, string costo);
        void DeleteComponente(int idComponente);
        List<tblP_CC> FillCboCentroCostros();
        List<tblM_CatSubConjunto> FillCboSubConjuntos(List<int> idConjunto, int idModelo);
        tblM_trackComponentes getLocacion(int idComponente);
        List<tblM_CatMarcasComponentes> getMarcas();
        tblM_CatMarcasComponentes getMarcaComponenteByID(int id);
        string getNumParte(int idModelo, int idSubconjunto);
        tblM_CatComponente getComponenteByID(int idComponente);
        string getLocacionDescripcion(int idComponente);
        string getCCByID(int id);
        void guardarModificaciones(int cicloVidaHoras, int garantia, int estatusNuevo, List<ComboDTO> cc, string descripcionComponente, string locacion, int subconjunto, bool estatusActual, int modelo);
        List<tblM_CatComponente> getComponentesByIDs(List<int> arrComponentes);
        List<ComboDTO> FillCboPosicionesComponente(int idSubconjunto);
        bool ActualizarTracking(int idComponente, int idTracking);
        List<int> GetMaquinaByListaCC(List<string> obras);
    }
}
