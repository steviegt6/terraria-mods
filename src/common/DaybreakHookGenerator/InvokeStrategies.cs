using System.Linq;
using System.Text;

using Mono.Cecil;

namespace DaybreakHookGenerator;

public abstract class InvokeStrategy
{
    public abstract string GenerateMethodBody(MethodDefinition method);
}

internal sealed class SimpleVoidInvokeStrategy : InvokeStrategy
{
    public override string GenerateMethodBody(MethodDefinition method)
    {
        var sb = new StringBuilder();

        if (method.Parameters.Count > 0)
        {
            sb.AppendLine("            Event?.Invoke(self, " + string.Join(", ", method.Parameters.Select(Generator.GetParameterReference)) + ");");
        }
        else
        {
            sb.AppendLine("            Event?.Invoke(self);");
        }

        return sb.ToString();
    }
}