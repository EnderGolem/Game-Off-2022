using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
/// <summary>
/// Решение позволяющие связывать несколько решений ИИ в одно
/// с помощью логических операций
/// ОПЕРАЦИИ ВЫПОЛНЯЮТСЯ В ПРЯМОМ ПОРЯДКЕ!!!
/// НЕ НАДЕЙТЕСЬ, ЧТО ЭТОТ СКРИПТ САМ РАССТАВИТ ПРИОРИТЕТ ОПЕРАЦИЙ!!!
/// </summary>
public class AIDecisionLogicalOperation : AIDecision
{ 
    [Tooltip("Решения, которые нужно объединить в одно")]
    [SerializeField]
    protected AIDecision[] decisions;
    [Tooltip("Операция, которой связываются решения")]
    [SerializeField]
    protected BinaryLogicalOperation[] _logicalOperation;
    [Tooltip("Какие из решений нужно инвертировать")]
    [SerializeField]
    protected bool[] inverseDecisions;
    public override bool Decide()
    {
        var t = decisions[0].Decide();
        if (inverseDecisions[0]) t = !t;
        var f = t;
        for (int i = 1; i < decisions.Length; i++)
        {
            switch (_logicalOperation[i-1])
            {
              case BinaryLogicalOperation.AND:
                  t = decisions[i].Decide();
                  if (inverseDecisions[i]) t = !t;
                  f = f && t;
                  break;
              case BinaryLogicalOperation.OR: 
                  t = decisions[i].Decide();
                  if (inverseDecisions[i]) t = !t;
                  f = f || t;
                  break;
            }
        }

        return f;
    }
    
    public override void Initialization()
    {
        
    }
    
    public override void OnEnterState()
    {
        foreach (var dec in decisions)
        {
            dec.OnEnterState();
        }
    }
    
    public override void OnExitState()
    {
        foreach (var dec in decisions)
        {
            dec.OnExitState();
        }
    }

    private void OnValidate()
    {
        if (_logicalOperation.Length != decisions.Length - 1)
        {
            Debug.LogError("Ошибка! Количество логических операций должно быть на 1 меньше " +
                           "числа связываемых решений");
        }
        
        if (inverseDecisions.Length != decisions.Length)
        {
            Debug.LogError("Ошибка! Количество инверсий должно быть равно " +
                           "числу связываемых решений");
        }
    }
}

public enum BinaryLogicalOperation
{
    AND,OR
}
