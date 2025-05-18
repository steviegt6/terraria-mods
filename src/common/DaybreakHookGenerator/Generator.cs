using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Rocks;

using MonoMod.Utils;

namespace DaybreakHookGenerator;

public sealed class Generator(ModuleDefinition module, TypeDefinition type)
{
    private static readonly Dictionary<string, string> csharp_keyword_map = new()
    {
        { typeof(bool).FullName!, "bool" },
        { typeof(byte).FullName!, "byte" },
        { typeof(sbyte).FullName!, "sbyte" },
        { typeof(short).FullName!, "short" },
        { typeof(ushort).FullName!, "ushort" },
        { typeof(int).FullName!, "int" },
        { typeof(uint).FullName!, "uint" },
        { typeof(long).FullName!, "long" },
        { typeof(ulong).FullName!, "ulong" },
        { typeof(float).FullName!, "float" },
        { typeof(double).FullName!, "double" },
        { typeof(decimal).FullName!, "decimal" },
        { typeof(string).FullName!, "string" },
        { typeof(object).FullName!, "object" },
        { typeof(void).FullName!, "void" },
        { typeof(char).FullName!, "char" },
        { typeof(nint).FullName!, "nint" },
        { typeof(nuint).FullName!, "nuint" },
    };

    public string BuildType(string typeNamespace, string typeName, string[] excludedHooks)
    {
        var sb = new StringBuilder();

        var hooks = ResolveHooksFromType(type, excludedHooks);

        sb.AppendLine($"namespace {typeNamespace};");
        sb.AppendLine();
        sb.AppendLine("using System.Linq;");
        sb.AppendLine();
        sb.AppendLine("// ReSharper disable PartialTypeWithSinglePart");
        sb.AppendLine("// ReSharper disable UnusedType.Global");
        sb.AppendLine("#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member");
        sb.AppendLine();
        sb.AppendLine($"// Hooks to generate for '{type.FullName}':");
        foreach (var hook in hooks)
        {
            sb.AppendLine($"//     {hook}");
        }
        sb.AppendLine($"public static partial class {typeName}");
        sb.AppendLine("{");
        var ranOnce = false;
        foreach (var hook in hooks)
        {
            if (ranOnce)
            {
                sb.AppendLine();
            }
            ranOnce = true;

            sb.Append(BuildHook(hook, hasOverloads: type.GetMethods().Count(x => x.Name == hook.Name) > 1));
        }
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string BuildHook(MethodDefinition method, bool hasOverloads)
    {
        var sb = new StringBuilder();

        // TODO: This logic for resolving overloads is *very* naive.  For a more
        //       reliable approach, see how MonoMod does it:
        // https://github.com/MonoMod/MonoMod/blob/reorganize/src/MonoMod.RuntimeDetour.HookGen/HookGenerator.cs#L234

        var name = method.Name;
        if (hasOverloads)
        {
            foreach (var param in method.Parameters)
            {
                name += "_" + GetFullTypeNameOrCSharpKeyword(param.ParameterType, includeRefPrefix: false).Split('.').Last();
            }
        }

        sb.AppendLine($"    public static partial class {name}");
        sb.AppendLine("    {");
        sb.AppendLine(GetDescriptionForMethod(method));
        sb.AppendLine();
        sb.AppendLine("        public static event Definition? Event;");
        sb.AppendLine();
        sb.AppendLine("        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()");
        sb.AppendLine("        {");
        sb.AppendLine("            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];");
        sb.AppendLine("        }");
        sb.AppendLine("    }");

        return sb.ToString();
    }

    private static MethodDefinition[] ResolveHooksFromType(TypeDefinition typeDef, string[] excludedHooks)
    {
        var methods = typeDef.GetMethods().Where(
            x => x is
                 {
                     IsPublic: true, // Is accessible (ignore protected, too)
                     IsVirtual: true, // Is overridable
                     IsFinal: false, // Is not sealed
                 }
              && !excludedHooks.Contains(x.Name)
              && x.GetCustomAttribute(typeof(ObsoleteAttribute).FullName!) is null
        );

        return methods.ToArray();
    }

    private static string GetDescriptionForMethod(MethodDefinition method)
    {
        // generate public delegate <return type> Description(<parameters>), acccount
        // for ref, in, out, etc.

        var sb = new StringBuilder();

        sb.Append($"        public delegate {GetFullTypeNameOrCSharpKeyword(method.ReturnType, includeRefPrefix: true)} Definition(");

        var parameters = method.Parameters;
        if (parameters.Count != 0)
        {
            sb.AppendLine();

            for (var i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                sb.Append($"            {GetParameterDefinition(parameter)}");
                if (i < parameters.Count - 1)
                {
                    sb.AppendLine(",");
                }
                else
                {
                    sb.AppendLine();
                }
            }

            sb.Append("        );");
        }
        else
        {
            sb.Append(");");
        }

        return sb.ToString();
    }

    private static string GetParameterDefinition(ParameterDefinition parameter)
    {
        var prefix = GetReferencePrefix(parameter);
        var type = GetFullTypeNameOrCSharpKeyword(parameter.ParameterType, includeRefPrefix: false);
        var name = parameter.Name;

        return prefix + type + ' ' + name;
    }

    private static string GetFullTypeNameOrCSharpKeyword(TypeReference type, bool includeRefPrefix)
    {
        var prefix = includeRefPrefix && type.IsByReference ? "ref " : "";

        if (type is GenericInstanceType genericType)
        {
            // Special case for System.Nullable
            if (genericType.ElementType.FullName == "System.Nullable`1")
            {
                return prefix + GetFullTypeNameOrCSharpKeyword(genericType.GenericArguments[0], includeRefPrefix) + "?";
            }

            var genericArgs = string.Join(", ", genericType.GenericArguments.Select(arg => GetFullTypeNameOrCSharpKeyword(arg, includeRefPrefix: false)));
            var baseTypeName = GetCSharpRepresentation(genericType.ElementType.FullName);
            return prefix + $"{baseTypeName}<{genericArgs}>";
        }

        var csharpName = GetCSharpRepresentation(type.FullName);

        if (csharp_keyword_map.TryGetValue(csharpName, out var keyword))
        {
            return prefix + keyword;
        }

        return prefix + csharpName;
    }

    private static string GetReferencePrefix(ParameterDefinition parameter)
    {
        if (parameter.IsOut)
        {
            return "out ";
        }

        if (parameter.IsIn)
        {
            return "in ";
        }

        if (parameter.ParameterType.IsByReference)
        {
            return "ref ";
        }

        return "";
    }

    private static string GetCSharpRepresentation(string fullName)
    {
        fullName = fullName.Replace('/', '.');
        fullName = fullName.Replace('+', '.');

        if (fullName.EndsWith('&'))
        {
            fullName = fullName[..^1];
        }

        // Generic parameters are denoted as `n at the end of the name where n
        // is the # of generic parameters.
        var index = fullName.IndexOf('`');
        if (index != -1)
        {
            fullName = fullName[..index];
        }

        return fullName;
    }
}