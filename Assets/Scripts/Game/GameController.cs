using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public new CameraController camera; // main game camera
	public HUD hud; // game hud to display health and ammo
    public Transition transition; // transition to use when generate a new floor
	public EnemyManager enemyManager; // script which manages enemy spawns
	public TerrainManager terrain; // game terrain
	public PlayerController player; // player game object
	public GameObject boss; // boss game object
    public AudioClip mainTheme; // main soundtrack
    public AudioClip bossTheme; // bossfight soundtrack
    public AudioClip victory; // victory sound effect
    public AudioClip defeat; // defeat sound effect
    public int numberOfFloors; // number of floors to traverse to win the game
    public List<GameObject> powerUps; // items to find in chests
	public IntRange itemCost; // cost of items in the shop

    private AudioSource soundtrack; // object that handles game music
    private Room currentRoom; // room that the player is currently in
    private float fadeSpeed = 0.001f; // speed with which music fades in/out
    private bool endGame; // flag for end game state

    // Whether or not the player is in a room.
    public bool playerInRoom {
		get {
			return currentRoom != null;
		}
	}

	void Start() {
        soundtrack = GameObject.FindWithTag("Soundtrack").GetComponent<AudioSource>();
        CreateNewFloor();
	}

	void FixedUpdate() {
        if (player != null) {
            UpdatePlayerRoom();
        }
	}

	// Create a new floor.
	public void CreateNewFloor() {
		StartCoroutine(CreateNewFloorHelper());
	}

	public void OnPlayerDeath() {
		StartCoroutine(GameOver());
	}

	private IEnumerator CreateNewFloorHelper() {
		transition.FadeIn();
		yield return new WaitForSeconds(transition.duration);
		--numberOfFloors;
		terrain.GenerateFloor();
		CenterPlayerOnFloor();
		hud.InitializeCompass();
		enemyManager.Initialize();
		transition.FadeOut();
	}

	// Center the player on the floor, putting him in the spawn room.
	//
	// The player is put at a certain elevation so that it looks like the player falls from the
	// previous floor.
	private void CenterPlayerOnFloor(float elevation=3.0f) {
		var centerPosition = terrain.floorConfiguration.FindCenterPosition();
		var worldCenterPosition = LayoutGrid.ToWorldPosition(centerPosition, true);
		player.transform.position = worldCenterPosition + Vector3.up * elevation;
		camera.CenterOnPlayer();
	}

	// Find the room that the player is in, if any, and perform actions upon room entry.
	private void UpdatePlayerRoom() {
		var playerGridPosition = LayoutGrid.FromWorldPosition(player.transform.position);
		currentRoom = terrain.FindRoomAtPosition(playerGridPosition);
		if (currentRoom != null) {
			if (!currentRoom.hasBeenEntered) {
				currentRoom.hasBeenEntered = true;

				// action triggered upon room entry
				if (currentRoom.type == RoomType.Enemy) {
					enemyManager.SpawnEnemiesInRoom(currentRoom);
				} else if (currentRoom.type == RoomType.Boss) {
					StartCoroutine(BossFight());
				}
			}
		}
	}

    //toggle between soundtracks at beginning and end of boss encounters
    private IEnumerator toggleSoundtrack()
    {
        float volume = soundtrack.volume;

        while (soundtrack.volume > 0)
        {
            soundtrack.volume -= fadeSpeed;
            if (soundtrack.volume == 0)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }

        if (soundtrack.clip == mainTheme)
        {
            soundtrack.clip = bossTheme;
        }
        else
        {
            soundtrack.clip = mainTheme;
        }
        soundtrack.Play();

        while (soundtrack.volume < volume)
        {
            soundtrack.volume += fadeSpeed;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator toggleEndGame(bool endGame)
    {
        float volume = soundtrack.volume;

        while (soundtrack.volume > 0)
        {
            soundtrack.volume -= fadeSpeed;
            if (soundtrack.volume == 0)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }

        if (endGame)
        {
            soundtrack.clip = victory;
        }
        else
        {
            soundtrack.clip = defeat;
        }
        soundtrack.loop = false;
        soundtrack.Play();

        while (soundtrack.volume < volume)
        {
            soundtrack.volume += fadeSpeed;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Victory() {
        endGame = true;
        StartCoroutine(toggleEndGame(endGame));
        yield return new WaitForSeconds(2.0f);
        hud.Victory();
        yield return new WaitForSeconds(4.0f);
        transition.FadeIn();
		
		// insert victory screen here

		yield return new WaitForSeconds(transition.duration);
		SceneManager.LoadScene("Menu");
	}

	private IEnumerator GameOver() {
        endGame = false;
        StartCoroutine(toggleEndGame(endGame));
        yield return new WaitForSeconds(2.0f);
        hud.GameOver();
        yield return new WaitForSeconds(4.0f);
        transition.FadeIn();
		
		// insert game over screen here

		yield return new WaitForSeconds(transition.duration);
		SceneManager.LoadScene("Menu");
	}

	// Start the boss fight and create a passage to the next floor upon beating it.
	private IEnumerator BossFight() {
		terrain.BlockRoomEntrances(currentRoom);
		enemyManager.Clear();
		boss = enemyManager.SpawnBoss(currentRoom);
		hud.DisplayBossHealth();
        StartCoroutine(toggleSoundtrack());
        yield return new WaitUntil(() => boss == null);
		hud.HideBossHealth();
        StartCoroutine(toggleSoundtrack());
        yield return new WaitForSeconds(0.25f);
		if (numberOfFloors > 0) {
			var centerPosition = currentRoom.centerPosition;
			var effectPosition = LayoutGrid.ToWorldPosition(centerPosition + new Vector2Int(0, 1), true);
            AudioSource.PlayClipAtPoint(terrain.breakSoundEffect, effectPosition, terrain.soundVolume);
            Instantiate(terrain.breakEffect, effectPosition, Quaternion.identity);
			terrain.PlaceProp(terrain.terrainProps.chest, centerPosition + new Vector2Int(0, 1));
			
			yield return new WaitForSeconds(3.0f);
			effectPosition = LayoutGrid.ToWorldPosition(centerPosition - new Vector2Int(0, 1), true);
            AudioSource.PlayClipAtPoint(terrain.breakSoundEffect, effectPosition, terrain.soundVolume);
            Instantiate(terrain.breakEffect, effectPosition, Quaternion.identity);
			terrain.Place(terrain.terrainBlocks.passage, centerPosition - new Vector2Int(0, 1));
		} else {
			StartCoroutine(Victory());
		}
	}
}
