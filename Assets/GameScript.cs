using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets._2D;

public class GameScript : MonoBehaviour
{
    private LoginController loginController;

    private string login;

    public GameObject firstHeart;
    public GameObject secondHeart;
    public GameObject thirdHeart;
    public GameObject gameOverMessage;
    public GameObject characterRobotBoy;
    public GameObject blackKnight;
    public Slider slider;
    public Text coinsDisplay;
    public InputField loginInput;
    public InputField passwordInput;
    public Text message;

    private int health;
    private int coins;
    private List<string> collectedCoinsNames;

    private bool hitProtection;

    private void Start()
    {
        loginController = new LoginController();
        login = PlayerPrefs.GetString("currentPlayer", "");
        health = PlayerPrefs.GetInt(login + "Health", 3);
        coins = PlayerPrefs.GetInt(login + "Coins", 0);
        if (!SceneManager.GetActiveScene().name.Equals("LoginScene"))
        {
            coinsDisplay.text = coins.ToString();
            int i = health;
            health = 3;
            while (health > i)
            {
                ReduceHealth();
            }
        }
        hitProtection = false;
    }

    private void Update()
    {
        if (!SceneManager.GetActiveScene().name.Equals("LoginScene"))
        {
            if (characterRobotBoy.transform.position.y < -20)
            {
                characterRobotBoy.transform.position = new Vector3(0, 1, 0);
            }
        }
    }

    private void ReduceHealth()
    {
        health--;
        switch (health)
        {
            case 2:
                thirdHeart.SetActive(false);
                break;
            case 1:
                secondHeart.SetActive(false);
                break;
            case 0:
                firstHeart.SetActive(false);
                GameOver();
                break;
            default:
                break;
        }
    }

    private void GameOver()
    {
        characterRobotBoy.GetComponent<Platformer2DUserControl>().enabled = false;
        gameOverMessage.SetActive(true);
        PlayerPrefs.DeleteKey(login + "Level");
        PlayerPrefs.DeleteKey(login + "Health");
        PlayerPrefs.DeleteKey(login + "Coins");
    }

    public void PlayAgain()
    {
        gameOverMessage.SetActive(false);
        characterRobotBoy.GetComponent<Platformer2DUserControl>().enabled = true;
        SceneManager.LoadScene("FirstScene");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Hazard":
                if (!hitProtection)
                {
                    ReduceHealth();
                    StartCoroutine(ActivateProtection());
                }
                break;
            case "Coin":
                IncraseCoins();
                collision.gameObject.SetActive(false);
                break;
            case "FirstPortal":
                PlayerPrefs.SetString(login + "Level", "SecondScene");
                PlayerPrefs.SetInt(login + "Health", health);
                PlayerPrefs.SetInt(login + "Coins", coins);
                SceneManager.LoadScene("SecondScene");
                break;
            case "BossFight":
                blackKnight.GetComponent<BlackKnightController>().enabled = true;
                collision.gameObject.tag = "Untagged";
                break;
            default:
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Platform") && characterRobotBoy.GetComponent<PlatformerCharacter2D>().IsGrounded())
        {
            characterRobotBoy.transform.parent = collision.gameObject.transform;
        }
    }

    private void IncraseCoins()
    {
        coins++;
        coinsDisplay.text = coins.ToString();
        if (coins == 6)
        {
            health = 3;
            secondHeart.SetActive(true);
            thirdHeart.SetActive(true);
            characterRobotBoy.GetComponent<PlatformerCharacter2D>().PowerUp();
            characterRobotBoy.GetComponent<AudioSource>().Play();
            slider.gameObject.SetActive(true);
        }
    }

    private IEnumerator ActivateProtection()
    {
        hitProtection = true;
        yield return new WaitForSeconds(1);
        hitProtection = false;
    }

    public void ChangeVolume()
    {
        characterRobotBoy.GetComponent<AudioSource>().volume = slider.value;
    }

    public void loginButtonOnClick()
    {
        string login = loginInput.text;
        if (loginController.login(login, passwordInput.text))
        {
            PlayerPrefs.SetString("currentPlayer", login);
            SceneManager.LoadScene(PlayerPrefs.GetString(login + "Level", "FirstScene"));
        }
        else
        {
            message.text = "Invalid login or password.";
            message.color = Color.red;
        }
    }

    public void registerButtonOnClick()
    {
        string result = loginController.register(loginInput.text, passwordInput.text);
        message.text = result;
        if (result.Equals("Player has been registered."))
        {
            message.color = Color.green;
        }
        else
        {
            message.color = Color.red;
        }
    }
}
