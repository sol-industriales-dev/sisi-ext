using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.RecursosHumanos.Captura;
using Data.EntityFramework.Context;
using Core.DAO.RecursosHumanos.Captura;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos;
using Data.EntityFramework.Generic;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Enum.Principal.Bitacoras;
using Core.DTO;
using Data.Factory.Principal.Usuarios;
using Core.Enum;
using Core.Entity.Principal.Alertas;
using Core.Enum.Principal.Alertas;
using Core.Enum.Principal;
using System.Web;
using Infrastructure.Utils;
using Core.DTO.Utils.Data;
using System.Data.Odbc;
using Core.Enum.Multiempresa;
using Data.DAO.Principal.Usuarios;
using Data.EntityFramework;
using Core.DTO.Enkontrol.Tablas.RH.Plantilla;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.DAO.Enkontrol.General.CC;
using Data.Factory.Enkontrol.General.CC;
using Core.Entity.Principal.Usuarios;
using Core.Entity.Administrativo.RecursosHumanos.Tabuladores;

namespace Data.DAO.RecursosHumanos.Captura
{
    public class AditivaDeductivaDAO : GenericDAO<tblRH_AditivaDeductiva>, IAditivaDeductivaDAO
    {
        int id_plantilla = 0;

        ICCDAO _ccFS_SP = new CCFactoryService().getCCServiceSP();

        public List<tblRH_CatPuestos> getCatPuestos(string cC)
        {
            //if (string.IsNullOrEmpty(cC))
            //{
            //    return new List<tblRH_CatPuestos>();
            //}
            id_plantilla = ObtenerPlantilla(cC).id_plantilla;
            //var odbc = new OdbcConsultaDTO()
            //{
            //    consulta = "SELECT a.puesto, b.descripcion, b.tipo_nomina from sn_plantilla_puesto as a inner join si_puestos as b on  a.puesto= b.puesto where id_plantilla=? and b.descripcion not like '%NO USA%'",
            //    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO() { nombre = "id_plantilla", tipo = OdbcType.Int, valor = id_plantilla } }
            //};
            //var lst = _contextEnkontrol.Select<tblRH_CatPuestos>(EnkontrolAmbienteEnum.Rh, odbc);

            //anterior
            //var lst = _context.Select<tblRH_CatPuestos>(new DapperDTO
            //{
            //    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
            //    consulta = "SELECT a.puesto, b.descripcion, b.FK_TipoNomina as tipo_nomina from tblRH_EK_Plantilla_Puesto as a inner join tblRH_EK_Puestos as b on  a.puesto= b.puesto where id_plantilla= @paramPlantilla and b.descripcion not like '%NO USA%'",
            //    parametros = new { paramPlantilla = id_plantilla }
            //}).ToList();
            //anterior fin

            using (var ctx = new MainContext())
            {
                var lst = ctx.Select<tblRH_CatPuestos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT
                                    a.FK_Puesto AS puesto,
                                    b.descripcion,
                                    b.FK_TipoNomina AS tipo_nomina
                                    FROM
                                        tblRH_Tab_PlantillasPersonalDet AS a
                                    INNER JOIN
                                        tblRH_EK_Puestos AS b ON a.FK_Puesto = b.puesto
                                    WHERE
                                        a.FK_Plantilla = @paramPlantilla AND
                                        b.descripcion NOT LIKE '%NO USA%' AND
                                        b.descripcion NOT LIKE '%NOUSA%' AND
                                        b.registroActivo = 1",
                    parametros = new { paramPlantilla = id_plantilla }
                }).ToList();

