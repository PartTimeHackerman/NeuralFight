using System.Threading.Tasks;
using Nakama;
using UnityEngine;

public class NakamaClient : MonoBehaviour
{
    public static string Key = "defaultkey";
    public static string IP = "127.0.0.1";
    public static int Port = 7350;
    public static bool SSL = false;

    public Client Client;
    public ISession Session;
    public IApiAccount Account;
    public IApiUser User;
    
    
    private async Task Awake()
    {
        Client = new Client(Key, IP, Port, SSL);
    }

    private async Task Update()
    {
        if (Client == null || Session == null || Session.IsExpired)
        {
            await RestoreSession();
            await GetAccount();
        }
    }

    private async Task RestoreSession()
    {
        var deviceid = PlayerPrefs.GetString("nakama.deviceid");
        if (string.IsNullOrEmpty(deviceid)) {
            deviceid = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString("nakama.deviceid", deviceid);
        }
        Session = await Client.AuthenticateDeviceAsync(deviceid);
        PlayerPrefs.SetString("nakama.session", Session.AuthToken);
        
        Debug.LogFormat("Session: '{0}'", Session.AuthToken);
    }
    
    private async Task GetAccount()
    {
        Account = await Client.GetAccountAsync(Session);
        User = Account.User;
        
        Debug.LogFormat("Name: {0}, ID: {1}", User.Username, User.Id);
    }

    private async Task SetUsername(string name)
    {
        await Client.UpdateAccountAsync(Session, name);
        GetAccount();
    }
}