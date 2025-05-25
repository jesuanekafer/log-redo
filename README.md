# trabalho_bd_log

# FERRAMENTA DE REFAZER LOG (Redo)

### 1. Dependências

Para executar este projeto, você precisa ter instalado:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download) ou superior
- PostgreSQL (com o banco `banco_log` criado e configurado)
- IDE (Visual Studio, Visual Studio Code ou outro de sua preferência)

---

### 2. Configuração

1. Clone o repositório para sua máquina:

```
git clone https://github.com/jesuanekafer/log-redo.git
```

1. Ajuste a string de conexão no arquivo `Redo.cs` conforme seu ambiente, especialmente usuário, senha, host:

```
private static string connectionString = "Host=localhost;Username=seu_usuario;Password=sua_senha_;Database=banco_log";

```

1. Garanta que exista uma tabela `log` com os campos usados pelo programa (`id`, `operacao`, `id_cliente`, `nome`, `saldo`). A criação da tabela pode ser feita seguindo o definido o arquivo scripts/script.sql:

---

### 3. Banco de Dados

Este projeto utiliza PostgreSQL como banco de dados.

- A tabela `clientes_em_memoria` será criada automaticamente no banco quando você rodar o programa, se ainda não existir.
- A tabela `log` deve conter os registros que serão recuperados e aplicados no redo.

---

### 4. Utilização

### 4.1 Compilando e rodando

No diretório do projeto, execute:

```
dotnet build
dotnet run --project caminho/para/seu/projeto.csproj
```

Ou, se preferir, execute direto o programa compilado:

```
dotnet caminho/para/seu/bin/Debug/net6.0/seuprojeto.dll
```

O programa irá conectar ao banco, buscar os logs da tabela `log` e aplicar as operações `INSERT`, `UPDATE` e `DELETE` na tabela `clientes_em_memoria` conforme o histórico.

---