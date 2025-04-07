using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace LD57.Web {
   public static class WebRequests {
#if UNITY_EDITOR
      private static string uriRoot => "http://localhost/nathanistace/gamejams/ludumdare57";
#else
      private static string uriRoot => "https://nathanistace.be/gamejams/ludumdare57";
#endif

      public static Uri GetUri(string action) => new Uri($"{uriRoot}/{action}");

      private static IEnumerator Request<TResult>(string verb, Uri url, object data, UnityAction<TResult> callback, UnityAction errorCallback) {
         using (var webRequest = new UnityWebRequest(url, verb)) {
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();

            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError) {
               Debug.Log($"{url} {webRequest.error}");
               errorCallback?.Invoke();
               yield break;
            }
            try {
               var result = JsonUtility.FromJson<TResult>(webRequest.downloadHandler.text);
               callback?.Invoke(result);
            }
            catch (Exception e) {
               Debug.LogException(e);
               errorCallback?.Invoke();
            }
         }
      }

      private static IEnumerator Request(string verb, Uri url, object data, UnityAction callback, UnityAction errorCallback) {
         using (var webRequest = new UnityWebRequest(url, verb)) {
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();

            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError) {
               Debug.Log(webRequest.error);
               errorCallback?.Invoke();
               yield break;
            }
            try {
               callback?.Invoke();
            }
            catch (Exception e) {
               Debug.LogError(e);
               errorCallback?.Invoke();
            }
         }
      }

      public static IEnumerator Get<TResult>(Uri url, object data, UnityAction<TResult> callback, UnityAction errorCallback) => Request("GET", url, data, callback, errorCallback);
      public static IEnumerator Get(Uri url, object data, UnityAction callback, UnityAction errorCallback) => Request("GET", url, data, callback, errorCallback);
      public static IEnumerator Post<TResult>(Uri url, object data, UnityAction<TResult> callback, UnityAction errorCallback) => Request("POST", url, data, callback, errorCallback);
      public static IEnumerator Post(Uri url, object data, UnityAction callback, UnityAction errorCallback) => Request("POST", url, data, callback, errorCallback);
      public static IEnumerator Put<TResult>(Uri url, object data, UnityAction<TResult> callback, UnityAction errorCallback) => Request("PUT", url, data, callback, errorCallback);
      public static IEnumerator Put(Uri url, object data, UnityAction callback, UnityAction errorCallback) => Request("PUT", url, data, callback, errorCallback);
      public static IEnumerator Delete<TResult>(Uri url, object data, UnityAction<TResult> callback, UnityAction errorCallback) => Request("DELETE", url, data, callback, errorCallback);
      public static IEnumerator Delete(Uri url, object data, UnityAction callback, UnityAction errorCallback) => Request("DELETE", url, data, callback, errorCallback);
   }
}