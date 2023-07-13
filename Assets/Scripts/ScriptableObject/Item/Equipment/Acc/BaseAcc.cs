using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Acc_", menuName ="Data/Item/Acc")]
public class BaseAcc : Equipment
{
    public BaseAcc()
    {
    }
    
    public EAccType accType;

    
    protected override void DataCopy(Item item)
    {
        base.DataCopy(item);
        
        accType = ((BaseAcc)item).accType;
    }
}
