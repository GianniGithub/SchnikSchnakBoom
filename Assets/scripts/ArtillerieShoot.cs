using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtillerieShoot : MonoBehaviour
{
	public Transform BulletPrefap;
	public float speed;
    [SerializeField]
	ArtilleriePathPointData pathData;
	ArtilleriePath aPath;
	private float trackCounter = 0f;

	int i = 0;

	void Start()
    {
        enabled = false;
        aPath = GetComponentInChildren<ArtilleriePath>();
	}

    void Update()
    {
		trackCounter += speed * Time.deltaTime;

		while (trackCounter > pathData.PointsDistanzToEacheuser)
		{
			trackCounter -= pathData.PointsDistanzToEacheuser;

			if (i + 2 >= pathData.points.Length)
			{
				enabled = false;
				return;
			}
            else
            {
				i++;
			}
		}

		var t = trackCounter / pathData.PointsDistanzToEacheuser;
		BulletPrefap.SetPositionAndRotation(Vector3.Lerp(pathData.points[i], pathData.points[i+1], t), Quaternion.Lerp(pathData.rotations[i], pathData.rotations[i+1], t));
	}
    public void ShootBullet()
    {
		if (enabled)
			return;
		trackCounter = 0f;
		i = 0;
		pathData = aPath.GetArtilleriePathData();
		enabled = true;
	}
}
