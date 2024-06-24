using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;
using System.Data.Common;
using System.Drawing;



public class DBManager : MonoBehaviour
{
    private string dbUri = "URI=file:mydb.sqlite";

    private string SQL_CREATE = " CREATE TABLE IF NOT EXISTS CoinData (" +
        "Id INTEGER UNIQUE NOT NULL PRIMARY KEY AUTOINCREMENT," +
        "PlayerId INTEGER NOT NULL," +
        "CoinType TEXT DEFAULT 'Basic'," +
        "CoinCount INTEGER NOT NULL);";
    private string SQL_CREATE_PLAYER = "CREATE TABLE IF NOT EXISTS PlayerData (" +
     "PlayerId INTEGER UNIQUE NOT NULL PRIMARY KEY AUTOINCREMENT," +
     "PlayerName TEXT NOT NULL," +
     "PlayerLevel INTEGER NOT NULL);";
    private string[] playerNames = { "Jorge", "Yolanda", "Charlie", "Vero", "Andrea" };
    private string[] COINTYPES = { "Basic", "Premium", "Advanced" };
    private int NUMDATA = 10;
    public TMP_Text input;
    public Button confirmButton;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        IDbConnection dbConnection = CreateAndOpenDatabase();
        AddRandomDataCoins(dbConnection);
        AddRandomDataPlayers(dbConnection);
        searchByCoinType("Basic");
        deleteCoinType("Advanced");
        ExportarDatos();
        confirmButton.onClick.AddListener(() => ButtonClick(dbConnection));
        dbConnection.Close();
        Debug.Log("End");
    }


    private IDbConnection CreateAndOpenDatabase()
    {

        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = SQL_CREATE;
        dbCmd.CommandText = SQL_CREATE_PLAYER;
        dbCmd.ExecuteReader();
        return dbConnection;
    }

    private void ButtonClick(IDbConnection dbConnection)
    {
        string namePlayer = input.text;
        if (!string.IsNullOrEmpty(namePlayer))
        {
            updatePlayerName( namePlayer);
            dbConnection.Close();
            Debug.Log("End");
        }
    }

    private void AddRandomDataCoins(IDbConnection dbConnection)
    {

        System.Random rnd = new System.Random();
        using (IDbCommand dbCmd = dbConnection.CreateCommand())
        {
            for (int i = 0; i < NUMDATA; i++)
            {
                int playerId = rnd.Next(1, 100);
                string coinType = COINTYPES[rnd.Next(COINTYPES.Length)];
                int coinCount = rnd.Next(0, 14);

                dbCmd.CommandText = "INSERT INTO CoinData (PlayerId, CoinType, CoinCount) VALUES (@PlayerId, @CoinType, @CoinCount)";
                dbCmd.Parameters.Clear();
                dbCmd.Parameters.Add(new SqliteParameter("@PlayerId", playerId));
                dbCmd.Parameters.Add(new SqliteParameter("@CoinType", coinType));
                dbCmd.Parameters.Add(new SqliteParameter("@CoinCount", coinCount));
                dbCmd.ExecuteNonQuery();
            }
        }
    }
    private void AddRandomDataPlayers(IDbConnection dbConnection)
    {
        System.Random rnd = new System.Random();
        using (IDbCommand dbCmd = dbConnection.CreateCommand())
        {
            for (int i = 0; i < NUMDATA; i++)
            {
                string playerName = playerNames[rnd.Next(playerNames.Length)];
                int playerLevel = rnd.Next(1, 101);

                dbCmd.CommandText = "INSERT INTO PlayerData (PlayerName, PlayerLevel) VALUES (@PlayerName, @PlayerLevel)";
                dbCmd.Parameters.Clear();
                dbCmd.Parameters.Add(new SqliteParameter("@PlayerName", playerName));
                dbCmd.Parameters.Add(new SqliteParameter("@PlayerLevel", playerLevel));
                dbCmd.ExecuteNonQuery();
            }
        }
    }
    private void searchByCoinType(string coinType)
    {
        using (IDbConnection dbConnection = new SqliteConnection(dbUri))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = @"
                SELECT CoinData.PlayerId, PlayerData.PlayerName, CoinData.CoinCount
                FROM CoinData
                JOIN PlayerData ON CoinData.PlayerId = PlayerData.PlayerId
                WHERE CoinData.CoinType = @CoinType";

                dbCmd.CommandText = sqlQuery;
                dbCmd.Parameters.Add(new SqliteParameter("@CoinType", coinType));

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int playerId = reader.GetInt32(0);
                        string playerName = reader.GetString(1);
                        int coinCount = reader.GetInt32(2);
                        Debug.Log($"Player ID: {playerId}, Player Name: {playerName}, Coin Count: {coinCount}");
                    }
                }
            }
        }
    }
    // Actualizar el nombre de un jugador sabiendo su ID
    private void updatePlayerName(int playerId, string newName)
    {
        using (IDbConnection dbConnection = new SqliteConnection(dbUri))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "UPDATE PlayerData SET PlayerName = @NewName " +
                                  "WHERE PlayerId = @PlayerId";

                dbCmd.CommandText = sqlQuery;
                dbCmd.Parameters.Add(new SqliteParameter("@NewName", newName));
                dbCmd.Parameters.Add(new SqliteParameter("@PlayerId", playerId));

                int rowsAffected = dbCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Debug.Log("Update correctamente realizado.");
                }
                else
                {
                    Debug.Log("No existe ese ID para actualizar el nombre.");
                }
            }
        }
    }
    //Actualizar  nombre del jugador sin saber ID
    private void updatePlayerName( string newName)
    {
        using (IDbConnection dbConnection = new SqliteConnection(dbUri))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "UPDATE PlayerData SET PlayerName = @NewName ";

                dbCmd.CommandText = sqlQuery;
                dbCmd.Parameters.Add(new SqliteParameter("@NewName", newName));

                int rowsAffected = dbCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Debug.Log("Update correctamente realizado.");
                }
                else
                {
                    Debug.Log("No se ha podido actualizar el nombre.");
                }
            }
        }
    }
    private void deleteCoinType(string tipoMoneda)
    {
        using (IDbConnection conexionBD = new SqliteConnection(dbUri))
        {
            conexionBD.Open();
            using (IDbCommand comandoBD = conexionBD.CreateCommand())
            {

                string consultaSQL = "DELETE FROM CoinData WHERE CoinType = @TipoMoneda";

                comandoBD.CommandText = consultaSQL;
                comandoBD.Parameters.Add(new SqliteParameter("@TipoMoneda", tipoMoneda));

                int filasAfectadas = comandoBD.ExecuteNonQuery();
                if (filasAfectadas > 0)
                {
                    Debug.Log($"Tipo de moneda '{tipoMoneda}' eliminado correctamente.");
                }
                else
                {
                    Debug.Log($"No existe ese tipo de moneda.");
                }
            }
        }
    }
    public void ExportarDatos()
    {
        List<PlayerData> playerDataList = ObtenerDatosDeJugadores();
        GenerarArchivoJSON(playerDataList);
        GenerarArchivoXML(playerDataList);
    }
    private List<PlayerData> ObtenerDatosDeJugadores()
    {
        List<PlayerData> playerDataList = new List<PlayerData>();

        using (IDbConnection conexionBD = new SqliteConnection(dbUri))
        {
            conexionBD.Open();
            using (IDbCommand comandoBD = conexionBD.CreateCommand())
            {
                string consultaSQL = "SELECT * FROM PlayerData";
                comandoBD.CommandText = consultaSQL;

                using (IDataReader reader = comandoBD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int playerId = reader.GetInt32(0);
                        string playerName = reader.GetString(1);
                        int playerLevel = reader.GetInt32(2);

                        PlayerData playerData = new PlayerData(playerId, playerName, playerLevel);
                        playerDataList.Add(playerData);
                    }
                }
            }
        }

        return playerDataList;
    }
    private void GenerarArchivoJSON(List<PlayerData> playerDataList)
    {
        string json = JsonConvert.SerializeObject(playerDataList, Newtonsoft.Json.Formatting.Indented);
        string filePath = Path.Combine(Application.persistentDataPath, "players.json");
        File.WriteAllText(filePath, json);
        Debug.Log("Archivo JSON generado en: " + filePath);
    }

    private void GenerarArchivoXML(List<PlayerData> playerDataList)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<PlayerData>));
        string filePath = Path.Combine(Application.persistentDataPath, "players.xml");

        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            serializer.Serialize(stream, playerDataList);
        }

        Debug.Log("Archivo XML generado en: " + filePath);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
