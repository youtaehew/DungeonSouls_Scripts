using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public string Text = "DungeonSouls";
}
public class TextSaveData : MonoBehaviour
{
    public static readonly string FilePath = Path.Combine(Application.persistentDataPath + "TextData.json");
    public static Data Data = new Data();

    [SerializeField]
    TextMeshProUGUI txt;


    public static void SaveGame()
    {
        string data = JsonUtility.ToJson(Data, true);

        File.WriteAllText(FilePath, data);
    }

    public static void LoadGame()
    {
        if (exits)
        {
            string json = File.ReadAllText(FilePath);
            Data loadJson = JsonUtility.FromJson<Data>(json);
            Data = loadJson;
        }
        txt.text = Data.Text;
    }
}
