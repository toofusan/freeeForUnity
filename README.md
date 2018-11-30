
会計freee、人事労務freee APIのためのUnity C#用ラッパーライブラリです。

https://developer.freee.co.jp/

自分用の必要最低限の実装しかしていないので足りてない部分が多そうです。

## 環境
Unity2018.2.8f1

## アクセストークンの取得

Authorization codeは手動で取得して、それを手打ちしています。

```c#

[SerializeField] private string appID;
[SerializeField] private string secret;
[SerializeField] private string redirectURL;
[SerializeField] private string authorization_code;

private void Start () {
    string refresh_token = PlayerPrefs.GetString("refresh_token", "");

    client = new Freee.Client(appID, secret, redirectURL);
    if (string.IsNullOrEmpty(refresh_token)) {
        Debug.Log("authorization_code");
        client.authorizationCode = authorization_code;
    } else {
        Debug.Log("refresh_token");
        client.refreshToken = refresh_token;
    }

    StartCoroutine(client.GenerateAccessToken(OnGenerateAccessToken));
}

```


## 各種情報の取得例

### 事業所の取得

```c#
void GetCompanies() {
    StartCoroutine(client.Get("https://api.freee.co.jp/api/1/companies", new Dictionary<string, string>(), OnGetCompanies));
}

void OnGetCompanies(bool success, string response) {
        if (!success) return;

        Company[] companies = JsonUtility.FromJson<Companies>(response).companies;
    }
```

### ログインユーザー情報の取得

```c#
void GetLoginUser() {
    string employee_endpoint = "https://api.freee.co.jp/hr/api/v1/users/me";
    StartCoroutine(client.Get(employee_endpoint, new Dictionary<string, string>(), OnGetLoginUser));
}

private void OnGetLoginUser(bool success, string response) {
    if (!success) return;

    LoginUser companies = JsonUtility.FromJson<LoginUser>(response);
}

```

### 勤怠打刻情報の取得

```c#
private void GetTimeClocks() {
    string endpoint = "https://api.freee.co.jp/hr/api/v1/employees/" + employee_id + "/time_clocks";
    Dictionary<string, string> parameter = new Dictionary<string, string> {
        {"company_id", company_id.ToString()},
        {"from_date", DateTime.Now.ToString("yyyy-MM-dd")},
        {"to_date", DateTime.Now.ToString("yyyy-MM-dd")}
    };
    StartCoroutine(client.Get(endpoint, parameter, OnGetTimeClocks));
}

private void OnGetTimeClocks(bool success, string response) {
    if (!success) return;

    TimeClock[] timeClocks = JsonUtility.FromJson<TimeClocksResponse>(response).items;
}

```


### 勤怠打刻

```c#
public void PostTimeClocks(string type) {
    string endpoint = "https://api.freee.co.jp/hr/api/v1/employees/" + employee_id + "/time_clocks";
    string p = TimeClockRequestJson(type, DateTime.Now);
    StartCoroutine(client.Post(endpoint, p, OnPostTimeClocks));
}

private string TimeClockRequestJson(string type, DateTime dt)
{
    TimeClockRequest r = new TimeClockRequest();
    r.company_id = company_id;
    r.type = type;
    r.base_date = dt.ToString("yyyy-MM-dd");
    return JsonUtility.ToJson(r);
}

void OnPostTimeClocks(bool success, string response) {
    if (!success) return;

    TimeClock tc = JsonUtility.FromJson<PostTimeClocksResponse>(response).employee_time_clock;
}

```

## ライセンス
MIT