using Fody;
using Mono.Cecil;
using System.Collections.Generic;

// https://github.com/dotnet/runtime/issues/34978#issuecomment-614845405

public class ModuleWeaver : BaseModuleWeaver
{
    public override void Execute()
    {
        foreach (var type in ModuleDefinition.Types)
        {
            FixType(type);
        }
    }

    private void FixType(TypeDefinition type)
    {
        foreach (var prop in type.Properties)
        {
            if (prop is null ||
                prop.SetMethod is null ||
                prop.SetMethod.ReturnType is not RequiredModifierType setReturnType ||
                setReturnType.ModifierType.FullName != "System.Runtime.CompilerServices.IsExternalInit")
            {
                continue;
            }

            prop.SetMethod.ReturnType = setReturnType.ElementType;
        }

        foreach (var nestedType in type.NestedTypes)
        {
            FixType(nestedType);
        }
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "System.Runtime";
    }

    public override bool ShouldCleanReference => true;
}
