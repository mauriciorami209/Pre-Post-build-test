using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class UpdateBuildManifest : IPostprocessBuildWithReport
{
    // Set the callback order (lower runs first)
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
        // Define the path for the UnityCloudBuildManifest.json file
        string manifestPath = Path.Combine(report.summary.outputPath, "Assets/__UnityCloud__/Resources/UnityCloudBuildManifest.json");

        // Ensure the directory exists
        string manifestDirectory = Path.GetDirectoryName(manifestPath);
        if (!Directory.Exists(manifestDirectory))
        {
            Directory.CreateDirectory(manifestDirectory);
        }

        // Define the JSON content to write or update
        var manifestContent = new
        {
            projectName = Application.productName,
            //buildNumber = report.summary.buildNumber,
            buildTarget = report.summary.platform.ToString(),
            buildDate = System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            scmBranch = "main" // Replace with your branch logic if needed
        };

        // Convert to JSON
        string jsonContent = JsonUtility.ToJson(manifestContent, true);

        // Write or overwrite the JSON file
        File.WriteAllText(manifestPath, jsonContent);

        Debug.Log($"UnityCloudBuildManifest.json updated at: {manifestPath}");
    }
}
