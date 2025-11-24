

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using User;
using cfg;
//using System.Numerics;

public class BattleUI : UIView
{
    public Image[] _iamges;
    private GameObject _movePrefab;

    public GameObject _downObj;
    public Button   _downBtn;
    public GameObject _rightObj;
    public Button   _rightBtn;
    public Button _upBtn;
    public Button _onStartBtn;

    private bool isDownClose = false;
    private bool isRightClose = false;


    // 轮数
    public Text _RoundText;
    // 金币
    public Text _PointText;
    // 血量
    public Text _BloodText;

    // 血量默认20
    private int _blood = 20;

    // 金币默认为0
    private int _point = UserModel.Instance.getUserPoint();

    // 是否已经点击了
    private bool _isClick = false;

    public void Start()
    {
        int roundCount = BattleModel.Instance.getRoundCount();
        int round = BattleModel.Instance.getCurRound();
        _RoundText.text = round + "/" + roundCount;
        _PointText.text = _point.ToString();
        _BloodText.text = _blood.ToString();

        _movePrefab = null;

        EventManager.Instance.AddEventListener(MyConstants.jump_next_round, updateNextRound);
        EventManager.Instance.AddEventListener(MyConstants.home_attack, updateLeft);
        EventManager.Instance.AddEventListener<int>(MyConstants.unit_up, onUnitUp);
        EventManager.Instance.AddEventListener<int>(MyConstants.unit_show_can_up, onShowUnitUp);
        EventManager.Instance.AddEventListener<int>(MyConstants.unit_hide_can_up, onHideUnitUp);
        EventManager.Instance.AddEventListener<int>(MyConstants.add_Point, onAddPoint);
    }
    public void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener(MyConstants.jump_next_round, updateNextRound);
        EventManager.Instance.RemoveEventListener(MyConstants.home_attack, updateLeft);
        EventManager.Instance.RemoveEventListener<int>(MyConstants.unit_up, onUnitUp);
        EventManager.Instance.RemoveEventListener<int>(MyConstants.unit_show_can_up, onShowUnitUp);
        EventManager.Instance.RemoveEventListener<int>(MyConstants.unit_hide_can_up, onHideUnitUp);
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

