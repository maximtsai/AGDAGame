using UnityEngine;

public class ObstacleContainer {

    public Transform transformData;
    public bool isSpinnable;

    public ObstacleContainer(Transform transformData, bool isSpinnable)
    {
        this.transformData = transformData;
        this.isSpinnable = isSpinnable;
    }
}
