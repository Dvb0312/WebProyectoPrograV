
<%@ Page Title="Empleado" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Async="true" CodeBehind="EmpleadoForm.aspx.cs" Inherits="WebProyectoPrograV.EmpleadoForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h1><asp:Label ID="lblTitulo" runat="server" Text="Nuevo Empleado"></asp:Label></h1>
        <p>Complete la información del empleado</p>
    </div>

    <div class="content-wrapper p-3">
        <!-- Mensajes -->
        <asp:Label ID="lblMensaje" runat="server" Visible="false"></asp:Label>

        <!-- Formulario -->
        <div class="card">
            <div class="card-body">
                <div class="row">
                    <!-- Información Personal -->
                    <div class="col-md-6">
                        <h5>Información Personal</h5>
                        
                        <div class="form-group mb-3">
                            <label>Nombre <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" 
                                placeholder="Ingrese el nombre" MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                ControlToValidate="txtNombre" 
                                ErrorMessage="El nombre es requerido" 
                                CssClass="text-danger" ValidationGroup="EmpleadoGroup"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group mb-3">
                            <label>Apellido <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" 
                                placeholder="Ingrese el apellido" MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvApellido" runat="server" 
                                ControlToValidate="txtApellido" 
                                ErrorMessage="El apellido es requerido" 
                                CssClass="text-danger" ValidationGroup="EmpleadoGroup"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group mb-3">
                            <label>Correo Electrónico</label>
                            <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" 
                                placeholder="correo@empresa.com" MaxLength="150" TextMode="Email"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revCorreo" runat="server" 
                                ControlToValidate="txtCorreo" 
                                ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" 
                                ErrorMessage="Formato de correo inválido" 
                                CssClass="text-danger" ValidationGroup="EmpleadoGroup"></asp:RegularExpressionValidator>
                        </div>

                        <div class="form-group mb-3">
                            <label>Teléfono</label>
                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" 
                                placeholder="8888-1234" MaxLength="20"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Información Laboral -->
                    <div class="col-md-6">
                        <h5>Información Laboral</h5>
                        
                        <div class="form-group mb-3">
                            <label>Puesto</label>
                            <asp:TextBox ID="txtPuesto" runat="server" CssClass="form-control" 
                                placeholder="Cargo o posición" MaxLength="100"></asp:TextBox>
                        </div>

                        <div class="form-group mb-3">
                            <label>Fecha de Contratación <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtFechaContratacion" runat="server" CssClass="form-control" 
                                TextMode="Date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFecha" runat="server" 
                                ControlToValidate="txtFechaContratacion" 
                                ErrorMessage="La fecha de contratación es requerida" 
                                CssClass="text-danger" ValidationGroup="EmpleadoGroup"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group mb-3">
                            <label>Salario <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtSalario" runat="server" CssClass="form-control" 
                                placeholder="0.00" TextMode="Number" step="0.01"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvSalario" runat="server" 
                                ControlToValidate="txtSalario" 
                                ErrorMessage="El salario es requerido" 
                                CssClass="text-danger" ValidationGroup="EmpleadoGroup"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvSalario" runat="server" 
                                ControlToValidate="txtSalario" 
                                MinimumValue="0" MaximumValue="999999999" Type="Double"
                                ErrorMessage="Salario debe ser mayor a 0" 
                                CssClass="text-danger" ValidationGroup="EmpleadoGroup"></asp:RangeValidator>
                        </div>

                        <div class="form-group mb-3">
                            <label>Departamento <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlDepartamento" runat="server" CssClass="form-control">
                                <asp:ListItem Value="" Text="-- Seleccione departamento --"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvDepartamento" runat="server" 
                                ControlToValidate="ddlDepartamento" 
                                ErrorMessage="Debe seleccionar un departamento" 
                                CssClass="text-danger" ValidationGroup="EmpleadoGroup"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group mb-3">
                            <label>Rol <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control">
                                <asp:ListItem Value="" Text="-- Seleccione rol --"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvRol" runat="server" 
                                ControlToValidate="ddlRol" 
                                ErrorMessage="Debe seleccionar un rol" 
                                CssClass="text-danger" ValidationGroup="EmpleadoGroup"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group mb-3">
                            <label>Estado</label>
                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control">
                                <asp:ListItem Value="1" Text="Activo" Selected="true"></asp:ListItem>
                                <asp:ListItem Value="0" Text="Inactivo"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Botones -->
        <div class="mt-3">
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" 
                CssClass="btn btn-success" OnClick="btnGuardar_Click" 
                ValidationGroup="EmpleadoGroup" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                CssClass="btn btn-secondary" OnClick="btnCancelar_Click" 
                CausesValidation="false" />
            <asp:Button ID="btnVolver" runat="server" Text="Volver a Lista" 
                CssClass="btn btn-primary" OnClick="btnVolver_Click" 
                CausesValidation="false" />
        </div>
    </div>
</asp:Content>
