using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapForm : MonoBehaviour
{
    [Tooltip("Начальная форма")] 
    public GameObject form1;
    [Tooltip("Вторая форма")] 
    public GameObject form2;

    
    [Tooltip("Имя формы на начальном обьекте")] 
    public string nameForm1;
    [Tooltip("Имя формы 2 на обьекте")] 
    public string nameForm2;
    private int numForm;

    
    private Vector3 scale1; 
    private Vector3 position1;
    
    
    [Tooltip("Размер обьекта 2")] 
    public Vector3 scale2;
    [Tooltip("Расположение обьекта 2")] 
    public Vector3 position2;
    private bool startForm = true;

    //Здесь должен быть старт, потому что, как я понял, inizialization вызывается до создания модельки и локаньные размеры будут 0 0 0 
    public void Start()
    {
        scale1 = transform.Find(nameForm1).localScale;
        position1 = transform.Find(nameForm1).localPosition;
    }
    public void changeForm()
    {
        if (startForm)
        {
            var obj = Instantiate(form2, transform.position, transform.rotation);
            obj.transform.SetParent(transform);
            obj.name = nameForm2;
            obj.transform.localScale = scale2;
            obj.transform.localPosition = position2;
            Destroy(transform.Find(nameForm1).gameObject);
            startForm = !startForm;
        }
        else
        {
            var obj = Instantiate(form1, transform.position, transform.rotation);
            obj.transform.SetParent(transform);
            obj.name = nameForm1;
            obj.transform.localScale = scale1;
            obj.transform.localPosition = position1;
            Destroy(transform.Find(nameForm2).gameObject);
            startForm = !startForm;
        }
    }
}