# Fin X API

# Passos para rodar Localmente

# 1- Baixando imagem e subindo um container com MongoDb
    docker pull mongo
    docker run -d --name FinXDb -e MONGO_INITDB_ROOT_USERNAME=admin -e MONGO_INITDB_ROOT_PASSWORD=admin -p 27017:27017 mongo:latest

# 2-  Criar banco, collection e validacoes OBS: Este passo não é obrigatório pois o banco, collections sao criados automaticamente, porem é interssante popular 
#     a collection Exam, e PRINCIPALMENTE criar os validators.

```
use FinXDb;

db.createCollection("Exam");

db.Exam.createIndex(
    { Code: 1 }, // 1 for ascending order (or -1 for descending)
    { unique: true } // Enforce uniqueness
);

db.Exam.insertOne({
    Name: "Tomografia", 
    Code: "TOM"
});

db.Exam.insertOne({
    Name: "Ressonancia", 
    Code: "RM"
});

db.Exam.insertOne({
    Name: "Sangue", 
    Code: "SNG"
});



db.createCollection("Patient", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["Name", "Docto", "CreateDate", "BirthDate"],
      properties: {
        Name: { bsonType: "string" },
        Docto: { bsonType: "string" },
        CreateDate: { bsonType: "date" },
        BirthDate: { bsonType: "date" },
        PhoneNumber: { bsonType: "string" },
        DeletedDate: { bsonType: ["date", "null"] }
      }
    }
  },
  validationLevel: "strict",
  validationAction: "error"
});


db.createCollection("PatientHistory", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["PatientDocumentId","CreateDate", "User"],
      properties: {
        PatientDocumentId: { bsonType: "objectId" },
        User: { bsonType: "string" },
        Diagnostic: { bsonType: "string" },
        Prescription: { bsonType: "string" },
        CreateDate: { bsonType: "date" },
		Exams :{
		    bsonType: "array",
		   items: {
            bsonType: "object",
            required: ["Code", "Name"],
            properties: {
              Code: {
                bsonType: "string",
                description: "must be a string and is required"
              },
              Name: {
                bsonType: "string",
                description: "must be a string and is required"
              },
            }
          }
		    
		},
        PlaceId: 
        { 
            bsonType: "int" ,
            description: "A prioridade deve ser 1(Clinic), 2(Laboratory) ou 3(Hospital).",
            // Você pode usar 'enum' com números também
            enum: [1, 2, 3]
            
        }
      }
    }
  },
  validationLevel: "strict",
  validationAction: "error"
}); 
```


Link [Swaggwer](http://localhost:5043/swagger/index.html)



# 3 - Rodando a applicação
# 3.1 - Importar a collection Finx.postman_collection.json no Postman para facilitar os testes.
    Executar o projeto Fin X.Api.
    1- Efetuar [Login](http://localhost:5043/Login). Utilize o usuario finx e senha finx123
    2- Cadastrar um [paciente](http://localhost:5043/Patient). Usar o JWT do response do login
    3- Cadastrar um [Atendimento](http://localhost:5043/Patient/history)

# 4 Para rodar os testes unitarios basta somente excuta-los. Projeto : Fin X.Tests



# 5- Para rodar tudo dentro do docker(docker compose)docker(porem ainda esta com problemas precisa de ajuste.)

    docker compose -f docker-compose.yml down
    docker compose -f docker-compose.yml up -d
