using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HabitDisc.Helpers;
using HabitDisc.Services;
using HabitDisc.Telas.Componentes;

namespace HabitDisc.Telas
{
    // Tela Estatísticas (UC04 / RF04): taxa de conclusão, comparativo com o
    // período anterior, check-ins feitos e um gráfico por dia.
    // Filtros: hábito (todos ou um), semana ou mês, e navegação entre períodos.
    public partial class EstatisticasControl : TelaBase
    {
        private static readonly Color CorMelhorou = Color.FromArgb(55, 155, 95);
        private static readonly Color CorPiorou = Color.FromArgb(205, 90, 90);

        private readonly EstatisticaService _estatisticas = new EstatisticaService();
        private readonly HabitoService _habitos = new HabitoService();

        // Largura base 176: os 3 cartões cabem lado a lado na janela padrão
        private readonly CartaoNumero _cartaoTaxa = new CartaoNumero(176);
        private readonly CartaoNumero _cartaoComparativo = new CartaoNumero(176);
        private readonly CartaoNumero _cartaoTotal = new CartaoNumero(176);

        // Qualquer dia dentro do período exibido
        private DateOnly _referencia = DateOnly.FromDateTime(DateTime.Today);

        // true enquanto a ComboBox é preenchida por código (não recalcula à toa)
        private bool _carregando;

        // Último resultado, para redesenhar quando o tema muda sem ir ao banco
        private bool _temDados;
        private ResumoPeriodo? _atual;
        private ResumoPeriodo? _anterior;

        public EstatisticasControl()
        {
            InitializeComponent();

            flpCartoes.Controls.Add(_cartaoTaxa);
            flpCartoes.Controls.Add(_cartaoComparativo);
            flpCartoes.Controls.Add(_cartaoTotal);

            AplicarTema();
        }

        private bool PorMes
        {
            get { return rdoMes.Checked; }
        }

        // ---------- Eventos ----------

        private void EstatisticasControl_VisibleChanged(object? sender, EventArgs e)
        {
            if (Visible && !DesignMode)
            {
                CarregarHabitos();
                Atualizar();
            }
        }

        private void Filtro_Alterado(object? sender, EventArgs e)
        {
            // O CheckedChanged também dispara no RadioButton que foi DESmarcado
            if (_carregando || (sender is RadioButton rdo && !rdo.Checked))
            {
                return;
            }
            Atualizar();
        }

        private void btnAnterior_Click(object? sender, EventArgs e)
        {
            _referencia = PorMes ? _referencia.AddMonths(-1) : _referencia.AddDays(-7);
            Atualizar();
        }

        private void btnProximo_Click(object? sender, EventArgs e)
        {
            _referencia = PorMes ? _referencia.AddMonths(1) : _referencia.AddDays(7);
            Atualizar();
        }

        // ---------- Carregamento ----------

        // Os hábitos podem ter mudado na tela Hábitos: refaz a lista do filtro,
        // mantendo o que estava escolhido (se o hábito ainda existir)
        private void CarregarHabitos()
        {
            long? escolhido = (cmbHabito.SelectedItem as ItemFiltro)?.IdHabito;

            _carregando = true;

            cmbHabito.Items.Clear();
            cmbHabito.Items.Add(new ItemFiltro(null, "Todos os hábitos"));
            foreach (HabitoResumo habito in _habitos.Listar())
            {
                cmbHabito.Items.Add(new ItemFiltro(habito.Id, habito.Nome));
            }

            int indice = 0;
            for (int i = 0; i < cmbHabito.Items.Count; i++)
            {
                if (((ItemFiltro)cmbHabito.Items[i]!).IdHabito == escolhido)
                {
                    indice = i;
                }
            }
            cmbHabito.SelectedIndex = indice;

            _carregando = false;
        }

        private (DateOnly Inicio, DateOnly Fim) Periodo(DateOnly referencia)
        {
            return PorMes ? EstatisticaService.Mes(referencia) : EstatisticaService.Semana(referencia);
        }

        private void Atualizar()
        {
            DateOnly hoje = DateOnly.FromDateTime(DateTime.Today);
            long? idHabito = (cmbHabito.SelectedItem as ItemFiltro)?.IdHabito;

            // Não deixa ir para um período que ainda nem começou
            if (Periodo(_referencia).Inicio > hoje)
            {
                _referencia = hoje;
            }

            (DateOnly inicio, DateOnly fim) = Periodo(_referencia);
            (DateOnly inicioAnterior, DateOnly fimAnterior) =
                Periodo(PorMes ? inicio.AddMonths(-1) : inicio.AddDays(-7));

            _temDados = _estatisticas.TemDadosSuficientes(idHabito);
            if (_temDados)
            {
                _atual = _estatisticas.Calcular(idHabito, inicio, fim);
                _anterior = _estatisticas.Calcular(idHabito, inicioAnterior, fimAnterior);
            }

            lblPeriodo.Text = PorMes ? Textos.MesAno(inicio) : Textos.Intervalo(inicio, fim);
            DateOnly inicioProximo = PorMes ? inicio.AddMonths(1) : inicio.AddDays(7);
            btnProximo.Enabled = inicioProximo <= hoje;

            AlinharFiltros();
            MostrarResultado();
        }

