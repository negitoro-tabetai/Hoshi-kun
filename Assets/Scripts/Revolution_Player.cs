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
    [SerializeField, Tooltip("弾道を表示するための点のプレファブ")] GameObject _point;
    [SerializeField, Tooltip("点の親オブジェクト")] GameObject _trajectory;
    //流れ星のエフェクト
    [SerializeField] GameObject _shootingStar;
    [SerializeField, Tooltip("回収する半径")] float _radius;
    [SerializeField, Tooltip("初速")] float _throwForce;
    [SerializeField, Tooltip("衝突するオブジェクトのレイヤー")] LayerMask _mask;

    // 
    [SerializeField] float _distance;
    // 追従速度
    [SerializeField] float _followSpeed;
    //公転する速さ
    [SerializeField] float _rotateSpeed;
    [SerializeField, Tooltip("公転オブジェクトの大きさ倍率")] float _scale;
    //弾道予測の点の数   
    [SerializeField] int _pointCount;
    //弾道を表示する間隔の秒数
    [SerializeField] float _interval;

    //点が移動する速度
    [SerializeField] float _offsetSpeed;

    float _offset;

    List<GameObject> _points = new List<GameObject>();//弾道予測を表示するための点のリスト
    List<GameObject> _nearObject = new List<GameObject>(); //公転可能リスト
    List<GameObject> _revolutionObject = new List<GameObject>(); //公転リスト

    bool IsIndicateing;//弾道予測を表示してるかどうか

    // 引力源
    [SerializeField] GameObject _gravityObject;

    void Start()
    {
        _player = GetComponent<Player>();
        _rigidbody = GetComponent<Rigidbody2D>();
        //弾道予測を表示するための点を生成
        for (int i = 0; i < _pointCount; i++)
        {
            GameObject obj = Instantiate(_point, _trajectory.transform) as GameObject;
            _points.Add(obj);
        }
    }

    void Update()
    {

        //回せるか判定
        if (Input.GetButtonDown("Revolution"))
        {
            // 公転可能のリストの更新
            _results = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, _radius));
            _nearObject.Clear();
            for (int i = 0; i < _results.Count; i++)
            {
                if (_results[i].GetComponent<BaseRevolution>())
                {
                    _nearObject.Add(_results[i].gameObject);
                }
            }
            if (_nearObject.Count > 0 && _revolutionObject.Count < 3)// 公転可能なオブジェクトが1つでもあれば
            {
                //公転可能リストから公転中リストへ
                GameObject go = _nearObject[_nearObject.Count - 1];
                _revolutionObject.Add(go);
                _nearObject.Remove(go);

                Instantiate(_shootingStar, go.transform); //公転ブロックの子オブジェクトにパーティクルのオブジェクトを生成
                go.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                // enemyスクリプトがアタッチされていたら消す
                if (go.GetComponent<BaseEnemy>())
                {
                    Destroy(go.GetComponent<BaseEnemy>());
                }
                go.GetComponent<Rigidbody2D>().isKinematic = true;
                go.GetComponent<BaseRevolution>().On = true;
                go.layer = LayerMask.NameToLayer("Forecast");
                go.transform.SetParent(transform);//プレイヤーの子に

                //体積変更
                go.transform.localScale *= _scale;
            }
        }
        if (_revolutionObject.Count > 0)
        {
            Rotate();
            if (Input.GetAxisRaw("R_Horizontal") != 0 || Input.GetAxisRaw("R_Vertical") != 0)
            {
                _rightInput = Vector2.Lerp(_rightInput.normalized, new Vector2(Input.GetAxisRaw("R_Horizontal"), Input.GetAxisRaw("R_Vertical")).normalized, Time.deltaTime * 5);
                _trajectory.SetActive(true);
            }
            else
            {
                _trajectory.SetActive(false);
                _rightInput = Vector2.zero;
            }
            _revolutionObject[_revolutionObject.Count - 1].GetComponent<BoxCollider2D>().enabled = false;//回ってる間は当たり判定オフ

            bool hit = false;
            //弾道予測の位置に点を移動
            for (int i = 0; i < _pointCount; i++)
            {
                float t = (i * _interval) + _offset;
                float x = t * _rightInput.x * _throwForce;
                float y = (_rightInput.y * _throwForce * t) - 0.5f * (-Physics.gravity.y) * Mathf.Pow(t, 2.0f);

                _points[i].transform.localPosition = new Vector3(x, y);
                //　↑鉛直上方投射の公式
                if (i > 0)
                {
                    if (Physics2D.Linecast(_points[i - 1].transform.position, _points[i].transform.position, _mask))
                    {
                        hit = true;
                    }
                    if (hit)
                    {
                        _points[i].SetActive(false);
                    }
                    else
                    {
                        _points[i].SetActive(true);
                    }
                }
            }
            _offset = Mathf.Repeat(Time.time * _offsetSpeed, _interval);   
        }


        //Enterで飛ばす
        if (_trajectory.activeSelf)//弾道予測がでてるときしかとばせない
        {
            if (Input.GetButtonDown("Throw"))
            {
                Throw();
                _trajectory.SetActive(false);
            }

            if (Input.GetButtonDown("Give"))
            {
                Give();
                _trajectory.SetActive(false);
            }
        }
    }

    void Throw()
    {
        GameObject go = _revolutionObject[_revolutionObject.Count - 1];
        go.transform.localScale /= _scale;//大きさ戻す
        go.transform.position = _trajectory.transform.position;
        go.transform.rotation = Quaternion.identity;//Rotationを戻す

        go.GetComponent<Rigidbody2D>().isKinematic = false;//重力
        go.GetComponent<Rigidbody2D>().AddForce(_throwForce * _rightInput, ForceMode2D.Impulse);//とばす
        // タグ変更
        go.tag = "RevolutionBlock";
        go.GetComponentInChildren<ParticleSystem>().Play();//子オブジェクトのパーティクルを再生
        StartCoroutine(ActivateCollider(go));
        _revolutionObject.Remove(go);
    }
    IEnumerator ActivateCollider(GameObject obj, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        obj.GetComponent<BoxCollider2D>().enabled = true;//判定オンに
        obj.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void Give()
    {
        _gravityObject.SetActive(true);
        _gravityObject.transform.SetParent(null);
        _gravityObject.GetComponent<Rigidbody2D>().AddForce(_throwForce * _rightInput, ForceMode2D.Impulse);//とばす
    }

    void Rotate()
    {
        for (int i = 0; i < _revolutionObject.Count; i++)
        {

            _revolutionObject[i].transform.localPosition = Vector3.Lerp(_revolutionObject[i].transform.localPosition, Quaternion.Euler(0, 360 * i / _revolutionObject.Count + 360 * _rotateSpeed * Time.time, 0) * Vector3.right * _distance, _followSpeed * Time.deltaTime);
            _revolutionObject[i].transform.rotation = Quaternion.Euler(0, 360 * i / _revolutionObject.Count + Time.time * -360 * _rotateSpeed, 0);
        }
    }
}
