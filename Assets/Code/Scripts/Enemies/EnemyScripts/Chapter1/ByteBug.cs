using UnityEngine;

public class ByteBug : Enemy
{
    [Header("Byte Bug Attributes")]
    [SerializeField] private float speedMultiplier = 1.5f; // Increases speed

    void Start()
    {
        base.Start();
        this.moveSpeed *= speedMultiplier; // Make the Byte Bug faster
    }
}
