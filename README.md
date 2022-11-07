## API criada para realização do teste para a vaga de desenvolvedor júnior do Banco Master.

### Esta API utiliza:

-ASP.NET 5;

-Entity Framework como ORM;

-SQL Server como banco de dados;

-Swagger para documentação;

-JWT Bearer para autenticação;

### Endopoints e suas funções:

-/Clients = Carrega todos os usuários do banco de dados;

-/Clients/pix{pix} = Carrega cliente a partir da chave PIX inserida;

-/Clients/register = Cadastra um novo cliente;

-/Clients/login = Login do cliente;

-/Transfers(GET) = Carrega todas as transferências de acordo com a chave PIX informada;

-/Transfers(POST) = Realiza transferência de valores;

### *** O endpoint /Transfer(POST) necessita apenas do preenchimento dos campos: e verifica se o saldo do usuário***

value;

pixKeySend -> pixKey;

pixKeyReceive -> pixKey;

### Após efetuar a transação PIX os valores informados de pixKeySend e pixKeyReceive estão sendo gravados no banco de dados como clientes também e isto não é uma funcionalidade desejada, porém não interfere na funcionalidade da transação.