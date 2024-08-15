using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IAutorizacionSolicitudesDAO
    {
        void Guardar(tblM_AutorizacionSolicitudes obj);
        tblM_AutorizacionSolicitudes getAutorizadores(int idSolicitud);
        List<tblM_CatMaquina> ListaEconomicos(int grupoid, int modeloid, int tipoId,int idEconomico);
        tblM_AutorizacionSolicitudes GetAutorizacionSolicitudes(int id);

       

    }
}
