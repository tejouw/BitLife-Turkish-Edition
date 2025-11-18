#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace BitLifeTR.Editor
{
    /// <summary>
    /// Build configuration and automation for Android.
    /// </summary>
    public static class BuildConfig
    {
        private const string CompanyName = "BitLifeTR";
        private const string ProductName = "BitLife Türkiye";
        private const string BundleIdentifier = "com.bitlifetr.game";
        private const string Version = "1.0.0";
        private const int BundleVersionCode = 1;

        [MenuItem("Build/Configure Android Settings")]
        public static void ConfigureAndroidSettings()
        {
            // Company and product
            PlayerSettings.companyName = CompanyName;
            PlayerSettings.productName = ProductName;

            // Bundle identifier
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, BundleIdentifier);

            // Version
            PlayerSettings.bundleVersion = Version;
            PlayerSettings.Android.bundleVersionCode = BundleVersionCode;

            // Android specific
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;

            // Architecture
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;

            // Graphics
            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] {
                UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3,
                UnityEngine.Rendering.GraphicsDeviceType.Vulkan
            });

            // Other settings
            PlayerSettings.Android.forceInternetPermission = false;
            PlayerSettings.Android.forceSDCardPermission = false;

            // Resolution and presentation
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.allowedAutorotateToPortrait = true;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = true;
            PlayerSettings.allowedAutorotateToLandscapeLeft = false;
            PlayerSettings.allowedAutorotateToLandscapeRight = false;

            // Splash screen
            PlayerSettings.SplashScreen.show = true;
            PlayerSettings.SplashScreen.showUnityLogo = false;

            Debug.Log("[BuildConfig] Android settings configured successfully!");
        }

        [MenuItem("Build/Build Android APK")]
        public static void BuildAndroidAPK()
        {
            // Configure settings first
            ConfigureAndroidSettings();

            // Build options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = GetEnabledScenes(),
                locationPathName = "Builds/Android/BitLifeTR.apk",
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            // Build
            var report = BuildPipeline.BuildPlayer(buildOptions);

            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[BuildConfig] Build succeeded: {report.summary.outputPath}");
                Debug.Log($"[BuildConfig] Build size: {report.summary.totalSize / 1024 / 1024} MB");
            }
            else
            {
                Debug.LogError($"[BuildConfig] Build failed with {report.summary.totalErrors} errors");
            }
        }

        [MenuItem("Build/Build Android AAB (Play Store)")]
        public static void BuildAndroidAAB()
        {
            // Configure settings first
            ConfigureAndroidSettings();

            // Enable AAB format
            EditorUserBuildSettings.buildAppBundle = true;

            // Build options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = GetEnabledScenes(),
                locationPathName = "Builds/Android/BitLifeTR.aab",
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            // Build
            var report = BuildPipeline.BuildPlayer(buildOptions);

            // Reset to APK
            EditorUserBuildSettings.buildAppBundle = false;

            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[BuildConfig] AAB build succeeded: {report.summary.outputPath}");
            }
            else
            {
                Debug.LogError($"[BuildConfig] AAB build failed with {report.summary.totalErrors} errors");
            }
        }

        private static string[] GetEnabledScenes()
        {
            var scenes = new System.Collections.Generic.List<string>();

            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    scenes.Add(scene.path);
                }
            }

            // If no scenes in build settings, use a default
            if (scenes.Count == 0)
            {
                scenes.Add("Assets/Scenes/Main.unity");
            }

            return scenes.ToArray();
        }

        [MenuItem("Build/Open Builds Folder")]
        public static void OpenBuildsFolder()
        {
            var path = System.IO.Path.Combine(Application.dataPath, "../Builds/Android");

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            EditorUtility.RevealInFinder(path);
        }

        [MenuItem("Build/Increment Version")]
        public static void IncrementVersion()
        {
            // Increment bundle version code
            PlayerSettings.Android.bundleVersionCode++;

            // Parse and increment version
            var versionParts = PlayerSettings.bundleVersion.Split('.');
            if (versionParts.Length >= 3)
            {
                int patch = int.Parse(versionParts[2]) + 1;
                PlayerSettings.bundleVersion = $"{versionParts[0]}.{versionParts[1]}.{patch}";
            }

            Debug.Log($"[BuildConfig] Version incremented to {PlayerSettings.bundleVersion} ({PlayerSettings.Android.bundleVersionCode})");
        }
    }
}
#endif
