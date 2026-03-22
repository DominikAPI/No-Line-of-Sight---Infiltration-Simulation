using System.Collections;
using UnityEngine;

public interface IPatrol
{
    public void Turn(float angle);

    public void Patrol();
}
