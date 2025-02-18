using UnityEditor;
using UnityEditor.Compilation;

namespace _ImmersiveGames.Scripts.Utils.Hotkeys.Editor {
    public static class CompileProject {
        [MenuItem("File/Compile _F5")]
        private static void Compile() {
            CompilationPipeline.RequestScriptCompilation(RequestScriptCompilationOptions.CleanBuildCache);
        }
    }
}