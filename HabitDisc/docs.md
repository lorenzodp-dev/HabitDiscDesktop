# HabitDisc — Versão Desktop

**Versão**: v6.0 (Desktop) — atualizada em 16/11/2026 (contas de usuário e API)

**Membros**
Lorenzo, Felipe, Enzo, João Pedro e Maycon

> Esta documentação cobre **somente a versão desktop** (C# / Windows Forms). As versões web e mobile terão documentação própria, a ser escrita separadamente.

---

## Sumário

1. [Introdução](#1-introdução)
2. [Requisitos do Sistema](#2-requisitos-do-sistema)
   - 2.1 Requisitos Funcionais
   - 2.2 Requisitos Não Funcionais
3. [Casos de Uso](#3-casos-de-uso)
4. [Diagramas UML](#4-diagramas-uml)
5. [Modelagem do Banco de Dados](#5-modelagem-do-banco-de-dados)
6. [Requisitos Técnicos](#6-requisitos-técnicos)

---

## 1. Introdução

### 1.1 Descrição Geral do Sistema

O HabitDisc Desktop é um aplicativo para Windows, desenvolvido em C# com Windows Forms, de gestão e rastreamento de hábitos pessoais. O app tem como proposta central ajudar o usuário a criar, acompanhar e manter hábitos em seu cotidiano, principalmente pelos lembretes, combinando simplicidade de uso com recursos de motivação e autoconhecimento.

### 1.2 Objetivo do Projeto

Desenvolver um aplicativo desktop funcional que permita ao usuário:

- Criar e organizar hábitos personalizados por categoria
- Realizar check-ins diários de forma rápida e intuitiva
- Acompanhar o progresso por meio de estatísticas e gráficos
- Manter a disciplina por meio de streaks e lembretes
- Registrar o humor diário com histórico pessoal

### 1.3 Público-Alvo

O HabitDisc é destinado a pessoas com 15 anos ou mais que desejam organizar melhor sua rotina, desenvolver novos hábitos ou superar hábitos negativos. O app atende perfis variados: estudantes, profissionais, atletas amadores e qualquer pessoa interessada em produtividade e bem-estar pessoal.

### 1.4 Justificativa

A formação de hábitos é a base para uma vida produtiva e ordenada. Entretanto, grande parte das pessoas encontra dificuldade em manter consistência sem ferramentas adequadas de acompanhamento e motivação. O HabitDisc surge como solução acessível e completa, unindo rastreamento eficiente com recursos de engajamento que reduzem o abandono ao longo do tempo.

### 1.5 Limitações do Sistema

O HabitDisc não afirma categoricamente que todo usuário irá evoluir apenas por utilizar o aplicativo. O crescimento e o desenvolvimento pessoal dependem do comprometimento e da disciplina de quem usa o sistema.

### 1.6 Escopo desta Versão

- **Vários usuários, com conta:** cada pessoa se cadastra com nome, e-mail e senha, entra com o próprio login e vê somente os próprios dados.
- **Banco em servidor compartilhado,** acessado somente por uma API (seção 6.8). O aplicativo nunca se conecta direto ao banco. Por isso a mesma conta funciona em qualquer computador com o aplicativo instalado.
- A aparência (tema, fonte e modo escuro) é do **computador**, e não da conta (seção 5.4).
- Plataforma exclusiva: Windows.

---

## 2. Requisitos do Sistema

### 2.1 Requisitos Funcionais

| ID   | Funcionalidade        | Descrição |
|------|-----------------------|-----------|
| RF01 | Cadastro de Hábito    | O usuário pode criar hábitos com nome livre, ícone, cor e categoria. |
| RF02 | Check-in Diário       | Interface para marcar hábitos concluídos no dia, com indicador de progresso (ex: 3 de 5 feitos) e exibição do streak atual calculado a partir do histórico de check-ins. |
| RF03 | Lembretes             | Horário personalizado por hábito, com notificação do Windows no horário configurado enquanto o aplicativo estiver em execução, e resumo semanal de desempenho exibido dentro do aplicativo. |
| RF04 | Estatísticas          | Gráfico semanal/mensal, taxa de conclusão e comparativo entre semanas. |
| RF05 | Categorias            | Categorias predefinidas (Saúde, Produtividade, Aprendizado, Relacionamentos, Finanças), com possibilidade de criação de categorias personalizadas. |
| RF06 | Registro de Humor     | Registro emocional diário opcional com histórico. |
| RF07 | Edição e Exclusão     | O usuário pode editar ou excluir hábitos cadastrados a qualquer momento. |
| RF08 | Log de Banco de Dados | O sistema deve registrar automaticamente um histórico de operações críticas (como criação, alteração e exclusão de hábitos, categorias personalizadas e registros de check-in) para fins de auditoria e recuperação de dados. |
| RF09 | Personalização da Aparência | O usuário pode escolher a cor do tema (entre paletas prontas), o tamanho da fonte (pequena, média ou grande) e o modo escuro. As escolhas valem na hora e ficam salvas para as próximas aberturas do aplicativo. |
| RF10 | Contas de Usuário     | Cadastro de conta (nome, e-mail e senha), login, logout, alteração dos dados da conta (nome, e-mail e senha) e exclusão da conta com todos os seus dados. Cada usuário acessa somente os próprios hábitos, check-ins, humores e categorias personalizadas. |
| RF11 | Recuperação de Senha  | O usuário que esqueceu a senha recebe por e-mail um código de uso único e validade curta, com o qual define uma nova senha. |

### 2.2 Requisitos Não Funcionais

| ID    | Categoria              | Descrição |
|-------|------------------------|-----------|
| RNF01 | Desempenho             | O app deve abrir a tela inicial e alternar entre as telas sem atrasos perceptíveis. |
| RNF02 | Usabilidade            | A interface deve ser intuitiva, com fluxo de check-in realizável em no máximo 2 cliques. |
| RNF03 | Segurança e Privacidade (LGPD) | O sistema coleta somente nome e e-mail, usados para identificar a conta e recuperar a senha (finalidade e minimização, LGPD — Lei nº 13.709/2018). A senha nunca é armazenada: só o hash dela, calculado na API. O e-mail só é usado para enviar o código de recuperação de senha, e os dados não são enviados a terceiros, exceto ao serviço de envio de e-mail. A comunicação entre aplicativo e API é feita por HTTPS. Somente a API acessa o banco, e as credenciais do banco e do e-mail ficam só no servidor, nunca no código-fonte nem nos computadores dos usuários. O banco tem criptografia em repouso habilitada no servidor. O usuário pode excluir hábitos e registros a qualquer momento (RF07) e excluir a conta, o que apaga todos os seus dados, inclusive do log (RF10). |
| RNF06 | Controle de Acesso     | Toda operação, exceto cadastro, login e recuperação de senha, exige um usuário autenticado. A API identifica o usuário pelo token do login, e nunca por um id enviado pelo aplicativo, e só devolve ou altera dados desse usuário. Senha com no mínimo 8 caracteres. |
| RNF04 | Compatibilidade        | O app deve funcionar em Windows 10 ou superior, inclusive em computadores de baixo desempenho. |
| RNF05 | Volume de Dados        | O histórico de check-ins e humores pode crescer por anos sem degradação perceptível de desempenho (uso de índices nas colunas consultadas com mais frequência, como `id_habito` e `data`). |

---

## 3. Casos de Uso

### 3.1 Atores

- **Visitante:** pessoa que ainda não entrou na conta. Pode se cadastrar, entrar e recuperar a senha.
- **Usuário:** pessoa autenticada, que utiliza o aplicativo para gerenciar seus hábitos.
- **Sistema:** o aplicativo HabitDisc e a sua API, responsáveis por processar e exibir dados.

### 3.2 Lista de Casos de Uso

| ID   | Caso de Uso              | Descrição |
|------|--------------------------|-----------|
| UC01 | Criar hábito             | O usuário configura um novo hábito com nome, categoria, ícone e cor. |
| UC02 | Realizar check-in        | O usuário marca um hábito como concluído no dia. |
| UC03 | Visualizar streak        | O usuário consulta o streak calculado a partir do histórico de check-ins. |
| UC04 | Consultar estatísticas   | O usuário acessa gráficos de desempenho por período. |
| UC05 | Registrar humor          | O usuário registra seu estado emocional do dia de forma opcional. |
| UC06 | Receber notificação      | O sistema exibe lembretes conforme configurado. |
| UC07 | Editar/excluir hábito    | O usuário modifica ou remove um hábito existente. |
| UC08 | Visualizar hábito        | O usuário acessa os detalhes de um hábito específico. |
| UC09 | Gerenciar categorias     | O usuário cria, edita ou exclui categorias personalizadas. |
| UC10 | Personalizar aparência   | O usuário muda a cor do tema, o tamanho da fonte e o modo escuro. |
| UC11 | Cadastrar conta          | O visitante cria uma conta com nome, e-mail e senha. |
| UC12 | Entrar (login)           | O visitante entra na conta com e-mail e senha. |
| UC13 | Sair (logout)            | O usuário sai da conta. |
| UC14 | Recuperar senha          | O visitante redefine a senha com um código recebido por e-mail. |
| UC15 | Alterar dados da conta   | O usuário altera o nome, o e-mail ou a senha. |
| UC16 | Excluir conta            | O usuário exclui a conta e todos os seus dados. |

> Os casos de uso UC01 a UC10, UC13, UC15 e UC16 exigem um usuário autenticado (UC12) e atuam somente sobre os dados desse usuário (RNF06).

### 3.3 Descrições Expandidas dos Casos de Uso

#### UC01 — Criar Hábito

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Usuário |
| Pré-condição      | Aplicativo em execução. |
| Fluxo principal   | 1. Usuário acessa "Novo Hábito". 2. Preenche nome, categoria, ícone e cor. 3. Confirma a criação. 4. Sistema salva e exibe o hábito na lista de hábitos. |
| Fluxo alternativo | Se o usuário não preencher o nome, o sistema exibe mensagem de erro e solicita preenchimento. |

#### UC02 — Realizar Check-in

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Usuário |
| Pré-condição      | Ao menos um hábito cadastrado. |
| Fluxo principal   | 1. Usuário acessa a tela inicial. 2. Visualiza a lista de hábitos do dia. 3. Marca um hábito como concluído. 4. Sistema registra o check-in e atualiza o indicador de progresso. |
| Fluxo alternativo | Se o usuário tentar fazer check-in em um hábito já concluído no dia, o sistema permite desmarcar a conclusão. |

#### UC03 — Visualizar Streak

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Usuário |
| Pré-condição      | Ao menos um check-in registrado. |
| Fluxo principal   | 1. Usuário acessa o hábito desejado. 2. Sistema calcula o streak a partir do histórico de check-ins e exibe o contador de dias consecutivos atual e o melhor recorde histórico. |
| Fluxo alternativo | Se o usuário não realizou check-in no dia anterior, o sistema exibe o streak zerado e o recorde anterior como histórico. |

#### UC04 — Consultar Estatísticas

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Usuário |
| Pré-condição      | Ao menos uma semana de check-ins registrados. |
| Fluxo principal   | 1. Usuário acessa a tela de estatísticas. 2. Sistema exibe gráfico semanal/mensal, taxa de conclusão e comparativo entre semanas. 3. Usuário pode filtrar por hábito ou período. |
| Fluxo alternativo | Se não houver dados suficientes, o sistema exibe mensagem orientando o usuário a registrar hábitos por mais dias. |

#### UC05 — Registrar Humor

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Usuário |
| Pré-condição      | Aplicativo em execução. |
| Fluxo principal   | 1. Usuário acessa a tela de registro de humor. 2. Seleciona seu estado emocional na escala de 1 a 5. 3. Opcionalmente, adiciona uma observação escrita. 4. Confirma o registro. 5. Sistema salva e exibe o histórico atualizado. |
| Fluxo alternativo | Se o usuário já registrou o humor no dia, o sistema permite editar o registro existente. |

#### UC06 — Receber Notificação

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Sistema |
| Pré-condição      | Aplicativo em execução, com hábitos cadastrados e ao menos um lembrete ativo. |
| Fluxo principal   | 1. Sistema identifica hábitos com lembrete configurado para o horário atual. 2. Exibe a notificação do Windows ao usuário. 3. O resumo semanal de desempenho fica disponível dentro do aplicativo. |
| Fluxo alternativo | Se as notificações do Windows estiverem desativadas, o sistema não consegue exibir o alerta e mostra um aviso dentro do aplicativo. |

#### UC07 — Editar/Excluir Hábito

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Usuário |
| Pré-condição      | Ao menos um hábito cadastrado. |
| Fluxo principal   | 1. Usuário acessa o hábito desejado. 2. Seleciona "Editar" ou "Excluir". 3a. Se editar: altera os dados desejados e confirma. O sistema salva as alterações. 3b. Se excluir: o sistema solicita confirmação. Após confirmação, o hábito e seus dados associados (check-ins e lembretes) são removidos. |
| Fluxo alternativo | Se o usuário cancelar a exclusão na confirmação, o sistema retorna ao hábito sem alterações. |

#### UC08 — Visualizar Hábito

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Usuário |
| Pré-condição      | Ao menos um hábito cadastrado. |
| Fluxo principal   | 1. Usuário seleciona um hábito na lista. 2. Sistema exibe os detalhes do hábito: nome, ícone, cor e categoria. 3. Sistema calcula e exibe o streak atual, melhor recorde e histórico recente de check-ins. |
| Fluxo alternativo | Se o hábito não possuir nenhum check-in registrado, o sistema exibe os detalhes do hábito com streak zerado e histórico vazio. |

#### UC09 — Gerenciar Categorias

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Usuário |
| Pré-condição      | Aplicativo em execução. |
| Fluxo principal   | 1. Usuário acessa a tela de categorias. 2. Visualiza as categorias predefinidas e as suas próprias. 2a. Para criar: preenche nome, ícone e cor e confirma. 3a. Sistema salva e exibe a nova categoria. 2b. Para editar: seleciona uma categoria personalizada, altera os dados e confirma. 3b. Sistema salva as alterações. 2c. Para excluir: seleciona uma categoria personalizada e confirma a exclusão. 3c. Sistema remove a categoria. |
| Fluxo alternativo | Se a categoria excluída estiver vinculada a hábitos existentes, o sistema exibe erro e sinaliza que categorias com hábitos vinculados não podem ser apagadas. Categorias predefinidas não podem ser editadas nem excluídas. |

#### UC10 — Personalizar Aparência

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Usuário |
| Pré-condição      | Aplicativo em execução. |
| Fluxo principal   | 1. Usuário acessa a tela de configurações (botão no rodapé do menu lateral). 2. Escolhe a cor do tema, o tamanho da fonte e/ou o modo escuro. 3. O sistema aplica a mudança na hora, em todas as telas, e salva a escolha. 4. Na próxima abertura, o aplicativo já inicia com as preferências salvas. |
| Fluxo alternativo | Se o usuário escolher "Restaurar padrão", o sistema volta para o tema verde, fonte média e modo claro. Se não for possível salvar o arquivo de preferências, o sistema avisa e mantém a mudança só até o aplicativo ser fechado. |

#### UC11 — Cadastrar Conta

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Visitante |
| Pré-condição      | Aplicativo em execução, na tela de entrada. |
| Fluxo principal   | 1. Visitante escolhe "Criar conta". 2. Preenche nome, e-mail, senha e confirmação da senha. 3. Confirma. 4. A API grava o e-mail em minúsculas e a senha como hash. 5. O sistema cria a conta e já entra nela, abrindo a tela inicial. |
| Fluxo alternativo | Se algum campo estiver vazio, se o e-mail for inválido, se a senha tiver menos de 8 caracteres ou se a confirmação não conferir, o sistema mostra o erro e não cria a conta. Se o e-mail já tiver uma conta, o sistema avisa e sugere entrar ou recuperar a senha. |

#### UC12 — Entrar (Login)

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Visitante |
| Pré-condição      | Conta cadastrada. |
| Fluxo principal   | 1. Visitante informa e-mail e senha. 2. A API compara a senha com o hash salvo. 3. Se conferir, a API devolve um token de acesso, que o aplicativo usa nas próximas operações. 4. O sistema abre a tela inicial com os dados do usuário. |
| Fluxo alternativo | Se o e-mail não existir ou a senha estiver errada, o sistema mostra a mesma mensagem nos dois casos ("E-mail ou senha incorretos"), para não revelar quais e-mails têm conta. Se a API estiver fora do ar, o sistema avisa que não foi possível conectar. |

#### UC13 — Sair (Logout)

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Usuário |
| Pré-condição      | Usuário autenticado. |
| Fluxo principal   | 1. Usuário escolhe "Sair". 2. O aplicativo descarta o token de acesso e os dados carregados. 3. O sistema volta para a tela de entrada. |
| Fluxo alternativo | Se o token expirar durante o uso, o sistema avisa que a sessão terminou e volta para a tela de entrada. |

#### UC14 — Recuperar Senha

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Visitante |
| Pré-condição      | Aplicativo em execução, na tela de entrada. |
| Fluxo principal   | 1. Visitante escolhe "Esqueci minha senha" e informa o e-mail. 2. Se houver conta com esse e-mail, a API gera um código de 6 dígitos, guarda só o hash dele, com validade de 15 minutos, e o envia por e-mail. 3. Visitante digita o código e a nova senha (com confirmação). 4. Se o código conferir, estiver válido e não tiver sido usado, a API troca o hash da senha e marca o código como usado. 5. O sistema avisa que a senha foi alterada e volta para a tela de entrada. |
| Fluxo alternativo | No passo 2, o sistema mostra sempre a mesma mensagem ("Se houver uma conta com esse e-mail, enviamos um código"), exista a conta ou não. Código errado conta uma tentativa; depois de 5 tentativas, ou depois do prazo, o código deixa de valer e é preciso pedir outro. Um novo pedido invalida os códigos anteriores. |

#### UC15 — Alterar Dados da Conta

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Usuário |
| Pré-condição      | Usuário autenticado. |
| Fluxo principal   | 1. Usuário acessa "Minha conta". 2. Altera o nome, o e-mail e/ou a senha. 3. Para trocar o e-mail ou a senha, informa a senha atual. 4. Confirma. 5. O sistema salva e mostra os dados atualizados. |
| Fluxo alternativo | Se a senha atual estiver errada, o sistema não altera nada. Se o novo e-mail já pertencer a outra conta, o sistema avisa. As mesmas regras do cadastro valem para o novo e-mail e a nova senha. |

#### UC16 — Excluir Conta

| Campo             | Descrição |
|-------------------|-----------|
| Ator principal    | Usuário |
| Pré-condição      | Usuário autenticado. |
| Fluxo principal   | 1. Usuário acessa "Minha conta" e escolhe "Excluir conta". 2. O sistema explica que todos os dados serão apagados para sempre e pede a senha atual. 3. Usuário confirma. 4. A API apaga a conta e todos os dados dela: hábitos, check-ins, lembretes, humores, categorias personalizadas, pedidos de recuperação e log (procedure `sp_excluir_conta`). 5. O sistema volta para a tela de entrada. |
| Fluxo alternativo | Se a senha estiver errada ou o usuário cancelar, nada é apagado. |

---

## 4. Diagramas UML

### Diagrama de Casos de Uso

Ir para *HabitDisc/docs/diagramas_uc*. Usamos arquivos drawio.

> **Atenção:** os diagramas existentes foram feitos para uma versão anterior. Eles já tinham casos de uso de conta (cadastro, login, edição de perfil e exclusão de conta), que voltaram a fazer parte do sistema. É necessário revisá-los e atualizar a numeração (UC01 a UC16) conforme a seção 3, incluindo o ator **Visitante**.

### Diagrama de Classes

- A definir

---

## 5. Modelagem do Banco de Dados

### 5.1 Modelo Entidade-Relacionamento (MER)

| Entidade | Atributos | Relacionamentos |
| :--- | :--- | :--- |
| usuarios | id, nome, email, senha_hash, criado_em, atualizado_em | 1 usuário possui N hábitos, N categorias personalizadas, N humores e N pedidos de recuperação de senha |
| recuperacoes_senha | id, id_usuario, codigo_hash, expira_em, tentativas, usado, criado_em | N pedidos pertencem a 1 usuário |
| categorias | id, id_usuario, nome, icone, cor, predefinida | 1 categoria possui N hábitos; 1 categoria personalizada pertence a 1 usuário (predefinidas não pertencem a ninguém) |
| habitos | id, id_usuario, nome, icone, cor, id_categoria, criado_em | 1 hábito pertence a 1 usuário e a 1 categoria; 1 hábito possui N check-ins e N lembretes |
| checkins | id, id_habito, data, concluido | N check-ins pertencem a 1 hábito |
| humores | id, id_usuario, data, valor (1-5), observacao | N humores pertencem a 1 usuário (um registro por usuário por dia) |
| lembretes | id, id_habito, horario, ativo | N lembretes pertencem a 1 hábito |

> **Nota técnica:** O sistema tem vários usuários, em um banco compartilhado. Os check-ins e os lembretes não têm `id_usuario` próprio: pertencem ao usuário dono do hábito. O "dispositivo" da versão multiplataforma não existe na versão desktop.

> **Nota técnica:** As categorias predefinidas (`predefinida = true`, `id_usuario` nulo) são inseridas pelo script do banco de dados e servem a todos os usuários. As categorias criadas por um usuário são gravadas com `predefinida = false` e o `id_usuario` dele, e só ele as vê. A exclusão de uma categoria com hábitos vinculados deve ser bloqueada (chave estrangeira com `ON DELETE RESTRICT`).

> **Nota técnica:** Excluir a conta apaga todos os dados do usuário, em cascata e numa única transação (procedure `sp_excluir_conta`, seção 5.3).

> **Nota técnica:** O streak não é armazenado como tabela própria. Ele é calculado dinamicamente a partir do histórico de check-ins sempre que necessário, evitando inconsistências entre dados primários e derivados.

### 5.2 Dicionário de Dados

#### 5.2.1. Tabela: `usuarios`
Armazena as contas de usuário (RF10).

| Campo | Tipo de Dado | Restrições | Descrição |
| :--- | :--- | :--- | :--- |
| `id` | BIGINT | PK, AUTO_INCREMENT | Identificador único do usuário. |
| `nome` | VARCHAR(100) | NOT NULL | Nome do usuário, exibido no aplicativo. |
| `email` | VARCHAR(254) | NOT NULL, UNIQUE | E-mail usado no login e na recuperação de senha. Gravado em minúsculas pela API. |
| `senha_hash` | VARCHAR(255) | NOT NULL | Hash da senha, calculado pela API (seção 6.8). **A senha nunca é armazenada.** |
| `criado_em` | TIMESTAMP | DEFAULT NOW() | Data e hora do cadastro. |
| `atualizado_em` | TIMESTAMP | NULL, ON UPDATE NOW() | Data e hora da última alteração dos dados da conta. |

#### 5.2.2. Tabela: `recuperacoes_senha`
Guarda os pedidos de recuperação de senha (RF11). O código enviado por e-mail não é armazenado, só o hash dele.

| Campo | Tipo de Dado | Restrições | Descrição |
| :--- | :--- | :--- | :--- |
| `id` | BIGINT | PK, AUTO_INCREMENT | Identificador único do pedido. |
| `id_usuario` | BIGINT | FK (`usuarios.id`), NOT NULL, ON DELETE CASCADE | Usuário que pediu a recuperação. |
| `codigo_hash` | VARCHAR(255) | NOT NULL | Hash do código de 6 dígitos enviado por e-mail. |
| `expira_em` | DATETIME | NOT NULL | Fim da validade do código (em UTC). |
| `tentativas` | INT | NOT NULL, DEFAULT 0 | Quantas vezes um código errado foi digitado neste pedido. |
| `usado` | BOOLEAN | NOT NULL, DEFAULT FALSE | Se o código já foi usado para trocar a senha (uso único). |
| `criado_em` | TIMESTAMP | DEFAULT NOW() | Momento do pedido. |

#### 5.2.3. Tabela: `categorias`
Armazena as categorias predefinidas pelo sistema (de todos) e as personalizadas (de cada usuário).

| Campo | Tipo de Dado | Restrições | Descrição |
| :--- | :--- | :--- | :--- |
| `id` | INT | PK, AUTO_INCREMENT | Identificador único da categoria. |
| `id_usuario` | BIGINT | FK (`usuarios.id`), NULL, ON DELETE CASCADE | Dono da categoria personalizada. Nulo nas predefinidas. |
| `nome` | VARCHAR(50) | NOT NULL | Nome da categoria (ex: Saúde, Finanças). Combinação (`id_usuario`, `nome`) única: um usuário não tem duas categorias com o mesmo nome. |
| `icone` | VARCHAR(50) | NOT NULL | String identificadora do ícone (ex: 'heart', 'book'). |
| `cor` | VARCHAR(7) | NOT NULL | Código hexadecimal da cor associada (ex: '#FF5733'). |
| `predefinida` | BOOLEAN | DEFAULT FALSE | Indica se a categoria é predefinida (`true`) ou personalizada (`false`). |

> **Nota técnica:** A regra "predefinida tem `id_usuario` nulo; personalizada tem dono" é garantida pela API. Ela não pode ser um `CHECK` no banco porque o MySQL não aceita `CHECK` em coluna com `ON DELETE CASCADE`. A API também impede que um usuário crie uma categoria com o nome de uma predefinida e que use a categoria personalizada de outro usuário.

#### 5.2.4. Tabela: `habitos`
Registra as configurações e detalhes dos hábitos criados pelo usuário.

| Campo | Tipo de Dado | Restrições | Descrição |
| :--- | :--- | :--- | :--- |
| `id` | BIGINT | PK, AUTO_INCREMENT | Identificador único do hábito. |
| `id_usuario` | BIGINT | FK (`usuarios.id`), NOT NULL, ON DELETE CASCADE | Dono do hábito. |
| `nome` | VARCHAR(100) | NOT NULL | Nome descritivo do hábito (ex: 'Beber 2L de água'). |
| `icone` | VARCHAR(50) | NOT NULL | Nome do ícone para estilização do hábito na interface. |
| `cor` | VARCHAR(7) | NOT NULL | Código hexadecimal da cor escolhida pelo usuário para o hábito. |
| `id_categoria` | INT | FK (`categorias.id`), NOT NULL | Categoria à qual este hábito pertence. |
| `criado_em` | TIMESTAMP | DEFAULT NOW() | Data e hora de criação do hábito. |

#### 5.2.5. Tabela: `checkins`
Armazena o histórico diário de conclusões de cada hábito para o cálculo de métricas e streaks.

| Campo | Tipo de Dado | Restrições | Descrição |
| :--- | :--- | :--- | :--- |
| `id` | BIGINT | PK, AUTO_INCREMENT | Identificador único do check-in. |
| `id_habito` | BIGINT | FK (`habitos.id`), NOT NULL | Hábito que foi marcado como concluído. |
| `data` | DATE | NOT NULL | Data em que o hábito foi realizado. Combinação (`id_habito`, `data`) única: um registro por hábito por dia. |
| `concluido` | BOOLEAN | DEFAULT TRUE | Status da conclusão (permite marcar e desmarcar a atividade). |

#### 5.2.6. Tabela: `humores`
Registra o estado emocional diário opcional inserido pelo usuário.

| Campo | Tipo de Dado | Restrições | Descrição |
| :--- | :--- | :--- | :--- |
| `id` | BIGINT | PK, AUTO_INCREMENT | Identificador único do registro de humor. |
| `id_usuario` | BIGINT | FK (`usuarios.id`), NOT NULL, ON DELETE CASCADE | Dono do registro. |
| `data` | DATE | NOT NULL | Data do registro do humor. Combinação (`id_usuario`, `data`) única: um registro por usuário por dia. |
| `valor` | INT | NOT NULL (CHECK 1-5) | Avaliação do humor na escala de 1 (Muito Ruim) a 5 (Excelente). |
| `observacao` | TEXT | NULL | Notas, diário ou comentários opcionais sobre o dia. |

#### 5.2.7. Tabela: `lembretes`
Guarda os horários de disparo de notificações configurados pelo usuário para cada hábito.

| Campo | Tipo de Dado | Restrições | Descrição |
| :--- | :--- | :--- | :--- |
| `id` | BIGINT | PK, AUTO_INCREMENT | Identificador único do lembrete. |
| `id_habito` | BIGINT | FK (`habitos.id`), NOT NULL | Hábito associado ao lembrete. |
| `horario` | TIME | NOT NULL | Horário programado para disparar a notificação (HH:MM:SS). |
| `ativo` | BOOLEAN | DEFAULT TRUE | Define se o lembrete específico está ligado ou desligado. |

#### 5.2.8. Tabela: `log_operacoes` (RF08)
Guarda o histórico de criação, alteração e exclusão de hábitos, categorias personalizadas e check-ins. É alimentada por triggers do próprio banco. Não possui chaves estrangeiras, para que o log continue existindo depois que o registro original for excluído.

| Campo | Tipo de Dado | Restrições | Descrição |
| :--- | :--- | :--- | :--- |
| `id` | BIGINT | PK, AUTO_INCREMENT | Identificador único do registro de log. |
| `id_usuario` | BIGINT | NULL | Dono do registro afetado (sem chave estrangeira). Nos check-ins, vem do hábito. |
| `tabela` | VARCHAR(50) | NOT NULL | Nome da tabela afetada (`habitos`, `categorias` ou `checkins`). |
| `operacao` | VARCHAR(10) | NOT NULL (CHECK) | Tipo da operação: `INSERT`, `UPDATE` ou `DELETE`. |
| `id_registro` | BIGINT | NOT NULL | `id` do registro afetado na tabela de origem. |
| `dados_antigos` | JSON | NULL | Valores antes da operação (nulo em `INSERT`). |
| `dados_novos` | JSON | NULL | Valores depois da operação (nulo em `DELETE`). |
| `data_hora` | TIMESTAMP | DEFAULT NOW() | Momento em que a operação foi registrada. |

> **Nota técnica:** O MySQL não dispara triggers em exclusões feitas por `ON DELETE CASCADE`. Ao excluir um hábito, a exclusão do hábito é registrada, mas a remoção automática de seus check-ins e lembretes não aparece no log.

> **Decisão (LGPD x RF08):** quando o usuário exclui a **conta**, o log dele também é apagado. O log guarda nomes de hábitos e categorias, e a LGPD dá ao titular o direito de ter seus dados eliminados. A auditoria (RF08) vale enquanto a conta existe.

### 5.3 Script do Banco de Dados

O script completo (tabelas, restrições, categorias predefinidas, triggers de auditoria e a procedure de exclusão de conta) está em `HabitDisc/Database/script.sql`, dentro da pasta do projeto C#. Ele é compatível com MySQL 8.0.16+ e MariaDB 10.2+ (versões que aplicam `CHECK`) e pode ser executado mais de uma vez sem duplicar dados.

**Procedure `sp_excluir_conta(id_usuario)` (UC16).** Apaga, numa única transação (tudo ou nada):
1. os hábitos do usuário, e com eles, por cascata, os check-ins e lembretes. Os hábitos saem primeiro porque `habitos.id_categoria` é `RESTRICT`: as categorias personalizadas só podem ser apagadas depois dos hábitos que as usam;
2. o usuário, e com ele, por cascata, as categorias personalizadas, os humores e os pedidos de recuperação de senha;
3. o log de operações do usuário, que não tem chave estrangeira.

**Instalação no servidor:**
1. Rodar o `script.sql` no MySQL do servidor (pelo MySQL Workbench ou pela linha de comando). Ele cria o banco `habitdisc`.
2. Criar um usuário do MySQL **só para a API**, com permissão apenas no banco `habitdisc` (nunca usar o `root`).
3. Configurar a string de conexão e o servidor de e-mail **na API** (seção 6.8). Os computadores dos usuários não recebem nenhuma senha do banco.

> **Atenção:** o script cria a estrutura de um banco **novo**. Ele não altera um banco criado pela versão anterior, sem contas: como as tabelas já existem, o `IF NOT EXISTS` as ignora. Um banco antigo precisa ser apagado (`DROP DATABASE habitdisc;`) e criado de novo, o que **apaga todos os dados dele**.

> **Índices (RNF05):** com contas, as consultas de check-in sempre partem dos hábitos de um usuário (`id_habito`), e o índice único `(id_habito, data)` atende a todas elas. Hábitos, humores e categorias têm índice em `id_usuario`.

### 5.4 Preferências Locais (fora do banco)

As preferências de aparência (RF09) **não ficam no banco** e **não pertencem à conta**: são uma configuração da instalação, e não dados de hábito. Quem entrar com outra conta no mesmo computador vê o mesmo tema. Ficam em um arquivo JSON do usuário do Windows:

`%AppData%\HabitDisc\preferencias.json` (ex.: `C:\Users\<usuário>\AppData\Roaming\HabitDisc\preferencias.json`)

| Campo | Valores | Padrão |
| :--- | :--- | :--- |
| `Paleta` | `Verde`, `Azul`, `Roxo`, `Vinho`, `Grafite` | `Verde` |
| `TamanhoFonte` | `Pequena`, `Media`, `Grande` | `Media` |
| `ModoEscuro` | `true` / `false` | `false` |

Se o arquivo não existir ou estiver corrompido, o aplicativo abre com os valores padrão.

---

## 6. Requisitos Técnicos

### 6.1 Plataforma

| Item | Definição |
| :--- | :--- |
| Linguagem | C# |
| Interface | Windows Forms |
| Runtime | .NET 10 (LTS), alvo `net10.0-windows` (confirmar a versão instalada no ambiente de desenvolvimento) |
| Sistema operacional | Windows 10 ou superior |

### 6.2 Banco de Dados e ORM

- **Banco:** MySQL 8.x em um servidor compartilhado, acessado somente pela API (seção 6.8).
- **ORM:** Entity Framework Core, com o provedor Pomelo para MySQL.
- **Abordagem de mapeamento:** Database First. O banco é criado pelo script SQL (seção 5.3) e as classes de `Models` são mapeadas para as tabelas existentes com Data Annotations (`[Table]`, `[Key]`, `[Column]`).

**Compatibilidade de versões (verificada em outubro de 2026).** O provedor Pomelo ainda não possui versão para o EF Core 10: a última versão publicada é a 9.0.0, que aceita apenas o EF Core 9.x. Misturar o Pomelo 9 com pacotes EF Core 10 gera erro de restauração (NU1107) ou aviso (NU1608) e falha em tempo de execução (`MissingMethodException`). Opções:

| Opção | Pacote de banco | Pacotes EF Core | Observação |
| :--- | :--- | :--- | :--- |
| A (em uso) | `Pomelo.EntityFrameworkCore.MySql` 9.0.0 | Todos em 9.0.x (`Microsoft.EntityFrameworkCore`, `.Design`, `.Tools`) | Funciona em projetos `net10.0`. O suporte do EF Core 9 (STS) terminou em 10/11/2026: serve como ponte até o Pomelo lançar a versão 10. |
| B | `Microting.EntityFrameworkCore.MySql` 10.x | Todos em 10.0.x | Fork comunitário do Pomelo, com namespaces renomeados (`Microting.*`). |
| C | `MySql.EntityFrameworkCore` 10.x (Oracle) | Todos em 10.0.x | Provedor diferente, com API e tradução de consultas diferentes (`UseMySQL`). |

Regras para qualquer opção:
- Manter **todos** os pacotes `Microsoft.EntityFrameworkCore.*` na mesma versão major do provedor.
- Não suprimir o aviso NU1608 (`NoWarn`): o projeto compila e falha ao iniciar.
- Manter a ferramenta `dotnet-ef` na mesma major dos pacotes do projeto.
- Verificar a compatibilidade novamente antes de atualizar qualquer pacote.

**Situação atual (16/11/2026):** o projeto usa a opção A (`Pomelo.EntityFrameworkCore.MySql` 9.0.0 e `Microsoft.EntityFrameworkCore` 9.0.23). Pendências:
- Decidir se o projeto continua na opção A (fora do suporte desde 10/11/2026) ou passa para a B.
- O build mostra o aviso NU1902: o pacote `MySqlConnector` 2.4.0, usado pelo Pomelo, tem uma vulnerabilidade moderada conhecida. A correção é referenciar diretamente uma versão corrigida do `MySqlConnector`.

**Uso do DbContext nos Services:** cada método de um Service cria o próprio `HabitDiscContext` com `using` e o descarta ao terminar (contexto de vida curta). Assim nenhum dado fica carregado entre uma operação e outra, e ao excluir um hábito quem apaga os check-ins e lembretes é sempre o `ON DELETE CASCADE` do banco (comportamento descrito na nota da seção 5.2.8). Com a API, essa regra passa a valer para os Services da API.

### 6.3 Arquitetura da Interface (Navegação)

Decisões tomadas para a versão desktop:

1. Navegação por **Panel + UserControl**, no lugar do MdiContainer.
2. O `MainForm` é uma "casca" fixa: painel de navegação à esquerda e painel de conteúdo ocupando o restante.
3. A tela **Home** é carregada ao iniciar.
4. A lógica de troca de tela fica em métodos do próprio `MainForm`, que é o dono do painel de conteúdo. Não são usadas `IUserControl` nem classes `...Services` para isso.
5. Os parâmetros desses métodos são do tipo `UserControl`, para aceitar qualquer tela (polimorfismo).
6. As telas são criadas e **reaproveitadas**, em vez de uma instância nova a cada clique.
7. O `MainForm` guarda a tela exibida em um campo (`_ucSelecionado`) e o botão destacado do menu. Na primeira troca o campo ainda é `null`, e o método de troca trata esse caso.
8. Todas as telas são registradas uma única vez no painel de conteúdo (`Dock = Fill`, inicialmente invisíveis). A troca alterna a propriedade `Visible` e usa `BringToFront`. Como as telas ficam dentro do painel, o WinForms as descarta sozinho ao fechar o form.
9. Cada tela atualiza seus próprios dados quando fica visível (evento `VisibleChanged`). O `MainForm` não chama método de atualização, o que mantém o parâmetro do tipo `UserControl` sem precisar de interface para isso.
10. A Home é exibida no evento `Shown` do `MainForm`, quando o form já está na tela.
11. Todas as telas herdam de `TelaBase` (que herda de `UserControl`). A `TelaBase` escuta, uma única vez, o evento de troca de tema (`Tema.Alterado`). Cada tela só escreve o seu método `AplicarTema()` com `protected override`. A `TelaBase` não é `abstract` porque o designer do Visual Studio não abre telas que herdam de classe abstrata.
12. O botão **Configurações** fica no rodapé do menu lateral (`Dock = Bottom`), separado das seções principais.
13. Criar, editar e ver detalhes acontecem em **janelas de diálogo** (`ShowDialog`), e não dentro do painel de conteúdo. Todas herdam de `FormDialogoBase`: tamanho automático, centralizadas na janela principal e com as cores do tema. A janela recebe o Service de quem a abriu, em vez de criar o próprio.
14. Ao voltar de um diálogo, a tela recarrega os dados do banco.

Telas do menu lateral:

| Tela | Casos de uso atendidos |
| :--- | :--- |
| Home | UC02 (check-in do dia e progresso) |
| Hábitos | UC01, UC03, UC07, UC08 |
| Estatísticas | UC04 |
| Humor | UC05 |
| Categorias | UC09 |
| Configurações (rodapé) | UC10 |

O UC06 (notificação) é executado pelo sistema e não possui tela própria.

Janelas de diálogo:

| Janela | Aberta por | Casos de uso |
| :--- | :--- | :--- |
| `FormHabito` (novo / editar hábito) | Hábitos, Detalhes do hábito | UC01, UC07 |
| `FormDetalhesHabito` | Clique em um hábito na tela Hábitos | UC03, UC07, UC08 |
| `FormCategoria` (nova / editar categoria) | Categorias | UC09 |

### 6.4 Estrutura de Camadas

Estrutura de pastas do projeto:

```
HabitDisc/
├── Forms/
│   ├── MainForm.cs              (casca: menu lateral + painel de conteúdo)
│   ├── FormDialogoBase.cs       (base das janelas de diálogo)
│   ├── FormHabito.cs
│   ├── FormDetalhesHabito.cs
│   └── FormCategoria.cs
├── Telas/
│   ├── TelaBase.cs              (base de todas as telas: tema)
│   ├── HomeControl.cs
│   ├── HabitosControl.cs
│   ├── EstatisticasControl.cs
│   ├── HumorControl.cs
│   ├── CategoriasControl.cs
│   ├── ConfiguracoesControl.cs
│   └── Componentes/
│       ├── LinhaItemControl.cs  (linha de lista: faixa, ícone, título, ações)
│       ├── SeletorIcone.cs
│       ├── SeletorCor.cs
│       ├── CartaoNumero.cs      (cartão com número em destaque)
│       └── GraficoBarras.cs
├── Models/
│   ├── Categoria.cs
│   ├── Habito.cs
│   ├── Checkin.cs
│   ├── Humor.cs
│   └── Lembrete.cs
├── Data/
│   └── HabitDiscContext.cs
├── Services/
│   ├── HabitoService.cs
│   ├── CheckinService.cs
│   ├── CategoriaService.cs
│   ├── HumorService.cs
│   ├── EstatisticaService.cs
│   ├── PreferenciasService.cs
│   ├── HabitoResumo.cs, HabitoDetalhes.cs, Preferencias.cs   (dados prontos para as telas)
│   ├── Validacao.cs
│   └── RegraNegocioException.cs
├── Helpers/
│   ├── Tema.cs                  (cores e fontes atuais)
│   ├── Estilo.cs                (estilos reaproveitados: títulos, botões, caixas)
│   ├── BarraDeTitulo.cs         (barra de título escura no modo escuro)
│   ├── CorHelper.cs, IconeHelper.cs, Textos.cs, Mensagem.cs, ListaVertical.cs
├── Database/
│   └── script.sql
├── Resources/
├── appsettings.example.json
└── Program.cs
```

| Pasta | Responsabilidade |
| :--- | :--- |
| `Forms` | Janelas: o `MainForm` (casca com menu e painel de conteúdo) e as janelas de diálogo. |
| `Telas` | UserControls, um por seção do menu, trocados dentro do painel do `MainForm`. |
| `Telas/Componentes` | Pedaços de tela reaproveitados por várias telas e janelas. |
| `Models` | Classes de dados puras, sem lógica, mapeadas para as tabelas com Data Annotations. |
| `Data` | O `DbContext` do EF Core, que dispensa uma camada de repositórios. |
| `Services` | Regras de negócio e acesso aos dados (ex.: criação de hábito, check-in, cálculo do streak, estatísticas). Também guarda as classes de dados que os Services entregam prontas às telas (`HabitoResumo`, `HabitoDetalhes`), que não são tabelas. |
| `Helpers` | Utilitários pequenos, sem regra de negócio (tema, estilos, textos, conversões). Podem ser usados por qualquer camada. |
| `Resources` | Ícones e fontes. |
| `Database` | Script SQL do banco. |

**Regra de dependência:** as chamadas seguem um único sentido: **Telas / Forms → Services → Data → Models**. Uma tela chama um Service, e o Service usa o `DbContext`. Telas não acessam o `DbContext` diretamente, e `Models` não depende de nenhuma outra pasta.

O `MainForm` cuida apenas da navegação e não contém regras de negócio.

#### Regras de negócio e tratamento de erros

- As validações ficam nos Services, e não nas telas. Exemplos: nome obrigatório e tamanho máximo, cor no formato `#RRGGBB`, categoria predefinida bloqueada, categoria com hábitos não pode ser excluída, humor de 1 a 5.
- Quando uma regra é violada, o Service lança `RegraNegocioException` com uma mensagem já escrita para o usuário. A tela mostra essa mensagem como **aviso**, e a janela de cadastro continua aberta para correção.
- Qualquer outro erro (ex.: MySQL desligado) é capturado pelo tratamento global do `Program` (`Application.ThreadException`) e mostrado como **erro**, sem fechar o aplicativo.
- Na Home, se o check-in não puder ser gravado, a caixinha volta ao estado anterior, para a tela nunca mostrar algo que não foi salvo.

#### Tema e aparência (RF09)

- `Helpers/Tema.cs` guarda as cores e fontes **atuais**. Nenhuma tela usa cor fixa: todas leem do `Tema`.
- Ao mudar uma opção em Configurações, o `Tema` dispara o evento `Alterado`, e todas as telas se repintam na hora (veja a `TelaBase`, item 11 da seção 6.3).
- O tamanho da fonte multiplica fontes e alturas por 0,9 (pequena), 1,0 (média) ou 1,2 (grande). As alturas são calculadas pela altura real da fonte, para o texto caber também quando o Windows usa escala de tela maior que 100%.
- No modo escuro, a barra de título da janela também fica escura (`Helpers/BarraDeTitulo.cs`, via API do Windows).
- Para oferecer uma nova cor de tema, basta acrescentar uma paleta em `Tema.Paletas`. Para um novo ícone de hábito/categoria, basta acrescentá-lo em `IconeHelper`.

### 6.5 Gráficos

**Decisão:** o gráfico da tela de Estatísticas é desenhado pelo próprio projeto (`Telas/Componentes/GraficoBarras.cs`, com GDI+), **sem biblioteca externa**.

Motivos:
- O gráfico necessário é simples: barras de 0% a 100%, uma por dia, com 7 (semana) ou até 31 (mês) colunas.
- Não acrescenta dependência ao projeto, o que segue a regra da seção 6.2 de verificar cada pacote.
- As cores vêm do `Tema`, então o gráfico funciona no modo escuro e com qualquer paleta.

Se no futuro forem necessários gráficos mais complexos (linhas, zoom, legendas interativas), a alternativa recomendada é o ScottPlot.

### 6.6 Regras de Cálculo

**Streak (UC03, UC08).** É calculado a partir dos check-ins concluídos e nunca é gravado (seção 5.1).
- **Sequência atual:** dias seguidos com check-in, terminando hoje. Se o hábito ainda não foi feito hoje, a sequência não "quebra" até o dia acabar: a contagem começa por ontem. Se também não foi feito ontem, a sequência é 0 (fluxo alternativo do UC03).
- **Melhor sequência (recorde):** a maior sequência de dias seguidos em todo o histórico.
- "Hoje" é a data do relógio do Windows no momento do cálculo. Nenhuma rotina roda à meia-noite.

**Estatísticas (UC04).**
- **Taxa de conclusão** = check-ins concluídos ÷ check-ins esperados no período.
- **Esperados** = para cada hábito, os dias do período a partir do dia em que ele foi criado, contando só os dias que já passaram. Assim, um hábito criado na quarta não perde segunda e terça, e uma semana em andamento não parece ruim só porque ainda não terminou.
- **Períodos:** a semana vai de segunda a domingo; o mês vai do dia 1 ao último dia.
- **Comparativo:** diferença, em pontos percentuais, entre a taxa do período e a do período anterior (ex.: 72% − 64% = ▲ 8 pts).
- **Dados suficientes:** as estatísticas só aparecem quando o primeiro check-in tem pelo menos 7 dias. Antes disso, a tela mostra uma mensagem (fluxo alternativo do UC04).

### 6.7 Situação da Implementação (16/11/2026)

| Item | Situação |
| :--- | :--- |
| UC01, UC02, UC03, UC04, UC05, UC07, UC08, UC09 | Implementados no aplicativo, ainda **conectando direto ao banco e sem contas** (estrutura anterior do banco). Precisam passar a usar a API (seção 6.8) |
| Script do banco com contas (seção 5.3) | Pronto e testado. **O código atual ainda não é compatível com ele** (ex.: `habitos.id_usuario` é obrigatório) |
| API, contas e recuperação de senha (UC11 a UC16, RF10, RF11) | **Pendente** (plano na seção 6.8) |
| UC10 / RF09 (aparência) | Implementado |
| RF08 (log) | Implementado no banco (triggers do `script.sql`) |
| UC06 / RF03 (lembretes e notificações) | **Pendente:** tela de cadastro de lembretes, `NotificacaoService` e resumo semanal |
| Índice em `checkins(data)` (RNF05) | **Pendente** (seção 5.3) |
| Versão do EF Core e aviso NU1902 | **Pendente** (seção 6.2) |
| Testes automatizados | **Pendente:** os Services foram testados com um programa temporário, fora do repositório; falta um projeto de testes dentro dele |
| Diagramas UML (seção 4) | **Pendente** |
| Fonte Newsreader | **Pendente:** precisa estar instalada no computador; sem ela o Windows usa outra fonte |

### 6.8 Arquitetura com API e Contas de Usuário

**Decisão:** com o banco em um servidor compartilhado, o aplicativo **não** se conecta direto ao MySQL. Entre os dois fica uma **API ASP.NET Core**:

```
Aplicativo WinForms  ──HTTPS──▶  API ASP.NET Core  ──▶  MySQL (servidor)
(telas)                          (Services, regras,       (só a API acessa)
                                  DbContext, e-mail)
```

**Por quê:** se o aplicativo se conectasse direto, todo computador com ele instalado teria a senha do banco (e a do e-mail, para a recuperação de senha). Quem a encontrasse leria os dados de todos os usuários sem passar pelo login. Com a API, essas senhas ficam só no servidor, e a API decide o que cada usuário pode ver (RNF03, RNF06). Uma vantagem extra: a mesma API pode atender as futuras versões web e mobile.

**O que muda em cada lado:**

| Lado | Responsabilidade |
| :--- | :--- |
| **API** (projeto novo) | `Models`, `Data/HabitDiscContext`, as regras de negócio (os Services atuais migram para cá), hash de senha, login e token, envio de e-mail, filtro por usuário em toda consulta. |
| **Aplicativo** | Telas e chamadas à API (os Services do aplicativo passam a fazer requisições HTTP em vez de usar o `DbContext`). Guarda o token só na memória enquanto o usuário está logado. Tema e preferências continuam locais (seção 5.4). |

**Hash de senha — `Helpers/HashHelper.cs` (na API):**
- `GerarHash(string senha)`: gera um *salt* aleatório e calcula o hash com **PBKDF2** (`Rfc2898DeriveBytes.Pbkdf2`, já incluído no .NET). Devolve um texto com algoritmo, número de iterações, salt e hash, que é o que vai para `usuarios.senha_hash`.
- `Verificar(string senha, string hashSalvo)`: recalcula o hash da senha digitada com o mesmo salt e iterações do hash salvo e compara os dois em tempo constante (`CryptographicOperations.FixedTimeEquals`).
- Fica na **API**, e não no aplicativo: o aplicativo envia a senha por HTTPS e a API calcula o hash. Se o aplicativo enviasse o hash pronto, o hash viraria a própria senha.
- Por causa do salt, a mesma senha gera hashes diferentes a cada vez. Por isso se usa `Verificar`, e não uma comparação entre dois hashes.
- O mesmo helper gera o hash do código de recuperação de senha (`recuperacoes_senha.codigo_hash`).

**Autenticação:**
- No login (UC12), a API confere a senha com `HashHelper.Verificar` e devolve um **token de acesso** (ex.: JWT), com validade limitada (ex.: 12 horas).
- O aplicativo envia o token em toda requisição. A API descobre **pelo token** qual é o usuário e usa esse id em todas as consultas, nunca um id enviado pelo aplicativo (RNF06).
- Logout (UC13): o aplicativo descarta o token.

**Regras das contas:**
- E-mail: obrigatório, formato válido, gravado em minúsculas, único.
- Senha: no mínimo 8 caracteres; trocar senha ou e-mail e excluir a conta exigem a senha atual (UC15, UC16).
- Mensagens que não revelam quais e-mails têm conta: login (UC12) e recuperação de senha (UC14).

**Recuperação de senha (UC14):** código de 6 dígitos gerado com `RandomNumberGenerator`, guardado como hash, válido por 15 minutos, no máximo 5 tentativas e de uso único. Um novo pedido invalida os anteriores. O e-mail é enviado pela API, com as credenciais do servidor de e-mail (SMTP) guardadas só nela.

**Datas e fuso horário:**
- As datas de check-in e de humor (`DATE`) são a **data local do usuário**, enviada pelo aplicativo.
- Momentos (`TIMESTAMP`/`DATETIME`, como `criado_em` e `expira_em`) são tratados em **UTC** pela API. Assim, o servidor pode ficar em qualquer fuso.

**Ordem de implementação sugerida:**
1. Criar o projeto da API e mover para ele `Models`, `Data` e as regras dos Services.
2. `HashHelper`, cadastro, login e token (UC11, UC12).
3. Filtrar todas as consultas pelo usuário do token, com testes que confirmem que um usuário não vê os dados de outro.
4. Trocar os Services do aplicativo por chamadas à API.
5. Telas de entrada, cadastro, "Minha conta" e logout (UC13, UC15, UC16).
6. Recuperação de senha com envio de e-mail (UC14).

<!--
ATENÇÃO GRUPO: Decisão sobre Criptografia do Nome do Hábito

Por que NÃO criptografamos o nome do hábito diretamente no banco de dados (via código)?
 - Destruição do Desempenho (RNF01): O banco de dados perderia a capacidade de usar índices de texto. Consultas simples como `WHERE nome LIKE '%agua%'` deixariam de funcionar.
 - Complexidade Inútil: Para ordenar os hábitos por ordem alfabética, o aplicativo teria que carregar todos os registros do banco, descriptografar um por um na memória e só então ordenar. Isso inviabilizaria o app com histórico grande.

Como garantimos a privacidade e conformidade com a LGPD (RNF03) sem perder desempenho?
 1. Criptografia em Repouso (Encryption at Rest): Ativada diretamente no servidor do banco de dados (MySQL). Se alguém roubar o HD, os dados estarão ilegíveis.
 2. Conexão protegida: Se o MySQL estiver em outra máquina, usar conexão com SSL/TLS. Se estiver na própria máquina, não há tráfego de rede a proteger.
 3. Sem dados de identificação: A versão desktop não guarda nome nem e-mail, então os hábitos não ficam associados a uma pessoa identificável.

Comentario gerado com IA. Estava discutindo sobre a privacidade do usuario quanto aos nomes de um habito (as vezes, um usuario pode nao querer mostrar o que ele precisa melhorar)
-- Lorenzo
(Adaptado para a versão desktop.)
-->