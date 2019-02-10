using System.Collections.Generic;
using DG.Tweening;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using Nakama.TinyJson;
using UnityEngine;
using UnityEngine.UI;

public class Auth : MonoBehaviour
{

    public FighterPartsCollection FighterPartsCollection;
    public WeaponsCollection WeaponsCollection;
    public FightersCollection FightersCollection;
    
    public Transform RegisterTransform;
    public Text RegisterInput;
    public Button RegisterSetUserName;
    public MainMenu MainMenu;
    public static string UserName = "";
    public static string UserId = "";
    public AccountDetailsResponse AccountDetailsResponse;
    
    
    public static event Authorize OnAuth;
    public delegate void Authorize(string userName, string userID);
    
    private void Start()
    {
        RegisterSetUserName.onClick.AddListener(RegisterNewUser);
        //PlayerPrefs.DeleteKey("UserName");
        if (PlayerPrefs.HasKey("UserName"))
        {
            Waiter.Get().WaitForSecondsC(1, () => { }, DeviceAuthentication);
               
            //DeviceAuthentication();
        }
        else
        {
            ShowRegisterPrompt();
        }
    }

    public void DeviceAuthentication()
    {
        
        UserName = PlayerPrefs.GetString("UserName");
        new GameSparks.Api.Requests.DeviceAuthenticationRequest().SetDisplayName(UserName).Send((response) => {
            if (!response.HasErrors) {
                Debug.Log("Device Authenticated...");
                UserId = response.UserId;
                Storage.PlayerID = UserId;
                
                OnAuth?.Invoke(response.DisplayName, response.UserId);
                new GameSparks.Api.Requests.AccountDetailsRequest().Send((accountDetailsResponse) => {
                    if (!accountDetailsResponse.HasErrors)
                    {
                        AccountDetailsResponse = accountDetailsResponse;
                        Debug.Log(AccountDetailsResponse.DisplayName);
                        Debug.Log(AccountDetailsResponse.UserId);
                        //SaveAll();
                        
                    } else {
                        Debug.Log("Error AccountDetailsResponse");
                    }
                    
                });
            } else {
                Debug.Log("Error Authenticating Device...");
            }
        });
    }

    public void ShowRegisterPrompt()
    {
        MainMenu.MainContainer.DOLocalMove(new Vector3(0,-600f,0), .5f)
            .SetEase(Ease.OutQuad);
        RegisterTransform.DOLocalMove(Vector3.zero, .5f).SetEase(Ease.InBounce);
    }

    public void HideRegisterPrompt()
    {
        MainMenu.MainContainer.DOLocalMove(Vector3.zero, .5f).SetEase(Ease.InQuad);
        RegisterTransform.DOLocalMove(new Vector3(0,-450f,0), .5f).SetEase(Ease.OutQuad);
    }
    
    public void RegisterNewUser()
    {
        string userName = RegisterInput.text;
        PlayerPrefs.SetString("UserName", userName);
        PlayerPrefs.Save();
        HideRegisterPrompt();
        
        new GameSparks.Api.Requests.RegistrationRequest()
            .SetDisplayName(userName)
            .SetPassword("test_password_123456")
            .SetUserName(userName)
            .Send((response) => {
                    if (!response.HasErrors) {
                        Debug.Log("Player Registered");
                    }
                    else
                    {
                        Debug.Log("Error Registering Player");
                    }
                }
            );
        DeviceAuthentication();
    }

    private void SaveAll()
    {
        foreach (KeyValuePair<string,FighterPart> keyValuePair in FighterPartsCollection.AllFighterParts)
        {
            FighterPart fighterPart = keyValuePair.Value;
            fighterPart.Upgrade(1);
            Storage.SaveFighterPart(fighterPart);
        }

        foreach (KeyValuePair<string,Weapon> keyValuePair in WeaponsCollection.AllWeapons)
        {
            Weapon weapon = keyValuePair.Value;
            weapon.Upgrade(1);
            Storage.SaveWeapon(weapon);
        }
        
        foreach (KeyValuePair<FighterNum,Fighter> keyValuePair in FightersCollection.Fighters)
        {
            Fighter fighter = keyValuePair.Value;
            Storage.SaveFighter(fighter);
        }
    }
}