# MigracaoDeSQLServerParaMongoDB
Essa aplicação foi feita para migrar uma grande base de dados de relacionados a logs de auditoria para o mongoDB.
Foi pensada de modo que possa ser utilizada para migrar outras tabelas, basta criar uma model da tabela com os campos e utilizar a interface IPropriedadesSql.
Como essa implementação tem objetivo de uso interno não foi aplicada o parameters para prevenir SQL Injection.
