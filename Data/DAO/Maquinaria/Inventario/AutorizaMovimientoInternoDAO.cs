using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Inventario
{
    public class AutorizaMovimientoInternoDAO : GenericDAO<tblM_AutorizaMovimientoInterno>, IAutorizaMovimientoInternoDAO
    {

        public void GuardarActualizar(tblM_AutorizaMovimientoInterno obj, bool esUsuarioRecibe = false)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.AUTORIZACIONMOVIMIENTOINTERNO);
            else
            {
                if (esUsuarioRecibe)
                {
                    try
                    {
                        var listaEquiposAsignados = _context.tblM_AsignacionEquipos
                            .Where(x =>
                                x.noEconomicoID.Equals(obj.ControMovimientoInterno.EconomicoID) &&
                                x.estatus >= 3)
                            .ToList();
                        if (listaEquiposAsignados.Count > 0)
                        {
                            listaEquiposAsignados
                                .ForEach(x => x.estatus = 10);
                            _context.SaveChanges();
                        }
                    }
                    catch { }
                }
                Update(obj, obj.id, (int)BitacoraEnum.AUTORIZACIONMOVIMIENTOINTERNO);
            }
            SaveChanges();
        }

        public tblM_AutorizaMovimientoInterno GetAutorizadores(int ControMovimientoInternoID)
        {
            return _context.tblM_AutorizaMovimientoInterno.FirstOrDefault(x => x.ControMovimientoInternoID == ControMovimientoInternoID);
        }
        public int GetAutorizadores(int tipo, string cc)
        {
            return _context.tblM_ControMovimientoInterno_Permisos.FirstOrDefault(x => x.tipo == tipo && x.cc == cc).usuarioID;
        }
    }
}
