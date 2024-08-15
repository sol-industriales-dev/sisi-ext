using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IControlInternoMovimientosDAO
    {
        void GuardarActualizar(tblM_ControMovimientoInterno obj);
        List<tblM_CatMaquina> FillCboEconomicos(string cc);
        tblM_CatMaquina GetDataEconomicoID(int id);
        string LoadFolio();
        List<tblM_ControMovimientoInterno> GetControlesRealizados(int filtro);
        List<ComboDTO> FillCboEconomicosUsuarioID(int usuario);
        List<ComboDTO> getCentrosCostos(int TipoCbo);
        List<ComboDTO> getCentrosCostosRecepcion(string CentroCostos);
        int GetUsuarioAutoriza(string centroCostos);
    }
}
