using System.IO;
using System.Text.Json;

class Usuario{
    public string Cedula {get; set;} = string.Empty;
    public string Nombre {get; set;} = string.Empty;
    public string Apellido {get; set;} = string.Empty;
    public string Telefono {get; set;} = string.Empty;
    public string Correo {get; set;} = string.Empty;
    public string Clave {get; set;} = string.Empty;
}

class Incidencia{
    public string Pasaporte {get; set;} = string.Empty;
    public string Nombre {get; set;} = string.Empty;
    public string Apellido {get; set;} = string.Empty;
    public string WhatsApp {get; set;} = string.Empty;
    public string FechaNacimiento {get; set;} = string.Empty;
    public double Latitud {get; set;} = 0;
    public double Longitud {get; set;} = 0;
    public string CodigoAgente {get; set;} = string.Empty;
}

class DatosLogin{
    public string Cedula {get; set;} = string.Empty;
    public string Clave {get; set;} = string.Empty;
}

class ManejadorUsuario{

    public static ServerResult RegistroIncidencia(Incidencia incidencia){
        if(!Directory.Exists("incidencias")){
            Directory.CreateDirectory("incidencias");
        }

        var miid = Guid.NewGuid().ToString();

        var archivo = $"incidencia/{miid}.json";
        var json = JsonSerializer.Serialize(incidencia);
        File.WriteAllText(archivo, json);

        return new ServerResult(true, "Incidencia Registrada", miid);
    }

    public static ServerResult IniciarSesion(DatosLogin datos){

        var cedula = datos.Cedula;
        var clave = datos.Clave;
        
        if (cedula.Length != 11){
            return new ServerResult(false, "La cédula debe tener 11 dígitos");
        }

        if (clave.Length == 0){
            return new ServerResult(false, "La clave es obligatoria");
        }

        var archivo = $"usuarios/{cedula}.json";

        if (!File.Exists(archivo)){
            return new ServerResult(false, "El usuario no existe");
        }

        var json = File.ReadAllText(archivo);
        var usuario = JsonSerializer.Deserialize<Usuario>(json);

        if (usuario == null) {
            return new ServerResult(false, "Error al cargar el usuario");
        }

        if (usuario.Clave != clave){
            return new ServerResult(false, "Clave incorrecta");
        }

        // Retornar un resultado exitoso si todo es correcto
        return new ServerResult(true, "Inicio de sesión exitoso");
    }

    public static ServerResult Registro(Usuario usuario){
        
        List<string> errores = new List<string>();

        if (usuario.Cedula.Length != 11){
            errores.Add("La cédula debe tener 11 dígitos");
        }

        if (errores.Count > 0){
            Console.WriteLine("Errores en el registro:");
            foreach(var error in errores){
                Console.WriteLine(error);
            }
            return new ServerResult(false, "Errores en el registro", errores);
        }

        if(!Directory.Exists("usuarios")){
            Directory.CreateDirectory("usuarios");
        }

        var archivo = $"usuarios/{usuario.Cedula}.json";

        if(File.Exists(archivo)){
            return new ServerResult(false, "El usuario ya existe");
        }

        var json = JsonSerializer.Serialize(usuario);

        File.WriteAllText(archivo, json);

        return new ServerResult(true, "Usuario registrado exitosamente");
    }
}
