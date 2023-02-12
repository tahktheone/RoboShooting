using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RSGeneral
{
    public class RSComputer : RSUnit
    {
        private List<RSUnit> connectedUnits;
        private bool needToReset = true;

        public RSComputer()
        {
            TypeName = "computer";
        }

        public override void Start()
        {
            base.Start();
            connectedUnits = new List<RSUnit>();
        }

        public override void Update()
        {
            base.Update();
            if (needToReset)
            {
                connectedUnits = GetConnectedRSUnits();
                connectedUnits.Add(this);
                needToReset = false;
            }

            try
            {
                RSUnit radarUnit = connectedUnits.Find(unit => unit.TypeName == "radar");
                if (radarUnit != null)
                {
                    RSRadar radar = radarUnit as RSRadar;

                    float pd = radar.DistanceToPlayer();

                    Vector3 playerDirection = radar.FindPlayer();

                    List<RSUnit> weaponUnits = connectedUnits.FindAll(unit => unit.TypeName == "weapon");
                    foreach (RSUnit unit in weaponUnits)
                    {
                        RSWeapon weapon = unit as RSWeapon;
                        weapon.SetDirection(playerDirection);
                    }

                    List<RSUnit> caterpillarUnits = connectedUnits.FindAll(unit => unit.TypeName == "caterpillar");

                    Vector3 center = Vector3.zero;
                    foreach (RSUnit unit in caterpillarUnits)
                    {
                        center += unit.transform.position;
                    }
                    center /= caterpillarUnits.Count;

                    bool lForwardBlock = radar.CheckUnitsForward(connectedUnits);
                    bool lBackwardBlock = radar.CheckUnitsBackward(connectedUnits);

                    foreach (RSUnit caterpillarUnit in caterpillarUnits)
                    {
                        float d = Vector3.Dot(playerDirection, caterpillarUnit.transform.position - center); // farther or nearer to player
                        Vector3 caterpillarDirection = caterpillarUnit.transform.forward;
                        float dd = Vector3.Dot(playerDirection, caterpillarDirection); // to player or from player

                        RSCaterpillar caterpillar = caterpillarUnit as RSCaterpillar;

                        if (lForwardBlock & lBackwardBlock)
                        {
                            caterpillar.EngineForce = 0;
                            continue;
                        }

                        if (lForwardBlock)
                        {
                            caterpillar.EngineForce = -0.5F;
                            continue;
                        }

                        if (pd > 20.0F)
                        {
                            if (d > 0) // nearer
                            {
                                if (dd > 0)
                                { // to player
                                    caterpillar.EngineForce = dd * 4.0F - 3.0F;  //2.0F * (dd - 0.5F);
                                }
                                else
                                { // from player
                                    caterpillar.EngineForce = -1.0F;
                                }
                            }
                            else
                            { //farther
                                if (dd > 0)
                                { // to player
                                    caterpillar.EngineForce = 1.0F;
                                }
                                else
                                { // from player
                                    caterpillar.EngineForce = 1.0F;
                                }
                            }// farther or nearer to player
                        }
                        else
                        { //if (pd < 20.0F)
                            caterpillar.EngineForce = 0.0F;
                        } //if (pd > 10.0F)
                    } // foreach (RSUnit caterpillarUnit in caterpillarUnits)
                } //if (radarUnit != null)
            } // try
            catch
            {
                needToReset = true;
            } // catch
        } // public void Update()
    }    
}