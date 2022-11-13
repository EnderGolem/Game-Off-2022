using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class DeadBodyRagdoll : MonoBehaviour
{
    List<Rigidbody2D> bodies = new List<Rigidbody2D>();
    List<GameObject> objectsIK = new List<GameObject>();
    IKManager2D manager = null;
    Animator    animator = null;
    private void Awake()
    {
        manager = GetComponent<IKManager2D>();
        animator = GetComponent<Animator>();
        var container = GetComponentsInChildren<Rigidbody2D>();
        foreach (var body in container)
        {
            bodies.Add(body);
        }
        LimbSolver2D t;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out t))
            {
                objectsIK.Add(transform.GetChild(i).gameObject);
            }
        }
        //Чисто для демонстрации
        if (transform.parent == null) 
            Invoke("Kill", Random.Range(3, 12));
    }
    [ContextMenu("KILL!")]
    //Перевод тела в Ragdoll-состояние. Отключается анимация и инверсная кинематика, включаются коллайдеры конечностей
    public void Kill()
    {
        foreach (var body in bodies)
        {
            body.simulated = true;
            body.gameObject.layer = LayerMask.NameToLayer("Obstacles");
        }
        foreach (var obj in objectsIK)
        {
            obj.SetActive(false);
        }
        for (int i = 0; i<transform.childCount; i++)
        {
            transform.GetChild(i).SetParent(transform);
        }
        manager.enabled=false;
        animator.enabled=false;
        transform.SetParent(null);
    }
}
