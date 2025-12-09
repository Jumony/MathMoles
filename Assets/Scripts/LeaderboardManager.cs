using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ScoreEntry
{
    public int score;
    public string playerName;

    public ScoreEntry(int score, string playerName = "Player")
    {
        this.score = score;
        this.playerName = playerName;
    }
}

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    [SerializeField] private int maxLeaderboardEntries = 10;

    private LeaderboardData _leaderboard = new();
    
    private const string FileName = "leaderboard.json";
    private static string _filePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _filePath = Path.Combine(Application.persistentDataPath, FileName);
        Debug.Log("Leaderboard save path: " + _filePath);

        LoadFromDisk();
    }

    public void AddScore(int score, string playerName = "Player")
    {
        if (score <= 0) return;

        _leaderboard.entries.Add(new ScoreEntry(score, playerName));

        _leaderboard.entries = _leaderboard.entries
            .OrderByDescending(x => x.score)
            .Take(maxLeaderboardEntries)
            .ToList();

        SaveToDisk();
    }

    public List<ScoreEntry> GetLeaderboard()
    {
        return new List<ScoreEntry>(_leaderboard.entries);
    }

    public int GetPreviousHighScore()
    {
        return _leaderboard.entries.Count <= 1 ? 0 : _leaderboard.entries[1].score;
    }

    public void ClearLeaderboard()
    {
        _leaderboard = new LeaderboardData();
        SaveToDisk();
    }

    private void SaveToDisk()
    {
        var json = JsonUtility.ToJson(_leaderboard, true);
        
        var temp = _filePath + ".tmp";
        File.WriteAllText(temp, json);
        
        if (File.Exists(_filePath)) File.Replace(temp, _filePath, null);
        else File.Move(temp, _filePath);

        Debug.Log("Leaderboard saved to -> " + _filePath);
    }

    private void LoadFromDisk()
    {
        if (!File.Exists(_filePath))
        {
            Debug.Log("Leaderboard file not found. Creating new leaderboard file.");
            _leaderboard = new LeaderboardData { entries = new List<ScoreEntry>() };
            return;
        }

        try
        {
            var json = File.ReadAllText(_filePath);
            _leaderboard = JsonUtility.FromJson<LeaderboardData>(json);

            Debug.Log("Leaderboard loaded from -> " + _filePath + " with " + _leaderboard.entries.Count + " entries.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading leaderboard from file: " + e);
            _leaderboard = new LeaderboardData { entries = new List<ScoreEntry>() };
        }
    }
}

[Serializable]
public class LeaderboardData
{
    [FormerlySerializedAs("Entries")] public List<ScoreEntry> entries = new();
}
