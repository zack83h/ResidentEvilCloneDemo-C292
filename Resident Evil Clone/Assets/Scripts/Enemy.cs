using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float movespeed = 5f;
    [SerializeField] private float MaxHealth = 5f;

    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movespeed;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Zombie took damage: " + damage);
        currentHealth -= damage;
        if(currentHealth == 0)
        {
            Destroy(gameObject);
        }
    }
}
