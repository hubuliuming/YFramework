using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MCPForUnity.Editor.Helpers;
using MCPForUnity.Editor.Tools;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YFramework.Editor.MCP
{
    /// <summary>
    /// YFramework custom MCP tool: query/set scene auto-save state.
    /// </summary>
    [McpForUnityTool(
        "yf_autosave_config",
        Description = "Query or configure YFramework scene auto-save settings. Use action='get' to read current state, or action='set' to update enabled/interval/showMessage.",
        Group = "core"
    )]
    public static class YfAutoSaveConfigTool
    {
        public class Parameters
        {
            [ToolParameter("Action: 'get' to read current config, 'set' to modify.")]
            public string action { get; set; } = "get";

            [ToolParameter("Enable auto-save. Only used when action='set'.")]
            public bool? enabled { get; set; }

            [ToolParameter("Save interval in seconds. Only used when action='set'.")]
            public int? interval_seconds { get; set; }

            [ToolParameter("Show save messages in console. Only used when action='set'.")]
            public bool? show_message { get; set; }
        }

        public static object HandleCommand(JObject @params)
        {
            string action = (@params?["action"]?.ToString() ?? "get").ToLowerInvariant();

            if (action == "set")
            {
                if (@params["enabled"] != null)
                    AutoSaveWindow.autoSaveScene = ParamCoercion.CoerceBool(@params["enabled"], AutoSaveWindow.autoSaveScene);

                if (@params["interval_seconds"] != null)
                    AutoSaveWindow.intervalTime = Mathf.Max(1, (int)(@params["interval_seconds"]));

                if (@params["show_message"] != null)
                    AutoSaveWindow.showMessage = ParamCoercion.CoerceBool(@params["show_message"], AutoSaveWindow.showMessage);
            }

            var currentScene = EditorSceneManager.GetActiveScene();
            var lastSave = XPAutoSave.lastSaveTime;

            return new SuccessResponse("Auto-save config retrieved.", new
            {
                auto_save_enabled = AutoSaveWindow.autoSaveScene,
                interval_seconds = AutoSaveWindow.intervalTime,
                show_message = AutoSaveWindow.showMessage,
                last_save_time = lastSave.ToString("yyyy-MM-dd HH:mm:ss"),
                seconds_since_last_save = (DateTime.Now - lastSave).TotalSeconds.ToString("F1"),
                current_scene_path = currentScene.path,
                current_scene_name = currentScene.name
            });
        }
    }

    /// <summary>
    /// YFramework custom MCP tool: get YFramework version and module information.
    /// </summary>
    [McpForUnityTool(
        "yf_get_framework_info",
        Description = "Get YFramework metadata: version, loaded assemblies, editor tool modules, and AutoBind rules summary.",
        Group = "core"
    )]
    public static class YfGetFrameworkInfoTool
    {
        public class Parameters
        {
            [ToolParameter("Info category: 'all', 'version', 'modules', 'autobind'. Defaults to 'all'.")]
            public string category { get; set; } = "all";
        }

        public static object HandleCommand(JObject @params)
        {
            string category = (@params?["category"]?.ToString() ?? "all").ToLowerInvariant();

            bool includeVersion = category == "all" || category == "version";
            bool includeModules = category == "all" || category == "modules";
            bool includeAutoBind = category == "all" || category == "autobind";

            var result = new JObject();
            result["framework_name"] = "YFramework";
            result["unity_version"] = Application.unityVersion;
            result["product_name"] = Application.productName;

            if (includeVersion)
            {
                var yfAssembly = typeof(YMonoBehaviour).Assembly;
                result["yf_assembly"] = yfAssembly.GetName().Name;
                result["yf_assembly_version"] = yfAssembly.GetName().Version?.ToString() ?? "0.0.0.0";
            }

            if (includeModules)
            {
                var editorModules = new JArray();
                editorModules.Add("AutoBind (代码自动生成)");
                editorModules.Add("AutoSaveScene (场景自动保存)");
                editorModules.Add("ImportResSeting (资源导入规则)");
                editorModules.Add("UI/MenuOptions (UI快捷创建)");
                editorModules.Add("UI/CricleImageEditor (CircleImage检视面板)");
                editorModules.Add("MCP (UnityMCP自定义工具)");
                result["editor_modules"] = editorModules;

                var runtimeModules = new JArray();
                runtimeModules.Add("Framework (YMonoBehaviour / MonoGlobal)");
                runtimeModules.Add("Kit/ActionKit");
                runtimeModules.Add("Kit/Singleton");
                runtimeModules.Add("Kit/TimerManager");
                runtimeModules.Add("Kit/Net (TCP/UDP)");
                runtimeModules.Add("UI/GameUIKit");
                runtimeModules.Add("UI/UIKitRuntime");
                runtimeModules.Add("Math (Fixed64定点数)");
                runtimeModules.Add("Collections (可序列化字典)");
                runtimeModules.Add("Extension (各类扩展方法)");
                result["runtime_modules"] = runtimeModules;
            }

            if (includeAutoBind)
            {
                var autoBind = new JObject();
                autoBind["description"] = "根据节点命名前缀自动生成字段绑定代码";
                autoBind["supported_prefixes"] = new JArray(
                    "Go_", "Btn_", "Txt_", "Img_", "RawImg_",
                    "Slider_", "Toggle_", "Input_", "Scroll_",
                    "Tran_", "Rect_", "Obj_"
                );
                autoBind["entry_menu"] = "CONTEXT/MonoBehaviour/AutoBind";
                autoBind["generated_file_suffix"] = ".Designer.cs";
                result["autobind"] = autoBind;
            }

            return new SuccessResponse("YFramework info retrieved.", result);
        }
    }

    /// <summary>
    /// YFramework custom MCP tool: trigger AutoBind code generation for a target GameObject's MonoBehaviour.
    /// </summary>
    [McpForUnityTool(
        "yf_autobind_generate",
        Description = "Trigger YFramework AutoBind code generation. Finds the target GameObject's MonoBehaviour and invokes AutoBind to generate Designer.cs binding code.",
        Group = "core"
    )]
    public static class YfAutoBindGenerateTool
    {
        public class Parameters
        {
            [ToolParameter("The target GameObject name, path, or instance ID. Uses current Selection if omitted.")]
            public string target { get; set; }

            [ToolParameter("MonoBehaviour type name to bind. Auto-detects if omitted (uses first YFramework MonoBehaviour on the target).")]
            public string mono_type { get; set; }
        }

        public static object HandleCommand(JObject @params)
        {
            if (EditorApplication.isPlaying)
                return new ErrorResponse("autobind_blocked: AutoBind cannot run in Play mode.");

            if (EditorApplication.isCompiling)
                return new ErrorResponse("autobind_blocked: Unity is compiling scripts. Wait and retry.");

            GameObject targetGo = ResolveGameObject(@params?["target"]?.ToString());
            if (targetGo == null)
                return new ErrorResponse("GameObject not found. Provide a valid name, path, or select a GameObject in the Editor.");

            MonoBehaviour targetMono = null;

            string monoTypeFilter = @params?["mono_type"]?.ToString();
            var allMonos = targetGo.GetComponents<MonoBehaviour>();
            if (allMonos.Length == 0)
                return new ErrorResponse($"No MonoBehaviour found on GameObject '{targetGo.name}'.");

            if (!string.IsNullOrEmpty(monoTypeFilter))
            {
                targetMono = allMonos.FirstOrDefault(m =>
                    m.GetType().Name.IndexOf(monoTypeFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    m.GetType().FullName.IndexOf(monoTypeFilter, StringComparison.OrdinalIgnoreCase) >= 0);
                if (targetMono == null)
                    return new ErrorResponse($"No MonoBehaviour matching '{monoTypeFilter}' found on '{targetGo.name}'.");
            }
            else
            {
                targetMono = allMonos.FirstOrDefault(m =>
                    !string.IsNullOrEmpty(m.GetType().Namespace) &&
                    m.GetType().Namespace.StartsWith("YFramework"));
                if (targetMono == null)
                    targetMono = allMonos[0];
                if (targetMono == null)
                    return new ErrorResponse($"No suitable MonoBehaviour found on '{targetGo.name}'. Specify mono_type.");
            }

            try
            {
                var editorType = typeof(AutoBindEditor);
                var method = editorType.GetMethod("AutoBind",
                    BindingFlags.Static | BindingFlags.NonPublic,
                    null,
                    new[] { typeof(MenuCommand) },
                    null);

                if (method == null)
                    return new ErrorResponse("AutoBindEditor.AutoBind method not found via reflection.");

                var menuCommand = new MenuCommand(targetMono);
                method.Invoke(null, new object[] { menuCommand });

                AssetDatabase.Refresh();

                return new SuccessResponse(
                    $"AutoBind triggered for {targetMono.GetType().Name} on '{targetGo.name}'. " +
                    "After Unity compiles, check for .Designer.cs generation.",
                    new
                    {
                        game_object = targetGo.name,
                        mono_type = targetMono.GetType().FullName,
                        hint = "If compilation is needed, poll editor_state until ready_for_tools is true."
                    });
            }
            catch (TargetInvocationException ex)
            {
                return new ErrorResponse($"AutoBind failed: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return new ErrorResponse($"AutoBind error: {ex.Message}");
            }
        }

        private static GameObject ResolveGameObject(string target)
        {
            if (!string.IsNullOrEmpty(target))
            {
                if (int.TryParse(target, out int instanceId))
                {
                    var go = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
                    if (go != null) return go;
                }

                var found = GameObject.Find(target);
                if (found != null) return found;

                var scene = EditorSceneManager.GetActiveScene();
                foreach (var rootGo in scene.GetRootGameObjects())
                {
                    var t = rootGo.transform.Find(target);
                    if (t != null) return t.gameObject;
                }
            }

            return Selection.activeGameObject;
        }
    }
}
