using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 通用过渡脚本 V1.0
 * 用途：用于场景过渡，包括场景切换、场景加载、场景卸载等
 * 作者：LX
 * 创建时间：2025-03-19
 * 最新修改时间：2025-03-19
 * 描述：包含，单例模式，可全局调用
 */

namespace Manager
 {
    public class TransitionManager : SingletonMonobehaviour<TransitionManager>
    {
        
        private CanvasGroup _canvasGroup;  // 画布组件 用于实现淡入淡出效果
        
        [Header("淡入淡出速度")]
        public float _scaler = 2;

        protected override void Awake()
        {  
            base.Awake();
            _canvasGroup = GetComponentInChildren<CanvasGroup>();
            _canvasGroup.alpha = 1; // 初始化为1，即不透明
        }
        
        private void Start()
        {
            // 首次启动时，淡出画布
            StartCoroutine(Fade(0));
            // 防止切换场景时，画布被销毁
            DontDestroyOnLoad(this);
        }
        
        /// <summary>
        /// 淡入淡出效果
        /// </summary>
        /// <param name="targetAlpha">目标值</param>
        /// <returns></returns>
        private IEnumerator Fade(int targetAlpha)
        {
            _canvasGroup.blocksRaycasts = true;
            while(!Mathf.Approximately(_canvasGroup.alpha, targetAlpha))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, targetAlpha, Time.deltaTime * _scaler);
                yield return null;
            }
            _canvasGroup.blocksRaycasts = false;
        }
        
        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void Transition(string sceneName)
        {
            StartCoroutine(TransitionToScene(sceneName));
        }
        
        /// <summary>
        /// 切换场景时的过渡效果
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private IEnumerator TransitionToScene(string sceneName)
        {
            // 淡入
            yield return StartCoroutine(Fade(1));
            // 加载场景
            yield return SceneManager.LoadSceneAsync(sceneName);
            // 淡出
            yield return StartCoroutine(Fade(0));
        }
    }
}
