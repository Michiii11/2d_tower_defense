using UnityEngine;

public class Animal : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 0.2f;
    public float fireRate = 0.5f;
    public int damage = 10;
    public float range = 2f;
    public float bulletDieTime = 0.3f;
    public int price = 100;

    void Start(){
        InvokeRepeating(nameof(attack), 0f, fireRate);
    }

    void Update(){
        
    }

    private void attack(){
        if (GameState.Instance.playMode == PlayMode.PLAY){
            GameObject closestTarget = FindClosestTarget();
            if (closestTarget != null){
                Debug.Log("Found target: " + closestTarget);
                ShootAtTarget(closestTarget);
                closestTarget.GetComponent<Enemy>().DealDamage(damage);
            }
        }
    }

    GameObject FindClosestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = target;
            }
        }

        if(shortestDistance <= range){
            return closest;
        }
        else{
            return null;
        }
    }

    void ShootAtTarget(GameObject target){
        Debug.Log("Shooting at target: " + target);

        // Instantiate the bullet at the spawn point
        GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);

        // Calculate the direction to the target
        Vector3 direction = target.transform.position - transform.position;
    
        // Normalize the direction vector
        direction.Normalize();

        // Set the bullet's velocity
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;

        Destroy(bullet, bulletDieTime);

        // Rotate the bullet towards the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
