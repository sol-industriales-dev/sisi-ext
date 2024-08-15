using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Inventario
{
    public class AutorizacionSolicitudReemplazoDAO : GenericDAO<tblM_AutorizacionSolicitudReemplazo>, IAutorizacionSolicitudReemplazoDAO
    {
        public void Guardar(tblM_AutorizacionSolicitudReemplazo obj)
        {

            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.AUTORIZACIONSOLICITUDREEMPLAZO);
            else
                Update(obj, obj.id, (int)BitacoraEnum.AUTORIZACIONSOLICITUDREEMPLAZO);
        }

        public tblM_AutorizacionSolicitudReemplazo getAutorizadores(int id)
        {

            var obj = _context.tblM_AutorizacionSolicitudReemplazo.FirstOrDefault(x => x.solicitudReemplazoEquipoID.Equals(id));

            return obj;
        }

        public List<tblM_SolicitudReemplazoEquipo> getListPendientes(int idUsuario, int tipo, string folio)
        {
            if (string.IsNullOrEmpty(folio.Trim()))
            {
                folio = null;
            }
            List<tblM_AutorizacionSolicitudReemplazo> SolicitudesAutorizacion2 = new List<tblM_AutorizacionSolicitudReemplazo>();

            List<tblM_AutorizacionSolicitudReemplazo> SolicitudesAutorizacion = _context.tblM_AutorizacionSolicitudReemplazo
                                          .Where(x => x.idAutorizaElabora.Equals(idUsuario)
                                                || x.idAutorizaGerente.Equals(idUsuario)
                                                || x.idAutorizaAsigna.Equals(idUsuario)
                                                ).ToList();


            switch (tipo)
            {
                case 1:
                    {

                        List<tblM_AutorizacionSolicitudReemplazo> Temp2 = SolicitudesAutorizacion.Where(x =>
                                (idUsuario == x.idAutorizaAsigna && x.CadenaAsigna == null && !string.IsNullOrEmpty(x.CadenaGerente))
                            ).ToList();

                        SolicitudesAutorizacion2.AddRange(Temp2);
                        List<tblM_AutorizacionSolicitudReemplazo> Temp = SolicitudesAutorizacion.Where(x =>
                            (idUsuario == x.idAutorizaGerente && x.CadenaGerente == null)
                            ).ToList();


                        SolicitudesAutorizacion2.AddRange(Temp);


                        SolicitudesAutorizacion = SolicitudesAutorizacion2;
                    }
                    break;
                case 2:
                    {
                        SolicitudesAutorizacion = SolicitudesAutorizacion.Where(x =>
                                (x.AutorizaAsigna == true && !string.IsNullOrEmpty(x.CadenaAsigna)) &&
                                (x.AutorizaElabora == true && !string.IsNullOrEmpty(x.CadenaElabora)) &&
                                (x.AutorizaGerente == true && !string.IsNullOrEmpty(x.CadenaGerente))

                            ).ToList();
                    }
                    break;
                case 3:
                    {
                        SolicitudesAutorizacion = SolicitudesAutorizacion.Where(x =>
                                (x.AutorizaAsigna == false && !string.IsNullOrEmpty(x.CadenaAsigna)) ||
                                (x.AutorizaElabora == false && !string.IsNullOrEmpty(x.CadenaElabora)) ||
                                (x.AutorizaGerente == false && !string.IsNullOrEmpty(x.CadenaGerente))
                            ).ToList();
                    }
                    break;
                default:
                    break;
            }

            List<string> SolicitudesPendientes = new List<string>();
            SolicitudesPendientes = SolicitudesAutorizacion.Select(x => x.solicitudReemplazoEquipoID.ToString()).ToList();

            foreach (tblM_AutorizacionSolicitudReemplazo obj in SolicitudesAutorizacion)
            {

                var dato = tipoAutorizador(obj, idUsuario);
                if (dato != 0)
                { SolicitudesPendientes.Add(dato.ToString()); }
            }

            var result = from c in _context.tblM_SolicitudReemplazoEquipo
                         where SolicitudesPendientes.Contains(c.id.ToString()) && (folio == null ? true : c.folio.Contains(folio))
                         select c;

            return result.ToList();
        }

        public string obtenerComentarioSolicitudReemplazo(int solicitudID)
        {
            try
            {
                var comentario = from aut in _context.tblM_AutorizacionSolicitudReemplazo
                                 where solicitudID == aut.solicitudReemplazoEquipoID
                                 select aut.Comentarios.Trim();
                if (comentario.Count() > 0)
                {
                    return WebUtility.HtmlEncode(comentario.FirstOrDefault().Replace("\n", " "));
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public tblM_AutorizacionSolicitudReemplazo GetAutorizacionReemplazoByID(int id)
        {
            return _context.tblM_AutorizacionSolicitudReemplazo.FirstOrDefault(x => x.id.Equals(id));
        }
        public tblM_AutorizacionSolicitudReemplazo GetAutorizacionReemplazoByIDReemplazo(int id)
        {
            return _context.tblM_AutorizacionSolicitudReemplazo.FirstOrDefault(x => x.solicitudReemplazoEquipoID.Equals(id));
        }

        private int tipoAutorizador(tblM_AutorizacionSolicitudReemplazo obj, int idUsuario)
        {
            if (obj.idAutorizaElabora.Equals(idUsuario))
            {
                return obj.solicitudReemplazoEquipoID;
            }

            if (obj.idAutorizaGerente.Equals(idUsuario))
            {

                if (obj.AutorizaElabora.Equals(true) && obj.AutorizaGerente.Equals(false))
                {
                    return obj.solicitudReemplazoEquipoID;
                }
            }
            if (obj.AutorizaAsigna.Equals(idUsuario))
            {
                if (obj.AutorizaAsigna.Equals(false) && obj.AutorizaGerente.Equals(true) && obj.AutorizaElabora.Equals(true))
                {
                    return obj.solicitudReemplazoEquipoID;
                }
            }
            return 0;
        }
    }
}
