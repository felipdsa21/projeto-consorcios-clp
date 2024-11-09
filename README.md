# Projeto de Conceitos de Linguagem de Programação
- Microserviço de consórcios para o projeto Oxebanking
- Implementação de testes básicos para as rotas do microserviço

## Como executar
Primeiramente, instale o Microsoft .NET SDK 8.0.
Baixe no site https://dotnet.microsoft.com/download ou, se usar Windows, execute:

```console
winget install --exact --id Microsoft.DotNet.SDK.8
```

Em seguida, abra um terminal na pasta ConsorciosCLP e execute:

```console
dotnet run
```

## Como executar (Docker)
Abra um terminal na pasta raíz e execute os seguintes comandos:

```console
docker build --tag consorcios-clp ConsorciosCLP
docker run --volume $PWD/Database:/app/Database consorcios-clp
```

## Equipe
- Felipe da Silva Araújo
- Iury Kauann David Nogueira
