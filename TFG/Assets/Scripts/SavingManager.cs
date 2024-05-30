using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SavingManager
{
    public static string path = Application.persistentDataPath + "/partidaguardada";

    public static void Guardar(int i, Personaje p, LibretaPruebas l, Inventario inv, Light[] lights)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path + i.ToString(), FileMode.Create);
        GameData gameData = new GameData(p, l, inv, lights);
        binaryFormatter.Serialize(fileStream, gameData);
        fileStream.Close();
    }

    public static GameData Cargar(int i)
    {
        if (File.Exists(path + i.ToString()))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path + i.ToString(), FileMode.Open);
            GameData gameData = binaryFormatter.Deserialize(fileStream) as GameData;
            fileStream.Close();
            return gameData;
        }
        else
        {
            Debug.LogError("Partida guardada no encontrada");
            return null;
        }
    }
}

