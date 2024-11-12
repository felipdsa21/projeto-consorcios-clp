# Rotas do microserviço
Este documento descreve as rotas do microserviço de consórcios, incluindo detalhes das requisições, respostas e possíveis erros. Os identificadores de consórcios e participantes são números inteiros.

Em caso de erro, o corpo da resposta terá o seguinte formato:

```json
{
  "mensagem": "consorcio_nao_existe"
}
```

## Criar um consórcio
```http
POST /consorcios
```

Cria um novo consórcio com as informações fornecidas no corpo da requisição.

### Corpo da requisição:
```json
{
  "nome": "Consórcio",
  "descricao": "Lorem ipsum",
  "imagem": "",
  "data_inicio": "2024-01-01",
  "data_fim": "2024-12-31",
  "valor_total": 10000,
  "taxa_administrativa": 5,
  "taxa_fundo_reserva": 10,
  "qtd_parcelas": 12,
  "limite_participantes": 10,
  "status": "Criado"
}
```

### Corpo da resposta:
```json
{
  "id": 1
}
```

## Listar consórcios
```
GET /consorcios
```

Retorna uma lista de todos os consórcios cadastrados.

### Corpo da resposta:
```json
{
  "consorcios": [
    {
      "id": 1,
      "data_criacao": "2024-09-01T12:00:00",
      "nome": "Consórcio",
      "descricao": "Lorem ipsum",
      "imagem": "",
      "data_inicio": "2024-01-01",
      "data_fim": "2024-12-31",
      "valor_total": 10000,
      "taxa_administrativa": 5,
      "taxa_fundo_reserva": 10,
      "qtd_parcelas": 12,
      "limite_participantes": 10,
      "status": "Criado"
    }
  ]
}
```

## Detalhar um consórcio
```http
GET /consorcios/{consorcio_id}
```

Retorna os detalhes de um consórcio específico.

### Corpo da resposta:
```json
{
  "id": 1,
  "data_criacao": "2024-09-01T12:00:00",
  "nome": "Consórcio",
  "descricao": "Lorem ipsum",
  "imagem": "",
  "data_inicio": "2024-01-01",
  "data_fim": "2024-12-31",
  "valor_total": 10000,
  "taxa_administrativa": 5,
  "taxa_fundo_reserva": 10,
  "qtd_parcelas": 12,
  "limite_participantes": 10,
  "status": "Criado"
}
```

### Erros:
- 404 consorcio_nao_existe -> Consórcio não existe

## Alterar um consórcio
```http
PUT /consorcios/{consorcio_id}
```

Atualiza as informações de um consórcio específico.

### Corpo da requisição:
```json
{
  "nome": "Consórcio Editado",
  "descricao": "Sit dolor amet",
  "imagem": "",
  "data_inicio": "2024-01-01",
  "data_fim": "2024-12-31",
  "valor_total": 20000,
  "taxa_administrativa": 10,
  "taxa_fundo_reserva": 3,
  "qtd_parcelas": 12,
  "limite_participantes": 15,
  "status": "Criado"
}
```

### Erros:
- 404 consorcio_nao_existe -> Consórcio não existe

## Apagar um consórcio
```http
DELETE /consorcios/{consorcio_id}
```

Remove um consórcio específico.

### Erros:
- 404 consorcio_nao_existe -> Consórcio não existe

## Contemplar um participante do consórcio
```http
POST /consorcios/{consorcio_id}/contemplar
```

Contempla um participante aleatório do consórcio que ainda não tenha sido contemplado.

### Corpo da resposta:
```json
{
  "usuario_id": 1
}
```

### Erros:
- 404 consorcio_nao_existe -> Consórcio não existe
- 404 sem_participantes -> Sem participantes não contemplados

## Participar em um consórcio
```http
POST /consorcios/{consorcio_id}/participantes
```

Adiciona um usuário a um consórcio.

### Corpo da requisição:
```json
{
  "usuario_id": 1
}
```

### Erros:
- 404 consorcio_nao_existe -> Consórcio não existe
- 403 limite_participantes_excedido -> Limite de participantes excedido
- 409 ja_participando -> Já está participando no consórcio
- 410 fora_do_prazo -> Tentou entrar antes ou depois do prazo

## Sair do consórcio
```http
DELETE /consorcios/{consorcio_id}/participantes/{usuario_id}
```

Remove a participação de um usuário em um consórcio.

### Erros:
- 404 consorcio_nao_existe -> Consórcio não existe
- 404 nao_participando -> Não está participando no consórcio
- 410 fora_do_prazo -> Tentou sair antes ou depois do prazo
- 410 ja_contemplado -> O participante já foi contemplado

## Listar participantes do consórcio
```http
GET /consorcios/{consorcio_id}/participantes
```

Retorna uma lista dos usuários participantes de um consórcio específico.

### Corpo da resposta:
```json
{
  "participantes": [
    {
      "usuario_id": 1,
      "data_entrada": "2024-10-01T12:00:00",
      "status": "Contemplado"
    },
    {
      "usuario_id": 2,
      "data_entrada": "2024-11-01T12:00:00",
      "status": "Participando"
    }
  ]
}
```

### Erros:
- 404 consorcio_nao_existe -> Consórcio não existe

## Listar consórcios em que o usuário participa
```http
GET /participantes/{usuario_id}
```

Retorna uma lista de consórcios em que um usuário específico participa.

### Corpo da resposta:
```json
{
    {
      "consorcio_id": 1,
      "data_entrada": "2024-10-01T12:00:00",
      "status": "Contemplado"
    },
    {
      "consorcio_id": 2,
      "data_entrada": "2024-11-01T12:00:00",
      "status": "Participando"
    },
    {
      "consorcio_id": 3,
      "data_entrada": "2024-12-01T12:00:00",
      "status": "Participando"
    }
}
```
