/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Unity Technologies.
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Unity.CodeEditor;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.PackageManager;
using UnityEditorInternal;
using UnityEngine;

namespace Microsoft.Unity.VisualStudio.Editor
{
    public enum ScriptingLanguage
    {
        None,
        CSharp
    }

    public interface IGenerator {
        bool SyncIfNeeded(IEnumerable<string> affectedFiles, IEnumerable<string> reimportedFiles);
        void Sync();
        bool HasSolutionBeenGenerated();
        string SolutionFile();
        string ProjectDirectory { get; }
        void GenerateAll(bool generateAll);
        bool IsSupportedFile(string path);
    }

    public interface IAssemblyNameProvider
    {
        string GetAssemblyNameFromScriptPath(string path);
        IEnumerable<Assembly> GetAllAssemblies(Func<string, bool> shouldFileBePartOfSolution);
        IEnumerable<string> GetAllAssetPaths();
        UnityEditor.PackageManager.PackageInfo FindForAssetPath(string assetPath);
    }

    public struct TestSettings {
        public bool ShouldSync;
        public Dictionary<string, string> SyncPath;
    }

    class AssemblyNameProvider : IAssemblyNameProvider
    {
        public string GetAssemblyNameFromScriptPath(string path)
        {
            return CompilationPipeline.GetAssemblyNameFromScriptPath(path);
        }

        public IEnumerable<Assembly> GetAllAssemblies(Func<string, bool> shouldFileBePartOfSolution)
        {
            return CompilationPipeline.GetAssemblies().Where(i => 0 < i.sourceFiles.Length && i.sourceFiles.Any(shouldFileBePartOfSolution));
        }

        public IEnumerable<string> GetAllAssetPaths()
        {
            return AssetDatabase.GetAllAssetPaths();
        }

        public UnityEditor.PackageManager.PackageInfo FindForAssetPath(string assetPath)
        {
            return UnityEditor.PackageManager.PackageInfo.FindForAssetPath(assetPath);
        }
    }

    public class ProjectGeneration : IGenerator
    {
        public static readonly string MSBuildNamespaceUri = "http://schemas.microsoft.com/developer/msbuild/2003";

        const string k_WindowsNewline = "\r\n";

        string m_SolutionProjectEntryTemplate = string.Join("\r\n",
            @"Project(""{{{0}}}"") = ""{1}"", ""{2}"", ""{{{3}}}""",
            @"EndProject").Replace("    ", "\t");

        string m_SolutionProjectConfigurationTemplate = string.Join("\r\n",
            @"        {{{0}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU",
            @"        {{{0}}}.Debug|Any CPU.Build.0 = Debug|Any CPU",
            @"        {{{0}}}.Release|Any CPU.ActiveCfg = Release|Any CPU",
            @"        {{{0}}}.Release|Any CPU.Build.0 = Release|Any CPU").Replace("    ", "\t");

        static readonly string[] k_ReimportSyncExtensions = { ".dll", ".asmdef" };

