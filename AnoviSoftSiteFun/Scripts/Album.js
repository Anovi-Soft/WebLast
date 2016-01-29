var typeAlbum = "";
var img;
var index = 0;
var dictNames;
var loadedImg = {};
var IsIe = false;

function InitAlbum(dict, isIe) {
    dictNames = dict;
    IsIe = isIe;
    for (var key in dictNames) {
        loadedImg[key] = [];
    }
    window.onhashchange = LocationUpdate;
    if (IsIe) {
        document.onkeyup = KeyUp;
    } else {
        window.onkeyup = KeyUp;
    }
    window.onundo = LocationUpdate;
    LocationUpdate();
    ResizeAlbum();

}
function LocationUpdate() {
    var splt = window.location.toString().split("#");
    if (splt.length > 1) {
        if (splt[1].length > 0) {
            var afterSharp = splt[1].split("-");
            ImageClick(parseInt(afterSharp[1]), afterSharp[0]);
        } else {
            Close();
        }
    } else {
        window.location = "#";
    }
}

function ImageClick(i, name) {
    typeAlbum = name;
    index = i;
    var back = document.getElementById("albumGrayBack");
    back.style.width = "100%";
    back.style.height = "100%";
    back.style.opacity = "1";
    var gbLeft = document.getElementById("gbLeft");
    gbLeft.style.height = "100%";
    gbLeft.style.opacity = "1";
    var gbRight = document.getElementById("gbRight");
    gbRight.style.height = "100%";
    gbRight.style.opacity = "1"; 
    document.getElementById("loading").style.opacity = "1";
    SetImage();
}

function SetImage() {
    window.location.href = "#"+typeAlbum + "-" + index;
    document.getElementById("content").style.opacity = "0";
    document.getElementById("leftAlbum").style.top = "70px";
    document.getElementById("rightAlbum").style.top = "70px";
    if (!IsIe) {
        img = new Image();
        img.src = dictNames[typeAlbum][index];
        img.onload = function() {
            var content = document.getElementById("content");
            if (typeAlbum == "") return;
            content.src = dictNames[typeAlbum][index];
            content.style.opacity = "1";
            document.getElementById("loading").style.opacity = "0";
            ResizeAlbum();
            PreLoad(1, index);
            PreLoad(-1, index);
            document.getElementById("buttonBack").innerHTML = backOn;
            if (checkCookie("backGround") && GetCookie("backGround") == document.getElementById("content").src) {
                document.getElementById("buttonBack").innerHTML = backOff;
            }
        };
    } else {
        var content = document.getElementById("content");
        if (typeAlbum == "") return;
        content.src = dictNames[typeAlbum][index];
        content.style.opacity = "1";
        document.getElementById("loading").style.opacity = "0";
        document.getElementById("loading").style.left = "-100%";
        ResizeAlbum();
        PreLoad(1, index);
        PreLoad(-1, index);
        document.getElementById("buttonBack").innerHTML = backOn;
        if (checkCookie("backGround") && GetCookie("backGround") == document.getElementById("content").src) {
            document.getElementById("buttonBack").innerHTML = backOff;
        }
    }
}

