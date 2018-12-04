using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


namespace Freee {
  public class Client {


    private string appID;
    private string appSecret;
    private string redirectUrl;
    public string authorizationCode;
    
    private string accessToken;
    public string refreshToken;

    public delegate void Callback(bool success, string response);

    

    #region constructor
    public Client(string appID, string appSecret, string redirectUrl) {
      this.appID = appID;
      this.appSecret = appSecret;
      this.redirectUrl = redirectUrl;
    }
    public Client(string AccessToken) {
        this.accessToken = AccessToken;
    }
    public Client(){}
    #endregion

    #region Public Methods
    public IEnumerator GenerateAccessToken(Callback callback) {
      string endPoint = "https://accounts.secure.freee.co.jp/public_api/token";

      WWWForm form = new WWWForm();
      
      form.AddField("client_id", appID);
      form.AddField("client_secret", appSecret);

      if (string.IsNullOrEmpty(refreshToken)) {
        form.AddField("grant_type", "authorization_code");
        form.AddField("code", authorizationCode);
        form.AddField("redirect_uri", redirectUrl);
      } else {
        form.AddField("grant_type", "refresh_token");
        form.AddField("refresh_token", refreshToken);
      }

      UnityWebRequest req = UnityWebRequest.Post(endPoint, form);
      req.SetRequestHeader("Content-type", "application/x-www-form-urlencoded");
      yield return req.SendWebRequest();

      if (req.isNetworkError || req.isHttpError) {
        callback(false, req.error);
      } else {
        AccessToken token = JsonUtility.FromJson<AccessToken>(req.downloadHandler.text);
        accessToken = token.access_token;
        refreshToken = token.refresh_token;
        PlayerPrefs.SetString("refresh_token", refreshToken);

        callback(true, accessToken);
      }
    }


    public IEnumerator Get(string url, Dictionary<string, string> p, Callback callback) {
      StringBuilder s = new StringBuilder();
      foreach(KeyValuePair<string, string> kv in p) {
        s.Append(Uri.EscapeDataString(kv.Key) + "=" + Uri.EscapeDataString(kv.Value) + "&");
      }
      if (s.Length > 0) s.Length -= 1;
      
      UnityWebRequest req = UnityWebRequest.Get(url + "?" + s.ToString());
      req.SetRequestHeader("Authorization", "Bearer " + this.accessToken);
      req.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

      yield return SendRequest(req, callback);
    }

    public IEnumerator Post(string url, string requestString, Callback callback) {

      byte[] bodyRaw = Encoding.UTF8.GetBytes(requestString);

      UnityWebRequest req = new UnityWebRequest(url, "POST");
      req.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
      req.downloadHandler = new DownloadHandlerBuffer();
      req.SetRequestHeader("Authorization", "Bearer " + this.accessToken);
      req.SetRequestHeader("Content-Type", "application/json");

      yield return SendRequest(req, callback);           
    }
    #endregion


    #region Helper
    private IEnumerator SendRequest(UnityWebRequest req, Callback callback) {
      yield return req.SendWebRequest();

      if (req.isNetworkError || req.isHttpError) {
        callback(false, req.downloadHandler.text);
      } else {
        callback(true, Helper.ArrayToObject(req.downloadHandler.text));
      }
    }

    #endregion
  }
}