                return lst;
            }
        }

        public List<tblRH_CatPuestos> getAllCatPuestos(string cC)
        {


            id_plantilla = ObtenerPlantilla(cC).id_plantilla;
            //var odbc = new OdbcConsultaDTO()
            //{
            //    consulta = "SELECT a.puesto, a.cc, b.descripcion, b.tipo_nomina from sn_plantilla_puesto as a inner join si_puestos as b on  a.puesto= b.puesto where id_plantilla<>? and b.descripcion not like '%NO USA%'",
            //    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO() { nombre = "id_plantilla", tipo = OdbcType.Int, valor = id_plantilla } }
            //};
            //var lst = _contextEnkontrol.Select<tblRH_CatPuestos>(EnkontrolAmbienteEnum.Rh, odbc);

            #region VERSION TOMA TODOS LOS PUESTO HASTA LOS DE LA MISMA PLANTILLA
            //var lst = _context.Select<tblRH_CatPuestos>(new DapperDTO
            //{
            //    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
            //    consulta = "SELECT a.puesto, a.cc, b.descripcion, b.tipo_nomina FROM tblRH_EK_Plantilla_Puesto AS a INNER JOIN tblRH_EK_Puestos AS b on a.puesto = b.puesto WHERE id_plantilla <> @paramPlantilla AND b.descripcion NOT LIKE '%NO USA%'",
            //    parametros = new { paramPlantilla = id_plantilla }
            //}).ToList();
            #endregion

            //anterior
            //var lst = _context.Select<tblRH_CatPuestos>(new DapperDTO
            //{
            //    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
            //    consulta = "SELECT puesto, @cC, descripcion, FK_TipoNomina as tipo_nomina FROM tblRH_EK_Puestos WHERE puesto > 0 AND descripcion NOT LIKE '%NO USA%'",
            //    parametros = new { cC }
            //}).ToList();
            //anterior fin

            using (var ctx = new MainContext())
            {
                var lst = ctx.Select<tblRH_CatPuestos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT
                                    puesto,
                                    @cC,
                                    descripcion,
                                    FK_TipoNomina AS tipo_nomina
                                FROM
                                    tblRH_EK_Puestos
                                WHERE
                                    puesto > 0 AND
                                    descripcion NOT LIKE '%NO USA%' AND
                                    descripcion NOT LIKE '%NOUSA%' AND
                                    registroActivo = 1",
                    parametros = new { cC }
                }).ToList();

                return lst;
            }
        }

        public AditivaPersonalPlantillaDTO ObtenerPlantilla(string cC)
        {
            //var odbc = new OdbcConsultaDTO()
            //{
            //    consulta =  "SELECT id_plantilla FROM sn_plantilla_personal WHERE cc = ?",
            //    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = cC } }
            //};
            //var lst = _contextEnkontrol.Select<AditivaPersonalPlantillaDTO>(EnkontrolAmbienteEnum.Rh, odbc);

            //anterior
            //var lst = _context.tblRH_EK_Plantilla_Personal.Where(x => x.cc == cC).Select(x => new AditivaPersonalPlantillaDTO
            //{
            //    id_plantilla = x.id_plantilla
            //}).ToList();
            //anterior fin

            using (var ctx = new MainContext())
            {
                var lst = ctx.tblRH_TAB_PlantillasPersonal.Where(x => x.cc == cC && x.registroActivo).Select(x => new AditivaPersonalPlantillaDTO
                {
                    id_plantilla = x.id
                }).ToList();

                return lst.LastOrDefault();
            }
        }
        //carga cc
        public List<ComboDTO> getListaCC()
        {
            if (vSesiones.sesionEmpresaActual == 1)
                return (List<ComboDTO>)_contextEnkontrol.Where("SELECT descripcion as Value, cc as Text FROM cc where st_ppto!='T'").ToObject<List<ComboDTO>>();
            else
                return (List<ComboDTO>)ContextArrendadora.Where("SELECT DISTINCT area AS Text, descripcion AS Value FROM si_area_cuenta ORDER BY area").ToObject<List<ComboDTO>>();
        }
        //raguilar 23/11/17 consulta Aditivainfo
        public AditivaPersonal getInfoAditiva(string puestoDescripcion, string CentroCostos)
        {
            AditivaPersonal objAditivaPersonal = new AditivaPersonal();
            id_plantilla = ObtenerPlantilla(CentroCostos).id_plantilla;
            //string query = "SELECT * FROM  sn_plantilla_puesto AS a";
            //query += " INNER JOIN si_puestos AS b";
            //query += " ON a.puesto= b.puesto";
            //query += " WHERE id_plantilla='" + id_plantilla + "' AND b.descripcion='" + puestoDescripcion + "'";
            try
            {
                //var result = (IList<AditivaPersonal>)ContextEnKontrolNomina.Where(query).ToObject<IList<AditivaPersonal>>();
                var result = _context.Select<AditivaPersonal>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = "SELECT * FROM tblRH_EK_Plantilla_Puesto AS a INNER JOIN tblRH_EK_Puestos AS b ON a.puesto = b.puesto WHERE id_plantilla = @paramPlantilla AND b.descripcion = @paramDesc",
                    parametros = new { paramPlantilla = id_plantilla, paramDesc = puestoDescripcion }
                }).ToList();
                foreach (var item in result)
                {
                    objAditivaPersonal.cantidad = item.cantidad;
                    objAditivaPersonal.puesto = item.puesto;
                    objAditivaPersonal.id_plantilla = item.id_plantilla;
                }
                //string Altas = "SELECT * FROM  sn_empleados WHERE puesto='" + objAditivaPersonal.puesto + "' AND cc_contable='" + CentroCostos + "' AND estatus_empleado='a'";
                try
                {
                    //var result2 = (IList<AditivaPersonal>)ContextEnKontrolNomina.Where(Altas).ToObject<IList<AditivaPersonal>>();
                    var result2 = _context.Select<AditivaPersonal>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = "SELECT * FROM tblRH_EK_Empleados WHERE puesto = @paramPuesto AND cc_contable = @paramCC AND estatus_empleado = 'A'",
                        parametros = new { paramPuesto = objAditivaPersonal.puesto, paramCC = CentroCostos }
                    }).ToList();
                    objAditivaPersonal.altas = result2.Count;
                }
                catch (Exception)
                {
                    objAditivaPersonal.altas = 0;

                }
                //string cantidad = "SELECT * FROM  sn_plantilla_aditiva WHERE puesto='" + objAditivaPersonal.puesto + "' AND id_plantilla='" + objAditivaPersonal.id_plantilla + "' and estatus='A'";
                try
                {
                    //var result3 = (IList<AditivaPersonal>)ContextEnKontrolNomina.Where(cantidad).ToObject<IList<AditivaPersonal>>();
                    var result3 = _context.Select<AditivaPersonal>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = "SELECT * FROM tblRH_EK_Plantilla_Aditiva WHERE puesto = @paramPuesto AND id_plantilla = @paramPlantilla AND estatus = 'A'",
                        parametros = new { paramPuesto = objAditivaPersonal.puesto, paramPlantilla = objAditivaPersonal.id_plantilla }
                    }).ToList();
                    int contador = 0;
                    foreach (var item in result3)
                    {
                        //objAditivaPersonal.altas = item.altas;
                        contador = item.cantidad + contador;

                    }

                    objAditivaPersonal.cantidad = (objAditivaPersonal.cantidad + contador);

                }
                catch (Exception)
                {

                }
            }
            catch (Exception)
            {
                throw;
            }
            return objAditivaPersonal;
        }
        public tblRH_AditivaDeductiva GuardarAditivaDeduc(tblRH_AditivaDeductiva objAditivaDet)
        {
            try
            {
                if (objAditivaDet.id == 0)
                {
                    SaveEntity(objAditivaDet, (int)BitacoraEnum.AditivaPersonal);
                }
                else
                {
                    var temp = _context.tblRH_AditivaDeductiva.FirstOrDefault(x => x.id == objAditivaDet.id);
                    temp.id = objAditivaDet.id;
                    temp.cCid = objAditivaDet.cCid;
                    temp.cC = objAditivaDet.cC;
                    temp.aprobado = objAditivaDet.aprobado;
                    temp.fechaCaptura = objAditivaDet.fechaCaptura;
                    temp.fecha_Alta = objAditivaDet.fecha_Alta;
                    temp.folio = objAditivaDet.folio;
                    temp.rechazado = objAditivaDet.rechazado;
                    Update(temp, temp.id, (int)BitacoraEnum.AditivaPersonal);
                }
            }
            catch (Exception o_O)
            {
                return new tblRH_AditivaDeductiva();
            }
            return objAditivaDet;
        }
        public List<tblRH_AditivaDeductiva> GetListAditivaDeducPersonal()
        {
            var result = new List<tblRH_AditivaDeductiva>();
            try
            {
                result = _context.tblRH_AditivaDeductiva.Where(x => x.id != 0).ToList();
            }
            catch (Exception)
            {
                result = new List<tblRH_AditivaDeductiva>();
            }
            return result;
        }
        /// <summary>
        /// Consulta de tblRH_AditivaDeductiva
        /// </summary>
        /// <param name="id">Id de adictiva</param>
        /// <param name="cc">centro de costos</param>
        /// <param name="folio">folio</param>
        /// <param name="estado">Estatus</param>
        /// <returns>Lista de adictivas</returns>
        public List<tblRH_AditivaDeductiva> getListAditivaDeductivaPendientes(int id, string cc, string folio, int estado)
        {
            var result = new List<tblRH_AditivaDeductiva>();
            var usuario = vSesiones.sesionUsuarioDTO;
            var ccUsuario = !string.IsNullOrEmpty(vSesiones.sesionUsuarioDTO.cc)?vSesiones.sesionUsuarioDTO.cc.Equals("012"):false;
            var esAdmin = usuario.perfil.Equals("Administrador") && ccUsuario;
            var ud = new UsuarioDAO();
            var rh = ud.getViewAction(vSesiones.sesionCurrentView, "VerTodoFormato");
            var esPermiso = ud.getViewAction(vSesiones.sesionCurrentView, "EditarTodoAditiva");
            //var esPermiso = usuario.permisos2.Count > 0 && usuario.permisos2.Any(a => a.menu.id.Equals(vSesiones.sesionCurrentView) && a.accion.Any(aa => aa.Accion.Equals("EditarTodoAditiva")));
            if (id > 0)
            {
                result = _context.tblRH_AditivaDeductiva.Where(x => x.id == id).ToList();
            }
            else if (usuario.id == 4 || usuario.id == 1164 || usuario.id == 1063)
            {
                var dataBase = _context.tblRH_AutorizacionAditivaDeductiva.Where(x => x.clave_Aprobador == usuario.id );
                if (dataBase != null) {
                    switch (estado)
                    {
                        case 1:
                            {
                                var dataTemp = dataBase.Where(x=>x.autorizando).Select(x=>x.id_AditivaDeductiva).ToList();
                                result = _context.tblRH_AditivaDeductiva.Where(x => dataTemp.Contains(x.id) && (cc.Equals("0") ? true : x.cCid.Equals(cc)) && (folio.Equals("0") ? true : x.folio.Equals(folio))).ToList();
                                break;
                            }
                        case 2:
                            {
                                var dataTemp = dataBase.Where(x => x.estatus).Select(x => x.id_AditivaDeductiva).ToList();
                                result = _context.tblRH_AditivaDeductiva.Where(x => dataTemp.Contains(x.id) && x.aprobado && (cc.Equals("0") ? true : x.cCid.Equals(cc)) && (folio.Equals("0") ? true : x.folio.Equals(folio))).ToList();
                                break;
                            };
                        case 3:
                            {
                                var dataTemp = dataBase.Select(x => x.id_AditivaDeductiva).ToList();
                                result = _context.tblRH_AditivaDeductiva.Where(x => dataTemp.Contains(x.id) && x.rechazado && (cc.Equals("0") ? true : x.cCid.Equals(cc)) && (folio.Equals("0") ? true : x.folio.Equals(folio))).ToList();
                                break;
                            }
                        default:
                            {
                                var dataTemp = dataBase.Select(x => x.id_AditivaDeductiva).ToList();
                                result = _context.tblRH_AditivaDeductiva.Where(x => dataTemp.Contains(x.id) && (cc.Equals("0") ? true : x.cCid.Equals(cc)) && (folio.Equals("0") ? true : x.folio.Equals(folio))).ToList();
                                break;
                            }
                    }
                }

            }
            else if (esAdmin || esPermiso || rh)
            {
                result = _context.tblRH_AditivaDeductiva.ToList()
                    .Where(a => (cc.Equals("0") ? true : cc == a.cCid) &&
                                (folio.Equals("0") ? true : a.folio.Contains(folio)) &&
                                (estado == 1 ? (!a.aprobado && !a.rechazado) : (estado == 2 ? (a.aprobado) : estado == 3 ? a.rechazado : true)))
                    .ToList();
            }
            else
            {
                var lstCC = (from a in _context.tblRH_AditivaDeductiva
                             join b in _context.tblRH_AutorizacionAditivaDeductiva
                             on a.id equals b.id_AditivaDeductiva
                             select a)
                             .Where(a => (cc.Equals("0") ? true : cc == a.cCid) &&
                                (folio.Equals("0") ? true : a.folio.Contains(folio)) &&
                                (estado == 1 ? (!a.aprobado && !a.rechazado) : (estado == 2 ? (a.aprobado) : estado == 3 ? a.rechazado : true)))
                             .Distinct().ToList();
                if (estado == 1)
                {
                    var lstAuth = _context.tblRH_AutorizacionAditivaDeductiva.ToList()
                        .Where(w => lstCC.Any(a => a.id == w.id_AditivaDeductiva))
                        .Where(w => w.autorizando)
                        .Where(w => w.clave_Aprobador == usuario.id)
                        .ToList();
                    lstCC = lstCC.Where(w => lstAuth.Any(a => a.id_AditivaDeductiva == w.id) ||w.usuarioCap==vSesiones.sesionUsuarioDTO.id).ToList();
                }
                result.AddRange(lstCC);
            }
            return result;
        }
        public void eliminarFormato(int formatoID)
        {

            var alertas = _context.tblP_Alerta.Where(x => x.objID == formatoID && x.sistemaID == (int)SistemasEnum.RH && x.msj.Contains("Firma-Formato Aditiva-Deductiva")).ToList();
            _context.tblP_Alerta.RemoveRange(alertas);
            _context.SaveChanges();

            var autorizantes = _context.tblRH_AutorizacionAditivaDeductiva.Where(x => x.id_AditivaDeductiva == formatoID).ToList();
            _context.tblRH_AutorizacionAditivaDeductiva.RemoveRange(autorizantes);
            _context.SaveChanges();

            var formato = _context.tblRH_AditivaDeductiva.FirstOrDefault(x => x.id == formatoID);
            _context.tblRH_AditivaDeductiva.Remove(formato);
            _context.SaveChanges();

            var formatoDetalle = _context.tblRH_AditivaDeductivaDet.Where(x => x.id_AditivaDeductiva == formatoID).ToList();
            _context.tblRH_AditivaDeductivaDet.RemoveRange(formatoDetalle);
            _context.SaveChanges();
        }
        public tblRH_AditivaDeductiva getFormatoAditivaDeductivaByID(int idFormatoAditiva)
        {
            return _context.tblRH_AditivaDeductiva.FirstOrDefault(x => x.id.Equals(idFormatoAditiva));
        }
        public Dictionary<string, object> AutorizarPlantilla(int plantillaID, int autorizacion, int estatus, string comentario)
        {
            bool flag = false;
            var resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var aprobadoAnteriormente = false;
                    var p = _context.tblRH_AditivaDeductiva.FirstOrDefault(x => x.id == plantillaID);
                    aprobadoAnteriormente = p.aprobado;
                    var data = _context.tblRH_AutorizacionAditivaDeductiva.FirstOrDefault(x => x.id == autorizacion);
                    var firna = "--" + plantillaID.ToString() + "|" + DateTime.Now.ToString("ddMMyyyy|HHmm") + "|" + (int)BitacoraEnum.AditivaPersonal + "|" + data.clave_Aprobador + "--";

                    var est = estatus == 2 ? true : false;
                    data.estatus = est;
                    data.fechafirma = DateTime.Now.ToString();
                    data.autorizando = false;
                    data.firma = firna;
                    _context.SaveChanges();
                    if (estatus == (int)EstatusRegEnum.RECHAZADO)
                    {
                        p.aprobado = false;
                        p.rechazado = true;
                        data.firma = "RECHAZADO";
                        data.rechazado = true;
                        data.comentario = comentario;
                        _context.SaveChanges();

                        var alerts = _context.tblP_Alerta.Where(x => x.sistemaID == (int)SistemasEnum.RH && x.objID == plantillaID && x.msj.Contains("Aditiva")).ToList();
                        alerts.ForEach(x => x.visto = true);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var siguiente = _context.tblRH_AutorizacionAditivaDeductiva.Where(x => x.id_AditivaDeductiva == plantillaID && x.estatus == false).OrderBy(x => x.orden).FirstOrDefault();
                        if (siguiente != null)
                        {
                            if (siguiente.clave_Aprobador == data.clave_Aprobador)
                            {
                                siguiente.estatus = true;
                                siguiente.fechafirma = DateTime.Now.ToString();
                                siguiente.firma = firna;
                                _context.SaveChanges();
                                var siguiente2 = _context.tblRH_AutorizacionAditivaDeductiva.Where(x => x.id_AditivaDeductiva == plantillaID && x.estatus == false).OrderBy(x => x.orden).FirstOrDefault();
                                if (siguiente2 != null)
                                {
                                    if (siguiente2.clave_Aprobador == data.clave_Aprobador)
                                    {
                                        siguiente2.estatus = true;
                                        siguiente2.fechafirma = DateTime.Now.ToString();
                                        siguiente2.firma = firna;
                                        _context.SaveChanges();
                                        var siguiente3 = _context.tblRH_AutorizacionAditivaDeductiva.Where(x => x.id_AditivaDeductiva == plantillaID && x.estatus == false).OrderBy(x => x.orden).FirstOrDefault();
                                        if (siguiente3 != null)
                                        {
                                            if (siguiente3.clave_Aprobador == data.clave_Aprobador)
                                            {
                                                siguiente3.estatus = true;
                                                siguiente3.fechafirma = DateTime.Now.ToString();
                                                siguiente3.firma = firna;
                                                _context.SaveChanges();
                                                var siguiente4 = _context.tblRH_AutorizacionAditivaDeductiva.Where(x => x.id_AditivaDeductiva == plantillaID && x.estatus == false).OrderBy(x => x.orden).FirstOrDefault();
                                                if (siguiente4 == null)
                                                {
                                                    p.aprobado = true;
                                                    p.rechazado = false;
                                                    _context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                siguiente3.autorizando = true;
                                                _context.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            p.aprobado = true;
                                            p.rechazado = false;
                                            _context.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        siguiente2.autorizando = true;
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    p.aprobado = true;
                                    p.rechazado = false;
                                    _context.SaveChanges();
                                }
                            }
                            else
                            {
                                siguiente.autorizando = true;
                                _context.SaveChanges();
                            }

                        }
                        else
                        {
                            p.aprobado = true;
                            p.rechazado = false;
                            _context.SaveChanges();
                        }
                    }

                    #region guardar enkontrol
                    if (p.aprobado && !aprobadoAnteriormente)
                    {
                        var aditivasSigoplan = _context.tblRH_AditivaDeductivaDet.Where(x => x.id_AditivaDeductiva == p.id && x.estado).ToList();

                        var plantillaEK = _context.tblRH_TAB_PlantillasPersonal.FirstOrDefault(x => x.cc == p.cCid && x.registroActivo && x.plantillaAutorizada == Core.Enum.RecursosHumanos.Tabuladores.EstatusGestionAutorizacionEnum.AUTORIZADO);

                        if (plantillaEK != null)
                        {
                            var solicitanteAutorizador = _context.tblRH_AutorizacionAditivaDeductiva.Where(x => x.id_AditivaDeductiva == data.id_AditivaDeductiva && (x.responsable.Contains("Solicitante") || x.responsable.Contains("VoBo"))).ToList();
                            var solicitante = solicitanteAutorizador.First(x => x.responsable.Contains("Solicitante"));
                            var vobo = solicitanteAutorizador.First(x => x.responsable.Contains("VoBo"));

                            var cveSolicitante = _context.tblP_Usuario.FirstOrDefault(x => x.id == solicitante.clave_Aprobador);
                            var cveVobo = _context.tblP_Usuario.FirstOrDefault(x => x.id == vobo.clave_Aprobador);

                            foreach (var aditivaSP in aditivasSigoplan.GroupBy(x => x.puesto))
                            {
                                var puestoEK = new List<int>();
                                if (aditivaSP.First().idPuesto.HasValue)
                                {
                                    var idP = aditivaSP.First().idPuesto.Value;
                                    puestoEK = _context.tblRH_EK_Puestos.Where(x => x.puesto == idP).Select(x => x.puesto).ToList();
                                }
                                else
                                {
                                    var desc = aditivaSP.First().puesto.Trim();
                                    puestoEK = _context.tblRH_EK_Puestos.Where(x => x.descripcion == desc && x.registroActivo).Select(x => x.puesto).ToList();
                                }

                                if (puestoEK != null)
                                {
                                    var nuevaAditivaEK = new sn_plantilla_aditivaDTO();
                                    nuevaAditivaEK.id_plantilla = 0;
                                    nuevaAditivaEK.cc = p.cCid;
                                    nuevaAditivaEK.puesto = puestoEK.First();
                                    nuevaAditivaEK.tipo = aditivaSP.First().aditiva > 0 ? "A" : "D";
                                    nuevaAditivaEK.cantidad = aditivaSP.First().aditiva > 0 ? aditivaSP.First().aditiva : (aditivaSP.First().deductiva * -1);
                                    if (cveSolicitante != null)
                                    {
                                        nuevaAditivaEK.solicita = Convert.ToInt32(cveSolicitante.cveEmpleado);
                                    }
                                    else
                                    {
                                        throw new Exception("No se encontro la clave_empleado en sigoplan del solicitante");
                                    }
                                    nuevaAditivaEK.fecha_solicita = p.fechaCaptura;
                                    if (cveVobo != null)
                                    {
                                        nuevaAditivaEK.autoriza = Convert.ToInt32(cveVobo.cveEmpleado);
                                    }
                                    else
                                    {
                                        throw new Exception("No se encontro la clave_empleado en sigoplan del vobo");
                                    }
                                    nuevaAditivaEK.fecha_autoriza = DateTime.Now;
                                    nuevaAditivaEK.visto_bueno = 1;
                                    nuevaAditivaEK.fecha_visto_bueno = null;
                                    nuevaAditivaEK.estatus = "A";
                                    nuevaAditivaEK.fecha = DateTime.Now;
                                    nuevaAditivaEK.observaciones = string.IsNullOrEmpty(aditivaSP.First().justificacion) ? "" : aditivaSP.First().justificacion.Substring(0, aditivaSP.First().justificacion.Length > 100 ? 99 : aditivaSP.First().justificacion.Length);
                                    
                                    var tblPlantillaAditiva = new tblRH_EK_Plantilla_Aditiva();
                                    tblPlantillaAditiva.id_plantilla = nuevaAditivaEK.id_plantilla;
                                    tblPlantillaAditiva.cc = nuevaAditivaEK.cc;
                                    tblPlantillaAditiva.puesto = nuevaAditivaEK.puesto;
                                    tblPlantillaAditiva.tipo = nuevaAditivaEK.tipo;
                                    tblPlantillaAditiva.cantidad = nuevaAditivaEK.cantidad;
                                    tblPlantillaAditiva.solicita = nuevaAditivaEK.solicita;
                                    tblPlantillaAditiva.fecha_solicita = nuevaAditivaEK.fecha_solicita;
                                    tblPlantillaAditiva.autoriza = nuevaAditivaEK.autoriza;
                                    tblPlantillaAditiva.fecha_autoriza = nuevaAditivaEK.fecha_autoriza;
                                    tblPlantillaAditiva.visto_bueno = nuevaAditivaEK.visto_bueno;
                                    tblPlantillaAditiva.estatus = nuevaAditivaEK.estatus;
                                    tblPlantillaAditiva.fecha = nuevaAditivaEK.fecha;
                                    tblPlantillaAditiva.observaciones = nuevaAditivaEK.observaciones;
                                    _context.tblRH_EK_Plantilla_Aditiva.Add(tblPlantillaAditiva);
                                    _context.SaveChanges();

                                    if (!_context.tblRH_TAB_PlantillasPersonalDet.Any(x => x.FK_Plantilla == plantillaEK.id && x.FK_Puesto == tblPlantillaAditiva.puesto))
                                    {
                                        var ppuesto = new tblRH_TAB_PlantillasPersonalDet();
                                        ppuesto.FK_Plantilla = plantillaEK.id;
                                        ppuesto.FK_Puesto = tblPlantillaAditiva.puesto;
                                        ppuesto.personalNecesario = 0;
                                        ppuesto.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                        ppuesto.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                        ppuesto.fechaCreacion = DateTime.Now;
                                        ppuesto.fechaModificacion = ppuesto.fechaCreacion;
                                        ppuesto.registroActivo = true;
                                        _context.tblRH_TAB_PlantillasPersonalDet.Add(ppuesto);
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    throw new Exception("No se encontro información del puesto [" + aditivaSP.First().puesto + "]");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("No se encontro información de la plantilla [" + p.cCid + "]");
                        }
                        //anterior
                        //var aditivasSigoplan = _context.tblRH_AditivaDeductivaDet.Where(x => x.id_AditivaDeductiva == p.id && x.estado).ToList();

                        ////Obtenemos el id de la plantilla en enkontrol
                        ////var query_plantilla = new OdbcConsultaDTO();
                        ////query_plantilla.consulta = "SELECT id_plantilla FROM sn_plantilla_personal WHERE cc = ? AND estatus = 'A'";
                        ////query_plantilla.parametros.Add(new OdbcParameterDTO
                        ////{
                        ////    nombre = "cc",
                        ////    tipo = OdbcType.NVarChar,
                        ////    valor = p.cCid
                        ////});
                        ////var plantillaEK = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolRh, query_plantilla).FirstOrDefault();
                        //var plantillaEK = _context.tblRH_EK_Plantilla_Personal.Where(x => x.cc == p.cCid && x.estatus == "A").Select(x => x.id_plantilla).FirstOrDefault();

                        //if (plantillaEK != null)
                        //{
                        //    //var data = _context.tblRH_AutorizacionAditivaDeductiva.FirstOrDefault(x => x.id == autorizacion);
                        //    var solicitanteAutorizador = _context.tblRH_AutorizacionAditivaDeductiva.Where(x => x.id_AditivaDeductiva == data.id_AditivaDeductiva && (x.responsable.Contains("Solicitante") || x.responsable.Contains("VoBo"))).ToList();
                        //    var solicitante = solicitanteAutorizador.First(x => x.responsable.Contains("Solicitante"));
                        //    var vobo = solicitanteAutorizador.First(x => x.responsable.Contains("VoBo"));

                        //    //var cveSolicitante = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == solicitante.clave_Aprobador);
                        //    //var cveVobo = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vobo.clave_Aprobador);
                        //    var cveSolicitante = _context.tblP_Usuario.FirstOrDefault(x => x.id == solicitante.clave_Aprobador);
                        //    var cveVobo = _context.tblP_Usuario.FirstOrDefault(x => x.id == vobo.clave_Aprobador);

                        //    //Por cada aditiva de la recien autorización se realiza el guardado en enkontrol.
                        //    foreach (var aditivaSP in aditivasSigoplan.GroupBy(x => x.puesto))
                        //    {
                        //        var puestoEK = new List<int>();
                        //        //var query_puesto = new OdbcConsultaDTO();
                        //        if (aditivaSP.First().idPuesto.HasValue)
                        //        {
                        //            //    query_puesto.consulta = "SELECT puesto FROM si_puestos WHERE puesto = ?";
                        //            //    query_puesto.parametros.Add(new OdbcParameterDTO
                        //            //    {
                        //            //        nombre = "puesto",
                        //            //        tipo = OdbcType.Int,
                        //            //        valor = aditivaSP.First().idPuesto.Value
                        //            //    });
                        //            var idP = aditivaSP.First().idPuesto.Value;
                        //            puestoEK = _context.tblRH_EK_Puestos.Where(x => x.puesto == idP).Select(x => x.puesto).ToList();
                        //        }
                        //        else
                        //        {
                        //            //    query_puesto.consulta = "SELECT puesto FROM si_puestos WHERE descripcion = ?";
                        //            //    query_puesto.parametros.Add(new OdbcParameterDTO
                        //            //    {
                        //            //        nombre = "descripcion",
                        //            //        tipo = OdbcType.NVarChar,
                        //            //        valor = aditivaSP.First().puesto.Trim()
                        //            //    });
                        //            var desc = aditivaSP.First().puesto.Trim();
                        //            puestoEK = _context.tblRH_EK_Puestos.Where(x => x.descripcion == desc).Select(x => x.puesto).ToList();
                        //        }
                        //        //var puestoEK = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolRh, query_puesto).FirstOrDefault();


                        //        if (puestoEK != null)
                        //        {
                        //            var nuevaAditivaEK = new sn_plantilla_aditivaDTO();
                        //            nuevaAditivaEK.id_plantilla = (int)plantillaEK;
                        //            nuevaAditivaEK.cc = p.cCid;
                        //            nuevaAditivaEK.puesto = puestoEK.First();
                        //            nuevaAditivaEK.tipo = aditivaSP.First().aditiva > 0 ? "A" : "D";
                        //            nuevaAditivaEK.cantidad = aditivaSP.First().aditiva > 0 ? aditivaSP.First().aditiva : (aditivaSP.First().deductiva * -1);
                        //            if (cveSolicitante != null)
                        //            {
                        //                nuevaAditivaEK.solicita = Convert.ToInt32(cveSolicitante.cveEmpleado);
                        //            }
                        //            else
                        //            {
                        //                throw new Exception("No se encontro la clave_empleado en sigoplan del solicitante");
                        //            }
                        //            nuevaAditivaEK.fecha_solicita = p.fechaCaptura;
                        //            if (cveVobo != null)
                        //            {
                        //                nuevaAditivaEK.autoriza = Convert.ToInt32(cveVobo.cveEmpleado);
                        //            }
                        //            else
                        //            {
                        //                throw new Exception("No se encontro la clave_empleado en sigoplan del vobo");
                        //            }
                        //            nuevaAditivaEK.fecha_autoriza = DateTime.Now;
                        //            nuevaAditivaEK.visto_bueno = 1;
                        //            nuevaAditivaEK.fecha_visto_bueno = null;
                        //            nuevaAditivaEK.estatus = "A";
                        //            nuevaAditivaEK.fecha = DateTime.Now;
                        //            nuevaAditivaEK.observaciones = string.IsNullOrEmpty(aditivaSP.First().justificacion) ? "" : aditivaSP.First().justificacion.Substring(0, aditivaSP.First().justificacion.Length > 100 ? 99 : aditivaSP.First().justificacion.Length);

                        //            //var ultimaAditiva = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolRh, new OdbcConsultaDTO {
                        //            //    consulta = "SELECT TOP 1 id FROM sn_plantilla_aditiva ORDER BY id DESC"
                        //            //}).FirstOrDefault();
                        //            //var siguienteAditivaId = ultimaAditiva != null ? (int)ultimaAditiva.id + 1 : 1;

                        //            var tblPlantillaAditiva = new tblRH_EK_Plantilla_Aditiva();
                        //            tblPlantillaAditiva.id_plantilla = nuevaAditivaEK.id_plantilla;
                        //            tblPlantillaAditiva.cc = nuevaAditivaEK.cc;
                        //            tblPlantillaAditiva.puesto = nuevaAditivaEK.puesto;
                        //            tblPlantillaAditiva.tipo = nuevaAditivaEK.tipo;
                        //            tblPlantillaAditiva.cantidad = nuevaAditivaEK.cantidad;
                        //            tblPlantillaAditiva.solicita = nuevaAditivaEK.solicita;
                        //            tblPlantillaAditiva.fecha_solicita = nuevaAditivaEK.fecha_solicita;
                        //            tblPlantillaAditiva.autoriza = nuevaAditivaEK.autoriza;
                        //            tblPlantillaAditiva.fecha_autoriza = nuevaAditivaEK.fecha_autoriza;
                        //            tblPlantillaAditiva.visto_bueno = nuevaAditivaEK.visto_bueno;
                        //            tblPlantillaAditiva.estatus = nuevaAditivaEK.estatus;
                        //            tblPlantillaAditiva.fecha = nuevaAditivaEK.fecha;
                        //            tblPlantillaAditiva.observaciones = nuevaAditivaEK.observaciones;
                        //            _context.tblRH_EK_Plantilla_Aditiva.Add(tblPlantillaAditiva);
                        //            _context.SaveChanges();

                        //            if (!_context.tblRH_EK_Plantilla_Puesto.Any(x => x.id_plantilla == tblPlantillaAditiva.id_plantilla && x.puesto == tblPlantillaAditiva.puesto))
                        //            {
                        //                var ppuesto = new tblRH_EK_Plantilla_Puesto();
                        //                ppuesto.id_plantilla = tblPlantillaAditiva.id_plantilla;
                        //                ppuesto.puesto = tblPlantillaAditiva.puesto;
                        //                ppuesto.cantidad = 0;
                        //                ppuesto.observaciones = "ADITIVA";
                        //                ppuesto.check_bobo = false;
                        //                ppuesto.check_autoriza = false;
                        //                ppuesto.estatus = "A";
                        //                ppuesto.altas = 0;
                        //                ppuesto.bajas = 0;
                        //                ppuesto.cc = "";
                        //                ppuesto.fechaCreacion = DateTime.Now;
                        //                ppuesto.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                        //                ppuesto.esActivo = true;
                        //                _context.tblRH_EK_Plantilla_Puesto.Add(ppuesto);
                        //                _context.SaveChanges();
                        //            }

                        //            //                                                var query_insert = string.Format(
                        //            //                                                    @"INSERT INTO sn_plantilla_aditiva (
                        //            //                                                            id,
                        //            //                                                            id_plantilla,
                        //            //                                                            cc,
                        //            //                                                            puesto,
                        //            //                                                            tipo,
                        //            //                                                            cantidad,
                        //            //                                                            solicita,
                        //            //                                                            fecha_solicita,
                        //            //                                                            autoriza,
                        //            //                                                            fecha_autoriza,
                        //            //                                                            visto_bueno,
                        //            //                                                            fecha_visto_bueno,
                        //            //                                                            estatus,
                        //            //                                                            fecha,
                        //            //                                                            observaciones
                        //            //                                                        ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

                        //            //                                                using (var cmd = new OdbcCommand(query_insert))
                        //            //                                                {
                        //            //                                                    var parametros = cmd.Parameters;
                        //            //                                                    parametros.Add("@id", OdbcType.Int).Value = siguienteAditivaId;
                        //            //                                                    parametros.Add("@id_plantilla", OdbcType.Int).Value = nuevaAditivaEK.id_plantilla;
                        //            //                                                    parametros.Add("@cc", OdbcType.NVarChar).Value = nuevaAditivaEK.cc;
                        //            //                                                    parametros.Add("@puesto", OdbcType.Int).Value = nuevaAditivaEK.puesto;
                        //            //                                                    parametros.Add("@tipo", OdbcType.NVarChar).Value = nuevaAditivaEK.tipo;
                        //            //                                                    parametros.Add("@cantidad", OdbcType.Int).Value = nuevaAditivaEK.cantidad;
                        //            //                                                    parametros.Add("@solicita", OdbcType.Int).Value = nuevaAditivaEK.solicita;
                        //            //                                                    parametros.Add("@fecha_solicita", OdbcType.Date).Value = nuevaAditivaEK.fecha_solicita;
                        //            //                                                    parametros.Add("@autoriza", OdbcType.Int).Value = nuevaAditivaEK.autoriza;
                        //            //                                                    parametros.Add("@fecha_autoriza", OdbcType.Date).Value = nuevaAditivaEK.fecha_autoriza;
                        //            //                                                    parametros.Add("@visto_bueno", OdbcType.Int).Value = nuevaAditivaEK.visto_bueno;
                        //            //                                                    parametros.Add("@fecha_visto_bueno", OdbcType.Date).Value = nuevaAditivaEK.fecha_visto_bueno ?? (object)DBNull.Value;
                        //            //                                                    parametros.Add("@estatus", OdbcType.NVarChar).Value = nuevaAditivaEK.estatus;
                        //            //                                                    parametros.Add("@fecha", OdbcType.Date).Value = nuevaAditivaEK.fecha;
                        //            //                                                    parametros.Add("@observaciones", OdbcType.NVarChar).Value = nuevaAditivaEK.observaciones ?? (object)DBNull.Value;

                        //            //                                                    cmd.Connection = tranEK.Connection;
                        //            //                                                    cmd.Transaction = tranEK;
                        //            //                                                    cmd.ExecuteNonQuery();
                        //            //                                                }
                        //        }
                        //        else
                        //        {
                        //            throw new Exception("No se encontro información del puesto [" + aditivaSP.First().puesto + "]");
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    throw new Exception("No se encontro información de la plantilla [" + p.cCid + "]");
                        //}anterior fin
                    }
                    #endregion

                    dbContextTransaction.Commit();

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }
        public bool EnviarCorreo(int plantillaID, int autorizacion, int estatus)
        {
            UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
            bool enviado = false;
            try
            {
                var plantilla = GetPlantillaSP(plantillaID);
                var aut = _context.tblRH_AutorizacionAditivaDeductiva.FirstOrDefault(x => x.id == autorizacion);
                var usuarioSolicito = _context.tblP_Usuario.FirstOrDefault(x => x.id == plantilla.usuarioCap);
                var usuarioEnvia = vSesiones.sesionUsuarioDTO;
                var downloadPDF = vSesiones.downloadPDF;
                var usuariosFormato = GetAutorizadores(plantillaID);
                var sig = _context.tblRH_AutorizacionAditivaDeductiva.FirstOrDefault(x => x.id_AditivaDeductiva == plantillaID && x.autorizando);
                if (sig != null)
                {
                    var a = _context.tblP_Alerta.Where(x => x.sistemaID == (int)SistemasEnum.RH && x.moduloID == (int)BitacoraEnum.AditivaPersonal && x.objID == plantillaID).ToList();
                    if (a.Count > 0)
                    {
                        foreach (var i in a)
                        {
                            i.visto = true;
                        }
                        _context.SaveChanges();
                        var b = new tblP_Alerta();
                        b.userEnviaID = 1;
                        b.userRecibeID = sig.clave_Aprobador;
                        b.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                        b.visto = false;
                        b.url = "/Administrativo/AditivaPersonal/Index?obj=" + plantillaID;
                        b.objID = plantillaID;
                        b.obj = "";
                        b.msj = "Firma-Formato Aditiva-Deductiva " + plantilla.folio;
                        b.documentoID = 0;
                        b.sistemaID = (int)SistemasEnum.RH;
                        b.moduloID = (int)BitacoraEnum.AditivaPersonal;
                        _context.tblP_Alerta.Add(b);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var b = new tblP_Alerta();
                        b.userEnviaID = 1;
                        b.userRecibeID = sig.clave_Aprobador;
                        b.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                        b.visto = false;
                        b.url = "/Administrativo/AditivaPersonal/Index?obj=" + plantillaID;
                        b.objID = plantillaID;
                        b.obj = "";
                        b.msj = "Firma-Formato Aditiva-Deductiva " + plantilla.folio;
                        b.documentoID = 0;
                        b.sistemaID = (int)SistemasEnum.RH;
                        b.moduloID = (int)BitacoraEnum.AditivaPersonal;
                        _context.tblP_Alerta.Add(b);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    var a = _context.tblP_Alerta.Where(x => x.sistemaID == (int)SistemasEnum.RH && x.moduloID == (int)BitacoraEnum.AditivaPersonal && x.objID == plantillaID).ToList();
                    a.ForEach(x => x.visto = true);
                    _context.SaveChanges();
                }
                List<string> CorreoEnviar = new List<string>();
                string AsuntoCorreo = @"<html>
                                            <head>
                                                <style>
                                                    table {
                                                        font-family: arial, sans-serif;
                                                        border-collapse: collapse;
                                                        width: 100%;
                                                    }

                                                    td, th {
                                                        border: 1px solid #dddddd;
                                                        text-align: left;
                                                        padding: 8px;
                                                    }

                                                    tr:nth-child(even) {
                                                        background-color: #dddddd;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                                <div class=WordSection1>
                                                    <p class=MsoNormal>
                                                        Buen día <o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>";
                var folioID = plantillaID;
                var folio = plantillaID.ToString().PadLeft(6, '0');
                if (plantilla.aprobado==true || plantilla.rechazado == true)
                {
                    if (plantilla.aprobado == true)
                    {
                        AsuntoCorreo += @" <p class=MsoNormal>
                                                        Se informa que se finalizo correctamente el proceso de autorización del Formato de Aditiva-Deductiva de Personal con Folio: &#8220;" + folio + @"&#8221 para el CC " + plantilla.cC + " por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                    </p>";
                    }
                    else if (plantilla.rechazado == true)
                    {
                        AsuntoCorreo += @" <p class=MsoNormal>
                                                Se informa que el Formato de Aditiva-Deductiva de Personal con Folio: &#8220;" + folio + @"&#8221 para el CC " + plantilla.cC + " fue rechazado por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                            </p>";
                        AsuntoCorreo += @" <p class=MsoNormal>
                                                    <strong>La razón del rechazo fue: </strong> " + HttpUtility.HtmlEncode((aut.comentario ?? "")) + @"<o:p></o:p>
                                                </p>";
                    }
                }
                else if (autorizacion == 0)
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                                        Se informa que se registro una nuevo Formato de Aditiva-Deductiva de Personal con Folio: &#8220;" + folio + @"&#8221 para el CC " + plantilla.cC + " por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                    </p>";
                }
                else if (estatus == (int)EstatusRegEnum.AUTORIZADO)
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                                        Se informa que fue realizada una autorización en el Formato de Aditiva-Deductiva de Personal con Folio: &#8220;" + folio + @"&#8221 para el CC " + plantilla.cC + " por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                    </p>";
                }
                else if (estatus == (int)EstatusRegEnum.RECHAZADO)
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                            Se informa que el Formato de Aditiva-Deductiva de Personal con Folio: &#8220;" + folio + @"&#8221 para el CC " + plantilla.cC + " fue rechazado por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                        </p>";
                    AsuntoCorreo += @" <p class=MsoNormal>
                                                <strong>La razón del rechazo fue: </strong> " + HttpUtility.HtmlEncode((aut.comentario ?? "")) + @"<o:p></o:p>
                                            </p>";
                }
                AsuntoCorreo += @" <p class=MsoNormal>
                                    <o:p>&nbsp;</o:p>
                                </p><br/><br/>
                                    <table>
                                        <thead>
                                            <tr>
                                            <th>Nombre Autorizador </th>
                                            <th>Tipo</th>
                                            <th>Autorizó</th>
                                            </tr></thead>
                                        <tbody>";

                var excepcionesCorreo = usuarioFactoryServices.getUsuarioService().getPermisosAutorizaCorreo(1);

                List<int> excepcionesCorreoIDs = new List<int>();

                if (excepcionesCorreo.Count > 0)
                {
                    excepcionesCorreoIDs.AddRange(excepcionesCorreo.Select(x => x.usuarioID));
                }

                if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    #region PERU
                    foreach (var item in usuariosFormato)
                    {
                        string puestoAprobador = item.puestoAprobador;
                        if (puestoAprobador.Contains("Responsable del Centro de Costos"))
                            item.orden = 1;
                        else if (puestoAprobador.Contains("Capital Humano"))
                            item.orden = 2;
                        else if (puestoAprobador.Contains("Gerente/SubDirector/Director de Área"))
                            item.orden = 3;
                        else if (puestoAprobador.Contains("Director de Línea de Negocios"))
                            item.orden = 4;
                        else if (puestoAprobador.Contains("Alta Dirección"))
                            item.orden = 5;
                    }

                    foreach (var i in usuariosFormato.OrderBy(o => o.orden))
                    {
                        AsuntoCorreo += @"<tr>
                                <td>" + i.nombre_Aprobador + "</td>" +
                                        "<td>" + i.puestoAprobador + "</td>" +
                                            getEstatus(i.estatus, i.rechazado, i.autorizando) +
                                        "</tr>";

                        var usuarioCorreo = usuarioFactoryServices.getUsuarioService().ListUsersById(i.clave_Aprobador).FirstOrDefault();

                        if (i.autorizando)
                        {
                            CorreoEnviar.Add(usuarioCorreo.correo);
                        }
                        else
                        {
                            if (!excepcionesCorreoIDs.Contains(i.clave_Aprobador))
                            {
                                CorreoEnviar.Add(usuarioCorreo.correo);
                            }
                        }
                    }
                    #endregion
                } else if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                {
                    #region COLOMBIA
                    foreach (var item in usuariosFormato)
                    {
                        string puestoAprobador = item.puestoAprobador;
                        if (puestoAprobador.Contains("Responsable del Centro de Costos"))
                            item.orden = 1;
                        else if (puestoAprobador.Contains("Capital Humano"))
                            item.orden = 2;
                        else if (puestoAprobador.Contains("Gerente/SubDirector/Director de Área"))
                            item.orden = 3;
                        else if (puestoAprobador.Contains("Director de Línea de Negocios"))
                            item.orden = 4;
                        else if (puestoAprobador.Contains("Alta Dirección"))
                            item.orden = 5;
                    }

                    foreach (var i in usuariosFormato.OrderBy(o => o.orden))
                    {
                        AsuntoCorreo += @"<tr>
                                <td>" + i.nombre_Aprobador + "</td>" +
                                        "<td>" + i.puestoAprobador + "</td>" +
                                            getEstatus(i.estatus, i.rechazado, i.autorizando) +
                                        "</tr>";

                        var usuarioCorreo = usuarioFactoryServices.getUsuarioService().ListUsersById(i.clave_Aprobador).FirstOrDefault();

                        if (i.autorizando)
                        {
                            CorreoEnviar.Add(usuarioCorreo.correo);
                        }
                        else
                        {
                            if (!excepcionesCorreoIDs.Contains(i.clave_Aprobador))
                            {
                                CorreoEnviar.Add(usuarioCorreo.correo);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region CP/ARR
                    foreach (var i in usuariosFormato.OrderBy(o => o.orden))
                    {
                        AsuntoCorreo += @"<tr>
                                <td>" + i.nombre_Aprobador + "</td>" +
                                        "<td>" + i.puestoAprobador + "</td>" +
                                            getEstatus(i.estatus, i.rechazado, i.autorizando) +
                                        "</tr>";

                        var usuarioCorreo = usuarioFactoryServices.getUsuarioService().ListUsersById(i.clave_Aprobador).FirstOrDefault();

                        if (i.autorizando)
                        {
                            CorreoEnviar.Add(usuarioCorreo.correo);
                        }
                        else
                        {
                            if (!excepcionesCorreoIDs.Contains(i.clave_Aprobador))
                            {
                                CorreoEnviar.Add(usuarioCorreo.correo);
                            }
                        }
                    }
                    #endregion
                }

                AsuntoCorreo += @"</tbody>" +
                            @"</table>
                                    <p class=MsoNormal>
                                        <o:p>&nbsp;</o:p>
                                    </p>
                                    <p class=MsoNormal>
                                        Favor de ingresar al sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx/</a>), en el apartado de ADMINISTRACION, menú RH en la opción Formato de Aditiva-Deductiva<o:p></o:p>
                                    </p>
                                    <p class=MsoNormal>
                                        <o:p>&nbsp;</o:p>
                                    </p>
                                    <p class=MsoNormal>
                                        <o:p>&nbsp;</o:p>
                                    </p>
                                    <p class=MsoNormal>
                                        PD. Se informa que esta es una notificación autogenerada por el sistema SIGOPLAN no es necesario dar una respuesta <o:p></o:p>
                                    </p>
                                    <p class=MsoNormal>
                                        <o:p>&nbsp;</o:p>
                                    </p>
                                    <p class=MsoNormal>
                                        Gracias.<o:p></o:p>
                                    </p>
                                </div>
                            </body>
                        </html>";
                CorreoEnviar.Add(usuarioEnvia.correo);
                CorreoEnviar.Add(usuarioSolicito.correo);
                var diana = _context.tblP_Usuario.FirstOrDefault(u => u.id == 1019);
                CorreoEnviar.Add(diana.correo);
                var aranza = _context.tblP_Usuario.FirstOrDefault(u => u.id == 79552);
                CorreoEnviar.Add(aranza.correo);

                #region ADJUNTAR CH EN PROYECTO
                var lstAutorizantesCC = _context.tblRH_REC_Notificantes_Altas.Where(e => e.esActivo && e.cc == plantilla.cCid).ToList();

                foreach (var item in lstAutorizantesCC)
                {
                    var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == item.idUsuario);

                    if (objUsuario != null)
                    {
                        CorreoEnviar.Add(objUsuario.correo);
                    }
                }
                #endregion

                var tipoFormato = "FormatoDeAditivas.pdf";
                #region Remover_Gerardo Reina de seguimiento una ves autorizado
                try
                {
                    if (CorreoEnviar.Contains("g.reina@construplan.com.mx"))
                    {
                        var autorizadores = _context.tblRH_AutorizacionAditivaDeductiva.Where(x => x.id_AditivaDeductiva == plantillaID);
                        var greina = autorizadores.FirstOrDefault(x => x.clave_Aprobador == 1164);
                        if (greina != null)
                        {
                            if(greina.estatus || greina.rechazado)
                            {
                                CorreoEnviar.Remove("g.reina@construplan.com.mx");
                            }
                        }
                        else
                        {

                            CorreoEnviar.Remove("g.reina@construplan.com.mx");
                        }
                    }
                }
                catch{}
                #endregion
                List<string> prueba = new List<string>();
                //prueba.Add("angel.devora@construplan.com.mx");

#if DEBUG
                CorreoEnviar = new List<string>() { "miguel.buzani@construplan.com.mx"};
#endif

                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0} - Alerta de Autorizaciones 'ADITIVAS/DEDUCTIVAS' ({1})'", PersonalUtilities.GetNombreEmpresa(), plantilla.cC), 
                                                        AsuntoCorreo, CorreoEnviar.Distinct().ToList(), downloadPDF, tipoFormato);
                vSesiones.downloadPDF = null;
                enviado = true;
            }
            catch (Exception e)
            {
                enviado = false;
            }
            return enviado;
        }

        public List<tblRH_AutorizacionAditivaDeductiva> GetAutorizadores(int plantillaID)
        {
            var data = new List<tblRH_AutorizacionAditivaDeductiva>();
            data = _context.tblRH_AutorizacionAditivaDeductiva.Where(x => x.id_AditivaDeductiva == plantillaID).OrderBy(x => x.orden).ToList();
            return data;
        }
        public tblRH_AditivaDeductiva GetPlantillaSP(int id)
        {
            var data = new tblRH_AditivaDeductiva();
            data = _context.tblRH_AditivaDeductiva.FirstOrDefault(x => x.id == id);
            return data;
        }
        private string getEstatus(bool est,bool rec, bool aut)
        {
            if (aut)
                return "<td style='background-color: yellow;'>AUTORIZANDO</td>";
            else if (est)
                return "<td style='background-color: #82E0AA;'>AUTORIZADO</td>";
            else if (rec)
                return "<td style='background-color: #EC7063;'>RECHAZADO</td>";
            else
                return "<td style='background-color: #FAE5D3;'>PENDIENTE</td>";
        }
        public void GuardarSolicitudEvidencia(int solicitudID, string dir)
        {
            if (true)
            {
                var data = _context.tblRH_AditivaDeductiva.FirstOrDefault(x => x.id == solicitudID);
                data.link = dir;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Error ocurrio un error al insertar un registro");
            }
        }
    }
}