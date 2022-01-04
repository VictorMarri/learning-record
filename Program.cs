using System;

/*
 * Beneficios do Record:
 * - Extremamente facil de Configurar comparado a uma classe
 * - É Thread-Safe, pois é imutavel, por isso não temos conflito entre threads operando sobre o mesmo valor
 * 
 * Quando usar:
 *  - Pegar dados externos que não mudam (leitura) (Se formos pegar dados de uma API)
 *  - Processar dados num bannco (Podemos usar Record dentro de uma classe)
 *  - TODA VEZ QUE TRABALHARMOS COM DADOS SOMENTE DE LEITURA (Como por exemplo só pegar algum dado, seja do banco ou da API e não alterar eles)
 *  
 *  Quando NÃO USAR:
 *  - Quando temos que alterar os dados (EntityFramework)
 */

namespace RecordLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Record1 r1a = new("Victor", "Marri");
            Record1 r1b = new("Victor", "Marri");
            Record1 r1c = new("Jorge", "Mendes");

            Class1 c1a = new("Victor", "Marri");
            Class1 c1b = new("Victor", "Marri");
            Class1 c1c = new("Jorge", "Mendes");

            Console.WriteLine("Record Type:\n");

            //Record faz um override automatico no metodo ToString
            Console.WriteLine($"To String: { r1a }");

            //Um record, como falado abaixo, age como se fosse um tipo de valor, então, mesmo que os records sejam diferentes, os valores são exatamente os mesmos, e se formos comparar, o record vai apontar essa igualdade
            //Logo, o record tambem, assim como o ToString, faz um override do Equals, com parando cada propriedade dos records umas com as outras
            Console.WriteLine($"Os dois objetos são iguais?: { Equals(r1a, r1b) }");

            Console.WriteLine($"As duas REFERENCIAS dos objetos são iguais?: { ReferenceEquals(r1a, r1b) }");

            Console.WriteLine($"Os dois objetos são iguais? ==: { r1a == r1b }");

            Console.WriteLine($"Os dois objetos são diferentes? !=: { r1a != r1c }");

            //Um record tambem faz override no metodod GetHashCode, de forma que, mesmo que os objetos sejam 'instancias diferentes', o record vai perceber que os valores dentro são iguais, e com isso, o codigo hash dos dois vão ser iguais. Isso não ocorre com as classes
            Console.WriteLine($"Codigo hash dod objeto A: {r1a.GetHashCode()}");
            Console.WriteLine($"Codigo hash dod objeto B: {r1b.GetHashCode()}");
            Console.WriteLine($"Codigo hash dod objeto C: {r1c.GetHashCode()}");

            Console.WriteLine();

            //Podemos tambem no record ter uma especie de 'Desconstrutor', baseado no mesmo padrão que vem no record
            //Um Class não tem isso, e por isso nele fiz um metodo desconstrutor
            var (fn, ln) = r1a;
            Console.WriteLine($"O valor de fn é { fn } e o valor de ln é { ln }");

            Record1 r1d = r1a with
            {
                FirstName = "John"
            };

            Console.WriteLine($"Dados do John: { r1d }");

            Console.WriteLine();

            Record2 r2a = new("Victor", "Marri");

            Console.WriteLine($"Valores do Record2: { r2a }\n");

            Console.WriteLine(r2a.DigaOla());

            Console.WriteLine("\n");
            Console.WriteLine();

            Console.WriteLine("Class Type:\n");

            Console.WriteLine($"To String: { c1a }");

            //Uma classe vai comparar o lugar na memoria para fazer a equalização, como se fosse o 'endereço' , com isso, SEMPRE dois objetos diferentes (classes) vão ser considerados diferentes
            Console.WriteLine($"Os dois objetos são iguais?: { Equals(c1a, c1b) }");

            Console.WriteLine($"As duas REFERENCIAS dos objetos são iguais?: { ReferenceEquals(c1a, c1b) }");

            Console.WriteLine($"Os dois objetos são iguais? ==: { c1a == c1b }");

            Console.WriteLine($"Os dois objetos são diferentes? !=: { c1a != c1c }");

            Console.WriteLine($"Codigo hash dod objeto A: {c1a.GetHashCode()}");
            Console.WriteLine($"Codigo hash dod objeto B: {c1b.GetHashCode()}");
            Console.WriteLine($"Codigo hash dod objeto C: {c1c.GetHashCode()}");



        }
    }

    //Record não usamos o padrão camelCase com a primeira letra minúscula, pq diferente das classes, essas propridades não são propriamente parametros, e sim definições de valores
    //Records agem como tipos de valor, porem, assim como classes, são TIPOS DE REFERENCIA!
    //Records são basicamente classes com uma funcionalidade pouco diferente, é como se fosse um coddigo pre feito pra facilitar o trabalho
    //Uma vez que setamos os valores no Record, não podemos muda-los, é uma classe somente de leitura (read-only)
    //Um Record é bem diferente de um Struct. Record podemos instanciar, records são tipos de referencia, e não valor
    //Podemos herdar com records, com struct não. Records podem herdar somente de outros records, asssim como classes só podem herdar de outras classes.
    public record Record1(string FirstName, string LastName); 

    public record User1(int Id, string FirstName, string LastName) : Record1(FirstName, LastName);

    public record Record2(string FirstName, string LastName)
    {
        public string FirstName { get; init; } = FirstName;

        public string FullName { get => $"{FirstName} { LastName}"; }

        public string DigaOla()
        {
            return $"Olá {FirstName}";
        }
    }

    //Num nivel mais superficial, essa classe abaixo é igual ao record acima
    //A diferença, pra equiparar com o Record, é que no lugar do set,temos um init
    //Esse init significa que só podemos criar o valor da popriedade dentro do construtor, ou quando atribuimos os valores por chaves na declaração do objeto
    public class Class1
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }

        public Class1(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public void Desconstruir(out string FirstName, out string LastName)
        {
            FirstName = this.FirstName;
            LastName = this.LastName;
        }
    }
}
