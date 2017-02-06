using UnityEngine;
using System.Collections;

//개쩌는 클라이언트가 시작하는 부분, 유니티 메인 쓰레드에서 통신을 담당한다
public class KingGodClient : MonoBehaviour {
	private NetworkTranslator networkTranslator;

	public static KingGodClient instance;

	void Awake(){
		DontDestroyOnLoad(gameObject);
		instance = this;
		networkTranslator = GetComponent<NetworkTranslator>();

	}

	void Start () {
		networkTranslator.SetMsgHandler(gameObject.AddComponent<DemoMsgHandler>());

		Network_Client.Begin ();
	}		

	void OnApplicationQuit(){
		Network_Client.ShutDown();
	}

    public void Matching()
    {
        Network_Client.Send("Matching");
    }
}
