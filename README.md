# Comentário

- Primeiramente gostaria de agradecer a todos os envolvidos!
- Sou muito grato a essa oportunidade, desde que comecei a ler sobre a trinca me apaixonei pela empresa, após a apresentação da [Isadora Oliveira](mailto:isadora@trinca.recruitee.com) fiquei mais ansioso ainda para saber quando <ins>podemos começar a trabalhar juntos</ins>? hahahah

### Projeto

Sobre o início, fiquei um pouco perdido quando o link do repositório do [GitHub](https://github.com/trinca137/trinca-challenge) me levou para um projeto concluído.
Após essa minha confusão, comecei o projeto do zero como indicado mesmo, porém o projeto que vi lá estava em serverless usando <span style="color:blue">***Azure Functions*** </span><img src="https://assets-global.website-files.com/5f8b0a1abe69652278dad51c/62672357643c183cdaf33792_590eed1f.png " alt="Azure Functions" style="width:15px;"/>**(algo que até então só tinha ouvido falar)**
Venci o desafio que me propus, entendi o básico sobre o funcionamento e consegui concluir o desafio!  :tw-1f603:
**Obs.:** Nas minhas leituras descobri que geralmente quando se escreve essas funções da Azure ou Aws*(seja qual escolher)* geralmente o projeto acaba sendo orientado a eventos. Para não fugir muito do que me era comum, usei o [Repository Pattern](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design) e Orientação a Objeto.

<br />

------------

<img src="https://cdn-icons-png.flaticon.com/512/14/14480.png" alt="Azure Functions" style="width:300px;"/>

------------

<br />

## Comandos Básicos
1. Você precisará da versão do [.NetCore 7.0.14](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
2. Você precisará da versão do [Azure CLI(x64)](https://aka.ms/installazurecliwindowsx64)

`$ dotnet build`
`$ func start`

<br />

## Endpoints

- <span style="color:DarkGoldenRod">**POST**</span> http://localhost:7296/api/churras
**BODY:**
```
{
    "date": "<yyyy-MM-ddTHH: mm:ss>",
    "reason": <string>,
    "isTrincasPaying": <bool>
}
```
- <span style="color:green">**GET**</span> http://localhost:7296/api/churras/{churrasId}
- <span style="color:darkblue">**PUT**</span> http://localhost:7296/api/churras/{{churrasId}}/moderar
**BODY:**
```
{
    "GonnaHappen": <bool>,
    "TrincaWillPay": <bool>
}
```
- <span style="color:green">**GET**</span> http://localhost:7296/api/person/invites
- <span style="color:darkblue">**PUT**</span> http://localhost:7296/api/person/invites/{churrasId}/accept
**BODY:**
```
{
    "isVeg": <bool>
}
```
- <span style="color:darkblue">**PUT**</span> http://localhost:7296/api/person/invites/{churrasId}/decline
**BODY:**
```
{
    "isVeg": <bool>
}
```

**Obs.:** Em todos as requisições o header <ins><span style="color:darkred">personId</span></ins> deve ser passado para quando a autenticação estiver funcional!

<br />

##### Observações Pessoais

Acho que vale a pena comentar que essa semana não foi muito 100% aqui em casa, várias coisas acontecendo! Mas mesmo assim queria usar esse espaço mais uma vez para lembrar que todos somos humanos e que Deus nos ama incondicionalmente!
Se você chegou até aqui você é um guerreiro!

<br />

<span style="color:red">***BORA SOLUCIONAR ESSES PROBLEMAS JUNTOS?***</span>