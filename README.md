# trabalho_bd_log

# FERRAMENTA DE REFAZER LOG (Redo)

### 1. Depend�ncias

Para executar este projeto, voc� precisa ter instalado:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download) ou superior
- PostgreSQL (com o banco `banco_log` criado e configurado)
- IDE (Visual Studio, Visual Studio Code ou outro de sua prefer�ncia)

---

### 2. Configura��o

1. Clone o reposit�rio para sua m�quina:

```
git clone https://github.com/jesuanekafer/log-redo.git
```

1. Ajuste a string de conex�o no arquivo `Redo.cs` conforme seu ambiente, especialmente usu�rio, senha, host:

```
private static string connectionString = "Host=localhost;Username=seu_usuario;Password=sua_senha_;Database=banco_log";

```

1. Garanta que exista uma tabela `log` com os campos usados pelo programa (`id`, `operacao`, `id_cliente`, `nome`, `saldo`). A cria��o da tabela pode ser feita seguindo o definido o arquivo scripts/script.sql:

---

### 3. Banco de Dados

Este projeto utiliza PostgreSQL como banco de dados.

- A tabela `clientes_em_memoria` ser� criada automaticamente no banco quando voc� rodar o programa, se ainda n�o existir.
- A tabela `log` deve conter os registros que ser�o recuperados e aplicados no redo.

---

### 4. Utiliza��o

### 4.1 Compilando e rodando

No diret�rio do projeto, execute:

```
dotnet build
dotnet run --project caminho/para/seu/projeto.csproj
```

Ou, se preferir, execute direto o programa compilado:

```
dotnet caminho/para/seu/bin/Debug/net6.0/seuprojeto.dll
```

O programa ir� conectar ao banco, buscar os logs da tabela `log` e aplicar as opera��es `INSERT`, `UPDATE` e `DELETE` na tabela `clientes_em_memoria` conforme o hist�rico.

---