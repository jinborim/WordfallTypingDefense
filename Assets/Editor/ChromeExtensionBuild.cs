using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class ChromeExtensionBuild
{
    private const string OutputPath = "ChromeExtension/UnityWebGL";

    [MenuItem("Wordfall/Build WebGL For Chrome Extension")]
    public static void BuildWebGLForChromeExtension()
    {
        var scenes = new[]
        {
            "Assets/Scenes/MainMenuScene.unity",
            "Assets/Scenes/LobbyScene.unity",
            "Assets/Scenes/GameScene.unity"
        };

        Directory.CreateDirectory(OutputPath);
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = OutputPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        });

        if (report.summary.result != BuildResult.Succeeded)
            throw new BuildFailedException($"WebGL build failed: {report.summary.result}");

        Debug.Log($"WebGL build completed: {OutputPath}");
    }
}
