using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject cam;
    [SerializeField] private float parallaxEffect;
    private float xPosition;
    private float yPosition;
    private float length;
    private float high;
    void Start()
    {
        cam = GameObject.Find("Main Camera");


        length = GetComponent<SpriteRenderer>().bounds.size.x;
        high = GetComponent<SpriteRenderer>().bounds.size.y;
        xPosition = transform.position.x;
        yPosition=transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToMoveX = cam.transform.position.x * parallaxEffect;
        float distanceMoveX = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMoveY = cam.transform.position.y * parallaxEffect;
        float distanceMoveY = cam.transform.position.y * (1 - parallaxEffect);

        transform.position = new Vector3(xPosition + distanceToMoveX, yPosition+ distanceToMoveY);
        if(distanceMoveX>xPosition+length)
        { xPosition=xPosition+length; }
        else if(distanceMoveX<xPosition-length) 
        { xPosition=xPosition-length; }
        if (distanceMoveY > yPosition + high)
        { yPosition = yPosition + high; }
        else if (distanceMoveY < yPosition - high)
        { yPosition = yPosition - high; }
    }
}
