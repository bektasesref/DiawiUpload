#if UNITY_ANDROID
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using System.Collections;
using static UploaderWindow;
using UnityEngine.Networking;
using UnityEditor.Build.Reporting;
using Unity.EditorCoroutines.Editor;

#pragma warning disable CS0618
class UploaderPostBuild : IPostprocessBuildWithReport
#pragma warning restore CS0618
{
    public int callbackOrder { get { return 0; } }
    public void OnPostprocessBuild(BuildReport report)
    {
        if (File.Exists(Application.persistentDataPath + "/uploader.json"))
        {
            UploaderData data = JsonUtility.FromJson<UploaderData>(File.ReadAllText(Application.persistentDataPath + "/uploader.json"));
            if (data == null)
                data = new UploaderData();
            if (data.automaticallyUploadToDiawi)
            {
                foreach (var v in report.GetFiles())
                    if (v.path.EndsWith(".apk"))
                    {
                        EditorCoroutineUtility.StartCoroutineOwnerless(UploadFile(v.path));
                        EditorPrefs.SetBool("postProcessTriggered", true);
                    }
            }
        }
    }

    public IEnumerator UploadFile(string apkPath)
    {
        UploaderData data;

        if (File.Exists(Application.persistentDataPath + "/uploader.json"))
            data = JsonUtility.FromJson<UploaderData>(File.ReadAllText(Application.persistentDataPath + "/uploader.json"));
        else
            data = new UploaderData();

        Debug.Log($"APK Upload Started to {data.invokeURLPath}");

        using (UnityWebRequest webRequest = new UnityWebRequest(data.invokeURLPath + "?notifyOnSlack=" + data.notifyOnSlack.ToString().ToLower() + "&diawiToken=" + data.diawiToken + "&slackWebHook=" + data.slackWebHook, "GET"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(File.ReadAllBytes(apkPath));
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/vnd.android.package-archive");
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
                Debug.Log(webRequest.downloadHandler.text);
        }
    }
}
#endif