-- =====================================================================
-- HabitDisc Desktop - Script do banco de dados
-- Versão de usuário único (sem usuario, usuario_categoria e dispositivo)
--
-- Compatível com MySQL 8.x e MariaDB 10.x.
-- Pode ser executado mais de uma vez: usa IF NOT EXISTS, recria as
-- triggers e só insere as categorias predefinidas se ainda não existirem.
--
-- Estrutura baseada no dicionário de dados da documentação (seção 5.2).
-- =====================================================================

CREATE DATABASE IF NOT EXISTS habitdisc
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE habitdisc;

SET NAMES utf8mb4;

-- ---------------------------------------------------------------------
-- Tabela: categorias
-- Categorias predefinidas (predefinida = TRUE) e personalizadas (FALSE)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS categorias (
    id           INT          NOT NULL AUTO_INCREMENT,
    nome         VARCHAR(50)  NOT NULL,
    icone        VARCHAR(50)  NOT NULL,
    cor          VARCHAR(7)   NOT NULL,
    predefinida  BOOLEAN      NOT NULL DEFAULT FALSE,
    PRIMARY KEY (id)
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- Tabela: habitos
-- Excluir uma categoria com hábitos vinculados é bloqueado (RESTRICT)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS habitos (
    id            BIGINT       NOT NULL AUTO_INCREMENT,
    nome          VARCHAR(100) NOT NULL,
    icone         VARCHAR(50)  NOT NULL,
    cor           VARCHAR(7)   NOT NULL,
    id_categoria  INT          NOT NULL,
    criado_em     TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
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
-- Um registro por dia (data UNIQUE), valor de 1 a 5
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS humores (
    id          BIGINT  NOT NULL AUTO_INCREMENT,
    `data`      DATE    NOT NULL,
    valor       INT     NOT NULL,
    observacao  TEXT    NULL,
    PRIMARY KEY (id),
    UNIQUE KEY uq_humores_data (`data`),
    CONSTRAINT chk_humores_valor CHECK (valor BETWEEN 1 AND 5)
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
-- o log deve continuar existindo depois que o registro for excluído.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS log_operacoes (
    id             BIGINT       NOT NULL AUTO_INCREMENT,
    tabela         VARCHAR(50)  NOT NULL,
    operacao       VARCHAR(10)  NOT NULL,
    id_registro    BIGINT       NOT NULL,
    dados_antigos  JSON         NULL,
    dados_novos    JSON         NULL,
    data_hora      TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    KEY idx_log_tabela_registro (tabela, id_registro),
    KEY idx_log_data_hora (data_hora),
    CONSTRAINT chk_log_operacao CHECK (operacao IN ('INSERT', 'UPDATE', 'DELETE'))
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- Dados iniciais: categorias predefinidas
-- (só insere se ainda não existir nenhuma predefinida)
-- ---------------------------------------------------------------------
INSERT INTO categorias (nome, icone, cor, predefinida)
SELECT seed.nome, seed.icone, seed.cor, seed.predefinida
FROM (
    SELECT 'Saúde'           AS nome, 'heart'        AS icone, '#468CC8' AS cor, TRUE AS predefinida
    UNION ALL
    SELECT 'Produtividade',           'check-circle',          '#5FAE7A',         TRUE
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
    INSERT INTO log_operacoes (tabela, operacao, id_registro, dados_antigos, dados_novos)
    VALUES ('habitos', 'INSERT', NEW.id, NULL,
            JSON_OBJECT('nome', NEW.nome, 'icone', NEW.icone, 'cor', NEW.cor,
                        'id_categoria', NEW.id_categoria));
END$$

DROP TRIGGER IF EXISTS trg_habitos_au$$
CREATE TRIGGER trg_habitos_au AFTER UPDATE ON habitos
FOR EACH ROW
BEGIN
    INSERT INTO log_operacoes (tabela, operacao, id_registro, dados_antigos, dados_novos)
    VALUES ('habitos', 'UPDATE', NEW.id,
            JSON_OBJECT('nome', OLD.nome, 'icone', OLD.icone, 'cor', OLD.cor,
                        'id_categoria', OLD.id_categoria),
            JSON_OBJECT('nome', NEW.nome, 'icone', NEW.icone, 'cor', NEW.cor,
                        'id_categoria', NEW.id_categoria));
END$$

DROP TRIGGER IF EXISTS trg_habitos_ad$$
CREATE TRIGGER trg_habitos_ad AFTER DELETE ON habitos
FOR EACH ROW
BEGIN
    INSERT INTO log_operacoes (tabela, operacao, id_registro, dados_antigos, dados_novos)
    VALUES ('habitos', 'DELETE', OLD.id,
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
        INSERT INTO log_operacoes (tabela, operacao, id_registro, dados_antigos, dados_novos)
        VALUES ('categorias', 'INSERT', NEW.id, NULL,
                JSON_OBJECT('nome', NEW.nome, 'icone', NEW.icone, 'cor', NEW.cor));
    END IF;
END$$

DROP TRIGGER IF EXISTS trg_categorias_au$$
CREATE TRIGGER trg_categorias_au AFTER UPDATE ON categorias
FOR EACH ROW
BEGIN
    IF OLD.predefinida = FALSE THEN
        INSERT INTO log_operacoes (tabela, operacao, id_registro, dados_antigos, dados_novos)
        VALUES ('categorias', 'UPDATE', NEW.id,
                JSON_OBJECT('nome', OLD.nome, 'icone', OLD.icone, 'cor', OLD.cor),
                JSON_OBJECT('nome', NEW.nome, 'icone', NEW.icone, 'cor', NEW.cor));
    END IF;
END$$

DROP TRIGGER IF EXISTS trg_categorias_ad$$
CREATE TRIGGER trg_categorias_ad AFTER DELETE ON categorias
FOR EACH ROW
BEGIN
    IF OLD.predefinida = FALSE THEN
        INSERT INTO log_operacoes (tabela, operacao, id_registro, dados_antigos, dados_novos)
        VALUES ('categorias', 'DELETE', OLD.id,
                JSON_OBJECT('nome', OLD.nome, 'icone', OLD.icone, 'cor', OLD.cor),
                NULL);
    END IF;
END$$

-- ===== checkins =====
DROP TRIGGER IF EXISTS trg_checkins_ai$$
CREATE TRIGGER trg_checkins_ai AFTER INSERT ON checkins
FOR EACH ROW
BEGIN
    INSERT INTO log_operacoes (tabela, operacao, id_registro, dados_antigos, dados_novos)
    VALUES ('checkins', 'INSERT', NEW.id, NULL,
            JSON_OBJECT('id_habito', NEW.id_habito, 'data', NEW.`data`,
                        'concluido', NEW.concluido));
END$$

DROP TRIGGER IF EXISTS trg_checkins_au$$
CREATE TRIGGER trg_checkins_au AFTER UPDATE ON checkins
FOR EACH ROW
BEGIN
    INSERT INTO log_operacoes (tabela, operacao, id_registro, dados_antigos, dados_novos)
    VALUES ('checkins', 'UPDATE', NEW.id,
            JSON_OBJECT('id_habito', OLD.id_habito, 'data', OLD.`data`,
                        'concluido', OLD.concluido),
            JSON_OBJECT('id_habito', NEW.id_habito, 'data', NEW.`data`,
                        'concluido', NEW.concluido));
END$$

DROP TRIGGER IF EXISTS trg_checkins_ad$$
CREATE TRIGGER trg_checkins_ad AFTER DELETE ON checkins
FOR EACH ROW
BEGIN
    INSERT INTO log_operacoes (tabela, operacao, id_registro, dados_antigos, dados_novos)
    VALUES ('checkins', 'DELETE', OLD.id,
            JSON_OBJECT('id_habito', OLD.id_habito, 'data', OLD.`data`,
                        'concluido', OLD.concluido),
            NULL);
END$$

DELIMITER ;