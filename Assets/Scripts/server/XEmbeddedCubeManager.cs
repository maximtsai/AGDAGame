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

        if (data.tag == 000)
        {
            Debug.Log("YAY~~!!");
            Debug.Log((string) data.data);
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
