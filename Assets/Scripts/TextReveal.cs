using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextReveal : MonoBehaviour
{
    GameObject text;
    public bool isNPC = false;
    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(0).gameObject;
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            text.SetActive(true);
            if (isNPC)
                GameManager.instance.Invoke("EndGame", 2);
        }
    }

     private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
            text.SetActive(false);
    }
}

