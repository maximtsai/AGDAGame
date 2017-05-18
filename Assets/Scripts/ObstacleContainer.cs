using UnityEngine;

[System.Serializable]
public class ObstacleContainer : System.Object{
    public ushort id;
    public float[] position; //Vector3 is not Serializable
    public float[] rotation; //Quaternion is not Serializable
    public float[] localScale;
    public bool isSpinnable;

    //Used for initialization
    public ObstacleContainer(ushort id, Transform transformData, bool isSpinnable)
    {
        this.id = id;
        position = new float[] { transformData.position.x, transformData.position.y , transformData.position.z};
        rotation = new float[] { transformData.rotation.x, transformData.rotation.y, transformData.rotation.z, transformData.rotation.w };
        localScale = new float[] { transformData.localScale.x, transformData.localScale.y, transformData.localScale.z };
        this.isSpinnable = isSpinnable;
    }

    //Used for updates
    public ObstacleContainer(ushort id, Vector3 position, Quaternion rotation)
    {
        this.id = id;
        this.position = new float[] { position.x, position.y, position.z };
        this.rotation = new float[] { rotation.x, rotation.y, rotation.z, rotation.w };
    }
}
