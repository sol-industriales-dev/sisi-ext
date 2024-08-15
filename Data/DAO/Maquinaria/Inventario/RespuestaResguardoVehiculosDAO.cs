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
    public class RespuestaResguardoVehiculosDAO : GenericDAO<tblM_RespuestaResguardoVehiculos>, IRespuestaResguardoVehiculosDAO
    {
        public void Guardar(List<tblM_RespuestaResguardoVehiculos> obj)
        {
            if (true)
            {
                saveEntitys(obj, (int)BitacoraEnum.ResguardoRespuestas);
            }
            else
            {
                throw new Exception("");
            }
        }

        public List<RespuestasDTO> GetResguardoRespuestas(int id)
        {
            var result = (from r in _context.tblM_RespuestaResguardoVehiculos
                          join p in _context.tblM_CatPreguntaResguardoVehiculo
                          on r.RespuestaID equals p.id
                          where r.ResguardoID == id && p.GrupoID != 3
                          select new
                          {
                              r,
                              p
                          }).ToList().Select(x => new RespuestasDTO
                          {
                              Bueno = setX(x.r.Bueno),
                              DescripcionGrupo = x.p.DescripcionGrupo,
                              GrupoID = x.p.GrupoID,
                              Malo = setX(x.r.Malo),
                              NoAplica = setX(x.r.NA),
                              Observaciones = x.r.Observaciones,
                              Pregunta = x.p.Pregunta,
                              Regular = setX(x.r.Regular)
                          }).ToList();



            return result;
        }

        public List<RespuestasDTO> GetResguardoRespuestasLiberado(int id)
        {
            var result = (from r in _context.tblM_RespuestaResguardoVehiculos
                          join p in _context.tblM_CatPreguntaResguardoVehiculo
                          on r.RespuestaID equals p.id
                          where r.ResguardoID == id && p.GrupoID != 3 && r.TipoResguardo == 2
                          select new
                          {
                              r,
                              p
                          }).ToList().Select(x => new RespuestasDTO
                          {
                              Bueno = setX(x.r.Bueno),
                              DescripcionGrupo = x.p.DescripcionGrupo,
                              GrupoID = x.p.GrupoID,
                              Malo = setX(x.r.Malo),
                              NoAplica = setX(x.r.NA),
                              Observaciones = x.r.Observaciones,
                              Pregunta = x.p.Pregunta,
                              Regular = setX(x.r.Regular)
                          }).ToList();



            return result;
        }

        public List<DocumentoGridDTO> getDocumentos(int id)
        {
            var result = (from r in _context.tblM_RespuestaResguardoVehiculos
                          join p in _context.tblM_CatPreguntaResguardoVehiculo
                          on r.RespuestaID equals p.id
                          where r.ResguardoID == id && p.GrupoID == 3
                          select new
                          {
                              r,
                              p
                          }).ToList().Select(x => new DocumentoGridDTO
                          {
                              Concepto = x.p.Pregunta,
                              Observacion = x.r.Observaciones,
                              Si = x.r.HasDocumento == 1 ? "X" : "",
                              No = x.r.HasDocumento == 0 ? "X" : "",
                          }).ToList();


            return result;
        }
        private string setX(int c)
        {
            string x = "";

            if (c == 1)
            {
                x = "x";
            }

            return x;
        }

    }
}
