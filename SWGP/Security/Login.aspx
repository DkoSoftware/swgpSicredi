<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SWGP.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FGP -|- Ferramenta de Gestão da Prospecção</title>
    <link href="../Styles/DkoFormView.css" rel="stylesheet" type="text/css" />
    <script src="../messagebox/messagebox.js" type="text/javascript"></script>
     

      <style type="text/css">
         body 
        {
            background-image: url(../images/fundo_sicredi2.png);
            background-repeat:repeat-x;
        }

        .img_login
        {
            position:absolute;
            top:33%;
            left:33%;
   
            }

     
         #FormLogin table
         { 
              position:absolute;
              top:47%;
              left :40%;
              width:110px;
              z-index:1012;
           
             }
     
        .chb_Manterme
        {
            font-size:12px;
            }
            
                /* Button Entrar
            -------------------------------------------------------------------------*/
             input[type=button].DkoButtonSave, input[type=submit].DkoButtonSave  
             {  
                padding: 0 25px 0 40px;  
                height: 30px;  
               border: 1px solid #999999;  
                min-width: 100px;
                background-image: url(../images/Button/img_save.png);
                background-repeat:no-repeat;
                background-color:#E8E8E8;
                color:Green;
                font-weight:bold;
                font-size:12px;
      
             }  

            input[type=button].DkoButtonSave:hover, input[type=submit].DkoButtonSave:hover  
            {  
                background-color:#C5C5C5;
                color:#FFFFFF;
                border-bottom-color:White;
                border-right-color:White;
            }  


    </style>

     <!--[if IE 7]>
         <style type="text/css">
         body 
        {
            background-image: url(../images/fundo_sicredi2.png);
            background-repeat:repeat-x;
        }

        .img_login
        {
            position:absolute;
            top:33%;
            left:33%;
   
            }

     
         #FormLogin table
         { 
              position:absolute;
              top:47%;
              left :40%;
              width:110px;
              z-index:1012;
           
             }
     
        .chb_Manterme
        {
            font-size:12px;
            }
            
                /* Button Entrar
            -------------------------------------------------------------------------*/
             input[type=button].DkoButtonSave, input[type=submit].DkoButtonSave  
             {  
                padding: 0 25px 0 40px;  
                height: 30px;  
               border: 1px solid #999999;  
                min-width: 100px;
                background-image: url(~/images/Button/img_save.png);
                background-repeat:no-repeat;
                background-color:#E8E8E8;
                color:Green;
                font-weight:bold;
                font-size:12px;
      
             }  

            input[type=button].DkoButtonSave:hover, input[type=submit].DkoButtonSave:hover  
            {  
                background-color:#C5C5C5;
                color:#FFFFFF;
                border-bottom-color:White;
                border-right-color:White;
            }  


    </style>
     <![endif]-->

     <!--[if gte IE 8]>
         <style type="text/css">
         body 
        {
            background-image: url(../images/fundo_sicredi2.png);
            background-repeat:repeat-x;
        }

        .img_login
        {
            position:absolute;
            top:33%;
            left:33%;
   
            }

     
         #FormLogin table
         { 
              position:absolute;
              top:47%;
              left :40%;
              width:110px;
              z-index:1012;
           
             }
     
        .chb_Manterme
        {
            font-size:12px;
            }
            
                /* Button Entrar
            -------------------------------------------------------------------------*/
             input[type=button].DkoButtonSave, input[type=submit].DkoButtonSave  
             {  
                padding: 0 25px 0 40px;  
                height: 30px;  
               border: 1px solid #999999;  
                min-width: 40px;
                background-image: url(images/Button/img_save.png);
                background-repeat:no-repeat;
                background-color:#E8E8E8;
                color:Green;
                font-weight:bold;
                font-size:12px;
      
             }  

            input[type=button].DkoButtonSave:hover, input[type=submit].DkoButtonSave:hover  
            {  
                background-color:#C5C5C5;
                color:#FFFFFF;
                border-bottom-color:White;
                border-right-color:White;
            }  


    </style>
     <![endif]-->
     
</head>
<body>
    <form id="frmLogin" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
      <asp:UpdatePanel>
        <ContentTemplate>
                <div class="img_login">
                    <img src="../Images/img_loginSWGP.png" />
                </div>
                 <div id="FormLogin">
                     <table >
                         <tr >
                             <td >
                                 <asp:Label Text="Usuário" id="lblUsername" runat="server" />
                             </td>
                             <td nowrap="nowrap">
                                 <asp:TextBox runat="server" id="txtUsername"  /> 
                                 <asp:RequiredFieldValidator  ControlToValidate="txtUsername" Text="*" ForeColor="Red"
                                     runat="server" />
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 <asp:Label Text="Senha" id="lblSenha" runat="server" />
                             </td>
                             <td nowrap="nowrap">
                                 <asp:TextBox runat="server" id="txtPassword" TextMode="Password" Width="92%"/>
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  ControlToValidate="txtPassword" Text="*" ForeColor="Red"
                                     runat="server" /> 
                             </td>
                         </tr>
                         <tr >
                             <td>
                             <td>
                                 <asp:CheckBox Text="Manter-me conectado" runat="server" id="chbManterConectado" CssClass="chb_Manterme" Visible="false"/>
                             </td>
                         </tr>
                         <tr>
                             <td >
                             </td>
                             <td >
                             </td>
                             <td >
                                 <asp:Button Text="Entrar" runat="server" id="btnEntrar" OnClick="btnEntrar_Click" CssClass="DkoButtonSave"/>
                             </td>
                         </tr>
                     </table>
                 </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
