using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��λ����
public enum UserType
{
    NONE = 0, // Ĭ��ɶ������
    HERO = 1, // Ӣ�ۣ�Ĭ�ϣ�
    BOSS = 2 //boss��Ĭ�ϣ�
}

// ��������
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

// ս���еĹ���״̬
public enum PlayState
{
    start = 1, // ս����ʼ��ʼ���׶�
    startSend = 2, // ��ʼ����ʼ�ƽ׶�
    sendPuke = 3, // ˫���������ƽ׶�
    startAttackAnim = 4, // ���Ź��������׶�
    cleanTable = 5,// ��������ϵ��ƽ׶�
    end = 6// ս������
}