-- =====================================================================
-- HabitDisc Desktop - Script do banco de dados
-- Versão com contas de usuário: vários usuários em um servidor
-- compartilhado, acessado SOMENTE pela API (docs, seção 6.8).
--
-- Compatível com MySQL 8.0.16+ e MariaDB 10.2+ (versões que aplicam CHECK).
-- Pode ser executado mais de uma vez: usa IF NOT EXISTS, recria as
-- triggers e a procedure e só insere as categorias predefinidas se
-- ainda não existirem.
--
-- ATENÇÃO: este script cria a estrutura de um banco NOVO. Ele não altera
-- um banco criado pela versão anterior (sem contas): como as tabelas já
-- existem, o IF NOT EXISTS as ignora. Um banco antigo precisa ser apagado
-- e criado de novo (docs, seção 5.3).
--
-- Estrutura baseada no dicionário de dados da documentação (seção 5.2).
-- =====================================================================

CREATE DATABASE IF NOT EXISTS habitdisc
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE habitdisc;

SET NAMES utf8mb4;

-- ---------------------------------------------------------------------
-- Tabela: usuarios
-- A senha NUNCA é guardada: só o hash calculado pela API (HashHelper).
-- O e-mail é gravado em minúsculas pela API e é único.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS usuarios (
    id             BIGINT        NOT NULL AUTO_INCREMENT,
    nome           VARCHAR(100)  NOT NULL,
    email          VARCHAR(254)  NOT NULL,
    senha_hash     VARCHAR(255)  NOT NULL,
    criado_em      TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em  TIMESTAMP     NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    UNIQUE KEY uq_usuarios_email (email)
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- Tabela: recuperacoes_senha
-- Um pedido de recuperação de senha: o código enviado por e-mail é
-- guardado como hash, vale por pouco tempo, tem limite de tentativas
-- e só pode ser usado uma vez. Apagado junto com o usuário (CASCADE).
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS recuperacoes_senha (
    id           BIGINT        NOT NULL AUTO_INCREMENT,
    id_usuario   BIGINT        NOT NULL,
    codigo_hash  VARCHAR(255)  NOT NULL,
    expira_em    DATETIME      NOT NULL,
    tentativas   INT           NOT NULL DEFAULT 0,
    usado        BOOLEAN       NOT NULL DEFAULT FALSE,
    criado_em    TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    KEY idx_recuperacoes_usuario (id_usuario),
    CONSTRAINT fk_recuperacoes_usuario
        FOREIGN KEY (id_usuario) REFERENCES usuarios (id)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- Tabela: categorias
-- Predefinidas: predefinida = TRUE e id_usuario NULL (de todos).
-- Personalizadas: predefinida = FALSE e id_usuario do dono.
-- O nome é único por usuário (a collation não diferencia maiúsculas).
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS categorias (
    id           INT          NOT NULL AUTO_INCREMENT,
    id_usuario   BIGINT       NULL,
    nome         VARCHAR(50)  NOT NULL,
    icone        VARCHAR(50)  NOT NULL,
    cor          VARCHAR(7)   NOT NULL,
    predefinida  BOOLEAN      NOT NULL DEFAULT FALSE,
    PRIMARY KEY (id),
    UNIQUE KEY uq_categorias_usuario_nome (id_usuario, nome),
    CONSTRAINT fk_categorias_usuario
        FOREIGN KEY (id_usuario) REFERENCES usuarios (id)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- Tabela: habitos
-- Excluir uma categoria com hábitos vinculados é bloqueado (RESTRICT)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS habitos (
    id            BIGINT       NOT NULL AUTO_INCREMENT,
    id_usuario    BIGINT       NOT NULL,
    nome          VARCHAR(100) NOT NULL,
    icone         VARCHAR(50)  NOT NULL,
    cor           VARCHAR(7)   NOT NULL,
    id_categoria  INT          NOT NULL,
    criado_em     TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    KEY idx_habitos_usuario (id_usuario),
    CONSTRAINT fk_habitos_usuario
        FOREIGN KEY (id_usuario) REFERENCES usuarios (id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT fk_habitos_categoria
        FOREIGN KEY (id_categoria) REFERENCES categorias (id)
        ON DELETE RESTRICT
        ON UPDATE CASCADE
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- Tabela: checkins
-- Um registro por hábito por dia. Ao excluir o hábito, seus check-ins
-- são removidos junto (CASCADE).
-- O índice único (id_habito, data) também acelera as consultas de
-- streak e estatísticas (RNF05).
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS checkins (
    id         BIGINT   NOT NULL AUTO_INCREMENT,
    id_habito  BIGINT   NOT NULL,
    `data`     DATE     NOT NULL,
    concluido  BOOLEAN  NOT NULL DEFAULT TRUE,
    PRIMARY KEY (id),
    UNIQUE KEY uq_checkins_habito_data (id_habito, `data`),
    CONSTRAINT fk_checkins_habito
        FOREIGN KEY (id_habito) REFERENCES habitos (id)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- Tabela: humores
-- Um registro por usuário por dia, valor de 1 a 5
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS humores (
    id          BIGINT  NOT NULL AUTO_INCREMENT,
    id_usuario  BIGINT  NOT NULL,
    `data`      DATE    NOT NULL,
    valor       INT     NOT NULL,
    observacao  TEXT    NULL,
    PRIMARY KEY (id),
    UNIQUE KEY uq_humores_usuario_data (id_usuario, `data`),
    CONSTRAINT chk_humores_valor CHECK (valor BETWEEN 1 AND 5),
    CONSTRAINT fk_humores_usuario
        FOREIGN KEY (id_usuario) REFERENCES usuarios (id)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- Tabela: lembretes
-- Ao excluir o hábito, seus lembretes são removidos junto (CASCADE)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS lembretes (
    id         BIGINT   NOT NULL AUTO_INCREMENT,
    id_habito  BIGINT   NOT NULL,
    horario    TIME     NOT NULL,
    ativo      BOOLEAN  NOT NULL DEFAULT TRUE,
    PRIMARY KEY (id),
    CONSTRAINT fk_lembretes_habito
        FOREIGN KEY (id_habito) REFERENCES habitos (id)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- Tabela: log_operacoes (RF08)
-- Histórico de criação, alteração e exclusão de hábitos, categorias
-- personalizadas e check-ins. Sem chaves estrangeiras de propósito:
-- o log continua existindo depois que o registro for excluído.
-- id_usuario diz de quem era o registro. Quando a CONTA é excluída,
-- o log dela também é apagado (LGPD), pela procedure sp_excluir_conta.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS log_operacoes (
    id             BIGINT       NOT NULL AUTO_INCREMENT,
    id_usuario     BIGINT       NULL,
    tabela         VARCHAR(50)  NOT NULL,
    operacao       VARCHAR(10)  NOT NULL,
    id_registro    BIGINT       NOT NULL,
    dados_antigos  JSON         NULL,
    dados_novos    JSON         NULL,
    data_hora      TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    KEY idx_log_usuario (id_usuario),
    KEY idx_log_tabela_registro (tabela, id_registro),
    KEY idx_log_data_hora (data_hora),
    CONSTRAINT chk_log_operacao CHECK (operacao IN ('INSERT', 'UPDATE', 'DELETE'))
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- Dados iniciais: categorias predefinidas (de todos os usuários)
-- (só insere se ainda não existir nenhuma predefinida)
-- ---------------------------------------------------------------------
INSERT INTO categorias (id_usuario, nome, icone, cor, predefinida)
SELECT NULL, seed.nome, seed.icone, seed.cor, seed.predefinida
FROM (
    SELECT 'Saúde'           AS nome, 'heart'        AS icone, '#468CC8' AS cor, TRUE AS predefinida
    UNION ALL
    SELECT 'Produtividade',           'circle-check',          '#5FAE7A',         TRUE
    UNION ALL
    SELECT 'Aprendizado',             'book',                  '#966EBE',         TRUE
    UNION ALL
    SELECT 'Relacionamentos',         'users',                 '#D2646E',         TRUE
    UNION ALL
    SELECT 'Finanças',                'wallet',                '#DCA03C',         TRUE
) AS seed
WHERE NOT EXISTS (SELECT 1 FROM categorias WHERE predefinida = TRUE);

-- ---------------------------------------------------------------------
-- Triggers de auditoria (RF08)
-- Observação: o MySQL NÃO dispara triggers em exclusões feitas por
-- CASCADE. Ao excluir um hábito, o DELETE do hábito é registrado, mas
-- a remoção automática dos seus check-ins e lembretes não aparece no log.
-- ---------------------------------------------------------------------
DELIMITER $$

-- ===== habitos =====
DROP TRIGGER IF EXISTS trg_habitos_ai$$
CREATE TRIGGER trg_habitos_ai AFTER INSERT ON habitos
FOR EACH ROW
BEGIN
    INSERT INTO log_operacoes (id_usuario, tabela, operacao, id_registro, dados_antigos, dados_novos)
    VALUES (NEW.id_usuario, 'habitos', 'INSERT', NEW.id, NULL,
            JSON_OBJECT('nome', NEW.nome, 'icone', NEW.icone, 'cor', NEW.cor,
                        'id_categoria', NEW.id_categoria));
END$$

DROP TRIGGER IF EXISTS trg_habitos_au$$
CREATE TRIGGER trg_habitos_au AFTER UPDATE ON habitos
FOR EACH ROW
BEGIN
    INSERT INTO log_operacoes (id_usuario, tabela, operacao, id_registro, dados_antigos, dados_novos)
    VALUES (NEW.id_usuario, 'habitos', 'UPDATE', NEW.id,
            JSON_OBJECT('nome', OLD.nome, 'icone', OLD.icone, 'cor', OLD.cor,
                        'id_categoria', OLD.id_categoria),
            JSON_OBJECT('nome', NEW.nome, 'icone', NEW.icone, 'cor', NEW.cor,
                        'id_categoria', NEW.id_categoria));
END$$

DROP TRIGGER IF EXISTS trg_habitos_ad$$
CREATE TRIGGER trg_habitos_ad AFTER DELETE ON habitos
FOR EACH ROW
BEGIN
    INSERT INTO log_operacoes (id_usuario, tabela, operacao, id_registro, dados_antigos, dados_novos)
    VALUES (OLD.id_usuario, 'habitos', 'DELETE', OLD.id,
            JSON_OBJECT('nome', OLD.nome, 'icone', OLD.icone, 'cor', OLD.cor,
                        'id_categoria', OLD.id_categoria),
            NULL);
END$$

-- ===== categorias (somente as personalizadas) =====
DROP TRIGGER IF EXISTS trg_categorias_ai$$
CREATE TRIGGER trg_categorias_ai AFTER INSERT ON categorias
FOR EACH ROW
BEGIN
    IF NEW.predefinida = FALSE THEN
        INSERT INTO log_operacoes (id_usuario, tabela, operacao, id_registro, dados_antigos, dados_novos)
        VALUES (NEW.id_usuario, 'categorias', 'INSERT', NEW.id, NULL,
                JSON_OBJECT('nome', NEW.nome, 'icone', NEW.icone, 'cor', NEW.cor));
    END IF;
END$$

DROP TRIGGER IF EXISTS trg_categorias_au$$
CREATE TRIGGER trg_categorias_au AFTER UPDATE ON categorias
FOR EACH ROW
BEGIN
    IF OLD.predefinida = FALSE THEN
        INSERT INTO log_operacoes (id_usuario, tabela, operacao, id_registro, dados_antigos, dados_novos)
        VALUES (NEW.id_usuario, 'categorias', 'UPDATE', NEW.id,
                JSON_OBJECT('nome', OLD.nome, 'icone', OLD.icone, 'cor', OLD.cor),
                JSON_OBJECT('nome', NEW.nome, 'icone', NEW.icone, 'cor', NEW.cor));
    END IF;
END$$

DROP TRIGGER IF EXISTS trg_categorias_ad$$
CREATE TRIGGER trg_categorias_ad AFTER DELETE ON categorias
FOR EACH ROW
BEGIN
    IF OLD.predefinida = FALSE THEN
        INSERT INTO log_operacoes (id_usuario, tabela, operacao, id_registro, dados_antigos, dados_novos)
        VALUES (OLD.id_usuario, 'categorias', 'DELETE', OLD.id,
                JSON_OBJECT('nome', OLD.nome, 'icone', OLD.icone, 'cor', OLD.cor),
                NULL);
    END IF;
END$$

-- ===== checkins =====
-- O check-in não tem id_usuario: ele vem do hábito
DROP TRIGGER IF EXISTS trg_checkins_ai$$
CREATE TRIGGER trg_checkins_ai AFTER INSERT ON checkins
FOR EACH ROW
BEGIN
    INSERT INTO log_operacoes (id_usuario, tabela, operacao, id_registro, dados_antigos, dados_novos)
    VALUES ((SELECT id_usuario FROM habitos WHERE id = NEW.id_habito),
            'checkins', 'INSERT', NEW.id, NULL,
            JSON_OBJECT('id_habito', NEW.id_habito, 'data', NEW.`data`,
                        'concluido', NEW.concluido));
END$$

DROP TRIGGER IF EXISTS trg_checkins_au$$
CREATE TRIGGER trg_checkins_au AFTER UPDATE ON checkins
FOR EACH ROW
BEGIN
    INSERT INTO log_operacoes (id_usuario, tabela, operacao, id_registro, dados_antigos, dados_novos)
    VALUES ((SELECT id_usuario FROM habitos WHERE id = NEW.id_habito),
            'checkins', 'UPDATE', NEW.id,
            JSON_OBJECT('id_habito', OLD.id_habito, 'data', OLD.`data`,
                        'concluido', OLD.concluido),
            JSON_OBJECT('id_habito', NEW.id_habito, 'data', NEW.`data`,
                        'concluido', NEW.concluido));
END$$

DROP TRIGGER IF EXISTS trg_checkins_ad$$
CREATE TRIGGER trg_checkins_ad AFTER DELETE ON checkins
FOR EACH ROW
BEGIN
    INSERT INTO log_operacoes (id_usuario, tabela, operacao, id_registro, dados_antigos, dados_novos)
    VALUES ((SELECT id_usuario FROM habitos WHERE id = OLD.id_habito),
            'checkins', 'DELETE', OLD.id,
            JSON_OBJECT('id_habito', OLD.id_habito, 'data', OLD.`data`,
                        'concluido', OLD.concluido),
            NULL);
END$$

-- ---------------------------------------------------------------------
-- Procedure: exclusão de conta (UC16)
-- Apaga TODOS os dados do usuário, em uma transação (tudo ou nada):
-- 1. os hábitos (e, por CASCADE, os check-ins e lembretes deles).
--    Vêm antes porque habitos.id_categoria é RESTRICT: as categorias
--    personalizadas só podem sair depois dos hábitos que as usam;
-- 2. o usuário (e, por CASCADE, as categorias personalizadas, os
--    humores e os pedidos de recuperação de senha);
-- 3. o log de operações do usuário (LGPD), que não tem chave estrangeira.
-- Uso pela API: CALL sp_excluir_conta(id_do_usuario);
-- ---------------------------------------------------------------------
DROP PROCEDURE IF EXISTS sp_excluir_conta$$
CREATE PROCEDURE sp_excluir_conta(IN p_id_usuario BIGINT)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;
    DELETE FROM habitos WHERE id_usuario = p_id_usuario;
    DELETE FROM usuarios WHERE id = p_id_usuario;
    DELETE FROM log_operacoes WHERE id_usuario = p_id_usuario;
    COMMIT;
END$$

DELIMITER ;
