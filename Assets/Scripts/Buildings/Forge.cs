using UnityEngine;

public class Forge : Building
{
    public float QualityMultiplier;

    protected override void Start()
    {
        base.Start();
        DiggingManager.Instance.Quality *= QualityMultiplier;
    }

    private void OnDestroy()
    {
        DiggingManager.Instance.Quality /= QualityMultiplier;
    }
}