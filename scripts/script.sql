
--CREATE UNLOGGED TABLE clientes_em_memoria (
--  id SERIAL PRIMARY KEY,
--  nome TEXT,
--  saldo NUMERIC
--);

--CREATE TABLE log (
--	id SERIAL PRIMARY key,
--	operacao TEXT, 
--	id_cliente INT, 
--	nome TEXT, 
--	saldo numeric
--);


--CREATE OR REPLACE FUNCTION gravar_log() RETURNS TRIGGER AS $$
--BEGIN
--  IF TG_OP = 'INSERT' THEN
--    INSERT INTO log (operacao, id_cliente, nome, saldo)
--    VALUES ('INSERT', NEW.id, NEW.nome, NEW.saldo);
--    RETURN NEW;

--  ELSIF TG_OP = 'UPDATE' THEN
--    INSERT INTO log (operacao, id_cliente, nome, saldo)
--    VALUES ('UPDATE', NEW.id, NEW.nome, NEW.saldo);
--    RETURN NEW;

--  ELSIF TG_OP = 'DELETE' THEN
--    INSERT INTO log (operacao, id_cliente, nome, saldo)
--    VALUES ('DELETE', OLD.id, OLD.nome, OLD.saldo);
--    RETURN OLD;
--  END IF;

--  RETURN NULL;
--END;
--$$ LANGUAGE plpgsql;


--CREATE TRIGGER trg_log_clientes
--AFTER INSERT OR UPDATE OR DELETE ON clientes_em_memoria
--FOR EACH ROW EXECUTE FUNCTION gravar_log();
