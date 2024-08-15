// Crear Cookie
var crearCookie = function (key, value) {
    expires = new Date();
    expires.setTime(expires.getTime() + 86400000/*31536000000*/); 
    cookie = key + "=" + value + ";expires=" + expires.toUTCString();
    return document.cookie = cookie;
}

// Leer Cookie
var leerCookie = function (key) {
    keyValue = document.cookie.match("(^|;) ?" + key + "=([^;]*)(;|$)");
    if (keyValue) {
        return keyValue[2];
    } else {
        return null;
    }
}

// Eliminar Cookie
var eliminarCookie = function (key) {
    return document.cookie = key + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}