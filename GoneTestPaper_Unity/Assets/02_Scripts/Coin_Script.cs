using UnityEngine;

public class Coin_Script : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // new를 쓰지 않고, static으로 열려있는 Instance에 바로 접근합니다.
            ScoreManager_Script.Instance.score += 1; 

            Destroy(gameObject); // 코인 삭제
        }
    }
}
