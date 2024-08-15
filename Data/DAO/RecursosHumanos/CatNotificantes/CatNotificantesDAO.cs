using Core.DAO.Enkontrol.General.CC;
using Core.DAO.Maquinaria.Reporte;
using Core.DAO.RecursosHumanos.CatNotificantes;
using Core.Entity.Principal.Usuarios;
using Data.EntityFramework.Generic;
using Data.Factory.Enkontrol.General.CC;
using Data.Factory.Maquinaria.Reporte;
using Data.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.RecursosHumanos.CatNotificantes;
using Core.Enum.RecursosHumanos.CatNotificantes;
using Core.DTO.Utils.Data;
using Core.Entity.RecursosHumanos.CatNotificantes;
using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.Enum.Principal.Bitacoras;

namespace Data.DAO.RecursosHumanos.CatNotificantes
{
    public class CatNotificantesDAO : GenericDAO<tblP_Usuario>, ICatNotificantesDAO
    {
        Dictionary<string, object> resultado = new Dictionary<string, object>();
        ICCDAO _ccFS_SP = new CCFactoryService().getCCServiceSP();
        IEncabezadoDAO encabezadoFactoryServices = new EncabezadoFactoryServices().getEncabezadoServices();

        #region CAT NOTIFICANTES
        public Dictionary<string,object> GetNotificantes()
        {
            resultado.Clear();
            
            using (var ctx = new MainContext())
            {
                try
                {
                    var lstCCs = _ccFS_SP.GetCCsNomina(true).ToList();
                    var lstUsuarios = ctx.Select<UsuariosNotificantesDTO>(new DapperDTO()
                    {
                        consulta = @"SELECT 
                                        t1.id as idRelNoti,
                                        t1.idUsuario,
                                        t1.idConcepto,
                                        t1.cc,
                                        (ISNULL(t2.apellidoPaterno, '') + ' ' + ISNULL(t2.apellidoMaterno, '') + ' ' + ISNULL(t2.nombre, '')) as usrNomCompleto
                                    FROM tblRH_Notis_RelConceptoUsuario as t1 
                                    INNER JOIN tblP_Usuario as t2 ON t1.idUsuario = t2.id
                                    WHERE t1.esActivo = 1"
                    });

                    var lstRowsDTO = new List<RowsNotificantesDTO>();

                    foreach (var item in lstCCs)
                    {
                        var objRow = new RowsNotificantesDTO();

                        objRow.cc = item.cc;
                        objRow.ccDesc = item.cc + " " + item.descripcion;
                        objRow.nombresTaller = lstUsuarios.Where(e => e.cc == item.cc && e.idConcepto == (int)ConceptosNotificantesEnum.Taller).Select(e => e.usrNomCompleto).ToList();
                        objRow.nombresAlmacen = lstUsuarios.Where(e => e.cc == item.cc && e.idConcepto == (int)ConceptosNotificantesEnum.Almacen).Select(e => e.usrNomCompleto).ToList();
                        objRow.nombresConta = lstUsuarios.Where(e => e.cc == item.cc && e.idConcepto == (int)ConceptosNotificantesEnum.Contabilidad).Select(e => e.usrNomCompleto).ToList();
                        objRow.nombresNominas = lstUsuarios.Where(e => e.cc == item.cc && e.idConcepto == (int)ConceptosNotificantesEnum.Nominas).Select(e => e.usrNomCompleto).ToList();
                        objRow.nombresResponsableCC = lstUsuarios.Where(e => e.cc == item.cc && e.idConcepto == (int)ConceptosNotificantesEnum.ResponsableCC).Select(e => e.usrNomCompleto).ToList();
                        objRow.nombresAltas = lstUsuarios.Where(e => e.cc == item.cc && e.idConcepto == (int)ConceptosNotificantesEnum.Altas).Select(e => e.usrNomCompleto).ToList();
                        objRow.nombresBajas = lstUsuarios.Where(e => e.cc == item.cc && e.idConcepto == (int)ConceptosNotificantesEnum.Bajas).Select(e => e.usrNomCompleto).ToList();
                        objRow.nombresCH = lstUsuarios.Where(e => e.cc == item.cc && e.idConcepto == (int)ConceptosNotificantesEnum.CH).Select(e => e.usrNomCompleto).ToList();
                        objRow.nombresIncapacidades = lstUsuarios.Where(e => e.cc == item.cc && e.idConcepto == (int)ConceptosNotificantesEnum.Incapacidades).Select(e => e.usrNomCompleto).ToList();

                        lstRowsDTO.Add(objRow);
                    }

                    resultado.Add(ITEMS, lstRowsDTO);
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string,object> GetNotificantesDet(string cc, int idConcepto)
        {
            resultado.Clear();

            using (var ctx = new MainContext())
            {
                try
                {
                    var lstRelNoti = ctx.Select<UsuariosNotificantesDTO>(new DapperDTO()
                    {
                        consulta = @"SELECT 
                                        t1.id as idRelNoti,
                                        t1.idUsuario,
                                        t1.idConcepto,
                                        t1.cc,
                                        (t2.apellidoPaterno + ' ' + t2.apellidoMaterno + ' ' + t2.nombre) as usrNomCompleto
                                    FROM tblRH_Notis_RelConceptoUsuario as t1 
                                    INNER JOIN tblP_Usuario as t2 ON t1.idUsuario = t2.id
                                    WHERE t1.esActivo = 1 AND t1.cc = @cc AND t1.idConcepto = @idConcepto",
                        parametros = new { cc, idConcepto }
                    });

                    resultado.Add(ITEMS, lstRelNoti);
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> CrearEditarNotificantes(string cc, int idConcepto, List<int> lstUsuariosNuevos)
        {
            resultado.Clear();

            using (var ctx = new MainContext())
            {
                using (var dbTransac = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in lstUsuariosNuevos)
                        {
                            var objCENotificante = new tblRH_Notis_RelConceptoUsuario() 
                            {
                                idUsuario = item,
                                idConcepto = idConcepto,
                                cc = cc,
                                fechaCreacion = DateTime.Now,
                                fechaModificacion = DateTime.Now,
                                idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                                idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                                esActivo = true,
                            };

                            ctx.tblRH_Notis_RelConceptoUsuario.Add(objCENotificante);
                            ctx.SaveChanges();
                        }

                        dbTransac.Commit();

                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        resultado.Add(MESSAGE, e.Message);
                        resultado.Add(SUCCESS, false);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string,object> RemoveNotificante(int idRelNoti)
        {
            resultado.Clear();

            using (var ctx = new MainContext())
            {
                using (var dbTransac = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var objRelNoti = ctx.tblRH_Notis_RelConceptoUsuario.FirstOrDefault(e => e.id == idRelNoti);

                        objRelNoti.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        objRelNoti.fechaModificacion = DateTime.Now;
                        objRelNoti.esActivo = false;

                        ctx.SaveChanges();

                        dbTransac.Commit();

                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        resultado.Add(MESSAGE, e.Message);
                        resultado.Add(SUCCESS, false);
                    }
                }
            }

            return resultado;
        }
        #endregion

        #region FILLCOMBO
        public Dictionary<string,object> FillCboCC()
        {
            resultado.Clear();

            try
            {
                List<ComboDTO> ccs = _ccFS_SP.GetCCsNomina(true).Select(x => new ComboDTO
                {
                    Value = x.cc,
                    Text = "[" + x.cc + "] " + x.descripcion.Trim()
                }).OrderBy(x => x.Value).ToList();

                resultado.Add(ITEMS, ccs);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);

            }

            return resultado;
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<tblP_Usuario> lstUsuarios = _context.tblP_Usuario.Where(w => w.estatus).OrderBy(o => o.nombre).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                string nombreCompleto = string.Empty;
                foreach (var item in lstUsuarios)
                {
                    nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        nombreCompleto = item.nombre.Trim();
                    if (!string.IsNullOrEmpty(item.apellidoPaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoPaterno.Trim());
                    if (!string.IsNullOrEmpty(item.apellidoMaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoMaterno.Trim());

                    if (!string.IsNullOrEmpty(nombreCompleto))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.id.ToString();
                        objComboDTO.Text = nombreCompleto;
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboConceptos()
        {
            resultado.Clear();

            using (var ctx = new MainContext())
            {
                try
                {
                    var lstConceptos = ctx.tblRH_Notis_Conceptos.Where(e => e.esActivo).Select(e => new ComboDTO
                    {
                        Value = e.id.ToString(),
                        Text = e.descripcion
                    }).ToList();

                    resultado.Add(ITEMS, lstConceptos);
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);

                }
            }

            return resultado;
        }
        #endregion
    }
}
