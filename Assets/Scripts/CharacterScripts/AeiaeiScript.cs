using UnityEngine;

namespace Characters
{
    public class AeiaeiScript : Champion
    {
        public override void LoadBasicStats()
        {
            MaxHealth = 670;
            AttackDamage = 69; //Dla testów!
            AbilityPower = 0;
            Armor = 69; // Dla testów!
            MagicResist = 56;
            MaxMana = 100;
            ManaType = ManaTypes.Stamina;
            MovementSpeed = 330;
            AttackRange = 4f; // Dla testów!
            AttackSpeed = 0.64f;
            CriticalChance = 30; // Dla testów!

        }

        public override void CharacterUpdate()
        {
            
        }

        int _aastate = 0;
        public override void Animations()
        {
            Debug.Log(CurrentAnimation);
            if (!AnimationRun)
            {
                float _attackspeed = Mathf.Clamp(AttackSpeed, 1, 2);
                charactercontroller.Anim.SetFloat("AttackSpeed", _attackspeed / 1.4f);

                charactercontroller.Anim.ResetTrigger("Stay");
                charactercontroller.Anim.ResetTrigger("Movement");

                if (_aastate != 0 && CurrentAnimation != AnimState.BasicAttack) _aastate = 0;

                if (CurrentAnimation == AnimState.Stay) charactercontroller.Anim.SetTrigger("Stay");
                else if (CurrentAnimation == AnimState.Movement) charactercontroller.Anim.SetTrigger("Movement");
                else if (CurrentAnimation == AnimState.BasicAttack && !AACooldown)
                {
                    if (!NextAACrit)
                    {
                        AnimationRun = true;
                        Attack(AttackTarget, false);
                        switch (_aastate)
                        {
                            case 0:
                                charactercontroller.Anim.CrossFade("AA1", 0.2f, 0, 0);
                                _aastate++;
                                break;
                            case 1:
                                charactercontroller.Anim.CrossFade("AA2", 0.2f, 0, 0);
                                _aastate++;
                                break;
                            case 2:
                                charactercontroller.Anim.CrossFade("AA3", 0.2f, 0, 0);
                                _aastate++;
                                break;
                            case 3:
                                charactercontroller.Anim.CrossFade("AA4", 0.2f, 0, 0);
                                _aastate++;
                                break;
                            case 4:
                                charactercontroller.Anim.CrossFade("AA5", 0.2f, 0, 0);
                                _aastate = 1;
                                break;
                        }
                        charactercontroller.StartCoroutine(AATimerCooldown(1 / AttackSpeed));
                    }
                    else
                    {
                        AnimationRun = true;
                        Attack(AttackTarget, true);
                        charactercontroller.Anim.CrossFade("Critical", 0.3f, 0, 0);
                        charactercontroller.StartCoroutine(AATimerCooldown(1 / AttackSpeed));
                    }
                }
            }
            if (CurrentAnimation != AnimState.Stay)
                if (charactercontroller.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
                    AnimationRun = false;
        }

        //Używanie umiejętności

        public override void UseFirstAbility()
        {
            throw new System.NotImplementedException();
        }

        public override void UseSecondAbility()
        {
            throw new System.NotImplementedException();
        }

        public override void UseThirdAbility()
        {
            throw new System.NotImplementedException();
        }

        public override void UseUltimate()
        {
            throw new System.NotImplementedException();
        }

        //Zwracanie opisów umiejętności

        public override string PassiveDesc => throw new System.NotImplementedException();

        public override string FirstAbilityDesc => throw new System.NotImplementedException();

        public override string SecondAbilityDesc => throw new System.NotImplementedException();

        public override string ThirdAbilityDesc => throw new System.NotImplementedException();

        public override string UltimateDesc => throw new System.NotImplementedException();
    }
}