function PreLoad(step, i) {
    i += step;
    if (i < 0)
        i = dictNames[typeAlbum].length - 1;
    if (i >= dictNames[typeAlbum].length)
        i = 0;

    if (typeAlbum == "") return;
    if (loadedImg[typeAlbum].length == dictNames[typeAlbum].length) {
        return;
    }

    if (!IsIe) {
        if (loadedImg[typeAlbum].indexOf(i) == -1) {
            loadedImg[typeAlbum].push(i);
            var img = new Image();
            img.src = dictNames[typeAlbum][i];
            img.onload = function() {
                var preload = document.getElementById("load" + typeAlbum + i);
                if (!preload) {
                    document.getElementById("baseBody").innerHTML += "<img class='unvisiable' id='load" + typeAlbum + i + "' src='" + dictNames[typeAlbum][i] + "'></img>";
                }
            };
        }
    } else {
        var preload = document.getElementById("load" + typeAlbum + i);
        if (!preload) {
            document.getElementById("baseBody").innerHTML += "<img class='unvisiable' id='load" + typeAlbum + i + "' src='" + dictNames[typeAlbum][i] + "'></img>";
        }
    }
}
function ResizeAlbum() {
    var content = document.getElementById("content");
    if (typeAlbum == "") return;

    var w = window,
        d = document,
        e = d.documentElement,
        g = d.getElementsByTagName('body')[0],
        widthWindow = w.innerWidth || e.clientWidth || g.clientWidth,
        heightWindow = w.innerHeight || e.clientHeight || g.clientHeight;

    if (widthWindow < 1000)
        document.getElementById("leftAlbum").style.left = '20px';
    else
        document.getElementById("leftAlbum").style.left = '220px';
    var imgMaxWidth = widthWindow * 0.7;
    var imgMaxHeight = heightWindow * 0.7;

    if (!IsIe) {
        var windowSmallerThenImage = img.width > imgMaxWidth || img.height > imgMaxHeight;
        var setWidth;
        var setHeight;
        var currentImageWidth = parseInt(img.width);
        var currentImageHeight = parseInt(img.height);

        if ((currentImageWidth / imgMaxWidth > currentImageHeight / imgMaxHeight) || !windowSmallerThenImage) {
            setWidth = imgMaxWidth;
            setHeight = img.height * ((img.width > imgMaxWidth) ? imgMaxWidth / img.width : img.width / imgMaxWidth);
        } else {
            setHeight = imgMaxHeight;
            setWidth = img.width * ((img.height > imgMaxHeight) ? imgMaxHeight / img.height : img.height / imgMaxHeight);
        }

        if (widthWindow < 1000) {
            document.getElementById("leftAlbum").style.left = '20px';
            content.style.left = (widthWindow - setWidth) / 2 + "px";
            var gbLeft = document.getElementById("gbLeft");
            gbLeft.style.left = "0";
            gbLeft.style.width = widthWindow / 2 + "px";
            var gbRight = document.getElementById("gbRight");
            gbRight.style.left = widthWindow / 2 + "px";
            gbRight.style.width = widthWindow / 2 + "px";
            document.getElementById("loading").style.left = widthWindow / 2 + "px";

        } else {
            content.style.left = (widthWindow - setWidth) / 2 + 100 + "px";
            document.getElementById("leftAlbum").style.left = '220px';
            var gbLeft = document.getElementById("gbLeft");
            gbLeft.style.left = "200px";
            gbLeft.style.width = (widthWindow / 2 - 100) + "px";
            var gbRight = document.getElementById("gbRight");
            gbRight.style.left = (widthWindow / 2 + 100) + "px";
            gbRight.style.width = (widthWindow / 2 - 100) + "px";
            document.getElementById("loading").style.left = (widthWindow / 2 + 100) + "px";
        }
        content.style.top = (heightWindow - setHeight) / 1.75 + "px";
        content.style.width = setWidth + "px";
        content.style.height = setHeight + "px";

        var button = document.getElementById("buttonBack");
        button.style.top = ((heightWindow - setHeight) / 1.75 + setHeight) + "px";
        if (widthWindow < 1000)
            button.style.left = (widthWindow - setWidth) / 2 + "px";
        else
            button.style.left = (widthWindow - setWidth) / 2 + 100 + "px";
    } else {

        var button = document.getElementById("buttonBack");
        if (typeAlbum == "Films") {
            content.style.width = "auto";
            content.style.height = "70%";
            content.style.top = "18%";
            content.style.left = IsMenuOpend ? "45%" : "35%";
            button.style.top = "18%";
            button.style.left = IsMenuOpend ? "45%" : "35%";
        } else {
            content.style.height = "auto";
            content.style.width = "70%";
            content.style.left = IsMenuOpend ? "20%" : "15%";
            content.style.top = "10%";
            button.style.left = IsMenuOpend ? "20%" : "15%";
            button.style.top = "10%";
        }

    }
    
}
function Next() {
    index = (index + 1) % dictNames[typeAlbum].length;
    SetImage();
}

function Previos() {
    index--;
    if (index < 0)
        index = dictNames[typeAlbum].length - 1;
    SetImage();
}

function Close() {
    //preloadOn = false;
    window.location.href = "#";
    typeAlbum = "";
    var back = document.getElementById("albumGrayBack");
    back.style.width = "0";
    back.style.height = "0";
    back.style.opacity = "0";
    var gbLeft = document.getElementById("gbLeft");
    gbLeft.style.height = "0";
    gbLeft.style.opacity = "0";
    var gbRight = document.getElementById("gbRight");
    gbRight.style.height = "0";
    gbRight.style.opacity = "0";
    var content = document.getElementById("content");
    content.style.height = "0";
    content.style.opacity = "0";
    var button = document.getElementById("buttonBack");
    button.style.left = "-100%";
    document.getElementById("leftAlbum").style.top = "-100%";
    document.getElementById("rightAlbum").style.top = "-100%";
}

function KeyUp(event) {
    event = event || window.event;
    var keyCode = event.keyCode;
    switch (keyCode) {
        case 27:
            if (typeAlbum != "") Close();
            break;
        case 39:
        case 68:
            if (typeAlbum != "") Next();
            break;
        case 37:
        case 65:
            if (typeAlbum != "") Previos();
            break;
        case 112:
            if (typeAlbum != "") alert("Help:\n\nNext picture: button(right), button(d), click(picture)\nPrevios picture: button(left), button(a), click(left)\nClose: button(esc), click(right)\n");
            break;
    default:
        return;
    }
}

var backOn = "<h2>SetAsBackground</h2>";
var backOff = "<h2>UnSetAsBackground</h2>";
var backOffUp = "<H2>UnSetAsBackground</H2>";
function SetAsBackGround(sender) {
    if (sender.innerHTML == backOff || sender.innerHTML == backOffUp) {
        DeleteCookie("backGround");
        sender.innerHTML = backOn;
    } else {
        var content = document.getElementById("content");
        SetCookie("backGround", content.src);
        sender.innerHTML = backOff;
    }
    UpdateBackground();
}