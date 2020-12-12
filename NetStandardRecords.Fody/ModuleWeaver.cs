using Fody;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

// https://github.com/dotnet/runtime/issues/34978#issuecomment-614845405

public class ModuleWeaver : BaseModuleWeaver
{
    public override void Execute()
    {
        var types = ModuleDefinition.Types.Concat(ModuleDefinition.Types.SelectMany(t => t.NestedTypes));

        foreach (var type in types)
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
        }
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "System.Runtime";
    }

    public override bool ShouldCleanReference => true;
}
