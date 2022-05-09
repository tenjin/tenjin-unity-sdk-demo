//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System.Collections.Generic;

public class BuildPostProcessor : MonoBehaviour
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            BuildiOS(path: path);
        }
        else if (buildTarget == BuildTarget.Android)
        {
            BuildAndroid(path: path);
        }
    }

    private static void BuildAndroid(string path = "")
    {
        Debug.Log("TenjinSDK: Starting Android Build");
    }

    private static void BuildiOS(string path = "")
    {
#if UNITY_IOS
        Debug.Log("TenjinSDK: Starting iOS Build");

        string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject project = new PBXProject();
        project.ReadFromFile(projectPath);

#if UNITY_2019_3_OR_NEWER
        string buildTarget = project.GetUnityFrameworkTargetGuid();
#else
    string buildTarget = project.TargetGuidByName("Unity-iPhone");
#endif

        List<string> frameworks = new List<string>();

        frameworks.Add("AdServices.framework");
        frameworks.Add("AdSupport.framework");
        frameworks.Add("AppTrackingTransparency.framework");
        frameworks.Add("iAd.framework");
        frameworks.Add("StoreKit.framework");

        foreach (string framework in frameworks)
        {
            Debug.Log("TenjinSDK: Adding framework: " + framework);
            project.AddFrameworkToProject(buildTarget, framework, true);
        }

        Debug.Log("TenjinSDK: Adding -ObjC flag to other linker flags (OTHER_LDFLAGS)");
        project.AddBuildProperty(buildTarget, "OTHER_LDFLAGS", "-ObjC");

        File.WriteAllText(projectPath, project.WriteToString());
#endif  
    }
}
