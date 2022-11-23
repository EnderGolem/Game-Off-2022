using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.U2D.IK;

public class DeadBodyRagdoll : MonoBehaviour
{
    List<Rigidbody2D> bodies = new List<Rigidbody2D>();
    List<GameObject> objectsIK = new List<GameObject>();
    IKManager2D manager = null;
    Animator    animator = null;
    Character owner;
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
            transform.SetParent(null);
            if (owner.gameObject.tag=="Player")
            {
                var cam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
                cam.Follow = cameraTarget;
                (cam as CinemachineVirtualCamera).m_Lens.OrthographicSize = 4;
                Time.timeScale = 0.25f;
            }
            //dublicate.transform.localScale = transform.parent.localScale;
            //dublicate.Kill();
        }

        {
            foreach (var body in bodies)
            {
                body.simulated = true;
                body.gameObject.layer = LayerMask.NameToLayer("Obstacles");
            }
            foreach (var obj in objectsIK)
            {
                Destroy(obj);
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
