using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    static string path = Application.persistentDataPath + "/highscore.bin";
    public static void Save(int score)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveStream = new FileStream(path, FileMode.Create);
        formatter.Serialize(saveStream, score);
        saveStream.Close();
    }

    public static int Load()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            int? data = formatter.Deserialize(stream) as int?;
            stream.Close();

            return data == null ? 0 : (int)data;
        }

        return 0;
    }
}
