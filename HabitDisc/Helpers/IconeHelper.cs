using System.Collections.Generic;
using FontAwesome.Sharp;

namespace HabitDisc.Helpers
{
    // Converte o nome do ícone guardado no banco (ex.: "heart") para o ícone do
    // FontAwesome.Sharp (IconChar.Heart).
    // O banco guarda o nome em texto, como no FontAwesome da web, e não o número
    // do enum: assim os dados não dependem da biblioteca usada na tela.
    public static class IconeHelper
    {
        private static readonly Dictionary<string, IconChar> _icones = new Dictionary<string, IconChar>
        {
            ["heart"] = IconChar.Heart,
            ["circle-check"] = IconChar.CircleCheck,
            ["book"] = IconChar.Book,
            ["users"] = IconChar.Users,
            ["wallet"] = IconChar.Wallet,
            ["dumbbell"] = IconChar.Dumbbell,
            ["person-running"] = IconChar.PersonRunning,
            ["droplet"] = IconChar.Droplet,
            ["apple-whole"] = IconChar.AppleWhole,
            ["bed"] = IconChar.Bed,
            ["brain"] = IconChar.Brain,
            ["graduation-cap"] = IconChar.GraduationCap,
            ["code"] = IconChar.Code,
            ["briefcase"] = IconChar.Briefcase,
            ["pen"] = IconChar.Pen,
            ["music"] = IconChar.Music,
            ["sun"] = IconChar.Sun,
            ["moon"] = IconChar.Moon,
            ["leaf"] = IconChar.Leaf,
            ["piggy-bank"] = IconChar.PiggyBank,
            ["phone"] = IconChar.Phone,
            ["star"] = IconChar.Star,
            ["fire"] = IconChar.Fire,
            ["house"] = IconChar.House,

            // Nome antigo (FontAwesome 5) usado pelo script do banco na categoria
            // Produtividade. Continua funcionando, mas não aparece para escolha.
            ["check-circle"] = IconChar.CircleCheck,
        };

        // Ícones oferecidos para escolha, na ordem em que aparecem
        public static readonly IReadOnlyList<string> Disponiveis = new List<string>
        {
            "heart", "circle-check", "book", "users", "wallet", "dumbbell",
            "person-running", "droplet", "apple-whole", "bed", "brain", "graduation-cap",
            "code", "briefcase", "pen", "music", "sun", "moon",
            "leaf", "piggy-bank", "phone", "star", "fire", "house",
        };

        // Nome desconhecido vira um círculo, em vez de quebrar a tela
        public static IconChar ParaIconChar(string? nome)
        {
            if (nome != null && _icones.TryGetValue(nome, out IconChar icone))
            {
                return icone;
            }
            return IconChar.Circle;
        }
    }
}
