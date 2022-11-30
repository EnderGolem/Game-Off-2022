using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.U2D.IK;

public class DeadBodyRagdoll : MonoBehaviour
{
    List<Rigidbody2D> bodies = new List<Rigidbody2D>();
    List<GameObject> objectsIK = new List<GameObject>();
    List<GameObject> sprites = new List<GameObject>();
    IKManager2D manager = null;
    Animator    animator = null;
    Character owner;

    [SerializeField, Header("Нужно ли уничтожать тело")]
    protected bool autoDestruct = false;
    [SerializeField, Header("Время, по истечении которого тело будет уничтожено ")]
    protected float autoDestructTime = 5;
    [SerializeField, Header("Уничтожаются ли части тела в случайной последовательности")]
    protected bool autoDestructRandomize = true;
    [SerializeField, Header("Разброс времени уничтожения частей тела")]
    protected float autoDestructSpread = .25f;
    [SerializeField] Transform cameraTarget;
    private void Awake()
    {
        transform.parent?.TryGetComponent(out owner);
        manager = GetComponent<IKManager2D>();
        animator = GetComponent<Animator>();
        var container = GetComponentsInChildren<Rigidbody2D>();
        foreach (var body in container)
        {
            bodies.Add(body);
        }
        var container2 = GetComponentsInChildren<SpriteRenderer>();
        foreach (var spr in container2)
        {
            sprites.Add(spr.gameObject);
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
        //if (transform.parent == null) 
        //    Invoke("Kill", Random.Range(3, 12));
    }

    [ContextMenu("KILL!")]
    //Перевод тела в Ragdoll-состояние. Отключается анимация и инверсная кинематика, включаются коллайдеры конечностей
    public void Kill()
    {
        if (owner != null)
        {
            //DeadBodyRagdoll dublicate = Instantiate(gameObject, transform.position,transform.rotation).GetComponent<DeadBodyRagdoll>();
            transform.SetParent(owner.transform.parent);
            if (owner.gameObject.tag=="Player")
            {
                CameraParams.SetFollowTarget(cameraTarget);
                CameraParams.SetOrthoSizeOnDead();
                Time.timeScale = 0.25f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }
            //dublicate.transform.localScale = transform.parent.localScale;
            //dublicate.Kill();
        }

        {
            foreach (var body in bodies)
            {
                body.simulated = true;
                body.gameObject.layer = LayerMask.NameToLayer("Corpses");
            }
            foreach (var obj in objectsIK)
            {
                Destroy(obj);
            }
            if (autoDestruct)
            {
                if (autoDestructRandomize)
                {
                    foreach (var spr in sprites)
                    {
                        Destroy(spr.gameObject, autoDestructTime + Random.Range(-autoDestructSpread,autoDestructSpread));
                    }
                }
                int b = autoDestructRandomize ? 1 : 0;
                Destroy(gameObject,autoDestructTime+autoDestructSpread*b);
            }
            //for (int i = 0; i<transform.childCount; i++)
            //{
            //    transform.GetChild(i).SetParent(transform);
            //}
            Destroy(manager);
            Destroy(animator);
        }
    }
}
