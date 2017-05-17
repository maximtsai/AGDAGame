using UnityEngine;

[System.Serializable]
public class ObstacleContainer : System.Object{

    public float[] position;
    public float[] rotation;
    public float[] localScale;
    public bool isSpinnable;

    public ObstacleContainer(Transform transformData, bool isSpinnable)
    {
        position = new float[] { transformData.position.x, transformData.position.y , transformData.position.z};
        rotation = new float[] { transformData.rotation.x, transformData.rotation.y, transformData.rotation.z, transformData.rotation.w };
        localScale = new float[] { transformData.localScale.x, transformData.localScale.y, transformData.localScale.z };
        this.isSpinnable = isSpinnable;
    }
}
