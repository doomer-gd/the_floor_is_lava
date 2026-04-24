using UnityEditor;

public static class ForceRecompile
{
    [MenuItem("Services/Force Recompile")]
    public static void Recompile()
    {
        UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
    }
}