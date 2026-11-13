namespace HabitDisc.Services
{
    // Dados de um hábito na lista do dia (Home / UC02).
    // Não é uma tabela: é o resultado que o CheckinService monta para a tela.
    // A cor vai como texto HEX (igual ao banco); a tela converte com o CorHelper.
    public class HabitoDoDia
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Icone { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;

        // true = já tem check-in concluído hoje
        public bool Concluido { get; set; }

        // Dias seguidos até hoje (calculado, nunca gravado)
        public int Streak { get; set; }
    }
}
