using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    private SpriteRenderer spriteRenderer;
	
	void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}	
	
	void Update ()
    {
        FollowMouse();
	}

    private void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //transform.position = new Vector3(mousePos.x, mousePos.y, 0);

            //Vector3 mousePos = Input.mousePosition;
            //transform.position = new Vector3(mousePos.x, mousePos.y, 0);

        }        
    }

    public void Activate(Sprite sprite)
    {
        this.spriteRenderer.enabled = true;
        this.spriteRenderer.sprite = sprite;
    }

    public void Deactivate()
    {
        this.spriteRenderer.enabled = false;
        GameManager.Instance.ClickedButton = null;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
