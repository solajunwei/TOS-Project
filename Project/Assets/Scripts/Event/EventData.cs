using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyConstants
{
    // 开始战斗
    public const string start_game = "start_game";

    public const string start_game_run = "start_game_run";

    // 单位死亡
    public const string Enemy_deal = "enemy deal";

    // 发射子弹
    public const string attack_enemy = "attack enemy";

    // 下一轮
    public const string jump_next_round = "jump next round";

    // 基地受伤
    public const string home_attack = "home attack";
    
    // 游戏结束
    public const string gameoverLevel = "gameoverlevel";

    // 创建一个单位战斗
    public const string create_unit = "create unit";

    // 显示单位可升级
    public const string unit_show_can_up = "unit show can up";
    // 隐藏单位可升级
    public const string unit_hide_can_up = "unit hide can up";
    // 单位升级
    public const string unit_up = "unit up";

    // 英雄出售
    public const string unit_sell = "unit sell";

    // 隐藏单位出售
    public const string hide_unit_sell = "hide unit sell";

    // 添加金币
    public const string add_Point = "add Point";

    // 返回界面
    public const string backUI = "backUI";
}

class BattleConfig
{
    public static List<int> RoundGroupList = new List<int>{
        2, 4, 6,8
    };

    public const int UPLEVELNEEDPETNUM = 3;

    // 抽卡所需要的金币
    public const int DRAWNUM = 5;
}
