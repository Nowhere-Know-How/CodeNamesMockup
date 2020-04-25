#if UNITY_EDITOR
// Editor specific code here
using UnityEditor;
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;
using System.IO;


// Output the build size or a failure depending on BuildPlayer.

public class BuildPlayer
{
    static string project_name = "codenames";
    static string out_dir = @"bin\";
    
    static string exe_name = out_dir + project_name + @".exe";

    static string[] scenes = new[] {
            @"Assets\_NoWhereKnowHow\_Assets\Scenes\MainMenu.unity",
            @"Assets\_NoWhereKnowHow\_Assets\Scenes\Template_Lobby.unity",
        };

    static string data_dir = out_dir + project_name + "_Data/_NoWhereKnowHow/Data/";
    static string sqlite_db_path = @"Assets\_NoWhereKnowHow\Data\";
    static string sqlite_db_file = "codenames.db";

    #if UNITY_EDITOR
    [MenuItem("Build/Development %b")]
    public static void ClientBuild()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;

        buildPlayerOptions.locationPathName = exe_name;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.Development;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            
            Directory.CreateDirectory(data_dir);
            File.Copy(sqlite_db_path + sqlite_db_file, data_dir + sqlite_db_file);
            Debug.Log("Development build succeeded: " + summary.totalSize + " bytes");


        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Development build failed");
        }
    }

   
    #endif

}