using System;
using UnityEngine;
public interface ICharacter
{
   
    void TakeDamage(long damage, bool value);
    string GetName();
    int GetId();
    void SetTarGet();
    void SetActiveArrow(bool active);
    void Selectobject();
   
}
