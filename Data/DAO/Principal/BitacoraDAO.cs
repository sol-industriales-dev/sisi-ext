using Core.DAO.Principal.Bitacoras;
using Core.DTO.Principal.Bitacoras;
using Core.Entity.Principal.Bitacoras;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Principal
{
    public class BitacoraDAO : GenericDAO<tblP_Bitacora>, IBitacoraDAO
    {
        public IList<BitacoraDTO> getBitacora(int Modulo, int RegistroID)
        {
            var a = new List<tblP_Bitacora>();
            switch (Modulo)
            {
                case (int)BitacoraEnum.TIPOMAQUINARIA:
                case (int)BitacoraEnum.USUARIO:
               
                default:
                    a = _context.tblP_Bitacora.Where(x => x.modulo == Modulo && x.registroID == RegistroID).ToList();
                    break;
            }

            var b = new List<BitacoraDTO>();
            a.ForEach(x => b.Add(new BitacoraDTO
            {
                id = x.id,
                modulo = (x.modulo).ToString(),
                accion = (x.accion).ToString(), ///EnumExtensions.GetDescription((AccionEnum)x.accion),
                usuario = _context.tblP_Usuario.FirstOrDefault(y => y.id == x.usuarioID).nombre,
                fechaCreacion = x.fecha,
                registroID = x.registroID,
                objeto = x.objeto
            }));
            return b.ToList();
        }

    }
}
