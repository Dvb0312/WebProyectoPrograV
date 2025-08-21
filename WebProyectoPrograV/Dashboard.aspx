<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Async="true" CodeBehind="Dashboard.aspx.cs" Inherits="WebProyectoPrograV.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h1>Dashboard</h1>
        <p>Resumen del sistema de empleados</p>
    </div>

    <div class="content-wrapper p-3">
        <!-- Estadísticas rápidas -->
        <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 20px; margin-bottom: 30px;">
            <div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 20px; border-radius: 8px; text-align: center;">
                <h3 style="margin: 0; font-size: 2rem;">
                    <asp:Label ID="lblTotalEmpleados" runat="server" Text="0"></asp:Label>
                </h3>
                <p style="margin: 5px 0 0 0; opacity: 0.9;">Total Empleados</p>
            </div>
            
            <div style="background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); color: white; padding: 20px; border-radius: 8px; text-align: center;">
                <h3 style="margin: 0; font-size: 2rem;">
                    <asp:Label ID="lblVacacionesPendientes" runat="server" Text="0"></asp:Label>
                </h3>
                <p style="margin: 5px 0 0 0; opacity: 0.9;">Vacaciones Pendientes</p>
            </div>
            
            <div style="background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%); color: white; padding: 20px; border-radius: 8px; text-align: center;">
                <h3 style="margin: 0; font-size: 2rem;">
                    <asp:Label ID="lblPeticionesPendientes" runat="server" Text="0"></asp:Label>
                </h3>
                <p style="margin: 5px 0 0 0; opacity: 0.9;">Peticiones Pendientes</p>
            </div>
            
            <div style="background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%); color: white; padding: 20px; border-radius: 8px; text-align: center;">
                <h3 style="margin: 0; font-size: 2rem;">
                    <asp:Label ID="lblDepartamentos" runat="server" Text="0"></asp:Label>
                </h3>
                <p style="margin: 5px 0 0 0; opacity: 0.9;">Departamentos</p>
            </div>
        </div>

        <!-- Acciones rápidas -->
        <div class="mb-3">
            <h3>Acciones Rápidas</h3>
            <div style="display: flex; gap: 10px; flex-wrap: wrap; margin-top: 15px;">
                <asp:Button ID="btnVerEmpleados" runat="server" Text="Ver Empleados" CssClass="btn btn-primary" 
                    OnClick="btnVerEmpleados_Click" />
                <asp:Button ID="btnNuevoEmpleado" runat="server" Text="Nuevo Empleado" CssClass="btn btn-success" 
                    OnClick="btnNuevoEmpleado_Click" />
                <asp:Button ID="btnVerVacaciones" runat="server" Text="Ver Vacaciones" CssClass="btn btn-warning" 
                    OnClick="btnVerVacaciones_Click" />
                <asp:Button ID="btnVerPeticiones" runat="server" Text="Ver Peticiones" CssClass="btn btn-primary" 
                    OnClick="btnVerPeticiones_Click" />
            </div>
        </div>

        <!-- Últimos empleados registrados -->
        <div class="mb-3">
            <h3>Últimos Empleados Registrados</h3>
            <asp:GridView ID="gvUltimosEmpleados" runat="server" AutoGenerateColumns="false" 
                CssClass="gridview" EmptyDataText="No hay empleados registrados">
                <Columns>
                    <asp:BoundField DataField="IdEmpleado" HeaderText="ID" />
                    <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre Completo" />
                    <asp:BoundField DataField="Puesto" HeaderText="Puesto" />
                    <asp:BoundField DataField="FechaContratacion" HeaderText="Fecha Contratación" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="EstadoTexto" HeaderText="Estado" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- Mensajes -->
        <asp:Label ID="lblMensaje" runat="server" Visible="false"></asp:Label>
        
        <!-- Botón para recargar datos -->
        <div class="text-center mt-3">
            <asp:Button ID="btnActualizar" runat="server" Text="Actualizar Dashboard" 
                CssClass="btn btn-primary" OnClick="btnActualizar_Click" />
        </div>
    </div>
</asp:Content>