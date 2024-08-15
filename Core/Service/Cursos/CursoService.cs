using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO.Cursos;
using Core.Entity.Cursos;
using Core.DTO.Principal.Generales;
namespace Core.Service.Cursos
{
    public class CursoService : ICursoDAO
    {
        private ICursoDAO cursoDAO { get; set; }
        public CursoService(ICursoDAO cursoDAO)
        {
            this.cursoDAO = cursoDAO;
        }
        public tblCU_Curso GuardarCurso(tblCU_Curso objCurso) 
        {
            return this.cursoDAO.GuardarCurso(objCurso);
        }
        public tblCU_Modulo GuardarModulo(tblCU_Modulo objModulo)
        {
            return this.cursoDAO.GuardarModulo(objModulo);
        }
        public tblCU_ModuloDet GuardarModuloDet(tblCU_ModuloDet objModuloDet)
        {
            return this.cursoDAO.GuardarModuloDet(objModuloDet);
        }
        public List<tblCU_Modulo> getModuloid(int id)
        {
            return cursoDAO.getModuloid(id);
        }
        public List<tblCU_Curso> GetListCursos(int id, string folio, string nombre, int combo) {
            return cursoDAO.GetListCursos(id, folio, nombre, combo);
        }
        public List<tblCU_ModuloDet> getModuloDetid(int id)
        {
            return cursoDAO.getModuloDetid(id);
        }
        public List<ComboDTO> FillComboCurso() 
        {
            return cursoDAO.FillComboCurso();
        }
        public tblCU_Examen GuardarExamen(tblCU_Examen objExamen) {
            return cursoDAO.GuardarExamen(objExamen);
        }
        public tblCU_ExamenPregunta GuardarPregunta(tblCU_ExamenPregunta objPregunta)
        {
            return cursoDAO.GuardarPregunta(objPregunta);
        }
        public tblCU_ExamenRespuesta GuardarRespuesta(tblCU_ExamenRespuesta objRespuesta)
        {
            return cursoDAO.GuardarRespuesta(objRespuesta);
        }
        public void EliminaModulo(int idModulo) {
            cursoDAO.EliminaModulo(idModulo);
        }
        public void EliminaModuloDet(int idModulo)
        {
            cursoDAO.EliminaModuloDet(idModulo);
        }
        public bool ComparaPagina(int pagina, int modulo)
        {
          return cursoDAO.ComparaPagina(pagina, modulo);
        }
        public void EliminaModuloDetbyId(int id)
        {
            cursoDAO.EliminaModuloDetbyId(id);
        }
        public int EliminaCurso(int IdCurso) {
            return cursoDAO.EliminaCurso(IdCurso);
        }
        public List<object> GetListDeptos() {
            return cursoDAO.GetListDeptos();
        }
        public void AsignarCurso(List<tblCU_Asignacion> lstAsignacion)
        {
            cursoDAO.AsignarCurso(lstAsignacion);
        }
    }
}
