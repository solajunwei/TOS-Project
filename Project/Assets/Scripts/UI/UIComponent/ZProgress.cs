using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ZProgress : UIComponent
{
    [Header("进度条设置")]
    [Tooltip("进度条填充图像")]
    [SerializeField] private Image fillImage;

    [Tooltip("显示百分比的文本")]
    [SerializeField] private Text percentageText;

    [Tooltip("进度变化时的平滑过度速度")]
    [SerializeField] private float smoothSpeed = 5.0f;

    // 当前进度（0-1）
    private float currentProgress;
    //目标进度（0-1）
    private float targetProgress;

    private void Awake()
    {
        if (fillImage == null)
        {
            fillImage = GetComponent<Image>();
        }
        currentProgress = 0;
        targetProgress = 0;
        UpdateProgressUI();
    }


    private void Update()
    {
        if(currentProgress != targetProgress)
        {
            currentProgress = Mathf.MoveTowards(currentProgress, targetProgress, smoothSpeed*Time.deltaTime);
            UpdateProgressUI();
        }
    }


    /// <summary>
    /// 更新进度条UI
    /// </summary>
    private void UpdateProgressUI()
    {
        fillImage.fillAmount = currentProgress;
        if (percentageText != null)
        {
            percentageText.text = $"{Mathf.RoundToInt(currentProgress*100)}%";
        }
    }

    /// <summary>
    /// 设置目标进度（0-1）
    /// </summary>
    /// <param name="progress"></param>
    public void SetProgress(float progress)
    {
        targetProgress = Mathf.Clamp01(progress);
    }
    
    

}
