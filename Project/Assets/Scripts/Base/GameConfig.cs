using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameConfig : BaseManager<GameConfig>
{
    private cfg.Tables m_pTable;
    public cfg.Tables getTables()
    {
        if (m_pTable == null)
        {
            m_pTable = new cfg.Tables(LoadByteBuf);
        }
        return m_pTable;
    }

    private static JSONNode LoadByteBuf(string file)
    {
        return JSON.Parse(File.ReadAllText(Application.dataPath + "../../../Tool/GenerateDatas/json/" +
            file + ".json", System.Text.Encoding.UTF8));
    }
}
