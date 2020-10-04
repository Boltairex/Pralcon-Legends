using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Characters
{
    public sealed class AeiaeiScript : Champion
    {
        public AeiaeiScript()
        {
            basicMaxHealth = 670;
            basicMaxMana = 100;
            basicMovementSpeed = 330;
            basicArmor = 64;
            basicMagicResist = 56;
            basicAttackDamage = 59;
            basicAttackSpeed = 2;
            basicAttackRange = 4;
            basicHealthRegen = 2;
            basicManaRegen = 5;

            basicMaxHealthPerLevel = 30;
            basicMaxManaPerLevel = 5;
            basicMovementSpeedPerLevel = 3;
            basicArmorPerLevel = 2;
            basicMagicResistPerLevel = 2;
            basicAttackDamagePerLevel = 3;
            basicAttackSpeedPerLevel = 0.02f;
            basicAttackRangePerLevel = 0;
            basicHealthRegenPerLevel = 0.1f;
            basicManaRegenPerLevel = 0.5f;

            AbilityInfo QAI = new AbilityInfo()
            {
                BasicPower = 100,
                BasicPowerPerLevel = 20,

                Level = 1,
                MaxUpgradeLevel = 5,
                Type = DamageType.AD,

                BasicCooldown = 2,
                CooldownPerLevel = 2,
            };

            QInfo = QAI;

            AbilityInfo WAI = new AbilityInfo()
            {
                BasicPower = 100,
                BasicPowerPerLevel = 20,

                Level = 1,
                MaxUpgradeLevel = 5,
                Type = DamageType.AD,

                BasicCooldown = 10f,
                CooldownPerLevel = 2,
            };

            WInfo = WAI;

            AbilityInfo EAI = new AbilityInfo()
            {
                BasicPower = 100,
                BasicPowerPerLevel = 20,

                Level = 1,
                MaxUpgradeLevel = 5,
                Type = DamageType.AD,

                BasicCooldown = 2,
                CooldownPerLevel = 2,
            };

            EInfo = EAI;

            AbilityInfo RAI = new AbilityInfo()
            {
                BasicPower = 100,
                BasicPowerPerLevel = 20,

                Level = 1,
                MaxUpgradeLevel = 3,
                Type = DamageType.AD,

                BasicCooldown = 10f,
                CooldownPerLevel = 2,
            };

            RInfo = RAI;
        }

        public GameObject Weapon1;
        public bool HaveHead;

        public override void OnChampionStart()
        {
            Weapon1 = Resources.Load<GameObject>("Champions/Aeiaei/Little_Scythe");
            HaveHead = true;
        }

        public override void CharacterUpdate()
        {
            
        }

        int aastate = 0;
        public override void Animations()
        {
            Debug.Log(CurrentAnimation);
            if (!AnimationRun)
            {
                float _movementspeed = MovementSpeed / 400;
                float _attackspeed = Mathf.Clamp(AttackSpeed, 1, 2);

                _movementspeed = Mathf.Clamp(_movementspeed, 0.9f, 2);

                charactercontroller.Anim.ResetTrigger("Stay");
                charactercontroller.Anim.ResetTrigger("Movement");

                charactercontroller.Anim.SetFloat("MovementSpeed", _movementspeed);
                charactercontroller.Anim.SetFloat("AttackSpeed", _attackspeed / 1.4f);

                if (aastate != 0 && CurrentAnimation != AnimState.BasicAttack) aastate = 0;

                if (HaveHead)
                {
                    if (CurrentAnimation == AnimState.Stay)
                    {
                        if(!charactercontroller.Anim.GetCurrentAnimatorStateInfo(0).IsName("Stay"))
                            charactercontroller.Anim.CrossFade("Stay", 1, 0, 0.5f);
                        charactercontroller.Anim.SetTrigger("Stay");
                    }
                    else if (CurrentAnimation == AnimState.Movement)
                    {
                        charactercontroller.Anim.SetTrigger("Movement");
                    }
                    else if (CurrentAnimation == AnimState.BasicAttack && !AACooldown && AAState == BasicAttackState.None)
                    {
                        charactercontroller.transform.LookAt(AttackTarget);
                        if (!NextAACrit && !CancelNextAutoattack)
                        {
                            switch (aastate)
                            {
                                case 0:
                                    charactercontroller.Anim.CrossFade("AA1", 0.3f, 0, 0);
                                    SetAASettings(0.15f, 0.15f);
                                    aastate++;
                                    break;
                                case 1:
                                    charactercontroller.Anim.CrossFade("AA2", 0.3f, 0, 0);
                                    SetAASettings(0.15f, 0.15f);
                                    aastate++;
                                    break;
                                case 2:
                                    charactercontroller.Anim.CrossFade("AA3", 0.3f, 0, 0);
                                    SetAASettings(0.15f, 0.15f);
                                    aastate++;
                                    break;
                                case 3:
                                    charactercontroller.Anim.CrossFade("AA4", 0.3f, 0, 0);
                                    SetAASettings(0.15f, 0.15f);
                                    aastate++;
                                    break;
                                case 4:
                                    charactercontroller.Anim.CrossFade("AA5", 0.3f, 0, 0);
                                    SetAASettings(0.15f, 0.15f);
                                    aastate = 1;
                                    break;
                            }
                        }
                        else if (NextAACrit && !CancelNextAutoattack)
                        {
                            SetAASettings(0.2f,0.2f);
                            charactercontroller.Anim.CrossFade("Critical", 0.3f, 0, 0);
                        }
                        else if (CancelNextAutoattack)
                        {
                            CurrentAnimation = AnimState.Movement;
                            IsAttacking = true;
                            CancelNextAutoattack = false;
                        }
                    }
                }
                else
                { }// Miejsce na brak głowy
            }
        }

        //Używanie umiejętności

        public override void UseFirstAbility()
        {
            CurrentAnimation = AnimState.QAbility;
            RotateTowardCursor();
            if (HaveHead)
                charactercontroller.Anim.CrossFade("QAbility", 0.2f, 0, 0);
            else
                charactercontroller.Anim.CrossFade("Q2Ability", 0.5f, 0, 0);

            var W = charactercontroller.LocalInstantiate(Weapon1);
            W.SetActive(false);
            currentSpells.Add(new SpellSystem(new SpellInfo {
                Owner = This,
                Power1 = QAbilityDmg,
                Speed = 40,
                ReturnSpeed = 50f,
                CanTargetMinion = true,
                CanTargetTower = false,
                DType = QInfo.Type,
                ActivateTime = 0.20f,
                Target = Cursor.WorldPointer,
                MaxDistance = 15,
                Type = SpellType.ThrowAndGrab,
                Delay = 0.25f,
            },W));
            charactercontroller.StartCoroutine(AnimationTime(1.0f));
        }

        public override void UseSecondAbility()
        {
            //Tu animacji nie miała
        }

        public override void UseThirdAbility()
        {
            CurrentAnimation = AnimState.EAbility;
            RotateTowardCursor();
            DashToPoint(Cursor.WorldPointer, 16, 22);
            if (HaveHead)
            {
                charactercontroller.Anim.CrossFade("EAbility", 0.5f, 0, 0);
                currentSpells.Add(new SpellSystem(new SpellInfo
                {
                    Owner = This,
                    Power1 = EAbilityDmg,
                    CanTargetMinion = true,
                    CanTargetTower = false,
                    DType = EInfo.Type,
                    ActivateTime = 0.3f,
                    ScanDistance = 4,
                    Type = SpellType.DamageInCircle,
                    Delay = 0.40f,
                }, null));
            }
            else
                charactercontroller.Anim.CrossFade("E2Ability", 0.5f, 0, 0);

            charactercontroller.StartCoroutine(AnimationTime(0.8f));
        }

        public override void UseUltimate()
        {
            CurrentAnimation = AnimState.RAbility;
            RotateTowardCursor();
            if (HaveHead) // Tu inny warunek sie da ale póki co niech zostanie
            {
                charactercontroller.Anim.CrossFade("Ultimate", 0.5f, 0, 0);

                currentSpells.Add(new SpellSystem(new SpellInfo
                {
                    Owner = This,
                    Power1 = RAbilityDmg,
                    CanTargetMinion = true,
                    CanTargetTower = false,
                    DType = RInfo.Type,
                    ActivateTime = 0.2f,
                    ScanDistance = 12,
                    Type = SpellType.DamageInCircle,
                    Delay = 0.1f,
                }, null));
            }
            else
                charactercontroller.Anim.CrossFade("Ultimate2", 0.5f, 0, 0);

            charactercontroller.StartCoroutine(AnimationTime(0.7f));
        }

        public override float QAbilityDmg => QInfo.BasicPower + QInfo.Level * QInfo.BasicPowerPerLevel + AbilityPower;

        public override float WAbilityDmg => throw new System.NotImplementedException();

        public override float EAbilityDmg => EInfo.BasicPower + EInfo.Level * EInfo.BasicPowerPerLevel + AbilityPower;

        public override float RAbilityDmg => RInfo.BasicPower + RInfo.Level * RInfo.BasicPowerPerLevel + AbilityPower;

        //Zwracanie opisów umiejętności

        public override string PassiveDesc => throw new System.NotImplementedException();

        public override string FirstAbilityDesc => throw new System.NotImplementedException();

        public override string SecondAbilityDesc => throw new System.NotImplementedException();

        public override string ThirdAbilityDesc => throw new System.NotImplementedException();

        public override string UltimateDesc => throw new System.NotImplementedException();
    }
}