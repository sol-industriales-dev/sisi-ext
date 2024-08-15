using Core.DAO.ControlObra;
using Core.DTO.ControlObra.Gestion;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.ControlObra;
using Core.Entity.ControlObra.GestionDeCambio;
using Core.Entity.Maquinaria.Reporte;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Menus;
using Core.Entity.SubContratistas.Usuarios;
using Core.Entity.Principal.Usuarios;
using Core.Entity.SubContratistas;
using Core.Entity.SubContratistas.Menus;
using Core.Enum.ControlObra;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Core.DTO.ControlObra;
using Core.DTO.Utils.Firmas;
using Core.Enum.Principal.Bitacoras;
using System.Data.Entity;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Administrativo.FacultamientosDpto;
using Core.DTO;
using Core.Enum;

namespace Data.DAO.ControlObra
{
    class GestionDeProyectoDAO : GenericDAO<tblCO_OrdenDeCambio>, IGestionDeProyecto
    {
        Dictionary<string, object> resultado;
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SUBCONTRATISTAS\GESTION";
        private readonly string RutaBaseArchivoFirmado = @"\\REPOSITORIO\Proyecto\SUBCONTRATISTAS\GESTION\DOCUMENTOFIRMADO";
        private const string RutaLocal = @"C:\Proyecto\SUBCONTRATISTAS\GESTION";
        private const string RutaLocalArchivoFirmado = @"C:\Proyecto\SUBCONTRATISTAS\GESTION\DOCUMENTOFIRMADO";
        private const string NombreControlador = "ControlObraController";

        public GestionDeProyectoDAO()
        {
            resultado = new Dictionary<string, object>();
#if DEBUG
            RutaBase = RutaLocal;
            RutaBaseArchivoFirmado = RutaLocalArchivoFirmado;
#endif

        }


        #region ORDEN DE CAMBIO
        public string obtenerNombreArchivo(int idOrdenDeCambio)
        {
            using (contextSigoplan db = new contextSigoplan())
            {
                string nombreArchivo = "";

                var objNombre = db.tblCO_OrdenDeCambio.Where(r => r.id == idOrdenDeCambio).FirstOrDefault();
                if (objNombre != null)
                {
                    nombreArchivo = objNombre.cc + "-" + objNombre.NoOrden + "-" + objNombre.nombreDelArchivo + "-" + objNombre.Contratista;
                }
                else
                {
                    nombreArchivo = "reporte";
                }
                return nombreArchivo;
            }
        }
        public List<ComboDTO> getProyecto()
        {
            var resultado = _context.tblP_CC.ToList().Select(y => new ComboDTO
            {
                Value = y.cc.ToString(),
                Text = y.cc + " - " + y.descripcion
            }).ToList();

            #region Facultamientos CC
            contextSigoplan db = new contextSigoplan();
            var registroFacultamiento = db.tblCO_OC_GestionFirmas.FirstOrDefault(x => x.estatus && x.idEmpleado == vSesiones.sesionUsuarioDTO.id);

            if (registroFacultamiento != null)
            {
                var listaPermisoCC = obtenerCC(registroFacultamiento.cc);

                if (listaPermisoCC.Count() > 0)
                {
                    return resultado.Where(x => listaPermisoCC.Contains(x.Value)).OrderBy(x => x.Text).ToList();
                }
                else
                {
                    return new List<ComboDTO>();
                }
            }
            else
            {
                return new List<ComboDTO>();
            }
            #endregion
        }

        public List<ComboDTO> FillComboEstados()
        {
            return _context.tblRH_EK_Estados.Where(x => x.clave_pais == 1).ToList().Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToList();

        }

        public List<ComboDTO> FillComboMunicipios(int estado_id)
        {
            return _context.tblRH_EK_Cuidades.Where(x => x.clave_pais == 1).ToList().Where(x => estado_id > 0 ? x.clave_estado == estado_id : true).Select(x => new ComboDTO
             {
                 Value = x.id.ToString(),
                 Text = x.descripcion
             }).ToList();
        }

        public List<ordenesDeCambioDTO> obtenerOrdenesDeCambio(string cc, int idUsuario)
        {
            using (contextSigoplan db = new contextSigoplan())
            {
                var lst = new List<ordenesDeCambioDTO>();
                if (cc == null)
                {
                    cc = "";
                }
                try
                {
                    var datos = db.tblCO_OrdenDeCambio.Where(r =>
                        (cc == "" ? r.cc == r.cc : r.cc == cc)
                        && (r.status == (int)ordenesEnum.Pendiente || r.status == (int)ordenesEnum.PendienteVobo2)
                        ).ToList();

                    lst = datos.Select(y => new ordenesDeCambioDTO
                    {
                        id = y.id,
                        fechaEfectiva = y.fechaEfectiva,
                        Proyecto = y.Proyecto,
                        CLiente = y.CLiente,
                        Contratista = y.Contratista,
                        Direccion = y.Direccion,
                        NoOrden = y.NoOrden,
                        esCobrable = y.esCobrable,
                        cc = y.cc + " - " + _context.tblP_CC.Where(r => r.cc == y.cc).FirstOrDefault().descripcion,
                        noContrato = db.tblX_Contrato.Where(r => r.id == y.idContrato).FirstOrDefault().numeroContrato,
                        Antecedentes = y.Antecedentes,
                        tipo = obtenerTipoDependiendoDelPuesto(_context.tblP_Usuario.Where(r => r.id == idUsuario).FirstOrDefault().cveEmpleado),
                        otrasCondicioes = y.otrasCondicioes,
                        lstMontos = db.tblCO_OC_Montos.Where(r => r.idOrdenDeCambio == y.id).ToList().Count() == 0 ? null : db.tblCO_OC_Montos.Where(r => r.idOrdenDeCambio == y.id).ToList(),
                        lstSoportesEvidencia = db.tblCO_OC_SoportesEvidencia.Where(r => r.idOrdenDeCambio == y.id).FirstOrDefault() == null ? null : db.tblCO_OC_SoportesEvidencia.Where(r => r.idOrdenDeCambio == y.id).ToList().Select(n => new SoportesEvidenciaDTO
                        {
                            id = n.id,
                            idOrdenDeCambio = n.idOrdenDeCambio,
                            alcancesNuevos = n.alcancesNuevos,
                            modificacionesPorCambio = n.modificacionesPorCambio,
                            requerimientosDeCampo = n.requerimientosDeCampo,
                            ajusteDeVolumenes = n.ajusteDeVolumenes,
                            serviciosYSuministros = n.serviciosYSuministros,
                            fechaInicial = n.fechaInicial,
                            FechaFinal = n.FechaFinal,
                            MontoContratoOriginal = db.tblX_Contrato.Where(r => r.id == y.idContrato).FirstOrDefault().montoContractual,
                            MontoContratoOriginalSuma = 0,
                        }).FirstOrDefault(),
                        status = y.status,
                        voboPMO = y.voboPMO,
                        idSubContratista = y.idSubContratista
                    }).OrderByDescending(n => n.id).ToList();
                }
                catch (Exception ex)
                {

                }

                return lst;
            }
        }

        public int obtenerTipoDependiendoDelPuesto(string claveEmpleado)
        {
            int Tipo = 0;
            List<string> puestosIDPERSONALOBRA = new List<string>() { "91", "378", "417", "418", "521", "646", "862" };

            //            string sql = @"SELECT A.clave_empleado, A.nombre +' '+ A.ape_paterno +' '+ A.ape_materno AS nombre_completo, B.puesto, B.descripcion 
            //                            FROM 
            //                                sn_empleados AS A 
            //                            INNER JOIN 
            //                                si_puestos AS B 
            //                           ON A.puesto = B.puesto 
            //                            WHERE B.descripcion LIKE'%gerente de proyecto%' AND A.estatus_empleado = 'A' AND A.clave_empleado = " + claveEmpleado + "";
            //            var odbcRH = new OdbcConsultaDTO();
            //            odbcRH.consulta = String.Format(sql);

            //            var ClavesPuestosEmpelados = _contextEnkontrol.Select<PuestosEmpleadosDTO>(EnkontrolEnum.CplanRh, odbcRH);

            var ClavesPuestosEmpelados = _context.Select<PuestosEmpleadosDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = @"SELECT A.clave_empleado, A.nombre +' '+ A.ape_paterno +' '+ A.ape_materno AS nombre_completo, B.puesto, B.descripcion 
                                FROM 
                                    tblRH_EK_Empleados AS A 
                                INNER JOIN 
                                    tblRH_EK_Puestos AS B 
                                ON A.puesto = B.puesto 
                                WHERE B.descripcion LIKE'%gerente de proyecto%' AND A.estatus_empleado = 'A' AND A.clave_empleado = " + claveEmpleado,
            });

            List<string> puestosPERSONALPMO = new List<string>() { "813" };

            //            string sqlPMO = @"SELECT A.clave_empleado, A.nombre +' '+ A.ape_paterno +' '+ A.ape_materno AS nombre_completo, B.puesto, B.descripcion 
            //                            FROM 
            //                                sn_empleados AS A 
            //                            INNER JOIN 
            //                                si_puestos AS B 
            //                           ON A.puesto = B.puesto 
            //                            WHERE B.descripcion LIKE'%LIDER DE S%' AND A.estatus_empleado = 'A' AND A.clave_empleado = " + claveEmpleado + "";
            List<PuestosEmpleadosDTO> ClavesPuestosEmpeladosTipo11 = new List<PuestosEmpleadosDTO>();

            //            using (var dbcon = new OdbcConnection(ConextSigoDapper.conexionEnkontrolRHNOm()))
            //            {
            //                ClavesPuestosEmpeladosTipo11 = dbcon.Query<PuestosEmpleadosDTO>(sqlPMO, null, null, true, 300).ToList();
            //            }

