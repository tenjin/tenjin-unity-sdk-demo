using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System;
using System.IO;
using UnityEditor.Compilation;
using System.Linq;
using UnityEditor.Build.Content;
using UnityEditor.Callbacks;
using Assembly = System.Reflection.Assembly;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

namespace Tenjin
{
    class TenjinEditorPrefs : IPreprocessBuildWithReport
    {
        private static string tenjin_mopub = "tenjin_mopub_enabled";
        private static string tenjin_facebook = "tenjin_facebook_enabled";

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            TenjinEditorPrefs.Update3rdPartyIntegrations();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            Update3rdPartyIntegrations();
        }

        private static void Update3rdPartyIntegrations()
        {
            UpdateMoPub();
            UpdateFacebook();
        }

        [PostProcessBuild(0)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) 
        {
            ProcessIosBuild(target, pathToBuiltProject);
        }

        private static void ProcessIosBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.iOS)
            {
                if (PlayerSettings.iOS.sdkVersion == iOSSdkVersion.SimulatorSDK)
                {
                   Debug.Log("Using simulator sdk - delete non universal tenjin lib from generated xcode project");
                   RemoveFileFromXcodeProject("Libraries/Plugins/iOS/libTenjinSDK.a", pathToBuiltProject); 
                }
                else
                {
                   Debug.Log("Using device sdk - delete universal tenjin lib from generated xcode project");
                   RemoveFileFromXcodeProject("Libraries/Plugins/iOS/libTenjinSDKUniversal.a", pathToBuiltProject); 
                }
            }
        }

        private static void RemoveFileFromXcodeProject(string filePath, string pathToBuiltProject)
        {
#if UNITY_IOS
            var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            
            PBXProject pbxProject = new PBXProject ();
            pbxProject.ReadFromFile (projectPath);
            
            var fileToRemove = pbxProject.FindFileGuidByProjectPath(filePath);
            pbxProject.RemoveFile(fileToRemove);
            
            File.WriteAllText (projectPath, pbxProject.WriteToString ());
#endif
        }
        
        private static void UpdateDefines(string entry, bool enabled, BuildTargetGroup[] groups)
        {
            foreach (var group in groups)
            {
                var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group)
                                        .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Where(d => d != entry);

                if (enabled)
                    defines = defines.Concat(new[] { entry });
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defines.ToArray()));
            }
        }

        #region 3rd Party Lib Detection

        /// <summary>
        /// Sets the scripting define symbol `tenjin_facebook_enabled` to true if Facebook classes are detected within the Unity project
        /// </summary>
        private static void UpdateFacebook()
        {
            var facebookTypes = new string[]{"Facebook", "FB"};
            if(TypeExists(facebookTypes))
            {
                UpdateDefines(tenjin_facebook, true, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
            }
            else
            {
                UpdateDefines(tenjin_facebook, false, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
            }
        }
        

        /// <summary>
        /// Sets the scripting define symbol `tenjin_mopub_enabled` to true if MoPub classes are detected within the Unity project
        /// </summary>
        private static void UpdateMoPub()
        {
            var mopubTypes = new string[]{"MoPubBase", "MoPubManager"};
            if(TypeExists(mopubTypes))
            {
                UpdateDefines(tenjin_mopub, true, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
            }
            else
            {
                UpdateDefines(tenjin_mopub, false, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
            }
        }

        private static bool TypeExists(params string[] types)
        {
            if (types == null || types.Length == 0)
                return false;
            
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (types.Any(type => assembly.GetType(type) != null))
                    return true;
            }

            return false;
        }
        
        #endregion
    }
}