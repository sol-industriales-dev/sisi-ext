using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario;
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
    public class SolicitudEquipoDetDAO : GenericDAO<tblM_SolicitudEquipoDet>, ISolicitudEquipoDetDAO
    {
        public void Guardar(List<tblM_SolicitudEquipoDet> array)
        {
            foreach (tblM_SolicitudEquipoDet obj in array)
            {


                if (obj.modeloEquipoID == 0)
                {

                    obj.modeloEquipoID = 668;
                }
                else
                {
                    var existe = _context.tblM_CatModeloEquipo.Where(x => x.id == obj.modeloEquipoID);
                    if (existe.Count() == 0)
                    {
                        obj.modeloEquipoID = 668;
                    }
                }
                if (true)
                {
                    if (obj.id == 0)
                        SaveEntity(obj, (int)BitacoraEnum.SOLICITUDEQUIPO);
                    else
                        Update(obj, obj.id, (int)BitacoraEnum.SOLICITUDEQUIPO);
                }
                else
                {
                    throw new Exception("Error ocurrio un error al insertar un registro");
                }
            }

        }
        public void delete(int id)
        {

            try
            {
                tblM_SolicitudEquipoDet entidad = _context.tblM_SolicitudEquipoDet.FirstOrDefault(x => x.id.Equals(id));

                Delete(entidad, 24);
            }
            catch (Exception)
            {
                throw new Exception("Error al eliminar el registro");
            }
        }

        public List<tblM_SolicitudEquipoDet> listaDetalleSolicitud(int obj)
        {
            var data = _context.tblM_SolicitudEquipoDet.Where(x => x.solicitudEquipoID.Equals(obj)).ToList();

            return data;

        }

        public tblM_SolicitudEquipoDet DetSolicitud(int obj)
        {
            return _context.tblM_SolicitudEquipoDet.FirstOrDefault(x => x.id.Equals(obj));

        }
        public void Guardar(tblM_SolicitudEquipoDet obj)
        {
            if (true)
            {
         

                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.SOLICITUDEQUIPO);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.SOLICITUDEQUIPO);
            }
            else
            {
                throw new Exception("Ya se genero un folio con ese consecutivo consultar con sistemas...");
            }

        }

        public List<tblM_SolicitudEquipo> ListaSolicitudes(string cc)
        {

            var res = from s in _context.tblM_SolicitudEquipo
                      //          join a in _context.tblM_AutorizacionSolicitudes
                      //            on s.id equals a.solicitudEquipoID
                      where s.CC.Equals(cc)
                      select s;

            //    return _context.tblM_SolicitudEquipo 

            //      .Where(x => x.CC.Equals(cc) && x.Estatus == false && x.cantidad > 0).ToList();

            return res.ToList();
        }

        public tblM_SolicitudEquipo getSolicitudbyID(int obj)
        {
            return _context.tblM_SolicitudEquipo.FirstOrDefault(x => x.id.Equals(obj));
        }



        //public List<SolicitudEquipoDTO> getListaDetalleSolicitud(int idSolicitud)
        //{

        //    var res = (from sd in _context.tblM_SolicitudEquipoDet
        //               // join s in _context.tblM_SolicitudEquipo on sd.solicitudEquipoID equals s.id
        //               where sd.Equals(idSolicitud)
        //               select sd).ToList();

        //    return res;
        //}
    }
}
