# CRUDClienteAPI
CRUD de Cliente usando Azure Functions e base MySQL local

# Lista de códigos de estado HTTP usados neste exemplo

# 200 OK
Padrão de resposta para solicitações HTTP sucesso. A resposta real dependerá do método de solicitação usado. Em uma solicitação GET, a resposta conterá uma entidade que corresponde ao recurso solicitado. Em uma solicitação POST a resposta conterá a descrição de uma entidade, ou contendo o resultado da ação.

# 4xx Erro de cliente
A classe 4xx de código de status é destinado para os casos em que o cliente parece ter cometido um erro. Exceto quando estiver respondendo a uma solicitação HEAD, o servidor deve incluir uma entidade que contém uma explicação sobre a situação de erro, e se é uma condição temporária ou permanente. Esses códigos de status são aplicáveis a qualquer método de solicitação. Os agentes do usuário devem exibir qualquer entidade incluída para o usuário. Estes são tipicamente os códigos de erro mais comuns encontrados durante online.

# 400 Requisição inválida
O pedido não pode ser entregue devido à sintaxe incorreta.

# 412 Pré-condição falhou
O servidor não cumpre uma das condições que o solicitante coloca na solicitação.

# Conexão com o MySql

```json
    "ConnectionStrings": {
      "sqldb_connection": "Server=localhost; Port=3306; Database=talkcode; Uid=dbuser@talkcode; Pwd=B@z1nga2018; SslMode=Preferred;"
    }
```

# Criação do Schema no MySql
```sql
CREATE SCHEMA `talkcode` DEFAULT CHARACTER SET latin1 COLLATE latin1_general_ci ;
```

# Criação da Tabela no banco talkcode
```sql
CREATE TABLE `talkcode`.`data` (
  `key` VARCHAR(50) NOT NULL,
  `collection` VARCHAR(45) NOT NULL,
  `value` JSON NOT NULL,
  PRIMARY KEY (`key`))
ENGINE = InnoDB;
```