        static readonly Regex k_ScriptReferenceExpression = new Regex(
            @"^Library.ScriptAssemblies.(?<dllname>(?<project>.*)\.dll$)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        string[] m_ProjectSupportedExtensions = new string[0];
        string[] m_BuiltinSupportedExtensions = new string[0];

        public string ProjectDirectory { get; }

        public TestSettings Settings { get; set; }

        readonly string m_ProjectName;
        readonly IAssemblyNameProvider m_AssemblyNameProvider;
        bool m_ShouldGenerateAll;

        public ProjectGeneration() : this(Directory.GetParent(Application.dataPath).FullName,  new AssemblyNameProvider())
        {
        }

        public ProjectGeneration(string tempDirectory) : this(tempDirectory, new AssemblyNameProvider()) {
        }

        public ProjectGeneration(string tempDirectory, IAssemblyNameProvider assemblyNameProvider) {
            Settings = new TestSettings { ShouldSync = true };
            ProjectDirectory = tempDirectory.Replace('\\', '/');
            m_ProjectName = Path.GetFileName(ProjectDirectory);
            m_AssemblyNameProvider = assemblyNameProvider;

            SetupProjectSupportedExtensions();
        }

        public void GenerateAll(bool generateAll)
        {
            m_ShouldGenerateAll = generateAll;
        }

        /// <summary>
        /// Syncs the scripting solution if any affected files are relevant.
        /// </summary>
        /// <returns>
        /// Whether the solution was synced.
        /// </returns>
        /// <param name='affectedFiles'>
        /// A set of files whose status has changed
        /// </param>
        /// <param name="reimportedFiles">
        /// A set of files that got reimported
        /// </param>
        public bool SyncIfNeeded(IEnumerable<string> affectedFiles, IEnumerable<string> reimportedFiles)
        {
            SetupProjectSupportedExtensions();

            // Don't sync if we haven't synced before
            if (HasSolutionBeenGenerated() && HasFilesBeenModified(affectedFiles, reimportedFiles))
            {
                Sync();
                return true;
            }
            return false;
        }

        bool HasFilesBeenModified(IEnumerable<string> affectedFiles, IEnumerable<string> reimportedFiles)
        {
            return affectedFiles.Any(ShouldFileBePartOfSolution) || reimportedFiles.Any(ShouldSyncOnReimportedAsset);
        }

        static bool ShouldSyncOnReimportedAsset(string asset)
        {
            return k_ReimportSyncExtensions.Contains(new FileInfo(asset).Extension);
        }

        public void Sync()
        {
            SetupProjectSupportedExtensions();
            bool externalCodeAlreadyGeneratedProjects = OnPreGeneratingCSProjectFiles();

            if (!externalCodeAlreadyGeneratedProjects)
            {
                GenerateAndWriteSolutionAndProjects();
            }
            OnGeneratedCSProjectFiles();
        }

        public bool HasSolutionBeenGenerated()
        {
            return File.Exists(SolutionFile());
        }

        void SetupProjectSupportedExtensions()
        {
            m_ProjectSupportedExtensions = EditorSettings.projectGenerationUserExtensions;
            m_BuiltinSupportedExtensions = EditorSettings.projectGenerationBuiltinExtensions;
        }

        bool ShouldFileBePartOfSolution(string file)
        {
            // Exclude files coming from packages except if they are internalized.
            if (!m_ShouldGenerateAll && IsInternalizedPackagePath(file))
            {
                return false;
            }

            return IsSupportedFile(file);
        }

        static string GetExtensionWithoutDot(string path)
        {
            // Prevent re-processing and information loss
            if (!Path.HasExtension(path))
                return path;

            return Path
                .GetExtension(path)
                .TrimStart('.')
                .ToLower();
        }

        public bool IsSupportedFile(string path)
        {
            var extension = GetExtensionWithoutDot(path);

            // Dll's are not scripts but still need to be included
            if (extension == "dll")
                return true;

            if (extension == "asmdef")
                return true;

            if (m_BuiltinSupportedExtensions.Contains(extension))
                return true;

            if (m_ProjectSupportedExtensions.Contains(extension))
                return true;

            return false;
        }

        static ScriptingLanguage ScriptingLanguageFor(Assembly island)
        {
            var files = island.sourceFiles;

            if (files.Length == 0)
                return ScriptingLanguage.None;

            return ScriptingLanguageFor(files[0]);
        }

        static ScriptingLanguage ScriptingLanguageFor(string path)
        {
            return GetExtensionWithoutDot(path) == "cs" ? ScriptingLanguage.CSharp : ScriptingLanguage.None;
        }

        static List<Type> SafeGetTypes(System.Reflection.Assembly a)
        {
            var ret = new List<Type>();

            try
            {
                ret = a.GetTypes().ToList();
            }
            catch (System.Reflection.ReflectionTypeLoadException rtl)
            {
                ret = rtl.Types.ToList();
            }
            catch (Exception)
            {
                return new List<Type>();
            }

            return ret.Where(r => r != null).ToList();
        }

        public void GenerateAndWriteSolutionAndProjects()
        {
            // Only synchronize islands that have associated source files and ones that we actually want in the project.
            // This also filters out DLLs coming from .asmdef files in packages.
            var assemblies = m_AssemblyNameProvider.GetAllAssemblies(ShouldFileBePartOfSolution);

            var allAssetProjectParts = GenerateAllAssetProjectParts();

            var monoIslands = assemblies.ToList();

            SyncSolution(monoIslands);
            var allProjectIslands = RelevantIslandsForMode(monoIslands).ToList();
            foreach (Assembly assembly in allProjectIslands)
            {
                var responseFileData = ParseResponseFileData(assembly);
                SyncProject(assembly, allAssetProjectParts, responseFileData, allProjectIslands);
            }
        }

        IEnumerable<ResponseFileData> ParseResponseFileData(Assembly assembly)
        {
            var systemReferenceDirectories = CompilationPipeline.GetSystemAssemblyDirectories(assembly.compilerOptions.ApiCompatibilityLevel);

            Dictionary<string, ResponseFileData> responseFilesData = assembly.compilerOptions.ResponseFiles.ToDictionary(x => x, x => CompilationPipeline.ParseResponseFile(
                Path.Combine(ProjectDirectory, x),
                ProjectDirectory,
                systemReferenceDirectories
            ));

            Dictionary<string, ResponseFileData> responseFilesWithErrors = responseFilesData.Where(x => x.Value.Errors.Any())
                .ToDictionary(x => x.Key, x => x.Value);

            if (responseFilesWithErrors.Any())
            {
                foreach (var error in responseFilesWithErrors)
                foreach (var valueError in error.Value.Errors)
                {
                    Debug.LogError($"{error.Key} Parse Error : {valueError}");
                }
            }

            return responseFilesData.Select(x => x.Value);
        }

        Dictionary<string, string> GenerateAllAssetProjectParts()
        {
            Dictionary<string, StringBuilder> stringBuilders = new Dictionary<string, StringBuilder>();

            foreach (string asset in m_AssemblyNameProvider.GetAllAssetPaths())
            {
                // Exclude files coming from packages except if they are internalized.
                if (!m_ShouldGenerateAll && IsInternalizedPackagePath(asset))
                {
                    continue;
                }

                if (IsSupportedFile(asset) && ScriptingLanguage.None == ScriptingLanguageFor(asset))
                {
                    // Find assembly the asset belongs to by adding script extension and using compilation pipeline.
                    var assemblyName = m_AssemblyNameProvider.GetAssemblyNameFromScriptPath(asset + ".cs");

                    if (string.IsNullOrEmpty(assemblyName))
                    {
                        continue;
                    }

                    assemblyName = FileUtility.FileNameWithoutExtension(assemblyName);

                    if (!stringBuilders.TryGetValue(assemblyName, out var projectBuilder))
                    {
                        projectBuilder = new StringBuilder();
                        stringBuilders[assemblyName] = projectBuilder;
                    }

                    projectBuilder.Append("     <None Include=\"").Append(EscapedRelativePathFor(asset)).Append("\" />").Append(k_WindowsNewline);
                }
            }

            var result = new Dictionary<string, string>();

            foreach (var entry in stringBuilders)
                result[entry.Key] = entry.Value.ToString();

            return result;
        }

        bool IsInternalizedPackagePath(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return false;
            }

            var packageInfo = m_AssemblyNameProvider.FindForAssetPath(file);
            if (packageInfo == null) {
                return false;
            }
            var packageSource = packageInfo.source;
            return packageSource != PackageSource.Embedded && packageSource != PackageSource.Local;
        }

