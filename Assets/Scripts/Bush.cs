using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    bool inRange = false;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(8, 10);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Collect") && inRange && transform.childCount > 0)
        {
            GameManager.instance.CollectBerry();
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            inRange = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            inRange = false;
    }
}
