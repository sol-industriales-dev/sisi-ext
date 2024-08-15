using Core.DAO.Principal.Usuarios;
using Core.DTO.Principal.Usuarios;
using Core.Entity.Principal.Usuarios;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Principal.Usuarios
{
    public class OrganigramaDAO : GenericDAO<tblP_Usuario>, IOrganigramaDAO
    {
        public List<OrganigramaDTO> getByUserID(int id)
        {
            var result = new List<OrganigramaDTO>();
            var temp = _context.tblP_Usuario.FirstOrDefault(x => x.id == id && x.estatus==true);
            var parent = new OrganigramaDTO();
            parent = getOrganigramaDTO(temp);
            parent.childs.AddRange(getFirsttUser(parent.usuarioID, (int)parent.puestoID, parent.departamentoID, parent.nivel));
            parent.childsCount = parent.childs.Count();
            result.Add(parent);

            return result;
        }
        public OrganigramaDTO getOrganigramaDTO(tblP_Usuario u)
        {
            var r = new OrganigramaDTO();
            r.usuarioID = u.id;
            r.usuario = u.nombre + " " + u.apellidoPaterno + " " + u.apellidoMaterno;
            r.departamentoID = u.puesto.departamentoID;
            r.departamento = u.puesto.departamento.descripcion;
            r.puestoID = u.puestoID;
            r.puesto = u.puesto.descripcion;
            r.puestoPadreID = u.puesto.puestoPadreID;
            var puestoPadre = _context.tblP_Puesto.FirstOrDefault(x => x.id == r.puestoPadreID);
            r.puestoPadre = puestoPadre != null ? puestoPadre.descripcion : "";
            r.nivel = u.puesto.nivel;
            r.childsCount = 0;
            r.childs = new List<OrganigramaDTO>();
            return r;
        }
        public tblP_Usuario getOrganigramaChild(tblP_Usuario u)
        {
            var temp = _context.tblP_Usuario.FirstOrDefault(x => x.id == u.id && x.estatus==true);
            bool next = true;
            while (next)
            {
                var owner = getOrganigramaDTO(temp);
                //result.Add(owner);
                var nextObj = _context.tblP_Puesto.Where(x => x.puestoPadreID == owner.puestoID).ToList();
                if (nextObj.Count() > 0)
                {

                }
                else
                {
                    next = false;
                }

            }
            return new tblP_Usuario();
        }
        public List<OrganigramaDTO> getFirsttUser(int userID, int puestoID, int deptoID, int nivel)
        {
            var next = new List<OrganigramaDTO>();
            var nextPuestos = _context.tblP_Puesto.Where(x => x.puestoPadreID == puestoID).ToList();
            if (nextPuestos.Count() > 0)
            {
                var listUsuarios = new List<tblP_Usuario>();
                foreach (var i in nextPuestos)
                {

                    if (i.usuarios != null)
                    {
                        foreach (var j in i.usuarios.Where(x=>x.estatus==true))
                        {
                            var parent = new OrganigramaDTO();
                            parent = getOrganigramaDTO(j);
                            parent.childs.AddRange(getSecondUser(parent.usuarioID, (int)parent.puestoID, parent.departamentoID, parent.nivel));
                            parent.childsCount = parent.childs.Count();
                            next.Add(parent);
                        }
                    }
                }
            }
            return next.OrderBy(x => x.childsCount).ToList();
        }
        public List<OrganigramaDTO> getSecondUser(int userID, int puestoID, int deptoID, int nivel)
        {
            var next = new List<OrganigramaDTO>();
            var nextPuestos = _context.tblP_Puesto.Where(x => x.puestoPadreID == puestoID).ToList();
            if (nextPuestos.Count() > 0)
            {
                var listUsuarios = new List<tblP_Usuario>();
                foreach (var i in nextPuestos)
                {

                    if (i.usuarios != null)
                    {
                        foreach (var j in i.usuarios.Where(x => x.estatus == true))
                        {
                            var parent = new OrganigramaDTO();
                            parent = getOrganigramaDTO(j);
                            parent.childsCount = 0;
                            next.Add(parent);
                        }
                    }
                }
            }
            return next;
        }
    }
}
