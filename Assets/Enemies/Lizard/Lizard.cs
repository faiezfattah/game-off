using UnityEngine;
using UnityEngine.Assertions;

public class Lizard : MonoBehaviour
{
    [SerializeField]
    private Transform headPivotTransform;

    private void Awake()
    {
        Assert.IsNotNull(headPivotTransform);
    }

    // Update is called once per frame
    void Update()
    {
        headPivotTransform.Rotate(0, 0, 1);
    }
}
