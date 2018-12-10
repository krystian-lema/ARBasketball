using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
	private const string DB_NAME = "HighScoreDB.sqlite";
	private const string EMPTY_DB_NAME = "EmptyHighScoreDB.sqlite";
	
	private bool dbInitialized;
	private float timeOut = 10f;
	
	private static string DbPath
	{
		get
		{
			#if !UNITY_EDITOR && UNITY_ANDROID
			return Application.persistentDataPath + "/" + DB_NAME;
			#else
			return Application.dataPath + "/" + DB_NAME;
			#endif
		}
	}

	private static string ConnectionPath
	{
		get { return "URI=file:" + DbPath; }
	}

	private static string DbStreamingAssetsPath
	{
		get { return "jar:file://" + Application.dataPath + "!/assets/" + EMPTY_DB_NAME; }
	}

	private void Start()
	{
		InitConnection();
		GetScores(Test);
	}

	public void Test(List<HighScore> highScores)
	{
		Debug.LogFormat("HighScores:");
		foreach (var highScore in highScores)
		{
			Debug.LogFormat(highScore.ToString());
		}
	}

	private void InitConnection()
	{
		dbInitialized = false;
		StartCoroutine(InitConnectionCoroutine());
	}

	private IEnumerator InitConnectionCoroutine()
	{		
		if (!File.Exists(DbPath))
		{
			// if it doesn't ->
			// open StreamingAssets directory and load the db ->

			Debug.LogFormat("There is no database. Creating new one from stream assets.");
			WWW loadDB = new WWW(DbStreamingAssetsPath);

			float timer = timeOut;
			while (!loadDB.isDone && timer > 0)
			{
				timer -= Time.deltaTime;
				yield return null;
			}

			if (timer <= 0)
			{
				Debug.LogError("Time out! Can't load database.");
			}
			// CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
 
			// then save to Application.persistentDataPath
 
			Debug.LogFormat("Writing new database.");
			File.WriteAllBytes(DbPath, loadDB.bytes);
			yield return null;
			
			dbInitialized = true;
		}
		else
		{
			Debug.LogFormat("Database already exists.");
			dbInitialized = true;
		}
	}

	public void GetScores(Action<List<HighScore>> onComplete)
	{
		StartCoroutine(GetScoresCoroutine(onComplete));
	}

	private IEnumerator GetScoresCoroutine(Action<List<HighScore>> onComplete)
	{
		float timer = timeOut;
		while (!dbInitialized && timer > 0)
		{
			timer -= Time.deltaTime;
			yield return null;
		}

		if (timer <= 0)
		{
			Debug.LogError("Time out! Database is still not initialized.");
		}
		
		var highScores = new List<HighScore>();
		
		using (IDbConnection dbConnection = new SqliteConnection(ConnectionPath))
		{
			dbConnection.Open();

			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				string sqlQuery = "SELECT * FROM HighScores";

				dbCmd.CommandText = sqlQuery;

				using (IDataReader reader = dbCmd.ExecuteReader())
				{
					while (reader.Read())
					{
						highScores.Add(new HighScore(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
					}
					
					dbConnection.Close();
					reader.Close();
				}
			}
		}

		highScores.Sort();

		if (onComplete != null)
		{
			onComplete(highScores);
		}
	}

	public void InsertScore(string username, int newScore, Action onComplete)
	{
		StartCoroutine(InsertScoreCoroutine(username, newScore, onComplete));
	}
	
	private IEnumerator InsertScoreCoroutine(string username, int newScore, Action onComplete)
	{
		float timer = timeOut;
		while (!dbInitialized && timer > 0)
		{
			timer -= Time.deltaTime;
			yield return null;
		}

		if (timer <= 0)
		{
			Debug.LogError("Time out! Database is still not initialized.");
		}

		using (IDbConnection dbConnection = new SqliteConnection(ConnectionPath))
		{
			dbConnection.Open();

			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				string sqlQuery = string.Format("INSERT INTO HighScores(Name,Score) VALUES(\"{0}\",\"{1}\")", username, newScore);

				dbCmd.CommandText = sqlQuery;
				dbCmd.ExecuteScalar();
				dbConnection.Close();
			}
		}

		if (onComplete != null)
		{
			onComplete();
		}
	}

	public void DeleteScore(int id, Action onComplete)
	{
		StartCoroutine(DeleteScoreCoroutine(id, onComplete));
	}
	
	public IEnumerator DeleteScoreCoroutine(int id, Action onComplete)
	{
		float timer = timeOut;
		while (!dbInitialized && timer > 0)
		{
			timer -= Time.deltaTime;
			yield return null;
		}

		if (timer <= 0)
		{
			Debug.LogError("Time out! Database is still not initialized.");
		}
		
		using (IDbConnection dbConnection = new SqliteConnection(ConnectionPath))
		{
			dbConnection.Open();

			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				string sqlQuery = string.Format("DELETE FROM HighScores WHERE Id = \"{0}\"", id);

				dbCmd.CommandText = sqlQuery;
				dbCmd.ExecuteScalar();
				dbConnection.Close();
			}
		}
		
		if (onComplete != null)
		{
			onComplete();
		}
	}
}
