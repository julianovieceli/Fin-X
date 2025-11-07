# Porter

# Passos para rodar Localmente

# 1- Baixando imagem e subindo um container com MongoDb
    docker pull mongo
    docker run -d --name FinXDb -e MONGO_INITDB_ROOT_USERNAME=admin -e MONGO_INITDB_ROOT_PASSWORD=admin -p 27017:27017 mongo:latest



Link [Swaggwer](http://localhost:5043/swagger/index.html)


# 3 - Rodando a applicação
    Executar o projeto Porter.Api.
    1- Cadastrar um [cliente](http://localhost:5259/Client)
    2- Cadastrar uma [Room](http://localhost:5259/Room)
    3- Fazer uma reserva [booking]: (http://localhost:5259/Booking)

# 4 Para rodar os testes unitarios basta somente excuta-los. Nao foi possivel usra o InMemory(estou verificando o pq), entao eu removo todos os dados no startup.


# OBS:
 Para rodar em containers, bata subir o container abaixo(nao esquecendo de criar o script no devido banco acima):
 ```
    docker compose -f docker-compose-porter.yml down
    docker compose -f docker-compose-porter.yml up -d

    ## Ou para forçar somente o build da imagem novamente
    docker compose  -f docker-compose-porter.yml up --build --force-recreate porter.api -d
```
    Rodando a applicação dentro docker(porta:6001)
    1- Cadastrar um [cliente](http://localhost:6001/Client)
    2- Cadastrar uma [Room](http://localhost:6001/Room)
    3- Fazer uma reserva [booking]: (http://localhost:6001/Booking)




