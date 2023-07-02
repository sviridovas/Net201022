using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField email;
    [SerializeField] TMP_InputField password;
    [SerializeField] Button signUp;
    [SerializeField] Button loginBtn;
    [SerializeField] TMP_Text error;

    private const string AuthGuidKey = "authorization-guid";
    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();

    public void Start()
    {
        // error.enabled = false;
        error.text = "Loadiing...";

        if (signUp != null)
        {
            signUp.onClick = new Button.ButtonClickedEvent();
            signUp.onClick.AddListener(CreateAccount);
        }

        if (loginBtn != null)
        {
            loginBtn.onClick = new Button.ButtonClickedEvent();
            loginBtn.onClick.AddListener(SignIn);
        }

        LoginWithCustomID();
    }

    public void LoginWithCustomID()
    {
        // Here we need to check whether TitleId property is configured in settings or not
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            * If not we need to assign it to the appropriate variable manually
            * Otherwise we can just remove this if statement at all
            */
            PlayFabSettings.staticSettings.TitleId = "20C7F";
        }

        var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
        var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = id,
            CreateAccount = !needCreation
        }, success =>
        {
            error.enabled = false;
            PlayerPrefs.SetString(AuthGuidKey, id);

            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), result =>
            {
                HandleCatalog(result.Catalog);
                Debug.Log($"Catalog was loaded successfully!");
            },
            error =>
            {
                var errorMessage = error.GenerateErrorReport();
                Debug.LogError($"Something went wrong: {errorMessage}");
            });
        }, err =>
        {
            Debug.LogError($"Fail: {err.ErrorMessage}");
            error.enabled = true;
            error.text = err.ErrorMessage;
        });
    }

    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            _catalog.Add(item.ItemId, item);
            Debug.Log($"Catalog item {item.ItemId} was added successfully!");
        }
    }
    // public void LogOut()
    // {
    //     PlayFabClientAPI.ForgetAllCredentials();

    //     loginBtn.image.color = Color.blue;
    //     loginBtn.GetComponentInChildren<TMP_Text>().text = "Log In";
    // }

    // public void OnLoginSuccess(LoginResult result)
    // {
    //     Debug.Log("Congratulations, you made successful API call!");

    //     loginBtn.image.color = Color.green;
    //     loginBtn.GetComponentInChildren<TMP_Text>().text = "Log Out";
    // }

    // private void OnLoginFailure(PlayFabError error)
    // {
    //     var errorMessage = error.GenerateErrorReport();
    //     Debug.LogError($"Something went wrong: {errorMessage}");

    //     loginBtn.image.color = Color.blue;
    //     loginBtn.GetComponentInChildren<TMP_Text>().text = "Log In";
    // }

    public void CreateAccount()
    {
        var userName = username.text;

        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = userName,
            Email = email.text,
            Password = password.text,
            RequireBothUsernameAndEmail = true
        }, result =>
        {
            Debug.Log($"Success: {userName}");
            error.enabled = false;
        }, err =>
        {
            Debug.LogError($"Fail: {err.ErrorMessage}");
            error.enabled = true;
            error.text = err.ErrorMessage;
        });
    }

    public void SignIn()
    {
        var userName = username.text;

        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = userName,
            Password = password.text
        }, result =>
        {
            Debug.Log($"Success: {userName}");
            error.enabled = false;
        }, err =>
        {
            Debug.LogError($"Fail: {err.ErrorMessage}");
            error.enabled = true;
            error.text = err.ErrorMessage;
        });
    }

}