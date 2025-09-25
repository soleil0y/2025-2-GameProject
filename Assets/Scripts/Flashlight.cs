using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public enum flashlights
    {
        spot,
        point,
        laser
    };

    //public float damage;
    public GameObject[] flashlightList;
    public float[] damage;

    public int changeFlashlight(int lightnum)
    {
        flashlightList[0].SetActive(false);
        flashlightList[1].SetActive(false);
        flashlightList[2].SetActive(false);

        flashlightList[lightnum].SetActive(true);

        // �ϴ� ���Ѵ��..
        // ��... ���͸��Ҹ𷮵� �ٲ���ϴµ�
        //damage = flashlightList[lightnum].GetComponent<FlashlightInfo>().damage;
        StageManager.Instance.flashlightDamage = damage[lightnum];

        return lightnum;
    }
}
