using UnityEngine;
using System.Collections;

public class tazo_UVScroller : MonoBehaviour {

public int targetMaterialSlot=0;
		private Renderer myrender;
public float speedY=0.5f;
public float speedX=0.0f;
private float timeWentX=0;
private float timeWentY=0;

void Start (){
				
		myrender = GetComponent<Renderer>();
}

void Update (){
		timeWentY += Time.deltaTime*speedY;
		timeWentX += Time.deltaTime*speedX;
		myrender.material.SetTextureOffset("_MainTex",new Vector2(timeWentX, timeWentY));
}

//void OnEnable (){
//
//		myrender.material.SetTextureOffset ("_MainTex",new Vector2(0, 0));
//		timeWentX = 0;
//		timeWentY = 0;
//}
}