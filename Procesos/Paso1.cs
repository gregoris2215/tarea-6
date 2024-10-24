using System.Text.Json;
using System.Text.RegularExpressions;

public class Excerpt
    {
        public string rendered { get; set; }
        public bool @protected { get; set; }
    }

    public class Notice
    {
        public Title title { get; set; }
        public Excerpt excerpt { get; set; }
    }

    public class Title
    {
        public string rendered { get; set; }
    }

class Paso1{
    public static async Task<ServerResult> Ejecutar(){
        try{
        var url = "https://remolacha.net/wp-json/wp/v2/posts?search=migraci%C3%B3n&_fields=title,excerpt";
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var noticias = JsonSerializer.Deserialize<List<Notice>>(json);

        var finai = new List<Dictionary<string, string>>();

        foreach(var noticia in noticias){
            var titulo = noticia.title.rendered;   
            var resumen = noticia.title.rendered;
            resumen = Regex.Replace(resumen,"<.*?>", String.Empty);

            var dic = new Dictionary<string, string>();
            dic.Add("titulo", titulo);
            dic.Add("resumen", resumen);
            finai.Add(dic);
        }

        return new ServerResult(true, "Noticias cargadas", finai);
        }
        catch(Exception ex)
        {
            return new ServerResult(false, ex.Message);
        }
    }
}