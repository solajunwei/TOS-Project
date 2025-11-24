using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using User;

namespace User
{
    class UserInfo
    {
        public int id; // id，目前只有一个，后续可能有多个
        public string name; // 名字
        public int point; // 金币
    }
}

public class UserModel : BaseManager<UserModel>
{
    private UserInfo userInfo = new UserInfo();

    // 获取金币
    public int getUserPoint()
    {
        return userInfo.point;
    }

    public void AddPoint(int point)
    {
        int tmpPoint = userInfo.point + point;
        if(tmpPoint < 0)
        {
            tmpPoint = 0;
        }
        userInfo.point = tmpPoint;
    }

}
