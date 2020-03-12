var StandardMasterCallback;
var StandardMasterTheme;
var winW, winH;


//window.onresize = resizePage;

function resizePage() {
    if (self.innerWidth) {
        winW = self.innerWidth;
        winH = self.innerHeight;
    } else if (document.documentElement && document.documentElement.clientWidth) {
        winW = document.documentElement.clientWidth;
        winH = document.documentElement.clientHeight;
    } else if (document.body) {
        winW = document.body.clientWidth;
        winH = document.body.clientHeight;
    }
    var h;
    //h = document.forms[0].namedItem(HwinHId);
    h = document.getElementById(HwinHId);
    h.value = winH
    var w;
    //w = document.forms[0].namedItem(HwinWId);
    w = document.getElementById(HwinWId);
    w.value = winW
}


function StandardMasterImpostaCampi(s, nome) {
    switch (nome) {
        case 'StandardMasterCallback':
            StandardMasterCallback = s;
            break;
        case 'StandardMasterTheme':
            StandardMasterTheme = s;
            break;
        default:
            break;
    }
}
function StandardMasterCambioTheme() {
    StandardMasterCallback.PerformCallback(StandardMasterTheme.GetValue());
}
function StandardMasterImpostaHW() {
    resizePage();
    //StandardMasterCallback.PerformCallback('H=' + document.forms[0].namedItem(HwinHId).value + ';W=' + document.forms[0].namedItem(HwinWId).value);
    StandardMasterCallback.PerformCallback('H=' + document.getElementById(HwinHId).value + ';W=' + document.getElementById(HwinWId).value);
}
function StandardMasterCallbackComplete(s, e) {
    if (e.parameter != 'TIMEOUT') {
        document.forms[0].submit();
    }
}

function MenuClick(s, e) {
    if (e.item.name = 'Esci') {

    }
}

function TimeOutSessione() {
    StandardMasterCallback.PerformCallback('TIMEOUT');
}

/*
function PopUpInsTelefonate(pagpath) {
    window.open(pagpath, 'Inserimento', 'width=910px,height=500px,resizable=0,top=150,left=100');
}
*/

function PopUpInsTelefonate(pagpath) {
    var w = 910;    
    var h = 500;
    var l = Math.floor((screen.width - w) / 2);
    var t = Math.floor((screen.height - h) / 2);
    window.open(pagpath, 'Inserimento', 'width=' + w + ',height=' + h + ',top=' + t + ',left=' + l + ',resizable=yes');
}
