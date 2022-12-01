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

    
    
    [Tooltip("Размер обьекта 2")] 
    public Vector3 scale2;
    [Tooltip("Расположение обьекта 2")] 
    public Vector3 position2;
    [Tooltip("Размер колайдера 2 обьекта")] 
    public Vector2 scalecollider2;
    [Tooltip("Оффсет 2 обьекта")] 
    public Vector2 offsetcollider2;
    
    [Tooltip("Отключаемый обьект")] 
    public GameObject offGameObject;

    protected Vector2 scalecollider1;
    protected Vector2 offsetcollider1;
    
    private Vector3 scale1; 
    private Vector3 position1;
    private bool startForm = true;
    
    
    protected Character _character;
    protected CapsuleCollider2D _collider;
    
    //Здесь должен быть старт, потому что, как я понял, inizialization вызывается до создания модельки и локаньные размеры будут 0 0 0 
    public void Start()
    {
        scale1 = transform.Find(nameForm1).localScale;
        position1 = transform.Find(nameForm1).localPosition;
        _character = GetComponent<Character>();
        _collider = GetComponent<CapsuleCollider2D>();
        scalecollider1 = _collider.size;
        offsetcollider1 = _collider.offset;
    }
    public void changeForm()
    {
        if (startForm)
        {
            var obj = Instantiate(form2, transform);
            obj.name = nameForm2;
            obj.transform.localScale = scale2;
            obj.transform.localPosition = position2;
            Destroy(transform.Find(nameForm1).gameObject);
            startForm = !startForm;
            _character.Animator = transform.Find(nameForm2).GetComponent<Animator>();
            _collider.size = scalecollider2;
            _collider.offset = offsetcollider2;
            offGameObject.SetActive(false);
        }
        else
        {
            var obj = Instantiate(form1, transform);
            obj.name = nameForm1;
            obj.transform.localScale = scale1;
            obj.transform.localPosition = position1;
            Destroy(transform.Find(nameForm2).gameObject);
            startForm = !startForm;
            _character.Animator  = transform.Find(nameForm1).GetComponent<Animator>();
            _collider.size = scalecollider1;
            _collider.offset = offsetcollider1;
            offGameObject.SetActive(true);
        }
    }
}