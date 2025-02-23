using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace NISGAB
{
    public static class InputActionGenerator
    {
        private const string ContextMenuName = "Assets/Generate Input Action Binding";
        private const string OutputPath = "Assets/Scripts/Generated/";
        private const string OutputNamespace = "NISGAB.Generated";
        
        [MenuItem(ContextMenuName)]
        public static void GenerateInputActionCode()
        {
            var selectedObject = Selection.activeObject as InputActionAsset;
            if (selectedObject == null)
            {
                Debug.LogError("Must select an InputActionAsset to generate input action code");
                return;
            }
            
            GenerateCodeFromAsset(selectedObject);
        }

        [MenuItem(ContextMenuName, true)]
        private static bool ValidateGenerateInputActionCode()
        {
            return Selection.activeObject is InputActionAsset;
        }

        private static void GenerateCodeFromAsset(InputActionAsset selectedObject)
        {
            ReadOnlyArray<InputActionMap> inputActionMaps = selectedObject.actionMaps;
            foreach (InputActionMap inputActionMap in inputActionMaps)
            {
                GenerateInputActionClass(inputActionMap);
            }
            GenerateInputEventClass(inputActionMaps);
        }

        private static void GenerateInputEventClass(ReadOnlyArray<InputActionMap> inputActionMaps)
        {
            StringBuilder sb = new();
            
            sb.AppendLine("namespace " + OutputNamespace);
            sb.AppendLine("{");
            sb.AppendLine("\tpublic class InputEvent : LazySingleton<InputEvent>");
            sb.AppendLine("\t{");
            foreach (InputActionMap inputActionMap in inputActionMaps)
            {
                string mapName = inputActionMap.name;
                sb.AppendLine("\t\tpublic static " + mapName + "InputActions " + mapName + " { get; private set; } = new();");
            }
            sb.AppendLine("");
            sb.AppendLine("\t\tprivate InputSystemActions _inputSystemActions;");
            sb.AppendLine("");
            sb.AppendLine("\t\tprotected override void Awake()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tbase.Awake();");
            sb.AppendLine("\t\t\tInitialize();");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");
            sb.AppendLine("\t\tprivate void OnDestroy()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tUnInitialize();");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");
            foreach (InputActionMap inputActionMap in inputActionMaps)
            {
                string mapName = inputActionMap.name;
                sb.AppendLine("\t\tpublic void Enable" + mapName + "Input()");
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t    if (_inputSystemActions." + mapName + ".enabled) { return; }");
                sb.AppendLine("\t\t    _inputSystemActions." + mapName + ".Enable();");
                sb.AppendLine("\t\t}");
                sb.AppendLine("");        
                sb.AppendLine("\t\tpublic void Disable" + mapName + "Input()");
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\tif (!_inputSystemActions." + mapName + ".enabled) { return; }");
                sb.AppendLine("\t\t\t_inputSystemActions." + mapName + ".Disable();");
                sb.AppendLine("\t\t}");
                sb.AppendLine("");
            }
            sb.AppendLine("");
            sb.AppendLine("\t\tprivate void Initialize()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\t_inputSystemActions = new InputSystemActions();");
            foreach (InputActionMap inputActionMap in inputActionMaps)
            {
                string mapName = inputActionMap.name;
                sb.AppendLine("\t\t\t" + mapName + ".Bind(_inputSystemActions." + mapName + ");");
            }
            sb.AppendLine("\t\t\t_inputSystemActions.Player.Enable();");
            sb.AppendLine("\t\t\t_inputSystemActions.UI.Enable();");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");
            sb.AppendLine("\t\tprivate void UnInitialize()");
            sb.AppendLine("\t\t{");
            foreach (InputActionMap inputActionMap in inputActionMaps)
            {
                string mapName = inputActionMap.name;
                sb.AppendLine("\t\t\t" + mapName + ".UnBind(_inputSystemActions." + mapName + ");");
            }
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            
            string filePath = Path.Combine(OutputPath, "InputEvent.cs");
            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
            File.WriteAllText(filePath, sb.ToString());
            AssetDatabase.Refresh();
        }

        private static void GenerateInputActionClass(InputActionMap inputActionMap)
        {
            string mapName = inputActionMap.name;
            string classString = GenerateClassString(inputActionMap);
            string filePath = Path.Combine(OutputPath, mapName + "Inputactions.cs");
            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
            File.WriteAllText(filePath, classString);
            AssetDatabase.Refresh();
        }

        private static string GenerateClassString(InputActionMap inputActionMap)
        {
            string mapName = inputActionMap.name;
            ReadOnlyArray<InputAction> actions = inputActionMap.actions;
            StringBuilder sb = new();
                sb.AppendLine("using System;");
                sb.AppendLine("using UnityEngine.InputSystem;\n");
                sb.AppendLine("namespace " + OutputNamespace);
                sb.AppendLine("{");
                sb.AppendLine("\t/// <summary>");
                sb.AppendLine("\t/// Simple event container for " + mapName + " actions");
                sb.AppendLine("\t/// </summary>");
                sb.AppendLine("\tpublic class " + mapName + "InputActions");
                sb.AppendLine("\t{");
                foreach (InputAction action in actions)
                {
                    sb.AppendLine("\t\tpublic event Action<InputAction.CallbackContext> " + action.name + ";");
                }
                sb.AppendLine("");
                sb.AppendLine("\t\tpublic void Bind(InputSystemActions." + mapName + "Actions actions)");
                sb.AppendLine("\t\t{");
                foreach (string actionName in actions.Select(action => action.name))
                {
                    sb.AppendLine("\t\t\tactions." + actionName + ".performed += On" + actionName + ";");
                }
                sb.AppendLine("\t\t}");
                sb.AppendLine("");
                sb.AppendLine("\t\tpublic void UnBind(InputSystemActions." + mapName + "Actions actions)");
                sb.AppendLine("\t\t{");
                foreach (InputAction action in actions)
                {
                    string actionName = action.name;
                    sb.AppendLine("\t\t\tactions." + actionName + ".performed -= On" + actionName + ";");
                }
                sb.AppendLine("\t\t}");
                foreach (InputAction action in actions)
                {
                    string actionName = action.name;
                    sb.AppendLine("");
                    sb.AppendLine("\t\tprivate void On" + actionName + "(InputAction.CallbackContext context)");
                    sb.AppendLine("\t\t{");
                    sb.AppendLine("\t\t\t" + actionName + "?.Invoke(context);");
                    sb.AppendLine("\t\t}");
                }
                sb.AppendLine("\t}");
                sb.AppendLine("}");
                            
                return sb.ToString();
        }
    }
}
