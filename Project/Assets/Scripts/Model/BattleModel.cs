using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;
using Battle;

namespace Battle
{
    // 抽奖后的状态
    public enum PetState
    {
        PETEGG, // 宠物蛋
        PETPERSON     // 宠物人
    }

    // 抽奖后的宠物数据
    public class DrawPetInfo
    {
        public int nPetId;  // 宠物ID
        public int nDrawLevel;  // 抽中该宠物时的回合
        public PetState nPetState; // 抽奖宠物信息的状态
        public GameObject petObj; // 生成的组件

    }

    // 战斗数据
    public class BattleInfo
    {
        // 当前第几轮
        private int round = 0; 
        public int Round
        {
            get{return round;}
            set{round = value;}
        }

        // 设置下一轮
        public void setNextRound()
        {
            round++;
            enemyHaveCount = 0;
            if(round > roundCount)
            {
                // 发送消息，当前已经通关结束
                EventManager.Instance.EventTrigger(MyConstants.gameoverLevel);
            }
            else
            {
                // 下一关
                EventManager.Instance.EventTrigger(MyConstants.jump_next_round);
            }
        }

        // 总共几轮
        private int roundCount = 4;
        public int RoundCount
        {
            set{roundCount = value;}
            get{return roundCount;}
        }

        // 当前轮有总共有多少个敌方单位,默认时3个
        private int enemyCount = 3;
        public int EnemyCount
        {
            set{enemyCount = value;}
            get{return enemyCount;}
        }

        // 当前轮已经出现过多少个单位
        private int enemyHaveCount = 0;
        public int EnemyHaveCount
        {
            get{return enemyHaveCount;}
            set{enemyHaveCount = value;}
        }

        // boss 或者 家 的血量
        private bool ishome = false;
        //TODO

#region  我方单位的数据
        // 我方单位
        private List<GameObject> playerList = new List<GameObject>();
        public List<GameObject> PlayerList
        {
            get{return playerList;}
        }

        /// <summary>
        /// 添加我方单位
        /// </summary>
        /// <param name="playerObj">单位的组件</param>
        public void addPlayer(GameObject playerObj)
        {
            playerList.Add(playerObj);
        }

        /// <summary>
        /// 删除我方单位
        /// </summary>
        /// <param name="playerObj">单位的组件</param>
        public void removePlayer(GameObject playerObj)
        {
            foreach(GameObject player in playerList)
            {
                if(player == playerObj)
                {
                    playerList.Remove(player);
                    return;
                }
            }
        }
#endregion


#region 敌方单位
        private List<GameObject> enemyList = new List<GameObject>();
        public List<GameObject> EnemyList
        {
            get{return enemyList;}
        }
        
        /// <summary>
        /// 添加敌方单位
        /// </summary>
        /// <param name="enemyObj">敌方的组件</param>
        public void addEnemy(GameObject enemyObj)
        {
            if(enemyList.Count >= enemyCount)
            {
                Debug.LogError("敌方单位已经达到最大值");
                return;
            }
            enemyList.Add(enemyObj);
            enemyHaveCount++;
        }

