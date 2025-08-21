<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Login.aspx.cs" Inherits="WebProyectoPrograV.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sistema de Empleados - Login</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }
        .login-container {
            background-color: white;
            padding: 40px;
            border-radius: 8px;
            box-shadow: 0 0 20px rgba(0,0,0,0.1);
            width: 400px;
        }
        .login-header {
            text-align: center;
            margin-bottom: 30px;
            color: #333;
        }
        .form-group {
            margin-bottom: 20px;
        }
        .form-group label {
            display: block;
            margin-bottom: 5px;
            color: #555;
            font-weight: bold;
        }
        .form-group input {
            width: 100%;
            padding: 12px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 14px;
            box-sizing: border-box;
        }
        .btn-login {
            width: 100%;
            padding: 12px;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 4px;
            font-size: 16px;
            cursor: pointer;
        }
        .btn-login:hover {
            background-color: #0056b3;
        }
        .error-message {
            color: #dc3545;
            margin-top: 10px;
            text-align: center;
        }
        .info-panel {
            margin-top: 20px;
            padding: 15px;
            background-color: #e9ecef;
            border-radius: 4px;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="login-header">
                <h2>HR Manager</h2>
                <p>Ingrese sus credenciales para acceder</p>
            </div>
            
            <div class="form-group">
                <label for="txtCorreo">Correo Electrónico:</label>
                <asp:TextBox ID="txtCorreo" runat="server" TextMode="Email" placeholder="ejemplo@empresa.com"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCorreo" runat="server" 
                    ControlToValidate="txtCorreo" 
                    ErrorMessage="El correo es obligatorio" 
                    CssClass="error-message" Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>
            
            <div class="form-group">
                <label for="txtPassword">Contraseña:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Ingrese su contraseña"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                    ControlToValidate="txtPassword" 
                    ErrorMessage="La contraseña es obligatoria" 
                    CssClass="error-message" Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>
            
            <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" 
                OnClick="btnLogin_Click" CssClass="btn-login" />
            
            <asp:Label ID="lblMensaje" runat="server" CssClass="error-message" Visible="false"></asp:Label>
            
            <div class="info-panel">
                <strong>Usuarios de prueba:</strong><br />
                <strong>Administrador:</strong> admin@empresa.com / admin123<br />
                <strong>Empleado:</strong> maria.gonzalez@empresa.com / empleado123
            </div>
        </div>
    </form>
</body>
</html>