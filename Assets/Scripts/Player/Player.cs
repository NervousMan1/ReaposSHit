using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private Transform _attackPos;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _enemy;
    [SerializeField] private int _damage;
    [SerializeField] private GrapplingRope _grapplingRope;

    [SerializeField] private PlayerController controller;
    [SerializeField] private float runSpeed = 40f;

    [SerializeField] private float timeBetweenAttack;
    private float timeCount;

    private float horizontalMove = 0f;
    private bool canChangeDirection = true;

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (Input.GetAxisRaw("Horizontal") > 0 && canChangeDirection)
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && canChangeDirection)
        {
            gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (horizontalMove != 0 && !_grapplingRope.enabled)
        {
            _anim.SetBool("isRunning", true);
        }
        else
        {
            _anim.SetBool("isRunning", false);
        }

        if (Input.GetMouseButtonDown(0) && timeCount <= 0)
        {
            _anim.SetTrigger("Attack");
            timeCount = timeBetweenAttack;
        }
        else
        {
            timeCount -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime);
    }


    public void Attack() // ���������� � ��������
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_attackPos.position, _attackRange, _enemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Health>().TakeDamage(_damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_attackPos.position, _attackRange);
    }
}
