using Core.DAO.Maquinaria.Inventario;
using Core.DTO;
using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
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
    public class AutorizacionSolicitudesDAO : GenericDAO<tblM_AutorizacionSolicitudes>, IAutorizacionSolicitudesDAO
    {
        public void Guardar(tblM_AutorizacionSolicitudes obj)
        {
            if (true)
            {


                if (obj.id == 0)
                {
                    SaveEntity(obj, (int)BitacoraEnum.AUTORIZASOLICITUD);

                    try
                    {
                        var alert = _context.tblP_Alerta.FirstOrDefault(x => x.objID == obj.id && x.sistemaID == 1 && x.visto == true && x.userRecibeID == vSesiones.sesionUsuarioDTO.id);
                        if (alert != null)
                        {
                            alert.visto = true;
                            _context.SaveChanges();
                        }
                    }
                    catch(Exception e){}
                }
                else
                    Update(obj, obj.id, (int)BitacoraEnum.AUTORIZASOLICITUD);
            }
            else
            {
                throw new Exception("Ya se genero un folio con ese consecutivo consultar con sistemas...");
            }

        }

        public tblM_AutorizacionSolicitudes getAutorizadores(int idSolicitud)
        {
            return _context.tblM_AutorizacionSolicitudes.FirstOrDefault(x => x.solicitudEquipoID.Equals(idSolicitud));
        }

        public List<tblM_CatMaquina> ListaEconomicos(int grupoid, int modeloid, int tipoId, int idEconomico)
        {
            //   List<string> ekonomicos = EconomicoEK.Select(x => x.Economico).ToList();
            var economicosAsignados = _context.tblM_AsignacionEquipos.Where(x => x.noEconomicoID != 0 && x.estatus != 4).Select(x => x.noEconomicoID);
            //var Economicos = _context.tblM_CatMaquina.Where(x => !economicosAsignados.Contains(x.id));
            var Economicos = _context.tblM_CatMaquina.Where(x => x.grupoMaquinariaID.Equals(grupoid) && x.estatus == 1);

            var res = Economicos.Where(x => x.grupoMaquinariaID.Equals(grupoid) && string.IsNullOrEmpty(x.noEconomico) != true);
            //var res = (from x in _context.tblM_CatMaquina
            //           where x.grupoMaquinariaID.Equals(grupoid) &&//|| x.modeloEquipoID.Equals(modeloid) && x.grupoMaquinaria.tipoEquipoID.Equals(tipoId) &&
            //           idEconomico == 0 ? x.id.Equals(x.id) : idEconomico.Equals(x.id)
            //           select x).ToList();

            return res.ToList();
        }



        //private string getNombre(string EconomicoEK, string economico)
        //{
        //    var CC = EconomicoEK.FirstOrDefault(y => y.Economico.Equals(economico)).CC;
        //    if (!string.IsNullOrEmpty(CC.ToString()))
        //    {
        //        return EconomicoEK.FirstOrDefault(y => y.Economico.Equals(economico)).CC;
        //    }
        //    else
        //    {
        //        return "";
        //    }

        //}

        //private int getCC(string cc, string economico)
        //{
        //    var CC = EconomicoEK.FirstOrDefault(y => y.Economico.Equals(economico)).CCOrigen;
        //    if (!string.IsNullOrEmpty(CC.ToString()))
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return EconomicoEK.FirstOrDefault(y => y.Economico.Equals(economico)).CCOrigen;

        //    }

        //}

        public tblM_AutorizacionSolicitudes GetAutorizacionSolicitudes(int id)
        {
            return _context.tblM_AutorizacionSolicitudes.FirstOrDefault(x => x.id.Equals(id));
        }

    }
}
