using System.Windows.Forms;
using HabitDisc.Helpers;
using HabitDisc.Models;
using HabitDisc.Services;
using HabitDisc.Telas.Componentes;

namespace HabitDisc.Forms
{
    // Criar (UC01) ou editar (UC07) um hábito: nome, categoria, ícone e cor.
    public class FormHabito : FormDialogoBase
    {
        private readonly HabitoService _habitos;
        private readonly HabitoDetalhes? _habito;

        private readonly TextBox _txtNome = new TextBox();
        private readonly ComboBox _cmbCategoria = new ComboBox();
        private readonly SeletorIcone _seletorIcone = new SeletorIcone();
        private readonly SeletorCor _seletorCor = new SeletorCor();

        // habito == null: novo hábito; senão, edição dele
        public FormHabito(HabitoService habitos, CategoriaService categorias, HabitoDetalhes? habito)
            : base(habito == null ? "Novo hábito" : "Editar hábito")
        {
            _habitos = habitos;
            _habito = habito;

            _txtNome.Width = Tema.Escalar(440);
            _txtNome.MaxLength = HabitoService.TamanhoMaximoNome;
            _txtNome.PlaceholderText = "Ex.: Beber 2L de água";
            Estilo.CaixaTexto(_txtNome);

            _cmbCategoria.Width = Tema.Escalar(440);
            Estilo.Combo(_cmbCategoria);
            // A ComboBox guarda o objeto Categoria inteiro e mostra só o Nome
            _cmbCategoria.DisplayMember = nameof(Categoria.Nome);
            foreach (Categoria categoria in categorias.Listar())
            {
                _cmbCategoria.Items.Add(categoria);
            }

            AdicionarCampo("Nome", _txtNome);
            AdicionarCampo("Categoria", _cmbCategoria);
            AdicionarCampo("Ícone", _seletorIcone);
            AdicionarCampo("Cor", _seletorCor);

            if (habito != null)
            {
                _txtNome.Text = habito.Nome;
                _seletorIcone.IconeSelecionado = habito.Icone;
                _seletorCor.CorSelecionada = habito.Cor;
                SelecionarCategoria(habito.IdCategoria);
            }
            else if (_cmbCategoria.Items.Count > 0)
            {
                _cmbCategoria.SelectedIndex = 0;
            }

            AdicionarBotaoFechar("Cancelar", principal: false);
            AcceptButton = AdicionarBotao("Salvar", principal: true, delegate { Salvar(); });
        }

        private void SelecionarCategoria(int idCategoria)
        {
            foreach (Categoria categoria in _cmbCategoria.Items)
            {
                if (categoria.IdCategoria == idCategoria)
                {
                    _cmbCategoria.SelectedItem = categoria;
                    return;
                }
            }
        }

        private void Salvar()
        {
            if (_cmbCategoria.SelectedItem is not Categoria categoria)
            {
                Mensagem.Aviso("Escolha uma categoria.");
                return;
            }

            try
            {
                if (_habito == null)
                {
                    _habitos.Criar(_txtNome.Text, _seletorIcone.IconeSelecionado,
                        _seletorCor.CorSelecionada, categoria.IdCategoria);
                }
                else
                {
                    _habitos.Atualizar(_habito.Id, _txtNome.Text, _seletorIcone.IconeSelecionado,
                        _seletorCor.CorSelecionada, categoria.IdCategoria);
                }

                DialogResult = DialogResult.OK;   // fecha a janela
            }
            catch (RegraNegocioException ex)
            {
                // Fluxo alternativo do UC01: ex.: nome vazio. A janela continua aberta.
                Mensagem.Aviso(ex.Message);
                _txtNome.Focus();
            }
        }
    }
}
