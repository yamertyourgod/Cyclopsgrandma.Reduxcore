using System;
using System.IO;
using UnityEditor;
using UnityEngine;
public class ReduxModuleGenerator : EditorWindow
{
    private string _moduleRootPath = @".//Assets//Code//MainLogic";
    private string _moduleName;
    private string _reduxSamplePath = @".//Assets//Code//ThirdParty//ReduxModuleSample";

    [MenuItem("Redux/Create New Module")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<ReduxModuleGenerator>("Create Redux Module");
    }

    // Update is called once per frame
    void OnGUI()
    {
        GUILayout.Label("Enter module path", EditorStyles.boldLabel);
        _moduleRootPath = EditorGUILayout.TextField("Module root path", _moduleRootPath);

        GUILayout.Label("Enter redux sample path", EditorStyles.boldLabel);
        _reduxSamplePath = EditorGUILayout.TextField("Redux sample path", _reduxSamplePath);

        GUILayout.Label("Enter module name", EditorStyles.boldLabel);
        _moduleName = EditorGUILayout.TextField("Module name", _moduleName);

        if(GUILayout.Button("Create!"))
        {
            GenerateModule();
        }

    }

    void GenerateModule()
    {
        if (string.IsNullOrEmpty(_moduleName) || string.IsNullOrEmpty(_moduleRootPath) || string.IsNullOrEmpty(_reduxSamplePath))
            return;
        _moduleName = _moduleName.Replace(" ", "");
        if (Directory.Exists(_reduxSamplePath))
        {
            var modulePath = $"{_moduleRootPath}//{_moduleName}";

            DirectoryCopy(_reduxSamplePath, modulePath, true, _moduleName);
            RenameSample(_moduleName, modulePath);
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("Error: Wrong path to sample");
        }
    }

    private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, string moduleName)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name).Replace("Sample",moduleName);
            file.CopyTo(temppath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs, moduleName);
            }
        }
    }

    private void RenameSample(string moduleName, string rootfolder)
    {
        string[] files = Directory.GetFiles(rootfolder, "*.*", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            try
            {
                string contents = File.ReadAllText(file);
                contents = contents.Replace(@"Sample",moduleName);
                // Make files writable
                File.SetAttributes(file, FileAttributes.Normal);

                File.WriteAllText(file, contents);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
