using System.IO;
using UnityEditor;
using UnityEngine;

public class UploaderWindow : EditorWindow
{
    #region Parameters
    public UploaderWindow editorScript;

    [System.Serializable]
    public class UploaderData
    {
        public bool automaticallyUploadToDiawi = false;
        public bool notifyOnSlack = false;
        public string invokeURLPath = "https://{replace}/Upload.php";
        public string diawiToken = "{replaceWithDiawiToken}";
        public string slackWebHook = "https://hooks.slack.com/workflows/{replaceWithSlackHookURL}";
    }
    public UploaderData data;
    #endregion
    #region Defaults

    [MenuItem("BE/Uploader", false, 0)]
    public static void ShowWindow()
    {
        EditorWindow editorWindow = ((UploaderWindow)GetWindow(typeof(UploaderWindow), true, "Uploader"));
        UploaderWindow window = (editorWindow as UploaderWindow);
        (editorWindow as UploaderWindow).editorScript = window;
        var position = editorWindow.position;
#if UNITY_EDITOR_WIN
        position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
        editorWindow.position = position;
#endif
        editorWindow.Show();
        if (File.Exists(Application.persistentDataPath + "/uploader.json"))
            window.data = JsonUtility.FromJson<UploaderData>(File.ReadAllText(Application.persistentDataPath + "/uploader.json"));
        else
            window.data = new UploaderData();
    }

    void OnGUI()
    {
        minSize = new Vector2(600, 150);
        maxSize = new Vector2(850, 250);

        EditorGUILayout.Space(10);

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("• Automatically Upload Builds to Diawi: ", GUILayout.ExpandWidth(true), GUILayout.MaxWidth(250), GUILayout.MinWidth(200));
        EditorGUILayout.LabelField("—————————————————————————————————————————————————————————————————————", GUILayout.MinWidth(50));
        EditorGUILayout.LabelField(">", GUILayout.MaxWidth(20));
        data.automaticallyUploadToDiawi = GUILayout.Toggle(data.automaticallyUploadToDiawi, "", GUILayout.Width(25));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("• Notify upload links on Slack: ", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(250), GUILayout.MinWidth(200));
        EditorGUILayout.LabelField("—————————————————————————————————————————————————————————————————————", GUILayout.MinWidth(50));
        EditorGUILayout.LabelField(">", GUILayout.MaxWidth(20));
        data.notifyOnSlack = GUILayout.Toggle(data.notifyOnSlack, "", GUILayout.Width(25));
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("• URL to Trigger: ", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(250), GUILayout.MinWidth(200));
        EditorGUILayout.LabelField(">", GUILayout.MaxWidth(20));
        data.invokeURLPath = EditorGUILayout.TextField(data.invokeURLPath, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();



        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("• Diawi Token: ", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(250), GUILayout.MinWidth(200));
        EditorGUILayout.LabelField(">", GUILayout.MaxWidth(20));
        data.diawiToken = EditorGUILayout.TextField(data.diawiToken, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("• Slack Webhook URL: ", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(250), GUILayout.MinWidth(200));
        EditorGUILayout.LabelField(">", GUILayout.MaxWidth(20));
        data.slackWebHook = EditorGUILayout.TextField(data.slackWebHook, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck())
            File.WriteAllText(Application.persistentDataPath + "/uploader.json", JsonUtility.ToJson(data));

        EditorGUILayout.Space(10);
    }
    #endregion
}