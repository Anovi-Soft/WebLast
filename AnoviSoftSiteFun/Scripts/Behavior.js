
var isBgEffectActive = true;
var timer;
var timerInterval = 500;
var opacity = 0;
var timerStep = 0.05;

var IsMenuActivate = false;
function Init() {

    document.getElementById("topBar").innerHTML = '<h2 id="title">' + document.title.split(' - ')[1].split(' ')[0] + '</h2>' + document.getElementById("topBar").innerHTML;
    document.getElementById("title").style.margin = "0 0 0 65px";
    var column = document.getElementById("center_column"); 
    column.style.maxWidth = "100%";
    column.style.marginLeft = "0";
    column.style.left = "0";
    CloseMenu();
    Resize();

    if (window.addEventListener)
        window.addEventListener('click', Click);
    else {
        document.attachEvent("onclick", Click);
    }
    isBgEffectActive = !checkCookie("backGround");
    if (isBgEffectActive) {

        timer = window.setTimeout("Timer();", timerInterval);
    } else {
        
    UpdateBackground();
    }
}

function UpdateBackground() {
    isBgEffectActive = !checkCookie("backGround");
    if (isBgEffectActive) {
        //var tmp = GetCookie("_bg_opacity");
        //opacity = tmp == undefined ? 0 : parseInt(tmp);
        //window.addEventListener('beforeunload', SaveOpacity);
        var back = document.getElementById("firstBack");
        back.style.background = "url('images/background.png')";
        document.getElementById('secondBack').style.left = "0";
        //timer = window.setTimeout("Timer();", timerInterval);
    } else {
        var src = GetCookie("backGround");
        var back = document.getElementById("firstBack");
        back.style.background = "url('" + src + "')";
        document.getElementById('secondBack').style.opacity = "0";
        document.getElementById('secondBack').style.left = "-100%";
    }
}

//function SaveOpacity() {
//    //alert("---- "+opacity);
//    if (opacity > 1)
//        opacity = 1;
//    if (opacity < 0)
//        opacity = 0;
//    SetCookie("_bg_opacity", opacity);
//    window.onbeforeunload = undefined;
//}

//TIMER
function Timer() {
    if (!isBgEffectActive) return;
    if (opacity >= 1.1 || opacity < -0.1) {
        timerStep *= -1;
    }
    opacity += timerStep;
    document.getElementById('secondBack').style.opacity = "" + opacity;
    timer = window.setTimeout("Timer();", timerInterval);
}

var IsMenuOpend = false;
//RESIZE
function Resize() {
    var w = window,
        d = document,
        e = d.documentElement,
        g = d.getElementsByTagName('body')[0],
        widthWindow = w.innerWidth || e.clientWidth || g.clientWidth,
        heightWindow = w.innerHeight || e.clientHeight || g.clientHeight;
    var width = widthWindow;
    var column = d.getElementById("center_column");

    document.getElementById('leftMenu').style.width = 200 + "px";
    document.getElementById('leftMenu').style.height = (heightWindow - 45) + "px";
    var left;
    if (widthWindow < 1000) {
        width = widthWindow * 0.98;
        if (width > 800)
            width = 800;
        left = (widthWindow - width) / 2;
        document.getElementById('topBar').style.left = '0';
        document.getElementById('leftMenu').style.top = '45px';
        if (widthWindow < 400)
            document.getElementById('leftMenu').style.width = (widthWindow * 0.8) + "px";
        MenuClosed();
    } else {
        width = widthWindow * 0.6;
        if (width < 800)
            width = 800;
        left = 200 + (widthWindow - 200 - width) / 2;

        document.getElementById('topBar').style.left = '200px';
        document.getElementById('leftMenu').style.top = '0';
        document.getElementById('leftMenu').style.left = '0';
        document.getElementById('leftMenu').style.height = heightWindow + 'px';
        MenuOpend();
    }
    column.style.width = width + "px";
    column.style.left = left + "px";
}

function MenuOpend() {
    if (IsMenuOpend) return;
    IsMenuOpend = true;
    document.getElementById('menuButton').style.left = "-100%";
    document.getElementById('title').style.margin = "0 0 0 20px";
    ActivateMenu();
}
function MenuClosed() {
    if (!IsMenuOpend) return;
    IsMenuOpend = false;
    document.getElementById('menuButton').style.left = "0";
    document.getElementById('title').style.margin = "0 0 0 65px";
    CloseMenu();
    IsMenuActivate = false;
}

//MENU
function Click(event) {
    event = event || window.event;
    if (IsMenuOpend) return;
    if (IsMenuActivate) {
            CloseMenu();
    } else {
        if (event.target) {
            if (event.target.id == 'menuButton')
                ActivateMenu();
        } else {
            if (event.srcElement.id == 'menuButton')
                ActivateMenu();
        }
    }
}
function ActivateMenu() {
    if (IsMenuOpend) return;
    document.getElementById('leftMenu').style.left = '0px';
    IsMenuActivate = true;
}
function CloseMenu() {
    document.getElementById('leftMenu').style.left = '-100%';
    IsMenuActivate = false;
}
