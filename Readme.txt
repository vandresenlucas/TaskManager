Para rodar a API são necessários seguir os seguintes passos:

1- Ter o docker instalado.
2- Rodar o comando docker-compose up -d para criar e rodar o container do Redis.
3- Executar o arquivo TaskManager.exe ou pelo Visual Studio Rodar a aplicação.

Será aberto um console e API estará rodando.
Também será aberto o Swagger com a documentação da API no browser.

Autenticação de usuários:

- Importar o arquivo TaskManager.postman_collection no postman para realização das operações da API
- No postman fazer a autenticação de usuário:
	- TaskManager/Tasks/Authenticate
		- Login (dados:
			{
			  "email": "admin@gmail.com",
			  "password": "Admin@1234"
			})
- Também é possível criar um usuário próprio e fazer o login para poder utilizar as operações das tarefas.
	- TaskManager/Tasks/User
		- AddUser
		