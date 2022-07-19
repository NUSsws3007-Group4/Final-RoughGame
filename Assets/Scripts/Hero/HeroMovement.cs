using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroMovement : MonoBehaviour
{
    public Text text;
    public void settestui()
    {
        string ui = "Position status: " + mPlace + ", Facing: " + mFaceDir
            + ", Health: " + mHealth + "\nSpawn at: " + mRespawnPoint + "\nDsah used:" +
            mDashUsed + "\nSpeed: " + mRigidbody.velocity + "\nRunning: " + mIsRun +
            "\nSneak: " + mIsSneak + ", Forced Sneak: " + mForceSneak;
        text.text = ui;
    }
    /// <summary>
    /// 定义常量
    /// 在糅合时修改
    /// </summary>
    //默认最大速度
    private const float mMaxSpeedDefault = 5f;
    //默认加速度
    private const float mAccelerationDefault = 30f;
    //弹跳能力
    private const float mJumpForce = 13f;
    //冲刺能力
    private const float mDashForce = 0.15f;
    private bool respaned = false;
    //重生无敌
    private float respawnTimer = 0f;
    //重生无敌计时
    public Animator mAnimeControl = null;

    private Vector3 mRespawnPoint;
    //移动方向
    public enum mDirection { left = -1, stop, right };
    private mDirection mMoveDir;
    private mDirection mFaceDir = mDirection.right;
    //位置状态
    private enum mPlaceStatus
    {
        Invalid = 0,
        OnGround = 1,
        OnWallRight = 18,
        OnWallLeft = 34,
        InAir = 4,
        OnLadder = 8,
    }
    private mPlaceStatus mPlace = mPlaceStatus.InAir;
    //人物速度
    private float mSpeed = 0f;
    //最大速度
    private float mMaxSpeed = 5f;
    //加速度
    private float mAcceleration;
    //冲刺
    private bool mIsDash = false;
    //冲刺已使用
    private bool mDashUsed = false;
    private float mDashTimeCount = 0;
    //加速过程
    private bool smoothmove;
    //蹲下状态
    private bool mIsSneak = false;
    //顶到头了
    private bool mForceSneak = false;
    //跑动
    private bool mIsRun = false;
    //能跳的段数
    private int mJumpSkill = 1;
    //空中已经跳的段数
    private int mJumpCount = 0;
    //碰撞体
    private Rigidbody2D mRigidbody;
    public bloodbarcontrol mHealth;

    public void setRespawnPoint(Vector2 _point)
    {
        mRespawnPoint = new Vector3(_point.x, _point.y);
    }
    public void respawn()
    {
        respaned = true;
        mHealth.setvolume(mHealth.maxblood);
        mRigidbody.velocity = new Vector2(0f, 0f);
        gameObject.transform.localPosition = mRespawnPoint;
        mSpeed = 0f;
        mAcceleration = mAccelerationDefault;
    }

    public void hurt(int _damage = 1)
    {
        mAnimeControl.SetTrigger("Hurt");
    }

    void Start()
    {
        //!!!Change it in real game!!!
        mRespawnPoint = new Vector3(0f, 0f);
        mAcceleration = mAccelerationDefault;
        mMaxSpeed = mMaxSpeedDefault;
        smoothmove = false;
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.x < -30f || transform.position.x > 34f || transform.position.y > 20f || transform.position.y < -20f)
        {
            respawn();
        }
        if (mIsSneak)
        {
            Vector2 pos = transform.position;
            pos.y += gameObject.GetComponent<BoxCollider2D>().size.y * 2;
            mForceSneak = Physics2D.OverlapCircle(pos, 0.5f, LayerMask.GetMask("Ground"));
        }
        if (mHealth.getvolume() <= 0)
        {
            respawn();
        }
        mAnimeControl.SetBool("IsMove", !(mSpeed == 0f));
        mAnimeControl.SetBool("IsJump", mPlace == mPlaceStatus.InAir && mRigidbody.velocity.y > 0);
        mAnimeControl.SetBool("IsFall", mPlace == mPlaceStatus.InAir && mRigidbody.velocity.y <= 0);
        //esc键退出
        if (Input.GetKey(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }

        //蹲下操作
        if (!mIsSneak && (Input.GetKeyDown(KeyCode.LeftControl) || mForceSneak))
        {
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            Vector2 colliderSize = collider.size;
            colliderSize.y /= 2;
            collider.size = colliderSize;
            Vector2 colliderOffset = collider.offset;
            colliderOffset.y = colliderSize.y / 2;
            collider.offset = colliderOffset;
            mIsSneak = true;
            mSpeed /= 2f;
            mMaxSpeed = mMaxSpeedDefault / 2f;
            mAcceleration = mAccelerationDefault / 2f;
        }
        if (mIsSneak && (Input.GetKeyUp(KeyCode.LeftControl) || (!mForceSneak && !Input.GetKey(KeyCode.LeftControl))))
        {

            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            Vector2 colliderSize = collider.size;
            colliderSize.y *= 2;
            collider.size = colliderSize;
            Vector2 colliderOffset = collider.offset;
            colliderOffset.y = colliderSize.y / 2;
            collider.offset = colliderOffset;
            mIsSneak = false;
            mMaxSpeed = mMaxSpeedDefault;
            mAcceleration = mAccelerationDefault;
        }

        //跑动
        if (Input.GetKeyDown(KeyCode.LeftShift) && mPlace == mPlaceStatus.OnGround && !mIsSneak)
        {
            mIsRun = true;
            mMaxSpeed = mMaxSpeedDefault * 2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || mPlace != mPlaceStatus.OnGround)
        {
            //todo：结束冲刺缓慢减速？
            mIsRun = false;
            mMaxSpeed = mMaxSpeedDefault;
        }
        mAnimeControl.SetBool("IsRun", mIsRun);

        //左移
        if (getMoveDirection() != mDirection.stop)
        {
            if (Mathf.Abs(mSpeed) < mMaxSpeed)
            {
                Debug.Log("Accelerating");
                mSpeed += ((float)getMoveDirection()) * mAcceleration * Time.smoothDeltaTime;
            }
            else
            {
                mSpeed = ((float)getMoveDirection()) * mMaxSpeed;
            }
        }
        //不移&减速
        else
        {
            if (mPlace != mPlaceStatus.InAir && mSpeed != 0)
            {
                Debug.Log("Slowing Down");
                //新的速度是原速度减去加速度*时间*方向
                float newSpeed = ((smoothmove) ? mSpeed - mAcceleration * Time.smoothDeltaTime * (mSpeed > 0 ? 1 : -1) : 0f);
                //到0不再减速
                mSpeed = (newSpeed * mSpeed < 0) ? 0 : newSpeed;
            }
        }
        //跳
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (mPlace == mPlaceStatus.OnGround)
            {
                Debug.Log("Jump");
                mPlace = mPlaceStatus.InAir;
                Vector2 vel = mRigidbody.velocity;
                vel.y = mJumpForce;
                mRigidbody.velocity = vel;
            }
            else if (mJumpCount < mJumpSkill)
            {
                mPlace = mPlaceStatus.InAir;
                Vector2 vel = mRigidbody.velocity;
                vel.y = mJumpForce;
                mRigidbody.velocity = vel;
                mJumpCount++;
            }
            else
            {
                Debug.Log("Jump limit place:" + mPlace);
            }
        }
        //爬梯子
        if (mPlace == mPlaceStatus.OnLadder)
        {
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                Vector2 vel = mRigidbody.velocity;
                vel.y = mMaxSpeed;
                mRigidbody.velocity = vel;
            }
            if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            {
                Vector2 vel = mRigidbody.velocity;
                vel.y = -mMaxSpeed;
                mRigidbody.velocity = vel;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                Vector2 vel = mRigidbody.velocity;
                vel.y = 0;
                mRigidbody.velocity = vel;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                Vector2 vel = mRigidbody.velocity;
                vel.y = 0;
                mRigidbody.velocity = vel;
            }
        }
        //刷新面向
        if (mSpeed * (int)mFaceDir < 0)
        {
            mFaceDir = mFaceDir == mDirection.right ? mDirection.left : mDirection.right;
            gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
        }
        //冲刺
        if (Input.GetKeyDown(KeyCode.Q) && isDashAvalible())
        {
            mDashUsed = true;
            mIsDash = true;
            if (mJumpCount >= mJumpSkill)
                mJumpCount--;
            mRigidbody.velocity = new Vector2(75f * (float)mFaceDir, 0);
        }
        //刷新位置
        /*
        Vector3 CurrentLocalPosition = gameObject.transform.localPosition;
        CurrentLocalPosition.x += mSpeed * Time.smoothDeltaTime;
        gameObject.transform.localPosition = CurrentLocalPosition;
        */
        if (mIsDash)
        {
            mDashTimeCount += Time.smoothDeltaTime;
            if (mDashTimeCount >= mDashForce)
            {
                mIsDash = false;
                mDashTimeCount = 0;
            }
            mRigidbody.velocity = new Vector2(15f * (float)mFaceDir, 0);
        }
        else
        {
            Vector2 velocity = mRigidbody.velocity;
            velocity.x = mSpeed;
            mRigidbody.velocity = velocity;
        }
        if (respaned)
        {
            respawnTimer += Time.deltaTime;
            if (respawnTimer >= 2f)
            {
                respawnTimer = 0;
                respaned = false;
            }
        }
    }

    private bool isDashAvalible()
    {
        Debug.LogWarning(mPlace);
        if (((int)mPlace & 14) == 0)
            return false;
        return !mDashUsed;
    }

    private void touchGround()
    {
        mDashUsed = false;
        mJumpCount = 0;
        mPlace = mPlaceStatus.OnGround;
    }

    private void touchLadder()
    {
        mRigidbody.gravityScale = 0;
        mPlace = mPlaceStatus.OnLadder;
        mDashUsed = false;
    }

    private void touchWall(float _xNormal)
    {
        Vector2 velocity = mRigidbody.velocity;
        if (velocity.y <= 0)
            velocity.y = -mMaxSpeed / 2;
        mDashUsed = false;
        if (_xNormal > 0)
        {
            mPlace = mPlaceStatus.OnWallRight;
            velocity.x = velocity.x < 0 ? 0 : velocity.x;
        }
        else if (_xNormal < 0)
        {
            mPlace = mPlaceStatus.OnWallLeft;
            velocity.x = velocity.x > 0 ? 0 : velocity.x;
        }
        else
        {
            mPlace = mPlaceStatus.OnGround;
            return;
        }
        mRigidbody.velocity = velocity;

    }
    private void leftWall()
    {

    }

    private void leftLadder()
    {
        mRigidbody.gravityScale = 3;
        mPlace = mPlaceStatus.InAir;
        if (mJumpCount >= mJumpSkill)
        {
            mJumpCount--;
        }
    }

    private mDirection getMoveDirection()
    {
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            return mDirection.left;
        }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            return mDirection.right;
        }
        else
        {
            return mDirection.stop;
        }
    }

    private void OnCollisionEnter2D(Collision2D collisionobj)
    {
        Vector2 colliNormal = collisionobj.contacts[0].normal;
        switch (LayerMask.LayerToName(collisionobj.gameObject.layer))
        {
            case "Ground":
                //平面
                if (colliNormal.y > 0 && colliNormal.y > Mathf.Abs(colliNormal.x))
                {
                    touchGround();
                }
                else if (colliNormal.y < 0 && -colliNormal.y > Mathf.Abs(colliNormal.x))
                {
                    if (mPlace == mPlaceStatus.OnGround)
                    {
                        mForceSneak = true;
                    }
                }
                //侧面且没有站在地上
                else if (!(mPlace == mPlaceStatus.OnGround))
                {
                    touchWall(colliNormal.x);
                }
                break;
            case "Ladder":
                if (colliNormal.y > 0 && colliNormal.y > Mathf.Abs(colliNormal.x))
                {
                    touchGround();
                }
                else
                {
                    touchLadder();
                }
                break;
            case "Items":
            default:
                Debug.LogWarning("Unknow collision enter");
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D collisionobj)
    {
        foreach (ContactPoint2D colliPoint in collisionobj.contacts)
        {
            Vector2 colliNormal = colliPoint.normal;
            switch (LayerMask.LayerToName(collisionobj.gameObject.layer))
            {
                case "Ground":
                    mPlace = mPlaceStatus.InAir;
                    if (colliNormal.y > 0 && colliNormal.y > Mathf.Abs(colliNormal.x))
                    {

                        if (mPlace != mPlaceStatus.OnGround)
                        {
                            touchGround();
                        }
                    }
                    //顶到头了
                    else if (colliNormal.y < 0 && -colliNormal.y > Mathf.Abs(colliNormal.x))
                    {
                        if (mPlace == mPlaceStatus.OnGround)
                        {
                            mForceSneak = true;
                        }
                    }
                    else if (!(mPlace == mPlaceStatus.OnGround))
                    {
                        touchWall(colliNormal.x);
                    }
                    break;
                case "Ladder":
                    if (colliNormal.y > 0 && colliNormal.y > Mathf.Abs(colliNormal.x))
                    {
                        touchGround();
                    }
                    else
                    {
                        touchLadder();
                    }
                    break;
                default:
                    Debug.LogWarning("Unknow collision stay");
                    break;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collisionobj)
    {
        Debug.Log("Collision exit");
        switch (LayerMask.LayerToName(collisionobj.gameObject.layer))
        {
            case "Ground":
                if (mPlace == mPlaceStatus.OnWallRight || mPlace == mPlaceStatus.OnWallLeft)
                {
                    leftWall();
                }
                mPlace = mPlaceStatus.InAir;
                break;
            case "Ladder":
                leftLadder();
                break;
            default:
                Debug.LogWarning("Unknow collision exit");
                break;
        }
    }
    public bool IsRespawned()
    {
        return respawnTimer >= 0.001f;
    }
}