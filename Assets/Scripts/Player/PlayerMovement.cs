using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController kontroler;

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    //  provera da li je na zemlju uz pomoc f-je Phisics
    //  groundMask je ground, groundDistance je razdaljina izmedju plejera i grounda
    //  groundCheck se nalazi na dnu karaktera i sluzi kao provera
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    //  velocity = ubrzanje
    Vector3 velocity;

    //  2 bool vrednosti za proveru da li se karakter krece i da li je na zemlji
    bool isGrounded;
    bool isMoving;

    //  Vector3 - vektor u 3d prostoru, lastPosition cemo koristiti za pamcenje pozicije igraca
    //  na pocetku je 0 0 0
    private Vector3 lastPosition = new Vector3(0f,0f,0f);

    void Start()
    {
        //  inicijalizaci promenljive kontroler
        //  kada iskoristirimo u startu, kontroler sada ima referencu na CharacterController
        kontroler = GetComponent<CharacterController>();
    }
    void Update()
    {
        //  pravljenje funkcije isGrounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        //  kontrola ponasanja vertikalne brzine igraca dok se nalazi na zemlji
        //  ako je na zemlji && ako je vertikalna brzina manja od 0(ako je igrac u padu)
        if(isGrounded && velocity.y < 0)
        {
            //  resetujemo ubrzanje na -2f kada dotakne tlo 
            velocity.y = -2f;
        }

        // uzimanje inputa sa tastature (horizontal za horizontalni(levo - negativno, desno - pozitivno),
        // (gore - pozitivno, dole - negativno))
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //  pravljenje move vektora:
        Vector3 move = transform.right * x + transform.forward * z;
        //  (right - crvena koordinata, foward - plava koordinata)

        //  pomeranje karaktera
        kontroler.Move(move * speed * Time.deltaTime);
        //  Time.deltaTime - vreme proteklu izmedju dva frejma, kretanje se vrsi nezavisno od brzine rac sistema

        //  padanje:
        velocity.y += gravity * Time.deltaTime;
        //  skakanje:
        kontroler.Move(velocity * Time.deltaTime);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //  izracunavanje brzine skoka
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
       

        if(lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //  uzimanje trenutne pozicije igraca
        lastPosition = gameObject.transform.position;
    }
}
