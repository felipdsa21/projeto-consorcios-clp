# Rotas do microserviço
Os identificadores nos segmentos das URLs são números inteiros.

Em caso de erro, o corpo da resposta terá o seguinte formato:

```json
{
  "mensagem": "Consórcio não existe"
}
```

## Criar um consórcio
```http
POST /consorcios
```

### Corpo da requisição:
```json
{
  "nome": "Consórcio",
  "valor_total": 10000,
  "data_inicio": "2024-01-01",
  "data_fim": "2024-12-31",
  "limite_participantes": 10,
  "status": "Criado",
  "parcelas": 12
}
```

### Corpo da resposta:
```json
{
  "id": 1
}
```

## Listar consórcios
GET /consorcios

### Corpo da resposta:
```json
{
  "consorcios": [
    {
      "id": 1,
      "nome": "Consórcio",
      "valor_total": 10000,
      "data_inicio": "2024-01-01",
      "data_fim": "2024-12-31",
      "limite_participantes": 10,
      "status": "Criado",
      "parcelas": 12
    }
  ]
}
```

## Detalhar um consórcio
```http
GET /consorcios/{consorcio_id}
```

### Corpo da resposta:
```json
{
  "id": 1,
  "nome": "Consórcio",
  "valor_total": 10000,
  "data_inicio": "2024-01-01",
  "data_fim": "2024-12-31",
  "limite_participantes": 10,
  "status": "Criado",
  "parcelas": 12
}
```

### Erros:
- 404 Consórcio não existe

## Alterar um consórcio
```http
PUT /consorcios/{consorcio_id}
```

### Corpo da requisição:
```json
{
  "nome": "Consórcio Editado",
  "valor_total": 20000,
  "data_inicio": "2024-01-01",
  "data_fim": "2024-12-31",
  "limite_participantes": 15,
  "status": "Criado",
  "parcelas": 12
}
```

### Erros:
- 404 Consórcio não existe

## Participar em um consórcio
```http
POST /consorcios/{consorcio_id}/participantes
```

### Corpo da requisição:
```json
{
  "usuario_id": 1
}
```

### Erros:
- 404 Consórcio não existe
- 410 Tentou entrar antes ou depois do prazo
- 403 Limite de participantes excedido

## Listar participantes do consórcio
```http
GET /consorcios/{consorcio_id}/participantes
```

### Corpo da resposta:
```json
{
  "usuarios": [1, 2]
}
```

### Erros:
- 404 Consórcio não existe

## Listar consórcios em que o usuário participa
```http
GET /participantes/{usuario_id}
```

### Corpo da resposta:
```json
{
  "consorcios": [1, 2, 3]
}
```
