using UnityEngine;

public class Tavern : Building
{
    public float SpeedMultiplier;

    protected override void Start()
    {
        base.Start();
        DiggingManager.Instance.Speed *= SpeedMultiplier;
    }

    private void OnDestroy()
    {
        DiggingManager.Instance.Speed /= SpeedMultiplier;
    }
}