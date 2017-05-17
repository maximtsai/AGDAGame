using System;
using System.Collections.Generic;
using UnityEngine;

using DarkRift;

public class ClientSync : MonoBehaviour
{
    public GameObject hittables;
    public GameObject listOfWeapons;

    public GameObject cubePrefab;
    
	void Start()
	{
		DarkRiftAPI.onData += OnDataReceived;
	}

	void OnDataReceived(byte tag, ushort subject, object data)
	{
		if (tag == 011)
        {
            GameObject player = getPlayerById((ushort) subject);
            float[] rawPosition = (float[]) data;
            Vector2 position = new Vector2(rawPosition[0], rawPosition[1]);
            player.GetComponent<Rigidbody2D>().position = position;
            // TODO: If displacement is small then instead MovePosition() (can't use for large b/c explosions)
        }
        if (tag == 012)
        {
            GameObject player = getPlayerById((ushort)subject);
            player.GetComponent<Rigidbody2D>().rotation = (float) data;
        }
        if (tag == 013)
        {
            GameObject player = getPlayerById((ushort)subject);
            float[] rawVelocity = (float[])data;
            Vector2 velocity = new Vector2(rawVelocity[0], rawVelocity[1]);
            player.GetComponent<Rigidbody2D>().velocity = velocity;
        }
        if (tag == 014)
        {
            GameObject player = getPlayerById((ushort)subject);
            player.GetComponent<Rigidbody2D>().angularVelocity = (float) data;
        }

        if (tag == 030)
        {
            //Initialize weapons

        }
        
        if (tag == 040)
        {
            //Initialize obstacles
            List<ObstacleContainer> obstacleData = (List<ObstacleContainer>)data;
            foreach (ObstacleContainer obsCon in obstacleData) {
                GameObject newObstacle = Instantiate(
                    cubePrefab, 
                    obsCon.transformData.position, //TODO test that this doesn't need to be localPosition and localRotation
                    obsCon.transformData.rotation, 
                    hittables.transform); //TODO test that this actually adds this to the hittables GameObject
                newObstacle.transform.localScale = obsCon.transformData.localScale;
            }
        }

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
