<%@ Page Title="Empleados" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"Async="true" CodeBehind="Empleados.aspx.cs" Inherits="WebProyectoPrograV.Empleados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h1>Gestión de Empleados</h1>
        <p>Administre la información de todos los empleados</p>
    </div>

    <div class="content-wrapper p-3">
        <!-- Acciones principales -->
        <div class="mb-3">
            <asp:Button ID="btnNuevoEmpleado" runat="server" Text="Nuevo Empleado" 
                CssClass="btn btn-success" OnClick="btnNuevoEmpleado_Click" />
            <asp:Button ID="btnActualizar" runat="server" Text="Actualizar Lista" 
                CssClass="btn btn-primary" OnClick="btnActualizar_Click" />
        </div>

        <!-- Filtros -->
        <div style="background-color: #f8f9fa; padding: 15px; border-radius: 4px; margin-bottom: 20px;">
            <div style="display: grid; grid-template-columns: 1fr 1fr 1fr auto; gap: 15px; align-items: end;">
                <div class="form-group">
                    <label>Departamento:</label>
                    <asp:DropDownList ID="ddlDepartamento" runat="server" CssClass="form-control">
                        <asp:ListItem Value="" Text="Todos los departamentos"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label>Estado:</label>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control">
                        <asp:ListItem Value="" Text="Todos"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Activos"></asp:ListItem>
                        <asp:ListItem Value="0" Text="Inactivos"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label>Buscar por nombre:</label>
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" 
                        placeholder="Nombre o apellido..."></asp:TextBox>
                </div>
                
                <div>
                    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" 
                        CssClass="btn btn-primary" OnClick="btnFiltrar_Click" />
                </div>
            </div>
        </div>

        <!-- Grid de empleados -->
        <asp:GridView ID="gvEmpleados" runat="server" AutoGenerateColumns="false" 
            CssClass="gridview" EmptyDataText="No se encontraron empleados"
            OnRowCommand="gvEmpleados_RowCommand" DataKeyNames="IdEmpleado">
            <Columns>
                <asp:BoundField DataField="IdEmpleado" HeaderText="ID" />
                <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre Completo" />
                <asp:BoundField DataField="Puesto" HeaderText="Puesto" />
                <asp:BoundField DataField="CorreoElectronico" HeaderText="Email" />
                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                <asp:BoundField DataField="FechaContratacion" HeaderText="Fecha Contratación" 
                    DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Salario" HeaderText="Salario" 
                    DataFormatString="{0:C}" />
                <asp:BoundField DataField="EstadoTexto" HeaderText="Estado" />
                
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:Button ID="btnEditar" runat="server" Text="Editar" 
                            CssClass="btn btn-primary" 
                            CommandName="Editar" 
                            CommandArgument='<%# Eval("IdEmpleado") %>' />
                        
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" 
                            CssClass="btn btn-danger" 
                            CommandName="Eliminar" 
                            CommandArgument='<%# Eval("IdEmpleado") %>'
                            OnClientClick="return confirm('¿Está seguro de eliminar este empleado?');" />
                        
                        <asp:Button ID="btnReportes" runat="server" Text="Reportes"
                            CssClass="btn btn-info"
                            CommandName="Reportes"
                            CommandArgument='<%# Eval("IdEmpleado") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <!-- Información adicional -->
        <div class="mt-3">
            <p><strong>Total de empleados: </strong><asp:Label ID="lblTotalRegistros" runat="server" Text="0"></asp:Label></p>
        </div>

        <!-- Mensajes -->
        <asp:Label ID="lblMensaje" runat="server" Visible="false"></asp:Label>
    </div>
</asp:Content>