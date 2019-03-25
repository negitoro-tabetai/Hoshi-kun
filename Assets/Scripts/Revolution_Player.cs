using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolution_Player : MonoBehaviour
{
    Player _player;
    Rigidbody2D _rigidbody;
    // 右スティック入力
    Vector2 _rightInput;
    List<Collider2D> _results;
    [SerializeField, Tooltip("弾道を表示するための点のプレファブ")] GameObject _dummyPrefab;
    [SerializeField, Tooltip("点の親オブジェクト")] GameObject _dummyParent;
    [SerializeField, Tooltip("半径")] float _radius;
    [SerializeField, Tooltip("初速")] float _throwForce;
    [SerializeField, Tooltip("衝突するオブジェクトのレイヤー")] LayerMask _mask;

    [SerializeField] float _distance;
    [SerializeField] float _followSpeed;
    //公転する速さ
    [SerializeField] float _rotateSpeed;
    [SerializeField, Tooltip("公転オブジェクトの大きさ倍率")] float _scale;
    //弾道予測の点の数   
    [SerializeField] int _dummyCount;
    //弾道を表示する間隔の秒数
    [SerializeField] float _secInterval;

    //点が移動する速度
    [SerializeField] float _offsetSpeed;

    float _offset;

    List<GameObject> dummySpheres = new List<GameObject>();//弾道予測を表示するための点のリスト
    public List<GameObject> Object = new List<GameObject>(); //公転可能なオブジェクト
    public List<GameObject> RevolutionObject = new List<GameObject>(); //公転してるオブジェクト

    bool enter;//弾道予測を表示してるかどうか
    void Start()
    {
        _player = GetComponent<Player>();
        _rigidbody = GetComponent<Rigidbody2D>();
        Off();
        //弾道予測を表示するための点を生成
        for (int i = 0; i < _dummyCount; i++)
        {
            var obj = (GameObject)Instantiate(_dummyPrefab, _dummyParent.transform);
            dummySpheres.Add(obj);
        }
    }

    void Update()
    {
        _results = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, _radius));
        Object.Clear();
        for (int i = 0; i < _results.Count; i++)
        {
            if (_results[i].GetComponent<Revolution>())
            {
                Object.Add(_results[i].gameObject);
            }
        }
        //回せるか判定
        if (Object.Count > 0 && RevolutionObject.Count < 3)//公転可能なオブジェクトが1つでもあれば
        {

            if (Input.GetButtonDown("Revolution") || Input.GetKeyDown(KeyCode.R))     //公転ボタン押す
            {
                // small = true;
                //公転可能リストから公転中リストへ
                RevolutionObject.Add(Object[Object.Count - 1]);
                Object.Remove(Object[Object.Count - 1]);

                GameObject obj = RevolutionObject[RevolutionObject.Count - 1];
                Destroy(obj.GetComponent<MovableBlock>());
                obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                if (obj.GetComponent<Enemy>())
                {
                    Destroy(obj.GetComponent<Enemy>());
                }
                obj.GetComponent<Rigidbody2D>().isKinematic = true;
                obj.GetComponent<Revolution>().On = true;
                obj.layer = LayerMask.NameToLayer("Forecast");
                obj.transform.SetParent(transform);//プレイヤーの子に

                //体積変更
                obj.transform.localScale *= _scale;
            }
        }
        if (RevolutionObject.Count > 0)
        {
            Rotate();
            Operate();//操作
            RevolutionObject[RevolutionObject.Count - 1].GetComponent<BoxCollider2D>().enabled = false;//回ってる間は当たり判定オフ

            bool hit = false;
            //弾道予測の位置に点を移動
            for (int i = 0; i < _dummyCount; i++)
            {
                var t = (i * _secInterval) + _offset;
                var x = t * _rightInput.x * _throwForce;
                var y = (_rightInput.y * _throwForce * t) - 0.5f * (-Physics.gravity.y) * Mathf.Pow(t, 2.0f);

                dummySpheres[i].transform.localPosition = new Vector3(x, y);
                //　↑鉛直上方投射の公式
                if (i > 0)
                {
                    if (Physics2D.Linecast(dummySpheres[i - 1].transform.position, dummySpheres[i].transform.position, _mask))
                    {
                        hit = true;
                    }
                    if (hit)
                    {
                        dummySpheres[i].SetActive(false);
                    }
                    else
                    {
                        dummySpheres[i].SetActive(true);
                    }
                }
            }
        }

        _offset = Mathf.Repeat(Time.time * _offsetSpeed, _secInterval);

        //Enterで飛ばす
        if (enter == true)//弾道予測がでてるときしかとばせない
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Throw"))
            {
                Off();
                Throw();//★★★
            }
        }
    }

    public void Throw()
    {
        int index = RevolutionObject.Count - 1;

        RevolutionObject[index].transform.localScale /= _scale;//大きさ戻す
        RevolutionObject[index].transform.position = _dummyParent.transform.position;
        RevolutionObject[index].transform.rotation = Quaternion.identity;//Rotationを戻す

        RevolutionObject[index].GetComponent<Rigidbody2D>().isKinematic = false;//重力
        RevolutionObject[index].GetComponent<Rigidbody2D>().AddForce(_throwForce * _rightInput, ForceMode2D.Impulse);//とばす
        // タグ変更
        RevolutionObject[index].tag = "RevolutionBlock";
        StartCoroutine(ActivateCollider(RevolutionObject[index], 0));
        RevolutionObject.Remove(RevolutionObject[index]);
    }
    IEnumerator ActivateCollider(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.GetComponent<BoxCollider2D>().enabled = true;//判定オンに
        obj.GetComponent<BoxCollider2D>().isTrigger = true;
    }
    void Operate()
    {
        if (Input.GetAxisRaw("R_Horizontal") != 0 || Input.GetAxisRaw("R_Vertical") != 0)
        {
            _rightInput = Vector2.Lerp(_rightInput.normalized, new Vector2(Input.GetAxisRaw("R_Horizontal"), Input.GetAxisRaw("R_Vertical")).normalized, Time.deltaTime * 5);
            On();
        }
        else
        {
            Off();
            _rightInput = Vector2.zero;
        }
    }

    public void On()
    {
        _dummyParent.SetActive(true); enter = true;
    }
    public void Off()
    {
        _dummyParent.SetActive(false); enter = false;

    }

    void Rotate()
    {
        for (int i = 0; i < RevolutionObject.Count; i++)
        {
            if (_player.IsRunning)
            {
                if (i == 0)
                {
                    RevolutionObject[i].transform.localPosition = Vector3.Lerp(RevolutionObject[i].transform.localPosition, new Vector3(-_rigidbody.velocity.normalized.x * _distance, -_rigidbody.velocity.normalized.y, 0), _followSpeed * Time.deltaTime);
                }
                else
                {
                    RevolutionObject[i].transform.localPosition = Vector3.Lerp(RevolutionObject[i].transform.localPosition, RevolutionObject[i - 1].transform.localPosition + new Vector3(-_rigidbody.velocity.normalized.x * _distance, -_rigidbody.velocity.normalized.y, 0), _followSpeed * Time.deltaTime);
                }
            }
            else
            {
                RevolutionObject[i].transform.localPosition = Vector3.Lerp(RevolutionObject[i].transform.localPosition, Quaternion.Euler(0, 360 * i / RevolutionObject.Count + 360 * _rotateSpeed * Time.time, 0) * Vector3.right * _distance, _followSpeed * Time.deltaTime);
                RevolutionObject[i].transform.rotation = Quaternion.Euler(0, 360 * i / RevolutionObject.Count + Time.time * -360 * _rotateSpeed, 0);
            }
        }
    }
}
