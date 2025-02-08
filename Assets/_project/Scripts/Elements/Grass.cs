using NUnit.Framework;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public float growTime;
    public int maxGrowTime;

    private int _curGrowTime;
    private float _lastGrowthTime;

    public Material material1;
    public Material material2;
    public Material material3;

    public Renderer renderer;

    private void Update()
    {
        if (Time.time - _lastGrowthTime > growTime && _curGrowTime < maxGrowTime)
        {
            transform.localScale = new Vector3(1, transform.localScale.y * 2, 1); 
            _lastGrowthTime = Time.time;
            _curGrowTime += 1;
            if (_curGrowTime == 1)
            {
                renderer.material = material1;
            }
            else if (_curGrowTime == 2)
            {
                renderer.material = material2;
            }
            else if (_curGrowTime == 3)
            {
                renderer.material = material3;
            }
        }
    }
}
