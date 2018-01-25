function MudarEstado() {
    var display = document.getElementById("DivTimeNaoCadastrado").style.display;

    document.getElementById("IDTIME2").value = 0;
    document.getElementById("TIMENAOCADASTRADO").value = "";

    if (display == "none") {
        document.getElementById("DivTimeNaoCadastrado").style.display = 'block';
        document.getElementById("DivTimeCadastrado").style.display = 'none';
    }
    else {
        document.getElementById("DivTimeNaoCadastrado").style.display = 'none';
        document.getElementById("DivTimeCadastrado").style.display = 'block';
    }
}

function MudarEstadoEdit() {

    var texto = document.getElementById("TIMENAOCADASTRADO").value;

    if (texto != "") {
        document.getElementById("DivTimeNaoCadastrado").style.display = 'block';
        document.getElementById("DivTimeCadastrado").style.display = 'none';
        document.getElementById("checkbox").checked = true;
        document.getElementById("IDTIME2").value = 0;
    }
    else {
        document.getElementById("DivTimeNaoCadastrado").style.display = 'none';
        document.getElementById("DivTimeCadastrado").style.display = 'block';
    }
}


function MudarEstadoDispensa() {
    //debugger;
    var display = document.getElementById("DivDispensa").style.display;

    if (display == "none") {
        document.getElementById("DATADISPENSA").value = "";
        document.getElementById("DivDispensa").style.display = 'block';
    }
    else {
        document.getElementById("DATADISPENSA").value = "1900-01-01";
        document.getElementById("DivDispensa").style.display = 'none';
    }
}

function MudarEstadoDispensaEdit() {
    var texto = document.getElementById("DATADISPENSA").value;

    if (texto != "1900-01-01") {
        document.getElementById("DivDispensa").style.display = 'block';
    }
    else {
        document.getElementById("DivDispensa").style.display = 'none';
        document.getElementById("DATADISPENSA").value = "1900-01-01";

    }
}

function DatetimePickerHora(value) {
    $(value).datetimepicker({
        format: 'HH:mm',
        showClose: false,
        showClear: false,
        toolbarPlacement: 'top',
        stepping: 10
    });
}

function verificaChecksCreateUser() {

    var senha = document.getElementById("SENHA").value;
    var rsenha = document.getElementById("RSENHA").value;

    if (senha == "") {
        alert("A senha é um campo obrigatorio.");
        return false;
    }

    if (rsenha == "") {
        alert("A repitição da senha é um campo obrigatorio.");
        return false;
    }

    if (senha != rsenha) {
        alert("As senhas não conferem.");
        return false;
    }

    var aChk = document.getElementsByName("TERMO");
    for (var i = 0; i < aChk.length; i++) {
        if (aChk[i].checked == true) {
            document.getElementById('CreateUser').submit();
        } else {
            alert("O termo de aceite deve está marcado.");
        }

    }

}

function verificaChecksReset() {

    var senha = document.getElementById("SENHA").value;
    var rsenha = document.getElementById("RSENHA").value;

    if (senha == "") {
        alert("A senha é um campo obrigatorio.");
        return false;
    }

    if (rsenha == "") {
        alert("A repitição da senha é um campo obrigatorio.");
        return false;
    }

    if (senha != rsenha) {
        alert("As senhas não conferem.");
        return false;
    } else {
        document.getElementById('ResetPasswordConfirm').submit();
    }
}

function verificaForgot() {

    var Email = document.getElementById("Email").value;
    var PERFIL = document.getElementById("PERFIL").value;

    debugger;
    if (PERFIL == "") {
        alert("Perfil é um campo obrigatorio.");
        return false;
    }

    if (Email == "") {
        alert("Email é um campo obrigatorio.");
        return false;
    }

    document.getElementById('ForgotPasswordConfirm').submit();

}

function verificaCreatePartida() {

    var IDCAMPO = document.getElementById("IDCAMPO").value;
    var IDInscrito1 = document.getElementById("IDInscrito1").value;
    var IDInscrito2 = document.getElementById("IDInscrito2").value;
    var iRodada = document.getElementById("iRodada").value;
    var iQntGols1 = document.getElementById("iQntGols1").value;
    var iQntGols2 = document.getElementById("iQntGols2").value;
    var dDataPartida = document.getElementById("dDataPartida").value;
    var sHoraPartida = document.getElementById("sHoraPartida").value;

    if (IDCAMPO == "") {
        alert("Local é um campo obrigatorio.");
        return false;
    }

    if (IDInscrito1 == "") {
        alert("Time 1 é um campo obrigatorio.");
        return false;
    }

    if (IDInscrito2 == "") {
        alert("Time 2 é um campo obrigatorio.");
        return false;
    }

    if (IDInscrito1 == IDInscrito2) {
        alert("Time 1 não pode ser igual ao time 2.");
        return false;
    }

    if (iRodada == "") {
        alert("Rodada é um campo obrigatorio.");
        return false;
    }

    if (iQntGols1 == "") {
        alert("Gols time 1 é um campo obrigatorio.");
        return false;
    }

    if (iQntGols2 == "") {
        alert("Gols time 2 é um campo obrigatorio.");
        return false;
    }

    if (dDataPartida == "") {
        alert("Data partida é um campo obrigatorio.");
        return false;
    }

    if (sHoraPartida == "") {
        alert("Hora partida é um campo obrigatorio.");
        return false;
    }

    document.getElementById('Create').submit();
}


function verificaEditPartida() {

    var IDCAMPO = document.getElementById("IDCAMPO").value;
    var iRodada = document.getElementById("iRodada").value;
    var iQntGols1 = document.getElementById("iQntGols1").value;
    var iQntGols2 = document.getElementById("iQntGols2").value;
    var dDataPartida = document.getElementById("dDataPartida").value;
    var sHoraPartida = document.getElementById("sHoraPartida").value;

    if (IDCAMPO == "") {
        alert("Local é um campo obrigatorio.");
        return false;
    }

    if (iRodada == "") {
        alert("Rodada é um campo obrigatorio.");
        return false;
    }

    if (iQntGols1 == "") {
        alert("Gols time 1 é um campo obrigatorio.");
        return false;
    }

    if (iQntGols2 == "") {
        alert("Gols time 2 é um campo obrigatorio.");
        return false;
    }

    if (dDataPartida == "") {
        alert("Data partida é um campo obrigatorio.");
        return false;
    }

    if (sHoraPartida == "") {
        alert("Hora partida é um campo obrigatorio.");
        return false;
    }

    document.getElementById('Edit').submit();
}