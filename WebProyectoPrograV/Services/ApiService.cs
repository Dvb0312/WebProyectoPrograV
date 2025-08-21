using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebProyectoPrograV.Models;

namespace WebProyectoPrograV.Services
{
    public class ApiService
    {
        private static readonly string BaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        private static HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        #region Empleados
        public static async Task<List<EmpleadoModel>> ObtenerEmpleadosAsync()
        {
            using (var client = CreateClient())
            {
                try
                {
                    var response = await client.GetAsync("api/Empleado");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<EmpleadoModel>>(json);
                    }
                    throw new ApplicationException($"Error al obtener empleados: {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al llamar a la API: " + ex.Message);
                }
            }
        }

        public static async Task<EmpleadoModel> ObtenerEmpleadoPorIdAsync(int id)
        {
            using (var client = CreateClient())
            {
                try
                {
                    var response = await client.GetAsync($"api/Empleado/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<EmpleadoModel>(json);
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al llamar a la API: " + ex.Message);
                }
            }
        }

        public static async Task<bool> CrearEmpleadoAsync(EmpleadoModel empleado)
        {
            using (var client = CreateClient())
            {
                try
                {
                    var json = JsonConvert.SerializeObject(empleado);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/Empleado", content);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al crear empleado: " + ex.Message);
                }
            }
        }

        public static async Task<bool> ActualizarEmpleadoAsync(int id, EmpleadoModel empleado)
        {
            using (var client = CreateClient())
            {
                try
                {
                    var json = JsonConvert.SerializeObject(empleado);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync($"api/Empleado/{id}", content);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al actualizar empleado: " + ex.Message);
                }
            }
        }

        public static async Task<bool> EliminarEmpleadoAsync(int id)
        {
            using (var client = CreateClient())
            {
                try
                {
                    var response = await client.DeleteAsync($"api/Empleado/{id}");
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al eliminar empleado: " + ex.Message);
                }
            }
        }
        #endregion

        #region Departamentos
        public static async Task<List<DepartamentoModel>> ObtenerDepartamentosAsync()
        {
            using (var client = CreateClient())
            {
                try
                {
                    var response = await client.GetAsync("api/Departamento");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<DepartamentoModel>>(json);
                    }
                    return new List<DepartamentoModel>();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al obtener departamentos: " + ex.Message);
                }
            }
        }
        #endregion

        #region Roles
        public static async Task<List<RolModel>> ObtenerRolesAsync()
        {
            using (var client = CreateClient())
            {
                try
                {
                    var response = await client.GetAsync("api/Rol");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<RolModel>>(json);
                    }
                    return new List<RolModel>();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al obtener roles: " + ex.Message);
                }
            }
        }
        #endregion

        #region Vacaciones
        public static async Task<List<VacacionModel>> ObtenerVacacionesAsync(int? idEmpleado = null)
        {
            using (var client = CreateClient())
            {
                try
                {
                    var url = "api/Vacacion";
                    if (idEmpleado.HasValue)
                        url += $"?idEmpleado={idEmpleado}";

                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<VacacionModel>>(json);
                    }
                    return new List<VacacionModel>();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al obtener vacaciones: " + ex.Message);
                }
            }
        }

        public static async Task<bool> CrearVacacionAsync(VacacionModel vacacion)
        {
            using (var client = CreateClient())
            {
                try
                {
                    var json = JsonConvert.SerializeObject(vacacion);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/Vacacion", content);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al crear vacación: " + ex.Message);
                }
            }
        }

        public static async Task<bool> ActualizarEstadoVacacionAsync(int id, byte estado, string observaciones = null)
        {
            using (var client = CreateClient())
            {
                try
                {
                    var url = $"api/Vacacion/{id}/estado?estado={estado}";
                    var content = new StringContent(JsonConvert.SerializeObject(observaciones), Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(url, content);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al actualizar estado de vacación: " + ex.Message);
                }
            }
        }
        #endregion

        #region Peticiones
        public static async Task<List<PeticionModel>> ObtenerPeticionesAsync(int? idEmpleado = null, string tipo = null, byte? estado = null)
        {
            using (var client = CreateClient())
            {
                try
                {
                    var url = "api/Peticion?";
                    var parameters = new List<string>();

                    if (idEmpleado.HasValue)
                        parameters.Add($"idEmpleado={idEmpleado}");
                    if (!string.IsNullOrEmpty(tipo))
                        parameters.Add($"tipo={tipo}");
                    if (estado.HasValue)
                        parameters.Add($"estado={estado}");

                    url += string.Join("&", parameters);

                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<PeticionModel>>(json);
                    }
                    return new List<PeticionModel>();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al obtener peticiones: " + ex.Message);
                }
            }
        }

        public static async Task<bool> CrearPeticionAsync(PeticionModel peticion)
        {
            using (var client = CreateClient())
            {
                try
                {
                    var json = JsonConvert.SerializeObject(peticion);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/Peticion", content);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al crear petición: " + ex.Message);
                }
            }
        }
        #endregion

        #region Autenticación
        public static async Task<LoginResponse> LoginAsync(string correo, string password)
        {
            using (var client = CreateClient())
            {
                try
                {
                    var loginRequest = new LoginRequest { Correo = correo, Password = password };
                    var json = JsonConvert.SerializeObject(loginRequest);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/Auth/login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<LoginResponse>(responseJson);
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al autenticar: " + ex.Message);
                }
            }
        }
        #endregion
    }
}