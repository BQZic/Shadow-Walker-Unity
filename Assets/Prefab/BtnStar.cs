using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnStar : MonoBehaviour
{


    private float timer;
    public GameObject Btn01;
    public GameObject Btn02;

    // Use this for initialization

    void Start()
    {

        timer = 0.0f;

    }

    // Update is called once per frame

    void Update()
    {

        timer += Time.deltaTime * 2;

        if (timer % 3 > 1.0f)

        {
            Btn01.SetActive(true);

        }

        else
        {
            Btn01.SetActive(false);

        }

    }

    public void Level01()
    {
        SceneManager.LoadScene(2);
    }
    public void CallBtn()
    {
        Btn02.SetActive(true);
    }

}
