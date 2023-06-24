using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] Button loginBtn = null;

    public void Start()
    {
        if(loginBtn != null) {
            loginBtn.onClick = new Button.ButtonClickedEvent();
            loginBtn.onClick.AddListener(LogInOut);
        }
    }

    public void LogInOut(){
        // Here we need to check whether TitleId property is configured in settings or not
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            * If not we need to assign it to the appropriate variable manually
            * Otherwise we can just remove this if statement at all
            */
            PlayFabSettings.staticSettings.TitleId = "20С7А";
        }
     
        if(!PlayFabClientAPI.IsClientLoggedIn()) {
            var request = new LoginWithCustomIDRequest { CustomId = "GeekBrainsLesson3", CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        } else {
            PlayFabClientAPI.ForgetAllCredentials();
            
            loginBtn.image.color = Color.blue;
            loginBtn.GetComponentInChildren<TMP_Text>().text = "Log In";
        }
    }

    public void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made successful API call!");

        loginBtn.image.color = Color.green;
        loginBtn.GetComponentInChildren<TMP_Text>().text = "Log Out";
    }

    private void OnLoginFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
        
        loginBtn.image.color = Color.blue;
        loginBtn.GetComponentInChildren<TMP_Text>().text = "Log In";
    }
}