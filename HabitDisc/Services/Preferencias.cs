namespace HabitDisc.Services
{
    public enum TamanhoFonte
    {
        Pequena,
        Media,
        Grande
    }

    // Preferências de aparência do usuário (tela Configurações).
    // Não é tabela do banco: fica num arquivo JSON local (ver PreferenciasService),
    // pois é configuração da instalação e não dado de hábito.
    public class Preferencias
    {
        // Nome de uma das paletas de Tema.Paletas (ex.: "Verde")
        public string Paleta { get; set; } = "Verde";

        public TamanhoFonte TamanhoFonte { get; set; } = TamanhoFonte.Media;

        public bool ModoEscuro { get; set; }

        public Preferencias Copiar()
        {
            return new Preferencias
            {
                Paleta = Paleta,
                TamanhoFonte = TamanhoFonte,
                ModoEscuro = ModoEscuro
            };
        }
    }
}
