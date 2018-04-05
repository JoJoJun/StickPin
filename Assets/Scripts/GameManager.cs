using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

    private Transform startPoint;//起始位置
    private Transform spawnPoint;//屏幕外实例化针的位置
    public GameObject pinPrefab;//整个针
    private Pin currentPin;//当前针
    private bool isGameOver = false;//是否游戏结束
    private int score = 0;//分数
    public Text scoreText;//显示分数的组件
    private Camera mainCamera;//获得主场景的相机
    public float animationSpeed=3 ;//gmeover动画速度
    // Use this for initialization
    void Start()
    {
        //获取具体位置
        startPoint = GameObject.Find("StartPosition").transform;
        spawnPoint = GameObject.Find("SpawnPoint").transform;
        mainCamera = Camera.main;
        SpawnPin();
        animationSpeed = 3;
    }
    //生成针：在spawnpoint位置实例化
    void SpawnPin()
    {
        currentPin= GameObject.Instantiate(pinPrefab, spawnPoint.position, pinPrefab.transform.rotation).GetComponent<Pin>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;//不响应鼠标点击了
        //鼠标点击事件
        if(Input.GetMouseButtonDown(0))
        {
            currentPin.StartFly();//从起始位置向上
            score++;
            scoreText.text = score.ToString();
            SpawnPin();//重新实例化针
        }
    }
    public void GameOver()
    {
        if (isGameOver) return;
        
        GameObject.Find("Circle").GetComponent<RotateSelf>().enabled = false;//停止旋转
        StartCoroutine(GameOverAnimation());//游戏结束动画


        isGameOver = true;
    }
    /// <summary>
    /// unity协程是一个能暂停执行，暂停后立即返回，直到中断指令完成后继续执行的函数。

   /// 它类似一个子线程单独出来处理一些问题，性能开销较小，但是他在一个MonoBehaviour提供的主线程里只能有一个处于运行状态的协程。
    /// </summary>
    /// <returns></returns>
    IEnumerator GameOverAnimation()
    {
        while(true)
        {
            
            //lerp()进行差值计算。背景色和大小都线性变化
            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, Color.red, animationSpeed* Time.deltaTime);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 4, animationSpeed * Time.deltaTime);
            Debug.Log(animationSpeed);
            if (Mathf.Abs(mainCamera.orthographicSize - 4) < 0.01f) break;
            yield return 0;//每次暂停一帧
        }
        yield return new WaitForSeconds(0.2f);//动画暂停0.5秒
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//重新加载场景
    }
}
