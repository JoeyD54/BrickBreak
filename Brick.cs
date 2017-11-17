using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	public GameObject smoke;
	public Sprite[] hitSprites;
	public AudioClip punch;
	public AudioClip brickBreak;
	private int hitCount;
	private LevelManager levelManager;
	public static int breakableCount = 0;
	private bool isBreakable;

	// Use this for initialization
	void Start () {
		isBreakable = (this.tag == "Breakable");
		//Keep track of breakable bricks
		if (isBreakable)
			breakableCount++;
		hitCount = 0;
		levelManager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//When hitting walls, play first sound, else play brick break sound.
	private void OnCollisionEnter2D(Collision2D col) {
		if(hitCount < hitSprites.Length + 1)
			AudioSource.PlayClipAtPoint(punch, transform.position);
		else
			AudioSource.PlayClipAtPoint(brickBreak, transform.position);
		if(isBreakable)
			HitHandler();
	}

	//This creates the smoke effects once a brick is destroyed. The smoke is the same color as the brick.
	void SmokeStart() {
		GameObject smokePuff = Instantiate(smoke, transform.position, Quaternion.identity) as GameObject;
		smokePuff.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
	}
	
	//This keeps track of how many times a brick has been hit and will destroy it when the hits count reaches 0.
	void HitHandler() {
		hitCount++;
		int maxHits = hitSprites.Length + 1;
		if (hitCount >= maxHits){
			breakableCount--;               //Decrement local count of breakable bricks.
			levelManager.BrickDestroyed();  //Tell game to decrement brick count by 1
			SmokeStart();                   //Start smoke FX
			Destroy(gameObject);            //Remove the brick! 
		}
		else
			LoadSprites();                  //Load in next damaged brick sprite. 	

	//Swaps brick sprites with more damaged states with each hit.
	void LoadSprites() {
		int spriteIndex = hitCount - 1;
		if(hitSprites[spriteIndex])
			GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
		else
			Debug.LogError("Brick sprite missing");
	}
}
