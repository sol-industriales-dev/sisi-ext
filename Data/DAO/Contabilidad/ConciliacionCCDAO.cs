using Core.DAO.Contabilidad;
using Core.DTO;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad;
using Core.Enum.Principal;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;
using Core.DTO.Contabilidad;

namespace Data.DAO.Contabilidad
{
    public class ConciliacionCCDAO : GenericDAO<tblC_Cta_RelCC>, IConciliacionCCDAO
    {
        Dictionary<string, object> resultado = new Dictionary<string, object>();



        public Dictionary<string, object> FillCCPrincipal()
        {             
            try
            {
                var EmpresaCCPal = _context.Select<ComboDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.PERUSTARSOFT,
                consulta = "SELECT CENCOST_CODIGO AS VALUE ,CENCOST_DESCRIPCION AS TEXT FROM CENTRO_COSTOS WHERE CENCOST_CODIGO in(010101,020101)",
            });

            resultado.Add(SUCCESS, true);
            resultado.Add(ITEMS, EmpresaCCPal);                
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> FillCCSecundario()
        {            
            try
            {
            var EmpresaCCSec = _context.Select<ComboDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = "SELECT cc as VALUE , descripcion as TEXT FROM tblP_CC order by cc asc",

            });

            resultado.Add(SUCCESS, true);
            resultado.Add(ITEMS, EmpresaCCSec);
             }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> GetBuscarConciliacionCC(List<string> palEmpresaCC)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var listConciliacionCC = _context.tblC_Cta_RelCC.Where(e => e.esActivo == true).ToList();

            

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, listConciliacionCC);
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GuardarEditarConciliacionCC(tblC_Cta_RelCC data)
        {
            #region
            var resultado = new Dictionary<string, object>();
            
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var conciliacionCC = _context.tblC_Cta_RelCC.Where(w => w.id == data.id).FirstOrDefault();
                    if (conciliacionCC == null)
                    {
                    data.idUsuarioRegistro = vSesiones.sesionUsuarioDTO.id;
                    data.fechaRegistro = DateTime.Now;
                    data.esActivo = true;
                    _context.tblC_Cta_RelCC.Add(data);
                    _context.SaveChanges();
                    int validarCC = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.PERU,
                        consulta = @"SELECT COUNT(*) FROM tblC_Cta_RelCC WHERE ccPrincipal = @ccPrincipal",
                        parametros = new { ccPrincipal = data.ccPrincipal }
                    }).Count();

                        if(validarCC>0)
                        {
                            throw new Exception("CC ya existe");
                        }
                    }
                    else
                    {

                    conciliacionCC.ccPrincipal = data.ccPrincipal;
                    conciliacionCC.descripcionCCPrincipal = data.descripcionCCPrincipal;
                    conciliacionCC.ccSecundario = data.ccSecundario;
                    conciliacionCC.descripcionCCSecundario = data.descripcionCCSecundario;
                    _context.SaveChanges();
                    }
                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception ex)
                {
                    resultado.Add(MESSAGE, ex.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
            #endregion
        }
        public Dictionary<string, object> EliminarConciliacionCC(int id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var conciliacionCC = _context.tblC_Cta_RelCC.FirstOrDefault(x => x.id == id);

                    conciliacionCC.esActivo = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }



    }
}
