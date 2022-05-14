using UnityEngine;

public class Sentouon : MonoBehaviour
{
	// 音データの再生装置を格納する変数
	private AudioSource audio;

	// 音データを格納する変数（Inspectorタブからも値を変更できるようにする）
	[SerializeField]
	private AudioClip sound;

	// Start is called before the first frame update
	void Start()
	{
		// ゲームスタート時にAudioSource（音再生装置）のコンポーネントを加える
		audio = gameObject.AddComponent<AudioSource>();

	}

	/// <summary>
	/// 衝突した時
	/// </summary>
	/// <param name="collision"></param>
	void OnCollisionEnter(Collision collision)
	{
		// 衝突した相手にPlayerタグが付いているとき
		if (collision.gameObject.tag == "Bishop_w")
		{
			// 音（sound）を一度だけ（PlayOneShot）再生する
			audio.PlayOneShot(sound);
		}
	}
}