        // Preenche os cartões e o gráfico com o último resultado
        private void MostrarResultado()
        {
            // Mostra uma coisa ou outra: o resultado inteiro ou o aviso de falta de dados
            lblSemDados.Visible = !_temDados;
            pnlResultado.Visible = _temDados;

            if (!_temDados || _atual == null || _anterior == null)
            {
                return;
            }

            string noPeriodo = PorMes ? "no mês" : "na semana";

            _cartaoTaxa.Valor = Textos.Porcentagem(_atual.Taxa);
            _cartaoTaxa.Rotulo = "Conclusão";
            _cartaoTaxa.Detalhe = noPeriodo;

            _cartaoComparativo.Rotulo = PorMes ? "Vs. mês anterior" : "Vs. semana anterior";
            if (_atual.Taxa == null || _anterior.Taxa == null)
            {
                _cartaoComparativo.Valor = "—";
                _cartaoComparativo.CorValor = Tema.TextoSuave;
                _cartaoComparativo.Detalhe = "sem dados para comparar";
            }
            else
            {
                // Diferença em pontos percentuais (72% - 64% = 8 pts)
                int pontos = (int)Math.Round((_atual.Taxa.Value - _anterior.Taxa.Value) * 100);

                if (pontos > 0)
                {
                    _cartaoComparativo.Valor = "▲ " + pontos + " pts";
                    _cartaoComparativo.CorValor = CorMelhorou;
                }
                else if (pontos < 0)
                {
                    _cartaoComparativo.Valor = "▼ " + (-pontos) + " pts";
                    _cartaoComparativo.CorValor = CorPiorou;
                }
                else
                {
                    _cartaoComparativo.Valor = "= 0 pts";
                    _cartaoComparativo.CorValor = Tema.Titulo;
                }
                _cartaoComparativo.Detalhe = "antes: " + Textos.Porcentagem(_anterior.Taxa);
            }

            _cartaoTotal.Valor = _atual.Feitos + " de " + _atual.Esperados;
            _cartaoTotal.Rotulo = "Check-ins feitos";
            _cartaoTotal.Detalhe = "dos esperados " + noPeriodo;

            // Semana: "seg 9", "ter 10"...; mês: só o dia
            grafico.MostrarValores = !PorMes;
            grafico.DefinirPontos(_atual.Dias.Select(d =>
                (PorMes ? d.Data.Day.ToString() : Textos.DiaSemanaCurto(d.Data) + " " + d.Data.Day, d.Taxa)));
        }

        // Centraliza na altura todos os itens da linha de filtros
        private void AlinharFiltros()
        {
            int alturaLinha = 0;
            foreach (Control c in flpFiltros.Controls)
            {
                alturaLinha = Math.Max(alturaLinha, c.PreferredSize.Height);
            }

            foreach (Control c in flpFiltros.Controls)
            {
                int topo = (alturaLinha - c.PreferredSize.Height) / 2;
                c.Margin = new Padding(0, topo, Tema.Escalar(12), 0);
            }
        }

        // ---------- Tema ----------

        protected override void AplicarTema()
        {
            BackColor = Tema.Fundo;

            Estilo.Titulo(lblTitulo);
            lblTitulo.Height = lblTitulo.Font.Height + Tema.Escalar(12);

            Estilo.Combo(cmbHabito);
            cmbHabito.Width = Tema.Escalar(240);
            Estilo.Texto(rdoSemana);
            Estilo.Texto(rdoMes);

            int tamanhoSeta = Tema.Escalar(34);
            foreach (FontAwesome.Sharp.IconButton seta in new[] { btnAnterior, btnProximo })
            {
                Estilo.BotaoSecundario(seta);
                seta.IconSize = Tema.Escalar(16);
                seta.Size = new Size(tamanhoSeta, tamanhoSeta);
            }

            lblPeriodo.Font = Tema.Fonte(13f, FontStyle.Bold);
            lblPeriodo.ForeColor = Tema.Titulo;
            AlinharFiltros();

            foreach (CartaoNumero cartao in new[] { _cartaoTaxa, _cartaoComparativo, _cartaoTotal })
            {
                cartao.AplicarTema();
                cartao.Margin = new Padding(0, 0, Tema.Escalar(12), Tema.Escalar(4));
            }

            Estilo.Secao(lblGrafico);
            lblGrafico.Height = lblGrafico.Font.Height + Tema.Escalar(26);

            grafico.BackColor = Tema.Fundo;
            grafico.ForeColor = Tema.Texto;
            grafico.CorBarra = Tema.Destaque;
            grafico.CorTrilha = Tema.Superficie;
            grafico.Font = Tema.Fonte(10f);
            grafico.Invalidate();

            lblSemDados.Font = Tema.Fonte(13f);
            lblSemDados.ForeColor = Tema.TextoSuave;

            MostrarResultado();
        }

        // Item da ComboBox de hábitos. IdHabito nulo = "Todos os hábitos".
        private class ItemFiltro
        {
            public long? IdHabito { get; }
            public string Nome { get; }

            public ItemFiltro(long? idHabito, string nome)
            {
                IdHabito = idHabito;
                Nome = nome;
            }

            // É o texto que a ComboBox mostra
            public override string ToString()
            {
                return Nome;
            }
        }
    }
}
