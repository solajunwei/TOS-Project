using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 单位类型
public enum UserType
{
    NONE = 0, // 默认啥都不是
    HERO = 1, // 英雄（默认）
    BOSS = 2 //boss（默认）
}

// 卡牌类型
public enum PUKENUM
{
    NONE = 0,
    PUKEA = 1,
    PUKE2 = 2,
    PUKE3 = 3,
    PUKE4 = 4,
    PUKE5 = 5,
    PUKE6 = 6,
    PUKE7 = 7,
    PUKE8 = 8,
    PUKE9 = 9,
    PUKE10 = 10,
    PUKEJ = 11,
    PUKEQ = 12,
    PUKEK = 13,
}

// 战斗中的过程状态
public enum PlayState
{
    start = 1, // 战斗开始初始化阶段
    startSend = 2, // 开始发初始牌阶段
    sendPuke = 3, // 双方轮流发牌阶段
    startAttackAnim = 4, // 播放攻击动画阶段
    cleanTable = 5,// 清空桌面上的牌阶段
    end = 6// 战斗结束
}