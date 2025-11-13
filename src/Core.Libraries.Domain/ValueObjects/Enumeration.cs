using System.Reflection;

namespace Core.Libraries.Domain.ValueObjects;

/// <summary>
/// Classe base para enumerações ricas. Permite definir constantes nomeadas com lógica adicional,
/// útil para cenários onde enums tradicionais são muito limitados (ex: domain-driven design).
/// </summary>
public abstract class Enumeration : IComparable
{
    /// <summary>
    /// Cria uma nova instância da enumeração com o código e nome especificados.
    /// </summary>
    protected Enumeration(int code, string name) => (Code, Name) = (code, name);

    /// <summary>
    /// Obtém o nome do valor da enumeração.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Obtém o código (valor numérico) da enumeração.
    /// Diferencia-se de IDs de banco de dados, sendo um código de identificação da enumeração.
    /// </summary>
    public int Code { get; private set; }

    /// <summary>
    /// Retorna todos os valores definidos do tipo de enumeração <typeparamref name="T"/>.
    /// Útil para listar todas as instâncias estáticas declaradas em classes derivadas.
    /// </summary>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Select(f => f.GetValue(null))
                 .Cast<T>();

    /// <summary>
    /// Retorna o nome da enumeração.
    /// </summary>
    public override string ToString() => Name;

    /// <summary>
    /// Compara esta instância com outra para igualdade.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
            return false;

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Code.Equals(otherValue.Code);

        return typeMatches && valueMatches;
    }

    /// <summary>
    /// Retorna o código hash desta instância.
    /// </summary>
    public override int GetHashCode() => Code.GetHashCode();

    /// <summary>
    /// Compara a instância atual com outra baseado no valor do código.
    /// </summary>
    /// <param name="obj">O objeto a ser comparado.</param>
    /// <returns>
    /// Um valor que indica a ordem relativa dos objetos comparados.
    /// Retorna um número negativo se esta instância for menor que o objeto,
    /// zero se forem iguais, ou um número positivo se esta instância for maior.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando <paramref name="obj"/> não é do tipo <see cref="Enumeration"/>.
    /// </exception>
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (obj is not Enumeration other)
            throw new ArgumentException("Object must be of type Enumeration.", nameof(obj));
        return Code.CompareTo(other.Code);
    }
}
