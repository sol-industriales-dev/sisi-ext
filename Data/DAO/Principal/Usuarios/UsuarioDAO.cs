using Core.DAO.Principal.Usuarios;
using Core.DTO;
using Core.DTO.Encuestas;
using Core.DTO.Principal.Menus;
using Core.DTO.Principal.Usuarios;
using Core.DTO.Sistemas;
using Core.DTO.Utils.Data;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Menus;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Entity.SeguimientoAcuerdos;
using Core.Entity.Sistemas;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.Principal.Usuario;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Principal.Usuarios
{
    public class UsuarioDAO : GenericDAO<tblP_Usuario>, IUsuarioDAO
    {


        public string getNombreUsuario(int id)
        {
            var usuario = _context.tblP_Usuario.FirstOrDefault(x => x.id == id);
            return usuario != null ? usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno : "";

        }
        public void removerAlerta(int idAlerta)
        {
            tblP_Alerta Getalerta = _context.tblP_Alerta.Where(x => x.id == idAlerta).FirstOrDefault();

            if (Getalerta != null)
            {
                Getalerta.visto = true;
                _context.SaveChanges();
            }

        }

        public List<tblP_AccionesVista> getListaAcciones()
        {
            return _context.tblP_AccionesVista.Where(x => x.vistaID == 2079).ToList();
        }

        public List<tblP_AccionesVista> getLstAccionesActual()
        {
            var idUsuario = vSesiones.sesionUsuarioDTO.id;
            var idVista = vSesiones.sesionCurrentView;
            var lstIdVistas = new List<int>() { 0, idVista };
            var lst = (from av in _context.tblP_AccionesVista
                       join avu in _context.tblP_AccionesVistatblP_Usuario on av.id equals avu.tblP_AccionesVista_id
                       where avu.tblP_Usuario_id == idUsuario && lstIdVistas.Contains(av.vistaID)
                       select av).ToList();
            return lst;
        }

        public List<usuariosEncuestasDTO> getUsuariosData(int tipoCliente)
        {
            List<tblP_Usuario> usuarios = new List<tblP_Usuario>();
            List<usuariosEncuestasDTO> dataReturn = new List<usuariosEncuestasDTO>();

            if (tipoCliente == 0)
            {
                usuarios = _context.tblP_Usuario.Where(x => x.estatus && x.usuarioGeneral == false).ToList();
            }
            else if (tipoCliente == 1)
            {
                usuarios = _context.tblP_Usuario.Where(x => x.estatus && x.cliente == false && x.usuarioGeneral == false).ToList();
            }
            else
            {
                usuarios = _context.tblP_Usuario.Where(x => x.estatus && x.cliente == true && x.usuarioGeneral == false).ToList();
            }

            var lstClavesEmpleados = string.Join(", ", usuarios.Where(x => x.cveEmpleado != null && x.cveEmpleado.Trim() != "").Select(y => y.cveEmpleado));
            var personalTodosEK = getPersonalTodosEK(lstClavesEmpleados);



            dataReturn.AddRange(usuarios.Select(x => new usuariosEncuestasDTO
            {
                id = x.id,
                Nombre = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                Empresa = x.empresa,
                CC = personalTodosEK.Where(y => x.cveEmpleado == y.clave_empleado.ToString()).Select(z => z.cc).FirstOrDefault() ?? "",
                cveEmpleado = x.cveEmpleado,
                Cliente = (x.cliente ? "CLIENTE EXTERNO" : "CLIENTE INTERNO"),
                departamento = x.puesto.departamento.descripcion,
                accion = "",
                crearEncuesta = (_context.tblP_MenutblP_Usuario.FirstOrDefault(y => y.tblP_Menu_id == 2080 && y.tblP_Usuario_id == x.id) != null ? true : false)
            }));

            return dataReturn;
        }

        public List<tblRH_CatEmpleados> getPersonalByClaveEmpleado(string claveEmpleado)
        {
            List<tblRH_CatEmpleados> lstCatEmpleado = new List<tblRH_CatEmpleados>();

//            var getCatEmpleado = @"
//                                    SELECT 
//                                        A.clave_empleado, 
//                                        (LTRIM(RTRIM(A.nombre)) + ' ' + REPLACE(A.ape_paterno, ' ', '') + ' ' + REPLACE(A.ape_materno, ' ', '')) AS Nombre, 
//                                        B.descripcion AS puesto, 
//                                        A.cc_contable + ' ' + C.descripcion AS CC 
//                                    FROM DBA.sn_empleados A 
//                                        INNER JOIN si_puestos B ON A.puesto = B.puesto 
//                                        INNER JOIN cc C on A.cc_contable = C.cc 
//                                    WHERE A.clave_empleado = " + claveEmpleado;

            try
            {
                foreach (var empresaID in _context.tblP_Empresas.Select(x => x.id).ToList())
                {
                    switch ((MainContextEnum)empresaID)
                    {
                        case MainContextEnum.Construplan:
                        case MainContextEnum.Arrendadora:
                        case MainContextEnum.Colombia:
                        case MainContextEnum.PERU:
                            var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)empresaID,
                                consulta = @"SELECT
                                            a.clave_empleado,
                                            (LTRIM(RTRIM(A.nombre)) + ' ' + REPLACE(A.ape_paterno, ' ', '') + ' ' + REPLACE(A.ape_materno, ' ', '')) AS Nombre,
                                            b.descripcion AS puesto,
                                            a.cc_contable + ' ' + c.ccDescripcion AS CC
                                        FROM
                                            tblRH_EK_Empleados AS a
                                        INNER JOIN
                                            tblRH_EK_Puestos AS b ON b.puesto = a.puesto
                                        INNER JOIN
                                            tblC_Nom_CatalogoCC AS c ON c.cc = a.cc_contable
                                        WHERE
                                            a.clave_empleado = @paramCE",
                                parametros = new { paramCE = claveEmpleado }
                            });

                            lstCatEmpleado.AddRange(resultado);
                            break;
                        default:
                            break;
                    }
                }
                //var resultado = (IList<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<IList<tblRH_CatEmpleados>>();

                //foreach (var item in resultado)
                //{
                //    lstCatEmpleado.Add(item);
                //}

                //var resultado2 = (IList<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 2).ToObject<IList<tblRH_CatEmpleados>>();

                //foreach (var item in resultado2.Where(x => !lstCatEmpleado.Select(y => y.clave_empleado).ToList().Contains(x.clave_empleado)))
                //{
                //    lstCatEmpleado.Add(item);
                //}
            }
            catch
            {
                return lstCatEmpleado;
            }

            return lstCatEmpleado;
        }

        public List<tblRH_CatEmpleados> getPersonalTodosEK(string lstClavesEmpleados)
        {
            List<tblRH_CatEmpleados> lstCatEmpleado = new List<tblRH_CatEmpleados>();

            var getCatEmpleado = @"
                                    SELECT 
                                        A.clave_empleado, 
                                        (LTRIM(RTRIM(A.nombre)) + ' ' + REPLACE(A.ape_paterno, ' ', '') + ' ' + REPLACE(A.ape_materno, ' ', '')) AS Nombre, 
                                        B.descripcion AS puesto, 
                                        A.cc_contable + ' ' + C.descripcion AS CC 
                                    FROM DBA.sn_empleados A 
                                        INNER JOIN si_puestos B ON A.puesto = B.puesto 
                                        INNER JOIN cc C on A.cc_contable = C.cc
                                    WHERE A.clave_empleado IN (" + lstClavesEmpleados + @")";

            try
            {
                var resultado = (List<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<List<tblRH_CatEmpleados>>();

                lstCatEmpleado.AddRange(resultado);

                var resultado2 = (IList<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 2).ToObject<IList<tblRH_CatEmpleados>>();

                var empleadosAgregados = lstCatEmpleado.Select(y => y.clave_empleado).ToList();

                lstCatEmpleado.AddRange(resultado2.Where(x => !empleadosAgregados.Contains(x.clave_empleado)));
            }
            catch
            {
                return lstCatEmpleado;
            }

            return lstCatEmpleado;
        }

        public List<string> correoPerfil(int perfilID, string areaCuenta)
        {
            return (from a in _context.tblP_Autoriza.ToList()
                    join u in _context.tblP_Usuario.ToList()
                    on a.usuarioID equals u.id
                    join ccu in _context.tblP_CC_Usuario.ToList()
                    on a.cc_usuario_ID equals ccu.id
                    where a.perfilAutorizaID == perfilID && ccu.cc.Equals(areaCuenta)
                    select u.correo).ToList();

        }

        public tblP_EmpresasDTO getURLEmpresa(int empresaID)
        {
            var urlEmpresa = vSesiones.sesionUsuarioDTO.empresas.FirstOrDefault(x => x.id == empresaID);
            return urlEmpresa;
        }

        private bool AlertaActivaMantenimiento()
        {
            if (vSesiones.sesionBestRouting == 1)
            {
                return false;
            }
            var contextConstruplan = new MainContext((int)EmpresaEnum.Construplan);
            var alerta = contextConstruplan.tblP_AlertaMantenimiento.First();
            if (alerta.activo)
            {
                TimeSpan diferencia = alerta.fechaProgramada - DateTime.Now;
                return diferencia.TotalMinutes < 1;
            }
            return alerta.activo;
        }

        public UsuarioDTO IniciarSesion(string user, string password)
        {
            var empresaID = vSesiones.sesionEmpresaActual;
            tblP_Usuario usuario = ObtenerPorNombreUsuario(user);
            var result = new UsuarioDTO();
            if (usuario == null)
            {
                throw new ApplicationException("Datos de acceso inválidos.");
            }
            else
            {
                if (validatePassword(password, usuario.contrasena) && validateUsuario(user, usuario.nombreUsuario))
                {

                    // Verifica que no haya alerta activa de mantenimiento
                    if (AlertaActivaMantenimiento())
                    {
                        throw new ApplicationException("No puede ingresar a SIGOPLAN debido a que se esta relizando una publicación de mejoras y/o correcciones, favor de esperar un momento.");
                    }

                    var menusGenerales = _context.tblP_Menu.Where(x => x.generalSIGOPLAN).Select(x => x.id).ToList();
                    var sistemasGenerales = _context.tblP_Sistema.Where(x => x.general).Select(x => x.id).ToList();
                    var accionesVista = _context.tblP_AccionesVista.ToList();
                    var accionesVistaUsuario = _context.tblP_AccionesVistatblP_Usuario.ToList();

                    var usuarioDTO = new UsuarioDTO
                    {
                        id = usuario.id,
                        idPerfil = usuario.perfil.id,
                        perfil = usuario.perfil.nombre,
                        nombre = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno,
                        cc = usuario.cc,
                        esCliente = usuario.cliente,
                        puestoID = usuario.puestoID,
                        puesto = usuario.puesto,
                        departamento = usuario.puesto.departamento,
                        contrasena = usuario.contrasena,
                        nombreUsuario = usuario.nombreUsuario,
                        sistemas = new List<tblP_Sistema>(),
                        empresas = new List<tblP_EmpresasDTO>(),
                        tipoSGC = usuario.tipoSGC,
                        usuarioSGC = usuario.usuarioSGC,
                        tipoSeguridad = usuario.tipoSeguridad,
                        usuarioSeguridad = usuario.usuarioSeguridad,
                        usuarioMAZDA = usuario.usuarioMAZDA,
                        dashboardMaquinariaAdmin = usuario.dashboardMaquinariaAdmin,
                        dashboardMaquinariaPermiso = usuario.dashboardMaquinariaPermiso,
                        esAuditor = usuario.esAuditor,
                        externoSeguridad = usuario.externoSeguridad,
                        VistaCalendario = getViewActionTemp(0, "VistaCalendario", usuario.id),
                        cveEmpleado = usuario.cveEmpleado,
                        gestionRH = usuario.gestionRH,
                        permisos2 = new List<PermisosDTO>(),
                        usuarioGeneral = usuario.usuarioGeneral,
                        estatus = usuario.estatus,
                        externoGestor = usuario.externoGestor,
                        esColombia = usuario.esColombia,
                        isBajio = usuario.isBajio,
                        externoPatoos = usuario.externoPatoos,
                        externoPatoosNombre = usuario.externoPatoosNombre,
                        tipoPatoos = usuario.tipoPatoos
                    };

                    if (usuarioDTO.perfil.Equals("Administrador"))
                    {
                        try
                        {
                            usuarioDTO.sistemas = _context.tblP_Sistema.Where(x => x.estatus == true).ToList();
                            foreach (var item in usuarioDTO.sistemas)
                            {
                                if (item.esVirtual)
                                {
                                    if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                    item.url += "/GetReedireccion/?blob=" + Encriptacion.encriptar(usuarioDTO.nombreUsuario) + "@" + Encriptacion.encriptar(usuarioDTO.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                                }
                            }
                            usuarioDTO.permisos = _context.tblP_Menu.Where(x => x.visible).ToList();
                            usuarioDTO.permisos2 = usuarioDTO.permisos.Select(p => new PermisosDTO()
                            {
                                menu = p,
                                accion = accionesVista.Where(a => a.vistaID.Equals(p.id)).ToList()
                            }).ToList();
                            var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                            var empresasTemp = _context.tblP_Empresas.Where(x => x.estatus && (usuarioDTO.departamento.id == 14 ? true : !x.desarrollo)).ToList();
                            usuarioDTO.empresas.AddRange(
                                empresasTemp.Select(x => new tblP_EmpresasDTO
                                {
                                    id = x.id,
                                    nombre = x.nombre,
                                    activo = x.activo,
                                    estatus = x.estatus,
                                    desarrollo = x.desarrollo,
                                    icono = x.icono,
                                    iconoRedireccion = x.iconoRedireccion,
                                    url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                                }).ToList()
                            );

                        }
                        catch (Exception)
                        {
                            usuarioDTO.sistemas = null;
                            usuarioDTO.permisos = null;
                            usuarioDTO.permisos2 = null;
                        }
                    }
                    else if (usuarioDTO.esAuditor)
                    {
                        try
                        {
                            usuarioDTO.sistemas = _context.tblP_Sistema.Where(x => x.estatus == true).ToList();
                            foreach (var item in usuarioDTO.sistemas)
                            {
                                if (item.esVirtual)
                                {
                                    if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                    item.url += "/GetReedireccion/?blob=" + Encriptacion.encriptar(usuarioDTO.nombreUsuario) + "@" + Encriptacion.encriptar(usuarioDTO.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                                }
                            }
                            usuarioDTO.permisos = _context.tblP_Menu.Where(x => (x.tipo == 1 ? (x.activo && x.visible) : (x.visible && x.activo && x.liberado)) && !x.desarrollo).ToList();
                            ////--> Agregar permiso especial para omar.ramirez a modulo de desempeño
                            //var permisoDesempenio = _context.tblP_Menu.FirstOrDefault(x => x.id == 7598);
                            //if (usuarioDTO.id == 1079 || usuarioDTO.id == 1080) usuarioDTO.permisos.Add(permisoDesempenio);
                            ////<--
                            usuarioDTO.permisos2 = usuarioDTO.permisos.Select(p => new PermisosDTO()
                            {
                                menu = p,
                                accion = accionesVista.Where(a => a.vistaID.Equals(p.id)).ToList()
                            }).ToList();
                            var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                            var empresasTemp = _context.tblP_Empresas.Where(x => listaEmpresas.Contains(x.id) && x.estatus && !x.desarrollo).ToList();
                            usuarioDTO.empresas.AddRange(
                                empresasTemp.Select(x => new tblP_EmpresasDTO
                                {
                                    id = x.id,
                                    nombre = x.nombre,
                                    activo = x.activo,
                                    estatus = x.estatus,
                                    desarrollo = x.desarrollo,
                                    icono = x.icono,
                                    iconoRedireccion = x.iconoRedireccion,
                                    url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                                }).ToList()
                            );

                        }
                        catch (Exception)
                        {
                            usuarioDTO.sistemas = null;
                            usuarioDTO.permisos = null;
                            usuarioDTO.permisos2 = null;
                        }
                    }
                    else if (usuarioDTO.externoGestor)
                    {
                        try
                        {
                            var sistema = _context.tblP_Sistema.FirstOrDefault(x => x.id == 17 && x.activo);
                            usuarioDTO.sistemas.Add(sistema);
                            foreach (var item in usuarioDTO.sistemas)
                            {
                                if (item.esVirtual)
                                {
                                    if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                    item.url += "/GetReedireccion/?blob=" + Encriptacion.encriptar(usuarioDTO.nombreUsuario) + "@" + Encriptacion.encriptar(usuarioDTO.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                                }
                            }
                            usuarioDTO.permisos = _context.tblP_Menu.Where(x => x.sistemaID == sistema.id).ToList();

                            var empresasTemp = _context.tblP_Empresas
                                .Where(x => x.id == 1)
                                .Select(x => new tblP_EmpresasDTO
                                {
                                    id = x.id,
                                    nombre = x.nombre,
                                    activo = x.activo,
                                    estatus = x.estatus,
                                    desarrollo = x.desarrollo,
                                    icono = x.icono,
                                    iconoRedireccion = x.iconoRedireccion,
                                    url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                                }).FirstOrDefault();

                            usuarioDTO.empresas.Add(empresasTemp);

                        }
                        catch (Exception)
                        {
                            usuarioDTO.sistemas = null;
                            usuarioDTO.permisos = null;
                            usuarioDTO.permisos2 = null;
                        }
                    }
                    else if (usuarioDTO.perfil.Equals("AccesoUnico"))
                    {
                        List<int> sistemaInt = new List<int>();
                        List<tblP_Menu> PermisosFull = new List<tblP_Menu>();


                        var menusGeneralSIGOPLAN = _context.tblP_Menu.Where(x => x.generalSIGOPLAN == true && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && (vSesiones.sesionEmpresaActual != 3 && usuarioDTO.esColombia ? x.esColombia : true)).ToList();

                        var lstPermisos = _context.tblP_MenutblP_Usuario.Where(x => x.tblP_Usuario_id == usuarioDTO.id).ToList();
                        var lstPermisosRel = lstPermisos.Select(x => x.tblP_Menu_id).Distinct().ToList();

                        var lstPermisosMenu = _context.tblP_Menu.Where(x => (lstPermisosRel.Contains(x.id)) && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && !x.desarrollo && (vSesiones.sesionEmpresaActual != 3 && usuarioDTO.esColombia ? x.esColombia : true)).ToList();
                        sistemaInt.AddRange(lstPermisosMenu.Select(x => x.sistemaID).Distinct().ToList());
                        sistemaInt = sistemaInt.Distinct().ToList();

                        PermisosFull.AddRange(lstPermisosMenu);


                        var sistemas = _context.tblP_Sistema.Where(x => sistemaInt.Distinct().Contains(x.id)).ToList();
                        sistemas.ForEach(x => x.url = (vSesiones.sesionEmpresaActual == 1 ? "" : ":8084") + x.url);

                        usuarioDTO.sistemas = sistemas;
                        foreach (var item in usuarioDTO.sistemas)
                        {
                            if (item.esVirtual)
                            {
                                if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                item.url += "/GetReedireccion/?blob=" + Encriptacion.encriptar(usuarioDTO.nombreUsuario) + "@" + Encriptacion.encriptar(usuarioDTO.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                            }
                        }
                        usuarioDTO.permisos = PermisosFull.Distinct().ToList();
                        usuarioDTO.permisos2 = usuarioDTO.permisos.Where(m => accionesVista.Any(av =>
                                av.vistaID.Equals(m.id) &&
                                    accionesVistaUsuario.Any(avu =>
                                        avu.tblP_AccionesVista_id.Equals(av.id) &&
                                        avu.tblP_Usuario_id.Equals(usuarioDTO.id))))
                            .Select(m => new PermisosDTO()
                        {
                            menu = m,
                            accion = accionesVista.Where(av => m.id.Equals(av.vistaID)).ToList()
                        }).ToList();
                        var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                        var empresasTemp = _context.tblP_Empresas.Where(x => listaEmpresas.Contains(x.id) && x.estatus && !x.desarrollo).ToList();
                        usuarioDTO.empresas.AddRange(
                            empresasTemp.Select(x => new tblP_EmpresasDTO
                            {
                                id = x.id,
                                nombre = x.nombre,
                                activo = x.activo,
                                estatus = x.estatus,
                                desarrollo = x.desarrollo,
                                icono = x.icono,
                                iconoRedireccion = x.iconoRedireccion,
                                url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                            }).ToList()
                        );
                    }
                    else if (usuarioDTO.perfil.Equals("Contratista"))
                    {
                        List<int> sistemaInt = new List<int>();
                        List<tblP_Menu> PermisosFull = new List<tblP_Menu>();


                        //var menusGeneralSIGOPLAN = _context.tblP_Menu.Where(x => x.generalSIGOPLAN == true && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && (vSesiones.sesionEmpresaActual != 3 && usuarioDTO.esColombia ? x.esColombia : true)).ToList();

                        var lstPermisos = _context.tblP_MenutblP_Usuario.Where(x => x.tblP_Usuario_id == usuarioDTO.id).ToList();
                        var lstPermisosRel = lstPermisos.Select(x => x.tblP_Menu_id).Distinct().ToList();

                        var lstPermisosMenu = _context.tblP_Menu.Where(x => (lstPermisosRel.Contains(x.id)) && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && !x.desarrollo && (vSesiones.sesionEmpresaActual != 3 && usuarioDTO.esColombia ? x.esColombia : true)).ToList();
                        sistemaInt.AddRange(lstPermisosMenu.Select(x => x.sistemaID).Distinct().ToList());
                        sistemaInt = sistemaInt.Distinct().ToList();

                        PermisosFull.AddRange(lstPermisosMenu);


                        var sistemas = _context.tblP_Sistema.Where(x => sistemaInt.Distinct().Contains(x.id)).ToList();
                        sistemas.ForEach(x => x.url = (vSesiones.sesionEmpresaActual == 1 ? "" : ":8084") + x.url);

                        usuarioDTO.sistemas = sistemas;
                        foreach (var item in usuarioDTO.sistemas)
                        {
                            if (item.esVirtual)
                            {
                                if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                item.url += "/GetReedireccion/?blob=" + Encriptacion.encriptar(usuarioDTO.nombreUsuario) + "@" + Encriptacion.encriptar(usuarioDTO.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                            }
                        }
                        usuarioDTO.permisos = PermisosFull.Distinct().ToList();
                        usuarioDTO.permisos2 = usuarioDTO.permisos.Where(m => accionesVista.Any(av =>
                                av.vistaID.Equals(m.id) &&
                                    accionesVistaUsuario.Any(avu =>
                                        avu.tblP_AccionesVista_id.Equals(av.id) &&
                                        avu.tblP_Usuario_id.Equals(usuarioDTO.id))))
                            .Select(m => new PermisosDTO()
                            {
                                menu = m,
                                accion = accionesVista.Where(av => m.id.Equals(av.vistaID)).ToList()
                            }).ToList();
                        var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                        var empresasTemp = _context.tblP_Empresas.Where(x => listaEmpresas.Contains(x.id) && x.estatus && !x.desarrollo).ToList();
                        usuarioDTO.empresas.AddRange(
                            empresasTemp.Select(x => new tblP_EmpresasDTO
                            {
                                id = x.id,
                                nombre = x.nombre,
                                activo = x.activo,
                                estatus = x.estatus,
                                desarrollo = x.desarrollo,
                                icono = x.icono,
                                iconoRedireccion = x.iconoRedireccion,
                                url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                            }).ToList()
                        );
                    }
                    else
                    {
                        if (usuarioDTO.usuarioMAZDA != true)
                        {

                            if (usuarioDTO.externoSeguridad == true)
                            {
                                try
                                {
                                    vSesiones.sesionUsuarioExternoSeguridad = true;

                                    usuarioDTO.sistemas = _context.tblP_Sistema.Where(x => x.id == 10).ToList();
                                    foreach (var item in usuarioDTO.sistemas)
                                    {
                                        if (item.esVirtual)
                                        {
                                            if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                            item.url += "/GetReedireccion/?blob=" + Encriptacion.encriptar(usuarioDTO.nombreUsuario) + "@" + Encriptacion.encriptar(usuarioDTO.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                                        }
                                    }

                                    var empresasTemp = _context.tblP_Empresas.Where(x => x.id == 1).ToList();
                                    usuarioDTO.empresas.AddRange(
                                        empresasTemp.Select(x => new tblP_EmpresasDTO
                                        {
                                            id = x.id,
                                            nombre = x.nombre,
                                            activo = x.activo,
                                            estatus = x.estatus,
                                            desarrollo = x.desarrollo,
                                            icono = x.icono,
                                            iconoRedireccion = x.iconoRedireccion,
                                            url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                                        }).ToList()
                                    );
                                }
                                catch (Exception)
                                {
                                    usuarioDTO.sistemas = null;
                                    usuarioDTO.permisos = null;
                                    usuarioDTO.permisos2 = null;
                                }
                            }
                            else
                            {
                                List<int> sistemaInt = new List<int>();
                                List<tblP_Menu> PermisosFull = new List<tblP_Menu>();
                                var sistemasGeneral = _context.tblP_Sistema.Where(x => x.general == true && x.estatus == true && (vSesiones.sesionEmpresaActual != 3 && usuarioDTO.esColombia ? x.esColombia : true)).ToList();
                                sistemaInt.AddRange(sistemasGeneral.Where(x => x.general == true).Select(x => x.id));
                                var seg = _context.tblP_Sistema.FirstOrDefault(x => x.nombre.Equals("Seguridad"));
                                if (usuario.tipoSeguridad && seg.activo)
                                {
                                    sistemaInt.Add(_context.tblP_Sistema.FirstOrDefault(x => x.nombre.Equals("Seguridad")).id);
                                }
                                var menusGeneralSIGOPLAN = _context.tblP_Menu.Where(x => x.generalSIGOPLAN == true && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && (vSesiones.sesionEmpresaActual != 3 && usuarioDTO.esColombia ? x.esColombia : true)).ToList();

                                var lstPermisos = _context.tblP_MenutblP_Usuario.Where(x => x.tblP_Usuario_id == usuarioDTO.id).ToList();
                                var lstPermisosRel = lstPermisos.Select(x => x.tblP_Menu_id).Distinct().ToList();

                                var lstPermisosMenu = _context.tblP_Menu.Where(x => (lstPermisosRel.Contains(x.id) || x.generalSIGOPLAN == true) && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && !x.desarrollo && (vSesiones.sesionEmpresaActual != 3 && usuarioDTO.esColombia ? x.esColombia : true)).ToList();
                                sistemaInt.AddRange(lstPermisosMenu.Select(x => x.sistemaID).Distinct().ToList());
                                sistemaInt = sistemaInt.Distinct().ToList();
                                var generalPorSistema = _context.tblP_Menu.Where(x => (x.generalPorSistema == true && sistemaInt.Contains(x.sistemaID)) && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && (vSesiones.sesionEmpresaActual != 3 && usuarioDTO.esColombia ? x.esColombia : true)).ToList();

                                PermisosFull.AddRange(lstPermisosMenu);
                                PermisosFull.AddRange(generalPorSistema);

                                var sistemas = _context.tblP_Sistema.Where(x => sistemaInt.Distinct().Contains(x.id) && (usuario.sistemasGenerales ? !sistemasGenerales.Contains(x.id) : true) && (vSesiones.sesionEmpresaActual != 3 && usuarioDTO.esColombia ? x.esColombia : true)).ToList();
                                sistemas.ForEach(x => x.url = (vSesiones.sesionEmpresaActual == 1 ? "" : ":8084") + x.url);

                                usuarioDTO.sistemas = sistemas;
                                foreach (var item in usuarioDTO.sistemas)
                                {
                                    if (item.esVirtual)
                                    {
                                        if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                        item.url += "/GetReedireccion/?blob=" + Encriptacion.encriptar(usuario.nombreUsuario) + "@" + Encriptacion.encriptar(usuario.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                                    }
                                }
                                usuarioDTO.permisos = PermisosFull.Where(x => (usuario.sistemasGenerales ? !menusGenerales.Contains(x.id) : true)).Distinct().ToList();
                                usuarioDTO.permisos2 = usuarioDTO.permisos.Where(m => accionesVista.Any(av =>
                                        av.vistaID.Equals(m.id) &&
                                            accionesVistaUsuario.Any(avu =>
                                                avu.tblP_AccionesVista_id.Equals(av.id) &&
                                                avu.tblP_Usuario_id.Equals(usuarioDTO.id))))
                                    .Select(m => new PermisosDTO()
                                {
                                    menu = m,
                                    accion = accionesVista.Where(av => m.id.Equals(av.vistaID)).ToList()
                                }).ToList();
                                var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                                var empresasTemp = _context.tblP_Empresas.Where(x => listaEmpresas.Contains(x.id) && x.estatus && !x.desarrollo).ToList();
                                usuarioDTO.empresas.AddRange(
                                    empresasTemp.Select(x => new tblP_EmpresasDTO
                                    {
                                        id = x.id,
                                        nombre = x.nombre,
                                        activo = x.activo,
                                        estatus = x.estatus,
                                        desarrollo = x.desarrollo,
                                        icono = x.icono,
                                        iconoRedireccion = x.iconoRedireccion,
                                        url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                                    }).ToList()
                                );
                            }
                        }
                        else
                        {
                            try
                            {
                                vSesiones.sesionUsuarioMAZDA = true;

                                usuarioDTO.sistemas = _context.tblP_Sistema.Where(x => x.id == 7).ToList();
                                foreach (var item in usuarioDTO.sistemas)
                                {
                                    if (item.esVirtual)
                                    {
                                        if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                        item.url += "/GetReedireccion/?blob=" + Encriptacion.encriptar(usuarioDTO.nombreUsuario) + "@" + Encriptacion.encriptar(usuarioDTO.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                                    }
                                }

                                //Ajustar las condiciones
                                usuarioDTO.permisos = _context.tblP_Menu.Where(x => (x.id == 4201 || x.padre == 4201 || x.url.Contains("MAZDA")) && !x.desarrollo).ToList();
                                usuarioDTO.permisos2 = usuarioDTO.permisos.Where(m => accionesVista.Any(av =>
                                        av.vistaID.Equals(m.id) &&
                                            accionesVistaUsuario.Any(avu =>
                                                avu.tblP_AccionesVista_id.Equals(av.id) &&
                                                avu.tblP_Usuario_id.Equals(usuarioDTO.id))))
                                    .Select(m => new PermisosDTO()
                                    {
                                        menu = m,
                                        accion = accionesVista.Where(av => m.id.Equals(av.vistaID)).ToList()
                                    }).ToList();
                                var empresasTemp = _context.tblP_Empresas.Where(x => x.id == 1).ToList();
                                usuarioDTO.empresas.AddRange(
                                    empresasTemp.Select(x => new tblP_EmpresasDTO
                                    {
                                        id = x.id,
                                        nombre = x.nombre,
                                        activo = x.activo,
                                        estatus = x.estatus,
                                        desarrollo = x.desarrollo,
                                        icono = x.icono,
                                        iconoRedireccion = x.iconoRedireccion,
                                        url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                                    }).ToList()
                                );

                            }
                            catch (Exception)
                            {
                                usuarioDTO.sistemas = null;
                                usuarioDTO.permisos = null;
                                usuarioDTO.permisos2 = null;
                            }
                        }
                    }

                    usuarioDTO.tipoSGC = usuario.tipoSGC;
                    usuarioDTO.usuarioSGC = usuario.usuarioSGC;
                    usuarioDTO.tipoSeguridad = usuario.tipoSeguridad;
                    usuarioDTO.usuarioSeguridad = usuario.usuarioSeguridad;
                    usuarioDTO.externoSeguridad = usuario.externoSeguridad;
                    usuarioDTO.correosVinculados = getCorreosVinculados();
                    usuarioDTO.correo = usuario.correo;
                    usuarioDTO.tipoPatoos = usuario.tipoPatoos;
                    usuarioDTO.externoPatoos = usuario.externoPatoos;
                    usuarioDTO.externoPatoosNombre = usuario.externoPatoosNombre;
                    result = usuarioDTO;
                    vSesiones.sesionUsuarioDTO = result;

                    return result;
                }
                else
                {
                    throw new ApplicationException("Datos de acceso inválidos.");
                }
            }
        }
        public UsuarioDTO setCambioEmpresa(string user, string password)
        {
            var empresaID = vSesiones.sesionEmpresaActual;
            var usuario = ObtenerPorNombreUsuario(user);

            if (usuario == null)
            {
                throw new Exception("El usuario ingresado no existe.");
            }

            if (validatePasswordEncryptado(password, usuario.contrasena) && validateUsuario(user, usuario.nombreUsuario))
            {
                if (AlertaActivaMantenimiento()) //Verifica que no haya alerta activa de mantenimiento
                {
                    throw new ApplicationException("No puede ingresar a SIGOPLAN debido a que se está realizando una publicación de mejoras y/o correcciones, favor de esperar un momento.");
                }

                #region Información Inicial
                var menusGenerales = _context.tblP_Menu.Where(x => x.generalSIGOPLAN).Select(x => x.id).ToList();
                var sistemasGenerales = _context.tblP_Sistema.Where(x => x.general).Select(x => x.id).ToList();
                var accionesVista = _context.tblP_AccionesVista.ToList();
                var accionesVistaUsuario = _context.tblP_AccionesVistatblP_Usuario.ToList();
                #endregion

                #region Objeto Usuario
                var obj = new UsuarioDTO
                {
                    id = usuario.id,
                    idPerfil = usuario.perfil.id,
                    perfil = usuario.perfil.nombre,
                    nombre = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno,
                    cc = usuario.cc,
                    esCliente = usuario.cliente,
                    puestoID = usuario.puestoID,
                    puesto = usuario.puesto,
                    departamento = usuario.puesto.departamento,
                    contrasena = usuario.contrasena,
                    nombreUsuario = usuario.nombreUsuario,
                    sistemas = new List<tblP_Sistema>(),
                    empresas = new List<tblP_EmpresasDTO>(),
                    permisos2 = new List<PermisosDTO>(),
                    tipoSGC = usuario.tipoSGC,
                    usuarioSGC = usuario.usuarioSGC,
                    tipoSeguridad = usuario.tipoSeguridad,
                    usuarioSeguridad = usuario.usuarioSeguridad,
                    esAuditor = usuario.esAuditor,
                    gestionRH = usuario.gestionRH,
                    usuarioGeneral = usuario.usuarioGeneral,
                    estatus = usuario.estatus,
                    cveEmpleado = usuario.cveEmpleado,
                    esColombia = usuario.esColombia,
                    isBajio = usuario.isBajio,
                    externoPatoos = usuario.externoPatoos,
                    externoPatoosNombre = usuario.externoPatoosNombre,
                    tipoPatoos = usuario.tipoPatoos,
                    dashboardMaquinariaAdmin = usuario.dashboardMaquinariaAdmin,
                    dashboardMaquinariaPermiso = usuario.dashboardMaquinariaPermiso,
                    correosVinculados = getCorreosVinculados(),
                    correo = usuario.correo,
                };
                #endregion

                if (obj.perfil.Equals("Administrador"))
                {
                    #region Perfil Administrador
                    try
                    {
                        obj.sistemas = _context.tblP_Sistema.Where(x => x.estatus == true).ToList();
                        foreach (var item in obj.sistemas)
                        {
                            if (item.esVirtual)
                            {
                                if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                item.url += "/SISTEMA/Sistema/GetReedireccion/?blob=" + Encriptacion.encriptar(obj.nombreUsuario) + "@" + Encriptacion.encriptar(obj.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                            }
                        }
                        obj.permisos = _context.tblP_Menu.Where(x => x.visible).ToList();
                        obj.permisos2 = obj.permisos.Select(p => new PermisosDTO()
                        {
                            menu = p,
                            accion = accionesVista.Where(a => a.vistaID.Equals(p.id)).ToList()
                        }).ToList();
                        var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                        var empresasTemp = _context.tblP_Empresas.Where(x => x.estatus && (obj.departamento.id == 14 ? true : !x.desarrollo)).ToList();
                        obj.empresas.AddRange(
                            empresasTemp.Select(x => new tblP_EmpresasDTO
                            {
                                id = x.id,
                                nombre = x.nombre,
                                activo = x.activo,
                                estatus = x.estatus,
                                desarrollo = x.desarrollo,
                                icono = x.icono,
                                iconoRedireccion = x.iconoRedireccion,
                                url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                            }).ToList()
                        );

                    }
                    catch (Exception)
                    {
                        obj.sistemas = null;
                        obj.permisos = null;
                    }
                    #endregion
                }
                else if (obj.esAuditor)
                {
                    #region Perfil Auditor
                    try
                    {
                        obj.sistemas = _context.tblP_Sistema.Where(x => x.estatus == true).ToList();
                        foreach (var item in obj.sistemas)
                        {
                            if (item.esVirtual)
                            {
                                if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                item.url += "/SISTEMA/Sistema/GetReedireccion/?blob=" + Encriptacion.encriptar(obj.nombreUsuario) + "@" + Encriptacion.encriptar(obj.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                            }
                        }
                        obj.permisos = _context.tblP_Menu.Where(x => (x.tipo == 1 ? (x.activo && x.visible) : (x.visible && x.activo && x.liberado)) && !x.desarrollo).ToList();
                        ////--> Agregar permiso especial para omar.ramirez a modulo de desempeño
                        //var permisoDesempenio = _context.tblP_Menu.FirstOrDefault(x => x.id == 7598);
                        //if (obj.id == 1079 || obj.id == 1080) obj.permisos.Add(permisoDesempenio);
                        ////<--
                        obj.permisos2 = obj.permisos.Where(m => accionesVista.Any(av =>
                                    av.vistaID.Equals(m.id) &&
                                        accionesVistaUsuario.Any(avu =>
                                            avu.tblP_AccionesVista_id.Equals(av.id) &&
                                            avu.tblP_Usuario_id.Equals(obj.id))))
                                .Select(m => new PermisosDTO()
                                {
                                    menu = m,
                                    accion = accionesVista.Where(av => m.id.Equals(av.vistaID)).ToList()
                                }).ToList();
                        var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                        var empresasTemp = _context.tblP_Empresas.Where(x => listaEmpresas.Contains(x.id) && x.estatus && !x.desarrollo).ToList();
                        obj.empresas.AddRange(
                            empresasTemp.Select(x => new tblP_EmpresasDTO
                            {
                                id = x.id,
                                nombre = x.nombre,
                                activo = x.activo,
                                estatus = x.estatus,
                                desarrollo = x.desarrollo,
                                icono = x.icono,
                                iconoRedireccion = x.iconoRedireccion,
                                url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                            }).ToList()
                        );

                    }
                    catch (Exception)
                    {
                        obj.sistemas = null;
                        obj.permisos = null;
                        obj.permisos2 = null;
                    }
                    #endregion
                }
                else if (obj.perfil.Equals("AccesoUnico"))
                {
                    #region Perfil de Acceso Único
                    List<int> sistemaInt = new List<int>();
                    List<tblP_Menu> PermisosFull = new List<tblP_Menu>();


                    var lstPermisos = _context.tblP_MenutblP_Usuario.Where(x => x.tblP_Usuario_id == obj.id).ToList();
                    var lstPermisosRel = lstPermisos.Select(x => x.tblP_Menu_id).Distinct().ToList();

                    var lstPermisosMenu = _context.tblP_Menu.Where(x => (lstPermisosRel.Contains(x.id) || x.generalSIGOPLAN == true) && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && !x.desarrollo && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList();
                    sistemaInt.AddRange(lstPermisosMenu.Select(x => x.sistemaID).Distinct().ToList());
                    sistemaInt = sistemaInt.Distinct().ToList();

                    PermisosFull.AddRange(lstPermisosMenu);

                    var sistemas = _context.tblP_Sistema.Where(x => sistemaInt.Distinct().Contains(x.id)).ToList();
                    foreach (var item in sistemas)
                    {
                        if (item.esVirtual)
                        {
                            if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                            item.url += "/SISTEMA/Sistema/GetReedireccion/?blob=" + Encriptacion.encriptar(obj.nombreUsuario) + "@" + Encriptacion.encriptar(obj.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                        }
                    }

                    obj.sistemas = sistemas;
                    obj.permisos = PermisosFull.Distinct().ToList();
                    obj.permisos2 = obj.permisos.Where(m => accionesVista.Any(av =>
                                    av.vistaID.Equals(m.id) &&
                                        accionesVistaUsuario.Any(avu =>
                                            avu.tblP_AccionesVista_id.Equals(av.id) &&
                                            avu.tblP_Usuario_id.Equals(obj.id))))
                                .Select(m => new PermisosDTO()
                                {
                                    menu = m,
                                    accion = accionesVista.Where(av => m.id.Equals(av.vistaID)).ToList()
                                }).ToList();
                    var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                    var empresasTemp = _context.tblP_Empresas.Where(x => listaEmpresas.Contains(x.id) && x.estatus && !x.desarrollo).ToList();
                    obj.empresas.AddRange(
                        empresasTemp.Select(x => new tblP_EmpresasDTO
                        {
                            id = x.id,
                            nombre = x.nombre,
                            activo = x.activo,
                            estatus = x.estatus,
                            desarrollo = x.desarrollo,
                            icono = x.icono,
                            iconoRedireccion = x.iconoRedireccion,
                            url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                        }).ToList()
                    );
                    #endregion
                }
                else if (obj.perfil.Equals("Contratista"))
                {
                    #region Perfil Contratista
                    List<int> sistemaInt = new List<int>();
                    List<tblP_Menu> PermisosFull = new List<tblP_Menu>();


                    var lstPermisos = _context.tblP_MenutblP_Usuario.Where(x => x.tblP_Usuario_id == obj.id).ToList();
                    var lstPermisosRel = lstPermisos.Select(x => x.tblP_Menu_id).Distinct().ToList();

                    var lstPermisosMenu = _context.tblP_Menu.Where(x => (lstPermisosRel.Contains(x.id) || x.generalSIGOPLAN == true) && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && !x.desarrollo && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList();
                    sistemaInt.AddRange(lstPermisosMenu.Select(x => x.sistemaID).Distinct().ToList());
                    sistemaInt = sistemaInt.Distinct().ToList();

                    PermisosFull.AddRange(lstPermisosMenu);

                    var sistemas = _context.tblP_Sistema.Where(x => sistemaInt.Distinct().Contains(x.id)).ToList();
                    foreach (var item in sistemas)
                    {
                        if (item.esVirtual)
                        {
                            if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                            item.url += "/SISTEMA/Sistema/GetReedireccion/?blob=" + Encriptacion.encriptar(obj.nombreUsuario) + "@" + Encriptacion.encriptar(obj.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                        }
                    }

                    obj.sistemas = sistemas;
                    obj.permisos = PermisosFull.Distinct().ToList();
                    obj.permisos2 = obj.permisos.Where(m => accionesVista.Any(av =>
                                    av.vistaID.Equals(m.id) &&
                                        accionesVistaUsuario.Any(avu =>
                                            avu.tblP_AccionesVista_id.Equals(av.id) &&
                                            avu.tblP_Usuario_id.Equals(obj.id))))
                                .Select(m => new PermisosDTO()
                                {
                                    menu = m,
                                    accion = accionesVista.Where(av => m.id.Equals(av.vistaID)).ToList()
                                }).ToList();
                    var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                    var empresasTemp = _context.tblP_Empresas.Where(x => listaEmpresas.Contains(x.id) && x.estatus && !x.desarrollo).ToList();
                    obj.empresas.AddRange(
                        empresasTemp.Select(x => new tblP_EmpresasDTO
                        {
                            id = x.id,
                            nombre = x.nombre,
                            activo = x.activo,
                            estatus = x.estatus,
                            desarrollo = x.desarrollo,
                            icono = x.icono,
                            iconoRedireccion = x.iconoRedireccion,
                            url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                        }).ToList()
                    );
                    #endregion
                }
                else
                {
                    #region Perfil Usuario Normal
                    List<int> sistemaInt = new List<int>();
                    List<tblP_Menu> PermisosFull = new List<tblP_Menu>();
                    var sistemasGeneral = _context.tblP_Sistema.Where(x => x.general == true && x.estatus == true && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList();
                    sistemaInt.AddRange(sistemasGeneral.Where(x => x.general == true).Select(x => x.id));
                    var seg = _context.tblP_Sistema.FirstOrDefault(x => x.nombre.Equals("Seguridad"));
                    if (usuario.tipoSeguridad && seg.activo)
                    {
                        sistemaInt.Add(_context.tblP_Sistema.FirstOrDefault(x => x.nombre.Equals("Seguridad")).id);
                    }
                    var menusGeneralSIGOPLAN = _context.tblP_Menu.Where(x => x.generalSIGOPLAN == true && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList();

                    var lstPermisos = _context.tblP_MenutblP_Usuario.Where(x => x.tblP_Usuario_id == obj.id).ToList();
                    var lstPermisosRel = lstPermisos.Select(x => x.tblP_Menu_id).Distinct().ToList();

                    var lstPermisosMenu = _context.tblP_Menu.Where(x => (lstPermisosRel.Contains(x.id) || x.generalSIGOPLAN == true) && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && !x.desarrollo && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList();
                    sistemaInt.AddRange(lstPermisosMenu.Select(x => x.sistemaID).Distinct().ToList());
                    sistemaInt = sistemaInt.Distinct().ToList();
                    var generalPorSistema = _context.tblP_Menu.Where(x => (x.generalPorSistema == true && sistemaInt.Contains(x.sistemaID)) && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList();

                    PermisosFull.AddRange(lstPermisosMenu);
                    PermisosFull.AddRange(generalPorSistema);

                    UsuarioDTO auxusuario = new UsuarioDTO();

                    if (vSesiones.sesionUsuarioDTO != null) auxusuario = vSesiones.sesionUsuarioDTO;

                    var sistemas = _context.tblP_Sistema.Where(x => sistemaInt.Distinct().Contains(x.id) && (usuario.sistemasGenerales ? !sistemasGenerales.Contains(x.id) : true) && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList().Select(x =>
                    {
                        var url = x.url;
                        if (vSesiones.sesionBestRouting == 1) url = ":3676";
                        return new tblP_Sistema
                        {
                            id = x.id,
                            nombre = x.nombre,
                            icono = x.icono,
                            url = x.esVirtual ? (url + (vSesiones.sesionUsuarioDTO != null ? "/SISTEMA/Sistema/GetReedireccion/?blob=" + Encriptacion.encriptar(auxusuario.nombreUsuario) + "@" + Encriptacion.encriptar(auxusuario.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting : "")) : x.url,
                            estatus = x.estatus,
                            activo = x.activo,
                            general = x.general,
                            ext = x.ext,
                            esColombia = x.esColombia,
                            esVirtual = x.esVirtual
                        };
                    }).ToList();


                    obj.sistemas = sistemas;
                    obj.permisos = PermisosFull.Where(x => (usuario.sistemasGenerales ? !menusGenerales.Contains(x.id) : true)).Distinct().ToList();
                    obj.permisos2 = obj.permisos.Where(m => accionesVista.Any(av =>
                                    av.vistaID.Equals(m.id) &&
                                        accionesVistaUsuario.Any(avu =>
                                            avu.tblP_AccionesVista_id.Equals(av.id) &&
                                            avu.tblP_Usuario_id.Equals(obj.id))))
                                .Select(m => new PermisosDTO()
                                {
                                    menu = m,
                                    accion = accionesVista.Where(av => m.id.Equals(av.vistaID)).ToList()
                                }).ToList();
                    var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                    var empresasTemp = _context.tblP_Empresas.Where(x => listaEmpresas.Contains(x.id) && x.estatus && !x.desarrollo).ToList();
                    obj.empresas.AddRange(
                        empresasTemp.Select(x => new tblP_EmpresasDTO
                        {
                            id = x.id,
                            nombre = x.nombre,
                            activo = x.activo,
                            estatus = x.estatus,
                            desarrollo = x.desarrollo,
                            icono = x.icono,
                            iconoRedireccion = x.iconoRedireccion,
                            url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                        }).ToList()
                    );
                    #endregion
                }

                #region Bloqueo/Mensaje de facturas pendientes para Administradores/Auxiliares/Gerentes de obra
                //var hoy = DateTime.Now.DayOfWeek;

                //if (hoy == DayOfWeek.Monday) //El bloqueo se realiza los miércoles.
                //{
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora) //La revisión de los facultamientos de facturas nomás aplica para Construplan y Arrendadora.
                {
                    var listaFacultamientosFacturas = new List<tblP_UsuarioFacultamientoFactura>();

                    using (var _contextConstruplan = new MainContext(EmpresaEnum.Construplan)) //Se consulta la información de la base de datos de Construplan ya que nomás ahí se guarda la información.
                    {
                        listaFacultamientosFacturas = _contextConstruplan.tblP_UsuarioFacultamientoFactura.Where(x => x.registroActivo && x.usuario_id == usuario.id).ToList();//.Where(x => x.empresa == (EmpresaEnum)vSesiones.sesionEmpresaActual).ToList();
                        if (vSesiones.sesionEmpresaActual == 1) listaFacultamientosFacturas = listaFacultamientosFacturas.Where(x => x.empresa == (EmpresaEnum)vSesiones.sesionEmpresaActual).ToList();
                    }

                    if (listaFacultamientosFacturas.Count() > 0) //Se revisa si el usuario logeado tiene facultamientos de facturas.
                    {
                        var facultamientoFacturas = new UsuarioFacultamientoFacturaDTO();

                        facultamientoFacturas.listaFacturasPendientes = new List<GastosProveedorDTO>();
                        facultamientoFacturas.stringFacturasPendientes = "";

                        var fecha18DiasAtras = DateTime.Now.AddDays(-18);

                        if (listaFacultamientosFacturas.Any(x => x.tipoUsuario == TipoUsuarioFacultamientoFacturaEnum.ADMINISTRADOR_AUXILIAR)) //Usuario con facultamientos de Administrador/Auxiliar. Aplica el bloqueo.
                        {
                            var facultamientos = listaFacultamientosFacturas.Select(x => x.cc).ToList();
                            var lstCCxAC = _context.tblP_CC.Where(x => facultamientos.Contains(x.cc)).Select(x => x.areaCuenta).ToList();

                            List<GastosProveedorDTO> listaFacturasEnkontrol = new List<GastosProveedorDTO>();

                            if (vSesiones.sesionEmpresaActual == 2 && lstCCxAC.Count() > 0)
                            {
                                var stringFacultamientosAC = string.Join(" OR ", lstCCxAC.Select(x => "('" + x + "' = ANY(SELECT CAST(area AS varchar) + '-' + CAST(cuenta AS varchar) FROM so_orden_compra_det ocd WHERE ocd.cc = oc.cc AND ocd.numero = oc.numero))").ToList());

                                listaFacturasEnkontrol = _contextEnkontrol.Select<GastosProveedorDTO>(getEnkontrolAmbienteConsulta(), string.Format(@"
                                    SELECT
                                        g.*, oc.estatus,
                                        (SELECT CAST(ocd.area as varchar) + '-' + CAST(ocd.cuenta as varchar) FROM so_orden_compra_det ocd WHERE ocd.cc = oc.cc AND ocd.numero = oc.numero AND ocd.partida = 1) AS areaCuenta
                                    FROM sp_gastos_prov g
                                        LEFT JOIN so_orden_compra oc ON g.cc = oc.cc AND g.referenciaoc = oc.numero
                                    WHERE g.estatus = 'C' AND g.fecha < '{0}' AND oc.estatus != 'C' AND ({1})
                                    ORDER BY g.fecha DESC
                                ", fecha18DiasAtras.ToString("yyyy-MM-dd"), stringFacultamientosAC));
                            }
                            else
                            {
                                var stringFacultamientosCC = string.Join(", ", facultamientos.Select(x => "'" + x + "'").ToList());

                                listaFacturasEnkontrol = _contextEnkontrol.Select<GastosProveedorDTO>(getEnkontrolAmbienteConsulta(), string.Format(@"
                                    SELECT
                                        g.*, oc.estatus
                                    FROM sp_gastos_prov g
                                        LEFT JOIN so_orden_compra oc ON g.cc = oc.cc AND g.referenciaoc = oc.numero
                                    WHERE g.estatus = 'C' AND g.fecha < '{0}' AND g.cc IN ({1}) AND oc.estatus != 'C'
                                    ORDER BY g.fecha DESC
                                ", fecha18DiasAtras.ToString("yyyy-MM-dd"), stringFacultamientosCC));
                            }

                            if (listaFacturasEnkontrol.Count() > 0)
                            {
                                facultamientoFacturas.bloqueo = true;
                                facultamientoFacturas.listaFacturasPendientes = listaFacturasEnkontrol;
                                facultamientoFacturas.stringFacturasPendientes = "Facturas pendientes de validar: " +
                                    string.Join(", ", listaFacturasEnkontrol.Select(x => "[Proveedor = " + x.numpro + " - CFDI Folio = " + x.cfd_folio + " - Factura = " + (x.factura != "" ? x.factura : "N/A") + "]"));

                                //Se quitan los permisos de sistemas y vistas al usuario.
                                obj.sistemas = new List<tblP_Sistema>();
                                obj.sistemasGenerales = false;
                                obj.permisos = new List<tblP_Menu>();
                                obj.permisos2 = new List<PermisosDTO>();
                            }

                            obj.facultamientoFacturas = facultamientoFacturas;
                        }
                        else //Usuario con facultamientos de Gerente. No aplica el bloqueo.
                        {
                            var stringFacultamientosCC = string.Join(", ", listaFacultamientosFacturas.Select(x => "'" + x.cc + "'").ToList());
                            var listaFacturasEnkontrol = _contextEnkontrol.Select<GastosProveedorDTO>(getEnkontrolAmbienteConsulta(), string.Format(@"
                                    SELECT
                                        g.*, oc.estatus
                                    FROM sp_gastos_prov g
                                        LEFT JOIN so_orden_compra oc ON g.cc = oc.cc AND g.referenciaoc = oc.numero
                                    WHERE g.estatus = 'C' AND g.fecha < '{0}' AND g.cc IN ({1}) AND oc.estatus != 'C'
                                    ORDER BY g.fecha DESC
                                ", fecha18DiasAtras.ToString("yyyy-MM-dd"), stringFacultamientosCC));

                            if (listaFacturasEnkontrol.Count() > 0)
                            {
                                facultamientoFacturas.bloqueo = false;
                                facultamientoFacturas.listaFacturasPendientes = listaFacturasEnkontrol;
                                facultamientoFacturas.stringFacturasPendientes = "Facturas pendientes de autorización: " +
                                    string.Join(", ", listaFacturasEnkontrol.Select(x => "[Proveedor = " + x.numpro + " - CFDI Folio = " + x.cfd_folio + " - Factura = " + x.factura + "]"));
                            }

                            obj.facultamientoFacturas = facultamientoFacturas;
                        }

                        if (obj.facultamientoFacturas.listaFacturasPendientes.Count() > 0)
                        {
                            var listaCC = _context.tblP_CC.ToList();
                            var listaProveedoresEK = _contextEnkontrol.Select<GastosProveedorDTO>(getEnkontrolAmbienteConsulta(), string.Format(@"SELECT numpro, (CAST(numpro AS varchar(50)) + ' - ' + nombre) AS proveedorDesc FROM sp_proveedores"));

                            foreach (var f in obj.facultamientoFacturas.listaFacturasPendientes)
                            {
                                f.proveedorDesc = listaProveedoresEK.Where(x => x.numpro == f.numpro).Select(y => y.proveedorDesc).FirstOrDefault();

                                switch (vSesiones.sesionEmpresaActual)
                                {
                                    case (int)EmpresaEnum.Construplan:
                                        f.ccDesc = listaCC.Where(x => x.cc == f.cc).Select(y => y.cc + " - " + y.descripcion).FirstOrDefault();
                                        break;
                                    case (int)EmpresaEnum.Arrendadora:
                                        f.ccDesc = listaCC.Where(x => (x.area + "-" + x.cuenta) == f.areaCuenta).Select(y => "[" + y.area + "-" + y.cuenta + "] " + y.descripcion).FirstOrDefault();
                                        break;
                                }

                                f.fechaString = f.fecha.ToShortDateString();
                            }
                        }
                    }
                }
                //}
                #endregion

                vSesiones.sesionUsuarioDTO = obj;

                return obj;
            }
            else
            {
                throw new Exception("La contraseña ingresada no es correcta.");
            }
        }

        public EnkontrolAmbienteEnum getEnkontrolAmbienteConsulta()
        {
            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
            {
                return EnkontrolAmbienteEnum.ProdCPLAN;
            }
            else
            {
                return EnkontrolAmbienteEnum.ProdARREND;
            }
        }

        public UsuarioDTO setCambioEmpresaAnterior(string user, string password)
        {
            var empresaID = vSesiones.sesionEmpresaActual;
            tblP_Usuario usuario = ObtenerPorNombreUsuario(user);
            var result = new UsuarioDTO();
            if (usuario == null)
            {
                throw new Exception("El usuario ingresado no existe.");
            }
            else
            {
                if (validatePasswordEncryptado(password, usuario.contrasena) && validateUsuario(user, usuario.nombreUsuario))
                {
                    // Verifica que no haya alerta activa de mantenimiento
                    if (AlertaActivaMantenimiento())
                    {
                        throw new ApplicationException("No puede ingresar a SIGOPLAN debido a que se está realizando una publicación de mejoras y/o correcciones, favor de esperar un momento.");
                    }

                    var menusGenerales = _context.tblP_Menu.Where(x => x.generalSIGOPLAN).Select(x => x.id).ToList();
                    var sistemasGenerales = _context.tblP_Sistema.Where(x => x.general).Select(x => x.id).ToList();
                    var accionesVista = _context.tblP_AccionesVista.ToList();
                    var accionesVistaUsuario = _context.tblP_AccionesVistatblP_Usuario.ToList();
                    var obj = new UsuarioDTO();
                    obj.id = usuario.id;
                    obj.idPerfil = usuario.perfil.id;
                    obj.perfil = usuario.perfil.nombre;
                    obj.nombre = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
                    obj.cc = usuario.cc;
                    obj.esCliente = usuario.cliente;
                    obj.puestoID = usuario.puestoID;
                    obj.puesto = usuario.puesto;
                    obj.departamento = usuario.puesto.departamento;
                    obj.contrasena = usuario.contrasena;
                    obj.nombreUsuario = usuario.nombreUsuario;
                    obj.sistemas = new List<tblP_Sistema>();
                    obj.empresas = new List<tblP_EmpresasDTO>();
                    obj.permisos2 = new List<PermisosDTO>();
                    obj.tipoSGC = usuario.tipoSGC;
                    obj.usuarioSGC = usuario.usuarioSGC;
                    obj.tipoSeguridad = usuario.tipoSeguridad;
                    obj.usuarioSeguridad = usuario.usuarioSeguridad;
                    obj.esAuditor = usuario.esAuditor;
                    obj.gestionRH = usuario.gestionRH;
                    obj.usuarioGeneral = usuario.usuarioGeneral;
                    obj.estatus = usuario.estatus;
                    obj.cveEmpleado = usuario.cveEmpleado;
                    obj.esColombia = usuario.esColombia;
                    obj.isBajio = usuario.isBajio;
                    obj.externoPatoos = usuario.externoPatoos;
                    obj.externoPatoosNombre = usuario.externoPatoosNombre;
                    obj.tipoPatoos = usuario.tipoPatoos;
                    if (obj.perfil.Equals("Administrador"))
                    {
                        try
                        {
                            obj.sistemas = _context.tblP_Sistema.Where(x => x.estatus == true).ToList();
                            foreach (var item in obj.sistemas)
                            {
                                if (item.esVirtual)
                                {
                                    if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                    item.url += "/SISTEMA/Sistema/GetReedireccion/?blob=" + Encriptacion.encriptar(obj.nombreUsuario) + "@" + Encriptacion.encriptar(obj.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                                }
                            }
                            obj.permisos = _context.tblP_Menu.Where(x => x.visible).ToList();
                            obj.permisos2 = obj.permisos.Select(p => new PermisosDTO()
                            {
                                menu = p,
                                accion = accionesVista.Where(a => a.vistaID.Equals(p.id)).ToList()
                            }).ToList();
                            var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                            var empresasTemp = _context.tblP_Empresas.Where(x => x.estatus && (obj.departamento.id == 14 ? true : !x.desarrollo)).ToList();
                            obj.empresas.AddRange(
                                empresasTemp.Select(x => new tblP_EmpresasDTO
                                {
                                    id = x.id,
                                    nombre = x.nombre,
                                    activo = x.activo,
                                    estatus = x.estatus,
                                    desarrollo = x.desarrollo,
                                    icono = x.icono,
                                    iconoRedireccion = x.iconoRedireccion,
                                    url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                                }).ToList()
                            );

                        }
                        catch (Exception)
                        {
                            obj.sistemas = null;
                            obj.permisos = null;
                        }
                    }
                    else if (obj.esAuditor)
                    {
                        try
                        {
                            obj.sistemas = _context.tblP_Sistema.Where(x => x.estatus == true).ToList();
                            foreach (var item in obj.sistemas)
                            {
                                if (item.esVirtual)
                                {
                                    if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                    item.url += "/SISTEMA/Sistema/GetReedireccion/?blob=" + Encriptacion.encriptar(obj.nombreUsuario) + "@" + Encriptacion.encriptar(obj.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                                }
                            }
                            obj.permisos = _context.tblP_Menu.Where(x => (x.tipo == 1 ? (x.activo && x.visible) : (x.visible && x.activo && x.liberado)) && !x.desarrollo).ToList();
                            ////--> Agregar permiso especial para omar.ramirez a modulo de desempeño
                            //var permisoDesempenio = _context.tblP_Menu.FirstOrDefault(x => x.id == 7598);
                            //if (obj.id == 1079 || obj.id == 1080) obj.permisos.Add(permisoDesempenio);
                            ////<--
                            obj.permisos2 = obj.permisos.Where(m => accionesVista.Any(av =>
                                        av.vistaID.Equals(m.id) &&
                                            accionesVistaUsuario.Any(avu =>
                                                avu.tblP_AccionesVista_id.Equals(av.id) &&
                                                avu.tblP_Usuario_id.Equals(obj.id))))
                                    .Select(m => new PermisosDTO()
                                    {
                                        menu = m,
                                        accion = accionesVista.Where(av => m.id.Equals(av.vistaID)).ToList()
                                    }).ToList();
                            var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                            var empresasTemp = _context.tblP_Empresas.Where(x => listaEmpresas.Contains(x.id) && x.estatus && !x.desarrollo).ToList();
                            obj.empresas.AddRange(
                                empresasTemp.Select(x => new tblP_EmpresasDTO
                                {
                                    id = x.id,
                                    nombre = x.nombre,
                                    activo = x.activo,
                                    estatus = x.estatus,
                                    desarrollo = x.desarrollo,
                                    icono = x.icono,
                                    iconoRedireccion = x.iconoRedireccion,
                                    url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                                }).ToList()
                            );

                        }
                        catch (Exception)
                        {
                            obj.sistemas = null;
                            obj.permisos = null;
                            obj.permisos2 = null;
                        }
                    }
                    else if (obj.perfil.Equals("AccesoUnico"))
                    {
                        List<int> sistemaInt = new List<int>();
                        List<tblP_Menu> PermisosFull = new List<tblP_Menu>();


                        var lstPermisos = _context.tblP_MenutblP_Usuario.Where(x => x.tblP_Usuario_id == obj.id).ToList();
                        var lstPermisosRel = lstPermisos.Select(x => x.tblP_Menu_id).Distinct().ToList();

                        var lstPermisosMenu = _context.tblP_Menu.Where(x => (lstPermisosRel.Contains(x.id) || x.generalSIGOPLAN == true) && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && !x.desarrollo && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList();
                        sistemaInt.AddRange(lstPermisosMenu.Select(x => x.sistemaID).Distinct().ToList());
                        sistemaInt = sistemaInt.Distinct().ToList();

                        PermisosFull.AddRange(lstPermisosMenu);

                        var sistemas = _context.tblP_Sistema.Where(x => sistemaInt.Distinct().Contains(x.id)).ToList();

                        obj.sistemas = sistemas;
                        foreach (var item in obj.sistemas)
                        {
                            if (item.esVirtual)
                            {
                                if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                item.url += "/SISTEMA/Sistema/GetReedireccion/?blob=" + Encriptacion.encriptar(obj.nombreUsuario) + "@" + Encriptacion.encriptar(obj.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                            }
                        }
                        obj.permisos = PermisosFull.Distinct().ToList();
                        obj.permisos2 = obj.permisos.Where(m => accionesVista.Any(av =>
                                        av.vistaID.Equals(m.id) &&
                                            accionesVistaUsuario.Any(avu =>
                                                avu.tblP_AccionesVista_id.Equals(av.id) &&
                                                avu.tblP_Usuario_id.Equals(obj.id))))
                                    .Select(m => new PermisosDTO()
                                    {
                                        menu = m,
                                        accion = accionesVista.Where(av => m.id.Equals(av.vistaID)).ToList()
                                    }).ToList();
                        var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                        var empresasTemp = _context.tblP_Empresas.Where(x => listaEmpresas.Contains(x.id) && x.estatus && !x.desarrollo).ToList();
                        obj.empresas.AddRange(
                            empresasTemp.Select(x => new tblP_EmpresasDTO
                            {
                                id = x.id,
                                nombre = x.nombre,
                                activo = x.activo,
                                estatus = x.estatus,
                                desarrollo = x.desarrollo,
                                icono = x.icono,
                                iconoRedireccion = x.iconoRedireccion,
                                url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                            }).ToList()
                        );
                    }
                    else if (obj.perfil.Equals("Contratista"))
                    {
                        List<int> sistemaInt = new List<int>();
                        List<tblP_Menu> PermisosFull = new List<tblP_Menu>();


                        var lstPermisos = _context.tblP_MenutblP_Usuario.Where(x => x.tblP_Usuario_id == obj.id).ToList();
                        var lstPermisosRel = lstPermisos.Select(x => x.tblP_Menu_id).Distinct().ToList();

                        var lstPermisosMenu = _context.tblP_Menu.Where(x => (lstPermisosRel.Contains(x.id) || x.generalSIGOPLAN == true) && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && !x.desarrollo && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList();
                        sistemaInt.AddRange(lstPermisosMenu.Select(x => x.sistemaID).Distinct().ToList());
                        sistemaInt = sistemaInt.Distinct().ToList();

                        PermisosFull.AddRange(lstPermisosMenu);

                        var sistemas = _context.tblP_Sistema.Where(x => sistemaInt.Distinct().Contains(x.id)).ToList();

                        obj.sistemas = sistemas;
                        foreach (var item in obj.sistemas)
                        {
                            if (item.esVirtual)
                            {
                                if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                item.url += "/SISTEMA/Sistema/GetReedireccion/?blob=" + Encriptacion.encriptar(obj.nombreUsuario) + "@" + Encriptacion.encriptar(obj.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                            }
                        }
                        obj.permisos = PermisosFull.Distinct().ToList();
                        obj.permisos2 = obj.permisos.Where(m => accionesVista.Any(av =>
                                        av.vistaID.Equals(m.id) &&
                                            accionesVistaUsuario.Any(avu =>
                                                avu.tblP_AccionesVista_id.Equals(av.id) &&
                                                avu.tblP_Usuario_id.Equals(obj.id))))
                                    .Select(m => new PermisosDTO()
                                    {
                                        menu = m,
                                        accion = accionesVista.Where(av => m.id.Equals(av.vistaID)).ToList()
                                    }).ToList();
                        var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                        var empresasTemp = _context.tblP_Empresas.Where(x => listaEmpresas.Contains(x.id) && x.estatus && !x.desarrollo).ToList();
                        obj.empresas.AddRange(
                            empresasTemp.Select(x => new tblP_EmpresasDTO
                            {
                                id = x.id,
                                nombre = x.nombre,
                                activo = x.activo,
                                estatus = x.estatus,
                                desarrollo = x.desarrollo,
                                icono = x.icono,
                                iconoRedireccion = x.iconoRedireccion,
                                url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                            }).ToList()
                        );
                    }
                    else
                    {

                        List<int> sistemaInt = new List<int>();
                        List<tblP_Menu> PermisosFull = new List<tblP_Menu>();
                        var sistemasGeneral = _context.tblP_Sistema.Where(x => x.general == true && x.estatus == true && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList();
                        sistemaInt.AddRange(sistemasGeneral.Where(x => x.general == true).Select(x => x.id));
                        var seg = _context.tblP_Sistema.FirstOrDefault(x => x.nombre.Equals("Seguridad"));
                        if (usuario.tipoSeguridad && seg.activo)
                        {
                            sistemaInt.Add(_context.tblP_Sistema.FirstOrDefault(x => x.nombre.Equals("Seguridad")).id);
                        }
                        var menusGeneralSIGOPLAN = _context.tblP_Menu.Where(x => x.generalSIGOPLAN == true && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList();

                        var lstPermisos = _context.tblP_MenutblP_Usuario.Where(x => x.tblP_Usuario_id == obj.id).ToList();
                        var lstPermisosRel = lstPermisos.Select(x => x.tblP_Menu_id).Distinct().ToList();

                        var lstPermisosMenu = _context.tblP_Menu.Where(x => (lstPermisosRel.Contains(x.id) || x.generalSIGOPLAN == true) && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && !x.desarrollo && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList();
                        sistemaInt.AddRange(lstPermisosMenu.Select(x => x.sistemaID).Distinct().ToList());
                        sistemaInt = sistemaInt.Distinct().ToList();
                        var generalPorSistema = _context.tblP_Menu.Where(x => (x.generalPorSistema == true && sistemaInt.Contains(x.sistemaID)) && x.sistema.estatus == true && x.sistema.activo == true && x.visible == true && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList();

                        PermisosFull.AddRange(lstPermisosMenu);
                        PermisosFull.AddRange(generalPorSistema);

                        var sistemas = _context.tblP_Sistema.Where(x => sistemaInt.Distinct().Contains(x.id) && (usuario.sistemasGenerales ? !sistemasGenerales.Contains(x.id) : true) && (vSesiones.sesionEmpresaActual != 3 && obj.esColombia ? x.esColombia : true)).ToList();

                        obj.sistemas = sistemas;
                        foreach (var item in obj.sistemas)
                        {
                            if (item.esVirtual)
                            {
                                if (vSesiones.sesionBestRouting == 1) item.url = ":3676";
                                item.url += "/SISTEMA/Sistema/GetReedireccion/?blob=" + Encriptacion.encriptar(obj.nombreUsuario) + "@" + Encriptacion.encriptar(obj.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting ?? "1";
                            }
                        }
                        obj.permisos = PermisosFull.Where(x => (usuario.sistemasGenerales ? !menusGenerales.Contains(x.id) : true)).Distinct().ToList();
                        obj.permisos2 = obj.permisos.Where(m => accionesVista.Any(av =>
                                        av.vistaID.Equals(m.id) &&
                                            accionesVistaUsuario.Any(avu =>
                                                avu.tblP_AccionesVista_id.Equals(av.id) &&
                                                avu.tblP_Usuario_id.Equals(obj.id))))
                                    .Select(m => new PermisosDTO()
                                    {
                                        menu = m,
                                        accion = accionesVista.Where(av => m.id.Equals(av.vistaID)).ToList()
                                    }).ToList();
                        var listaEmpresas = _context.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == usuario.id).Select(x => x.tblP_Empresas_id).ToList();
                        var empresasTemp = _context.tblP_Empresas.Where(x => listaEmpresas.Contains(x.id) && x.estatus && !x.desarrollo).ToList();
                        obj.empresas.AddRange(
                            empresasTemp.Select(x => new tblP_EmpresasDTO
                            {
                                id = x.id,
                                nombre = x.nombre,
                                activo = x.activo,
                                estatus = x.estatus,
                                desarrollo = x.desarrollo,
                                icono = x.icono,
                                iconoRedireccion = x.iconoRedireccion,
                                url = (vSesiones.sesionBestRouting == 1 ? (x.urlLocal) : (vSesiones.sesionBestRouting == 2) ? x.urlInterna : (vSesiones.sesionBestRouting == 4) ? x.urlPrueba : x.url)
                            }).ToList()
                        );
                    }
                    obj.tipoSGC = usuario.tipoSGC;
                    obj.usuarioSGC = usuario.usuarioSGC;
                    obj.tipoSeguridad = usuario.tipoSeguridad;
                    obj.usuarioSeguridad = usuario.usuarioSeguridad;
                    obj.dashboardMaquinariaAdmin = usuario.dashboardMaquinariaAdmin;
                    obj.dashboardMaquinariaPermiso = usuario.dashboardMaquinariaPermiso;
                    obj.correosVinculados = getCorreosVinculados();
                    obj.correo = usuario.correo;
                    result = obj;
                    vSesiones.sesionUsuarioDTO = result;
                    return result;
                }
                else
                    throw new Exception("La contraseña ingresada no es correcta.");
            }
        }

        public tblP_Usuario ObtenerPorNombreUsuario(string user)
        {
            var u = _context.tblP_Usuario.ToList().FirstOrDefault(a => a.nombreUsuario == user && a.estatus == true);
            return u;

        }
        private bool validatePassword(string password, string passwordBD)
        {
            var temp = Encriptacion.encriptar(password);
            var desc = Encriptacion.desencriptar(HttpUtility.UrlDecode(passwordBD));
            return passwordBD.Equals(temp) ? true : false;
        }

        private bool validatePasswordEncryptado(string password, string passwordBD)
        {
            var temp = password;
            return passwordBD.Equals(temp) ? true : false;
        }


        private bool validateUsuario(string usuario, string usuariodBD)
        {
            return (usuario == usuariodBD) ? true : false;
        }
        public List<tblP_Usuario> ListUsersByName(string user)
        {
            if (user.Length == 0)
            {
                return _context.tblP_Usuario.Where(x => x.estatus == true && !string.IsNullOrEmpty(x.correo))
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.apellidoPaterno)
                    .ThenBy(x => x.apellidoMaterno)
                    .Take(10).ToList();
            }
            else
            {
                return _context.tblP_Usuario.Where(a => (a.estatus == true && !string.IsNullOrEmpty(a.correo)) && (a.nombre + " " + a.apellidoPaterno + " " + a.apellidoMaterno).Contains(user))
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.apellidoPaterno)
                    .ThenBy(x => x.apellidoMaterno)
                    .Take(10).ToList();
            }
        }
        public List<tblP_Usuario> ListUsersByNameWithException(string user)
        {
            var userException = _context.tblRH_AutorizanteExclusion.Select(x => x.usuarioID).ToList();
            if (user.Length == 0)
            {
                return _context.tblP_Usuario.Where(x => !userException.Contains(x.id) && x.estatus == true && !string.IsNullOrEmpty(x.correo) && x.cliente == false)
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.apellidoPaterno)
                    .ThenBy(x => x.apellidoMaterno)
                    .Take(10).ToList();
            }
            else
            {
                return _context.tblP_Usuario.Where(a => !userException.Contains(a.id) && (a.estatus == true && !string.IsNullOrEmpty(a.correo)) && (a.nombre + " " + a.apellidoPaterno + " " + a.apellidoMaterno).Contains(user) && a.cliente == false)
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.apellidoPaterno)
                    .ThenBy(x => x.apellidoMaterno)
                    .Take(10).ToList();
            }
        }
        public List<tblP_Usuario> ListUsersByNameAndMinute(string user, int id)
        {
            var lista = _context.tblSA_Participante.Where(x => x.minutaID == id).Select(x => x.participanteID);
            if (user.Length == 0)
            {

                return _context.tblP_Usuario.Where(x => !lista.Contains(x.id) && x.estatus)
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.apellidoPaterno)
                    .ThenBy(x => x.apellidoMaterno)
                    .Take(10).ToList();
            }
            else
            {
                return _context.tblP_Usuario.Where(a => (a.nombre + " " + a.apellidoPaterno + " " + a.apellidoMaterno).Contains(user) && !lista.Contains(a.id) && a.estatus)
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.apellidoPaterno)
                    .ThenBy(x => x.apellidoMaterno)
                    .Take(10).ToList();
            }
        }
        public tblP_Usuario UserByName(string user)
        {
            return _context.tblP_Usuario.FirstOrDefault(a => (a.nombre + " " + a.apellidoPaterno + " " + a.apellidoMaterno).Equals(user));
        }

        public List<tblP_Usuario> ListUsersByNameAndActivity(string user, int id)
        {
            if (user.Length == 0)
            {
                return _context.tblP_Usuario.Where(x => /*!lista.Contains(x.id) &&*/ x.estatus == true)
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.apellidoPaterno)
                    .ThenBy(x => x.apellidoMaterno)
                    .Take(10).ToList();
            }
            else
            {
                return _context.tblP_Usuario.Where(a => (a.nombre + " " + a.apellidoPaterno + " " + a.apellidoMaterno).Contains(user) && /*!lista.Contains(a.id) &&*/ a.estatus == true)
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.apellidoPaterno)
                    .ThenBy(x => x.apellidoMaterno)
                    .Take(20).ToList();
            }
        }

        public List<tblP_Usuario> ListResponsablesByNameAndActivity(string user, int id)
        {
            if (user.Length == 0)
            {
                return _context.tblP_Usuario.Where(x => x.estatus == true)
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.apellidoPaterno)
                    .ThenBy(x => x.apellidoMaterno)
                    .Take(10).ToList();
            }
            else
            {
                return _context.tblP_Usuario.Where(a => (a.nombre + " " + a.apellidoPaterno + " " + a.apellidoMaterno).Contains(user) && a.estatus == true)
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.apellidoPaterno)
                    .ThenBy(x => x.apellidoMaterno)
                    .Take(20).ToList();
            }
        }
        public List<tblP_Usuario> ListUsuariosAutoComplete(string user, int id)
        {
            if (user.Length == 0)
            {
                return _context.tblP_Usuario.Where(x => x.id.Equals(id))
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.apellidoPaterno)
                    .ThenBy(x => x.apellidoMaterno)
                    .Take(10).ToList();
            }
            else
            {
                return _context.tblP_Usuario.Where(a => (a.nombre + " " + a.apellidoPaterno + " " + a.apellidoMaterno).Contains(user))
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.apellidoPaterno)
                    .ThenBy(x => x.apellidoMaterno)
                    .Take(20).ToList();
            }
        }

        public List<tblP_Usuario> ListUsersById(int user)
        {
            var data = _context.tblP_Usuario.Where(x => x.id.Equals(user)).ToList();
            var enk = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == user);
            if (enk != null)
            {
                data.First().idEnkontrol = enk.empleado;
            }

            return data;
        }
        public List<tblP_Usuario> ListUsersActivos()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var data = _context.tblP_Usuario.Where(x => x.estatus && !x.cliente).ToList();
            Random random = new Random();
            foreach (var item in data)
            {
                item.contrasena = Encriptacion.desencriptar(HttpUtility.UrlDecode(item.contrasena));
            }

            //data = data.Where(x => x.contrasena.Contains("Proyecto")).ToList();

            foreach (var item in data)
            {
                if (item.contrasena.Contains("Proyecto"))
                {
                    var nuevaContrasena = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
                    item.contrasena = Encriptacion.encriptar(nuevaContrasena);
                    item.enviar = true;
                }
                else
                {
                    item.contrasena = Encriptacion.encriptar(item.contrasena);
                }
            }

            _context.SaveChanges();

            return data;
        }
        public tblP_Usuario getPassByID(int id)
        {
            var temp = _context.tblP_Usuario.FirstOrDefault(x => x.id == id);
            return temp;
        }
        public string getNombreSistemaActual()
        {
            if (vSesiones.sesionSistemaActual == 0)
                return "";
            else
                return _context.tblP_Sistema.FirstOrDefault(x => x.id == vSesiones.sesionSistemaActual).nombre;
        }
        public List<tblP_Usuario> ListUsersAll()
        {
            List<tblP_Usuario> data = _context.Select<tblP_Usuario>(new DapperDTO
            {
                baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                consulta = @"SELECT ISNULL(b.empleado,0) as idEnkontrol,a.* FROM tblP_Usuario a left join tblP_Usuario_Enkontrol b on a.id = b.idUsuario where a.cliente=0",
                parametros = new { registroActivo = true }
            }).ToList();
            return data;
        }
        public List<tblP_Sistema> ListSistemasAll()
        {
            return _context.tblP_Sistema.Where(x => x.estatus == true).ToList();
        }
        public List<tblP_UsuariotblP_Empresas> getUsuarioEmpresa(int IdUsuario)
        {
            return _context.tblP_UsuariotblP_Empresas.Where(w => w.tblP_Usuario_id.Equals(IdUsuario)).ToList();
        }
        /// <summary>
        /// Selecciona las empresas que guardara/actualizara en base a vSesiones.sesionEmpresaActual
        /// </summary>
        /// <returns>Listado de empresas</returns>
        List<EmpresaEnum> LstEmpresaParaGuardar()
        {
            var lstEmpresa = new List<EmpresaEnum>();
            var empresaActual = (EmpresaEnum)vSesiones.sesionEmpresaActual;
            switch (empresaActual)
            {
                case EmpresaEnum.Construplan:
                case EmpresaEnum.Arrendadora:
                    lstEmpresa.Add(EmpresaEnum.Construplan);
                    lstEmpresa.Add(EmpresaEnum.Arrendadora);
                    break;
                default:
                    lstEmpresa.Add(empresaActual);
                    break;
            }
            return lstEmpresa;
        }
        public tblP_Usuario SaveUsuario(tblP_Usuario user)
        {
            tblP_Usuario exUsuario = new tblP_Usuario();

            var lstID = new List<int>();
            var lstEmpresa = LstEmpresaParaGuardar();
            lstEmpresa.ForEach(empresa =>
            {
                using (var _db = new MainContext(empresa))
                {
                    if (user.id != 0)
                    {
                        exUsuario = _db.tblP_Usuario.Where(x => x.id == user.id).FirstOrDefault();
                        exUsuario.apellidoMaterno = user.apellidoMaterno ?? string.Empty;
                        exUsuario.apellidoPaterno = user.apellidoPaterno;
                        exUsuario.correo = user.correo;
                        exUsuario.nombre = user.nombre;
                        exUsuario.nombreUsuario = user.nombreUsuario;
                        exUsuario.contrasena = user.contrasena != null ? Encriptacion.encriptar(user.contrasena) : exUsuario.contrasena;
                        exUsuario.perfilID = user.perfilID;
                        exUsuario.puestoID = user.puestoID;
                        exUsuario.enviar = user.enviar;
                        exUsuario.estatus = user.estatus;
                        exUsuario.cveEmpleado = user.cveEmpleado;
                        exUsuario.usuarioAuditor = user.usuarioAuditor;
                        exUsuario.esAuditor = user.esAuditor;
                        exUsuario.tipoSeguridad = user.tipoSeguridad;
                        exUsuario.usuarioSeguridad = user.usuarioSeguridad;
                        exUsuario.externoPatoos = user.externoPatoos;
                        exUsuario.externoPatoosNombre = user.externoPatoos ? "visitante" : null;
                        _db.SaveChanges();

                        if (exUsuario.tipoSeguridad)
                        {
                            List<dynamic> insertado = _context.Select<dynamic>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.Construplan,
                                consulta = @"select * from SEGURIDAD..tblUsuario where nombreUsuario = '" + exUsuario.nombreUsuario + "'",
                                parametros = new { registroActivo = true }
                            }).ToList();

                            if (insertado.Count > 0)
                            {
                                List<dynamic> actualizaR = _context.Select<dynamic>(new DapperDTO
                                {
                                    baseDatos = MainContextEnum.Construplan,
                                    consulta = @"update SEGURIDAD..tblUsuario set nombreUsuario = '" + exUsuario.nombreUsuario + "', nombreCompleto = '" + exUsuario.nombre + "' where nombreUsuario = '" + exUsuario.nombreUsuario + "'",
                                    parametros = new { registroActivo = true }
                                }).ToList();
                            }
                            else
                            {
                                List<dynamic> insertar = _context.Select<dynamic>(new DapperDTO
                                {
                                    baseDatos = MainContextEnum.Construplan,
                                    consulta = @"insert into SEGURIDAD..tblUsuario values('" + exUsuario.nombreUsuario + "','" + exUsuario.nombre + "','1234',0); select 1",
                                    parametros = new { registroActivo = true }
                                }).ToList();
                            }
                        }

                        if (user.idEnkontrol != 0)
                        {
                            var temp = _db.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == exUsuario.id);
                            if (temp != null)
                            {
                                temp.idUsuario = exUsuario.id;
                                temp.empleado = user.idEnkontrol;
                                temp.sn_empleado = string.IsNullOrEmpty(exUsuario.cveEmpleado) ? 0 : int.Parse(exUsuario.cveEmpleado);
                                temp.password = "";
                                _db.SaveChanges();
                            }
                            else
                            {
                                temp = new tblP_Usuario_Enkontrol();
                                temp.idUsuario = exUsuario.id;
                                temp.empleado = user.idEnkontrol;
                                temp.sn_empleado = string.IsNullOrEmpty(exUsuario.cveEmpleado) ? 0 : int.Parse(exUsuario.cveEmpleado);
                                temp.password = "";
                                _db.tblP_Usuario_Enkontrol.Add(temp);
                                _db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        exUsuario = new tblP_Usuario()
                        {
                            id = 0,
                            apellidoPaterno = user.apellidoPaterno,
                            apellidoMaterno = user.apellidoMaterno ?? string.Empty,
                            perfil = user.perfil,
                            cc = user.cc,
                            cliente = user.cliente,
                            contrasena = Encriptacion.encriptar(user.contrasena),
                            correo = user.correo,
                            cveEmpleado = user.cveEmpleado,
                            enviar = user.enviar,
                            estatus = user.estatus,
                            nombre = user.nombre,
                            nombreUsuario = user.nombreUsuario,
                            perfilID = user.perfilID,
                            permisos = user.permisos,
                            permisosPorVista = user.permisosPorVista,
                            puesto = user.puesto,
                            puestoID = user.puestoID,
                            tipoSGC = user.tipoSGC,
                            usuarioSGC = user.usuarioSGC,
                            usuarioAuditor = user.usuarioAuditor,
                            esAuditor = user.esAuditor,
                            tipoSeguridad = user.tipoSeguridad,
                            usuarioSeguridad = user.usuarioSeguridad,
                            externoPatoos = user.externoPatoos,
                            externoPatoosNombre = (user.externoPatoos ? "visitante" : null)
                        };

                        _db.tblP_Usuario.Add(exUsuario);
                        _db.SaveChanges();

                        if (exUsuario.tipoSeguridad)
                        {
                            List<dynamic> insertado = _context.Select<dynamic>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.Construplan,
                                consulta = @"select * from SEGURIDAD..tblUsuario where nombreUsuario = '" + exUsuario.nombreUsuario + "'",
                                parametros = new { registroActivo = true }
                            }).ToList();

                            if (insertado.Count > 0)
                            {
                                List<dynamic> actualizaR = _context.Select<dynamic>(new DapperDTO
                                {
                                    baseDatos = MainContextEnum.Construplan,
                                    consulta = @"update SEGURIDAD..tblUsuario set nombreUsuario = '" + exUsuario.nombreUsuario + "', nombreCompleto = '" + exUsuario.nombre + "' where nombreUsuario = '" + exUsuario.nombreUsuario + "'",
                                    parametros = new { registroActivo = true }
                                }).ToList();
                            }
                            else
                            {
                                List<dynamic> insertar = _context.Select<dynamic>(new DapperDTO
                                {
                                    baseDatos = MainContextEnum.Construplan,
                                    consulta = @"insert into SEGURIDAD..tblUsuario values('" + exUsuario.nombreUsuario + "','" + exUsuario.nombre + "','1234',0); select 1",
                                    parametros = new { registroActivo = true }
                                }).ToList();
                            }
                        }

                        if (user.idEnkontrol != 0)
                        {
                            var temp = new tblP_Usuario_Enkontrol();
                            temp.idUsuario = exUsuario.id;
                            temp.empleado = user.idEnkontrol;
                            temp.sn_empleado = string.IsNullOrEmpty(exUsuario.cveEmpleado) ? 0 : int.Parse(exUsuario.cveEmpleado);
                            temp.password = "";
                            _db.tblP_Usuario_Enkontrol.Add(temp);
                            _db.SaveChanges();
                        }
                    }

                    lstID.Add(exUsuario.id);
                }
            });
            var primero = lstID.FirstOrDefault();
            var flag = lstID.All(i => i == primero);
            var respuesta = flag && exUsuario.id > 0 ? exUsuario : new tblP_Usuario();
            return respuesta;
        }

        public Dictionary<string, object> GuardarUsuario(tblP_Usuario usuario, List<tblP_MenutblP_Usuario> permisos, List<string> ccs, List<tblP_AccionesVistatblP_Usuario> accVistas, int sistema, List<int> empresa)
        {
            //Se mantiene la lógica de la función vieja por el momento.

            var resultado = new Dictionary<string, object>();

            try
            {
                usuario.usuarioSGC = "visitante";
                usuario.tipoSGC = false;
                usuario.empresa = string.Empty;

                #region Save Usuario
                tblP_Usuario registroUsuario = new tblP_Usuario();

                #region Lista Empresas Para Guardar
                List<EmpresaEnum> listaEmpresas = new List<EmpresaEnum>();
                EmpresaEnum empresaActual = (EmpresaEnum)vSesiones.sesionEmpresaActual;

                switch (empresaActual)
                {
                    case EmpresaEnum.Construplan:
                    case EmpresaEnum.Arrendadora:
                        listaEmpresas.Add(EmpresaEnum.Construplan);
                        listaEmpresas.Add(EmpresaEnum.Arrendadora);
                        break;
                    default:
                        listaEmpresas.Add(empresaActual); //Esto podría cambiar: en el default podrían ir Construplan y Arrendadora también para guardar en las tres empresas.
                        break;
                }
                #endregion

                foreach (var emp in listaEmpresas)
                {
                    using (var _contextEmpresa = new MainContext(emp))
                    {
                        using (var dbContextTransaction = _contextEmpresa.Database.BeginTransaction())
                        {
                            try
                            {
                                #region Registro Principal Usuario
                                if (usuario.id == 0)
                                {
                                    #region Guardar Nuevo Usuario
                                    registroUsuario.nombre = usuario.nombre;
                                    registroUsuario.apellidoPaterno = usuario.apellidoPaterno;
                                    registroUsuario.apellidoMaterno = usuario.apellidoMaterno ?? string.Empty;
                                    registroUsuario.nombreUsuario = usuario.nombreUsuario;
                                    registroUsuario.correo = usuario.correo;
                                    //registroUsuario.empresa = ;
                                    registroUsuario.perfilID = usuario.perfilID;
                                    registroUsuario.perfil = usuario.perfil;
                                    registroUsuario.contrasena = Encriptacion.encriptar(usuario.contrasena);
                                    registroUsuario.estatus = usuario.estatus;
                                    registroUsuario.permisos = usuario.permisos;
                                    registroUsuario.puestoID = usuario.puestoID;
                                    registroUsuario.puesto = usuario.puesto;
                                    registroUsuario.cc = usuario.cc;
                                    registroUsuario.permisosPorVista = usuario.permisosPorVista;
                                    registroUsuario.enviar = usuario.enviar;
                                    registroUsuario.cliente = usuario.cliente;
                                    registroUsuario.tipoSGC = usuario.tipoSGC;
                                    registroUsuario.usuarioSGC = usuario.usuarioSGC;
                                    registroUsuario.usuarioAuditor = usuario.usuarioAuditor;
                                    registroUsuario.cveEmpleado = usuario.cveEmpleado;
                                    registroUsuario.tipoSeguridad = usuario.tipoSeguridad;
                                    registroUsuario.usuarioSeguridad = usuario.usuarioSeguridad;
                                    //registroUsuario.usuarioMAZDA = ;
                                    //registroUsuario.dashboardMaquinariaPermiso = ;
                                    //registroUsuario.dashboardMaquinariaAdmin = ;
                                    registroUsuario.esAuditor = usuario.esAuditor;
                                    //registroUsuario.externoSeguridad = ;
                                    //registroUsuario.sistemasGenerales = ;
                                    //registroUsuario.gestionRH = ;
                                    //registroUsuario.usuarioGeneral = ;
                                    //registroUsuario.externoGestor = ;
                                    //registroUsuario.esColombia = ;
                                    //registroUsuario.isBajio = ;
                                    registroUsuario.externoPatoos = usuario.externoPatoos;
                                    registroUsuario.externoPatoosNombre = usuario.externoPatoos ? "visitante" : null;
                                    //registroUsuario.tipoPatoos = ;
                                    //registroUsuario.idEnkontrol = ;

                                    _contextEmpresa.tblP_Usuario.Add(registroUsuario);
                                    _contextEmpresa.SaveChanges();

                                    #region Usuario Tipo Seguridad
                                    if (registroUsuario.tipoSeguridad)
                                    {
                                        var registroUsuarioSeguridad = _context.Select<dynamic>(new DapperDTO
                                        {
                                            baseDatos = MainContextEnum.Construplan,
                                            consulta = string.Format(@"SELECT * FROM SEGURIDAD..tblUsuario WHERE nombreUsuario = '{0}'", registroUsuario.nombreUsuario)
                                        }).FirstOrDefault();

                                        if (registroUsuarioSeguridad == null)
                                        {
                                            var queryInsertar = _context.Select<dynamic>(new DapperDTO
                                            {
                                                baseDatos = MainContextEnum.Construplan,
                                                consulta = string.Format(@"INSERT INTO SEGURIDAD..tblUsuario VALUES('{0}', '{1}', '1234', 0)", registroUsuario.nombreUsuario, registroUsuario.nombre)
                                            }).ToList();
                                        }
                                        else
                                        {
                                            var queryActualizar = _context.Select<dynamic>(new DapperDTO
                                            {
                                                baseDatos = MainContextEnum.Construplan,
                                                consulta = string.Format(@"UPDATE SEGURIDAD..tblUsuario SET nombreUsuario = '{0}', nombreCompleto = '{1}' WHERE nombreUsuario = '{0}'", registroUsuario.nombreUsuario, registroUsuario.nombre)
                                            }).ToList();
                                        }
                                    }
                                    #endregion

                                    #region Usuario Enkontrol
                                    if (usuario.idEnkontrol != 0)
                                    {
                                        _contextEmpresa.tblP_Usuario_Enkontrol.Add(new tblP_Usuario_Enkontrol()
                                        {
                                            idUsuario = registroUsuario.id,
                                            empleado = usuario.idEnkontrol,
                                            sn_empleado = !string.IsNullOrEmpty(registroUsuario.cveEmpleado) ? int.Parse(registroUsuario.cveEmpleado) : 0,
                                            password = ""
                                        });
                                        _contextEmpresa.SaveChanges();
                                    }
                                    #endregion
                                    #endregion
                                }
                                else
                                {
                                    #region Editar Usuario
                                    registroUsuario = _contextEmpresa.tblP_Usuario.FirstOrDefault(x => x.id == usuario.id);

                                    registroUsuario.nombre = usuario.nombre;
                                    registroUsuario.apellidoMaterno = usuario.apellidoMaterno ?? string.Empty;
                                    registroUsuario.apellidoPaterno = usuario.apellidoPaterno;
                                    registroUsuario.nombreUsuario = usuario.nombreUsuario;
                                    registroUsuario.correo = usuario.correo;
                                    registroUsuario.perfilID = usuario.perfilID;
                                    registroUsuario.contrasena = usuario.contrasena != null ? Encriptacion.encriptar(usuario.contrasena) : registroUsuario.contrasena;
                                    registroUsuario.puestoID = usuario.puestoID;
                                    registroUsuario.estatus = usuario.estatus;
                                    registroUsuario.enviar = usuario.enviar;
                                    registroUsuario.cveEmpleado = usuario.cveEmpleado;
                                    registroUsuario.usuarioAuditor = usuario.usuarioAuditor;
                                    registroUsuario.esAuditor = usuario.esAuditor;
                                    registroUsuario.tipoSeguridad = usuario.tipoSeguridad;
                                    registroUsuario.usuarioSeguridad = usuario.usuarioSeguridad;
                                    registroUsuario.externoPatoos = usuario.externoPatoos;
                                    registroUsuario.externoPatoosNombre = usuario.externoPatoos ? "visitante" : null;

                                    _contextEmpresa.SaveChanges();

                                    #region Usuario Tipo Seguridad
                                    if (registroUsuario.tipoSeguridad)
                                    {
                                        var registroUsuarioSeguridad = _context.Select<dynamic>(new DapperDTO
                                        {
                                            baseDatos = MainContextEnum.Construplan,
                                            consulta = string.Format(@"SELECT * FROM SEGURIDAD..tblUsuario WHERE nombreUsuario = '{0}'", registroUsuario.nombreUsuario)
                                        }).FirstOrDefault();

                                        if (registroUsuarioSeguridad == null)
                                        {
                                            var queryInsertar = _context.Select<dynamic>(new DapperDTO
                                            {
                                                baseDatos = MainContextEnum.Construplan,
                                                consulta = string.Format(@"INSERT INTO SEGURIDAD..tblUsuario VALUES('{0}', '{1}', '1234', 0)", registroUsuario.nombreUsuario, registroUsuario.nombre)
                                            }).ToList();
                                        }
                                        else
                                        {
                                            var queryActualizar = _context.Select<dynamic>(new DapperDTO
                                            {
                                                baseDatos = MainContextEnum.Construplan,
                                                consulta = string.Format(@"UPDATE SEGURIDAD..tblUsuario SET nombreUsuario = '{0}', nombreCompleto = '{1}' WHERE nombreUsuario = '{0}'", registroUsuario.nombreUsuario, registroUsuario.nombre)
                                            }).ToList();
                                        }
                                    }
                                    #endregion

                                    #region Usuario Enkontrol
                                    if (usuario.idEnkontrol != 0)
                                    {
                                        var registroUsuarioEnkontrol = _contextEmpresa.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == registroUsuario.id);

                                        if (registroUsuarioEnkontrol == null)
                                        {
                                            _contextEmpresa.tblP_Usuario_Enkontrol.Add(new tblP_Usuario_Enkontrol()
                                            {
                                                idUsuario = registroUsuario.id,
                                                empleado = usuario.idEnkontrol,
                                                sn_empleado = !string.IsNullOrEmpty(registroUsuario.cveEmpleado) ? int.Parse(registroUsuario.cveEmpleado) : 0,
                                                password = ""
                                            });
                                            _contextEmpresa.SaveChanges();
                                        }
                                        else
                                        {
                                            registroUsuarioEnkontrol.idUsuario = registroUsuario.id;
                                            registroUsuarioEnkontrol.empleado = usuario.idEnkontrol;
                                            registroUsuarioEnkontrol.sn_empleado = !string.IsNullOrEmpty(registroUsuario.cveEmpleado) ? int.Parse(registroUsuario.cveEmpleado) : 0;
                                            registroUsuarioEnkontrol.password = "";

                                            _contextEmpresa.SaveChanges();
                                        }
                                    }
                                    #endregion
                                    #endregion
                                }
                                #endregion

                                #region Vistas
                                if (permisos != null)
                                {
                                    #region Eliminar Permisos Anteriores
                                    var objRemoveAV = _contextEmpresa.tblP_AccionesVistatblP_Usuario.Where(x => x.sistema == sistema && x.tblP_Usuario_id == registroUsuario.id).ToList();
                                    _contextEmpresa.tblP_AccionesVistatblP_Usuario.RemoveRange(objRemoveAV);
                                    _contextEmpresa.SaveChanges();

                                    var objRemoveV = _contextEmpresa.tblP_MenutblP_Usuario.Where(x => x.tblP_Usuario_id == registroUsuario.id && x.sistema == sistema).ToList();
                                    _contextEmpresa.tblP_MenutblP_Usuario.RemoveRange(objRemoveV);
                                    _contextEmpresa.SaveChanges();
                                    #endregion

                                    #region Agregar Nuevos Permisos
                                    permisos.ForEach(x => x.tblP_Usuario_id = registroUsuario.id);

                                    _contextEmpresa.tblP_MenutblP_Usuario.AddRange(permisos);
                                    _contextEmpresa.SaveChanges();
                                    #endregion
                                }
                                #endregion

                                #region Acciones
                                if (accVistas != null)
                                {
                                    accVistas.ForEach(x => x.tblP_Usuario_id = registroUsuario.id);

                                    _contextEmpresa.tblP_AccionesVistatblP_Usuario.AddRange(accVistas);
                                    _contextEmpresa.SaveChanges();
                                }
                                #endregion

                                #region Empresas
                                if (empresa != null)
                                {
                                    #region Eliminar Permisos Empresas Anteriores
                                    var tbl_empresa = _contextEmpresa.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == registroUsuario.id).ToList();

                                    _contextEmpresa.tblP_UsuariotblP_Empresas.RemoveRange(tbl_empresa);
                                    _contextEmpresa.SaveChanges();
                                    #endregion

                                    #region Agregar Nuevos Permisos Empresas
                                    listaEmpresas.ForEach(x =>
                                    {
                                        var empresa_id = (int)x;

                                        _contextEmpresa.tblP_UsuariotblP_Empresas.Add(new tblP_UsuariotblP_Empresas()
                                        {
                                            tblP_Usuario_id = registroUsuario.id,
                                            tblP_Empresas_id = empresa_id
                                        });
                                        _contextEmpresa.SaveChanges();
                                    });
                                    #endregion
                                }
                                #endregion

                                dbContextTransaction.Commit();
                            }
                            catch (Exception e)
                            {
                                dbContextTransaction.Rollback();

                                throw new Exception(e.Message);
                            }
                        }
                    }
                }
                #endregion

                #region Permisos Centros de Costo
                if (ccs == null)
                {
                    ccs = new List<string>();
                }

                var arrendadoraContext = new MainContext(2);
                var tablaCC = arrendadoraContext.tblP_CC.ToList();

                var ccsAutoriza = new List<string>();
                var usuarioAutorizaPorAgregar = new List<tblP_Autoriza>();

                // Verificamos si tiene para autorizar en arrendadora.
                var autorizaciones = arrendadoraContext.tblP_Autoriza.Where(x => x.usuarioID == registroUsuario.id).ToList();
                var autorizacionesPorAgregar = new List<AutorizaDTO>();

                if (autorizaciones.Count > 0)
                {
                    //Lógica de actualizar autorizaciones.
                    var ccsAutorizaUsuario = arrendadoraContext.tblP_CC_Usuario.Where(x => x.usuarioID == registroUsuario.id).ToList();

                    foreach (var autorizacion in autorizaciones)
                    {
                        var autorizacionExistente = ccsAutorizaUsuario.FirstOrDefault(x => x.id == autorizacion.cc_usuario_ID);
                        if (autorizacionExistente != null)
                        {
                            autorizacionesPorAgregar.Add(new AutorizaDTO { usuarioID = autorizacion.usuarioID, perfilID = autorizacion.perfilAutorizaID, cc = autorizacionExistente.cc });
                        }
                    }
                }
                var ccsUsuariosPorAgregar = new List<tblP_CC_Usuario>();
                var acsUsuariosPorAgregar = new List<tblP_CC_Usuario>();
                var nomUsuariosPorAgregar = new List<tblP_CC_Usuario>();
                tablaCC.Where(x => ccs.Select(y => Convert.ToInt32(y)).ToList().Contains(x.id)).ToList().ForEach(x =>
                {
                    // Se agrega Area cuenta
                    acsUsuariosPorAgregar.Add(new tblP_CC_Usuario
                    {
                        cc = x.areaCuenta,
                        usuarioID = registroUsuario.id
                    });

                    ccsUsuariosPorAgregar.Add(new tblP_CC_Usuario
                    {
                        cc = x.cc,
                        usuarioID = registroUsuario.id
                    });

                    // Se agrega ccRH en caso de aplicar.
                    if (String.IsNullOrEmpty(x.ccRH) == false)
                    {
                        nomUsuariosPorAgregar.Add(new tblP_CC_Usuario
                        {
                            cc = x.ccRH,
                            usuarioID = registroUsuario.id
                        });
                    }
                });
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                {
                    var contextContruplan = new MainContext((int)EmpresaEnum.Construplan);
                    using (var transaction = contextContruplan.Database.BeginTransaction())
                    {
                        // Eliminamos y agregamos nuevos CCs / Usuario.
                        var ccsUsuarioPorEliminar = contextContruplan.tblP_CC_Usuario.Where(x => x.usuarioID == registroUsuario.id).ToList();
                        contextContruplan.tblP_CC_Usuario.RemoveRange(ccsUsuarioPorEliminar);
                        contextContruplan.SaveChanges();

                        contextContruplan.tblP_CC_Usuario.AddRange(ccsUsuariosPorAgregar);
                        contextContruplan.SaveChanges();

                        transaction.Commit();
                    }

                    var contextArrendadora = new MainContext((int)EmpresaEnum.Arrendadora);
                    using (var transaction = contextArrendadora.Database.BeginTransaction())
                    {
                        // Eliminamos y agregamos nuevos CCs / Usuario.
                        var autPorEliminar = contextArrendadora.tblP_Autoriza.Where(x => x.usuarioID == registroUsuario.id).ToList();
                        contextArrendadora.tblP_Autoriza.RemoveRange(autPorEliminar);
                        contextArrendadora.SaveChanges();

                        var ccsUsuarioPorEliminar = contextArrendadora.tblP_CC_Usuario.Where(x => x.usuarioID == registroUsuario.id).ToList();
                        contextArrendadora.tblP_CC_Usuario.RemoveRange(ccsUsuarioPorEliminar);
                        contextArrendadora.SaveChanges();

                        contextArrendadora.tblP_CC_Usuario.AddRange(acsUsuariosPorAgregar);
                        contextArrendadora.tblP_CC_Usuario.AddRange(nomUsuariosPorAgregar);
                        contextArrendadora.SaveChanges();

                        if (autorizacionesPorAgregar.Count > 0)
                        {
                            // Filtramos por los ccs que apliquen.
                            var ccAuto = acsUsuariosPorAgregar.Select(x => x.cc).ToList();
                            var ccAplica = autorizacionesPorAgregar.Where(x => ccAuto.Contains(x.cc)).ToList();
                            List<tblP_Autoriza> nuevos = new List<tblP_Autoriza>();
                            foreach (var item in ccAplica)
                            {
                                var temp = acsUsuariosPorAgregar.FirstOrDefault(x => x.cc.Equals(item.cc));
                                var o = new tblP_Autoriza();
                                o.usuarioID = item.usuarioID;
                                o.perfilAutorizaID = item.perfilID;
                                o.cc_usuario_ID = temp.id;
                                nuevos.Add(o);
                            }
                            contextArrendadora.tblP_Autoriza.AddRange(nuevos);
                            contextArrendadora.SaveChanges();
                        }

                        transaction.Commit();
                    }
                }
                else
                {
                    var contextEmpresa = new MainContext((int)vSesiones.sesionEmpresaActual);
                    using (var transaction = contextEmpresa.Database.BeginTransaction())
                    {
                        // Eliminamos y agregamos nuevos CCs / Usuario.
                        var ccsUsuarioPorEliminar = contextEmpresa.tblP_CC_Usuario.Where(x => x.usuarioID == registroUsuario.id).ToList();
                        contextEmpresa.tblP_CC_Usuario.RemoveRange(ccsUsuarioPorEliminar);
                        contextEmpresa.SaveChanges();

                        contextEmpresa.tblP_CC_Usuario.AddRange(ccsUsuariosPorAgregar);
                        contextEmpresa.SaveChanges();

                        transaction.Commit();
                    }
                }
                #endregion

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public List<tblP_MenutblP_Usuario> getVistasUsuario(int id, int idVista)
        {
            var result = _context.tblP_MenutblP_Usuario.Where(x => x.tblP_Usuario_id == id).ToList();
            return result;
        }

        public List<tblP_AccionesVistatblP_Usuario> getAccionesUsuario(int id)
        {
            return _context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == id).ToList();
        }

        public List<tblP_CC_Usuario> getCCsUsuario(int id)
        {
            return _context.tblP_CC_Usuario.Where(x => x.usuarioID == id).ToList();
        }
        public List<tblP_CC> getCCsByUsuario(int id)
        {
            var ccs = _context.tblP_CC_Usuario.Where(x => x.usuarioID == id).Select(x => x.cc).ToList();

            if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
            {
                return _context.tblP_CC.Where(x => ccs.Contains(x.cc) && x.estatus).OrderBy(x => x.cc).Distinct().ToList();
            }
            else
            {
                return _context.tblP_CC.Where(x => ccs.Contains(x.areaCuenta) && x.estatus).OrderBy(x => x.area).ThenBy(x => x.cuenta).Distinct().ToList();
            }
        }
        public void SavePermisos(List<tblP_MenutblP_Usuario> permiso)
        {
            var lstEmpresa = LstEmpresaParaGuardar();
            lstEmpresa.ForEach(empresa =>
            {
                using (var _db = new MainContext(empresa))
                {
                    _db.tblP_MenutblP_Usuario.AddRange(permiso);
                    _db.SaveChanges();
                }
            });
        }
        public void SaveCCsUsuario(tblP_CC_Usuario Ccs)
        {
            var cc = _context.tblP_CC.FirstOrDefault(w => w.id.ToString().Equals(Ccs.cc));
            var lst = new List<string>() { cc.cc, cc.areaCuenta, cc.ccRH ?? string.Empty };
            var lstEmpresa = LstEmpresaParaGuardar();
            lstEmpresa.ForEach(empresa =>
            {
                using (var _db = new MainContext(empresa))
                {
                    lst.ForEach(c =>
                    {
                        if (!string.IsNullOrEmpty(c))
                        {
                            _db.tblP_CC_Usuario.Add(new tblP_CC_Usuario()
                            {
                                id = 0,
                                cc = c,
                                usuarioID = Ccs.usuarioID
                            });
                            _db.SaveChanges();
                        }
                    });
                }

            });
        }
        public void SavePermisosVista(List<tblP_AccionesVistatblP_Usuario> permiso)
        {
            var lstEmpresa = LstEmpresaParaGuardar();
            lstEmpresa.ForEach(empresa =>
            {
                using (var _db = new MainContext(empresa))
                {
                    _db.tblP_AccionesVistatblP_Usuario.AddRange(permiso);
                    _db.SaveChanges();
                }
            });
        }
        public void saveUsuarioEmpresa(int idUsuario, List<int> lstEmpresa)
        {
            var lstEmpresaSave = LstEmpresaParaGuardar();
            lstEmpresaSave.ForEach(empresa =>
            {
                using (var _db = new MainContext(empresa))
                {
                    lstEmpresa.ForEach(x =>
                    {
                        _db.tblP_UsuariotblP_Empresas.Add(new tblP_UsuariotblP_Empresas()
                        {
                            tblP_Usuario_id = idUsuario,
                            tblP_Empresas_id = x
                        });
                        _db.SaveChanges();
                    });
                }
            });
        }
        public void DeletePermisos(int sistema, int idUsuario, List<tblP_MenutblP_Usuario> lstPermisos, List<tblP_AccionesVistatblP_Usuario> lstAcciones)
        {
            var lstEmpresaSave = LstEmpresaParaGuardar();
            lstEmpresaSave.ForEach(empresa =>
            {
                using (var _db = new MainContext(empresa))
                {
                    var objRemoveAV = _db.tblP_AccionesVistatblP_Usuario.Where(x => x.sistema == sistema && x.tblP_Usuario_id == idUsuario).ToList();
                    _db.tblP_AccionesVistatblP_Usuario.RemoveRange(objRemoveAV);
                    _db.SaveChanges();
                    var objRemoveV = _db.tblP_MenutblP_Usuario.Where(x => x.tblP_Usuario_id == idUsuario && x.sistema == sistema).ToList();
                    _db.tblP_MenutblP_Usuario.RemoveRange(objRemoveV);
                    _db.SaveChanges();
                }
            });
        }
        public void DeleteCcsUsuario(int idUsuario)
        {
            var lstEmpresaSave = LstEmpresaParaGuardar();
            lstEmpresaSave.ForEach(empresa =>
            {
                using (var _db = new MainContext(empresa))
                {
                    var objRemoveCC = _db.tblP_CC_Usuario.Where(x => x.usuarioID == idUsuario);
                    _db.tblP_CC_Usuario.RemoveRange(objRemoveCC);
                    _db.SaveChanges();
                }
            });
        }
        public void DeleteUsuarioEmpresa(int idUsuario)
        {
            var lstEmpresaSave = LstEmpresaParaGuardar();
            lstEmpresaSave.ForEach(empresa =>
            {
                using (var _db = new MainContext(empresa))
                {
                    var tbl_empresa = _db.tblP_UsuariotblP_Empresas.Where(x => x.tblP_Usuario_id == idUsuario).ToList();
                    _db.tblP_UsuariotblP_Empresas.RemoveRange(tbl_empresa);
                    _db.SaveChanges();
                }
            });
        }
        public bool getViewAction(int viewID, string accion)
        {
            var result = (
                    from a in _context.tblP_AccionesVista
                    join b in _context.tblP_AccionesVistatblP_Usuario on a.id equals b.tblP_AccionesVista_id
                    where (a.vistaID == viewID && a.Accion.Equals(accion) && b.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id)
                    select a
                ).ToList();
            //var result = _context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_AccionesVista_id == viewID && x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id).ToList();
            if (result.Count > 0)
                return true;
            else
            {
                return false;
            }
        }
        public bool getViewActionTemp(int viewID, string accion, int usuarioID)
        {
            var result = (from a in _context.tblP_AccionesVista join b in _context.tblP_AccionesVistatblP_Usuario on a.id equals b.tblP_AccionesVista_id where (a.vistaID == viewID && a.Accion.Equals(accion) && b.tblP_Usuario_id == usuarioID) select a).ToList();
            //var result = _context.tblP_AccionesVistatblP_Usuario.FirstOrDefault(x => x.tblP_AccionesVista_id == viewID && x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id);
            if (result.Count > 0)
                return true;
            else
            {
                return false;
            }
        }
        public void replacePass()
        {
            var _usuarios = _context.tblP_Usuario.ToList();
            var lista = new List<replacePassDTO>();
            foreach (var i in _usuarios)
            {
                var o = new replacePassDTO();
                o.usuario = i.nombreUsuario;
                o.contrasena = Encriptacion.desencriptarTemp(i.contrasena);
                o.md5 = Encriptacion.encriptar(o.contrasena);
                //lista.Add(o);
                i.contrasena = o.md5;
                _context.SaveChanges();
            }
        }


        public List<tblP_Usuario> GetUsuariosByPuesto(List<int> idPuesto)
        {
            var result = _context.tblP_Usuario.Join(idPuesto, o => o.puestoID, id => id, (o, id) => o).ToList();
            return result;
        }

        public List<tblP_Usuario> getListUsuarioByName(string user)
        {
            return _context.tblP_Usuario.Where(a => (a.nombre + " " + a.apellidoPaterno + " " + a.apellidoMaterno).Contains(user) && a.estatus)
                    .OrderBy(x => x.nombre)
                    .ThenBy(x => x.apellidoPaterno)
                    .ThenBy(x => x.apellidoMaterno)
                    .Take(10).ToList();
        }

        public void enviarAcceso()
        {
            var data = _context.tblP_Usuario.Where(x => x.enviar && !x.cliente).ToList();

            if (data.Count == 0)
            {
                throw new Exception("No hay usuarios con el campo de enviar activado.");
            }

            foreach (var i in data)
            {
                var pass = Encriptacion.desencriptar(HttpUtility.UrlDecode(i.contrasena));

                var subject = "Acceso SIGOPLAN";

                var body = @"Buen día<br/><br/>
                        Le proporcionamos sus datos de acceso para el sistema SIGOPLAN en el cual podrá acceder a los sub-sistemas que se vayan liberando. Los datos son los siguientes: <br/>
                        <br/>
                        Usuario: " + i.nombreUsuario + @"<br/>
                        Contraseña: " + pass + @"<br/>
                        <br/>
                        Acceso local: http://10.1.0.2 <br/>
                        Acceso desde cualquier lugar: http://sigoplan.construplan.com.mx/ <br/>
                        <br/>
                        Favor de responder este correo con la nueva contraseña que gusta poner, la contraseña " + pass + @" es solo temporal.<br/>
                        <br/>
                        Saludos.";

                List<string> contactos = new List<string>();

                contactos.Add(i.correo);

                if (GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, contactos) == false)
                {
                    throw new Exception("Ocurrió un error al enviar el correo al usuario " + GlobalUtils.ObtenerNombreCompletoUsuario(i));
                }

                i.enviar = false;
                _context.SaveChanges();
            }
        }

        public List<tblP_Autoriza> getPerfilesUsuario(int tipoControl, string cc)
        {
            return (from a in _context.tblP_Autoriza
                    join b in _context.tblP_CC_Usuario on a.cc_usuario_ID equals b.id
                    where b.cc == cc && a.perfilAutorizaID == 5 && a.usuario.estatus != false
                    select a).ToList();
        }


        public List<string> getListaCorreosEnvioGlobal(int modulo, string cc)
        {
            List<string> listaRes = new List<string>();
            var LsitaCoreos = _context.tblP_EnvioCorreos.Where(x => x.moduloID == modulo);
            var lstUsuarioCCPermisos = _context.tblP_CC_Usuario.Where(r => LsitaCoreos.Select(x => x.usuarioID).ToList().Contains(r.usuarioID) && r.cc.Equals(cc)).ToList();
            listaRes.AddRange(LsitaCoreos.Where(x => x.estatus == true).Select(x => x.usuario.correo).ToList());
            var listaDeCCSinPermiso = lstUsuarioCCPermisos.Select(x => x.usuarioID).ToList();
            var ListaUsuariosCentrocostos = _context.tblP_Usuario.Where(x => listaDeCCSinPermiso.Contains(x.id)).Select(x => x.correo).ToList();
            listaRes.AddRange(ListaUsuariosCentrocostos);
            return listaRes;
        }
        public List<string> getListaCorreosEnvioGlobal(string cc)
        {
            List<string> listaRes = new List<string>();
            var LsitaCoreos = _context.tblM_ControlEnvioMaquinaria_CorreosCC.Where(x => x.cc.Equals(cc)).Select(x => x.usuarioID).ToList();
            var ListaUsuariosCentrocostos = _context.tblP_Usuario.Where(x => LsitaCoreos.Contains(x.id)).Select(x => x.correo).ToList();
            listaRes.AddRange(ListaUsuariosCentrocostos);
            return listaRes;
        }
        public List<tblP_Departamento> getLstDept()
        {
            return _context.tblP_Departamento.ToList();
        }
        public List<tblP_Puesto> getLstPuesto(int idDept)
        {
            return _context.tblP_Puesto.Where(p => p.departamentoID == idDept).ToList();
        }
        public List<tblP_Puesto> getAllPuesto()
        {
            return _context.tblP_Puesto.Where(p => p.estatus).ToList();
        }
        public List<tblP_Perfil> getLstPerfilActivo()
        {
            return _context.tblP_Perfil.Where(p => p.estatus).ToList();
        }
        public List<tblP_Empresas> getLstEmpresasActivas()
        {
            return _context.tblP_Empresas.Where(p => p.activo).ToList();
        }
        public List<int> getIdCCsUsuario(int id)
        {
            var lstUs = _context.tblP_CC_Usuario.Where(x => x.usuarioID == id).ToList();
            var lstCC = _context.tblP_CC.ToList();
            return lstCC.Where(c => lstUs.Any(u => u.cc.Equals(c.cc))).Select(c => c.id).ToList();
        }

        public List<int> ObtenerIDsCCsUsuarioAutoriza(int usuarioID)
        {
            try
            {
                var arrendadoraContext = new MainContext(2);

                var autorizaciones = arrendadoraContext.tblP_Autoriza
                    .Where(x => x.usuarioID == usuarioID)
                    .Select(x => x.cc_usuario_ID)
                    .ToList();

                if (autorizaciones.Count == 0)
                {
                    return new List<int>();
                }

                var areasCuenta = arrendadoraContext.tblP_CC_Usuario
                    .Where(x => autorizaciones.Contains(x.id))
                    .Select(x => x.cc)
                    .ToList();

                var tablaCC = arrendadoraContext.tblP_CC.ToList();
                var result = tablaCC.Where(c => areasCuenta.Contains(c.areaCuenta)).Select(c => c.id).ToList();
                return result;
            }
            catch (Exception e)
            {
                LogError(0, 0, "UsuariosController", "ObtenerIDsCCsUsuarioAutoriza", e, AccionEnum.CONSULTA, usuarioID, null);
                return new List<int>();
            }
        }

        public List<tblP_CC> getUsuarioCC(int id)
        {
            var lstUs = _context.tblP_CC_Usuario.Where(x => x.usuarioID == id).ToList();
            var lstCC = _context.tblP_CC.ToList();
            return lstCC.Where(c => lstUs.Any(u => u.cc.Equals(c.cc))).ToList();
        }
        public tblP_Usuario_Enkontrol getUserEk(int idUsuario)
        {
            return _context.tblP_Usuario_Enkontrol.FirstOrDefault(u => u.idUsuario == idUsuario);
        }
        public tblP_Usuario_Enkontrol saveUsuarioEnkontrol(tblP_Usuario_Enkontrol usuario)
        {
            var save = _context.tblP_Usuario_Enkontrol.FirstOrDefault(u => u.idUsuario.Equals(usuario.idUsuario));
            var isSave = save == null;
            if (isSave)
                save = new tblP_Usuario_Enkontrol();
            save.idUsuario = usuario.idUsuario;
            save.empleado = usuario.empleado;
            save.sn_empleado = usuario.sn_empleado;
            _context.tblP_Usuario_Enkontrol.AddOrUpdate(save);
            SaveChanges();
            //SaveBitacora((int)BitacoraEnum.UsuarioEnkontrol, isSave ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, save.id, JsonUtils.convertNetObjectToJson(save));
            //SaveChanges();
            return usuario;
        }

        public List<tblP_PermisosAutorizaCorreo> getPermisosAutorizaCorreo(int permiso)
        {
            return _context.tblP_PermisosAutorizaCorreo.Where(x => x.estatus && x.permiso == permiso).ToList();
        }
        public List<string> getFinalMailList(List<string> listaActual)
        {
            List<string> listaFinal = new List<string>();
            var usuarioFiltrada = listaActual.Where(x => x != null && !x.Equals(""));
            var usuariosActual = _context.tblP_Usuario.Where(x => usuarioFiltrada.Contains(x.correo) && x.estatus == true && x.cliente == false).Select(x => x.id).ToList();
            var vinculados = _context.tblP_CorreosVinculados.Where(x => usuariosActual.Contains(x.usuarioPrincipalID) && x.usuarioVinculado.estatus == true).Select(x => x.usuarioVinculado.correo).ToList();
            listaActual.AddRange(vinculados);


            listaFinal.AddRange(listaActual.Where(x => x != null && !x.Equals("")).Distinct());

            return listaFinal;
        }
        public List<UsuariosVinculadosDTO> getCorreosVinculados()
        {
            List<UsuariosVinculadosDTO> listaFinal = new List<UsuariosVinculadosDTO>();

            var data = _context.tblP_CorreosVinculados.Select(x => new UsuariosVinculadosDTO { principal = x.usuarioPrincipalID, principalCorreo = x.usuarioPrincipal.correo, vinculado = x.usuarioVinculadoID, vinculadoCorreo = x.usuarioVinculado.correo }).ToList();

            listaFinal.AddRange(data);
            return listaFinal;
        }

        public bool NecesitaIngresarDatosEnKontrol()
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                return _context.tblP_Usuario_Enkontrol.Any(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id) == false;
            }
            catch (Exception e)
            {
                LogError(vSesiones.sesionSistemaActual, 0, "BaseController", "setCurrentSystem", e, AccionEnum.CONSULTA, vSesiones.sesionUsuarioDTO.id, null);
                return false;
            }
        }

        public Dictionary<string, object> ValidarDatosUsuarioEnKontrol(string password, int claveEmpleado)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (string.IsNullOrEmpty(password) || claveEmpleado <= 0)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Datos insuficientes para validación.");
                    return resultado;
                }
                else if (password[0] != '0')
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Toda clave debe comenzar con cero.");
                    return resultado;
                }

                int usuarioActualID = vSesiones.sesionUsuarioDTO.id;

                password = password.Trim();

                var usuarioEnKontrol = Convert.ToInt32(password.Substring(0, 4));
                var passwordEnKontrol = string.Join("", password.Skip(4));

                EmpresaEnum[] enumEmpresas = new EmpresaEnum[] { EmpresaEnum.Construplan, EmpresaEnum.Arrendadora };

                foreach (var empresa in enumEmpresas)
                {
                    var contextEmpresa = new MainContext((int)empresa);
                    using (var transaction = contextEmpresa.Database.BeginTransaction())
                    {
                        try
                        {
                            var registro = contextEmpresa.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.empleado == usuarioEnKontrol);

                            if (registro != null)
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ya hay un registro existente para el usuario.");
                                return resultado;
                            }

                            var nuevoRegistro = new tblP_Usuario_Enkontrol
                            {
                                idUsuario = usuarioActualID,
                                empleado = usuarioEnKontrol,
                                sn_empleado = claveEmpleado,
                                password = passwordEnKontrol
                            };

                            contextEmpresa.tblP_Usuario_Enkontrol.Add(nuevoRegistro);

                            var usuario = contextEmpresa.tblP_Usuario.First(x => x.id == usuarioActualID);

                            usuario.cveEmpleado = claveEmpleado.ToString();
                            contextEmpresa.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                }

                var registrosEmpresas = new List<Tuple<tblP_Usuario_Enkontrol, EmpresaEnum>>();

                foreach (var empresa in enumEmpresas)
                {
                    var registro = new MainContext((int)empresa).tblP_Usuario_Enkontrol.FirstOrDefault(x => x.empleado == usuarioEnKontrol);
                    if (registro != null)
                    {
                        registrosEmpresas.Add(Tuple.Create(registro, empresa));
                    }
                }

                if (registrosEmpresas.Count == 1)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar verificar los datos en el servidor.");

                    try
                    {
                        var registro = registrosEmpresas.FirstOrDefault();
                        var contextEmpresa = new MainContext((int)registro.Item2);
                        var regitroCorrupto = contextEmpresa.tblP_Usuario_Enkontrol.First(x => x.empleado == registro.Item1.empleado);
                        contextEmpresa.tblP_Usuario_Enkontrol.Remove(regitroCorrupto);
                        contextEmpresa.SaveChanges();
                    }
                    catch (Exception)
                    {
                    }
                }

                if (registrosEmpresas.Count == 2)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("redireccionURl", "/Home/Index");
                }
            }
            catch (Exception e)
            {
                //dbContextTransaction.Rollback();
                LogError(0, 0, "UsuariosController", "ValidarDatosUsuarioEnKontrol", e, AccionEnum.AGREGAR, claveEmpleado, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar verificar los datos en el servidor.");
            }
            return resultado;
        }

        public void ActualizarCCsUsuario(List<int> ccsIDs, int usuarioID)
        {
            var arrendadoraContext = new MainContext(2);
            var tablaCC = arrendadoraContext.tblP_CC.ToList();

            var ccsAutoriza = new List<string>();
            var usuarioAutorizaPorAgregar = new List<tblP_Autoriza>();

            // Verificamos si tiene para autorizar en arrendadora.
            var autorizaciones = arrendadoraContext.tblP_Autoriza.Where(x => x.usuarioID == usuarioID).ToList();
            var autorizacionesPorAgregar = new List<AutorizaDTO>();

            if (autorizaciones.Count > 0)
            {
                //Lógica de actualizar autorizaciones.
                var ccsAutorizaUsuario = arrendadoraContext.tblP_CC_Usuario
                    .Where(x => x.usuarioID == usuarioID)
                    .ToList();

                foreach (var autorizacion in autorizaciones)
                {
                    var autorizacionExistente = ccsAutorizaUsuario.FirstOrDefault(x => x.id == autorizacion.cc_usuario_ID);
                    if (autorizacionExistente != null)
                    {
                        autorizacionesPorAgregar.Add(new AutorizaDTO { usuarioID = autorizacion.usuarioID, perfilID = autorizacion.perfilAutorizaID, cc = autorizacionExistente.cc });
                    }
                }
            }
            var ccsUsuariosPorAgregar = new List<tblP_CC_Usuario>();
            var acsUsuariosPorAgregar = new List<tblP_CC_Usuario>();
            var nomUsuariosPorAgregar = new List<tblP_CC_Usuario>();
            tablaCC.Where(x => ccsIDs.Contains(x.id)).ToList().ForEach(x =>
            {
                // Se agrega Area cuenta
                acsUsuariosPorAgregar.Add(new tblP_CC_Usuario
                {
                    cc = x.areaCuenta,
                    usuarioID = usuarioID
                });

                ccsUsuariosPorAgregar.Add(new tblP_CC_Usuario
                {
                    cc = x.cc,
                    usuarioID = usuarioID
                });

                // Se agrega ccRH en caso de aplicar.
                if (String.IsNullOrEmpty(x.ccRH) == false)
                {
                    nomUsuariosPorAgregar.Add(new tblP_CC_Usuario
                    {
                        cc = x.ccRH,
                        usuarioID = usuarioID
                    });
                }
            });
            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
            {
                var contextContruplan = new MainContext((int)EmpresaEnum.Construplan);
                using (var transaction = contextContruplan.Database.BeginTransaction())
                {
                    // Eliminamos y agregamos nuevos CCs / Usuario.
                    var ccsUsuarioPorEliminar = contextContruplan.tblP_CC_Usuario.Where(x => x.usuarioID == usuarioID).ToList();
                    contextContruplan.tblP_CC_Usuario.RemoveRange(ccsUsuarioPorEliminar);
                    contextContruplan.SaveChanges();

                    contextContruplan.tblP_CC_Usuario.AddRange(ccsUsuariosPorAgregar);
                    contextContruplan.SaveChanges();

                    transaction.Commit();
                }

                var contextArrendadora = new MainContext((int)EmpresaEnum.Arrendadora);
                using (var transaction = contextArrendadora.Database.BeginTransaction())
                {
                    // Eliminamos y agregamos nuevos CCs / Usuario.
                    var autPorEliminar = contextArrendadora.tblP_Autoriza.Where(x => x.usuarioID == usuarioID).ToList();
                    contextArrendadora.tblP_Autoriza.RemoveRange(autPorEliminar);
                    contextArrendadora.SaveChanges();

                    var ccsUsuarioPorEliminar = contextArrendadora.tblP_CC_Usuario.Where(x => x.usuarioID == usuarioID).ToList();
                    contextArrendadora.tblP_CC_Usuario.RemoveRange(ccsUsuarioPorEliminar);
                    contextArrendadora.SaveChanges();

                    contextArrendadora.tblP_CC_Usuario.AddRange(acsUsuariosPorAgregar);
                    contextArrendadora.tblP_CC_Usuario.AddRange(nomUsuariosPorAgregar);
                    contextArrendadora.SaveChanges();

                    if (autorizacionesPorAgregar.Count > 0)
                    {
                        // Filtramos por los ccs que apliquen.
                        var ccAuto = acsUsuariosPorAgregar.Select(x => x.cc).ToList();
                        var ccAplica = autorizacionesPorAgregar.Where(x => ccAuto.Contains(x.cc)).ToList();
                        List<tblP_Autoriza> nuevos = new List<tblP_Autoriza>();
                        foreach (var item in ccAplica)
                        {
                            var temp = acsUsuariosPorAgregar.FirstOrDefault(x => x.cc.Equals(item.cc));
                            var o = new tblP_Autoriza();
                            o.usuarioID = item.usuarioID;
                            o.perfilAutorizaID = item.perfilID;
                            o.cc_usuario_ID = temp.id;
                            nuevos.Add(o);
                        }
                        contextArrendadora.tblP_Autoriza.AddRange(nuevos);
                        contextArrendadora.SaveChanges();
                    }

                    transaction.Commit();
                }
            }
            else
            {
                var contextEmpresa = new MainContext((int)vSesiones.sesionEmpresaActual);
                using (var transaction = contextEmpresa.Database.BeginTransaction())
                {
                    // Eliminamos y agregamos nuevos CCs / Usuario.
                    var ccsUsuarioPorEliminar = contextEmpresa.tblP_CC_Usuario.Where(x => x.usuarioID == usuarioID).ToList();
                    contextEmpresa.tblP_CC_Usuario.RemoveRange(ccsUsuarioPorEliminar);
                    contextEmpresa.SaveChanges();

                    contextEmpresa.tblP_CC_Usuario.AddRange(ccsUsuariosPorAgregar);
                    contextEmpresa.SaveChanges();

                    transaction.Commit();
                }
            }
        }

        public tblP_Medico GetInformacionMedico(int usuario_id)
        {
            return _context.tblP_Medico.FirstOrDefault(x => x.registroActivo && x.usuario_id == usuario_id);
        }

        public Tuple<Stream, string> DescargarArchivosComprimidos()
        {
            string rutaFolderTemp = "";

            try
            {
                var nombreFolderTemp = String.Format("{0} {1}", "tmp", DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-"));
                rutaFolderTemp = Path.Combine(@"\\REPOSITORIO\Proyecto\SIGOPLAN\temp", nombreFolderTemp);

                Directory.CreateDirectory(rutaFolderTemp);

                List<string> listaRutasArchivos = _context.tblP_ArchivosComprimir.Select(x => x.ruta).ToList();

                // Se copian los archivos al folder temporal
                foreach (var ruta in listaRutasArchivos)
                {
                    try
                    {
                        var nombreArchivo = Path.GetFileName(ruta);
                        var rutaDestino = Path.Combine(rutaFolderTemp, nombreArchivo);

                        File.Copy(ruta, rutaDestino);
                    }
                    catch (Exception e)
                    {
                        LogError(0, 0, "SistemaController", "DescargarArchivosComprimidos_Archivo_" + ruta, e, AccionEnum.DESCARGAR, 0, 0);
                    }
                }

                // Ya que esta la carpeta temporal creada, se crea el zip
                string rutaNuevoZip = Path.Combine(@"\\REPOSITORIO\Proyecto\SIGOPLAN\temp", nombreFolderTemp + ".zip");
                GlobalUtils.ComprimirCarpeta(rutaFolderTemp, rutaNuevoZip);

                // Una vez creado el zip, se elimina el folder temporal 
                // y se obtiene el stream de bytes del zip
                Directory.Delete(rutaFolderTemp, true);
                var zipStream = GlobalUtils.GetFileAsStream(rutaNuevoZip);

                // Una vez cargado el stream, se elimina el zip
                File.Delete(rutaNuevoZip);

                string nombreZip = String.Format("Archivo Comprimido.zip");

                return Tuple.Create(zipStream, nombreZip);
            }
            catch (Exception e)
            {
                try { Directory.Delete(rutaFolderTemp); }
                catch (Exception) { }

                LogError(0, 0, "SistemaController", "DescargarArchivosComprimidos", e, AccionEnum.DESCARGAR, 0, 0);
                return null;
            }
        }
    }
}
