
--begin;
--	select inserir_begin_em_log();
--	insert into clientes_em_memoria (nome, saldo) values ('cliente 1', 100.00);
--	update clientes_em_memoria set saldo = saldo + 50 where id = 1;
--	select inserir_end_em_log();
--end;



--begin;
--	select inserir_begin_em_log();
--	insert into clientes_em_memoria (nome, saldo) values ('cliente 2', 200.00);
--	update clientes_em_memoria set saldo = saldo + 50 where id = 2;
--	select inserir_end_em_log();
--end;



--begin;
--	select inserir_begin_em_log();
--	insert into clientes_em_memoria (nome, saldo) values ('cliente 3', 300.00);
--	update clientes_em_memoria set saldo = saldo + 50 where id = 2;
--	select inserir_end_em_log();
--end;


--begin;
--	select inserir_begin_em_log();
--	insert into clientes_em_memoria (nome, saldo) values ('cliente 4', 400.00);
--	update clientes_em_memoria set saldo = saldo + 50 where id = 3;
--	select inserir_rollback_em_log();
	

--begin;
--	select inserir_begin_em_log();
--    insert into clientes_em_memoria (nome, saldo) values ('cliente 6', 600.00); 
--	select inserir_rollback_em_log();
