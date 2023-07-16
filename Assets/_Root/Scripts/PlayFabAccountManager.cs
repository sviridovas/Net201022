using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;
    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountSuccess, OnFailure);
    }
    private void OnGetAccountSuccess(GetAccountInfoResult result)
    {
        _titleLabel.text = $"Welcome back, Player {result.AccountInfo.TitleInfo.DisplayName} with ID {result.AccountInfo.PlayFabId}, ";
    }
    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }
}
