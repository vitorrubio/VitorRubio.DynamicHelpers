#VitorRubio.DynamicHelpers => Exemplos, testes e utilitários para trabalhar com dynamics

##Cenário geral:		
	Conforme nossos softwares vai ficando mais complexo, e conforme vamos isolando a camada de domínio da camada de apresentação e das APIs vai surgindo a necessidade de se criar muitas classes,
		a maioria anêmica, com o objetivo apenas de se transitar dados entre as camadas. Isso acaba gerando uma explosão de DTOs, ViewModels e até de tuples com o objetivo de combinar ou 
		agrupar diferentes combinações de propriedades e levar os dados entre as camadas da aplicação.
		Isso pode ser consequencia dos bad smells [Primitive Obsession](https://refactoring.guru/smells/primitive-obsession)
		e de não se aplicar o princípio do [Tell-Don't-Ask](https://martinfowler.com/bliki/TellDontAsk.html)
		mas não vamos entrar nesse mérito, o objetivo é criar um tipo de objeto que possa ser usado dinamicamente mas com alguns benefícios da tipagem estática, para que você possa usar no
		seu projeto mesmo que ele seja uma baderna, eu não julgo. Na verdade quero é favorecer a baderna. 
		A idéia é que qualquer objeto deveria poder ser usado como Bag igual o ViewBag do Razor
	O .net framework tem uma série de classes para lidar com isso:
	- [Using type dynamic (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/using-type-dynamic)
	- [DynamicObject Class](https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.dynamicobject?view=netcore-3.1)
	- [ExpandoObject Class](https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.expandoobject?view=netcore-3.1)


###Cenário 1:
	Quero retornar dos meus métodos e apis objetos de domínio,  objetos anônimos e projeções (selects feitos com linq) sem me importar com que tipo de objetos que são

###Cenário 2:
	Dado um objeto do meu domínio quero adicionar propriedades a ele que são transitórias / importantes para apenas uma etapa do processo (um timestamp, marcador de usuário, 
		propriedade customizada, marcador de novo, o que for necessário)

###Cenário 3:
	Dado um objeto anônimo / projeção quero adicionar propriedades transitórias a ele assim como no cenário 2

###Cenário 4:
	Poder converter para JSON ou de JSON corretamente

> Basicamente queremos misturar os benefícios de DynamicObject com ExpandoObject


##Problemas e possíveis soluções:
###Problema: 			
	Quero retornar para minhas actions ou serializar em Json objetos vujo tipo eu não conheço e acessar suas propriedades e métodos
###Solução: 			
	use dynamic, comporta qualquer tipo e você pode passar pra lá e pra cá na sua aplicação e serializar para JSON
###Para que serve:		
	Soluciona apenas o cenário 1 e 4, mas não o 2 e 3
Mas ... 



###Problema: 			
	com dynamic eu não posso adicionar novas propriedades e métodos igual o fazemos em linguagens dinâmicas tipo javascript. Quero ser capaz de adicionar propriedades dinamicamente 
		que sejam usadas só em um determinado momento do processo
###Solução: 			
	use ExpandoObject, você pode adicionar propriedades e métodos na hora e trabalhar com ele quase como se fosse um objeto javascript
###Para que serve:		
	Soluciona parcialmente  apenas o cenário 3, ExpandoObject nunca vai ser um objeto do seu domínio. Mesmo que você adicione todas as propriedades a ele, os métodos não serão adicionados. 
		E para projeções, você precisa criar o new ExpandoObject e adicionar cada uma das propriedades.
Mas ... 



###Problema:			
	O ExpandoObject não me atende completamente. Eu não posso usar como objeto de domínio, ele é sealed, não posso criar um descendente dele, e tem a necessidade de copiar todas 
		as propriedades (do domínio e da projeção), eu não aproveito as propriedades que eu já tinha, tenho que passar cada uma delas, e é fracamente tipado, 
		não há nenhuma assistência de código da parte da IDE. Outro problema é que o ExpandoObject é serializado para javascript como se fosse um dictionary ou collection de KeyValuePair.
		Para serializar corretamente em Json é necessário usar o Newtonsoft.JSON ou criar um converter customizado
###Solução:			
	Existe um objeto no C# chamado DynamicObject que não pode ser instanciado (deve ser herdado) e que tem suporte a propriedades declaradas e pode ser associada a um dicionário para 
		suportar propriedades dinâmicas como o ExpandoObject e mesmo assim ser uma classe do seu domínio com métodos e propriedades declarados.
###Para que serve:		
	Parece atender bem os cenários 1, 2 e 4, o cenário 3 não
Mas ...				



###Problema:			
	Não pode ser usado como Dictionary, não expõe seu dictionary interno, não se pode iterar pelas propriedades ou acessar as propriedades via reflection. Ele traz as propriedades 
		declaradas e esquece das dinâmicas.  Necessidade de implementar GetDynamicMemberNames() juntando as propriedades declaradas com as dinâmicas caso contrário ele não serializa 
		corretamente, não encontra essas propriedades. 
###Solução:			
	Pra que simplificar se podemos complicar? Vamos criar uma classe chamada BaseElasticObject derivada de DynamicObject mas que também se comporte como ExpandoObject, suporte JSON 
		e possa ser usada como DomainModel/EntityModel, inclusive com suporte a EF (e outros ORM's) e com suporte a bancos nosql como LiteDb e MongoDb


##Objetivos: (parecem conflitantes, mas talvez consigamos nos aproximar)
1. Criar uma classe chamada BaseElasticObject que implemente todas as interfaces de DynamicObject e ExpandoObject e que possa atuar como as duas, 
	funcionando como um ExpandoObject não sealed e com suporte a herança e que possa favorecer:
	1.1. Flexibilidade de uso igual linguagens dinâmicas / fracamente tipadas como Javascript, mas com suporte de algumas das vantagens de tipagem estática/forte
	1.2. Eliminar a necessidade de se criar tantos ViewModels diferentes com combinações diferentes de propriedades do meu domínio
	1.3. Eliminar a necessidade de se criar tantos DTOs diferentes para movimentar os dados entre as camadas
	1.4. Simplificar a sintaxe
	1.5. Ter verificações de tipo forte / estático e assistência de código da IDE quando não estiver sendo usado como dinâmico
	1.6. Ter possibilidade de adicionar propriedades como se fosse um objeto dinâmico
	1.7. suporte tanto propriedades declaradas para termos a verificação de tipo em design time quanto acessar propriedades dinâmicas em runtime. 
			O objeto precisa parecer dinâmico mas também funcionar como um objeto estaticamente/fortemente tipado do seu domínio e suportar herança
	1.8. ser conversível para dynamic ou de volta de dynamic pra ela 
	1.9. Os métodos que implementam a interface IDictionary, o GetDynamicMemberNames() e os métodos que implementam IEnumerable devem usar as propriedades contidas no dicionário mescladas com as 
			propriedades declaradas, levando-as em conta
2. Suporte a conversão para Json gerando um objeto json "de verdade" e não um dictionary ou vetor de key / value
3. Poder ser usado como Bag ou Dictionary
4. Apoiar o uso fácil de apis, objetos dinâmicos, COM interop, serializar a partir de um objeto JSON deve gerar um objeto válido, e suportar propriedades que ele não tinha e foram adicionadas no JSON
5. Funcionar com EF inclusive mapear propriedades que não foram declaradas
6. Funcionar com MongoDb e LiteDb


##QUESTIONAMENTOS:
1. adicionar item existente (VIA INTERFACE IDICTIONARY) deveria disparar exception?
2. mudar via reflection valor de propriedade readonly deveria disparar exception ?
3. remove deveria fazer o que com propriedades do proprio objeto? Limpar e voltar para o valor default?
4. add do dictionary deveria setar variavel estática de igual valor (para não ter diferença entre o valor armazenado no tictionary e o armazenado na property declarada)
5. acessar uma propriedade não definida estaticamente (declarada) e não definida dinamicamente deveria retornar null ou disparar uma exception ? 
	(isso mudaria o roadmap com relação aos testes em que tem que passar/falhar)
6. Deveria ter alguma propriedade imutável ? Um Identificador Universal ?
7. Deveria implementar os overrides de ToString / Equals / GetHashCode ? GetHashCode deveria se basear em qual propriedade para gerar o hash ? Ou no serializado total?


##Roadmap
1. Expor o cenário geral e os cenários 1, 2 e 3 na forma de casos de uso e estórias de usuário
2. O BaseElasticObject deve se comportar como qualquer outro objeto da linguagem C# com suporte a herança e polimorfismo E ainda suportar tudo o que o ExpandoObject suportar
3. O BaseElasticObject em um primeiro momento deve passar em todos os testes unitários que o ExpandoObject passaria E que um objeto do seu domínio passaria
4. O BaseElasticObject deve passar em todos os testes unitários que um DynamicObject passaria
5. O BaseElasticObject deve implementar as mesmas interfaces que o ExpandoObject e que o DynamicObject 
6. O BaseElasticObject deve passar nos mesmos testes que o ExpandoObject e que o DynamicObject passariam visando-se as interfaces ICollection<KeyValuePair<string,object>>, IDictionary<string,object> e IEnumerable
7. Criar testes unitários que explorem e mostrem possiveis falhas em todos os métodos sobrecarregados / overrides de DynamicObject exceto os já implementados TryGetMember, TrySetMember e TryInvokeMember
8. Criar o xmldoc dos métodos
9. Criar suporte a GetHashCode / Equals / ToString()
10. Implementar algo que suporte completamente o cenário 3, talvez um helper ou extension method ?
11. Fazer testes com MongoDb e LiteDb
12. Fazer testes comparativos entre dynamic, ExpandoObject, implementação básica de DynamicObject, BaseElasticObject
13. Testar com .net core
14. Implementar um objeto chamadao CasaDaMaeJoana onde pode tudo: que possa adicionar valores pela interface de dictionary sem disparar exception / obter valores inexistentes retornando null sem disparar exception e setar valores de propriedades readonly



##Disclaimer
O objetivo desse projeto é meramente didático. A quem ele se destina? Principalmente a mim, que estou aprendendo.
Não use em ambiente de produção, pois ele vai explodir a qualquer momento.
Ou melhor, use em ambiente de produção e me fale dos erros que encontrou, faça PR com suas soluções e testes que falharam, assim eu posso aprender com os seus erros.
Esse é meu primeiro projeto no GitHub com alguns testes unitários e com uma página de apresentação readme.md bonitinha, e é um projeto de uma classe só, basicamente. Por favor gostaria de críticas construtuvas quanto a essa apresentação e quanto aos testes unitários, se estão construídos corretamente.

##Referências e Agradecimentos
Abaixo lista de links que eu pesquisei no Stack Overflow e outros sites que me ajudaram a chegar nessa solução:
- [Adventures with C# 4.0 dynamic - ExpandoObject, ElasticObject, and a Twitter client in 10 minutes](https://www.codeproject.com/Articles/62839/Adventures-with-C-4-0-dynamic-ExpandoObject-Elasti) - Obrigado [Anoop Pillai](https://www.codeproject.com/script/Membership/View.aspx?mid=1117033) pelo seu código que eu e mais uns milhares de pessoas copiamos
- [How can I make Json.NET serialize and deserialize declared properties of custom dynamic types that also implement IDictionary<string, object>?](https://stackoverflow.com/questions/55464757/how-can-i-make-json-net-serialize-and-deserialize-declared-properties-of-custom)
- [DynamicObject.GetDynamicMemberNames Method](https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.dynamicobject.getdynamicmembernames?view=netframework-4.7.2)
- [Serialize instance of a class deriving from DynamicObject class](https://stackoverflow.com/questions/49118571/serialize-instance-of-a-class-deriving-from-dynamicobject-class/49120402#49120402)
- [My own dynamic class usingIDynamicMetaObjectProvider Implementation?](https://social.msdn.microsoft.com/Forums/sqlserver/en-US/852203d0-0f75-4e01-9008-66e180f61143/my-own-dynamic-class-usingidynamicmetaobjectprovider-implementation?forum=csharpgeneral) - parece que eu não fui o único a pensar nisso
- [C# How to serialize (JSON, XML) normal properties on a class that inherits from DynamicObject](https://stackoverflow.com/questions/18822121/c-sharp-how-to-serialize-json-xml-normal-properties-on-a-class-that-inherits/18822202#18822202)
- [Serialization Guide](https://www.newtonsoft.com/json/help/html/SerializationGuide.htm)
- [Working with the Dynamic Type in C#](https://www.red-gate.com/simple-talk/dotnet/c-programming/working-with-the-dynamic-type-in-c/)
