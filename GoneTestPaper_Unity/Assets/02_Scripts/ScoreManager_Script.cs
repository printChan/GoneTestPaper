using UnityEngine;

public class ScoreManager_Script : MonoBehaviour
{
    // 어디서나 접근할 수 있는 정적(static) 통로 개설
    public static ScoreManager_Script Instance { get; private set; }

    public int score = 0;

    private void Awake()
    {
        // 씬이 시작될 때 자기 자신을 Instance에 등록
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
