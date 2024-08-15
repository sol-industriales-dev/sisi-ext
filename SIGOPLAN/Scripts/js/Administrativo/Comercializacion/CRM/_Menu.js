//#region CONST MENU
const btnMenu_Proyectos = $("#btnMenu_Proyectos");
const btnMenu_Clientes = $("#btnMenu_Clientes");
const btnMenu_ProspectosClientes = $('#btnMenu_ProspectosClientes');
const btnMenu_Canales = $('#btnMenu_Canales');
const btnMenu_TrackingVentas = $('#btnMenu_TrackingVentas');
const btnMenu_Usuarios = $('#btnMenu_Usuarios');
//#endregion

//#region MENU
btnMenu_Proyectos.click(function () {
    location.href = "Proyectos?menuSeleccion=1";
});

btnMenu_Clientes.click(function () {
    location.href = "Clientes?menuSeleccion=1";
});

btnMenu_ProspectosClientes.click(function () {
    location.href = "ProspectosClientes?menuSeleccion=1";
});

btnMenu_Canales.click(function () {
    location.href = "Canales?menuSeleccion=1";
});

btnMenu_TrackingVentas.click(function () {
    location.href = "TrackingVentas?menuSeleccion=1";
});

btnMenu_Usuarios.click(function () {
    location.href = "UsuariosCRM?menuSeleccion=1";
});
//#endregion