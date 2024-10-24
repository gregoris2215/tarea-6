class ServerResult{
    public bool Exito {get; set;}
    public string Mensaje {get; set;}
    public object Datos {get; set;}

    public ServerResult(bool exito, string mensaje, object datos){
        Exito = exito;
        Mensaje = mensaje;
        Datos = datos;
    }
    
    public ServerResult(bool exito, string mensaje)
    {
        Exito = exito;
        Mensaje = mensaje;
    }

    public ServerResult(bool exito)
    {
        Exito = exito;
    }

    public ServerResult()
    {
        Exito = true;
    }

}