            ClavesPuestosEmpeladosTipo11 = _context.Select<PuestosEmpleadosDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = @"SELECT A.clave_empleado, A.nombre +' '+ A.ape_paterno +' '+ A.ape_materno AS nombre_completo, B.puesto, B.descripcion 
                            FROM 
                                tblRH_EK_Empleados AS A 
                            INNER JOIN 
                                tblRH_EK_Puestos AS B 
                            ON A.puesto = B.puesto 
                            WHERE B.descripcion LIKE'%LIDER DE S%' AND A.estatus_empleado = 'A' AND A.clave_empleado = " + claveEmpleado,
            });

            if (ClavesPuestosEmpelados.Where(r => puestosIDPERSONALOBRA.Contains(r.puesto)).ToList().Count() != 0)
            {
                Tipo = 12;
            }
            else if (ClavesPuestosEmpeladosTipo11.Where(r => puestosPERSONALPMO.Contains(r.puesto)).ToList().Count() != 0)
            {
                Tipo = 11;
            }

            if (claveEmpleado == "18563")
            {
                Tipo = 12;
            }

            return Tipo;
        }
        public Dictionary<string, object> EliminarRenglon(int id)
        {
            using (contextSigoplan db = new contextSigoplan())
            {
                resultado = new Dictionary<string, object>();
                try
                {

                    var objORdenDeCambio = db.tblCO_OrdenDeCambio.Where(r => r.id == id).FirstOrDefault();
                    var lstMontos = db.tblCO_OC_Montos.Where(r => r.idOrdenDeCambio == id).ToList();
                    var objSoportes = db.tblCO_OC_SoportesEvidencia.Where(r => r.idOrdenDeCambio == id).FirstOrDefault();
                    if (objORdenDeCambio != null)
                    {
                        db.tblCO_OrdenDeCambio.Remove(objORdenDeCambio);
                    }
                    if (objSoportes != null)
                    {
                        db.tblCO_OC_SoportesEvidencia.Remove(objSoportes);
                    }
                    if (lstMontos.Count() != 0)
                    {
                        db.tblCO_OC_Montos.RemoveRange(lstMontos);
                    }
                    resultado.Add(ITEMS, "Eliminado Con Exito.");
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception ex)
                {
                    resultado.Add(ITEMS, "Algo ocurrio mal comuniquese con el departamento de TI." + ex.Message.ToString());
                    resultado.Add(SUCCESS, false);
                }
                return resultado;
            }
        }
        public Dictionary<string, object> nuevoEditarOrdenesDeCambio(ordenesDeCambioDTO parametros, int idUsuario)
        {
            using (contextSigoplan db = new contextSigoplan())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    if (parametros.editar == false)
                    {
                        #region Esto es AGREGAR

                        var sql = @"SELECT * FROM tblP_Usuario WHERE estatus=1";
                        var lstUsuarios = new List<tblPUsuarioDTO>();
                        using (var conexion = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                        {
                            conexion.Open();
                            lstUsuarios = conexion.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                            conexion.Close();
                        }

                        //int realIDUser = db.tblP_Usuario.Where(e => e._user == db.tblX_SubContratista.Where(ee => ee.id == parametros.idSubContratista).FirstOrDefault().rfc).FirstOrDefault().id;
                        var usuario = db.tblX_SubContratista.Where(ee => ee.id == parametros.idSubContratista).FirstOrDefault();
                        var usuarioRFC = usuario == null ? "N/A" : usuario.rfc;
                        var auxUsuarioIDSub = lstUsuarios.Where(e => e._user == usuarioRFC).FirstOrDefault();
                        int usuarioIDSub = auxUsuarioIDSub == null ? 0 : auxUsuarioIDSub.id;

                        tblCO_OrdenDeCambio ordenDeCambio = new tblCO_OrdenDeCambio();
                        ordenDeCambio.cc = parametros.cc.Split('-')[0];
                        ordenDeCambio.Antecedentes = parametros.Antecedentes == null ? "" : parametros.Antecedentes;
                        ordenDeCambio.CLiente = parametros.CLiente;
                        ordenDeCambio.Contratista = parametros.Contratista;
                        ordenDeCambio.Direccion = parametros.Direccion;
                        ordenDeCambio.esCobrable = parametros.esCobrable;
                        ordenDeCambio.fechaEfectiva = parametros.fechaEfectiva;
                        ordenDeCambio.idSubContratista = parametros.idSubContratista;
                        ordenDeCambio.NoOrden = parametros.NoOrden;
                        ordenDeCambio.Proyecto = parametros.Proyecto;
                        if (parametros.status != 7)
                        {
                            ordenDeCambio.status = (int)ordenesEnum.Pendiente;
                        }
                        else
                        {
                            ordenDeCambio.status = (int)ordenesEnum.PendienteVobo2;
                        }
                        ordenDeCambio.idContrato = parametros.idContrato;
                        ordenDeCambio.otrasCondicioes = parametros.otrasCondicioes == null ? "" : parametros.otrasCondicioes;
                        ordenDeCambio.nombreDelArchivo = parametros.nombreDelArchivo;
                        ordenDeCambio.ubicacionProyecto = parametros.ubicacionProyecto == null ? "" : parametros.ubicacionProyecto;
                        ordenDeCambio.representanteLegal = parametros.representanteLegal;
                        //parametros.fechaAmpliacion == null ? "" : parametros.fechaAmpliacion;
                        ordenDeCambio.fechaAmpliacion = parametros.fechaAmpliacion;

                        db.tblCO_OrdenDeCambio.Add(ordenDeCambio);
                        db.SaveChanges();

                        tblCO_OC_Montos objMontos = new tblCO_OC_Montos();
                        var ordenCambio = db.tblCO_OrdenDeCambio.OrderByDescending(n => n.id).FirstOrDefault();
                        int idOrdenDeCambio = ordenCambio == null ? 0 : ordenCambio.id;

#if RELEASE
                        //foreach (var item in parametros.lstFirmas)
                        //{
                        //    var obtenerUltimo = db.tblCO_OC_Firmas.OrderByDescending(r => r.id).FirstOrDefault().id;
                        //    if (item.idRow == 1)
                        //    {
                        //        tblP_Alerta objAlerta = new tblP_Alerta();
                        //        objAlerta.userEnviaID = idUsuario;
                        //        objAlerta.userRecibeID = item.idFirma;
                        //        objAlerta.msj = "Tienes una autorizacion Pendiente de orden de cambio";
                        //        objAlerta.visto = false;
                        //        objAlerta.tipoAlerta = 2;
                        //        objAlerta.sistemaID = 3;
                        //        objAlerta.obj = idOrdenDeCambio.ToString();
                        //        objAlerta.objID = obtenerUltimo;
                        //        objAlerta.url = "/ControlObra/GestionDeProyectos/GestionOrden";
                        //        db.tblP_Alerta.Add(objAlerta);
                        //        db.SaveChanges();
                        //    }
                        //    else
                        //    {
                        //        tblP_Alerta objAlerta = new tblP_Alerta();
                        //        objAlerta.userEnviaID = idUsuario;
                        //        objAlerta.userRecibeID = item.idFirma;
                        //        objAlerta.msj = "Tienes una autorizacion Pendiente de orden de cambio";
                        //        objAlerta.visto = false;
                        //        objAlerta.tipoAlerta = 2;
                        //        objAlerta.sistemaID = 3;
                        //        objAlerta.obj = idOrdenDeCambio.ToString();
                        //        objAlerta.objID = obtenerUltimo;
                        //        objAlerta.url = "/ControlObra/ControlObra/GestionOrden";
                        //        _context.tblP_Alerta.Add(objAlerta);
                        //        _context.SaveChanges();
                        //    }
                        //}
#endif



                        tblCO_OC_SoportesEvidencia objSoportes = new tblCO_OC_SoportesEvidencia();

                        objSoportes.ajusteDeVolumenes = parametros.lstSoportesEvidencia.ajusteDeVolumenes;
                        objSoportes.alcancesNuevos = parametros.lstSoportesEvidencia.alcancesNuevos;
                        objSoportes.FechaFinal = parametros.lstSoportesEvidencia.FechaFinal.ToShortDateString() == "01/01/0001" ? DateTime.Now : parametros.lstSoportesEvidencia.FechaFinal;
                        objSoportes.fechaInicial = parametros.lstSoportesEvidencia.fechaInicial.ToShortDateString() == "01/01/0001" ? DateTime.Now : parametros.lstSoportesEvidencia.fechaInicial;
                        objSoportes.idOrdenDeCambio = idOrdenDeCambio;
                        objSoportes.modificacionesPorCambio = parametros.lstSoportesEvidencia.modificacionesPorCambio;
                        objSoportes.requerimientosDeCampo = parametros.lstSoportesEvidencia.requerimientosDeCampo;
                        objSoportes.serviciosYSuministros = parametros.lstSoportesEvidencia.serviciosYSuministros;

                        //string archivoIMG = parametros.AntecedentesArchivos == null ? "" : parametros.Antecedentes; 
                        string rutaIMG = parametros.Antecedentes == null ? "" : Path.Combine(RutaBase, parametros.AntecedentesArchivos.FileName);


                        //objSoportes.modificacionesPorCambioDescripcion = parametros.modificacionesPorCambioDescripcion == null ? "" : parametros.modificacionesPorCambioDescripcion;
                        //objSoportes.requerimientosDeCampoDescripcion = parametros.requerimientosDeCampoDescripcion == null ? "" : parametros.requerimientosDeCampoDescripcion;
                        //objSoportes.ajusteDeVolumenesDescripcion = parametros.ajusteDeVolumenesDescripcion == null ? "" : parametros.ajusteDeVolumenesDescripcion;
                        //objSoportes.serviciosYSuministrosDescripcion = parametros.serviciosYSuministrosDescripcion == null ? "" : parametros.serviciosYSuministrosDescripcion;
                        //objSoportes.fechaDescripcion = parametros.fechaDescripcion == null ? "" : parametros.fechaDescripcion;


                        //objSoportes.modificacionArchvios = parametros.modificacionArchvios == null ? "" : Path.Combine(RutaBase, parametros.modificacionArchvios.FileName);
                        //objSoportes.requerimientosArchivos = parametros.requerimientosArchivos == null ? "" : Path.Combine(RutaBase, parametros.requerimientosArchivos.FileName);
                        //objSoportes.ajusteDeVolumenesArchivos = parametros.ajusteDeVolumenesArchivos == null ? "" : Path.Combine(RutaBase, parametros.ajusteDeVolumenesArchivos.FileName);
                        //objSoportes.serviciosYSuministrosArchivos = parametros.serviciosYSuministrosArchivos == null ? "" : Path.Combine(RutaBase, parametros.serviciosYSuministrosArchivos.FileName);
                        string nombreArchivoImagenAntes = RutaBase + "\\" + ObtenerFormatoNombreArchivo(Path.GetFileNameWithoutExtension(rutaIMG), rutaIMG);
                        objSoportes.antecedentesArchivos = nombreArchivoImagenAntes;
                        db.tblCO_OC_SoportesEvidencia.Add(objSoportes);
                        db.SaveChanges();



                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
                        if (parametros.AntecedentesArchivos != null)
                            listaRutaArchivos.Add(Tuple.Create(parametros.AntecedentesArchivos, nombreArchivoImagenAntes));
                        //if (parametros.modificacionArchvios != null)
                        //    listaRutaArchivos.Add(Tuple.Create(parametros.modificacionArchvios, objSoportes.modificacionArchvios));
                        //if (parametros.requerimientosArchivos != null)
                        //    listaRutaArchivos.Add(Tuple.Create(parametros.requerimientosArchivos, objSoportes.requerimientosArchivos));
                        //if (parametros.ajusteDeVolumenesArchivos != null)
                        //    listaRutaArchivos.Add(Tuple.Create(parametros.ajusteDeVolumenesArchivos, objSoportes.ajusteDeVolumenesArchivos));
                        //if (parametros.serviciosYSuministrosArchivos != null)
                        //    listaRutaArchivos.Add(Tuple.Create(parametros.serviciosYSuministrosArchivos, objSoportes.serviciosYSuministrosArchivos));

                        foreach (var item in listaRutaArchivos)
                        {
                            if (GlobalUtils.SaveHTTPPostedFile(item.Item1, item.Item2) == false)
                            {
                                resultado.Clear();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                return resultado;
                            }
                        }


                        if (parametros.lstMontos != null)
                        {

                            foreach (var item in parametros.lstMontos)
                            {
                                objMontos = new tblCO_OC_Montos();
                                objMontos.idOrdenDeCambio = idOrdenDeCambio;
                                objMontos.importe = item.importe;
                                objMontos.tipoSoportes = item.tipoSoportes;
                                objMontos.no = item.no;
                                objMontos.PrecioUnitario = item.PrecioUnitario;
                                objMontos.unidad = item.unidad;
                                objMontos.descripcion = item.descripcion;
                                objMontos.cantidad = item.cantidad;

                                db.tblCO_OC_Montos.Add(objMontos);
                                db.SaveChanges();
                            }

                            if (parametros.lstMontos.Any(x => string.IsNullOrEmpty(x.no)))
                            {
                                enviarCorreoInsumosSinNumero(parametros.Contratista, parametros.cc, parametros.lstMontos.Where(x => string.IsNullOrEmpty(x.no)).Select(x => x.descripcion).ToList());
                            }
                        }
                        //tblCO_OC_Firmas objAutorizantes = new tblCO_OC_Firmas();
                        //if (parametros.lstFirmas != null)
                        //{
                        //    foreach (var item in parametros.lstFirmas)
                        //    {
                        //        objAutorizantes = new tblCO_OC_Firmas();
                        //        objAutorizantes.idOrdenDeCambio = idOrdenDeCambio;
                        //        objAutorizantes.firma = "";
                        //        objAutorizantes.idFirma = item.idFirma;
                        //        objAutorizantes.firmaDigital = "";
                        //        objAutorizantes.idRow = item.idRow;
                        //        objAutorizantes.Autorizando = false;
                        //        //objAutorizantes.fechaAutorizacion = DateTime.Now;
                        //        objAutorizantes.Estado = true;
                        //        objAutorizantes.estatusFirma = 1;

                        //        db.tblCO_OC_Firmas.Add(objAutorizantes);
                        //        db.SaveChanges();
                        //    }
                        //}


                        var listaAutorizantes = getAutorizantesCentroCosto(ordenDeCambio.cc);

                        var cont = 0;
                        foreach (var item in listaAutorizantes)
                        {
                            if (item.id.HasValue)
                            {
                                string cveEmpleado = item.id.Value.ToString();
                                var usuarioRecibe = _context.tblP_Usuario.FirstOrDefault(x => x.cveEmpleado == cveEmpleado);
                                if (usuarioRecibe != null)
                                {
                                    var alerta = new tblP_Alerta();
                                    alerta.userEnviaID = vSesiones.sesionUsuarioDTO.id;
                                    alerta.userRecibeID = usuarioRecibe.id;
                                    alerta.tipoAlerta = 2;
                                    alerta.sistemaID = 3;
                                    alerta.visto = false;
                                    alerta.url = "/ControlObra/ControlObra/GestionOrden";
                                    alerta.objID = ordenDeCambio.id;
                                    alerta.obj = null;
                                    alerta.msj = "Autorizar Orden Cambio: " + ordenDeCambio.NoOrden;
                                    alerta.documentoID = null;
                                    alerta.moduloID = 0;
                                    _context.tblP_Alerta.Add(alerta);
                                    _context.SaveChanges();
                                }
                            }

                            var subject = "Tienen una orden de cambio para autorizar ";
                            string titulo = "Buen dia " + item.nombre;
                            string modulo = "Estas recibiendo éste e-mail debido a que se a gestionado una orden de cambio.";
                            string asunto = "Orden de cambio pendiente";
                            string mensaje = "Favor de no contestar este correo ya que es generado de forma automatica por nuestros sistemas de cómputo.";
                            mensaje += "<p>Porfavor de revisar la pagina de expediente sigoplan para revisar la orden de cambio existente</p>";
                            mensaje += "<p>" + htmlCorreo(parametros.lstFirmas.Where(x => x.idFirma > 0).ToList()) + "</p>";
                            //string link = "http://expediente.construplan.com.mx/";
                            mensaje += "<p>Sección: PMO / Control de Obra / Ordenes de Cambio / Gestión</p>";
                            string texto = "";
                            string link = "http://sigoplan.construplan.com.mx/ControlObra/ControlObra/GestionOrden";
                            List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
                            lstFirmantes = obtenerFirmantes(parametros.lstFirmas.Where(x => x.idFirma > 0).ToList());
                            var body = GlobalUtils.cuerpoDeCorreoFormato(titulo, modulo, asunto, mensaje, texto, link, lstFirmantes);

                            List<string> correos = new List<string>();
                            correos.Add(item.correo);

#if DEBUG
                            List<string> lstCorreos = new List<string>();
                            lstCorreos.Add("aaron.gracia@construplan.com.mx");
                            lstCorreos.Add("martin.zayas@construplan.com.mx");
                            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstCorreos);
#else
                            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, new List<string> { "cesar.morales@construplan.com.mx" });
                            //DESCOMENTAR PRUEBAS
                            //GlobalUtils.sendEmail(subject, body, correos);
#endif
                        }


                        //var objFoliador = _context.tblCO_foliador.Where(r => r.id == 1).FirstOrDefault();
                        //objFoliador.descripcion += 1;
                        //_context.SaveChanges();

                        resultado.Add(ITEMS, "orden de cambio guardada con exito");
                        resultado.Add(SUCCESS, true);


                        #endregion
                    }
                    else
                    {
                        #region Esto es EDITAR

                        var objOrdenDeCambio = db.tblCO_OrdenDeCambio.Where(r => r.id == parametros.id).FirstOrDefault();
                        var objSoportes = db.tblCO_OC_SoportesEvidencia.Where(r => r.idOrdenDeCambio == parametros.id).FirstOrDefault();
                        var lstMontos = db.tblCO_OC_Montos.Where(r => r.idOrdenDeCambio == parametros.id).ToList();


                        if (objOrdenDeCambio != null)
                        {
                            objOrdenDeCambio.cc = parametros.cc;
                            objOrdenDeCambio.Antecedentes = parametros.Antecedentes;
                            objOrdenDeCambio.CLiente = parametros.CLiente;
                            objOrdenDeCambio.Contratista = parametros.Contratista;
                            objOrdenDeCambio.Direccion = parametros.Direccion;
                            objOrdenDeCambio.ubicacionProyecto = parametros.ubicacionProyecto;
                            objOrdenDeCambio.esCobrable = parametros.esCobrable;
                            objOrdenDeCambio.fechaEfectiva = parametros.fechaEfectiva;
                            objOrdenDeCambio.idSubContratista = parametros.idSubContratista;
                            objOrdenDeCambio.Proyecto = parametros.Proyecto;
                            objOrdenDeCambio.otrasCondicioes = parametros.otrasCondicioes;
                            objOrdenDeCambio.representanteLegal = parametros.representanteLegal;
                            objOrdenDeCambio.fechaAmpliacion = parametros.fechaAmpliacion;
                            //objOrdenDeCambio.status = (int)ordenesEnum.Pendiente;
                            objOrdenDeCambio.voboPMO = parametros.voboPMO;

                            _context.SaveChanges();
                        }

                        if (objSoportes != null)
                        {
                            objSoportes.ajusteDeVolumenes = parametros.lstSoportesEvidencia.ajusteDeVolumenes;
                            objSoportes.alcancesNuevos = parametros.lstSoportesEvidencia.alcancesNuevos;
                            objSoportes.FechaFinal = parametros.lstSoportesEvidencia.FechaFinal;
                            objSoportes.fechaInicial = parametros.lstSoportesEvidencia.fechaInicial;
                            objSoportes.modificacionesPorCambio = parametros.lstSoportesEvidencia.modificacionesPorCambio;
                            objSoportes.requerimientosDeCampo = parametros.lstSoportesEvidencia.requerimientosDeCampo;
                            objSoportes.serviciosYSuministros = parametros.lstSoportesEvidencia.serviciosYSuministros;

                            objSoportes.alcancesNuevosDescripcion = parametros.alcancesNuevosDescripcion == null ? "" : parametros.alcancesNuevosDescripcion;
                            objSoportes.modificacionesPorCambioDescripcion = parametros.modificacionesPorCambioDescripcion == null ? "" : parametros.modificacionesPorCambioDescripcion;
                            objSoportes.requerimientosDeCampoDescripcion = parametros.requerimientosDeCampoDescripcion == null ? "" : parametros.requerimientosDeCampoDescripcion;
                            objSoportes.ajusteDeVolumenesDescripcion = parametros.ajusteDeVolumenesDescripcion == null ? "" : parametros.ajusteDeVolumenesDescripcion;
                            objSoportes.serviciosYSuministrosDescripcion = parametros.serviciosYSuministrosDescripcion == null ? "" : parametros.serviciosYSuministrosDescripcion;
                            objSoportes.fechaDescripcion = parametros.fechaDescripcion == null ? "" : parametros.fechaDescripcion;
                            objSoportes.alcancesNuevosArchivos = parametros.AlcancesNuevosArchivos == null ? "" : Path.Combine(RutaBase, parametros.AlcancesNuevosArchivos.FileName);
                            objSoportes.modificacionArchvios = parametros.modificacionArchvios == null ? "" : Path.Combine(RutaBase, parametros.modificacionArchvios.FileName);
                            objSoportes.requerimientosArchivos = parametros.requerimientosArchivos == null ? "" : Path.Combine(RutaBase, parametros.requerimientosArchivos.FileName);
                            objSoportes.ajusteDeVolumenesArchivos = parametros.ajusteDeVolumenesArchivos == null ? "" : Path.Combine(RutaBase, parametros.ajusteDeVolumenesArchivos.FileName);
                            objSoportes.serviciosYSuministrosArchivos = parametros.serviciosYSuministrosArchivos == null ? "" : Path.Combine(RutaBase, parametros.serviciosYSuministrosArchivos.FileName);

                            objSoportes.alcancesNuevosArchivos = parametros.AlcancesNuevosArchivos == null ? "" : Path.Combine(RutaBase, parametros.AlcancesNuevosArchivos.FileName);
                            objSoportes.modificacionArchvios = parametros.modificacionArchvios == null ? "" : Path.Combine(RutaBase, parametros.modificacionArchvios.FileName);
                            objSoportes.requerimientosArchivos = parametros.requerimientosArchivos == null ? "" : Path.Combine(RutaBase, parametros.requerimientosArchivos.FileName);
                            objSoportes.ajusteDeVolumenesArchivos = parametros.ajusteDeVolumenesArchivos == null ? "" : Path.Combine(RutaBase, parametros.ajusteDeVolumenesArchivos.FileName);
                            objSoportes.serviciosYSuministrosArchivos = parametros.serviciosYSuministrosArchivos == null ? "" : Path.Combine(RutaBase, parametros.serviciosYSuministrosArchivos.FileName);


                            _context.SaveChanges();

                            var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
                            if (parametros.AlcancesNuevosArchivos != null)
                                listaRutaArchivos.Add(Tuple.Create(parametros.AlcancesNuevosArchivos, objSoportes.alcancesNuevosArchivos));
                            if (parametros.modificacionArchvios != null)
                                listaRutaArchivos.Add(Tuple.Create(parametros.modificacionArchvios, objSoportes.modificacionArchvios));
                            if (parametros.requerimientosArchivos != null)
                                listaRutaArchivos.Add(Tuple.Create(parametros.requerimientosArchivos, objSoportes.requerimientosArchivos));
                            if (parametros.ajusteDeVolumenesArchivos != null)
                                listaRutaArchivos.Add(Tuple.Create(parametros.ajusteDeVolumenesArchivos, objSoportes.ajusteDeVolumenesArchivos));
                            if (parametros.serviciosYSuministrosArchivos != null)
                                listaRutaArchivos.Add(Tuple.Create(parametros.serviciosYSuministrosArchivos, objSoportes.serviciosYSuministrosArchivos));

                            foreach (var item in listaRutaArchivos)
                            {
                                if (GlobalUtils.SaveHTTPPostedFile(item.Item1, item.Item2) == false)
                                {
                                    resultado.Clear();
                                    resultado.Add(SUCCESS, false);
                                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                    return resultado;
                                }
                            }
                        }

                        if (lstMontos.Count() != 0)
                        {
                            db.tblCO_OC_Montos.RemoveRange(lstMontos);
                            db.SaveChanges();
                        }

                        tblCO_OC_Montos objMontos = new tblCO_OC_Montos();
                        foreach (var item in parametros.lstMontos)
                        {
                            objMontos = new tblCO_OC_Montos();
                            objMontos.idOrdenDeCambio = objOrdenDeCambio.id;
                            objMontos.tipoSoportes = item.tipoSoportes;
                            objMontos.importe = item.importe;
                            objMontos.no = item.no;
                            objMontos.PrecioUnitario = item.PrecioUnitario;
                            objMontos.unidad = item.unidad;
                            objMontos.descripcion = item.descripcion;
                            objMontos.cantidad = item.cantidad;

                            db.tblCO_OC_Montos.Add(objMontos);
                            db.SaveChanges();
                        }

                        if (parametros.lstMontos.Any(x => string.IsNullOrEmpty(x.no)))
                        {
                            enviarCorreoInsumosSinNumero(parametros.Contratista, parametros.cc, parametros.lstMontos.Where(x => string.IsNullOrEmpty(x.no)).Select(x => x.descripcion).ToList());
                        }

                        List<tblCO_OC_Notificantes> lstNotificantes = new List<tblCO_OC_Notificantes>();
                        tblCO_OC_Notificantes objNotificantes;

                        int sizeNotificantes = 0;
                        foreach (var item in parametros.cveEmpleados)
                        {

                            var Existe = db.tblCO_OC_RelClavesEmpleadosEntreBD.Where(r => r.claveEmpleadoEncontrol == item.cveEmpleados).FirstOrDefault();
                            if (Existe != null)
                            {
                                objNotificantes = db.tblCO_OC_Notificantes.Where(r => r.cvEmpleados == item.cveEmpleados && r.idOrdenDeCambio == objOrdenDeCambio.id).FirstOrDefault();
                                if (objNotificantes == null)
                                {
                                    objNotificantes = new tblCO_OC_Notificantes();
                                    objNotificantes.idOrdenDeCambio = objOrdenDeCambio.id;
                                    objNotificantes.cvEmpleados = item.cveEmpleados;
                                    objNotificantes.idUsuario = Existe.idUsuarioSubcontratista;
                                    lstNotificantes.Add(objNotificantes);
                                }

                            }
                            else
                            {

                                List<string> nombresNotificantes = new List<string>();
                                nombresNotificantes.AddRange(parametros.nombreEmpleados[0].Split(','));
                                sizeNotificantes++;
                                string nombreActual = nombresNotificantes[sizeNotificantes].ToLower();
                                var usr = _context.tblP_Usuario.Where(e => nombreActual.Contains(e.nombre.ToLower()) && nombreActual.Contains(e.apellidoPaterno.ToLower())).FirstOrDefault();

                                var objRelClave = new tblCO_OC_RelClavesEmpleadosEntreBD()
                                {
                                    idUsuarioSigoplan = usr.id,
                                    idUsuarioSubcontratista = 0,
                                    claveEmpleadoEncontrol = item.cveEmpleados
                                };

                                db.tblCO_OC_RelClavesEmpleadosEntreBD.Add(objRelClave);
                                db.SaveChanges();

                                objNotificantes = db.tblCO_OC_Notificantes.Where(r => r.cvEmpleados == item.cveEmpleados).FirstOrDefault();
                                if (objNotificantes == null)
                                {
                                    objNotificantes = new tblCO_OC_Notificantes();
                                    objNotificantes.idOrdenDeCambio = objOrdenDeCambio.id;
                                    objNotificantes.cvEmpleados = item.cveEmpleados;
                                    objNotificantes.idUsuario = objRelClave.idUsuarioSubcontratista;
                                    lstNotificantes.Add(objNotificantes);
                                }


                            }
                        }
                        if (lstNotificantes.Count() != 0)
                        {
                            db.tblCO_OC_Notificantes.AddRange(lstNotificantes);
                            db.SaveChanges();
                        }


                        resultado.Add(ITEMS, "orden de cambio registrada con exito");
                        resultado.Add(SUCCESS, true);
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(ITEMS, "Algo ocurrio mal favor de comunicarse con TI.");
                    resultado.Add(SUCCESS, false);
                }
                return resultado;
            }
        }

        private void enviarCorreoInsumosSinNumero(string contratista, string cc, List<string> listaDescripciones)
        {
            var listaCorreos = new List<string>();

            listaCorreos.Add("javier.morales@construplan.com.mx");
            listaCorreos.Add("francisco.fregoso@construplan.com.mx");
            listaCorreos.Add("jose.rodriguez@construplan.com.mx");
            listaCorreos.Add("ruth.vargas@construplan.com.mx");
            listaCorreos.Add("maricela.ortiz@construplan.com.mx");

#if DEBUG
            listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif

            var asunto = "Orden de cambio con insumos no dados de alta";
            var mensaje = "Se ha capturado una orden de cambio para el contratista \"" + contratista + "\" en el centro de costo \"" + cc + "\" con insumos no dados de alta en SISUN:<br><br>";

            foreach (var descripcion in listaDescripciones)
            {
                mensaje += "- " + descripcion + "<br>";
            }

            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, listaCorreos);
        }

        public object GetInsumosSISUNAutocomplete(string term, bool busquedaPorNumero, string cc)
        {
            if (string.IsNullOrEmpty(cc))
            {
                return null;
            }

            if (busquedaPorNumero)
            {
                if (term.Count() != 7)
                {
                    return null;
                }

                return _contextEnkontrol.Select<InsumoAutocompleteDTO>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO
                {
                    consulta = string.Format(@"
                        SELECT
                            ex.OBRA AS cc, ex.INSUMO AS id, ins.descripcion AS value, ins.unidad, ex.PRECIO_BASE AS precio
                        FROM su_explosion_base ex
                            INNER JOIN insumos ins ON ex.INSUMO = ins.insumo
                        WHERE ex.OBRA = '{0}' AND ex.INSUMO = {1} 
                    ", cc, term)
                });
            }
            else
            {
                return _contextEnkontrol.Select<InsumoAutocompleteDTO>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO
                {
                    consulta = string.Format(@"
                        SELECT TOP 10
                            ex.OBRA AS cc, ex.INSUMO AS id, ins.descripcion AS value, ins.unidad, ex.PRECIO_BASE AS precio
                        FROM su_explosion_base ex
                            INNER JOIN insumos ins ON ex.INSUMO = ins.insumo
                        WHERE ex.OBRA = '{0}' AND ins.descripcion LIKE '%{1}%'
                    ", cc, term)
                });
            }
        }

        private string ObtenerFormatoNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, ObtenerFormatoCarpetaFechaActual(), Path.GetExtension(fileName));
        }
        private string ObtenerFormatoCarpetaFechaActual()
        {
            return DateTime.Now.ToString("dd-MM-y HH:mm:ss.ffffff").Replace(":", "-");
        }
        public List<FirmantesDTO> obtenerFirmantes(List<tblCO_OC_Firmas> lstFirmas)
        {
            //var sql = @"SELECT * FROM tblP_Usuario WHERE estatus=1";
            //var lstUsuarios = new List<tblPUsuarioDTO>();
            //using (var conexion = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
            //{
            //    conexion.Open();
            //    lstUsuarios = conexion.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
            //    conexion.Close();
            //}
            var listaUsuarios = _context.tblP_Usuario.ToList();
            List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();

            FirmantesDTO objFirmantes;


            foreach (var item in lstFirmas)
            {
                //var listaAutorizantes = getAutorizantesCentroCosto(ordenDeCambio.cc);

                objFirmantes = new FirmantesDTO();
                objFirmantes.puesto = obtenerPuestosEstaticos(item.idRow);
                objFirmantes.estado = item.Estado;
                objFirmantes.Fecha = item.fechaAutorizacion == null ? "" : item.fechaAutorizacion.ToString();
                objFirmantes.Firma = item.firmaDigital == "" ? "" : item.firmaDigital;
                //if (item.idRow == 1)
                //{

                //    objFirmantes.nombreCompleto = listaUsuarios.Where(r => r.cveEmpleado == item.idFirma.ToString()).FirstOrDefault() == null ? "" : listaUsuarios.Where(r => r.cveEmpleado == item.idFirma.ToString()).FirstOrDefault().nombre;
                //}
                //else
                //{
                objFirmantes.nombreCompleto = _context.tblP_Usuario.Where(r => r.cveEmpleado == item.idFirma.ToString()).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault();
                //}
                lstFirmantes.Add(objFirmantes);
            }

            return lstFirmantes;
        }


        public List<ComboDTO> comboObtenerContratosyUltimasOrdenesDeCambio(List<string> filtroCC)
        {
            using (contextSigoplan db = new contextSigoplan())
            {
                List<ComboDTO> result = new List<ComboDTO>();
                List<comboOrdenesDTO> lstNueva = new List<comboOrdenesDTO>();
                var lstOrdenesDeCambioCC = db.tblCO_OrdenDeCambio.Where(r => (r.status == (int)ordenesEnum.Pendiente || r.status == (int)ordenesEnum.Autorizada || r.status == (int)ordenesEnum.AutorizadaAnterior || r.status == (int)ordenesEnum.PendientePorAutorizar)).ToList().Select(y => y.idContrato).ToList();


                var lstObtenerContratos = db.tblX_Contrato.Where(r => r.estatus == true
                    && filtroCC.Contains(r.cc) && (lstOrdenesDeCambioCC.Count() != 0 ? !lstOrdenesDeCambioCC.Contains(r.id) : r.id == r.id)
                    ).ToList()
                    .Select(y => new comboOrdenesDTO
                    {
                        id = y.id,
                        text = y.numeroContrato + " " + db.tblX_Proyecto.Where(r => r.id == y.proyectoID).FirstOrDefault().nombre,
                        tabla = "tblX_Contrato",
                        idContrato = y.id
                    }).ToList();
                var lstOrdenDeCambio = db.tblCO_OrdenDeCambio.ToList()
                    .Select(y => new comboOrdenesDTO
                    {
                        id = y.id,
                        text = db.tblX_Contrato.Where(r => r.id == y.idContrato).FirstOrDefault().numeroContrato + " " + y.Proyecto,
                        tabla = "tblCO_OrdenDeCambio",
                        idContrato = y.idContrato
                    }).ToList().OrderByDescending(y => y.id).ToList();

                var lstSeparador = new List<comboOrdenesDTO>();
                foreach (var item in lstOrdenDeCambio)
                {
                    var existe = lstSeparador.Where(r => r.text == item.text).FirstOrDefault();
                    if (existe == null)
                    {
                        lstSeparador.Add(item);
                    }
                }


                lstNueva.AddRange(lstObtenerContratos);
                lstNueva.AddRange(lstSeparador);

                result = lstNueva.Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.text,
                    Prefijo = y.tabla + "-" + y.idContrato,

                }).ToList();

                return result;
            }
        }
        public Dictionary<string, object> obtenerContratosyUltimasOrdenesDeCambio(ordenesDeCambioDTO parametros)
        {
            resultado = new Dictionary<string, object>();

            return resultado;
        }
        public Dictionary<string, object> obtenerCamposDeOrdenDeCambio(ordenesDeCambioDTO parametros)
        {
            resultado = new Dictionary<string, object>();
            contextSigoplan db = new contextSigoplan();

            List<dynamic> listaEstados = _context.Select<dynamic>(new DapperDTO { baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR, consulta = @"SELECT * FROM tblP_Estados" }).ToList();
            List<dynamic> listaMunicipios = _context.Select<dynamic>(new DapperDTO { baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR, consulta = @"SELECT * FROM tblP_Municipios" }).ToList();

            if (parametros.tabla == "tblCO_OrdenDeCambio")
            {
                var lst = db.tblCO_OrdenDeCambio.Where(r => r.id == parametros.id).ToList().Select(y =>
                {

                    var contrato = db.tblX_Contrato.Where(r => r.id == y.idContrato).FirstOrDefault();
                    var numOCMontos = db.tblCO_OC_Montos.Where(r => r.idOrdenDeCambio == y.id).ToList();
                    var soportes = db.tblCO_OC_SoportesEvidencia.Where(r => r.idOrdenDeCambio == y.id).ToList().Count() == 0 ? null : db.tblCO_OC_SoportesEvidencia.Where(r => r.idOrdenDeCambio == y.id).ToList().Select(n => new SoportesEvidenciaDTO
                        {
                            id = n.id,
                            idOrdenDeCambio = n.idOrdenDeCambio,
                            alcancesNuevos = n.alcancesNuevos,
                            modificacionesPorCambio = n.modificacionesPorCambio,
                            requerimientosDeCampo = n.requerimientosDeCampo,
                            ajusteDeVolumenes = n.ajusteDeVolumenes,
                            serviciosYSuministros = n.serviciosYSuministros,
                            fechaInicial = n.fechaInicial,
                            FechaFinal = n.FechaFinal,
                            alcancesNuevosDescripcion = n.alcancesNuevosDescripcion,
                            //modificacionesPorCambioDescripcion = n.modificacionesPorCambioDescripcion,
                            //requerimientosDeCampoDescripcion = n.requerimientosDeCampoDescripcion,
                            //ajusteDeVolumenesDescripcion = n.ajusteDeVolumenesDescripcion,
                            //serviciosYSuministrosDescripcion = n.serviciosYSuministrosDescripcion,
                            //fechaDescripcion = n.fechaDescripcion,
                            MontoContratoOriginal = db.tblX_Contrato.Where(r => r.id == y.idContrato).FirstOrDefault().montoContractual,
                            MontoContratoOriginalSuma = y.voboPMO ? (n.alcancesNuevos + n.serviciosYSuministros + n.requerimientosDeCampo + n.ajusteDeVolumenes + n.modificacionesPorCambio) : 0,
                            //AlcancesNuevosArchivos = n.alcancesNuevosArchivos,
                            //modificacionArchvios = n.modificacionArchvios,
                            //requerimientosArchivos = n.requerimientosArchivos,
                            //ajusteDeVolumenesArchivos = n.ajusteDeVolumenesArchivos,
                            //serviciosYSuministrosArchivos = n.serviciosYSuministrosArchivos,
                        }).FirstOrDefault();
                    var firmas = db.tblCO_OC_Firmas.Where(r => r.idOrdenDeCambio == y.id).ToList();
                    var estado = (contrato != null && contrato.estado_id > 0) ? listaEstados.FirstOrDefault(z => (int)z.idEstado == db.tblX_Contrato.Where(r => r.id == y.idContrato).FirstOrDefault().estado_id) : null;
                    var municipio = (contrato != null && contrato.municipio_id > 0) ? listaMunicipios.FirstOrDefault(z => (int)z.idMunicipio == db.tblX_Contrato.Where(r => r.id == y.idContrato).FirstOrDefault().municipio_id) : null;
                    var subcontratista = db.tblX_SubContratista.FirstOrDefault(z => z.id == y.idSubContratista);
                    var ordenCambio = db.tblCO_OrdenDeCambio.Where(r => r.idContrato == y.idContrato).OrderByDescending(x => x.id).FirstOrDefault();

                    var lstAutorizantes = getAutorizantesCentroCosto(y.cc);
                    lstAutorizantes.Add(new AutorizanteDTO()
                    {
                        id = 0,
                        nombre = subcontratista != null ? subcontratista.representanteLegal : "",
                        puesto = "REPRESENTANTE LEGAL SUB",
                        tblp_usuario_id = 0,
                        correo = "",
                    });

                    return new ordenesDeCambioDTO
                    {
                        id = y.id,
                        fechaEfectiva = y.fechaEfectiva,
                        Proyecto = y.Proyecto,
                        numeroDeContrato = contrato == null ? "N/A" : contrato.numeroContrato,
                        montoContractual = contrato == null ? 0 : contrato.montoContractual,
                        fechaInicial = contrato == null ? new DateTime() : contrato.fechaInicial,
                        fechaFinal = contrato == null ? new DateTime() : contrato.fechaFinal,
                        CLiente = y.CLiente,
                        ubicacionProyecto = y.ubicacionProyecto,
                        Contratista = y.Contratista,
                        Direccion = y.Direccion,
                        NoOrden = y.status == (int)ordenesEnum.Pendiente || y.status == (int)ordenesEnum.PendienteVobo1 || y.status == (int)ordenesEnum.PendienteVobo2 ? y.NoOrden : (Convert.ToInt32(y.NoOrden) + 1).ToString(),
                        esCobrable = y.esCobrable,
                        cc = y.cc,
                        Antecedentes = y.Antecedentes,
                        //otrasCondicioes = y.otrasCondicioes,
                        nombreDelArchivo = y.nombreDelArchivo,
                        fechaSuscripcion = contrato == null ? new DateTime() : (DateTime)contrato.fechaSuscripcion,
                        fechaExpiracion = contrato == null ? new DateTime() : (DateTime)contrato.fechaVigencia,
                        lstMontos = numOCMontos.Count() == 0 ? null : numOCMontos,
                        lstSoportesEvidencia = soportes,
                        status = y.status,
                        lstFirmas = firmas.Count() != 0 ? firmas : null,
                        idSubContratista = y.idSubContratista,
                        estado = estado == null ? "N/A" : estado.Estado,
                        municipio = municipio == null ? "N/A" : municipio.Municipio,
                        subcontratistaNombre = subcontratista == null ? "N/A" : subcontratista.nombre,
                        representanteLegal = y.representanteLegal,

                        listaAutorizantes = lstAutorizantes,
                        fechaAmpliacion = y.fechaAmpliacion,
                        fechaAmpliacionAcumulada = ordenCambio == null ? null : ordenCambio.fechaAmpliacion
                    };
                }
                ).FirstOrDefault();
                resultado.Add(ITEMS, lst);
            }
            else
            {
                var lst = db.tblX_Contrato.Where(r => r.id == parametros.id).ToList().Select(y =>
                {


                    var proyecto = db.tblX_Proyecto.FirstOrDefault(r => r.id == y.proyectoID);
                    var cliente = proyecto == null ? null : db.tblX_Cliente.FirstOrDefault(r => r.id == proyecto.clienteID);
                    var ordenCambio = db.tblCO_OrdenDeCambio.Where(r => r.idContrato == y.id).ToList();
                    var cc = _context.tblP_CC.FirstOrDefault(r => r.cc == y.cc);
                    var contrato = db.tblX_Contrato.FirstOrDefault(r => r.id == y.id);
                    var subcontratista = db.tblX_SubContratista.FirstOrDefault(z => z.id == y.subcontratistaID);
                    var montos = db.tblCO_OC_Montos.Where(r => r.idOrdenDeCambio == y.id).ToList();

                    var lstAutorizantes = getAutorizantesCentroCosto(y.cc);
                    lstAutorizantes.Add(new AutorizanteDTO()
                    {
                        id = 0,
                        nombre = subcontratista.representanteLegal,
                        puesto = "REPRESENTANTE LEGAL SUB",
                        tblp_usuario_id = 0,
                        correo = "",
                    });

                    return new ordenesDeCambioDTO
                    {
                        id = y.id,
                        fechaEfectiva = DateTime.Now,
                        numeroDeContrato = y.numeroContrato,
                        montoContractual = (decimal)y.montoContractual,
                        //fechaInicial = y.fechaInicial,
                        //fechaFinal = y.fechaFinal,
                        Proyecto = proyecto == null ? "" : proyecto.nombre,
                        CLiente = cliente == null ? "N/A" : cliente.nombre,
                        //Contratista = db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault().nombre,
                        Direccion = cliente == null ? "N/A" : cliente.direccion,
                        NoOrden = ordenCambio.Count() == 0 ? "1" : ordenCambio.Count().ToString(),
                        esCobrable = false,
                        cc = y.cc + (cc == null ? "" : (" - " + cc.descripcion)),
                        fechaInicial = contrato == null ? null : contrato.fechaInicial,
                        fechaFinal = contrato == null ? null : contrato.fechaFinal,
                        //fechaSuscripcion = (DateTime)y.fechaSuscripcion,
                        //fechaExpiracion = (DateTime)y.fechaVigencia,
                        estado = y.estado_id > 0 ? listaEstados.FirstOrDefault(z => (int)z.idEstado == y.estado_id).Estado : "",
                        municipio = y.municipio_id > 0 ? listaMunicipios.FirstOrDefault(z => (int)z.idMunicipio == y.municipio_id).Municipio : "",
                        subcontratistaNombre = subcontratista == null ? "N/A" : subcontratista.nombre,
                        lstMontos = montos.Count() == 0 ? null : montos,
                        //}).FirstOrDefault();

                        lstSoportesEvidencia = contrato == null ?
                        new SoportesEvidenciaDTO
                        {
                            id = 0,
                            idOrdenDeCambio = 0,
                            alcancesNuevos = 0,
                            modificacionesPorCambio = 0,
                            requerimientosDeCampo = 0,
                            ajusteDeVolumenes = 0,
                            serviciosYSuministros = 0,
                            fechaInicial = DateTime.Now,
                            FechaFinal = DateTime.Now,
                            MontoContratoOriginal = 0,
                            MontoContratoOriginalSuma = 0,
                        }
                        :
                        new SoportesEvidenciaDTO
                        {
                            id = contrato.id,
                            idOrdenDeCambio = 0,
                            alcancesNuevos = 0,
                            modificacionesPorCambio = 0,
                            requerimientosDeCampo = 0,
                            ajusteDeVolumenes = 0,
                            serviciosYSuministros = 0,
                            fechaInicial = DateTime.Now,
                            FechaFinal = DateTime.Now,
                            MontoContratoOriginal = (decimal)contrato.montoContractual,
                            MontoContratoOriginalSuma = 0,
                        },
                        //Antecedentes = "",
                        idSubContratista = y.subcontratistaID,
                        listaAutorizantes = lstAutorizantes,

                        fechaAmpliacionAcumulada = ordenCambio.Count() > 0 ? ordenCambio.OrderByDescending(z => z.id).FirstOrDefault().fechaAmpliacion : null
                    };
                }).FirstOrDefault();
                resultado.Add(ITEMS, lst);
            }
            return resultado;
        }

        private List<AutorizanteDTO> getAutorizantesCentroCosto(string cc)
        {
            var listaAutorizantes = new List<AutorizanteDTO>();
            var listaUsuarios = _context.tblP_Usuario.ToList();

            // SE UTILIZA ORDEN = 5, YA QUE ES EL IDENTIFICADOR DE "ORDEN DE CAMBIO" DEL CATALOGO DE FACULTAMIENTOS.
            tblFA_Plantilla objPlantilla = _context.tblFA_Plantilla.Where(w => w.orden == 5 && w.esActiva).FirstOrDefault();
            //cc = "162";
            listaAutorizantes = _context.tblP_CC.Where(w => w.cc == cc && w.estatus).Join(
                _context.tblFA_Paquete.Where(w => w.estado == 1),
                c => c.id,
                p => p.ccID,
                (c, p) => new { c, p }
            ).Join(
                _context.tblFA_Facultamiento.Where(w => w.aplica && w.plantillaID == objPlantilla.id),
                cp => cp.p.id,
                f => f.paqueteID,
                (p, f) => new { p, f }
            ).Join(
                _context.tblFA_Empleado.Where(w => w.esActivo && w.aplica),
                cpf => cpf.f.id,
                e => e.facultamientoID,
                (cpf, e) => new { cpf, e }
            ).Join(
                _context.tblFA_ConceptoPlantilla.Where(w => w.plantillaID == objPlantilla.id && w.esActivo),
                cpfe => cpfe.e.conceptoID,
                co => co.id,
                (cpfe, co) => new { cpfe, co }
            ).ToList().Select(x => new AutorizanteDTO
            {
                id = x.cpfe.e.claveEmpleado,
                nombre = x.cpfe.e.nombreEmpleado != null ? x.cpfe.e.nombreEmpleado.Trim() : "",
                puesto = x.co.concepto,
                tblp_usuario_id = listaUsuarios.FirstOrDefault(u => u.cveEmpleado == x.cpfe.e.claveEmpleado.ToString()).id,
                correo = listaUsuarios.Where(y => ((y.cveEmpleado != null && x.cpfe.e.claveEmpleado != null) ? y.cveEmpleado == x.cpfe.e.claveEmpleado.ToString() : false)).Select(z => z.correo).FirstOrDefault(),
            }).ToList();

            return listaAutorizantes;
        }

        public string foliando(int numero)
        {
            string retfol = "";
            string fol = "";
            string fon = numero.ToString();
            for (int i = 0; i <= 8; i++)
            {
                if (i == 0)
                {
                    fol = fon;
                }
                if (fol.Length <= 8)
                {
                    fol = "0" + fol;
                }
            }
            retfol = "FA" + fol;

            return retfol;
        }
        public List<ComboDTO> comboObtenerContratosyUltimasOrdenesDeCambioEditar()
        {
            using (contextSigoplan db = new contextSigoplan())
            {
                List<ComboDTO> result = new List<ComboDTO>();
                List<comboOrdenesDTO> lstNueva = new List<comboOrdenesDTO>();
                var lstOrdenesDeCambioCC = db.tblCO_OrdenDeCambio.Where(r => (r.status == (int)ordenesEnum.Pendiente || r.status == (int)ordenesEnum.Autorizada || r.status == (int)ordenesEnum.AutorizadaAnterior)).ToList().Select(y => y.idContrato).ToList();


                var lstObtenerContratos = db.tblX_Contrato.Where(r => r.estatus == true
                    && (lstOrdenesDeCambioCC.Count() != 0 ? lstOrdenesDeCambioCC.Contains(r.id) : r.id == r.id)
                    ).ToList()
                    .Select(y => new comboOrdenesDTO
                    {
                        id = y.id,
                        text = y.numeroContrato + " " + db.tblX_Proyecto.Where(r => r.id == y.proyectoID).FirstOrDefault().nombre,
                        tabla = "tblX_Contrato",
                        idContrato = y.id
                    }).ToList();
                var lstOrdenDeCambio = db.tblCO_OrdenDeCambio.Where(r => (r.status == (int)ordenesEnum.Autorizada || r.status == (int)ordenesEnum.Pendiente || r.status == (int)ordenesEnum.PendienteVobo2)).ToList()
                    .Select(y => new comboOrdenesDTO
                    {
                        id = y.id,
                        text = y.NoOrden + " " + y.Proyecto,
                        tabla = "tblCO_OrdenDeCambio",
                        idContrato = y.idContrato
                    }).ToList();

                lstNueva.AddRange(lstObtenerContratos);
                lstNueva.AddRange(lstOrdenDeCambio);

                result = lstNueva.Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.text,
                    Prefijo = y.tabla + "-" + y.idContrato,
                }).ToList();

                return result;
            }
        }
        public ordenDTO obtenerOrdenDeCambioByID(int idOrdenDeCambio)
        {
            ordenDTO obj = new ordenDTO();

            try
            {
                contextSigoplan db = new contextSigoplan();

                obj = new ordenDTO
                {
                    ordenCambio = new ordenesCambioDTO(),
                    lstSoportesEvidencia = new soportesEviDTO(),
                    lstMontos = new List<montosDTO>(),
                    LstFirmas = new List<firmasDTO>()
                };

                obj.ordenCambio = db.tblCO_OrdenDeCambio.Where(r => r.id == idOrdenDeCambio).ToList().Select(y => new ordenesCambioDTO
                {
                    id = y.id,
                    fechaEfectiva = y.fechaEfectiva,
                    Proyecto = y.Proyecto,
                    CLiente = y.CLiente,
                    Contratista = y.Contratista,
                    Direccion = y.Direccion,
                    NoOrden = y.NoOrden,
                    esCobrable = y.esCobrable,
                    cc = y.cc,
                    Antecedentes = y.Antecedentes,

                    idSubContratista = y.idSubContratista,
                    status = y.status,
                    tabla = "",
                    numeroDeContrato = db.tblX_Contrato.Where(r => r.id == y.idContrato).FirstOrDefault().numeroContrato,
                    totalDeMontos = 0,
                    ubicacionProyecto = y.ubicacionProyecto,
                    fechaSuscripcion = (DateTime)db.tblX_Contrato.Where(r => r.id == y.idContrato).FirstOrDefault().fechaSuscripcion,
                    fechaExpiracion = (DateTime)db.tblX_Contrato.Where(r => r.id == y.idContrato).FirstOrDefault().fechaVigencia,
                    diasDeContrato = ObtenerCantidadDeDias((DateTime)db.tblX_Contrato.Where(r => r.id == y.idContrato).FirstOrDefault().fechaVigencia, (DateTime)db.tblX_Contrato.Where(r => r.id == y.idContrato).FirstOrDefault().fechaSuscripcion),
                    idContrato = y.idContrato,
                    otrasCondicioes = y.otrasCondicioes == null ? "" : y.otrasCondicioes,
                    sumaTotalDeDias = (int)sumaTotalDeDiasPrevias(y.idContrato),
                    representanteLegal = y.representanteLegal,
                    mostrarFirmas = y.status == (int)ordenesEnum.Autorizada ? true : false
                }).FirstOrDefault();

                obj.lstSoportesEvidencia = db.tblCO_OC_SoportesEvidencia.Where(r => r.idOrdenDeCambio == idOrdenDeCambio).ToList().Select(n => new soportesEviDTO
                {
                    id = n.id,
                    idOrdenDeCambio = n.idOrdenDeCambio,
                    alcancesNuevos = (double)n.alcancesNuevos,
                    modificacionesPorCambio = (double)n.modificacionesPorCambio,
                    requerimientosDeCampo = (double)n.requerimientosDeCampo,
                    ajusteDeVolumenes = (double)n.ajusteDeVolumenes,
                    serviciosYSuministros = (double)n.serviciosYSuministros,
                    fechaInicial = n.fechaInicial,
                    fechaFinal = n.FechaFinal,
                    Dias = ObtenerCantidadDeDias(n.fechaInicial, n.FechaFinal),
                    alcancesNuevosDescripcion = n.alcancesNuevosDescripcion,
                    modificacionesPorCambioDescripcion = n.modificacionesPorCambioDescripcion,
                    requerimientosDeCampoDescripcion = n.requerimientosDeCampoDescripcion,
                    ajusteDeVolumenesDescripcion = n.ajusteDeVolumenesDescripcion,
                    serviciosYSuministrosDescripcion = n.serviciosYSuministrosDescripcion,
                    fechaDescripcion = n.fechaDescripcion,
                    MontoContratoOriginal = (double)db.tblX_Contrato.Where(r => r.id == obj.ordenCambio.idContrato).FirstOrDefault().montoContractual,
                    MontoContratoOriginalFormatMX = db.tblX_Contrato.Where(r => r.id == obj.ordenCambio.idContrato).FirstOrDefault().montoContractual.ToString("C"),
                    MontoContratoOriginalSuma = 0,
                    numeroConLetras = GlobalUtils.toText(sumatotal(n.alcancesNuevos, n.modificacionesPorCambio, n.requerimientosDeCampo, n.ajusteDeVolumenes, n.serviciosYSuministros, db.tblX_Contrato.Where(r => r.id == obj.ordenCambio.idContrato).FirstOrDefault().montoContractual, sumaTotalDeOrdenesDeCambioPrevias(obj.ordenCambio.idContrato))),
                    numeroTotal = sumatotal(n.alcancesNuevos, n.modificacionesPorCambio, n.requerimientosDeCampo, n.ajusteDeVolumenes, n.serviciosYSuministros, db.tblX_Contrato.Where(r => r.id == obj.ordenCambio.idContrato).FirstOrDefault().montoContractual, sumaTotalDeOrdenesDeCambioPrevias(obj.ordenCambio.idContrato)),
                    numeroTotalFormatMX = sumatotal(n.alcancesNuevos, n.modificacionesPorCambio, n.requerimientosDeCampo, n.ajusteDeVolumenes, n.serviciosYSuministros, db.tblX_Contrato.Where(r => r.id == obj.ordenCambio.idContrato).FirstOrDefault().montoContractual, sumaTotalDeOrdenesDeCambioPrevias(obj.ordenCambio.idContrato)).ToString("C"),
                    numeroTotalSinMontoInicial = sumatotalSinMontoInicial(n.alcancesNuevos, n.modificacionesPorCambio, n.requerimientosDeCampo, n.ajusteDeVolumenes, n.serviciosYSuministros),
                    numeroTotalSinMontoInicialFormatMX = sumatotalSinMontoInicial(n.alcancesNuevos, n.modificacionesPorCambio, n.requerimientosDeCampo, n.ajusteDeVolumenes, n.serviciosYSuministros).ToString("C"),
                    sumaTotalDeOrdenesDeCambioPrevias = sumaTotalDeOrdenesDeCambioPrevias(obj.ordenCambio.idContrato),
                    sumaTotalDeOrdenesDeCambioPreviasFormatMX = sumaTotalDeOrdenesDeCambioPrevias(obj.ordenCambio.idContrato).ToString("C")
                }).FirstOrDefault();

                obj.lstMontos = obtenerLstFormateada(
                    db.tblCO_OC_Montos.Where(r => r.idOrdenDeCambio == idOrdenDeCambio).ToList().Select(y => new montosDTO
                    {
                        id = y.id,
                        no = y.no,
                        descripcion = y.descripcion,
                        unidad = y.unidad,
                        cantidad = y.cantidad,
                        PrecioUnitario = y.PrecioUnitario,
                        PrecioUnitarioFormatMX = y.PrecioUnitario.ToString("C"),
                        importe = y.importe,
                        importeFormatMX = y.importe.ToString("C"),
                        idOrdenDeCambio = y.idOrdenDeCambio,
                        tipoSoportes = y.tipoSoportes
                    }).ToList()
                );

                obj.LstFirmas = db.tblCO_OC_Firmas.Where(r => r.idOrdenDeCambio == idOrdenDeCambio).ToList().Select(y => new firmasDTO
                {
                    id = y.id,
                    nombreEmpleado = y.nombreFirmante,
                    puestoFirmante = y.puestoFirmante,
                    idUsuarioFirma = y.idFirma,
                }).ToList();

                var repLegalConstruplan = obj.LstFirmas.FirstOrDefault(e => e.puestoFirmante == "Gerente de Proyecto");

                if (repLegalConstruplan != null)
	            {
                   var firmaRepLegalConstruplan = db.tblCO_OC_ArchivosFirmas.FirstOrDefault(e => e.idUsuario == repLegalConstruplan.idUsuarioFirma);

                   if (firmaRepLegalConstruplan != null && obj.ordenCambio.mostrarFirmas)
                   {
#if DEBUG
                       obj.rutaFirmaConstruplan = RutaBase+"\\FIRMAS_REPRESENTANTES\\default.jpg";
#else
                       obj.rutaFirmaConstruplan = firmaRepLegalConstruplan.ruta;
#endif

                   }else{
                       obj.rutaFirmaConstruplan = RutaBase+"\\FIRMAS_REPRESENTANTES\\default.jpg";

                   }
                }
                else
                {
                    obj.rutaFirmaConstruplan = RutaBase + "\\FIRMAS_REPRESENTANTES\\default.jpg";
                }

                var objSubcontratista = db.tblX_SubContratista.FirstOrDefault(e => e.id == obj.ordenCambio.idSubContratista);
                if (objSubcontratista != null && objSubcontratista.rutaArchivoFirma != null && obj.ordenCambio.mostrarFirmas)
                {
#if DEBUG
                    obj.rutaFirmaSub = RutaBase + "\\FIRMAS_REPRESENTANTES\\default.jpg";
#else
                    obj.rutaFirmaSub = objSubcontratista.rutaArchivoFirma;
#endif
                }
                else
                {
                    obj.rutaFirmaSub = RutaBase + "\\FIRMAS_REPRESENTANTES\\default.jpg";

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return obj;
        }
        public string retornarNombreCompleto(List<tblX_SubContratista> lstUsuarios, int idFirma)
        {
            string nombrecompleto = "";
            if (lstUsuarios.Where(r => r.id == idFirma).FirstOrDefault() != null)
            {
                nombrecompleto = lstUsuarios.Where(r => r.id == idFirma).FirstOrDefault().nombre;
            }
            else
            {
                nombrecompleto = "";
            }
            return nombrecompleto;
        }
        public List<montosDTO> obtenerLstFormateada(List<montosDTO> lstMontos)
        {
            List<montosDTO> lst = new List<montosDTO>();
            montosDTO obj = new montosDTO();
            List<int> lstTipos = new List<int>();
            lstTipos.Add((int)tipoSoportes.Alcances);
            lstTipos.Add((int)tipoSoportes.Modificacion);
            lstTipos.Add((int)tipoSoportes.Req);
            lstTipos.Add((int)tipoSoportes.Ajuste);
            lstTipos.Add((int)tipoSoportes.Serv);

            foreach (var item in lstTipos)
            {
                switch (item)
                {
                    case (int)tipoSoportes.Alcances:
                        if (lstMontos.Where(r => r.tipoSoportes == item).ToList().Count() != 0)
                        {
                            obj = new montosDTO();
                            obj.descripcion = "A.- Alcances nuevos";
                            lst.Add(obj);
                            foreach (var item2 in lstMontos)
                            {
                                if (item2.tipoSoportes == item)
                                {
                                    lst.Add(item2);
                                }
                            }
                        }
                        break;
                    case (int)tipoSoportes.Modificacion:
                        if (lstMontos.Where(r => r.tipoSoportes == item).ToList().Count() != 0)
                        {
                            obj = new montosDTO();
                            obj.descripcion = "B.- Modificaciones por cambio de ingenieria";
                            lst.Add(obj);
                            foreach (var item2 in lstMontos)
                            {
                                if (item2.tipoSoportes == item)
                                {
                                    lst.Add(item2);
                                }
                            }
                        }
                        break;
                    case (int)tipoSoportes.Req:
                        if (lstMontos.Where(r => r.tipoSoportes == item).ToList().Count() != 0)
                        {
                            obj = new montosDTO();
                            obj.descripcion = "C.- Requerimientos en campo";
                            lst.Add(obj);
                            foreach (var item2 in lstMontos)
                            {
                                if (item2.tipoSoportes == item)
                                {
                                    lst.Add(item2);
                                }
                            }
                        }
                        break;
                    case (int)tipoSoportes.Ajuste:
                        if (lstMontos.Where(r => r.tipoSoportes == item).ToList().Count() != 0)
                        {
                            obj = new montosDTO();
                            obj.descripcion = "D.- Ajuste de volumenes por obra ejecutada";
                            lst.Add(obj);
                            foreach (var item2 in lstMontos)
                            {
                                if (item2.tipoSoportes == item)
                                {
                                    lst.Add(item2);
                                }
                            }
                        }
                        break;
                    case (int)tipoSoportes.Serv:
                        if (lstMontos.Where(r => r.tipoSoportes == item).ToList().Count() != 0)
                        {
                            obj = new montosDTO();
                            obj.descripcion = "E.- Servicios y suministros";
                            lst.Add(obj);
                            foreach (var item2 in lstMontos)
                            {
                                if (item2.tipoSoportes == item)
                                {
                                    lst.Add(item2);
                                }
                            }
                        }
                        break;
                }
            }

            return lst;
        }
        public double sumatotalSinMontoInicial(decimal alcance, decimal Modifi, decimal req, decimal ajus, decimal serv)
        {
            double total = 0;
            total = Convert.ToDouble(alcance + Modifi + req + ajus + serv);
            return total;
        }
        public double sumatotal(decimal alcance, decimal Modifi, decimal req, decimal ajus, decimal serv, decimal montoInicial, double sumaTotalDeOrdenesPrevias)
        {
            double total = 0;
            total = Convert.ToDouble(alcance + Modifi + req + ajus + serv + montoInicial + Convert.ToDecimal(sumaTotalDeOrdenesPrevias));
            return total;
        }
        public double sumaTotalDeOrdenesDeCambioPrevias(int idContrato)
        {
            using (contextSigoplan db = new contextSigoplan())
            {
                double total = 0;
                var lstOrdenesDeCambio = db.tblCO_OrdenDeCambio.Where(r => r.idContrato == idContrato).OrderByDescending(y => y.id).ToList();
                if (lstOrdenesDeCambio.Count() != 0)
                {
                    lstOrdenesDeCambio.RemoveAt(0);
                    foreach (var item in lstOrdenesDeCambio)
                    {
                        var objSoportes = db.tblCO_OC_SoportesEvidencia.Where(r => r.idOrdenDeCambio == item.id).FirstOrDefault();
                        total += sumatotalSinMontoInicial(objSoportes.alcancesNuevos, objSoportes.modificacionesPorCambio, objSoportes.requerimientosDeCampo, objSoportes.ajusteDeVolumenes, objSoportes.serviciosYSuministros);

                    }
                }
                return total;
            }
        }
        public int sumaTotalDeDiasPrevias(int idContrato)
        {
            using (contextSigoplan db = new contextSigoplan())
            {
                int total = 0;
                var lstOrdenesDeCambio = db.tblCO_OrdenDeCambio.Where(r => r.idContrato == idContrato).OrderByDescending(y => y.id).ToList();
                if (lstOrdenesDeCambio.Count() != 0)
                {
                    lstOrdenesDeCambio.RemoveAt(0);
                    foreach (var item in lstOrdenesDeCambio)
                    {
                        var objSoportes = db.tblCO_OC_SoportesEvidencia.Where(r => r.idOrdenDeCambio == item.id).FirstOrDefault();
                        total += ObtenerCantidadDeDias(objSoportes.fechaInicial, objSoportes.FechaFinal);
                    }
                }
                return total;
            }
        }
        public int ObtenerCantidadDeDias(DateTime fechaInicial, DateTime fechaFinal)
        {
            int a = 0;
            TimeSpan difFechas = (fechaFinal - fechaInicial);
            a = difFechas.Days;
            return a;
        }
        public tblP_Encabezado getEncabezadoDatos()
        {
            return _context.tblP_Encabezado.FirstOrDefault(w => w.id.Equals(1));
        }
        public decimal realizarSuma(List<tblCO_OC_Montos> lstMontos)
        {
            decimal r = 0;
            if (lstMontos.Count() != 0)
            {
                foreach (var item in lstMontos)
                {
                    r += item.importe;
                }
            }
            return r;
        }
        public Dictionary<string, object> GenerandoFirmas(List<firmasDTO> lstFirmas, int idUsuario)
        {
            contextSigoplan db = new contextSigoplan();
            resultado = new Dictionary<string, object>();
            try
            {
                tblCO_OC_Firmas objFirmas = new tblCO_OC_Firmas();
                foreach (var item in lstFirmas)
                {
                    objFirmas = db.tblCO_OC_Firmas.Where(r => r.id == item.id).FirstOrDefault();
                    if (objFirmas == null)
                    {
                        objFirmas = new tblCO_OC_Firmas();

                        objFirmas.idFirma = item.idUsuarioFirma.Value;
                        if (item.rutaFirma != "http://localhost:3676/ControlObra/GestionDeProyectos/Index")
                        {
                            objFirmas.firma = firmaBase64SinTransparencia(item.rutaFirma);
                            objFirmas.firmaDigital = GlobalUtils.CrearFirmaDigital(objFirmas.id, DocumentosEnum.AutorizacionEvaluacion, idUsuario);
                        }
                        else
                        {
                            objFirmas.firma = item.rutaFirma;
                        }
                        objFirmas.idOrdenDeCambio = item.idOrdenDeCambio;
                        db.tblCO_OC_Firmas.Add(objFirmas);
                        db.SaveChanges();
                    }
                    else
                    {
                        if (item.rutaFirma != "http://localhost:3676/ControlObra/GestionDeProyectos/Index")
                        {
                            objFirmas.firma = firmaBase64SinTransparencia(item.rutaFirma);
                            objFirmas.firmaDigital = GlobalUtils.CrearFirmaDigital(objFirmas.id, DocumentosEnum.AutorizacionEvaluacion, idUsuario);
                        }
                        else
                        {
                            objFirmas.firma = item.rutaFirma;
                        }
                        _context.SaveChanges();
                    }
                }

                for (int i = 0; i < lstFirmas.Count(); i++)
                {
                    if (lstFirmas[4].rutaFirma != "http://localhost:3676/ControlObra/GestionDeProyectos/Index")
                    {
                        var idOrden = lstFirmas.Select(y => y.idOrdenDeCambio).FirstOrDefault();
                        var orden = db.tblCO_OrdenDeCambio.Where(r => r.id == idOrden).FirstOrDefault();

                        if (orden != null)
                        {
                            orden.status = (int)ordenesEnum.Autorizada;
                            _context.SaveChanges();
                        }
                    }

                }

                resultado.Add(ITEMS, "Se guardaron las firmas correctamente.");
                resultado.Add(SUCCESS, true);

            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "Algo a ocurrido mal porfabor comuniquese con el departamento de ti." + ex.Message.ToString());
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        private string firmaBase64SinTransparencia(string imagenBase64)
        {
            //Todos esto para quitarle la transparencia a la imagen.
            byte[] imagenBytes = Convert.FromBase64String(imagenBase64.Split(',')[1]);
            using (var ms = new MemoryStream(imagenBytes, 0, imagenBytes.Length))
            {
                Image image = Image.FromStream(ms, true);

                var bmpImage = (System.Drawing.Bitmap)image;
                var bmp = new System.Drawing.Bitmap(bmpImage.Size.Width, bmpImage.Size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                var g = System.Drawing.Graphics.FromImage(bmp);
                g.Clear(System.Drawing.Color.White);
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.DrawImage(bmpImage, 0, 0);

                using (MemoryStream t = new MemoryStream())
                {
                    bmp.Save(t, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageBytesBmp = t.ToArray();
                    string base64 = Convert.ToBase64String(imageBytesBmp);
                    return imagenBase64.Split(',')[0] + "," + base64;
                }
            }
        }
        public List<tblCO_OC_Firmas> obtenerFirmas(int idOrdenDeCambio)
        {
            contextSigoplan db = new contextSigoplan();
            List<tblCO_OC_Firmas> lstFirmas = new List<tblCO_OC_Firmas>();

            lstFirmas = db.tblCO_OC_Firmas.Where(r => r.idOrdenDeCambio == idOrdenDeCambio).ToList();

            return lstFirmas;
        }
        public Dictionary<string, object> AutorizarOrdenDeCambio(int id, int tipo)
        {
            contextSigoplan db = new contextSigoplan();
            resultado = new Dictionary<string, object>();
            try
            {
                var obj = db.tblCO_OrdenDeCambio.Where(r => r.id == id).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.fechaVobo1 == null)
                    {
                        obj.status = (int)ordenesEnum.PendienteVobo2;
                        obj.fechaVobo1 = DateTime.Now;
                        db.SaveChanges();

                        resultado.Add(ITEMS, "Autorizado Vobo 1 con exito");
                        resultado.Add(SUCCESS, true);
                        return resultado;
                    }
                    if (obj.fechaVobo2 == null || tipo == 2)
                    {
                        obj.status = (int)ordenesEnum.PendientePorAutorizar;
                        obj.fechaVobo2 = DateTime.Now;
                        db.SaveChanges();

                        resultado.Add(ITEMS, "Autorizado Vobo 2 con exito");
                        resultado.Add(SUCCESS, true);
                        return resultado;
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "Algo ocurrio mal comuniquese con el departamento de ti" + ex.Message.ToString());
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> RechazarOrdenDeCambio(int id)
        {
            contextSigoplan db = new contextSigoplan();
            resultado = new Dictionary<string, object>();
            try
            {
                var obj = db.tblCO_OrdenDeCambio.Where(r => r.id == id).FirstOrDefault();
                if (obj != null)
                {
                    obj.status = (int)ordenesEnum.Rechazada;
                    db.SaveChanges();
                }
                resultado.Add(ITEMS, "Rechazada con exito");
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "Algo ocurrio mal comuniquese con el departamento de ti" + ex.Message.ToString());
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> Autorizar(int idOrdenDeCambio, int idUsuario, string firma)
        {
            resultado = new Dictionary<string, object>();
            contextSigoplan db = new contextSigoplan();
            try
            {
                var obj = db.tblCO_OC_Firmas.Where(r => r.idOrdenDeCambio == idOrdenDeCambio && r.idFirma == idUsuario && r.Autorizando == true).FirstOrDefault();
                if (obj != null)
                {
                    if (firma == null || firma == "")
                    {
                        firma = "";
                        obj.firma = firma;
                    }
                    else
                    {
                        obj.firma = firmaBase64SinTransparencia(firma);
                    }

                    obj.firmaDigital = GlobalUtils.CrearFirmaDigital(obj.id, DocumentosEnum.AutorizacionEvaluacion, idUsuario);
                    obj.Autorizando = false;
                    obj.fechaAutorizacion = DateTime.Now;
                    obj.Estado = true;
                    db.SaveChanges();
                    int a = obj.idRow + 1;
                    var objSiguiente = db.tblCO_OC_Firmas.Where(r => r.idOrdenDeCambio == idOrdenDeCambio && r.idRow == a).FirstOrDefault();
                    if (objSiguiente != null)
                    {
                        objSiguiente.Autorizando = true;
                        db.SaveChanges();
                        var lstFirmas = db.tblCO_OC_Firmas.Where(r => r.idOrdenDeCambio == idOrdenDeCambio).ToList();
                        foreach (var item in lstFirmas)
                        {
                            var objSubcontratista = db.tblP_Usuario.Where(r => r.id == item.idFirma).FirstOrDefault();
                            var subject = "Tienen una orden de cambio para autorizar ";
                            var body = @"Buen dia " + objSubcontratista.nombre_completo
                                + " Porfavor de revisar la pagina de sigoplan para revisar la orden de cambio existente <br>"
                                + htmlCorreo(lstFirmas);
                            List<string> correos = new List<string>();
                            correos.Add(objSubcontratista.correo);
#if DEBUG
                            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, new List<string> { "aaron.gracia@construplan.com.mx" });
#else
                            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, correos);
#endif
                        }

                    }

                    if (obj.idRow == 5)
                    {
                        tblCO_OrdenDeCambio objOrden = db.tblCO_OrdenDeCambio.Where(r => r.id == idOrdenDeCambio).FirstOrDefault();
                        objOrden.status = (int)ordenesEnum.Autorizada;
                        _context.SaveChanges();
                    }

                    resultado.Add(ITEMS, "Autorizando con exito.");
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add(ITEMS, "Debido ala autorizacion escalonada no se te permite autorizar.");
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "Algo ocurrio mal comuniquese con el departamento de ti" + ex.Message.ToString());
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> Rechazar(int idOrdenDeCambio, int idUsuario, string firma)
        {
            contextSigoplan db = new contextSigoplan();
            resultado = new Dictionary<string, object>();

            try
            {
                var obj = db.tblCO_OC_Firmas.Where(r => r.idOrdenDeCambio == idOrdenDeCambio && r.idFirma == idUsuario && r.Autorizando == true).FirstOrDefault();
                if (obj != null)
                {
                    obj.firma = firma;
                    obj.firmaDigital = GlobalUtils.CrearFirmaDigital(obj.id, DocumentosEnum.AutorizacionEvaluacion, idUsuario, TipoFirmaEnum.Rechazo);
                    obj.Autorizando = true;
                    db.SaveChanges();

                    tblCO_OrdenDeCambio objOrden = db.tblCO_OrdenDeCambio.Where(r => r.id == idOrdenDeCambio).FirstOrDefault();
                    objOrden.status = (int)ordenesEnum.PendienteVobo2;
                    objOrden.fechaVobo2 = null;
                    db.SaveChanges();

                    var objFirmas = db.tblCO_OC_Firmas.Where(e => e.idOrdenDeCambio == idOrdenDeCambio).ToList();

                    foreach (var item in objFirmas)
                    {
                        item.firma = "";
                        item.firmaDigital = "";
                        item.fechaAutorizacion = null;
                        item.Autorizando = false;
                    }

                    objFirmas.FirstOrDefault().Autorizando = true;
                    db.SaveChanges();

                    resultado.Add(ITEMS, "Autorizando con exito.");
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add(ITEMS, "Debido ala autorizacion escalonada no se te permite autorizar.");
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "Algo ocurrio mal comuniquese con el departamento de ti" + ex.Message.ToString());
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> obtenerTodasLasFirmas(string filtroCC, int filtroOrdenCambioID)
        {
            contextSigoplan db = new contextSigoplan();

            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener el listado de autorizantes.";
                if (string.IsNullOrEmpty(filtroCC)) { throw new Exception(mensajeError); }
                if (filtroOrdenCambioID <= 0) { throw new Exception(mensajeError); }
                #endregion

                //filtroCC = "162";
                #region SE OBTIENE LISTADO DE AUTORIZANTES DE ORDEN DE CAMBIO EN BASE AL CC SELECCIONADO
                // SE UTILIZA ORDEN = 5, YA QUE ES EL IDENTIFICADOR DE "ORDEN DE CAMBIO" DEL CATALOGO DE FACULTAMIENTOS.
                tblFA_Plantilla objPlantilla = _context.tblFA_Plantilla.Where(w => w.orden == 5 && w.esActiva).FirstOrDefault();
                if (objPlantilla == null)
                    throw new Exception("El CC no cuenta con listado de autorizantes.");

                List<firmaOCDTO> lstAutorizantesFacultamientos = _context.tblP_CC.Where(w => w.cc == filtroCC && w.estatus).Join(
                    _context.tblFA_Paquete.Where(w => w.estado == 1),
                    c => c.id,
                    p => p.ccID,
                    (c, p) => new { c, p }
                ).Join(
                    _context.tblFA_Facultamiento.Where(w => w.plantillaID == objPlantilla.id && w.aplica && w.plantillaID == objPlantilla.id),
                    cp => cp.p.id,
                    f => f.paqueteID,
                    (p, f) => new { p, f }
                ).Join(
                    _context.tblFA_Empleado.Where(w => w.esActivo && w.aplica),
                    cpf => cpf.f.id,
                    e => e.facultamientoID,
                    (cpf, e) => new { cpf, e }
                ).Join(
                    _context.tblFA_ConceptoPlantilla.Where(w => w.plantillaID == objPlantilla.id && w.esActivo),
                    cpfe => cpfe.e.conceptoID,
                    co => co.id,
                    (cpfe, co) => new firmaOCDTO
                    {
                        puesto = !string.IsNullOrEmpty(co.concepto) ? co.concepto.Trim() : null,
                        nombre_completo = !string.IsNullOrEmpty(cpfe.e.nombreEmpleado) ? cpfe.e.nombreEmpleado.Trim() : null,
                        puedeAutorizar = (string)vSesiones.sesionUsuarioDTO.cveEmpleado == cpfe.e.claveEmpleado.ToString() ? true : false,
                        claveEmpleado = cpfe.e.claveEmpleado.ToString(),
                    }
                ).ToList();

                List<firmaOCDTO> lstFirmasDTO = new List<firmaOCDTO>();
                firmaOCDTO objFirmasDTO = new firmaOCDTO();
                List<ValidarFirmaDTO> lstFirmaValidar = new List<ValidarFirmaDTO>();

                bool esRechazada = false;
                bool esTodasAutorizadas = true;

                foreach (var item in lstAutorizantesFacultamientos)
                {
                    objFirmasDTO = new firmaOCDTO();
                    objFirmasDTO.puesto = item.puesto;
                    objFirmasDTO.nombre_completo = item.nombre_completo;

                    objFirmasDTO.idUsuario = GetUsuarioID(item.claveEmpleado);

                    var objFirma = GetEstatusFirmaAutorizante(filtroOrdenCambioID, objFirmasDTO.idUsuario);

                    objFirmasDTO.strEstatusFirma = objFirma.strEstatusFirma;
                    objFirmasDTO.fechaAutorizacion = objFirma.fechaAutorizacion;
                    objFirmasDTO.estatusFirma = objFirma.estatusFirma;
                    objFirmasDTO.comentarioRechazo = objFirma.comentarioRechazo;

                    if (esRechazada)
                    {
                        objFirmasDTO.puedeAutorizar = false;
                        objFirmasDTO.strEstatusFirma = "-";

                    }
                    else
                    {
                        objFirmasDTO.puedeAutorizar = item.puedeAutorizar && objFirmasDTO.strEstatusFirma == "PENDIENTE";

                    }

                    if (objFirmasDTO.estatusFirma == 3)
                    {
                        esRechazada = true;
                    }

                    if (objFirmasDTO.estatusFirma != 2)
                    {
                        esTodasAutorizadas = false;
                    }

                    objFirmasDTO.tieneArchivo = objFirma.tieneArchivo;

                    lstFirmaValidar.Add(new ValidarFirmaDTO() { firmante = objFirma.idFirma, estatusFirmo = (objFirma.estatusFirma == 2 ? true : false) });

                    lstFirmasDTO.Add(objFirmasDTO);
                }
                var listaTotalAutorizanes = lstAutorizantesFacultamientos.Count();

                bool esUltimoFirmante = false;
                bool firmasCompletas = false;
                var lstFirmasNoAutorizadas = lstFirmaValidar.Where(e => e.estatusFirmo == false).ToList();

                if (lstFirmasNoAutorizadas.Count() == 0)
                {
                    var objLastFirmante = lstFirmasDTO.OrderByDescending(e => e.fechaAutorizacion).FirstOrDefault();

                    if (vSesiones.sesionUsuarioDTO.id == objLastFirmante.idUsuario)
                    {
                        esUltimoFirmante = true;
                    }
                    firmasCompletas = true;

                }
                var objOC = db.tblCO_OrdenDeCambio.FirstOrDefault(e => e.id == filtroOrdenCambioID);

                if (esRechazada && objOC.status != 2)
                {
                    if (objOC != null)
                    {
                        objOC.status = (int)ordenesEnum.Rechazada;
                        db.SaveChanges();

                    }
                }
                else if (esTodasAutorizadas && objOC.status != 1)
                {
                    if (objOC != null)
                    {
                        objOC.status = (int)ordenesEnum.Autorizada;
                        db.SaveChanges();

                    }
                }

                #endregion

                resultado.Add(SUCCESS, true);

                resultado.Add("firmaCompletas", firmasCompletas);
                resultado.Add("ultimoFirmante", esUltimoFirmante);
                resultado.Add("lstFirmasDTO", lstFirmasDTO);
                if (lstAutorizantesFacultamientos.Count() <= 0)
                    resultado.Add(MESSAGE, string.Format("El CC {0} no cuenta con facultamiento de autorizantes.", filtroCC));
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "obtenerTodasLasFirmas", e, AccionEnum.CONSULTA, 0, new { filtroCC = filtroCC });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetEstatusGlobalOrdenesCambio(string cc)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LA CANTIDAD DE ORDENES DE CAMBIO EN ESTATUS PENDIENTE, AUTORIZADAS Y RECHAZADAS DE MANERA GLOBAL
                // SE OBTIENE LISTADO DE ORDENES DE CAMBIO
                contextSigoplan db = new contextSigoplan();
                List<tblCO_OrdenDeCambio> lstOrdenesCambio = db.tblCO_OrdenDeCambio.ToList().Where(x =>
                    (cc != "" ? (x.cc.Trim() == cc) : true)
                ).ToList();
                List<tblCO_OC_Firmas> lstFirmas = db.tblCO_OC_Firmas.ToList();

                int cantPendientes = 0, cantAutorizadas = 0, cantRechazadas = 0;
                foreach (var item in lstOrdenesCambio)
                {
                    int cantFirmas = lstFirmas.Where(w => w.idOrdenDeCambio == item.id).Count();
                    int cantFirmasAutorizadas = lstFirmas.Where(w => w.idOrdenDeCambio == item.id && w.estatusFirma == (int)EstadoFirmaEnum.AUTORIZADO).Count();
                    int cantFirmasRechazadas = lstFirmas.Where(w => w.idOrdenDeCambio == item.id && w.estatusFirma == (int)EstadoFirmaEnum.RECHAZADO).Count();

                    if (cantFirmasRechazadas >= 1)
                        cantRechazadas++;
                    else if (cantFirmasAutorizadas == cantFirmas)
                        cantAutorizadas++;
                    else
                        cantPendientes++;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("cantPendientes", cantPendientes);
                resultado.Add("cantAutorizadas", cantAutorizadas);
                resultado.Add("cantRechazadas", cantRechazadas);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetEstatusGlobalOrdenesCambio", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private int GetUsuarioID(string claveEmpleado)
        {
            int idUsuario = 0;
            try
            {
                #region SE OBTIENE EL USUARIO_ID
                tblP_Usuario objUsuario = _context.tblP_Usuario.Where(w => w.cveEmpleado == claveEmpleado && w.estatus).FirstOrDefault();
                if (objUsuario == null)
                    throw new Exception("Ocurrió un error al obtener la información del usuario.");

                idUsuario = objUsuario.id;
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetUsuarioID", e, AccionEnum.CONSULTA, 0, new { claveEmpleado = claveEmpleado });
                return idUsuario;
            }
            return idUsuario;
        }

        private firmaOCDTO GetEstatusFirmaAutorizante(int filtroOrdenCambioID, int idUsuario)
        {
            firmaOCDTO objFirmaDTO = new firmaOCDTO();
            try
            {
                #region VALIDACIONES.
                string mensajeError = "Ocurrió un error al obtener el estatus de la firma del autorizante.";
                if (filtroOrdenCambioID <= 0) { throw new Exception(mensajeError); }
                if (idUsuario <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE EL ESTATUS DE LA FIRMA DEL AUTORIZANTE.
                contextSigoplan db = new contextSigoplan();
                tblCO_OC_Firmas objFirmaOC = db.tblCO_OC_Firmas.Where(w => w.idOrdenDeCambio == filtroOrdenCambioID && w.idFirma == idUsuario).FirstOrDefault();
                if (objFirmaOC == null)
                {
                    objFirmaDTO.strEstatusFirma = EnumHelper.GetDescription((EstadoFirmaEnum.PENDIENTE));
                    objFirmaDTO.fechaAutorizacion = null;
                }
                else
                {
                    switch (objFirmaOC.estatusFirma)
                    {
                        case (int)EstadoFirmaEnum.AUTORIZADO:
                            objFirmaDTO.strEstatusFirma = EnumHelper.GetDescription((EstadoFirmaEnum.AUTORIZADO));
                            break;
                        case (int)EstadoFirmaEnum.RECHAZADO:
                            objFirmaDTO.strEstatusFirma = EnumHelper.GetDescription((EstadoFirmaEnum.RECHAZADO));
                            objFirmaDTO.comentarioRechazo = objFirmaOC.comentarioRechazo;
                            break;
                    }
                    objFirmaDTO.idFirma = objFirmaOC.idFirma;
                    objFirmaDTO.tieneArchivo = !string.IsNullOrEmpty(objFirmaOC.ubicacionArchivoFirmado);
                    objFirmaDTO.fechaAutorizacion = objFirmaOC.fechaAutorizacion;
                    objFirmaDTO.estatusFirma = objFirmaOC.estatusFirma;
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetEstatusFirmaAutorizante", e, AccionEnum.CONSULTA, 0, new { filtroOrdenCambioID = filtroOrdenCambioID, idUsuario = idUsuario });
                return objFirmaDTO;
            }
            return objFirmaDTO;
        }
        public Dictionary<string, object> GuardarDocumentoFirmado(firmasDTO paramentros)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    contextSigoplan db = new contextSigoplan();
                    tblCO_OC_Firmas objArchivoFirmado = new tblCO_OC_Firmas();
                    var lstFirmas = db.tblCO_OC_Firmas.Where(a => a.idOrdenDeCambio == paramentros.idOrdenDeCambio).FirstOrDefault();


                    string rutaArchivoFirmado = paramentros.ArchivoFirmado == null ? "" : Path.Combine(RutaBaseArchivoFirmado, paramentros.ArchivoFirmado.FileName);

                    string nombreArchivoImagenAntes = RutaBaseArchivoFirmado + "\\" + ObtenerFormatoNombreArchivo(Path.GetFileNameWithoutExtension(rutaArchivoFirmado), rutaArchivoFirmado);

                    lstFirmas.ubicacionArchivoFirmado = nombreArchivoImagenAntes;
                    //db.tblCO_OC_Firmas.Add(objArchivoFirmado);
                    db.SaveChanges();


                    var listaRutaArchivosFirmados = new List<Tuple<HttpPostedFileBase, string>>();
                    if (paramentros.ArchivoFirmado != null)
                        listaRutaArchivosFirmados.Add(Tuple.Create(paramentros.ArchivoFirmado, nombreArchivoImagenAntes));


                    foreach (var item in listaRutaArchivosFirmados)
                    {
                        if (GlobalUtils.SaveHTTPPostedFile(item.Item1, item.Item2) == false)
                        {
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                            return resultado;
                        }
                    }

                    resultado.Add(ITEMS, "Firma Cargada con exito");
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    resultado.Add(ITEMS, "Algo ocurrio mal favor de comunicarse con TI.");
                    resultado.Add(SUCCESS, false);
                }

                return resultado;
            }
        }


        public Dictionary<string, object> AutorizarRechazarOrdenCambio(bool esAutorizar, int idOrdenCambio, string comentarioRechazo)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    string mensajeError = string.Format("Ocurrió un error al {0} el orden de cambio.", esAutorizar ? "autorizar" : "rechazar");
                    if (idOrdenCambio <= 0) { throw new Exception(mensajeError); }
                    #endregion

                    contextSigoplan db = new contextSigoplan();
                    var ordenCambio = db.tblCO_OrdenDeCambio.FirstOrDefault(x => x.id == idOrdenCambio);
                    var facultamientos = getAutorizantesCentroCosto(ordenCambio.cc);
                    var facultamientoFirmante = facultamientos.FirstOrDefault(x => x.tblp_usuario_id == vSesiones.sesionUsuarioDTO.id);
                    var puestoFirmante = "";
                    if (facultamientoFirmante != null)
                    {
                        puestoFirmante = facultamientoFirmante.puesto;
                    }

                    #region SE AUTORIZA/RECHAZA EL ORDEN DE CAMBIO
                    tblCO_OC_Firmas objFirma = new tblCO_OC_Firmas();
                    objFirma.idOrdenDeCambio = idOrdenCambio;
                    objFirma.idFirma = (int)vSesiones.sesionUsuarioDTO.id;
                    objFirma.firmaDigital = GlobalUtils.CrearFirmaDigital(idOrdenCambio, DocumentosEnum.FirmaOC, (int)vSesiones.sesionUsuarioDTO.id, esAutorizar ? TipoFirmaEnum.Autorizacion : TipoFirmaEnum.Rechazo);
                    objFirma.estatusFirma = esAutorizar ? (int)EstadoFirmaEnum.AUTORIZADO : (int)EstadoFirmaEnum.RECHAZADO;
                    objFirma.fechaAutorizacion = DateTime.Now;
                    objFirma.comentarioRechazo = comentarioRechazo;
                    objFirma.nombreFirmante = PersonalUtilities.NombreCompletoMayusculas(vSesiones.sesionUsuarioDTO.nombre, vSesiones.sesionUsuarioDTO.apellidoPaterno, vSesiones.sesionUsuarioDTO.apellidoMaterno);
                    objFirma.puestoFirmante = puestoFirmante;
                    db.tblCO_OC_Firmas.Add(objFirma);
                    db.SaveChanges();
                    #endregion

                    var alerta = _context.tblP_Alerta.FirstOrDefault(x => x.objID == idOrdenCambio && x.userRecibeID == vSesiones.sesionUsuarioDTO.id && !x.visto && x.sistemaID == 3);
                    if (alerta != null)
                    {
                        alerta.visto = true;
                        _context.SaveChanges();
                    }


                    var listaUsuarios = _context.tblP_Usuario.ToList();

                    var firmantes = db.tblCO_OC_Firmas.Where(x => x.idOrdenDeCambio == idOrdenCambio).ToList();

                    var nombre = listaUsuarios.Where(x => x.id == objFirma.idFirma).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault();
                    var correo = listaUsuarios.Where(x => x.id == objFirma.idFirma).FirstOrDefault().correo;

                    var listaFirmante = db.tblCO_OC_Firmas.Where(x => x.idOrdenDeCambio == idOrdenCambio).ToList();

                    var losqueAutorizaron = new List<tblCO_OC_Firmas>();
                    List<string> correos = new List<string>();
                    foreach (var item in facultamientos)
                    {
                        var usuarioX = listaFirmante.FirstOrDefault(x => x.idFirma == item.tblp_usuario_id);
                        if (usuarioX != null)
                        {

                            var au = new tblCO_OC_Firmas();
                            au.idFirma = usuarioX.idFirma;
                            au.firmaDigital = usuarioX.firmaDigital;
                            au.puesto = item.puesto;
                            au.estatusFirma = usuarioX.estatusFirma;
                            au.fechaAutorizacion = usuarioX.fechaAutorizacion;
                            losqueAutorizaron.Add(au);
                        }
                        else
                        {
                            var au = new tblCO_OC_Firmas();
                            au.idFirma = item.tblp_usuario_id;
                            au.firmaDigital = "PENDIENTE";
                            au.puesto = item.puesto;
                            au.estatusFirma = 1;
                            au.fechaAutorizacion = null;
                            losqueAutorizaron.Add(au);
                        }

                        correos.Add(item.correo);

                    }
#if DEBUG
                    correos = new List<string> { "martin.zayas@construplan.com.mx", "aaron.gracia@construplan.com.mx" };
#endif

                    var subject = "Se ha " + (esAutorizar ? "autorizado " : "rechazado ") + "orden de cambio " + (ordenCambio != null ? ordenCambio.NoOrden : "");
                    var body = @" <p> Buen dia " + nombre + "</p>";
                    body += htmlCorreoAutorizarRechazar(losqueAutorizaron);


                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, correos);


                    if (!esAutorizar)
                    {
                        var alertas = _context.tblP_Alerta.Where(x => x.objID == idOrdenCambio && !x.visto && x.sistemaID == 3).ToList();
                        foreach (var item in alertas)
                        {
                            item.visto = true;
                            _context.SaveChanges();
                        }
                    }

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, idOrdenCambio, JsonUtils.convertNetObjectToJson(objFirma));

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, esAutorizar ? "Se ha autorizado con éxito." : "Se ha rechazado con éxito.");
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "AutorizarRechazarOrdenCambio", e, AccionEnum.CONSULTA, 0, new { esAutorizar = esAutorizar, idOrdenCambio = idOrdenCambio });
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public List<tblPUsuarioDTO> obtenerTodlosUsuarios()
        {
            var sql = @"SELECT * FROM tblP_Usuario WHERE estatus=1";
            List<tblPUsuarioDTO> lstUsuarios = new List<tblPUsuarioDTO>();
#if DEBUG
            //using (var conexion = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTASPRUEBA()))
            using (var conexion = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
            {
                conexion.Open();
                lstUsuarios = conexion.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                conexion.Close();
            }
#else
            using (var conexion = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
            {
                conexion.Open();
                lstUsuarios = conexion.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                conexion.Close();
            }
#endif
            return lstUsuarios;
        }

        public string obtenerPuestosEstaticos(int idRow)
        {
            string puesto = "";
            switch (idRow)
            {
                case 1:
                    puesto = "Gerente de Administración de Proyectos";
                    break;
                case 2:
                    puesto = "Gerente de Área";
                    break;
                case 3:
                    puesto = "Director de División";
                    break;
                case 4:
                    puesto = "Gerente de Proyecto";
                    break;
                case 5:
                    puesto = "Director General";
                    break;

            }
            return puesto;
        }

        public Dictionary<string, object> autorizarArchivoFirmado(ordenesDeCambioDTO parametro)
        {
            contextSigoplan db = new contextSigoplan();
            resultado = new Dictionary<string, object>();
            try
            {
                var archivoValidado = db.tblCO_OrdenDeCambio.Where(a => a.id == parametro.id).FirstOrDefault();

                archivoValidado.archivoValidado = true;
                //db.tblCO_OC_Firmas.Add(objArchivoFirmado);


                resultado.Add(SUCCESS, true);
                db.SaveChanges();
                //resultado.Add(MESSAGE, "Firmas validadas");

            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, "Algo ocurrio mal comuniquese con el departamento de ti" + ex.Message.ToString());
                resultado.Add(SUCCESS, false);
            }
            return resultado;

        }
        public Dictionary<string, object> obtenerOrdenesDeCambiabosPorAutorizar(string cc, int estatus, int idUsuario, int tipo)
        {
            contextSigoplan db = new contextSigoplan();
            resultado = new Dictionary<string, object>();
            try
            {
                var lst = new List<ordenesDeCambioDTO>();
                if (cc == null)
                    cc = "";

                Core.Entity.SubContratistas.Usuarios.tblP_Usuarios objUsuario = new Core.Entity.SubContratistas.Usuarios.tblP_Usuarios();
                tblX_SubContratista objSubcontratista = new tblX_SubContratista();

                lst = db.tblCO_OrdenDeCambio.Where(r =>
                    (cc == "" ? r.cc == r.cc : r.cc == cc)
                    && r.status == estatus
                    && (tipo == 3 ? r.idSubContratista == objSubcontratista.id : r.idSubContratista == r.idSubContratista)
                    ).ToList().Select(y => new ordenesDeCambioDTO
                    {
                        id = y.id,
                        fechaEfectiva = y.fechaEfectiva,
                        Proyecto = y.Proyecto,
                        CLiente = y.CLiente,
                        Contratista = y.Contratista,
                        Direccion = y.Direccion,
                        NoOrden = y.NoOrden,
                        esCobrable = y.esCobrable,
                        cc = GetCC_Descripcion(y.cc),
                        filtroCC = y.cc,
                        noContrato = db.tblX_Contrato.Where(w => w.id == y.idContrato).FirstOrDefault().numeroContrato,
                        Antecedentes = y.Antecedentes,
                        otrasCondicioes = y.otrasCondicioes,
                        lstMontos = db.tblCO_OC_Montos.Where(r => r.idOrdenDeCambio == y.id).ToList().Count() == 0 ? null : db.tblCO_OC_Montos.Where(r => r.idOrdenDeCambio == y.id).ToList(),
                        lstSoportesEvidencia = db.tblCO_OC_SoportesEvidencia.Where(r => r.idOrdenDeCambio == y.id).FirstOrDefault() == null ? null : db.tblCO_OC_SoportesEvidencia.Where(r => r.idOrdenDeCambio == y.id).ToList().Select(n => new SoportesEvidenciaDTO
                        {
                            id = n.id,
                            idOrdenDeCambio = n.idOrdenDeCambio,
                            alcancesNuevos = n.alcancesNuevos,
                            modificacionesPorCambio = n.modificacionesPorCambio,
                            requerimientosDeCampo = n.requerimientosDeCampo,
                            ajusteDeVolumenes = n.ajusteDeVolumenes,
                            serviciosYSuministros = n.serviciosYSuministros,
                            fechaInicial = n.fechaInicial,
                            FechaFinal = n.FechaFinal,
                            MontoContratoOriginal = db.tblX_Contrato.Where(r => r.id == y.idContrato).FirstOrDefault().montoContractual,
                            MontoContratoOriginalSuma = 0,
                        }).FirstOrDefault(),
                        status = y.status,
                        voboPMO = y.voboPMO,
                        idSubContratista = y.idSubContratista,
                        esValidada = y.esValidada,
                        archivoValidado = y.archivoValidado.HasValue && y.archivoValidado.Value,
                        archivoCargado = db.tblCO_OC_Firmas.Any(x => x.idOrdenDeCambio == y.id && x.ubicacionArchivoFirmado != null),

                        turno = db.tblCO_OC_Firmas.Where(r => r.Autorizando == true && r.idOrdenDeCambio == y.id).FirstOrDefault() == null ? 0 : db.tblCO_OC_Firmas.Where(r => r.Autorizando == true && r.idOrdenDeCambio == y.id).FirstOrDefault().idRow,
                    }).OrderByDescending(n => n.id).ToList();

                resultado.Add(ITEMS, lst);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "Algo ocurrio mal comuniquese con el departamento de ti" + ex.Message.ToString());
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public List<Core.Entity.SubContratistas.Usuarios.tblP_Usuarios> ListUsersByNameWithException(string user, string cc)
        {
            contextSigoplan db = new contextSigoplan();
            if (user.Length == 0)
            {
                return db.tblP_Usuario.Where(x => x.estatus == true && !string.IsNullOrEmpty(x.correo) == false && x.tipo == 3)
                   .OrderBy(x => x.nombre_completo)
                    .ThenBy(x => x._user)
                    .Take(10).ToList();
            }
            else
            {
                var lst = db.tblP_Usuario.Where(a => (a.estatus == true && !string.IsNullOrEmpty(a.correo)) && (a.nombre_completo).Contains(user) && a.tipo == 3)
                    .OrderBy(x => x.nombre_completo)
                    .ThenBy(x => x._user)
                    .Take(10).ToList();
                return lst;
            }
        }
        public List<Core.Entity.Principal.Usuarios.tblP_Usuario> ListUsersByNameWithExceptionConstruplan(string user, string cc)
        {
            contextSigoplan db = new contextSigoplan();

            var lstCC = db.tblCO_OC_GestionFirmas.Where(r => r.cc.Contains(cc) && r.estatus).ToList().Select(y => y.idEmpleado).ToList();

            if (user.Length == 0)
            {
                return _context.tblP_Usuario.Where(x => (lstCC.Count() != 0 ? lstCC.Contains(x.id) : x.id == x.id) && x.estatus == true && !string.IsNullOrEmpty(x.correo) == false)
                   .OrderBy(x => x.nombre)
                    .ThenBy(x => x.nombreUsuario)
                    .Take(10).ToList();
            }
            else
            {
                return _context.tblP_Usuario.Where(a => (lstCC.Count() != 0 ? lstCC.Contains(a.id) : a.id == a.id) && (a.estatus == true && !string.IsNullOrEmpty(a.correo)) && (a.nombre).Contains(user))
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.nombreUsuario)
                    .Take(10).ToList();
            }
        }
        public Dictionary<string, object> obtenerArchivos(int id)
        {
            contextSigoplan db = new contextSigoplan();
            resultado = new Dictionary<string, object>();

            var lst = db.tblCO_OC_SoportesEvidencia.Where(r => r.idOrdenDeCambio == id).FirstOrDefault();

            resultado.Add(ITEMS, lst);
            resultado.Add(SUCCESS, true);

            return resultado;
        }
        public byte[] DescargarArchivos(long idDet, int tipo)
        {
            contextSigoplan db = new contextSigoplan();
            var resultado = new Dictionary<string, object>();
            Stream fileStream;
            try
            {
                var obj = db.tblCO_OC_SoportesEvidencia.Where(x => x.idOrdenDeCambio == idDet).FirstOrDefault();
                var objFirma = db.tblCO_OC_Firmas.Where(x => x.idOrdenDeCambio == idDet).FirstOrDefault();
                string pathExamen = "";
                switch (tipo)
                {

                    case 1:
                        pathExamen = obj.antecedentesArchivos;
                        break;
                    case 2:
                        pathExamen = objFirma.ubicacionArchivoFirmado;
                        break;
                    //case 2:
                    //    pathExamen = obj.modificacionArchvios;
                    //    break;
                    //case 3:
                    //    pathExamen = obj.requerimientosArchivos;
                    //    break;
                    //case 4:
                    //    pathExamen = obj.ajusteDeVolumenesArchivos;
                    //    break;
                    //case 5:
                    //    pathExamen = obj.serviciosYSuministrosArchivos;
                    //    break;
                }
                fileStream = GlobalUtils.GetFileAsStream(pathExamen);

            }
            catch (Exception e)
            {
                fileStream = null;
            }

            //resultado.Add("nombreDescarga", version.nombre);
            //resultado.Add(SUCCESS, true);
            return ReadFully(fileStream);
        }
        public string getFileName(long idDet, int tipo)
        {
            contextSigoplan db = new contextSigoplan();
            string fileName = "";
            try
            {
                var obj = db.tblCO_OC_SoportesEvidencia.Where(x => x.idOrdenDeCambio == idDet).FirstOrDefault();
                var objFirma = db.tblCO_OC_Firmas.Where(x => x.idOrdenDeCambio == idDet).FirstOrDefault();
                string pathExamen = "";
                switch (tipo)
                {
                    case 1:
                        pathExamen = obj.antecedentesArchivos;
                        if (pathExamen != null)
                        {
                            fileName = pathExamen.Split('\\').Last();
                        }
                        break;
                    case 2:
                        pathExamen = objFirma.ubicacionArchivoFirmado;
                        if (pathExamen != null)
                        {
                            fileName = pathExamen.Split('\\').Last();
                        }
                        break;
                    //case 2:
                    //    pathExamen = obj.modificacionArchvios;
                    //    if (pathExamen != null)
                    //    {
                    //        fileName = pathExamen.Split('\\').Last();
                    //    }
                    //    break;
                    //case 3:
                    //    pathExamen = obj.requerimientosArchivos;
                    //    if (pathExamen != null)
                    //    {
                    //        fileName = pathExamen.Split('\\').Last();
                    //    }
                    //    break;
                    //case 4:
                    //    pathExamen = obj.ajusteDeVolumenesArchivos;
                    //    if (pathExamen != null)
                    //    {
                    //        fileName = pathExamen.Split('\\').Last();
                    //    }
                    //    break;
                    //case 5:
                    //    pathExamen = obj.serviciosYSuministrosArchivos;
                    //    if (pathExamen != null)
                    //    {
                    //        fileName = pathExamen.Split('\\').Last();
                    //    }
                    //    break;
                }

            }
            catch (Exception e)
            {
                fileName = "";
            }

            return fileName;
        }
        public static byte[] ReadFully(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            if (input != null)
            {
                byte[] buffer = new byte[16 * 1024];
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
            }
            return ms.ToArray();
        }
        public string htmlCorreo(List<tblCO_OC_Firmas> lstAutorizadores)
        {
            string html = "";
            contextSigoplan db = new contextSigoplan();
            html += "<style>h3 {text-align: center;}table.dataTable tbody tr td, table thead tr th, table.dataTable, .dataTables_scrollBody {";
            html += "border: 0px solid #81bd72 !important;}table.dataTable thead {font-size: 15px;background-color: #81bd72;color: white;}";
            html += ".select2-container {width: 100% !important;}.seccion {padding: 15px 25px 15px 25px;margin: 10px 5px;background-color: white;";
            html += "border-radius: 4px 4px;box-shadow: 0 0 2px 0 rgba(0,0,0,0.14), 0 2px 2px 0 rgba(0,0,0,0.12), 0 1px 3px 0 rgba(0,0,0,0.2);}";
            html += ".my-card {position: absolute;left: 40%;top: -20px;border-radius: 50%;}#txtFechaInicio {background-color: #fff;}";
            html += "</style><br><table id='tblM_AutorizanteAdquisicion' class='table-bordered hover stripe order-column dataTable no-footer' role='grid'";
            html += "aria-describedby='tblM_AutorizanteAdquisicion_info'>";
            html += "<thead>";
            html += "<tr role='row'>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Puesto</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Nombre Completo</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Fecha Autorizacion</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Firma</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Estado de la firma</th>";

            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";

            foreach (var item in lstAutorizadores)
            {
                var a = item.Estado == true ? "AUTORIZADO" : "PENDIENTE";

                html += "<tr>";
                html += "<td>" + obtenerPuestosEstaticos(item.idRow) + "</td>";
                //if (item.idRow == 1)
                //{
                //    var sql = @"SELECT * FROM tblP_Usuario WHERE estatus=1";
                //    var lstUsuarios = new List<tblPUsuarioDTO>();
                //    using (var conexion = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                //    {
                //        conexion.Open();
                //        lstUsuarios = conexion.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                //        conexion.Close();
                //    }

                //    var listaUsuarios = _context.tblP_Usuario.ToList();
                //    html += "<td>" + listaUsuarios.Where(r => r.cveEmpleado == item.idFirma.ToString()).FirstOrDefault() == null ? "" : _context.tblP_Usuario.Where(x => x.cveEmpleado == item.idFirma.ToString()).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault() + "</td>";
                //}
                //else
                //{
                html += "<td>" + _context.tblP_Usuario.Where(r => r.cveEmpleado == item.idFirma.ToString()).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault() + "</td>";
                //}
                html += "<td>" + (item.fechaAutorizacion == null ? "PENDIENTE" : item.fechaAutorizacion.Value.ToString("dd/MM//yyyy")) + "</td>";
                html += "<td>" + (item.firmaDigital == null ? "PENDIENTE" : item.firmaDigital) + "</td>";
                html += "<td>" + a + "</td>";
                html += "</tr>";
            }

            html += "</tbody>";
            html += "</table>";
            html += "</div>";

            return html;
        }

        public string htmlCorreoAutorizarRechazar(List<tblCO_OC_Firmas> lstAutorizadores)
        {
            string html = "";
            contextSigoplan db = new contextSigoplan();
            html += "<style>h3 {text-align: center;}table.dataTable tbody tr td, table thead tr th, table.dataTable, .dataTables_scrollBody {";
            html += "border: 0px solid #81bd72 !important;}table.dataTable thead {font-size: 15px;background-color: #81bd72;color: white;}";
            html += ".select2-container {width: 100% !important;}.seccion {padding: 15px 25px 15px 25px;margin: 10px 5px;background-color: white;";
            html += "border-radius: 4px 4px;box-shadow: 0 0 2px 0 rgba(0,0,0,0.14), 0 2px 2px 0 rgba(0,0,0,0.12), 0 1px 3px 0 rgba(0,0,0,0.2);}";
            html += ".my-card {position: absolute;left: 40%;top: -20px;border-radius: 50%;}#txtFechaInicio {background-color: #fff;}";
            html += "</style><br><table id='tblM_AutorizanteAdquisicion' class='table-bordered hover stripe order-column dataTable no-footer' role='grid'";
            html += "aria-describedby='tblM_AutorizanteAdquisicion_info'>";
            html += "<thead>";
            html += "<tr role='row'>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Puesto</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Nombre Completo</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Fecha Autorizacion</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Firma</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1'>Estado de la firma</th>";

            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";
            int i = 1;
            foreach (var item in lstAutorizadores)
            {
                var a = item.estatusFirma == 2 ? "AUTORIZADO" : item.estatusFirma == 3 ? "RECHAZADO" : "PENDIENTE";

                html += "<tr>";
                html += "<td>" + item.puesto + "</td>";
                i++;
                //if (item.idRow == 1)
                //{
                //    var sql = @"SELECT * FROM tblP_Usuario WHERE estatus=1";
                //    var lstUsuarios = new List<tblPUsuarioDTO>();
                //    using (var conexion = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                //    {
                //        conexion.Open();
                //        lstUsuarios = conexion.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                //        conexion.Close();
                //    }

                //    var listaUsuarios = _context.tblP_Usuario.ToList();
                //    html += "<td>" + listaUsuarios.Where(r => r.cveEmpleado == item.idFirma.ToString()).FirstOrDefault() == null ? "" : _context.tblP_Usuario.Where(x => x.cveEmpleado == item.idFirma.ToString()).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault() + "</td>";
                //}
                //else
                //{
                html += "<td>" + _context.tblP_Usuario.Where(r => r.id == item.idFirma).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault() + "</td>";
                //}
                html += "<td>" + (item.fechaAutorizacion == null ? "PENDIENTE" : item.fechaAutorizacion.Value.ToString("dd/MM//yyyy")) + "</td>";
                html += "<td>" + (item.firmaDigital == null ? "PENDIENTE" : item.firmaDigital) + "</td>";
                html += "<td>" + a + "</td>";
                html += "</tr>";
            }

            html += "</tbody>";
            html += "</table>";
            html += "</div>";

            return html;
        }
        public List<ComboDTO> obtenerPuestos()
        {
            List<Core.DTO.Principal.Generales.ComboDTO> result = new List<Core.DTO.Principal.Generales.ComboDTO>();

            try
            {
                //                string query = @"
                //                                 SELECT clave_empleado AS Value,(nombre + ' ' + ape_paterno + ' ' +ape_materno)AS Text  FROM DBA.sn_empleados WHERE puesto IN(756,872,358,755,709,789,469,899,57)
                //                   ";

                //                result = (List<Core.DTO.Principal.Generales.ComboDTO>)ContextEnKontrolNomina.Where(query).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                //                result = result.Select(y => new ComboDTO
                //                {
                //                    Value = y.Value,
                //                    Text = y.Text
                //                }).ToList();

                result = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT clave_empleado AS Value,(nombre + ' ' + ape_paterno + ' ' +ape_materno) AS Text FROM tblRH_EK_Empleados WHERE puesto IN(756,872,358,755,709,789,469,899,57)",
                });
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }
        //        public Dictionary<string, object> EnviarCorreo(int idOrdenDeCambio, Byte[] archivo, int tipoCorreo)
        //        {
        //            resultado = new Dictionary<string, object>();
        //            contextSigoplan db = new contextSigoplan();
        //            List<tblCO_OC_Notificantes> lstNotificantes = db.tblCO_OC_Notificantes.Where(r => r.idOrdenDeCambio == idOrdenDeCambio).ToList();
        //            if (lstNotificantes.Count() != 0)
        //            {


        //                foreach (var item in lstNotificantes)   
        //                {


        //                    var objUsuario = db.tblP_Usuario.Where(r => r.id == item.idUsuario).FirstOrDefault();
        //                    if (objUsuario != null)
        //                    {


        //                        switch (tipoCorreo)
        //                        {
        //                            case 1:

        //                                var subject = "Se le esta notificando de una orden de cambio.";
        //                                var body = @"Buen dia " + objUsuario.nombre_completo
        //                                    + " Tiene una orden de cambio para visualizar";
        //                                List<string> correos = new List<string>();
        //#if DEBUG
        //                                correos.Add("adan.gonzalez@construplan.com.mx");
        //                                correos.Add("adan.gonzalez@construplan.com.mx");
        //#else
        //                                    correos.Add(objUsuario.correo);
        //#endif
        //                                string nombreDelArchivo = obtenerNombreArchivo(idOrdenDeCambio);
        //                                GlobalUtils.senEmailAdjuntos(subject, body, correos, archivo, nombreDelArchivo);

        //                                break;

        //                            case 2:

        //                                subject = "Autorizaciones completas de Orden de Cambio";
        //                                nombreDelArchivo = obtenerNombreArchivo(idOrdenDeCambio);
        //                                body = @"Buen dia " + objUsuario.nombre_completo
        //                                    + " \n La orden de compra "+nombreDelArchivo+" se ha autorizado con todas las firmas.";
        //                                correos = new List<string>();
        //#if DEBUG
        //                                correos.Add("adan.gonzalez@construplan.com.mx");
        //                                correos.Add("adan.gonzalez@construplan.com.mx");
        //#else
        //                                    correos.Add(objUsuario.correo);
        //#endif

        //                                GlobalUtils.senEmailAdjuntos(subject, body, correos, null , null);

        //                                break;
        //                        }


        //                    }
        //                }
        //            }
        //            return resultado;
        //        }
        public Dictionary<string, object> EnviarCorreo(int idOrdenDeCambio, Byte[] archivo, int tipoCorreo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                contextSigoplan db = new contextSigoplan();
                List<tblCO_OC_Notificantes> lstNotificantes = db.tblCO_OC_Notificantes.Where(r => r.idOrdenDeCambio == idOrdenDeCambio).ToList();
                List<string> correosAutorizantes = new List<string>();
                var objOrden = db.tblCO_OrdenDeCambio.Where(e => e.id == idOrdenDeCambio).FirstOrDefault();
                var lstFirmantes = db.tblCO_OC_Firmas.Where(r => r.idOrdenDeCambio == idOrdenDeCambio).ToList();


                foreach (var item in lstNotificantes)
                {
                    tblCO_OC_RelClavesEmpleadosEntreBD objRelacion = db.tblCO_OC_RelClavesEmpleadosEntreBD.Where(e => e.claveEmpleadoEncontrol == item.cvEmpleados).FirstOrDefault();
                    var objNotific = _context.tblP_Usuario.Where(e => e.id == objRelacion.idUsuarioSigoplan).FirstOrDefault();
                    correosAutorizantes.Add(objNotific.correo);
                }

                correosAutorizantes = correosAutorizantes.Distinct().ToList();

                var correo = new Infrastructure.DTO.CorreoDTO();

                string head = "";
                string body = "";

                switch (tipoCorreo)
                {
                    case 1:
                        head = "AUTORIZACIONES DE ORDEN DE CAMBIO COMPLETAS";
                        body = "SE HA AUTORIZADO POR TODOS NOTIFICANTES \n\nProyecto: " + objOrden.Proyecto + ". Cliente: " + objOrden.CLiente + ".";
                        body += htmlCorreo(lstFirmantes);
                        break;

                    case 2:
                        head = "TIENE UNA ORDEN DE CAMBIO PARA AUTORIZAR";
                        body = "TIENE UNA ORDEN DE CAMBIO PARA AUTORIZAR \n\nProyecto: " + objOrden.Proyecto + ". Cliente: " + objOrden.CLiente + ".";
                        body += htmlCorreo(lstFirmantes);
                        break;

                    default:
                        head = "Algo salio mal.";
                        body = "ALGO SALIO MAL CON SU CORREO DE ORDEN DE CAMBIO CONTACTE AL DEP. DE Tecnologias de la informacion\n\nProyecto: " + objOrden.Proyecto + ". Cliente: " + objOrden.CLiente + ".";
                        break;

                }
                correo.asunto = head;
#if DEBUG
                correosAutorizantes = new List<string>() { "adan.gonzalez@construplan.com.mx" };
#else 
                
#endif
                var lstUsuarios = new List<tblPUsuarioDTO>();
                foreach (var item in lstFirmantes)
                {
                    var lstUsuarios2 = new List<tblPUsuarioDTO>();
                    if (item.idRow == 1)
                    {
                        var sql = @"SELECT * FROM tblP_Usuario WHERE estatus=1";
                        using (var conexion = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                        {
                            conexion.Open();
                            lstUsuarios2 = conexion.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                            conexion.Close();
                        }
                        var obj = lstUsuarios2.Where(r => r.id == item.idFirma).FirstOrDefault();
                        lstUsuarios.Add(obj);
                    }
                    else
                    {
                        var obj = _context.tblP_Usuario.Where(r => r.id == item.idFirma).Select(y => new tblPUsuarioDTO
                        {
                            nombre_completo = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                            correo = y.correo
                        }).FirstOrDefault();
                        lstUsuarios.Add(obj);
                    }
                }
                var lstDeCorreosFirmantes = lstUsuarios.Select(y => y.correo).ToList();
                correo.correos.AddRange(lstDeCorreosFirmantes);
                correo.correos.AddRange(correosAutorizantes);
                correo.cuerpo = body;
                correo.Enviar();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, true);
            }
            return result;
        }
        public Dictionary<string, object> agregarPermisos(facultamientosDTO parametros)
        {
            contextSigoplan db = new contextSigoplan();
            resultado = new Dictionary<string, object>();

            var obj = db.tblP_Permisos.Where(r => r.id == parametros.id && r.tiene).FirstOrDefault();

            if (obj == null)
            {
                obj = db.tblP_Permisos.Where(r => r.idUsuario == parametros.idEmpleado).FirstOrDefault();
                if (obj != null)
                {
                    resultado.Add(ITEMS, "Ya se ha registrado este empleado.");
                    resultado.Add(SUCCESS, false);
                }
                else
                {
                    obj = new tblP_Permisos();
                    obj.tiene = true;
                    obj.idUsuario = parametros.idEmpleado;
                    obj.permiso = parametros.privilegio.GetDescription();
                    db.tblP_Permisos.Add(obj);
                    db.SaveChanges();
                }
            }
            else
            {
                obj.tiene = true;
                obj.idUsuario = parametros.idEmpleado;
                obj.permiso = parametros.privilegio.GetDescription();
                db.SaveChanges();
            }

            return resultado;
        }
        public List<tblP_Permisos> obtenerPermisos(int idUsuario)
        {
            using (contextSigoplan db = new contextSigoplan())
            {
                List<string> lstGerentesPro = new List<string>() { "91", "378", "417", "418", "521", "646", "8622" };
                List<tblP_Permisos> lstPermisos = new List<tblP_Permisos>();
                List<dynamic> detalles = new List<dynamic>();

                //                string sql = @"SELECT A.clave_empleado, A.nombre +' '+ A.ape_paterno +' '+ A.ape_materno AS nombre_completo, B.puesto, B.descripcion 
                //                            FROM 
                //                                sn_empleados AS A 
                //                            INNER JOIN 
                //                                si_puestos AS B 
                //                           ON A.puesto = B.puesto 
                //                            WHERE descripcion LIKE'%gerente de proyecto%' AND A.estatus_empleado ='A'";
                //                var odbcArrendadora = new OdbcConsultaDTO();
                //                odbcArrendadora.consulta = String.Format(sql);

                //                var ClavesPuestosEmpelados = _contextEnkontrol.Select<PuestosEmpleadosDTO>(EnkontrolEnum.CplanRh, odbcArrendadora);
                var lstPrivilegios = db.tblCO_OC_GestionFirmas.Where(r => r.estatus).ToList();
                var ClavesPuestosEmpelados = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT A.clave_empleado, A.nombre +' '+ A.ape_paterno +' '+ A.ape_materno AS nombre_completo, B.puesto, B.descripcion 
                                FROM 
                                    tblRH_EK_Empleados AS A 
                                INNER JOIN 
                                    tblRH_EK_Puestos AS B 
                                ON A.puesto = B.puesto 
                                WHERE descripcion LIKE '%gerente de proyecto%' AND A.estatus_empleado ='A'",
                });

                var purasClaves = ClavesPuestosEmpelados.Select(y => Convert.ToInt32(y.clave_empleado)).ToList();

                var lstUsuarioConectado = _context.tblP_Usuario.Where(r => r.id == idUsuario).ToList();
                var lst2 = lstUsuarioConectado.Select(y => Convert.ToInt32(y.cveEmpleado)).ToList();
                lst2.Add(7922);
                lst2.Add(3841);

                using (contextSigoplan dbCon = new contextSigoplan())
                {
                    lstPermisos = dbCon.tblP_Permisos.Where(r => lst2.Contains(r.idUsuario)).ToList();
                }

                return lstPermisos;
            }
        }

        public Dictionary<string, object> GetDashboardOrdenCambio(string cc, int contrato_id, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                contextSigoplan db = new contextSigoplan();

                fechaInicio = fechaInicio.Date;
                fechaFin = fechaFin.AddDays(1).Date;

                var auxListaOrdenesCambio = db.tblCO_OrdenDeCambio.Where(x =>
                    x.fechaEfectiva >= fechaInicio &&
                    x.fechaEfectiva < fechaFin &&
                    (!string.IsNullOrEmpty(cc) ? x.cc.Trim() == cc.Trim() : true) &&
                    (contrato_id > 0 ? x.idContrato == contrato_id : true)
                ).ToList();

                var listaOrdenesCambioIds = auxListaOrdenesCambio.Select(x => x.id).ToList();
                var listaContratosIDs = auxListaOrdenesCambio.Select(x => x.idContrato).ToList();
                var listaContratos = db.tblX_Contrato.Where(x => listaContratosIDs.Contains(x.id)).ToList();
                var listaMontos = db.tblCO_OC_Montos.Where(x => listaOrdenesCambioIds.Contains(x.idOrdenDeCambio)).ToList();

                var listaOrdenesCambio = auxListaOrdenesCambio.Select(x =>
                {
                    var montoTotal = listaMontos.Where(y => y.idOrdenDeCambio == x.id).Select(z => z.importe).Sum();
                    var contrato = listaContratos.FirstOrDefault(y => y.id == x.idContrato);
                    return new OrdenCambioDTO
                        {
                            id = x.id,
                            fechaEfectiva = x.fechaEfectiva,
                            cc = x.cc,
                            idContrato = x.idContrato,
                            NoOrden = x.NoOrden,
                            numeroContrato = contrato == null ? "N/A" : contrato.numeroContrato,
                            montoContrato = contrato == null ? 0 : contrato.montoContractual,
                            montoTotalOrdenCambio = montoTotal
                        };
                }).ToList();
                var listaOrdenesCambioFiltrada = new List<OrdenCambioDTO>();

                foreach (var oc in listaOrdenesCambio)
                {
                    var listaFirmas = db.tblCO_OC_Firmas.Where(x => x.idOrdenDeCambio == oc.id).ToList();
                    var totalFirmas = listaFirmas.Count();

                    if (totalFirmas > 0)
                    {
                        if (listaFirmas.Count() == totalFirmas && listaFirmas.All(x => x.estatusFirma == (int)EstadoFirmaEnum.AUTORIZADO))
                        {
                            listaOrdenesCambioFiltrada.Add(oc);
                        }
                    }
                }

                #region Gráfica Montos
                var montoContractualInicial = listaOrdenesCambioFiltrada.Sum(x => x.montoContrato);
                var montoActualContrato = montoContractualInicial + listaOrdenesCambioFiltrada.Sum(x => x.montoTotalOrdenCambio);

                var graficaOrdenCambio = new GraficaDTO();

                graficaOrdenCambio.categorias.Add("Monto contractual inicial");
                graficaOrdenCambio.serie1Descripcion = "";
                graficaOrdenCambio.serie1.Add(montoContractualInicial);

                graficaOrdenCambio.categorias.Add("Monto actual de contrato");
                graficaOrdenCambio.serie1Descripcion = "";
                graficaOrdenCambio.serie1.Add(montoActualContrato);
                #endregion

                var tablaContratos = listaOrdenesCambioFiltrada.Select(x => new
                {
                    numeroContrato = x.numeroContrato,
                    NoOrden = x.NoOrden,
                    fechaEfectiva = x.fechaEfectiva.ToShortDateString(),
                    montoTotalOrdenCambio = x.montoTotalOrdenCambio
                });

                resultado.Add("montoTotalOrdenCambio", listaOrdenesCambioFiltrada.Sum(x => x.montoTotalOrdenCambio));
                resultado.Add("graficaOrdenCambio", graficaOrdenCambio);
                resultado.Add("tablaContratos", tablaContratos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, "ControlObraController", "GetDashboardOrdenCambio", e, AccionEnum.CONSULTA, 0, new { cc = cc, contrato_id = contrato_id, fechaInicio = fechaInicio, fechaFin = fechaFin });
            }

            return resultado;
        }

        public Dictionary<string, object> fillComboContratistasByContrato(int idContrato)
        {
            resultado.Clear();
            contextSigoplan db = new contextSigoplan();

            //try
            //{
            //    var lstContratos = db.tblX_Contrato.Where(e => e.estatus && e.id == idContrato).ToList();

            //    resultado.Add(ITEMS, lstSubC);
            //    resultado.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    resultado.Add(MESSAGE, e.Message);
            //    resultado.Add(SUCCESS, false);
            //}

            return resultado;
        }

        #endregion

        #region GESTION FACULTAMIENTO DE ORDEN DE CAMBIO

        public List<string> obtenerCC(string lstCC)
        {
            List<string> lst = new List<string>();

            var arr = lstCC.Split(',');
            foreach (var item in arr)
            {
                if (item != "")
                {
                    lst.Add(item);
                }
            }
            return lst;
        }
        public List<string> obtenerLstCC(string lstCC)
        {
            List<string> lst = new List<string>();
            contextSigoplan db = new contextSigoplan();

            var arr = lstCC.Split(',');
            foreach (var item in arr)
            {
                if (item != "")
                {
                    var lstCCResultado = _context.tblP_CC.Where(r => r.estatus).ToList();
                    var centrosdecostos = item + " - " + lstCCResultado.Where(r => r.cc == item).FirstOrDefault().descripcion;
                    lst.Add(centrosdecostos);
                }
            }
            return lst;
        }
        public Dictionary<string, object> obtenerLstFacultamientos(string cc)
        {
            contextSigoplan db = new contextSigoplan();
            resultado = new Dictionary<string, object>();
            if (cc == null)
            {
                cc = "";
            }
            var lst = db.tblCO_OC_GestionFirmas.Where(r => r.estatus).ToList();
            try
            {
                var lstGestion = lst.Where(r => r.estatus && (cc == "" ? r.cc == r.cc : r.cc.Contains(cc))).ToList().Select(y => new rFacultamientosDTO
                {
                    id = y.id,
                    idEmpleado = y.idEmpleado,
                    nombreEmpleado = _context.tblP_Usuario.Where(r => r.id == y.idEmpleado).Select(l => l.nombre + " " + l.apellidoPaterno + " " + l.apellidoMaterno).FirstOrDefault(),
                    lstcc = obtenerCC(y.cc),
                    ccDescripcion = obtenerLstCC(y.cc),
                    privilegio = y.privilegio,
                    privilegioDesc = y.privilegio.GetDescription(),
                    estatus = y.estatus
                }).ToList();
                resultado.Add(ITEMS, lstGestion);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "");
                resultado.Add(SUCCESS, false);
                throw;
            }
            return resultado;
        }
        public Dictionary<string, object> agregarEditarFacultamientos(facultamientosDTO parametros)
        {
            contextSigoplan db = new contextSigoplan();
            resultado = new Dictionary<string, object>();

            var obj = db.tblCO_OC_GestionFirmas.Where(r => r.id == parametros.id && r.estatus).FirstOrDefault();

            if (obj == null)
            {
                obj = db.tblCO_OC_GestionFirmas.Where(r => r.idEmpleado == parametros.idEmpleado && r.estatus).FirstOrDefault();
                if (obj != null)
                {
                    resultado.Add(ITEMS, "Ya se ha registrado este empleado.");
                    resultado.Add(SUCCESS, false);
                }
                else
                {
                    obj = new tblCO_OC_GestionFirmas();
                    obj.estatus = true;
                    obj.idEmpleado = parametros.idEmpleado;
                    obj.cc = parametros.cc;
                    obj.privilegio = parametros.privilegio;
                    agregarPermisos(parametros);
                    db.tblCO_OC_GestionFirmas.Add(obj);
                    db.SaveChanges();
                }
            }
            else
            {
                obj.estatus = true;
                obj.cc = parametros.cc;
                obj.privilegio = parametros.privilegio;
                db.SaveChanges();
            }

            return resultado;
        }
        public Dictionary<string, object> EliminarFacultamiento(int idFacultamiento)
        {
            contextSigoplan db = new contextSigoplan();
            resultado = new Dictionary<string, object>();
            var obj = db.tblCO_OC_GestionFirmas.Where(r => r.id == idFacultamiento && r.estatus).FirstOrDefault();
            if (obj != null)
            {
                obj.estatus = false;
                db.SaveChanges();
            }

            return resultado;
        }
        public Dictionary<string, object> obtenerFacultamiento(int idUsuario)
        {
            contextSigoplan db = new contextSigoplan();
            resultado = new Dictionary<string, object>();
            var obj = db.tblCO_OC_GestionFirmas.Where(r => r.idEmpleado == idUsuario && r.estatus).FirstOrDefault();

            return resultado;
        }
        public List<ComboDTO> obtenerCC()
        {
            List<ComboDTO> resultado = new List<ComboDTO>();
            //contextSigoplan db = new contextSigoplan();

            resultado = _context.tblP_CC.Where(r => r.estatus).ToList().Select(y => new ComboDTO
            {
                Value = y.cc,
                Text = y.cc + " - " + y.descripcion
            }).ToList();

            return resultado;
            #region Facultamientos CC
            //contextSigoplan db = new contextSigoplan();
            //var registroFacultamiento = db.tblCO_OC_GestionFirmas.FirstOrDefault(x => x.estatus && x.idEmpleado == vSesiones.sesionUsuarioDTO.id);

            //if (registroFacultamiento != null)
            //{
            //    var listaPermisoCC = obtenerCC(registroFacultamiento.cc);

            //    if (listaPermisoCC.Count() > 0)
            //    {
            //        return resultado.Where(x => listaPermisoCC.Contains(x.Value)).OrderBy(x => x.Text).ToList();
            //    }
            //    else
            //    {
            //        return new List<ComboDTO>();
            //    }
            //}
            //else
            //{
            //    return new List<ComboDTO>();
            //}
            #endregion
        }
        public List<ComboDTO> obtenerUsuarios()
        {
            List<ComboDTO> resultado = new List<ComboDTO>();
            contextSigoplan db = new contextSigoplan();

            List<string> lstGerentesPro = new List<string>() { "91", "378", "417", "418", "521", "646", "8622" };
            List<tblP_Permisos> lstPermisos = new List<tblP_Permisos>();
            List<dynamic> detalles = new List<dynamic>();


            //            string sql = @"SELECT A.clave_empleado, A.nombre +' '+ A.ape_paterno +' '+ A.ape_materno AS nombre_completo, B.puesto, B.descripcion 
            //                            FROM 
            //                                sn_empleados AS A 
            //                            INNER JOIN 
            //                                si_puestos AS B 
            //                           ON A.puesto = B.puesto 
            //                            WHERE descripcion LIKE'%gerente de proyecto%'";
            //            var odbcArrendadora = new OdbcConsultaDTO();
            //            odbcArrendadora.consulta = String.Format(sql);

            //            var ClavesPuestosEmpelados = _contextEnkontrol.Select<PuestosEmpleadosDTO>(EnkontrolEnum.CplanRh, odbcArrendadora);

            var ClavesPuestosEmpelados = _context.Select<dynamic>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = @"SELECT A.clave_empleado, A.nombre +' '+ A.ape_paterno +' '+ A.ape_materno AS nombre_completo, B.puesto, B.descripcion 
                            FROM 
                                tblRH_EK_Empleados AS A 
                            INNER JOIN 
                                tblRH_EK_Puestos AS B 
                           ON A.puesto = B.puesto 
                            WHERE descripcion LIKE'%gerente de proyecto%'",
            });

            var lstClaves = ClavesPuestosEmpelados.Select(n => n.clave_empleado).ToList();

            resultado = _context.tblP_Usuario.Where(r => lstClaves.Contains(r.cveEmpleado)).Select(y => new ComboDTO
            {
                Value = y.id.ToString(),
                Text = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno
            }).ToList();

            return resultado;
        }
        public object GetUsuariosAutocomplete(string term, bool porClave)
        {
            var usuarios = _context.tblP_Usuario
                .Where(x =>
                    x.estatus &&
                    (porClave ? x.cveEmpleado.Contains(term) : (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno).Contains(term))
                ).OrderBy(x => x.id).Take(12).ToList();

            return porClave ?
                usuarios.Select(x => new
                {
                    id = x.id,
                    value = x.cveEmpleado,
                    claveEmpleado = x.cveEmpleado,
                    nombre = GlobalUtils.ObtenerNombreCompletoUsuario(x)
                })
                :
                usuarios.Select(x => new
                {
                    id = x.id,
                    value = GlobalUtils.ObtenerNombreCompletoUsuario(x),
                    claveEmpleado = x.cveEmpleado,
                    nombre = GlobalUtils.ObtenerNombreCompletoUsuario(x)
                });
        }

        #endregion

        public int GetPrivilegioUsuario()
        {
            contextSigoplan db = new contextSigoplan();
            var registroFacultamiento = db.tblCO_OC_GestionFirmas.FirstOrDefault(x => x.estatus && x.idEmpleado == vSesiones.sesionUsuarioDTO.id);

            if (registroFacultamiento != null)
            {
                return (int)registroFacultamiento.privilegio;
            }
            else
            {
                return 0; //Usuario sin facultamiento
            }
        }

        #region METODOS GENERALES
        private string GetCC_Descripcion(string cc)
        {
            string CC_Descripcion = string.Empty;
            try
            {
                #region VALIDACIONES
                if (string.IsNullOrEmpty(cc)) { throw new Exception("Ocurrió un error al obtener el CC."); }
                #endregion

                #region SE OBTIENE EL CC CON SU DESCRIPCIÓN
                tblP_CC objCC = _context.tblP_CC.Where(w => w.cc == cc && w.estatus).FirstOrDefault();
                if (objCC != null)
                {
                    if (!string.IsNullOrEmpty(objCC.cc) && !string.IsNullOrEmpty(objCC.descripcion))
                        CC_Descripcion = string.Format("[{0}] {1}", objCC.cc, objCC.descripcion);
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetCC_Descripcion", e, AccionEnum.CONSULTA, 0, new { cc = cc });
                return CC_Descripcion;
            }
            return CC_Descripcion;
        }
        #endregion

        public MemoryStream DescargarArchivo(int id)
        {
            MemoryStream archivo = new MemoryStream();
            contextSigoplan db = new contextSigoplan();

            var ordenCambio = db.tblCO_OrdenDeCambio.First(x => x.id == id);

            MemoryStream myXMLDocument = new MemoryStream(File.ReadAllBytes(@"c:\temp\myDemoXMLDocument.xml"));

            return archivo;
        }
    }
}
