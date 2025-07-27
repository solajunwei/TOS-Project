using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using cfg;


public class PetInfo : ExampleItemData
{
    public pet petInfo;
    public Pet Delegate;
    public bool isOnBattle = false;
}

public class Pet : BasePanel
{
    [SerializeField]
    private ListView listView;

    [SerializeField]
    private ListView goListView;

    public Button _goButton;
    // 技能
    public Text _Skill1;
    public Text _Skill2;
    public Text _Skill3;
    public Text _Skill4;

    // 属性
    public Text _Attri1;
    public Text _Attri2;
    public Text _Attri3;
    public Text _Attri4;

    // 元素
    public Text _Element;


    private Tbpet _Pet;
    private TbpetAttri _PetAttri;
    private Tbskills _Skills;
    private PetInfo _ShowPetInfo;
    private int _SelElementId;

    /// <summary>
    /// 展示宠物列表
    /// </summary>
    private List<PetInfo> _PetList = new List<PetInfo>();

    /// <summary>
    /// 上阵宠物列表
    /// </summary>
    private List<PetInfo> _GoBattle = new List<PetInfo>();

    private void Start()
    {
        cfg.Tables tb = GameConfig.Instance.getTables();
        _Pet = tb.Tbpet;
        _PetAttri = tb.TbpetAttri;
        _Skills = tb.Tbskills;
        _SelElementId = 0;
        showPetListByElement(1, true);
        //foreach (pet petInfo in petList)


        // Test();
    }

    public void setPetId(PetInfo petInfo)
    {
        _ShowPetInfo = petInfo;
        updateView();
    }

    public void updateView()
    {
        pet petInfo = _ShowPetInfo.petInfo;

        int elementId = petInfo.Type;
        _Element.text = _PetAttri.Get(elementId).Name;

        int petSkillId1 = petInfo.Skill1;
        skills skill1 = _Skills.Get(petSkillId1);
        _Skill1.text = skill1.Name;
        _Attri1.text = skill1.Attr;

        int petSkillId2 = petInfo.Skill2;
        skills skill2 = _Skills.Get(petSkillId2);
        _Skill2.text = skill2.Name;
        _Attri2.text = skill2.Attr;

        int petSkillId3 = petInfo.Skill3;
        skills skill3 = _Skills.Get(petSkillId3);
        _Skill3.text = skill3.Name;
        _Attri3.text = skill3.Attr;

        int petSkillId4 = petInfo.Skill4;
        skills skill4 = _Skills.Get(petSkillId4);
        _Skill4.text = skill4.Name;
        _Attri4.text = skill4.Attr;

        if (_ShowPetInfo.isOnBattle)
        {
            _goButton.interactable = false;
        }
        else
        {
            _goButton.interactable = true;
        }

    }
    public void showPetListByElement(int tag, bool isFirstIn = false)
    {
        _SelElementId = tag;
        _PetList.Clear();
        listView.ClearItems();
        PetInfo info = null;
        for (int i = 0; i < _Pet.DataList.Count; i++)
        {
            if (_Pet.DataList[i].Type == tag)
            {
                info = new PetInfo();
                info.Id = i;
                info.petInfo = _Pet.DataList[i];
                info.Delegate = this;
                _PetList.Add(info);
            }
        }

        listView.SetDataSource(_PetList.Cast<IListItemData>().ToList());
        info = _PetList.FirstOrDefault();
        if (info != null)
        {
            setPetId(info);
        }
        else if (tag < 5 && isFirstIn)
        {
            onClickElement(tag + 1);
        }
    }

    public void onClickElement(int tag)
    {
        if (tag == _SelElementId)
            return;

        showPetListByElement(tag);
    }


    public void onClickSkillTag(int skillid)
    {

    }

    public void onClickBack()
    {
        UIManager.Instance.HidePanel("Perfabs/Pet/Pet");
    }

    public void onGoBattle()
    {
        Debug.Log("onGoButtle");
        if (_ShowPetInfo.isOnBattle)
        {
            return;
        }
         _goButton.interactable = false;
        _ShowPetInfo.isOnBattle = true;
        _GoBattle.Add(_ShowPetInfo);
        goListView.SetDataSource(_GoBattle.Cast<IListItemData>().ToList());
    }


    /// <summary>
    /// 出击
    /// </summary>
    public void onBattle()
    {
        foreach (PetInfo petinfo in _GoBattle)
        {
            PetModel.Instance.AddOnBattlePet(petinfo.petInfo);
        }
        Debug.Log("出击");

        UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene");
        UIManager.Instance.SetCanvasVisible(false);
    }






















    /// <summary>
    /// // 下面是测试代码
    /// </summary>
    /// 
    //[SerializeField] private Button addButton;
    //[SerializeField] private Button clearButton;
    private List<ExampleItemData> dataList = new List<ExampleItemData>();
    private void Test()
    {
        // 初始化数据
        InitializeData();

        // 设置列表数据
        listView.SetDataSource(dataList.Cast<IListItemData>().ToList());

        // 注册事件
        listView.OnItemClicked += OnItemClicked;
        //addButton.onClick.AddListener(AddItem);
        //clearButton.onClick.AddListener(ClearItems);
    }
    private void InitializeData()
    {
        for (int i = 0; i < 10; i++)
        {
            dataList.Add(new ExampleItemData
            {
                Id = i,
            });
        }
    }
    private void OnItemClicked(int index, IListItemData data)
    {
        //Debug.Log($"Clicked on item {index}: {((ExampleItemData)data).Title}");
        listView.ScrollToItem(index);
    }

    private void AddItem()
    {
        int newId = dataList.Count;
        string str = $"string1 {newId}";
        dataList.Add(new ExampleItemData
        {
            Id = newId,
        });

        listView.SetDataSource(dataList.Cast<IListItemData>().ToList());
    }

    private void ClearItems()
    {
        dataList.Clear();
        listView.ClearItems();
    }
}