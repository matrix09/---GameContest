using Assets.Scripts.Helper;
using AttTypeDefine;

public class Begin : UIScene {

    void Start()
    {

        if (e_SceneGo == SceneGo.Select)
            Helpers.UIScene<UIScene_SelecteV1>();
        else
            Helpers.UIScene<UIScene_Start>();

    }
}