            // 遍历命中结果，判断是否包含目标Image
            foreach (RaycastResult result in hitResults)
            {
                for (int i = 0; i < _iamges.Length; i++)
                {
                    if (result.gameObject == _iamges[i].gameObject)
                    {
                        if (BattleModel.Instance.checkIsPetPersonState(i))
                        {
                            _isClick = true;
                            break; // 找到后退出循环
                        }
                    }
                }
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
            _isClick = false;

            Transform parent = _iamges[0].transform.parent;
            bool isIn = IsMouseUpInImageRect((RectTransform)parent);
         
            // 如果在image中，则取消
            if (!isIn)
            {
                EventManager.Instance.EventTrigger<Vector3>(MyConstants.create_unit, _movePrefab.transform.position);
            }
            
            if (_movePrefab)
            {
                Destroy(_movePrefab);
                _movePrefab = null;
            }
        }
    }

     /// <summary>
    /// 判断鼠标抬起位置是否在Image的矩形范围内（仅矩形，不考虑遮罩/裁剪）
    /// </summary>
    private bool IsMouseUpInImageRect(RectTransform rectTrans)
    {
        Canvas canvas = UIManager.Instance.UICanvas;

        // 1. 将鼠标抬起的屏幕坐标转换为Image本地坐标系的坐标
        Vector2 localMousePos;
        bool isConvertSuccess = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTrans,                          // 目标Image的RectTransform
            Input.mousePosition,                // 鼠标抬起的屏幕坐标
            canvas.worldCamera, // 适配Canvas渲染模式
            out localMousePos                   // 输出转换后的本地坐标
        );

        // 2. 转换失败直接返回false；转换成功则判断是否在矩形内
        if (!isConvertSuccess) return false;
        return rectTrans.rect.Contains(localMousePos);
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
                GameObject prefab = Resources.Load<GameObject>("UI/Perfabs/Pet/PetIcon");
                if (null == prefab)
                {
                    Debug.LogError("Resources load PetIcon");
                    return;
                }
                _movePrefab = Instantiate(prefab, gameObject.transform);
                _movePrefab.transform.localRotation = Quaternion.identity;
                _movePrefab.transform.localScale = Vector3.one;
            }
            _movePrefab.transform.localPosition = new Vector3(localPos.x, localPos.y, 0);
        }
    }

    // 改变轮数
    public void updateNextRound()
    {
        int roundCount = BattleModel.Instance.getRoundCount();
        int round = BattleModel.Instance.getCurRound();
        _RoundText.text = round + "/" + roundCount;
        BattleModel.Instance.changePetState();
    }

    // 更改生命值
    public void updateLeft()
    {
        _blood--;
        _BloodText.text = _blood.ToString();
    }


    public void onAddPoint(int point)
    {
        _point += point;
        _PointText.text = _point.ToString();
        UserModel.Instance.AddPoint(point);
    }

    // 抽卡
    public void Draw()
    {
        //GameConfig.Instance.getTables().GetType("TbFZGameConfig");
        
        //cfg.FZGameConfig config = GameConfig.Instance.getTables().TbFZGameConfig.Get("DrawPoint");

        if(BattleConfig.DRAWNUM > UserModel.Instance.getUserPoint())
        {

            return;
        }

        // 生成一个宠物蛋
        GameObject obj = BattleModel.Instance.addDrawPet(out int index);
        if(obj == null || index == -1)
        {
            return;
        }

        Image image = _iamges[index];
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        obj.transform.SetParent(image.transform, false);
    }

    public void onUnitUp(int petId)
    {
        BattleModel.Instance.upOnePet(petId);
    }

    public void onHideUnitUp(int petId)
    {
        _upBtn.gameObject.SetActive(false);
    }

    public void onShowUnitUp(int petId = 0)
    {
        _upBtn.gameObject.SetActive(true);
    }

    public void RunMove(Transform transform, Vector3 moveVec)
    {
        transform.DOMove(moveVec, 0.5f)
        .SetEase(Ease.InQuad)
        .OnComplete(() =>
        {
            if(transform.gameObject.activeSelf)
            {
                transform.gameObject.SetActive(false);
            }
            else
            {
                transform.gameObject.SetActive(true);
            }
        });
    }


#region Inspector 按钮事件
    public void OnClickSkill(int skillId)
    {

    }

    //private bool isDownClose = false;

    public void onClickDownShowHide()
    {
        Vector3 vec = _downObj.transform.position;
        if(isDownClose)
        {
            vec.y += 200;
        }
        else
        {
            vec.y -= 200;
        }

        _downObj.transform.DOMove(vec, 0.5f)
        .SetEase(Ease.InQuad)
        .OnComplete(() =>
        {
            _downBtn.transform.Rotate(new Vector3(0, 0, 180));
            isDownClose = !isDownClose;
        });


    }

    public void onClickRightShowHide()
    {
        Vector3 vec = _rightObj.transform.position;
        if(isRightClose)
        {
            vec.x -= 180;
        }
        else
        {
            vec.x += 180;
        }

        _rightObj.transform.DOMove(vec, 0.5f)
        .SetEase(Ease.InQuad)
        .OnComplete(() =>
        {
            _rightBtn.transform.Rotate(new Vector3(0, 0, 180));
            isRightClose = !isRightClose;
        });
    }

    // 升级
    public void onClickUp()
    {
        bool isUp = BattleModel.Instance.upAllpet();
        if (isUp)
        {
            onShowUnitUp();
        }
    }

    // 开始游戏
    public void onStartGame()
    {
        _onStartBtn.gameObject.SetActive(false);
        

        GameObject prefab = Resources.Load<GameObject>("UI/Perfabs/Tips/EnterObj");
        if (null == prefab)
        {
            Debug.LogError("Resources load EnterObj");
            return;
        }

        GameObject obj = Instantiate(prefab);
        obj.transform.localPosition = new Vector3(-1134, 0, 0);
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        BattleModel.Instance.initRound();

        int roundCount = BattleModel.Instance.getRoundCount();
        int round = BattleModel.Instance.getCurRound();
        _RoundText.text = round + "/" + roundCount;
        obj.transform.SetParent(gameObject.transform, false);
        EventManager.Instance.EventTrigger(MyConstants.start_game_run);
    }
#endregion



}

