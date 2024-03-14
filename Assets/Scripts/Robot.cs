using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Robot : MonoBehaviour
{
    private InputAction exitAction;
    [SerializeField] private TextMeshProUGUI ridingMessage;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject ridingSystem;

    // Start is called before the first frame update
    void OnEnable()
    {
        exitAction=new InputAction("Exit",binding:"<Keyboard>/E");
        exitAction.Enable();

        ridingMessage.text = "Press E to exit the robot";
    }

    // Update is called once per frame
    void Update()
    {
        if(exitAction.triggered)
        {
            Exit();
        }
    }
    private void Exit()
    {
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
        player.gameObject.SetActive(true);
        gameObject.SetActive(false);
        ridingSystem.SetActive(true );
        ridingSystem.transform.position=player.transform.position;
    }
}
