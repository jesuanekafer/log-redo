--CREATE UNLOGGED TABLE clientes_em_memoria (
--   id SERIAL PRIMARY KEY,
--   nome TEXT,
--   saldo NUMERIC

--);

--CREATE TABLE log (
--	txid BIGINT,
--	operacao TEXT, 
--	id_cliente INT, 
--	nome TEXT, 
--	saldo NUMERIC
--);


--CREATE OR REPLACE FUNCTION gravar_log() RETURNS TRIGGER AS $$
--BEGIN
--  IF TG_OP = 'INSERT' THEN
--    INSERT INTO log (txid, operacao, id_cliente, nome, saldo)
--    VALUES (txid_current(), 'INSERT', NEW.id, NEW.nome, NEW.saldo);
--    RETURN NEW;

--  ELSIF TG_OP = 'UPDATE' THEN
--    INSERT INTO log (txid, operacao, id_cliente, nome, saldo)
--    VALUES (txid_current(), 'UPDATE', NEW.id, NEW.nome, NEW.saldo);
--    RETURN NEW;

--  ELSIF TG_OP = 'DELETE' THEN
--    INSERT INTO log (txid, operacao, id_cliente, nome, saldo)
--    VALUES (txid_current(), 'DELETE', OLD.id, OLD.nome, OLD.saldo);
--    RETURN OLD;
--  END IF;

--  RETURN NULL;
--END;
--$$ LANGUAGE plpgsql;


--CREATE TRIGGER trg_log_clientes
--AFTER INSERT OR UPDATE OR DELETE ON clientes_em_memoria
--FOR EACH ROW EXECUTE FUNCTION gravar_log();



--CREATE OR REPLACE FUNCTION inserir_begin_em_log() RETURNS void AS $$
--BEGIN 
--  INSERT INTO log (txid, operacao) 
--  VALUES (txid_current(), 'BEGIN');
--END;
--$$ LANGUAGE plpgsql;


--CREATE OR REPLACE FUNCTION inserir_end_em_log() RETURNS void AS $$
--BEGIN 
--  INSERT INTO log (txid, operacao) 
--  VALUES (txid_current(), 'END');
--END;
--$$ LANGUAGE plpgsql;

--CREATE OR REPLACE FUNCTION inserir_rollback_em_log() RETURNS void AS $$
--BEGIN 
--  INSERT INTO log (txid, operacao) 
--  VALUES (txid_current(), 'ROLLBACK');
--END;
--$$ LANGUAGE plpgsql;

