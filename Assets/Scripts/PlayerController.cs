using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : NetworkBehaviour
{
    public static event EventHandler OnAnyPlayerSpawed;
    public static PlayerController LocalInstance { get; private set; }
    [SerializeField] private float velocidad;
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private GameObject bulletSpawner;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject weapon;
    [SerializeField] private List<GameObject> skinList;
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerData localPlayerData;
    private GameObject localSkin;
    private int skinIndex = 1;
    private bool right;

    private void Awake()
    {            
        right = true;
        DontDestroyOnLoad(transform.gameObject);
    }
    
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        localSkin = skinList[skinIndex];                        
        weapon.transform.SetParent(localSkin.transform);
        this.transform.SetPositionAndRotation(new Vector3(-1f, 0f, 0),Quaternion.identity);
    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            LocalInstance = this;
        }
        OnAnyPlayerSpawed?.Invoke(this, EventArgs.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        if (right)
        {
            // localSkin.transform.rotation = Quaternion.identity;
            localSkin.transform.localScale = new Vector3(2, 2, 2);
        }
        else
        {
            // localSkin.transform.rotation = Quaternion.Euler(0, 180, 0);
            localSkin.transform.localScale = new Vector3(-2, 2, 2);
        }

        if (!IsOwner)
        {
            return;
        }
            
        Move();
        CheckFire();
    }

    public void Move()
    {
        rb.velocity = (transform.right * velocidad * Input.GetAxis("Horizontal")) + (transform.up * rb.velocity.y);

        this.transform.SetLocalPositionAndRotation(this.transform.position, Quaternion.identity);

        if (Input.GetButtonDown("Jump") && (Mathf.Abs(rb.velocity.y) < 0.2f))
        {
            rb.AddForce(transform.up * fuerzaSalto);
        }

        anim.SetFloat("velocidadX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocidadY", rb.velocity.y);

        if (rb.velocity.x > 0.1f)
        {
            right = true;
        }
        else if (rb.velocity.x < -0.1f)
        {
            right = false;      
        }
    }

    public void CheckFire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FireServerRpc(localSkin.transform.rotation, bulletSpawner.transform.position);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void FireServerRpc(Quaternion rotation, Vector3 position)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        NetworkObject bulletNetwork = bullet.GetComponent<NetworkObject>();
        bulletNetwork.Spawn(true);
        bullet.transform.rotation = rotation;
        bulletNetwork.transform.rotation = rotation;
        bullet.transform.position = position;
        bulletNetwork.transform.position = position;

        Destroy(bullet, 2f);
    }

    public void SelectSkin(int skinIndex)
    {
        if (!IsOwner)
        {
            return;
        }

        this.skinIndex = skinIndex;
        GameObject newSkin = skinList[skinIndex];            
        newSkin.transform.rotation = localSkin.transform.rotation;
        weapon.transform.SetParent(newSkin.transform);
        localSkin.SetActive(false);
        newSkin.SetActive(true);
        localSkin = newSkin;
        anim = GetComponentInChildren<Animator>();
    }
}
