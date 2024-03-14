```
  _  __                     _                             _        __     ___   ___  
 | |/ /   ___   ____ _     / \   ___ ___ ___  _   _ _ __ | |_ ___  \ \   / / | / _ \ 
 | ' / | | \ \ / / _` |   / _ \ / __/ __/ _ \| | | | '_ \| __/ __|  \ \ / /| || | | |
 | . \ |_| |\ V / (_| |_ / ___ \ (_| (_| (_) | |_| | | | | |_\__ \   \ V / | || |_| |
 |_|\_\__,_| \_/ \__,_(_)_/   \_\___\___\___/ \__,_|_| |_|\__|___/    \_/  |_(_)___/ 
                                                           
```

# backend-accounts

Micro serviço referente ao controle de acesso dos usuários.

## Pre requisitos

- .NET 8
- Docker
- Docker Compose
  - SQL Server
  - Elastic Search
  - Kibana
- Azure Service BUS
- Azure Key Vault

> O Azure Service Bus e o Azure Key Vault são necessários para rodar o projeto, porém não estão inclusos no docker-compose.

## Como rodar

Levar em consideração que o projeto utiliza o user-secrets para armazenar as variáveis de ambiente.

Os scripts serão executados no terminal e na raiz do projeto levando em consideração o scaffold do projeto.

````
- backend-accounts
  - Source
	- Kuva.Accounts.Business
	- Kuva.Accounts.EFMigrations
	- Kuva.Accounts.Entities
	- Kuva.Accounts.Repository
	- Kuva.Accounts.Tests
  - docker-compose.yml
  	- README.md
	- LICENSE
```

Criar o user-secrets incluindo as variáveis:

DB:User => Usuário do banco de dados
DB:Password => Senha do banco de dados
DB:Host => Host do banco de dados
Azure:BusKey => Chave de acesso do Azure Service Bus

Para criar o user-secrets execute o comando:

```bash
dotnet user-secrets set DB:User <user>
dotnet user-secrets set DB:Password <password>
dotnet user-secrets set DB:Host <host>
dotnet user-secrets set Azure:BusKey <busKey>
```

Para rodar o projeto execute o comando na raiz do projeto:

```bash
docker compose up
```

Apos a configuracao do docker-compose, navegar até o projeto Kuva.Accounts.EFMigrations para que os objetos de banco de dados sejam criados:

Para que o banco de dados seja criado execute o comando:

```bash
dotnet ef migrations add <nome da migration>
```

Para que o banco de dados seja atualizado execute o comando:

```bash
dotnet ef database update
```

> Caso apresente o erro max virtual memory areas vm.max_map_count [65530] is too low, increase to at least [262144]
No linux pode ser necessário aumentar a memoria virtual utiliar o comando `sysctl -w vm.max_map_count=262144` e posteriormente `systemctl restart docker`.\

