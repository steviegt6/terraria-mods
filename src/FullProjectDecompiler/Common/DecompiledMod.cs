namespace Tomat.TML.Mod.FullProjectDecompiler.Common;

public readonly record struct DecompiledMod(
    string  DllPath,
    string? PdbPath,
    string? XmlPath
);