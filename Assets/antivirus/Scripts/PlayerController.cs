using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public bool disabled = false;
	public float speed = 1.0f;
	public float rotationSpeed = 1.0f;
	public float force = 3.0f;

	public Transform minBound;
	public Transform maxBound;
	public EnemySpawner enemySpawner;

	GameObject hasComponent;

	Animator anim;
	bool moving = false;

	void Awake()
	{
		anim = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update () {
		if ( disabled )
			return;
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		translation *= Time.deltaTime;
		if ( translation == 0f )
		{
			if ( moving )
			{
				anim.SetBool("Run", false);
				moving = false;
			}
		} else {
			if ( !moving )
			{
				anim.SetBool("Run", true);
				moving = true;
			}
		}
		transform.Translate(0, 0, translation);
		rotation *= Time.deltaTime;
		transform.Rotate(0, rotation, 0);
		checkBounds();
	}

	void OnCollisionEnter(Collision collision)
	{
		if ( disabled )
			return;
		GameObject other = collision.collider.gameObject;
		//Debug.Log("Player collision with " + other.name + " (" + other.tag + ")" );
		if ( other.CompareTag("Enemy") )
		{
			//Debug.Log("Destroying enemy");
			enemySpawner.RemoveEnemy(other);
		} else if ( other.CompareTag("Component"))
		{
			CircuitComponent cc = other.GetComponent<CircuitComponent>();
			if (!hasComponent && !cc.isInSocket)
			{
				Debug.Log("Picking up component");
				collision.collider.enabled = false;
				other.transform.SetParent(transform);
				other.transform.localRotation = Quaternion.identity;
				other.transform.position = transform.position + transform.forward;
				hasComponent = other;
			}
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if ( disabled )
			return;
		Debug.Log("Player trigger from " + other.tag);
		if ( other.CompareTag("ComponentSocket") && hasComponent != null )
		{
			DropComponent(other.gameObject);
		}
	}

	public void DropComponent(GameObject socket)
	{
		socket.GetComponent<Collider>().enabled = false;
		hasComponent.transform.SetParent(null);
		hasComponent.transform.position = socket.transform.position;
		hasComponent.transform.rotation = socket.transform.rotation;
		CircuitComponent cc = hasComponent.GetComponent<CircuitComponent>();
		cc.isInSocket = true;
		cc.enableCircuit();
		cc.highlightOff();
		hasComponent.GetComponent<Collider>().enabled = true;
		hasComponent = null;
		socket.SetActive(false);
		enemySpawner.ReadyNextComponent();
	}

	void checkBounds()
	{
		if (transform.position.x < minBound.position.x )
		{
			transform.position = new Vector3(minBound.position.x, transform.position.y, transform.position.z);
		}
		if (transform.position.z < minBound.position.z )
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, minBound.position.z);
		}
		if (transform.position.x > maxBound.position.x )
		{
			transform.position = new Vector3(maxBound.position.x, transform.position.y, transform.position.z);
		}
		if (transform.position.z > maxBound.position.z )
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, maxBound.position.z);
		}
	}
}
