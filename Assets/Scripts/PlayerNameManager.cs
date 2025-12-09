using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    public static PlayerNameManager Instance { get; private set; }

    private string _playerName = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerName(string newName)
    {
        _playerName = newName.Trim();
    }

    public string GetPlayerName()
    {
        return _playerName;
    }

    public bool HasValidName()
    {
        return !string.IsNullOrWhiteSpace(_playerName);
    }

    public void ClearPlayerName()
    {
        _playerName = "";
    }
}
