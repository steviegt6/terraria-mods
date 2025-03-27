using Mono.Cecil;
using Mono.Cecil.Cil;

using MonoMod.Cil;

namespace Tomat.TML.Mod.Nightshade.Common.Utilities;

internal static class CecilUtil
{
    public static VariableDefinition AddVariable<T>(this ILContext il)
    {
        return AddVariable(il.Body, il.Import(typeof(T)));
    }
    
    public static VariableDefinition AddVariable(this ILContext il, TypeReference type)
    {
        return AddVariable(il.Body, type);
    }
    
    public static VariableDefinition AddVariable(this ILCursor c, TypeReference type)
    {
        return AddVariable(c.Body, type);
    }
    
    private static VariableDefinition AddVariable(MethodBody body, TypeReference type)
    {
        var variable = new VariableDefinition(type);
        {
            body.Variables.Add(variable);
        }

        return variable;
    }
}