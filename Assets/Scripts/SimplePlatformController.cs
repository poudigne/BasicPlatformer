using UnityEngine;
using System.Collections;

public class SimplePlatformController : MonoBehaviour {

  [HideInInspector] public bool facingRight = true;
  [HideInInspector] public bool jump = false;
  public float moveForce = 365f;
  public float maxSpeed = 5f;
  public float jumpForce = 1000f;
  public Transform groundCheck;

  private bool grounded = false;
  private Animator anim;
  private Rigidbody2D rb2d;

	// Use this for initialization
	void Awake() {
    anim = GetComponent<Animator>();
    rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame : Framerate dependant.
	void Update () {
    grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

    if (Input.GetButtonDown("Jump") && grounded)
    {
      jump = true;
    }
	}

  // Update is called for physics : Framerate independant
  void FixedUpdate()
  {
    float h = Input.GetAxis("Horizontal");
    // h = -1 , 1 depending on direction so we want a value betweer 0 and 1 whatever direction we're going
    anim.SetFloat("Speed", Mathf.Abs(h));
    if (h * rb2d.velocity.x < maxSpeed)
    {
      rb2d.AddForce(Vector2.right * h * moveForce);
    }
    // clamping speed to MaxSpeed if speed is increasing too much
    if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
    {
      rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
    }
    // if we're trying to move left, then we flip the sprite
    if (h > 0 && !facingRight)
      Flip();
      // if we're trying to move right, then we flip the sprite
    else if (h < 0 && facingRight)
      Flip();

    if (jump)
    {
      anim.SetTrigger("Jump");
      rb2d.AddForce(new Vector2(0f, jumpForce));
      jump = false;
    }
  }

  /// <summary>
  /// Flip the sprite on the X-axis
  /// </summary>
  void Flip()
  {
    facingRight = !facingRight;
    Vector3 theScale = transform.localScale;
    theScale.x *= -1;
    transform.localScale = theScale;
  }
}
