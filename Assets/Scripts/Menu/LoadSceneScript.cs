using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneScript : MonoBehaviour {

    public Transition transition;
    public GameObject buttonReturn;
    public float delayBeforeNewScene = 1;
    public float openDoorsSpeed = 5;

    private Vector3 positionPage;
    private Vector3 positionButton;
    private Vector3 startingPosition;
    private Vector3 returnButtonPossition;

    private string sceneToLoad;
    private float timerTillNewScene;

    private GameObject optionsCanvis;
    private GameObject creditCanvis;
    private GameObject doorLeft;
    private GameObject doorRight;


    void Start()
    {
        sceneToLoad = null;
        positionPage = startingPosition = transform.position;
        returnButtonPossition = positionButton = buttonReturn.transform.position;
        creditCanvis = GameObject.Find("Credit");
        optionsCanvis = GameObject.Find("OptionButtons");
        doorLeft = GameObject.Find("DoorLeft");
        doorRight = GameObject.Find("DoorRight");
        transition.FadeOut();
    }

    void Update()
    {
        //change page
        ChangePage();
        goToScene();
    }

    private void goToScene()
    {
        if (sceneToLoad!= null)
        {
            animateDoors(doorLeft, 75);
            animateDoors(doorRight, -75);

            transition.FadeIn();
            if (Time.time > timerTillNewScene)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    private void animateDoors(GameObject door, float degree)
    {
        door.transform.rotation = Quaternion.Slerp(door.transform.rotation, Quaternion.Euler(0, degree, 0), Time.deltaTime * openDoorsSpeed);
    }

    public void LoadSceneNamed(string name)
    {
        if (name != null)
        {
            timerTillNewScene = Time.time + delayBeforeNewScene;
            sceneToLoad = name;
        }
    }

    public void EndGame()
    {
        //does not work in editor
        Application.Quit();
    }

    public void GoToCredits()
    {
        creditCanvis.SetActive(true);
        optionsCanvis.SetActive(false);
        positionPage = new Vector3(0, 500, 0) * Screen.height/600 + startingPosition;
        positionButton = new Vector3(0, 50, 0) * Screen.height/600 + returnButtonPossition;
    }

    public void GoToOptions()
    {
        creditCanvis.SetActive(false);
        optionsCanvis.SetActive(true);
        positionPage = new Vector3(0,500,0) * Screen.height/600 + startingPosition;
        positionButton = new Vector3(0, 50, 0) * Screen.height/600 + returnButtonPossition;
    }

    public void GoToMainPage()
    {
        positionPage = startingPosition;
        positionButton = returnButtonPossition;
    }

    
    private void ChangePage()
    {

            ScrollToNectPage(positionPage);
            ReturnButtonDisplay(positionButton);
    }

    private void ScrollToNectPage(Vector3 position)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, 15);
    }

    private void ReturnButtonDisplay(Vector3 position)
    {
        buttonReturn.transform.position = Vector3.MoveTowards(buttonReturn.transform.position, position, 5);
    }

}
