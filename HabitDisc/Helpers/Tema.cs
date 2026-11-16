using System;
using System.Collections.Generic;
using System.Drawing;
using HabitDisc.Services;

namespace HabitDisc.Helpers
{
    // Cores e fontes ATUAIS do sistema. As telas leem daqui em vez de usar cores fixas.
    //
    // Como funciona:
    // 1. Ao abrir o app, o Program chama Tema.Aplicar(preferências salvas).
    // 2. Cada tela tem um método AplicarTema() que pinta seus controles com as cores daqui,
    //    e se inscreve no evento Tema.Alterado.
    // 3. Quando o usuário muda algo em Configurações, Tema.Aplicar() troca os valores e
    //    dispara Alterado: todas as telas se repintam na hora.
    public static class Tema
    {
        public const string FonteTexto = "Newsreader";
        public const string FonteMenu = "Microsoft Sans Serif";

        // Paletas disponíveis. A primeira é a padrão (o verde original do HabitDisc).
        public static readonly IReadOnlyList<Paleta> Paletas = new List<Paleta>
        {
            new Paleta("Verde",   Color.FromArgb(35, 77, 51),  Color.FromArgb(47, 107, 69),  Color.FromArgb(111, 196, 116)),
            new Paleta("Azul",    Color.FromArgb(30, 58, 95),  Color.FromArgb(44, 82, 130),  Color.FromArgb(100, 170, 230)),
            new Paleta("Roxo",    Color.FromArgb(64, 40, 92),  Color.FromArgb(88, 58, 126),  Color.FromArgb(180, 140, 230)),
            new Paleta("Vinho",   Color.FromArgb(105, 38, 42), Color.FromArgb(138, 56, 58),  Color.FromArgb(240, 140, 120)),
            new Paleta("Grafite", Color.FromArgb(45, 48, 52),  Color.FromArgb(66, 70, 76),   Color.FromArgb(240, 175, 80)),
        };

        // ---------- Valores atuais ----------

        public static Preferencias Atual { get; private set; } = new Preferencias();

        // Barra lateral e cartão de progresso
        public static Color Menu { get; private set; }
        // Botão do menu selecionado / passando o mouse; fundo da barra de progresso
        public static Color MenuSelecionado { get; private set; }
        // Cor de realce: percentual e barra de progresso preenchida
        public static Color Destaque { get; private set; }
        // Texto sobre Menu (sempre claro)
        public static Color TextoSobreMenu { get; private set; } = Color.White;

        // Fundo das telas e das "superfícies" (linhas de hábito, cartões)
        public static Color Fundo { get; private set; }
        public static Color Superficie { get; private set; }

        // Títulos, texto comum e texto apagado (ex.: hábito concluído)
        public static Color Titulo { get; private set; }
        public static Color Texto { get; private set; }
        public static Color TextoSuave { get; private set; }

        // 1.0 = tamanho original; multiplica fontes e alturas
        public static float Escala { get; private set; } = 1f;

        public static event EventHandler? Alterado;

        private static readonly Dictionary<string, Font> _fontes = new Dictionary<string, Font>();

        static Tema()
        {
            // Valores padrão, para nada ficar sem cor antes do Program aplicar as preferências
            Definir(new Preferencias());
        }

        // ---------- Aplicação ----------

        public static void Aplicar(Preferencias preferencias)
        {
            Definir(preferencias);
            Alterado?.Invoke(null, EventArgs.Empty);
        }

        private static void Definir(Preferencias preferencias)
        {
            Atual = preferencias.Copiar();

            Paleta paleta = BuscarPaleta(preferencias.Paleta);
            Menu = paleta.Menu;
            MenuSelecionado = paleta.MenuSelecionado;
            Destaque = paleta.Destaque;

            if (preferencias.ModoEscuro)
            {
                Fundo = Color.FromArgb(32, 34, 37);
                Superficie = Color.FromArgb(45, 48, 52);
                // No escuro, a cor do Menu ficaria ilegível nos títulos: usa o Destaque
                Titulo = paleta.Destaque;
                Texto = Color.FromArgb(200, 200, 200);
                TextoSuave = Color.FromArgb(130, 130, 130);
            }
            else
            {
                Fundo = Color.FromArgb(246, 244, 236);
                Superficie = Color.White;
                Titulo = paleta.Menu;
                Texto = Color.FromArgb(100, 100, 100);
                TextoSuave = Color.FromArgb(140, 140, 140);
            }

            switch (preferencias.TamanhoFonte)
            {
                case TamanhoFonte.Pequena: Escala = 0.9f; break;
                case TamanhoFonte.Grande: Escala = 1.2f; break;
                default: Escala = 1f; break;
            }
        }

        // Nome desconhecido (ex.: arquivo editado à mão) volta para a paleta padrão
        public static Paleta BuscarPaleta(string? nome)
        {
            foreach (Paleta p in Paletas)
            {
                if (p.Nome == nome)
                {
                    return p;
                }
            }
            return Paletas[0];
        }

        // ---------- Fontes e medidas ----------

        // Fonte já ajustada à escala. "tamanho" é o tamanho original, em pontos.
        // As fontes ficam guardadas e são reaproveitadas, em vez de criar uma nova
        // a cada linha de hábito montada.
        public static Font Fonte(float tamanho, FontStyle estilo = FontStyle.Regular, string familia = FonteTexto)
        {
            float tamanhoFinal = (float)Math.Round(tamanho * Escala, 1);
            string chave = familia + "|" + tamanhoFinal + "|" + (int)estilo;

            if (!_fontes.TryGetValue(chave, out Font? fonte))
            {
                fonte = new Font(familia, tamanhoFinal, estilo);
                _fontes[chave] = fonte;
            }
            return fonte;
        }

        // Medida em pixels ajustada à escala (alturas, posições), para o texto maior caber
        public static int Escalar(int pixels)
        {
            return (int)Math.Round(pixels * Escala);
        }
    }

    public class Paleta
    {
        public string Nome { get; }
        public Color Menu { get; }
        public Color MenuSelecionado { get; }
        public Color Destaque { get; }

        public Paleta(string nome, Color menu, Color menuSelecionado, Color destaque)
        {
            Nome = nome;
            Menu = menu;
            MenuSelecionado = menuSelecionado;
            Destaque = destaque;
        }
    }
}
