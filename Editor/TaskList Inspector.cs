using UnityEngine;
using UnityEditor;
using System.IO;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(TaskList))]
    [CanEditMultipleObjects]
    public class TaskListInspector : ActormachineBaseInspector
    {
        private string tasklistContent = "";

        private void OnEnable()
        {
            string scriptGuid = AssetDatabase.FindAssets("TaskList t:MonoScript")[0];
            string scriptPath = AssetDatabase.GUIDToAssetPath(scriptGuid);
            string editorFolderPath = Path.GetDirectoryName(scriptPath);
            string packageFolderPath = Directory.GetParent(editorFolderPath).ToString();
            string tasklistPath = Path.Combine(packageFolderPath, "TASKLIST.md");

            if (File.Exists(tasklistPath))
            {
                tasklistContent = File.ReadAllText(tasklistPath);
            }
            else
            {
                EditorGUILayout.HelpBox("TASKLIST file not found at: " + tasklistPath, MessageType.Error);
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            tasklistContent = EditorGUILayout.TextArea(tasklistContent, GUILayout.ExpandHeight(true));

            if (EditorGUI.EndChangeCheck())
            {
                string scriptGuid = AssetDatabase.FindAssets("TaskList t:MonoScript")[0];
                string scriptPath = AssetDatabase.GUIDToAssetPath(scriptGuid);
                string editorFolderPath = Path.GetDirectoryName(scriptPath);
                string packageFolderPath = Directory.GetParent(editorFolderPath).ToString();
                string tasklistPath = Path.Combine(packageFolderPath, "TASKLIST.md");

                File.WriteAllText(tasklistPath, tasklistContent);
            }
        }
    }
}