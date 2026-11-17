using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HabitDisc.Telas.Componentes
{
    // Gráfico de barras simples, desenhado "à mão" (GDI+), para as Estatísticas.
    // Cada ponto é um dia: a barra vai de 0% a 100%. Valor nulo (dia futuro, ou
    // antes de o hábito existir) mostra só o fundo da barra.
    // Feito sem biblioteca externa para não acrescentar dependência ao projeto.
    public class GraficoBarras : Control
    {
        private List<(string Rotulo, double? Valor)> _pontos = new List<(string, double?)>();

        // Definidas por código (pelo Tema), por isso ocultas para o designer
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color CorBarra { get; set; } = Color.SeaGreen;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color CorTrilha { get; set; } = Color.Gainsboro;

        // Escreve a porcentagem em cima de cada barra (bom para poucas barras)
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool MostrarValores { get; set; }

        public GraficoBarras()
        {
            // Evita que o gráfico "pisque" e redesenha quando muda de tamanho
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        public void DefinirPontos(IEnumerable<(string Rotulo, double? Valor)> pontos)
        {
            _pontos = pontos.ToList();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.Clear(BackColor);

            int quantidade = _pontos.Count;
            if (quantidade == 0)
            {
                return;
            }

            // Espaço reservado para os rótulos embaixo e as porcentagens em cima
            int alturaRotulo = Font.Height + 6;
            int alturaValor = MostrarValores ? Font.Height + 4 : 0;
            int alturaArea = Height - alturaRotulo - alturaValor;
            if (alturaArea < 10)
            {
                return;
            }

            float larguraColuna = (float)Width / quantidade;
            float larguraBarra = Math.Max(2f, larguraColuna * 0.6f);

            // Com muitas colunas (mês), escreve o rótulo só de algumas, para não encavalar
            int larguraTexto = _pontos.Max(p => TextRenderer.MeasureText(p.Rotulo, Font).Width) + 4;
            int passo = Math.Max(1, (int)Math.Ceiling(larguraTexto / larguraColuna));

            using SolidBrush pincelTrilha = new SolidBrush(CorTrilha);
            using SolidBrush pincelBarra = new SolidBrush(CorBarra);

            for (int i = 0; i < quantidade; i++)
            {
                (string rotulo, double? valor) = _pontos[i];

                float centro = i * larguraColuna + larguraColuna / 2;
                float x = centro - larguraBarra / 2;

                // Fundo da barra (100%)
                g.FillRectangle(pincelTrilha, x, alturaValor, larguraBarra, alturaArea);

                if (valor != null)
                {
                    float altura = (float)(alturaArea * Math.Clamp(valor.Value, 0, 1));
                    float topo = alturaValor + alturaArea - altura;
                    g.FillRectangle(pincelBarra, x, topo, larguraBarra, altura);

                    if (MostrarValores)
                    {
                        string texto = Math.Round(valor.Value * 100) + "%";
                        Rectangle areaTexto = new Rectangle((int)(centro - 40), (int)topo - Font.Height - 2, 80, Font.Height);
                        TextRenderer.DrawText(g, texto, Font, areaTexto, ForeColor, TextFormatFlags.HorizontalCenter);
                    }
                }

                if (i % passo == 0)
                {
                    Rectangle areaRotulo = new Rectangle((int)(centro - larguraTexto), alturaValor + alturaArea + 4,
                                                         larguraTexto * 2, Font.Height);
                    TextRenderer.DrawText(g, rotulo, Font, areaRotulo, ForeColor, TextFormatFlags.HorizontalCenter);
                }
            }
        }
    }
}
