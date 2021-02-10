using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage
{
    void TakeDamage(int takeDamge);
}

public interface ITakePosition
{
    void TakePosition(Vector3 takeposition);
}