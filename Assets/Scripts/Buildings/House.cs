using UnityEngine;

public class House : Building
{
    public int DwarfCount;

    protected override void Start()
    {
        base.Start();
        DiggingManager.Instance.Dwarves += DwarfCount;
    }

    private void OnDestroy()
    {
        DiggingManager.Instance.Dwarves -= DwarfCount;
    }
}