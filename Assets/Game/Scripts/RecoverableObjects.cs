using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverableObjects : MonoBehaviour
{
    public static RecoverableObjects instance;
    GameObject duplicate;
    private void Awake() {    
        instance = this;
        duplicate = new GameObject();
        duplicate.name = "RecoverableObjects (clone)";
        for (int i = 0; i < transform.childCount; i++)
        {
            var objectCopy = Instantiate(transform.GetChild(i));
            objectCopy.SetParent(duplicate.transform);
        }
        gameObject.SetActive(false);
    }
    public void Recovery()
    {
        //Создаем новый объект-контейнер
        Destroy(duplicate);
        duplicate = new GameObject();
        duplicate.name = "RecoverableObjects (clone)";
        //Восстанавливаем объекты
        for (int i = 0; i < transform.childCount; i++)
        {
            var objectCopy = Instantiate(transform.GetChild(i));
            objectCopy.SetParent(duplicate.transform);
        }
    }
}
