using System;
using System.Collections.Generic;
using UnityEngine;

using DarkRift;

public class ClientSync : MonoBehaviour
{
    private GameObject hittables;
    private GameObject listOfWeapons;

    public GameObject cubePrefab;
    public GameObject spinnableCubePrefab;

    void Start()
	{
		DarkRiftAPI.onData += OnDataReceived;
        hittables = GameObject.Find("hittables"); //TODO find using tags instead?
        listOfWeapons = GameObject.Find("ListOfWeapons");
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
            Debug.Log("TIME TO INITIALIZE WEAPONS");

        }
        if (tag == 031)
        {
            List<ObstacleContainer> weapons = (List<ObstacleContainer>) data;
            foreach (ObstacleContainer weaponData in weapons)
            {
                GameObject weapon = getWeaponById(weaponData.id);
                weapon.transform.position = weaponData.GetPosition();
                weapon.transform.rotation = weaponData.GetRotation();
            }
        }
        
        if (tag == 040)
        {
            //Initialize obstacles
            List<ObstacleContainer> obstacleData = (List<ObstacleContainer>)data;
            foreach (ObstacleContainer obsCon in obstacleData) {
                Vector3 position = new Vector3(obsCon.position[0], obsCon.position[1], obsCon.position[2]);
                Quaternion rotation = new Quaternion(obsCon.rotation[0], obsCon.rotation[1], obsCon.rotation[2], obsCon.rotation[3]);
                GameObject newObstacle = Instantiate(
                    obsCon.isSpinnable ? spinnableCubePrefab : cubePrefab,
                    position, //TODO test that this doesn't need to be localPosition and localRotation
                    rotation,
                    hittables.transform); //TODO test that this actually adds this to the hittables GameObject

                newObstacle.transform.localScale = new Vector3(obsCon.localScale[0], obsCon.localScale[1], obsCon.localScale[2]);
                newObstacle.gameObject.GetComponent<Hittable>().HittableId = obsCon.id;
            }
        }

        if (tag == 041)
        {
            //Update Obstacles
            Dictionary<ushort, ObstacleContainer> updateData = (Dictionary<ushort, ObstacleContainer>)data;

            //TODO this would be a lot neater if hittables were stored in a Dictionary as well....
            foreach (Transform hittableToUpdate in hittables.transform)
            {
                ObstacleContainer obsUpdate;
                ushort thisHitId = hittableToUpdate.gameObject.GetComponent<Hittable>().HittableId;
                if (updateData.TryGetValue(thisHitId, out obsUpdate))
                {
                    hittableToUpdate.position = obsUpdate.GetPosition();
                    hittableToUpdate.rotation = obsUpdate.GetRotation();
                }
            }
        }


    }


    GameObject getWeaponById(ushort id)
    {
        if (id == 0)
        {
            Debug.Log("Unset weapon id!!");
            return null;
        }

        foreach (WeaponScript script in GameObject.FindObjectsOfType<WeaponScript>())
        {
            if (script.weaponId == id)
            {
                return script.gameObject;
            }
        }
        Debug.Log("Couldn't find weapon id #" + id);
        return null;
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
