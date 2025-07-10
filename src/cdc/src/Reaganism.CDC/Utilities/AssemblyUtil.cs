using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;

using ICSharpCode.Decompiler.Metadata;

namespace Reaganism.CDC.Utilities;

internal static class AssemblyUtil
{
    private sealed class AttributeTypeProvider : ICustomAttributeTypeProvider<object>
    {
        object ISimpleTypeProvider<object>.GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            return null!;
        }

        object ISimpleTypeProvider<object>.GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            throw new NotImplementedException();
        }

        object ISimpleTypeProvider<object>.GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            throw new NotImplementedException();
        }

        object ISZArrayTypeProvider<object>.GetSZArrayType(object elementType)
        {
            throw new NotImplementedException();
        }

        object ICustomAttributeTypeProvider<object>.GetSystemType()
        {
            throw new NotImplementedException();
        }

        object ICustomAttributeTypeProvider<object>.GetTypeFromSerializedName(string name)
        {
            throw new NotImplementedException();
        }

        PrimitiveTypeCode ICustomAttributeTypeProvider<object>.GetUnderlyingEnumType(object type)
        {
            throw new NotImplementedException();
        }

        bool ICustomAttributeTypeProvider<object>.IsSystemType(object type)
        {
            throw new NotImplementedException();
        }
    }

    private static readonly Dictionary<string, string> assembly_title_cache = [];

    private static readonly string[] known_attributes =
    [
        nameof(AssemblyCompanyAttribute),
        nameof(AssemblyCopyrightAttribute),
        nameof(AssemblyTitleAttribute),
    ];

    public static string GetAssemblyTitle(MetadataFile metadataFile)
    {
        if (assembly_title_cache.TryGetValue(metadataFile.FileName, out var title))
        {
            return title;
        }

        var customAttributes = GetCustomAttributes(metadataFile);
        if (customAttributes.TryGetValue(nameof(AssemblyTitleAttribute), out title))
        {
            return assembly_title_cache[metadataFile.FileName] = title;
        }

        return assembly_title_cache[metadataFile.FileName] = metadataFile.FileName.Replace(".dll", "");
    }

    public static Dictionary<string, string> GetCustomAttributes(MetadataFile metadataFile)
    {
        var dict = new Dictionary<string, string>();

        var reader     = metadataFile.Metadata;
        var attributes = reader.GetAssemblyDefinition().GetCustomAttributes().Select(reader.GetCustomAttribute);

        foreach (var attribute in attributes)
        {
            var constructor       = reader.GetMemberReference((MemberReferenceHandle)attribute.Constructor);
            var attributeTypeName = reader.GetString(reader.GetTypeReference((TypeReferenceHandle)constructor.Parent).Name);

            // TODO: Review these.
            if (!known_attributes.Contains(attributeTypeName))
            {
                continue;
            }

            var value = attribute.DecodeValue(new AttributeTypeProvider());
            dict[attributeTypeName] = (string)value.FixedArguments.Single().Value!;
        }

        return dict;
    }
}