using UnityEditor;
using UnityEditor.Build;

namespace Core.Editor.Tuner.Configs
{
    public struct BuildTargetConfig
    {
        public BuildTarget BuildTarget
        {
            get; private set;
        }
        
        public Il2CppCodeGeneration CodeGeneration
        {
            get; private set;
        }

        public ManagedStrippingLevel StrippingLevel
        {
            get; private set;
        }

        public BuildTargetConfig(BuildTarget buildTarget, Il2CppCodeGeneration codeGeneration, 
            ManagedStrippingLevel strippingLevel)
        {
            BuildTarget = buildTarget;
            CodeGeneration = codeGeneration;
            StrippingLevel = strippingLevel;
        }
    }
}