        void SyncProject(
            Assembly island,
            Dictionary<string, string> allAssetsProjectParts,
            IEnumerable<ResponseFileData> responseFilesData,
            List<Assembly> allProjectIslands)
        {
            SyncProjectFileIfNotChanged(ProjectFile(island), ProjectText(island, allAssetsProjectParts, responseFilesData, allProjectIslands));
        }

        void SyncProjectFileIfNotChanged(string path, string newContents)
        {
            if (Path.GetExtension(path) == ".csproj")
            {
                newContents = OnGeneratedCSProject(path, newContents);
            }

            SyncFileIfNotChanged(path, newContents);
        }

        void SyncSolutionFileIfNotChanged(string path, string newContents)
        {
            newContents = OnGeneratedSlnSolution(path, newContents);

            SyncFileIfNotChanged(path, newContents);
        }

        static void OnGeneratedCSProjectFiles()
        {
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => SafeGetTypes(x))
            .Where(x => typeof(AssetPostprocessor).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
            var args = new object[0];
            foreach (var type in types)
            {
                var method = type.GetMethod("OnGeneratedCSProjectFiles", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (method == null)
                {
                    continue;
                }
                method.Invoke(null, args);
            }
        }

        static bool OnPreGeneratingCSProjectFiles()
        {
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => SafeGetTypes(x))
            .Where(x => typeof(AssetPostprocessor).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
            bool result = false;
            foreach (var type in types)
            {
                var args = new object[0];
                var method = type.GetMethod("OnPreGeneratingCSProjectFiles", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (method == null)
                {
                    continue;
                }
                var returnValue = method.Invoke(null, args);
                if (method.ReturnType == typeof(bool))
                {
                    result |= (bool)returnValue;
                }
            }
            return result;
        }

        static string OnGeneratedCSProject(string path, string content)
        {
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => SafeGetTypes(x))
            .Where(x => typeof(AssetPostprocessor).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
            foreach (var type in types)
            {
                var args = new [] { path, content };
                var method = type.GetMethod("OnGeneratedCSProject", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (method == null)
                {
                    continue;
                }
                var returnValue = method.Invoke(null, args);
                if (method.ReturnType == typeof(string))
                {
                    content = (string)returnValue;
                }
            }
            return content;
        }

        static string OnGeneratedSlnSolution(string path, string content)
        {
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => SafeGetTypes(x))
            .Where(x => typeof(AssetPostprocessor).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
            foreach (var type in types)
            {
                var args = new [] { path, content };
                var method = type.GetMethod("OnGeneratedSlnSolution", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (method == null)
                {
                    continue;
                }
                var returnValue = method.Invoke(null, args);
                if (method.ReturnType == typeof(string))
                {
                    content = (string)returnValue;
                }
            }
            return content;
        }

        void SyncFileIfNotChanged(string filename, string newContents)
        {
            if (File.Exists(filename) &&
                newContents == File.ReadAllText(filename))
            {
                return;
            }

            if (Settings.ShouldSync)
            {
                File.WriteAllText(filename, newContents, Encoding.UTF8);
            }
            else
            {
                var utf8 = Encoding.UTF8;
                byte[] utfBytes = utf8.GetBytes(newContents);
                Settings.SyncPath[filename] = utf8.GetString(utfBytes, 0, utfBytes.Length);  
            }
        }

        string ProjectText(Assembly assembly,
            Dictionary<string, string> allAssetsProjectParts,
            IEnumerable<ResponseFileData> responseFilesData,
            List<Assembly> allProjectIslands)
        {
            var projectBuilder = new StringBuilder(ProjectHeader(assembly, responseFilesData));
            var references = new List<string>();
            var projectReferences = new List<Match>();

            projectBuilder.Append(@"  <ItemGroup>").Append(k_WindowsNewline);
            foreach (string file in assembly.sourceFiles)
            {
                if (!ShouldFileBePartOfSolution(file))
                    continue;

                var extension = Path.GetExtension(file).ToLower();
                var fullFile = EscapedRelativePathFor(file);
                if (".dll" != extension)
                {
                    projectBuilder.Append("    <Compile Include=\"").Append(fullFile).Append("\" />").Append(k_WindowsNewline);
                }
                else
                {
                    references.Add(fullFile);
                }
            }
            projectBuilder.Append(@"  </ItemGroup>").Append(k_WindowsNewline);

            var assemblyName = FileUtility.FileNameWithoutExtension(assembly.outputPath);

            projectBuilder.Append(@"  <ItemGroup>").Append(k_WindowsNewline);
            // Append additional non-script files that should be included in project generation.
            if (allAssetsProjectParts.TryGetValue(assemblyName, out var additionalAssetsForProject))
                projectBuilder.Append(additionalAssetsForProject);

            var islandRefs = references.Union(assembly.allReferences);

            foreach (string reference in islandRefs)
            {
                if (reference.EndsWith("/UnityEditor.dll", StringComparison.Ordinal)
                    || reference.EndsWith("/UnityEngine.dll", StringComparison.Ordinal)
                    || reference.EndsWith("\\UnityEditor.dll", StringComparison.Ordinal)
                    || reference.EndsWith("\\UnityEngine.dll", StringComparison.Ordinal))
                    continue;

                var match = k_ScriptReferenceExpression.Match(reference);
                if (match.Success)
                {
                    // assume csharp language
                    // Add a reference to a project except if it's a reference to a script assembly
                    // that we are not generating a project for. This will be the case for assemblies
                    // coming from .assembly.json files in non-internalized packages.
                    var dllName = match.Groups["dllname"].Value;
                    if (allProjectIslands.Any(i => Path.GetFileName(i.outputPath) == dllName))
                    {
                        projectReferences.Add(match);
                        continue;
                    }
                }

                string fullReference = Path.IsPathRooted(reference) ? reference : Path.Combine(ProjectDirectory, reference);

                AppendReference(fullReference, projectBuilder);
            }

            var responseRefs = responseFilesData.SelectMany(x => x.FullPathReferences.Select(r => r));
            foreach (var reference in responseRefs)
            {
                AppendReference(reference, projectBuilder);
            }
            projectBuilder.Append(@"  </ItemGroup>").Append(k_WindowsNewline);

            if (0 < projectReferences.Count)
            {
                projectBuilder.Append(@"  <ItemGroup>").Append(k_WindowsNewline);
                foreach (Match reference in projectReferences)
                {
                    var referencedProject = reference.Groups["project"].Value;

                    projectBuilder.Append("    <ProjectReference Include=\"").Append(referencedProject).Append(GetProjectExtension()).Append("\">").Append(k_WindowsNewline);
                    projectBuilder.Append("      <Project>{").Append(ProjectGuid(Path.Combine("Temp", reference.Groups["project"].Value + ".dll"))).Append("}</Project>").Append(k_WindowsNewline);
                    projectBuilder.Append("      <Name>").Append(referencedProject).Append("</Name>").Append(k_WindowsNewline);
                    projectBuilder.Append("    </ProjectReference>").Append(k_WindowsNewline);
                }
                projectBuilder.Append(@"  </ItemGroup>").Append(k_WindowsNewline);
            }

            projectBuilder.Append(ProjectFooter());
            return projectBuilder.ToString();
        }

        static string XmlFilename(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            path = path.Replace(@"%", "%25");
            path = path.Replace(@";", "%3b");

            return XmlEscape(path);
        }

        static string XmlEscape(string s)
        {
            return SecurityElement.Escape(s);
        }

        void AppendReference(string fullReference, StringBuilder projectBuilder)
        {
            var escapedFullPath = EscapedRelativePathFor(fullReference);
            projectBuilder.Append("    <Reference Include=\"").Append(FileUtility.FileNameWithoutExtension(escapedFullPath)).Append("\">").Append(k_WindowsNewline);
            projectBuilder.Append("      <HintPath>").Append(escapedFullPath).Append("</HintPath>").Append(k_WindowsNewline);
            projectBuilder.Append("    </Reference>").Append(k_WindowsNewline);
        }

        public string ProjectFile(Assembly assembly)
        {
            return Path.Combine(ProjectDirectory, $"{FileUtility.FileNameWithoutExtension(assembly.outputPath)}.csproj");
        }

        private static readonly Regex InvalidCharactersRegexPattern = new Regex(@"\?|&|\*|""|<|>|\||#|%|\^|;" + (VisualStudioEditor.IsWindows ? "" : "|:"));
        public string SolutionFile()
        {
            return Path.Combine(FileUtility.Normalize(ProjectDirectory), $"{InvalidCharactersRegexPattern.Replace(m_ProjectName,"_")}.sln");
        }

        string ProjectHeader(
            Assembly island,
            IEnumerable<ResponseFileData> responseFilesData
        )
        {
            var toolsVersion = "4.0";
            var productVersion = "10.0.20506";
            const string baseDirectory = ".";

            var targetFrameworkVersion = "v4.7.1";
            var targetLanguageVersion = "latest";

            var projectType = ProjectTypeOf(island.outputPath);

            var arguments = new object[]
            {
                toolsVersion, productVersion, ProjectGuid(island.outputPath),
                XmlFilename(FileUtility.Normalize(InternalEditorUtility.GetEngineAssemblyPath())),
                XmlFilename(FileUtility.Normalize(InternalEditorUtility.GetEditorAssemblyPath())),
                string.Join(";", new[] { "DEBUG", "TRACE" }.Concat(EditorUserBuildSettings.activeScriptCompilationDefines).Concat(island.defines).Concat(responseFilesData.SelectMany(x => x.Defines)).Distinct().ToArray()),
                MSBuildNamespaceUri,
                FileUtility.FileNameWithoutExtension(island.outputPath),
                EditorSettings.projectGenerationRootNamespace,
                targetFrameworkVersion,
                targetLanguageVersion,
                baseDirectory,
                island.compilerOptions.AllowUnsafeCode | responseFilesData.Any(x => x.Unsafe),
                // flavoring
                projectType + ":" + (int)projectType,
                EditorUserBuildSettings.activeBuildTarget + ":" + (int)EditorUserBuildSettings.activeBuildTarget,
                Application.unityVersion,
            };

            try
            {
                return string.Format(GetProjectHeaderTemplate(), arguments);
            }
            catch (Exception)
            {
                throw new NotSupportedException("Failed creating c# project because the c# project header did not have the correct amount of arguments, which is " + arguments.Length);
            }
        }

        private enum ProjectType
        {
            GamePlugins = 3,
            Game = 1,
            EditorPlugins = 7,
            Editor = 5,
        }

        private static ProjectType ProjectTypeOf(string fileName)
        {
            var plugins = fileName.Contains("firstpass");
            var editor = fileName.Contains("Editor");

            if (plugins && editor)
                return ProjectType.EditorPlugins;
            if (plugins)
                return ProjectType.GamePlugins;
            if (editor)
                return ProjectType.Editor;

            return ProjectType.Game;
        }

        static string GetSolutionText()
        {
            return string.Join("\r\n",
            @"",
            @"Microsoft Visual Studio Solution File, Format Version {0}",
            @"# Visual Studio {1}",
            @"{2}",
            @"Global",
            @"    GlobalSection(SolutionConfigurationPlatforms) = preSolution",
            @"        Debug|Any CPU = Debug|Any CPU",
            @"        Release|Any CPU = Release|Any CPU",
            @"    EndGlobalSection",
            @"    GlobalSection(ProjectConfigurationPlatforms) = postSolution",
            @"{3}",
            @"    EndGlobalSection",
            @"{4}",
            @"EndGlobal",
            @"").Replace("    ", "\t");
        }

        static string GetProjectFooterTemplate()
        {
            return string.Join("\r\n",
            @"  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />",
            @"  <Target Name=""GenerateTargetFrameworkMonikerAttribute"" />",
            @"  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. ",
            @"       Other similar extension points exist, see Microsoft.Common.targets.",
            @"  <Target Name=""BeforeBuild"">",
            @"  </Target>",
            @"  <Target Name=""AfterBuild"">",
            @"  </Target>",
            @"  -->",
            @"</Project>",
            @"");
        }

        string GetProjectHeaderTemplate()
        {
            var header = new[]
            {
                @"<?xml version=""1.0"" encoding=""utf-8""?>",
                @"<Project ToolsVersion=""{0}"" DefaultTargets=""Build"" xmlns=""{6}"">",
                @"  <PropertyGroup>",
                @"    <LangVersion>{10}</LangVersion>",
                @"  </PropertyGroup>",
                @"  <PropertyGroup>",
                @"    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>",
                @"    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>",
                @"    <ProductVersion>{1}</ProductVersion>",
                @"    <SchemaVersion>2.0</SchemaVersion>",
                @"    <RootNamespace>{8}</RootNamespace>",
                @"    <ProjectGuid>{{{2}}}</ProjectGuid>",
                @"    <OutputType>Library</OutputType>",
                @"    <AppDesignerFolder>Properties</AppDesignerFolder>",
                @"    <AssemblyName>{7}</AssemblyName>",
                @"    <TargetFrameworkVersion>{9}</TargetFrameworkVersion>",
                @"    <FileAlignment>512</FileAlignment>",
                @"    <BaseDirectory>{11}</BaseDirectory>",
                @"  </PropertyGroup>",
                @"  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">",
                @"    <DebugSymbols>true</DebugSymbols>",
                @"    <DebugType>full</DebugType>",
                @"    <Optimize>false</Optimize>",
                @"    <OutputPath>Temp\bin\Debug\</OutputPath>",
                @"    <DefineConstants>{5}</DefineConstants>",
                @"    <ErrorReport>prompt</ErrorReport>",
                @"    <WarningLevel>4</WarningLevel>",
                @"    <NoWarn>0169</NoWarn>",
                @"    <AllowUnsafeBlocks>{12}</AllowUnsafeBlocks>",
                @"  </PropertyGroup>",
                @"  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">",
                @"    <DebugType>pdbonly</DebugType>",
                @"    <Optimize>true</Optimize>",
                @"    <OutputPath>Temp\bin\Release\</OutputPath>",
                @"    <ErrorReport>prompt</ErrorReport>",
                @"    <WarningLevel>4</WarningLevel>",
                @"    <NoWarn>0169</NoWarn>",
                @"    <AllowUnsafeBlocks>{12}</AllowUnsafeBlocks>",
                @"  </PropertyGroup>"
            };

            var forceExplicitReferences = new[]
            {
                @"  <PropertyGroup>",
                @"    <NoConfig>true</NoConfig>",
                @"    <NoStdLib>true</NoStdLib>",
                @"    <AddAdditionalExplicitAssemblyReferences>false</AddAdditionalExplicitAssemblyReferences>",
                @"    <ImplicitlyExpandNETStandardFacades>false</ImplicitlyExpandNETStandardFacades>",
                @"    <ImplicitlyExpandDesignTimeFacades>false</ImplicitlyExpandDesignTimeFacades>",
                @"  </PropertyGroup>"
            };

            var flavoring = new[]
            {
                @"  <PropertyGroup>",
                @"    <ProjectTypeGuids>{{E097FAD1-6243-4DAD-9C02-E9B9EFC3FFC1}};{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}</ProjectTypeGuids>",
                @"    <UnityProjectGenerator>Package</UnityProjectGenerator>",
                @"    <UnityProjectType>{13}</UnityProjectType>",
                @"    <UnityBuildTarget>{14}</UnityBuildTarget>",
                @"    <UnityVersion>{15}</UnityVersion>",
                @"  </PropertyGroup>"
            };

            var footer = new[]
            {
                @"  <ItemGroup>",
                @"    <Reference Include=""UnityEngine"">",
                @"      <HintPath>{3}</HintPath>",
                @"    </Reference>",
                @"    <Reference Include=""UnityEditor"">",
                @"      <HintPath>{4}</HintPath>",
                @"    </Reference>",
                @"  </ItemGroup>",
                @""
            };

            var lines = header
                .Concat(forceExplicitReferences)
                .Concat(flavoring)
                .ToList();

            // Only add analyzer block for compatible Visual Studio
            if (CodeEditor.CurrentEditor is VisualStudioEditor editor && editor.TryGetVisualStudioInstallationForPath(CodeEditor.CurrentEditorInstallation, out var installation))
            {
                if (installation.SupportsAnalyzers)
                {
                    var analyzers = installation.GetAnalyzers();
                    if (analyzers != null && analyzers.Length > 0)
                    {
                        lines.Add(@"  <ItemGroup>");
                        foreach (var analyzer in analyzers)
                            lines.Add(string.Format(@"    <Analyzer Include=""{0}"" />", EscapedRelativePathFor(analyzer)));
                        lines.Add(@"  </ItemGroup>");
                    }
                }
            }

            return string.Join("\r\n", lines
                .Concat(footer));
        }

        void SyncSolution(IEnumerable<Assembly> islands)
        {
            if (InvalidCharactersRegexPattern.IsMatch(ProjectDirectory))
                Debug.LogWarning("Project path contains special characters, which can be an issue when opening Visual Studio");

            var solutionFile = SolutionFile();
            var previousSolution = File.Exists(solutionFile) ? SolutionParser.ParseSolutionFile(solutionFile) : null;
            SyncSolutionFileIfNotChanged(solutionFile, SolutionText(islands, previousSolution));
        }

        string SolutionText(IEnumerable<Assembly> islands, Solution previousSolution = null)
        {
            const string fileversion = "12.00";
            const string vsversion = "15";

            var relevantIslands = RelevantIslandsForMode(islands);
            var generatedProjects = ToProjectEntries(relevantIslands);

            SolutionProperties[] properties = null;

            // First, add all projects generated by Unity to the solution
            var projects = new List<SolutionProjectEntry>();
            projects.AddRange(generatedProjects);

            if (previousSolution != null)
            {
                // Add all projects that were previously in the solution and that are not generated by Unity, nor generated in the project root directory
                var externalProjects = previousSolution.Projects
                    .Where(p => !FileUtility.IsFileInProjectDirectory(p.FileName))
                    .Where(p => generatedProjects.All(gp => gp.FileName != p.FileName));

                projects.AddRange(externalProjects);
                properties = previousSolution.Properties;
            }

            string propertiesText = GetPropertiesText(properties);
            string projectEntriesText = GetProjectEntriesText(projects);
            string projectConfigurationsText = string.Join(k_WindowsNewline, projects.Select(p => GetProjectActiveConfigurations(p.ProjectGuid)).ToArray());
            return string.Format(GetSolutionText(), fileversion, vsversion, projectEntriesText, projectConfigurationsText, propertiesText);
        }

        static IEnumerable<Assembly> RelevantIslandsForMode(IEnumerable<Assembly> islands)
        {
            IEnumerable<Assembly> relevantIslands = islands.Where(i => ScriptingLanguage.CSharp == ScriptingLanguageFor(i));
            return relevantIslands;
        }

        private string GetPropertiesText(SolutionProperties[] array)
        {
            if (array == null || array.Length == 0)
            {
                // HideSolution by default
                array = new SolutionProperties[] {
                    new SolutionProperties() {
                        Name = "SolutionProperties",
                        Type = "preSolution",
                        Entries = new List<KeyValuePair<string,string>>() { new KeyValuePair<string, string> ("HideSolutionNode", "FALSE") }
                    }
                };
            }
            var result = new StringBuilder();

            for (var i = 0; i<array.Length; i++)
            {
                if (i > 0)
                    result.Append(k_WindowsNewline);

                var properties = array[i];

                result.Append($"\tGlobalSection({properties.Name}) = {properties.Type}");
                result.Append(k_WindowsNewline);

                foreach (var entry in properties.Entries)
                {
                    result.Append($"\t\t{entry.Key} = {entry.Value}");
                    result.Append(k_WindowsNewline);
                }

                result.Append("\tEndGlobalSection");
            }

            return result.ToString();
        }

        /// <summary>
        /// Get a Project("{guid}") = "MyProject", "MyProject.unityproj", "{projectguid}"
        /// entry for each relevant language
        /// </summary>
        string GetProjectEntriesText(IEnumerable<SolutionProjectEntry> entries)
        {
            var projectEntries = entries.Select(entry => string.Format(
                m_SolutionProjectEntryTemplate,
                entry.ProjectFactoryGuid, entry.Name, entry.FileName, entry.ProjectGuid
            ));

            return string.Join(k_WindowsNewline, projectEntries.ToArray());
        }

        IEnumerable<SolutionProjectEntry> ToProjectEntries(IEnumerable<Assembly> islands)
        {
            foreach (var island in islands)
                yield return new SolutionProjectEntry()
                {
                    ProjectFactoryGuid = SolutionGuid(island),
                    Name = FileUtility.FileNameWithoutExtension(island.outputPath),
                    FileName = Path.GetFileName(ProjectFile(island)),
                    ProjectGuid = ProjectGuid(island.outputPath)
                };
        }

        /// <summary>
        /// Generate the active configuration string for a given project guid
        /// </summary>
        string GetProjectActiveConfigurations(string projectGuid)
        {
            return string.Format(
                m_SolutionProjectConfigurationTemplate,
                projectGuid);
        }

        string EscapedRelativePathFor(string file)
        {
            var projectDir = FileUtility.Normalize(ProjectDirectory);
            file = FileUtility.Normalize(file);
            var path = SkipPathPrefix(file, projectDir);

            var packageInfo = m_AssemblyNameProvider.FindForAssetPath(path.Replace('\\', '/'));
            if (packageInfo != null) {
                // We have to normalize the path, because the PackageManagerRemapper assumes
                // dir seperators will be os specific.
                var absolutePath = Path.GetFullPath(FileUtility.Normalize(path));
                path = SkipPathPrefix(absolutePath, projectDir);
            }

            return XmlFilename(path);
        }

        static string SkipPathPrefix(string path, string prefix)
        {
            if (path.StartsWith($"{prefix}{Path.DirectorySeparatorChar}") && (path.Length > prefix.Length))
                return path.Substring(prefix.Length + 1);
            return path;
        }

        string ProjectGuid(string assembly)
        {
            return SolutionGuidGenerator.GuidForProject(m_ProjectName + FileUtility.FileNameWithoutExtension(assembly));
        }

        string SolutionGuid(Assembly island)
        {
            return SolutionGuidGenerator.GuidForSolution(m_ProjectName, ScriptingLanguageFor(island));
        }

        static string ProjectFooter()
        {
            return GetProjectFooterTemplate();
        }

        static string GetProjectExtension()
        {
            return ".csproj";
        }
    }

    public static class SolutionGuidGenerator
    {
        public static string GuidForProject(string projectName)
        {
            return ComputeGuidHashFor(projectName + "salt");
        }

        public static string GuidForSolution(string projectName, ScriptingLanguage language)
        {
            if (language == ScriptingLanguage.CSharp)
            {
                // GUID for a C# class library: http://www.codeproject.com/Reference/720512/List-of-Visual-Studio-Project-Type-GUIDs
                return "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
            }

            return ComputeGuidHashFor(projectName);
        }

        static string ComputeGuidHashFor(string input)
        {
            var hash = MD5.Create().ComputeHash(Encoding.Default.GetBytes(input));
            return HashAsGuid(HashToString(hash));
        }

        static string HashAsGuid(string hash)
        {
            var guid = hash.Substring(0, 8) + "-" + hash.Substring(8, 4) + "-" + hash.Substring(12, 4) + "-" + hash.Substring(16, 4) + "-" + hash.Substring(20, 12);
            return guid.ToUpper();
        }

        static string HashToString(byte[] bs)
        {
            var sb = new StringBuilder();
            foreach (byte b in bs)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
