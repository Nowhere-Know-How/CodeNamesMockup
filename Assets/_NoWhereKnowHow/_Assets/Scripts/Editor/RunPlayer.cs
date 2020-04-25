#if UNITY_EDITOR
// Editor specific code here
using UnityEditor;
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;
using System.Diagnostics;


// Output the build size or a failure depending on BuildPlayer.

public class RunPlayer
{
    static string project_name = "codenames";
    static string out_dir = @"bin\";
    
    static string exe_name = out_dir + project_name + @".exe";

    #if UNITY_EDITOR
    [MenuItem("Run/Latest %r")]
    public static void RunLatestBuild()
    {
        try { 
            Process myProcess = new Process();
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
            string path = exe_name;
            myProcess.StartInfo.Arguments = "/c" + path;
            myProcess.EnableRaisingEvents = true;
            myProcess.Start();
            //myProcess.WaitForExit();
            //int ExitCode = myProcess.ExitCode;
            //print(ExitCode);
        } catch (System.Exception e){
            UnityEngine.Debug.Log(e);
        }
    }

   
    #endif

}