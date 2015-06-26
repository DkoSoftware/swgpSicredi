//adiciona função para o início da requisição 
Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(InicioReq);
//adiciona função para o final da requisição 
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(FimReq);

function InicioReq(sender, args) {
    // Exibe o popup com mensagem para aguardar 
    $find(ModalProgress).show();
}
function FimReq(sender, args) {

    //Esconde o popup 
    $find(ModalProgress).hide();
} 

