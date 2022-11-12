using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Movement_controller : MonoBehaviour
{



    /*
     *so apparently the next is just about locking onto targets
     *
     *ther eis an issue where the 
     *
     *
     */


    public ParticleSystem Foot_L;
    public ParticleSystem Foot_R;
    public ParticleSystem double_jump_ptr;




    public Rigidbody rb_self;
    public Transform player_tra;
    private float movementX;
    private float movementY;


    private float rotationX;
    private float rotationY;

    private player_stats player;
    private GameManager manager;
    public Camera camera;
    public rotating rot;
    public Animator anim;

    public GameObject mid_player;    // distance from the ground is 2.137868


    [Range(0.5f, 10)]
    public float jump_force = 10;

    private bool grounded;

    private int incline_layerMask = 1 << 7;   // WIP

    private bool running = false;


    public GameObject rot_obj;


    public bool follow = false;
    public enemy_detection enemy_det;
    public GameObject enemy = null;



    private bool show_controllers = false;

    private float combo_timer_rate = 3f;
    private float combo_timer_recorded;
    private bool fight_on = false;
    private int fight_move = 0;


    private float speed = 4;



    private bool increased_speed = false;
    private float incr_speed_time_act;
    private float anim_speed_increment = 0f;


    private bool double_jump = false;
    private float double_jump_time_act;

    private int curr_jump_amount = 0;
    private float double_jump_time_offset;


    private bool debug = false;
    public bool action = false;


    // all movement states
    private enum action_state
    {
        IDLE = 1,
        FORWARD = 2,
        BACKWARDS = 3,
        LEFT = 4,
        RIGHT = 5,
        LEFT_FRONT = 6,
        RIGHT_FRONT = 7,
        LEFT_BACK = 8,
        RIGHT_BACK = 9,
        ROLLING = 10,
        JUMP = 11,
        FALL = 12,
        COMBO_1 = 13,
        COMBO_2 = 14,
        COMBO_3 = 15,
        RUNNING = 16,
        INTERACTION = 17
    }
    private action_state move_action;


    // different weapons the player can have WIP
    private enum weapon_type
    {
        UNARMED = 1,
        SWORD = 2
    }
    private weapon_type weapon_selected;


    // state of the player character  WIP
    private enum status_state
    {
        ALIVE = 1,
        HURT = 2,
        DEAD = 3
    }
    private status_state player_state;



    private void Awake()
    {
        player = FindObjectOfType<player_stats>();
        anim = GetComponent<Animator>();
        manager = FindObjectOfType<GameManager>();
        rot = FindObjectOfType<rotating>();
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }
    private void OnLook(InputValue lookValue)
    {
        Vector2 movementVector = lookValue.Get<Vector2>();

        rotationX = movementVector.x;
        rotationY = movementVector.y;

    }
    private void OnJump(InputValue jumpCall)
    {
        if (grounded && manager.curr_game_state == GameManager.game_state.GAME)
        {
            curr_jump_amount = 0;
            move_action = action_state.JUMP;
            rb_self.AddForce(Vector3.up * jump_force, ForceMode.Impulse);
            anim.speed = 1;
            anim.SetInteger("State", (int)move_action);

            if (double_jump && curr_jump_amount == 0)
            {
                curr_jump_amount++;
                double_jump_time_offset = Time.time + 0.3f;
            }
        }

        if (double_jump_time_offset <= Time.time && double_jump && curr_jump_amount == 1)
        {}


        if (double_jump && manager.curr_game_state == GameManager.game_state.GAME && double_jump_time_offset <= Time.time && curr_jump_amount == 1)
        {
            rb_self.velocity = new Vector3(rb_self.velocity.x, 0, rb_self.velocity.z);
            move_action = action_state.JUMP;
            rb_self.AddForce(Vector3.up * jump_force, ForceMode.Impulse);
            anim.speed = 1;
            anim.SetInteger("State", (int)move_action);

            curr_jump_amount = 0;
        }
    }
    private void OnPause(InputValue pauseCall)
    {
        if (manager.curr_game_state == GameManager.game_state.GAME)
        {
            manager.curr_game_state = GameManager.game_state.PAUSE;
        }
        else
        {
            manager.curr_game_state = GameManager.game_state.GAME;
        }
    }
    private void OnLockOn(InputValue lockonCall)
    {
        follow = !follow;
        if (follow)
        {
            if (enemy_det.get_size() > 0)
            {
                enemy_det.Start_tracking();
                camera.transform.parent = this.transform;
            }
            else
            {
                follow = false;
                enemy_det.End_tracking();
                camera.transform.parent = null;
            }
        }
        else
        {
            enemy_det.End_tracking();
            camera.transform.parent = null;
        }
    }
    private void OnRun(InputValue RunCall)
    {
        if (RunCall.isPressed)
        {
            running = true;
        }
        else
        {
            running = false;
        }
    }
    private void OnAttack(InputValue attackCall)
    {
        if (grounded && manager.curr_game_state == GameManager.game_state.GAME)
        {
            if (Mathf.Abs(movementX) <= 0.01f || Mathf.Abs(movementY) <= 0.01f)
            {

                fight_on = true;
                combo_timer_recorded = Time.time + combo_timer_rate;

                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Unarmed-Attack-R1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Unarmed-Attack-L2") || anim.GetCurrentAnimatorStateInfo(0).IsName("Unarmed-Attack-R3"))
                {
                }
                else
                {
                    anim.speed = 1;
                    fight_move += 1;

                    switch (fight_move)
                    {
                        case 1:
                            move_action = action_state.COMBO_1;
                            //Attack_ptr.startColor = Color.blue;
                            //Attack_ptr.Play();
                            break;

                        case 2:
                            move_action = action_state.COMBO_2;
                            //Attack_ptr.startColor = Color.magenta;
                            //Attack_ptr.Play();
                            break;

                        case 3:

                            move_action = action_state.COMBO_3;
                            //Attack_ptr.startColor = Color.red;
                            //Attack_ptr.Play();
                            fight_move = 0;
                            fight_on = false;
                            break;
                    }

                    anim.SetInteger("State", (int)move_action);
                }

            }

        }
    }
    private void OnRoll(InputValue rollCall)
    {

        if (Mathf.Abs(movementX) >= 0.01f || Mathf.Abs(movementY) >= 0.01f)
        {
            if (move_action == action_state.FORWARD)
            {
                anim.speed = 1;
                move_action = action_state.ROLLING;
                var camera = Camera.main;

                var forward = camera.transform.forward;
                var right = camera.transform.right;

                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();

                var desiredMoveDirection = forward * movementY + right * movementX;


                rb_self.AddForce(desiredMoveDirection * jump_force, ForceMode.Impulse);
                anim.SetInteger("State", (int)move_action);
            }
        }
    }
    private void OnCameraMovement(InputValue CameraMovementValue)
    {
        float value = CameraMovementValue.Get<float>();

        if (follow)
        {

            switch (value)
            {

                case -1:
                    enemy_det.New_enemy((int)value);

                    break;

                case 1:
                    enemy_det.New_enemy((int)value);
                    break;

                case 0:
                    break;

                default:

                    break;
            }
        }
        else
        {
            switch (value)
            {

                case -1:
                    rot.QuickTurn(value);
                    break;

                case 1:

                    rot.QuickTurn(value);
                    break;

                case 0:
                    break;

                default:

                    break;
            }
        }
    }
    private void OnAction(InputValue ActionCall)
    {
        if (follow)
        {

        }
        else
        {
            if (ActionCall.isPressed)
            {
                action = !action;
            }
        }
    }
    private void OnInstruction(InputValue InstructionCall) { show_controllers = !show_controllers; }


    public void interaction_call()
    {
        Debug.Log("this is interacting");
        move_action = action_state.INTERACTION;

        anim.SetInteger("State", (int)move_action);
    }


    public void SpeedBoostcall()
    {
        increased_speed = true;
        incr_speed_time_act = Time.time + 10f;

        anim_speed_increment = 1;
        speed = 8;

        Foot_L.startColor = Color.red;
        Foot_R.startColor = Color.red;

        Foot_R.emissionRate = 8;
        Foot_L.emissionRate = 8;

    }
    public void DoubleJumpcall()
    {
        double_jump_ptr.Play();
        double_jump = true;
        double_jump_time_act = Time.time + 20f;
    }
    private void powerUpsChecks()
    {
        if (increased_speed)
        {
            if (incr_speed_time_act <= Time.time)
            {
                increased_speed = false;

                Foot_L.startColor = new Color(0.708f, 0.333f, 0.097f, 1f);
                Foot_R.startColor = new Color(0.708f, 0.333f, 0.097f, 1f);

                Foot_R.emissionRate = 0;
                Foot_L.emissionRate = 0;

                speed = 4;

                anim_speed_increment = 0f;
            }


        }
        if (double_jump)
        {
            if (double_jump_time_act <= Time.time)
            {
                double_jump_ptr.Stop();
                double_jump = false;
            }
        }
    }

    private bool in_beetween(float A, float B, float X)
    {

        if (X >= A && X <= B)
        {
            return true;
        }

        return false;
    }
    private void lat_movement(Vector3 desiredMoveDirection)
    {
        float hyp = Mathf.Sqrt((movementX * movementX) + (movementY * movementY));
        anim.speed = hyp + anim_speed_increment;
        player_tra.Translate(desiredMoveDirection * (speed * Mathf.Sqrt((movementX * movementX) + (movementY * movementY))) * Time.deltaTime);
    }


    private void OnGUI()
    {

        

        GUI.Box(new Rect(0, 0, 300, 300), "CONTROLS \n" +
                " ----------- \n" +
                " CONTROLLER \n " +
                " A = Jump \n" +
                " B = Attack \n" +
                " X = Forward roll \n" +
                " Y = Lock-on target \n" +
                " D-PAD LEFT/RIGHT = Change target  \n\n" +
                " LEFT THUMB-STICK = Movement \n" +
                " pressed LTS = Run \n" +
                " RIGHT THUMB-STICK = Camera control \n\n" +
                " D-PAD LEFT/RIGHT = Quick camera movements \n" +
                " D-PAD UP = Interact \n" +
                " START = Pause game\n" +
                " ----------- \n" +
                " THIS DEMO WAS MADE FOR GAMEPAD ONLY \n" +
                "");

        
        
    }
    void FixedUpdate()
    {
        if (manager.curr_game_state == GameManager.game_state.GAME)
        {

            powerUpsChecks();


            //Debug.Log(curr_jump_amount);



            // time for the combo strikes
            if (combo_timer_recorded <= Time.time && fight_on)
            {
                fight_on = false;
                fight_move = 0;
            }


            RaycastHit hit;

            // checks for the player grounded bool
            if (Physics.Raycast(mid_player.transform.position, mid_player.transform.TransformDirection(Vector3.down), out hit, 2.4f))
            {
                grounded = true;
                if (Foot_L.isStopped)
                {
                    Foot_L.Play();
                }
                if (Foot_R.isStopped)
                {
                    Foot_R.Play();
                }
            }
            else
            {

                if (Foot_L.isPlaying)
                {
                    Foot_L.Stop();
                }
                if (Foot_R.isPlaying)
                {
                    Foot_R.Stop();
                }

                grounded = false;
            }

            // WIP (work in progress)
            if (Physics.Raycast(mid_player.transform.position, mid_player.transform.TransformDirection(Vector3.down), out hit, 2.17f, incline_layerMask))    //work in progress
            {
                Debug.Log(hit.transform.rotation.x);
            }




            var camera = Camera.main;





            if (follow)
            {
                Vector3 look_at_pos = new Vector3(enemy.transform.position.x, this.transform.position.y, enemy.transform.position.z);

                //hoenslty this works somehohw
                this.transform.LookAt(look_at_pos);
                //camera.transform.LookAt(look_at_pos);



                //camera.transform.rotation = this.transform.rotation;
                camera.transform.parent = this.transform;
            }

            var forward = camera.transform.forward;
            var right = camera.transform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            var desiredMoveDirection = forward * movementY + right * movementX;



            //rot.CallMove(rotationX);

            if (!follow)
            {
                if (Mathf.Abs(movementX) >= 0.01f || Mathf.Abs(movementY) >= 0.01f)
                {
                    transform.forward = forward;
                }
                camera.transform.parent = null;
                rot.CallMove(rotationX);
            }



            if (move_action != action_state.ROLLING)
            {
                //Linear movement
                if (in_beetween(-0.35f, 0.35f, movementY) && movementX >= 0.1f)
                {
                    player_tra.Translate(desiredMoveDirection * (speed * Mathf.Abs(movementX)) * Time.deltaTime);

                    move_action = action_state.RIGHT;
                    anim.speed = Mathf.Abs(movementX) + anim_speed_increment;
                }
                else if (in_beetween(-0.35f, 0.35f, movementY) && movementX <= -0.1f)
                {
                    player_tra.Translate(desiredMoveDirection * (speed * Mathf.Abs(movementX)) * Time.deltaTime);


                    move_action = action_state.LEFT;
                    anim.speed = Mathf.Abs(movementX) + anim_speed_increment;
                }
                else if (in_beetween(-0.35f, 0.35f, movementX) && movementY >= 0.1f)
                {
                    if (!running)
                    {
                        move_action = action_state.FORWARD;

                        player_tra.Translate(desiredMoveDirection * (speed * Mathf.Abs(movementY)) * Time.deltaTime);
                    }
                    else
                    {
                        move_action = action_state.RUNNING;


                        player_tra.Translate(desiredMoveDirection * (speed * 2 * Mathf.Abs(movementY)) * Time.deltaTime);
                    }
                    anim.speed = Mathf.Abs(movementY) + anim_speed_increment;
                }
                else if (in_beetween(-0.35f, 0.35f, movementX) && movementY <= -0.1f)
                {
                    move_action = action_state.BACKWARDS;

                    player_tra.Translate(desiredMoveDirection * (speed * Mathf.Abs(movementY)) * Time.deltaTime);
                    anim.speed = Mathf.Abs(movementY) + anim_speed_increment;
                }

                //diagonal movement
                else if (movementX >= 0.35f && movementY >= 0.35f)
                {
                    move_action = action_state.RIGHT_FRONT;

                    lat_movement(desiredMoveDirection);

                }
                else if (movementX <= -0.35f && movementY >= 0.35f)
                {
                    move_action = action_state.LEFT_FRONT;

                    lat_movement(desiredMoveDirection);
                }
                else if (movementX <= -0.35f && movementY <= -0.35f)
                {
                    move_action = action_state.LEFT_BACK;
                    lat_movement(desiredMoveDirection);
                }
                else if (movementX >= 0.35f && movementY <= -0.35f)
                {
                    move_action = action_state.RIGHT_BACK;
                    lat_movement(desiredMoveDirection);
                }
                //idle
                else
                {
                    move_action = action_state.IDLE;
                    anim.speed = 1;
                }

                // falling and jumping state detection for animator
                if (!grounded)
                {
                    if (rb_self.velocity.y > 0)
                    { // going up
                        move_action = action_state.JUMP;
                    }
                    else if (rb_self.velocity.y <= 0)
                    {
                        move_action = action_state.FALL;
                    }
                }
            }
            else
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("DiveRoll-Forward"))
                {
                    move_action = action_state.IDLE;
                }
            }
            anim.SetInteger("State", (int)move_action);
        }
        else if (manager.curr_game_state == GameManager.game_state.CUTSCENE)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Unarmed-Attack-L3"))
            {

            }
            else
            {

                move_action = action_state.IDLE;

                anim.SetInteger("State", (int)move_action);
            }

        }
    }



}
