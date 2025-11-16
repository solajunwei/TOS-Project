
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class BattleUI : UIView, IPointerClickHandler
{
    public Image[] _iamges;
    public GameObject _prefab;
    private GameObject _movePrefab;

    // 轮数
    public Text _RoundText;
    // 金币
    public Text _PointText;
    // 血量
    public Text _BloodText;

    // 默认是第一轮
    private int _round = 1;

    // 血量默认20
    private int _blood = 20;

    // 金币默认为0
    private int _point = 0;

    // 是否已经点击了
    private bool _isClick = false;

    private Dictionary<int, int> _drawPetDic = new Dictionary<int, int>();

    public void Start()
    {
        int roundCount = BattleModel.Instance.getRoundCount();
        _RoundText.text = _round + "/" + roundCount;
        _PointText.text = _point.ToString();
        _BloodText.text = _blood.ToString();

        _movePrefab = null;

        EventManager.Instance.AddEventListener(MyConstants.jump_next_round, updateNextRound);
        EventManager.Instance.AddEventListener(MyConstants.home_attack, updateLeft);
        EventManager.Instance.AddEventListener<GameObject>(MyConstants.Enemy_deal, EnemyDeal);
        EventManager.Instance.AddEventListener<int>(MyConstants.unit_up, onUnitUp);
        EventManager.Instance.AddEventListener<int>(MyConstants.add_Point, onAddPoint);
    }
    public void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener(MyConstants.jump_next_round, updateNextRound);
        EventManager.Instance.RemoveEventListener(MyConstants.home_attack, updateLeft);
        EventManager.Instance.RemoveEventListener<GameObject>(MyConstants.Enemy_deal, EnemyDeal);
        EventManager.Instance.RemoveEventListener<int>(MyConstants.unit_up, onUnitUp);
        EventManager.Instance.RemoveEventListener<int>(MyConstants.add_Point, onAddPoint);
    }

    public void Update()
    {
        // 检测鼠标左键按下
        if (Input.GetMouseButtonDown(0))
        {
            GraphicRaycaster uiRaycaster = _iamges[0].canvas.GetComponent<GraphicRaycaster>();
            EventSystem eventSystem = EventSystem.current;

            // 创建射线检测数据（位置为当前鼠标屏幕坐标）
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = Input.mousePosition;

            // 存储所有命中的UI元素
            List<RaycastResult> hitResults = new List<RaycastResult>();
            uiRaycaster.Raycast(pointerData, hitResults);

            Dictionary<int, GameObject> mPlayerList = BattleModel.Instance.PlayerList;

            // 遍历命中结果，判断是否包含目标Image
            bool isHide = false;
            foreach (RaycastResult result in hitResults)
            {
                for (int i = 0; i < _iamges.Length; i++)
                {
                    if (result.gameObject == _iamges[i].gameObject)
                    {
                        Debug.Log("鼠标按下位置在目标Image中！");
                        _isClick = true;
                        break; // 找到后退出循环
                    }
                }

                if (!isHide)
                {
                    foreach (GameObject playerObj in mPlayerList.Values)
                    {
                        if (result.gameObject == playerObj)
                        {
                            isHide = true;
                            break; // 找到后退出循环
                        }
                    }
                }
            }

            // 没有找到
            if (!isHide)
            {
                Debug.Log("sssssss");

                

                //EventManager.Instance.EventTrigger(MyConstants.hide_unit_sell);
            }
        }

        // 鼠标移动
        if (Input.GetMouseButton(0) && _isClick)
        {
            OnMouseMoveSel();
        }

        // 鼠标抬起
        if (Input.GetMouseButtonUp(0) && _movePrefab)
        {
            EventManager.Instance.EventTrigger<Vector3>(MyConstants.create_unit, _movePrefab.transform.position);
            if (_movePrefab)
            {
                Destroy(_movePrefab);
                _movePrefab = null;
            }
            _isClick = false;
        }
    }

    private void OnMouseMoveSel()
    {
        Vector2 screenPos = Input.mousePosition;
        Canvas canvas = UIManager.Instance.UICanvas;
        RectTransform uiRoot = gameObject.GetComponent<RectTransform>();

        Vector2 localPos;
        // 将屏幕坐标转换为指定 RectTransform 的本地坐标
        bool success = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiRoot,          // 目标 UI 的 RectTransform
            screenPos,       // 屏幕坐标
            canvas.worldCamera, // Canvas 关联的相机（Overlay 模式可传 null）
            out localPos     // 输出的本地坐标
        );

        if (success)
        {
            // 应用坐标到 UI 元素（例如移动一个按钮到鼠标位置）
            if (_movePrefab == null)
            {
                _movePrefab = Instantiate(_prefab, gameObject.transform);
                _movePrefab.transform.localRotation = Quaternion.identity;
                _movePrefab.transform.localScale = Vector3.one;
            }
            _movePrefab.transform.localPosition = new Vector3(localPos.x, localPos.y, 0);
        }
    }

    // 改变轮数
    public void updateNextRound()
    {
        _round++;
        int roundCount = BattleModel.Instance.getRoundCount();
        _RoundText.text = _round + "/" + roundCount;
    }

    // 更改生命值
    public void updateLeft()
    {
        _blood--;
        _BloodText.text = _blood.ToString();
    }

    // 获取金币
    public void EnemyDeal(GameObject obj)
    {
        EnemyUnit enemyUnit = obj.GetComponent<EnemyUnit>();
        _point += enemyUnit.PointNum;
        _PointText.text = _point.ToString();
    }

    public void onAddPoint(int point)
    {
        _point += point;
        _PointText.text = _point.ToString();
    }

    // 抽卡
    public void Draw()
    {
        int index = getInstantiateIndex();
        if (index == -1)
        {
            Debug.LogError("格子已满，无法抽取");
            return;
        }

        if (index < 0 && index > 10)
        {
            Debug.LogError("获取参数错误");
            return;
        }

        Image image = _iamges[index];
        GameObject obj = Instantiate(_prefab, image.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        obj.transform.SetParent(image.transform, false);

        int petId = 11001;
        PetUnit unit = obj.GetComponent<PetUnit>();
        unit.LoadAndReplaceImage(petId);
        _drawPetDic[index] = petId;

        if (checkCanMerge(petId))
        {
            EventManager.Instance.EventTrigger<int>(MyConstants.unit_show_can_up, petId);
            return;
        }
        else
        {
            Debug.Log("erroro");
        }
    }

    public void onUnitUp(int petId)
    {
        int nCount = 0;
        List<int> tempList = new List<int>();
        foreach (int key in _drawPetDic.Keys)
        {
            tempList.Add(key);
            nCount++;
            if (nCount >= 3)
            {
                break;
            }
        }

        foreach(int key in tempList)
        {
            Image image = _iamges[key];
            Transform child = image.transform.GetChild(0);
            Destroy(child.gameObject);

            _drawPetDic.Remove(key);
        }

        // 隐藏展示
        if (!checkCanMerge(petId))
        {
            EventManager.Instance.EventTrigger<int>(MyConstants.unit_hide_can_up, petId);
        }
    }

    // 获取抽取出来的宠物放在的位置
    private int getInstantiateIndex()
    {
        for (int i = 0; i < 10; i++)
        {
            if (!_drawPetDic.ContainsKey(i))
            {
                return i;
            }
            else if(_drawPetDic[i] == 0)
            {
                return i;
            }
        }

        return -1;
    }

    // 判断宠物是否可以合并
    private bool checkCanMerge(int petId)
    {
        int nCount = 0;
        // 判断是否已经存在了三个，可合并
        foreach (int value in _drawPetDic.Values)
        {
            if (value == petId)
            {
                nCount++;
            }

            if (nCount >= 3)
            {
                // 可合并
                return true;
            }
        }
        return false;
    }

    public void OnClickSkill(int skillId)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("sdfad=== ");
    }
}

