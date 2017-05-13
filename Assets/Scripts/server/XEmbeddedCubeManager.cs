using UnityEngine;
using System.Collections;

using DarkRift;

public class XEmbeddedCubeManager : MonoBehaviour
{
	[SerializeField]
	Transform[] cubes;

	void Start ()
	{
		//Latch on to onData
		ConnectionService.onData += OnData;
	}

	void Update()
	{
		/*
        //Broadcast all positions to all clients constantly
		foreach (ConnectionService cs in DarkRiftServer.GetAllConnections())
		{
			for (int i=0; i<cubes.Length; i++)
			{
				cs.SendReply(0, (ushort)i, (Vector3Carrier)cubes[i].position);
			}
		}
        */
	}

	//Called when we receive data
	void OnData(ConnectionService con, ref NetworkMessage data)
	{
		//Decode the data so it is readable
		data.DecodeData ();
        
        Debug.Log(data.tag);

        if (data.tag == 000)
        {
            Debug.Log((string) data.data);
        }

        if (data.tag == 110)
        {

            Debug.Log(data.data);
            // Key input
            GameObject player = GameObject.Find("Player");
            Debug.Log(player);
            EngineScript engineScript = player.GetComponentInChildren< EngineScript >();
            Debug.Log(engineScript);

            switch ((int) data.data)
            {
                case 10:
                    engineScript.setForward(true);
                    break;
                case 11:
                    engineScript.setForward(false);
                    break;
                case 20:
                    engineScript.setBackward(true);
                    break;
                case 21:
                    engineScript.setBackward(false);
                    break;
                case 30:
                    engineScript.setTurnRight(true);
                    break;
                case 31:
                    engineScript.setTurnRight(false);
                    break;
                case 40:
                    engineScript.setTurnLeft(true);
                    break;
                case 41:
                    engineScript.setTurnLeft(false);
                    break;
                case 50:
                    break;
                case 51:
                    break;
                case 60:
                    break;
                case 61:
                    break;
            }
        }
        /*
        //Convert the data to Vector3
        Vector3 pos = (Vector3)(Vector3Carrier)data.data;
        ushort id = data.subject;

        //Move the cube
        cubes[id].GetComponent<Rigidbody>().MovePosition(pos);
        */

	}
}
