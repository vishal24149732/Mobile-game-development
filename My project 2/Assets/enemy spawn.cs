using UnityEngine;

public class enemyspawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public BoxCollider spawnArea;
    private float timer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2f) {

            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector3 randomPoint = new Vector3(Random.Range(-spawnArea.size.x / 2, spawnArea.size.x / 2), 0f, 0f);

        Vector3 spawnPosition = spawnArea.transform.TransformPoint(
            randomPoint + spawnArea.center
        );

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, enemyPrefab.transform.rotation);

        enemy.GetComponent<Enemy>().StartMoving();
    }
}
