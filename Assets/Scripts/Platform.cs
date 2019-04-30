using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Slime"))
        {
            GameManager.instance.FreeSlime(collision.gameObject);
            collision.gameObject.GetComponent<SlimeAI>().underControl = false;
            transform.GetComponentInChildren<Animator>().SetBool("IsClosed", true);
            transform.GetChild(0).GetComponent<BoxCollider2D>().isTrigger = false;
            transform.parent.GetComponent<Gate>().Open();
            transform.parent = transform.parent.parent;            
        }
        else if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit player!");
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().animator.SetBool("IsAlive", false);
            GameManager.instance.Invoke("StartGame", 1f / 3f);
        }
    }
}
