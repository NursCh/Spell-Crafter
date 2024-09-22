using UnityEngine;

public class StickmanMovement : MonoBehaviour
{
    public float moveSpeed = -5f; 
    public float rotationSpeed = 75f; 
    public float movementDistance = 7f; 
    public float rotationAngle = 30f; 
    private bool rotateRight = true; 
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isMoving = false;
    private bool returnToStart = false;
    private float distanceMoved = 0f;
    private float currentAngle = 0f;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (isMoving)
        {
            MoveAndRotate();
        }
        else if (returnToStart)
        {
            ReturnToOriginalAngle();
        }
    }
    
    public void StartMovement(int distance)
    {
        movementDistance = distance;
        isMoving = true;
        distanceMoved = 0f; 
    }
    
    void MoveAndRotate()
    {
        if (rotateRight)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        else
        {
            
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
        
        if (transform.rotation.eulerAngles.z >= rotationAngle && rotateRight)
        {
            rotateRight = false;  
        }
        else if (transform.rotation.eulerAngles.z >= -rotationAngle+360 && !rotateRight)  
        {
            rotateRight = true;   
        }
        if (distanceMoved < movementDistance)
        {
            float moveStep = moveSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x + moveStep, transform.position.y,0);
            distanceMoved -= moveStep;
        }
        else
        {
            isMoving = false;
            returnToStart = true;
        }
    }
    
    void ReturnToOriginalAngle()
    {
        if (currentAngle > 0)
        {

            currentAngle -= rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
        else
        {
 
            returnToStart = false;
            transform.rotation = originalRotation; 
        }
    }
}
