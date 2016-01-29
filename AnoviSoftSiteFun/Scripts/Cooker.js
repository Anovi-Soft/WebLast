
function SetCookie(cname, cvalue) {
    var d = new Date();
    var tmp = document.cookie;
    d.setTime(d.getTime() + (365 * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

function GetCookie(cname) {
    var tmp = document.cookie;
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

function checkCookie(name) {
    return GetCookie(name) != "";
}

function DeleteCookie(name) {
    var tmp = document.cookie;
    document.cookie = name + "=;expires=Wed; 01 Jan 1970";
}