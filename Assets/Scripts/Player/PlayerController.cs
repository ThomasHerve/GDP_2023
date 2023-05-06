using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Parameters
    [Header("Parametres")]
    [SerializeField]
    float speed;
    [SerializeField]
    float acceleration;

    // Velocity management
    private Vector3 targetVelocity;
    private Vector3 currentVelocity;

    // Keyboard specific
    Plane m_Plane;
    Vector3 mousePosition;

    // Components
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        m_Plane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.deltaTime * acceleration);
        rigidbody.velocity = currentVelocity;
        if(mousePosition != null)
        {
            ComputeRotateMouse();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 newVelocity = context.ReadValue<Vector2>();
        targetVelocity = new Vector3(newVelocity.x, 0, newVelocity.y) * speed;
    }
    
    public void Rotate(InputAction.CallbackContext context)
    {
        Vector3 v = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0);
        
        if (v != Vector3.zero)
        {
            if (context.control.device.name.Contains("Mouse"))
            {
                mousePosition = v;
            }
            else
            {
                transform.forward = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
            }
        }

    }

    private void ComputeRotateMouse()
    {
        if (mousePosition != Vector3.zero)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            //Initialise the enter variable
            float enter = 0.0f;

            if (m_Plane.Raycast(ray, out enter))
            {
                //Get the point that is clicked
                Vector3 v = mousePosition;
                v = ray.GetPoint(enter);
                /*
                float angle = Vector3.SignedAngle((v - transform.position).normalized, new Vector3(0, 0, 1), Vector3.up) * -1;
                transform.eulerAngles = new Vector3(0, angle, 0);
                */
                transform.LookAt(new Vector3(v.x, transform.position.y, v.z));
            }
        }
    }

    
}