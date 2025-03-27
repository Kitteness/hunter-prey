using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;
    private Vector3 target;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject goal;
    [SerializeField] private int aggroRange;
    [SerializeField] private GameObject uiMessage;
    [SerializeField] private TextMeshProUGUI uiMessageText;
    private LifeManager lifeManager;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lifeManager = player.GetComponent<LifeManager>();
        UpdateDestination();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 1)
        {
            RespawnPlayer();
        }
        else if (Vector3.Distance(goal.transform.position, player.transform.position) < 1)
        {
            GoalReached();
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < aggroRange)
        {
            ChasePlayer();
        }
        else if (Vector3.Distance(transform.position, target) < 1)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }
    }

    private void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    private void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    private void RespawnPlayer()
    {
        uiMessageText.text = $"Captured!";
        lifeManager.LoseLife();
        if (lifeManager.totalLives > 0)
        {
            StartCoroutine(RestartScene());
        }
        else
        {
            StartCoroutine(GameOver());
        }
    }

    private void GoalReached()
    {
        uiMessageText.text = $"Success!";
        StartCoroutine(DisplayText());
    }

    IEnumerator DisplayText()
    {
        uiMessage.SetActive(true);

        yield return new WaitForSeconds(2);

        uiMessage.SetActive(false);
    }

    IEnumerator GameOver()
    {
        uiMessage.SetActive(true);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Game Over");
    }

    IEnumerator RestartScene()
    {
        uiMessage.SetActive(true);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
