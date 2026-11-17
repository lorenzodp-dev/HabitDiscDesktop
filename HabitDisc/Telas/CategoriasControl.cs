using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FontAwesome.Sharp;
using HabitDisc.Forms;
using HabitDisc.Helpers;
using HabitDisc.Models;
using HabitDisc.Services;
using HabitDisc.Telas.Componentes;

namespace HabitDisc.Telas
{
    // Tela Categorias (UC09): mostra as predefinidas e as personalizadas;
    // cria, edita e exclui as personalizadas.
    public partial class CategoriasControl : TelaBase
    {
        private readonly CategoriaService _service = new CategoriaService();

        // Dados exibidos agora (null até a primeira carga)
        private List<Categoria>? _categorias;
        private Dictionary<int, int> _habitosPorCategoria = new Dictionary<int, int>();

        public CategoriasControl()
        {
            InitializeComponent();
            ListaVertical.Configurar(flpLista);
            AplicarTema();
        }

        // ---------- Eventos ----------

        private void CategoriasControl_VisibleChanged(object? sender, EventArgs e)
        {
            if (Visible && !DesignMode)
            {
                Carregar();
            }
        }

        private void btnNova_Click(object? sender, EventArgs e)
        {
            using FormCategoria form = new FormCategoria(_service, null);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Carregar();
            }
        }

        // ---------- Carregamento ----------

        private void Carregar()
        {
            _categorias = _service.Listar();
            _habitosPorCategoria = _service.ContarHabitosPorCategoria();
            MontarLinhas();
        }

        private void MontarLinhas()
        {
            ListaVertical.Limpar(flpLista);

            flpLista.SuspendLayout();
            foreach (Categoria categoria in _categorias ?? new List<Categoria>())
            {
                LinhaItemControl linha = new LinhaItemControl(
                    CorHelper.HexParaCor(categoria.Cor),
                    IconeHelper.ParaIconChar(categoria.Icone),
                    categoria.Nome,
                    TextoQuantidade(categoria.IdCategoria));

                if (categoria.Predefinida)
                {
                    linha.AdicionarEtiqueta("Predefinida");
                }
                else
                {
                    Categoria c = categoria;
                    linha.AdicionarAcao(IconChar.Pen, "Editar", delegate { Editar(c); });
                    linha.AdicionarAcao(IconChar.TrashCan, "Excluir", delegate { Excluir(c); });
                }

                flpLista.Controls.Add(linha);
            }
            flpLista.ResumeLayout();
        }

        private string TextoQuantidade(int idCategoria)
        {
            _habitosPorCategoria.TryGetValue(idCategoria, out int quantidade);

            if (quantidade == 0) return "Nenhum hábito";
            if (quantidade == 1) return "1 hábito";
            return quantidade + " hábitos";
        }

        // ---------- Ações ----------

        private void Editar(Categoria categoria)
        {
            using FormCategoria form = new FormCategoria(_service, categoria);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Carregar();
            }
        }

        private void Excluir(Categoria categoria)
        {
            if (!Mensagem.Confirmar("Excluir a categoria \"" + categoria.Nome + "\"?"))
            {
                return;
            }

            try
            {
                _service.Excluir(categoria.IdCategoria);
                Carregar();
            }
            catch (RegraNegocioException ex)
            {
                // Fluxo alternativo do UC09: categoria com hábitos vinculados
                Mensagem.Aviso(ex.Message);
            }
        }

        // ---------- Tema ----------

        protected override void AplicarTema()
        {
            BackColor = Tema.Fundo;
            Estilo.Cabecalho(pnlTopo, lblTitulo, btnNova);

            Estilo.TextoSuave(lblDica);
            lblDica.Height = lblDica.Font.Height + Tema.Escalar(14);

            if (_categorias != null)
            {
                MontarLinhas();
            }
        }
    }
}
