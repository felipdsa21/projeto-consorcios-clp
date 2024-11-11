# Projeto de Conceitos de Linguagem de Programação
- Microserviço de consórcios para o projeto Oxebanking
- Implementação de testes básicos para as rotas do microserviço

A documentação das rotas está no arquivo [Rotas.md](./Rotas.md).

## Tecnologias utilizadas
- [F#](https://fsharp.org): Linguagem de programação funcional para a plataforma [.NET](https://dotnet.microsoft.com).
- [Suave](https://suave.io): Framework Web para criação de servidores e APIs em F#.
- [Entity Framework](https://learn.microsoft.com/ef): Biblioteca ORM que facilita a manipulação de dados em bancos de dados relacionais.
- [SQLite](https://sqlite.org): Banco de dados leve e autônomo.
- [Expecto](https://github.com/haf/expecto): Biblioteca de testes unitários para F#.

## Como executar
Primeiramente, instale o Microsoft .NET SDK 8.0.
Baixe no site <https://dotnet.microsoft.com/download> ou, se usar Windows, execute:

```console
winget install --exact --id Microsoft.DotNet.SDK.8
```

Em seguida, abra um terminal na pasta ConsorciosCLP e execute:

```console
dotnet run
```

O banco de dados será criado automaticamente.

## Como executar (Docker)
Abra um terminal na pasta raíz e execute os seguintes comandos:

```console
docker build --tag consorcios-clp ConsorciosCLP
docker run --volume $PWD/Database:/app/Database consorcios-clp
```

## Equipe
- Felipe da Silva Araújo
- Iury Kauann David Nogueira