        /// <summary>
        /// 删除一个单方单位
        /// </summary>
        /// <param name="enemyObj">敌方的组件</param>
        public void removeEnemy(GameObject enemyObj)
        {
            foreach(GameObject enemy in enemyList)
            {
                if(enemy == enemyObj)
                {
                    enemyList.Remove(enemy);

                    // 消灭所有的敌方单位，进去下一轮
                    if (IsRoundEnemyMax() && enemyList.Count == 0)
                    {
                        setNextRound();
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// 当前敌方单位是否为空
        /// </summary>
        /// <returns></returns>
        public bool isEnemyEmpty()
        {
            return enemyList.Count == 0;
        }

        /// <summary>
        /// 敌方单位是否已经全部出现了
        /// </summary>
        /// <returns></returns>
        public bool IsRoundEnemyMax()
        {
            return enemyHaveCount >= enemyCount;
        }


#endregion

        /// <summary>
        /// 获取第一个能够打到的敌方单位
        /// </summary>
        /// <param name="playerObj">要攻击的单位</param>
        /// <returns></returns>
        public GameObject getFirstAttackEnemy(GameObject playerObj)
        {
            HeroUnit heroUnit = playerObj.GetComponent<HeroUnit>();
            if(heroUnit == null)
            {
                return null;
            }

            Vector3 heroPositoion = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, 0);
            foreach(GameObject enemy in enemyList)
            {
                Vector3 enemyPositoion = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0);
                float tmpDis = Vector3.Distance(heroPositoion, enemyPositoion);
                if(heroUnit.Distance >= tmpDis)
                {
                    return enemy;
                }
            }
            return null;
        }
    

        private bool isOver = false;
        // 战斗是否结束
        public bool isGameOver()
        {   
            // 最后一轮结束，超过最大轮战斗结束
            if(round > roundCount)
            {
                return true;
            }

            // 家没了，战斗结束
            if (ishome)
            {
                return true;
            }

            return isOver;
        }

        public void setGameOver(bool isover)
        {
            isOver = isover;
        }
        
    }
}



public class BattleModel : BaseManager<BattleModel>
{

    // 一关有多少回合
    private List<int> _roundGroupList = new List<int>
    {
        2, 4, 6,8
    };


    private BattleInfo curBattleInfo = new BattleInfo();
    public BattleInfo CurBattleInfo
    {
        get{return curBattleInfo;}
    }

    private int petGridCount = 10;

    // 抽奖获得的宠物列表
    private Dictionary<int, DrawPetInfo> drawPetDic = new Dictionary<int, DrawPetInfo>();

    // 多少个英雄单位
    private Dictionary<int, GameObject> _PlayerList = new Dictionary<int,GameObject>();
    public Dictionary<int, GameObject> PlayerList
    {
        get { return _PlayerList; }
    }

    // 初始化轮数
    public void initRound()
    {
        curBattleInfo.Round = 1;
    }


    // 查看是否已经抽宠物已经达到最大值
    public bool checkDrawPetMax()
    {
        if(drawPetDic.Count < petGridCount)
        {
            return false;
        }

        return true;
    }

