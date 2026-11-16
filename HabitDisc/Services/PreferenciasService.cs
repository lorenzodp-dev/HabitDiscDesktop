using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HabitDisc.Services
{
    // Lê e grava as preferências de aparência em:
    //   %AppData%\HabitDisc\preferencias.json
    // (ex.: C:\Users\<usuário>\AppData\Roaming\HabitDisc\preferencias.json)
    public class PreferenciasService
    {
        private static readonly string Pasta = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HabitDisc");

        private static readonly string Arquivo = Path.Combine(Pasta, "preferencias.json");

        // Enum gravado como texto ("Grande"), e não como número, para o JSON ficar legível
        private static readonly JsonSerializerOptions Opcoes = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        // Nunca quebra a abertura do app: se o arquivo não existir ou estiver
        // corrompido, devolve as preferências padrão.
        public Preferencias Carregar()
        {
            try
            {
                if (!File.Exists(Arquivo))
                {
                    return new Preferencias();
                }

                string json = File.ReadAllText(Arquivo);
                return JsonSerializer.Deserialize<Preferencias>(json, Opcoes) ?? new Preferencias();
            }
            catch (Exception ex) when (ex is IOException || ex is JsonException || ex is UnauthorizedAccessException)
            {
                return new Preferencias();
            }
        }

        // Pode lançar IOException / UnauthorizedAccessException: quem chama decide o que mostrar
        public void Salvar(Preferencias preferencias)
        {
            Directory.CreateDirectory(Pasta);
            File.WriteAllText(Arquivo, JsonSerializer.Serialize(preferencias, Opcoes));
        }
    }
}
