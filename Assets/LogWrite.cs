using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Microsoft.Win32;

public class LogWrite : MonoBehaviour 
{
	public string log_application_name = "APP";
//	protected string machine_name="";
//	protected string log_filename="";
	
	public void writeLog( string value )
	{
		string machine_name = Environment.MachineName;
		string log_filename = Application.dataPath+"/"+DateTime.Now.ToString("yyyyMMdd") + "_"+log_application_name+"_"+machine_name+".csv";

		string s = "";
		s += DateTime.Now.ToString("yyyy/MM/dd");
		s += "," + DateTime.Now.ToString("HH:mm:ss");
		s += "," + machine_name;
		s += "," + value;
		Debug.Log( s );
		#if !UNITY_EDITOR
		StreamWriter sw = new StreamWriter(log_filename, true);
		sw.WriteLine(s);
		sw.Close();
		#endif
	}
	
	void Awake(){
		SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);

		//Debug.Log( "log_filename = "+log_filename );
		writeLog( "awake" );
	}

	void OnApplicationQuit()
	{
		writeLog( "quit" );
	}
	void OnDestroy()
	{
		writeLog( "OnDestroy" );
	}
	void OnDisable()
	{
		writeLog( "OnDisable" );
	}


	void OnGUI()
	{
		// http://dobon.net/vb/dotnet/process/appactivate.html
		System.Diagnostics.Process[] ps =
			System.Diagnostics.Process.GetProcesses();

		string str = "";
		str += ps.Length + "\n";
		for(int i = 0; i < ps.Length; i++){
			str += ( ps[i].MainWindowTitle  ) +"\n";
		}
		Debug.Log(str);

		GUILayout.BeginArea(new Rect(10,10,200,Screen.height));
		GUILayout.Label(str);
		GUILayout.EndArea();

	}
	//
	
	//ログオフ、シャットダウンの通知
	// http://dobon.net/vb/dotnet/system/sessionending.html
//	private void Form1_Load(object sender, System.EventArgs e)
//	{
//		//イベントをイベントハンドラに関連付ける
//		//フォームコンストラクタなどの適当な位置に記述してもよい
//		SystemEvents.SessionEnding +=
//			new SessionEndingEventHandler(SystemEvents_SessionEnding);
//	}
//	
//	private void Form1_Closed(object sender, System.EventArgs e)
//	{
//		//イベントを解放する
//		//フォームDisposeメソッド内の基本クラスのDisposeメソッド呼び出しの前に
//		//記述してもよい
//		SystemEvents.SessionEnding -=
//			new SessionEndingEventHandler(SystemEvents_SessionEnding);
//	}
	
	//ログオフ、シャットダウンしようとしているとき
	private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
	{
		writeLog( "SystemEvents_SessionEnding" );

		string s = "";

		if (e.Reason == SessionEndReasons.Logoff)
		{
			writeLog( "Logoff" );
		}
		else if (e.Reason == SessionEndReasons.SystemShutdown)
		{
			writeLog( "SystemShutdown" );
		}

		SystemEvents.SessionEnding -= new SessionEndingEventHandler(SystemEvents_SessionEnding);
	}
}