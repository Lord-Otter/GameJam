using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : MonoBehaviour
{
    public SpriteRenderer myRenderer;

    float activationdistance = 2f;
    public GameObject Enemy;
    public GameObject Player;
    // Start is called before the first frame update
    void Start() 
    {
        myRenderer.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if ((Enemy.transform.position - Player.transform.position).magnitude < activationdistance)
        {
            myRenderer.enabled = true;
        }
        else
        {
            myRenderer.enabled = false;
        }
    }
}