    // 抽到宠物后存放的格子id
    public int getDrawPetInIndex()
    {
        if (checkDrawPetMax())
        {
            return -1;
        }

        for(int i = 0; i < petGridCount; i++)
        {
            if (!drawPetDic.ContainsKey(i))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// 添加一个抽中的宠物
    /// </summary>
    public GameObject addDrawPet(out int index)
    {
        index = getDrawPetInIndex();
        if (index == -1)
        {
            Debug.LogError("格子已满，无法抽取");
            return null;
        }

        int petId = 11001; // TODO
        GameObject prefab = Resources.Load<GameObject>("UI/Perfabs/Pet/PetIcon");
        if (null == prefab)
        {
            Debug.LogError("Resources load PetIcon");
            return null;
        }
        GameObject obj = Object.Instantiate(prefab);
        PetUnit unit = obj.GetComponent<PetUnit>();
        unit.LoadPetInitialValue();

        DrawPetInfo petinfo = new DrawPetInfo();
        petinfo.nPetId = petId;
        petinfo.nDrawLevel = getCurRound();
        petinfo.nPetState = PetState.PETEGG;
        petinfo.petObj = obj;
        drawPetDic[index] = petinfo;
        return obj;
    }

    // 展示或隐藏单位的升级按钮, 返回是否存在可以升级的
    public bool showOrHidePlayBtn()
    {
        Dictionary<int, int> temp = new Dictionary<int, int>();
        for(int i = 0; i < petGridCount; i++)
        {
            if (drawPetDic.ContainsKey(i))
            {
                DrawPetInfo info = drawPetDic[i];
                if(info.nPetState == PetState.PETPERSON)
                {
                    if (temp.ContainsKey(info.nPetId))
                    {
                        temp[info.nPetId]++;
                    }
                    else
                    {
                        temp[info.nPetId] = 1;
                    }
                }

                // 先隐藏所有的升级按钮
                EventManager.Instance.EventTrigger<int>(MyConstants.unit_hide_can_up, info.nPetId);
            }
        }

        bool ret = false;
        // 再展示所有的可升级按钮
        foreach(int petid in temp.Keys)
        {
            if(temp[petid] >= BattleConfig.UPLEVELNEEDPETNUM)
            {
                ret = true;
                EventManager.Instance.EventTrigger<int>(MyConstants.unit_show_can_up, petid);
            }
        }

        return ret;
    }

    // 判断是否从蛋形状改编成宠物状态
    public void changePetState()
    {
        foreach(DrawPetInfo info in drawPetDic.Values)
        {
            if(info.nPetState == PetState.PETEGG)
            {
                PetUnit unit = info.petObj.GetComponent<PetUnit>();
                unit.LoadAndReplaceImage(info.nPetId);
                info.nPetState = PetState.PETPERSON;
            }
        }

        showOrHidePlayBtn();
    }

    // 升级一个单位
    public void upOnePet(int petid)
    {
        int nDelNum = 0;
        for(int i = 0; i < petGridCount; i++)
        {
            if(drawPetDic.ContainsKey(i))
            {
                DrawPetInfo petInfo = drawPetDic[i];
                if(petInfo.nPetState == PetState.PETPERSON && petInfo.nPetId == petid)
                {
                    Object.Destroy(petInfo.petObj.gameObject);
                    drawPetDic.Remove(i);
                    nDelNum++;
                    if(nDelNum == BattleConfig.UPLEVELNEEDPETNUM)
                    {
                        break;
                    }
                }
            }
        }

        showOrHidePlayBtn();
    }

    /// <summary>
    /// 升级所有的单位,返回还有可升级的
    /// </summary>
    public bool upAllpet()
    {
        Dictionary<int, int> temp = new Dictionary<int, int>();
        for(int i = 0; i < petGridCount; i++)
        {
            if (drawPetDic.ContainsKey(i))
            {
                DrawPetInfo info = drawPetDic[i];
                if(info.nPetState == PetState.PETPERSON)
                {
                    if (temp.ContainsKey(info.nPetId))
                    {
                        temp[info.nPetId]++;
                    }
                    else
                    {
                        temp[info.nPetId] = 1;
                    }
                }
            }
        }
        
        bool isShowUp = false;
        // 再展示所有的可升级按钮
        foreach(int petid in temp.Keys)
        {
            int petCount = temp[petid];
            if(petCount >= BattleConfig.UPLEVELNEEDPETNUM)
            {
                EventManager.Instance.EventTrigger<int>(MyConstants.unit_up, petid);
                petCount -=3;
                if(petCount >= BattleConfig.UPLEVELNEEDPETNUM)
                {
                    isShowUp = true;
                }
                else
                {
                    EventManager.Instance.EventTrigger<int>(MyConstants.unit_hide_can_up, petid);
                }
            }
        }

        return isShowUp;
    }

    /// <summary>
    /// 判断当前表格是否为宠物形态
    /// </summary>
    /// <param name="index">位置id</param>
    /// <returns></returns>
    public bool checkIsPetPersonState(int index)
    {

        if (drawPetDic.ContainsKey(index))
        {
            DrawPetInfo info = drawPetDic[index];
            if(info.nPetState == PetState.PETPERSON)
            {
                return true;
            }
        }

        return false;
    }

    // 删除一个我方单位
    public void removePlayerUnit(GameObject player)
    {
        curBattleInfo.removePlayer(player);
    }

    /// <summary>
    /// 获取当前场战斗总共多少轮
    /// </summary>
    /// <returns></returns>
    public int getRoundCount()
    {
        return curBattleInfo.RoundCount;
    }

    public int getCurRound()
    {
        return curBattleInfo.Round;
    }
}
