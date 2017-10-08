using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class NameToHashScript
{

    #region 静态常量
    public static int SpeedId = StringToHash("Speed");
    public static int IdleId = StringToHash("Base Layer.Idle");
    public static int InjuredId = StringToHash("Base Layer.Injured");
    public static int DeathId = StringToHash("Base Layer.Death");
    public static int AttackId = StringToHash("Base Layer.Attack");

    #endregion

    #region 静态变量
    static string[] laysers = new string[]{
        "Base Layer.",
        "Base Layer.Attacks."
    };

    static string[] names = new string[] {
        "Attack0", "Attack1", "Attack2", "Attack3", "Attack4", "Attack5", "Idle",
        "Skill_011", "Skill_012", "Skill_013",  "Skill_021", "Skill_022", "Skill_023", "Injured",
        "Injure0","Injure1","Injure2", "Death","InjureBack", "InjureDown", "Locomotion"
    };
    #endregion

    static Dictionary<int, string> m_dic;
    static Dictionary<int, string> DIC
    {
        get
        {
            if(null == m_dic)
                m_dic = new Dictionary<int, string>();
            return m_dic;
        }
    }
    public static int StringToHash(string name)
    {
        int hash = Animator.StringToHash(name);

        if (!DIC.ContainsKey(hash))
            DIC.Add(hash, name);

        return hash;

    }

    static bool bIsFirstLoad = true;

    public static string HashToString(int hash)
    {
        if (bIsFirstLoad)
        {
            bIsFirstLoad = false;
            LoadNames();
        }

        if (DIC.ContainsKey(hash))
            return DIC[hash];
        else
            return null;

    }

    static void LoadNames()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < laysers.Length; i++)
        {
            for (int j = 0; j < names.Length; j++)
            {
                if(sb.Length != 0)
                sb.Remove(0, sb.Length);
                sb.Append(laysers[i]);
                sb.Append(names[j]);
                StringToHash(sb.ToString());
            }
        }
        StringToHash("Speed");

    }
}
