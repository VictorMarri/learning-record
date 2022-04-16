### Fontes de estudo onde me baseei para escrever esse 'artigo':
- https://www.macoratti.net/20/11/c9_record1.htm -> Macoratti
- https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/builtin-types/record -> Microsoft Docs
- https://www.c-sharpcorner.com/article/c-sharp-9-0-introduction-to-record-types/ -> CsharpCorner
- https://www.infoworld.com/article/3607372/how-to-work-with-record-types-in-csharp-9.html -> InfoWorld

# Records

O principal objeto dos Records em C# é trazer **imutabilidade e tornar os objetos thread-safe.**

Imutabilidade deixa nossos objetos da aplicação seguros quanto ao processo asincrono, melhorando nosso gerenciamento de memória e deixando nosso codigo mais legível e fácil de se manter. Até recentemente, não existia a possibilidade de, no contexto .NET, criamos classes imutáveis no contexto ‘fora da caixa’, que seria após a sua criação. Agora, temos suporte de imutabilidade com os novos **init-only properties.**

**Propriedades marcadas como init-only podem ser usadas para criar propriedades individuais imutáveis de um objeto, e, se voce ta pensando que podemos usar somente o readonly que ele faz basicamente o mesmo trabalho, saiba que o record torna o objeto INTEIRO imutável, logo, nosso programa não tem que gastar consumo de memoria pra saber quais entidades/propriedades são somente leitura, ele ja infere que o objeto inteiro é, e por isso torna nosso consumo de memória mínimo, leve.**

Como objetos imutáveis **não mudam seu estado**, imutabilidade acaba se tornando uma feature muito desejada em vários casos, como por exemplo **multi-threading e os famosos DTO’s (Data Transfer Objects).**

- O Tipo Record é criado usando a palavra chave **record**
- O Tipo Record é um tipo **de referência, como uma classe**
- A igualdade no tipo Record funciona como nos **tipos por valor**
- Quando comparamos dois Record, essa comparação é feita por **valor e não por referencia!**

Comparando a primeira vista um Record e uma classe, podemos não notar muita diferença

```csharp
//Class
public class Aluno
{
	public string Nome {get;set;}
	public int Idade {get; set;}
}
```

```csharp
//Record
public record Aluno
{
	public string Nome {get; init;}
	public int Idade {get; init;}
}
```

Mas, se voce reparar, nos records temos a propriedade **init;** definindo a propriedade, ou as propriedades. Isso quer dizer que, após receberem um valor, elas se tornam **imutáveis.** Esse record vai ser chamado exatamente como se chama uma classe comum, sendo instanciado:

```csharp
class Program
{
	static void Main(string[] args)
	{
		var aluno = new Aluno("Joao", 47); //A partir daqui eu n posso passar nenhum valor mais para as propriedades dessa instancia, por isso o Init-only
		Console.WriteLine(aluno.Nome);
		Console.WriteLine(aluno.Idade);
	}
}
```

Caso queiramos alterar o valor que está contido nesse record ‘aluno’ nós não vamos conseguir alterar diretamente da instancia, e aí **devemos criar uma ‘copia’ da instancia e alterar essa copia na inicialização dos valores, utilizando a palavra chave ‘With’:**

```csharp
class Program
{
	static void Main(string[] args)
	{
		var aluno = new Aluno("Joao", 47); //A partir daqui eu n posso passar nenhum valor mais para as propriedades dessa instancia, por isso o Init-only
		Console.WriteLine(aluno.Nome); //Joao
		Console.WriteLine(aluno.Idade); //47

		var aluno2 = aluno with {Idade = 35};
		Console.WriteLine(aluno2.Nome); //Joao
		Console.WriteLine(aluno2.Idade); //35
	}
}
```

# Construtor e desconstrutor em record

Quando temos um record, podemos definir um construtor, mas tambem um desconstrutor:

```csharp
public record Cliente
{
	public string Nome;
	public int Idade;

	//Construtor
	public Cliente(string nome, int idade)
	{
		Nome = nome;
		Idade = idade;
	}

	//Desconstrutor
	public void Deconstruct(out string nome, out string idade)
	{
		nome = Nome;
		idade = Idade;
	}

}

```

Essa desconstrução permite que a gente atribua o objeto Cliente a uma tupla, que vai especificar as variaveis individuais pra armazenar os valores desse record:

```csharp
class Program
{
	static void Main(string[] args)
	{
		var cliente= new Cliente("Joaquim", 18); //A partir daqui eu n posso passar nenhum valor mais para as propriedades dessa instancia, por isso o Init-only
		Console.WriteLine(cliente.Nome); //Joao
		Console.WriteLine(cliente.Idade); //47

		var (nome, idade) = cliente;
		Console.WriteLine(nome); 
		Console.WriteLine(idade); 
	}
}
```

# Herdando em tipos record

Tipos record suportam herança. Podemos criar um novo tipo record derivado de um record já existente e adicionar novas propriedades. O Codigo abaixo ilustra um exemplo de como podemos fazer 

```csharp
public record Pessoa
{
	public string Nome {get;init;}
	public string Sobrenome {get;init}
}

public record Empregado : Pessoa
{
	public int Id {get; init;}
	public double Salario {get;init}
}
```

# Positional Records

Instancias de tipos Record que são criados utilizando positional arguments sao imutáveis por padrão. Em outras palavras, podemos criar uma instancia imutável de um tipo Record passando pra ele uma lista ordenada de parametros, usando um construtor de argumentos como mostrado no codigo abaixo:

```csharp
var Pessoa = new Pessoa("Victor", "Marri", "Av dos Andradas", "Brazil");
```

Ou, da forma declarativa seguindo o sentido de classe podemos ter tambem:

```csharp
public record Cliente(string Nome, int Idade);
```

que seria basicamente a forma abreviada de se fazer isso:

```csharp
//Positional Record: Abreviado
public record Cliente(string Nome, int Idade);

//|
//|
//v

//Entire Record Style:
public record Cliente
{
	public string Nome;
	public int Idade;

	//Construtor
	public Cliente(string nome, int idade)
	{
		Nome = nome;
		Idade = idade;
	}

	//Desconstrutor
	public void Deconstruct(out string nome, out string idade)
	{
		nome = Nome;
		idade = Idade;
	}

}
```

Somente nessa unica linha de Record que eu fiz pra demonstrar o Positional Record, eu tenho:

- Um Tipo Record de Cliente
- Propriedades publicas com Init (Init-Only) implementadas (Nome e Idade)
- Construtor publico parametrizado
- Metodo de desconstrução

E se observarmos, **os parametros estão escritos em PascalCase, com o primeiro caractere maiúsculo. Mas por qual motivo esses parametros estão escritos em PascalCase?**

**Os parametros estão escritos em PascalCase pois as propriedades são geradas a partir desses parametros, e no .Net, as propriedades de uma classe são escirtas em PascalCase. O Motivo é que temos que passar todos os valores em posição para o construtor para ele criar os objetos. Esse recurso vai economizar muito codigo**
