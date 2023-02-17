using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class DataManager : MonoManager<DataManager>
{
    [SerializeField]
    private bool m_dontDestroyOnLoad = true;

    [SerializeField]
    private string m_fileName = "save.bin";

    [SerializeField]
    private string m_path = string.Empty;

    private void Awake()
    {
        if (m_dontDestroyOnLoad)
            DontDestroyOnLoad(this.gameObject);

        m_path = Path.Combine(Application.streamingAssetsPath, m_fileName);
    }

    public void SavePlayer(Player player)
    {
        string playerJsonData = JsonConvert.SerializeObject(player, Formatting.Indented);
        string playerBase64Data = Base64Encode(playerJsonData);
        PlayerPrefs.SetString("Player", playerBase64Data);
        //File.WriteAllText(m_path, playerBase64Data);
    }

    public Player LoadPlayerFromFile()
    {
        try
        {
            string loadedBase64Data = File.ReadAllText(m_path);
            string loadedJsonData = Base64Decode(loadedBase64Data);
            Player player = JsonConvert.DeserializeObject<Player>(loadedJsonData);
            return player;
        }
        catch(FileNotFoundException ex)
        {
            Debug.LogWarning(ex.Message);
            return null;
        }
    }

    public Player LoadPlayerFromMemory()
    {
        try
        {
            string loadedBase64Data = PlayerPrefs.GetString("Player");
            string loadedJsonData = Base64Decode(loadedBase64Data);
            Player player = JsonConvert.DeserializeObject<Player>(loadedJsonData);
            return player;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning(ex.Message);
            return null;
        }
    }

    private string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    private string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}