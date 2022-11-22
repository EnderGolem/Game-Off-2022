using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIActionSwapForm : AIAction
{
    protected SwapForm _swapForm;

    public override void Initialization()
    {
        _swapForm = GetComponent<SwapForm>();
    }
    public override void PerformAction()
    {
        _swapForm.changeForm();
    }
}
}
