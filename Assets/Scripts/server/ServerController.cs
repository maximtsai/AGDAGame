﻿using UnityEngine;
using System.Collections.Generic;
using DarkRift;

public class ServerController : MonoBehaviour 
{
    private GameObject hittables;
    private GameObject listOfWeapons;

    void Start ()
	{
		//Latch on to onData
		ConnectionService.onData += OnData;
        hittables = GameObject.Find("hittables");
        listOfWeapons = GameObject.Find("ListOfWeapons");
	}

	void Update()
	{
		foreach (ConnectionService cs in DarkRiftServer.GetAllConnections())
		{
			foreach (PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
            {
                // Sync playerbodies
                Vector2 position = pc.playerBody.position;
                cs.SendReply(011, pc.playerId, new float[] { position.x, position.y });
                cs.SendReply(012, pc.playerId, pc.playerBody.rotation);

                Vector2 velocity = pc.playerBody.velocity;
                cs.SendReply(013, pc.playerId, new float[] { velocity.x, velocity.y });
                cs.SendReply(014, pc.playerId, pc.playerBody.angularVelocity);
            }
		}
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

        //New Client has connected
        if (data.tag == 100)
        {
            List<ObstacleContainer> obstacleData = new List<ObstacleContainer>();

            //Prepare the Weapon and Obstacle data
            //foreach (Transform weapon in listOfWeapons.transform)
            //{
            //    //TODO
            //    
            //}
            foreach (Transform hitTransform in hittables.transform)
            {
                bool isSpinCube = hitTransform.gameObject.GetComponent<Rigidbody2D>().sharedMaterial != null;
                ObstacleContainer toSend = new ObstacleContainer(hitTransform, isSpinCube);
                obstacleData.Add(toSend);
            }
            //con.SendReply(030, 255, weaponData);
            Debug.Log("Send it out!!!");
            con.SendReply(040, 255, obstacleData);
        }

        if (data.tag == 110)
        {
            Debug.Log(data.data);
            // Key input
            GameObject player = getPlayerById(data.subject);
            EngineScript engineScript = player.GetComponentInChildren< EngineScript >();
            WeaponControl weaponControl = player.GetComponentInChildren<WeaponControl>();

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
                    weaponControl.pressEquipKey();
                    break;
                case 51:
                    weaponControl.releaseEquipKey();
                    break;
                case 60:
                    weaponControl.pressActivateKey();
                    break;
                case 61:
                    weaponControl.releaseActivateKey();
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

    GameObject getPlayerById(ushort id)
    {
        foreach (PlayerController script in GameObject.FindObjectsOfType<PlayerController>())
        {
            if (script.playerId == id)
            {
                return script.gameObject;
            }
        }
        Debug.Log("Couldn't find player id #" + id);
        return null;
    }
}
