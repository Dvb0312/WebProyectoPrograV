<%@ Page Title="Reporte de Empleado" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Async="true" CodeBehind="EmpleadoReportes.aspx.cs" Inherits="WebProyectoPrograV.EmpleadoReportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .reporte-container {
            border: 1px solid #ccc;
            padding: 20px;
            width: 500px;
            margin: 20px auto;
            text-align: center;
            font-family: Arial, sans-serif;
        }
        
        .loading-message {
            font-size: 18px;
            color: #333;
            margin-bottom: 15px;
        }
        
        .description {
            color: #666;
            margin-bottom: 20px;
        }
        
        .btn-volver {
            padding: 8px 20px;
            background-color: #6c757d;
            color: white;
            border: 1px solid #5a6268;
            cursor: pointer;
            text-decoration: none;
            border-radius: 4px;
        }
        
        .btn-volver:hover {
            background-color: #5a6268;
        }
        
        .error-message {
            color: #dc3545;
            background-color: #f8d7da;
            border: 1px solid #f5c6cb;
            padding: 10px;
            border-radius: 4px;
            margin-bottom: 15px;
        }
    </style>

    <div class="reporte-container">
        <h2 class="loading-message">Generando reporte PDF...</h2>
        <p class="description">La descarga comenzará automáticamente.</p>
        
        <div style="margin-top: 20px;">
            <asp:Button ID="btnVolver" runat="server" 
                Text="Volver a Empleados" 
                CssClass="btn-volver"
                OnClick="btnVolver_Click" />
        </div>
    </div>
</asp:Content>