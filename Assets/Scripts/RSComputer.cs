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

        public void Start()
        {
            connectedUnits = new List<RSUnit>();
        }

        public void Update()
        {
            if (needToReset)
            {
                connectedUnits = GetConnectedRSUnits();
                needToReset = false;
            }

            RSUnit radarUnit = connectedUnits.Find(unit => unit.TypeName == "radar");
            if (radarUnit != null)
            {
                RSRadar radar = radarUnit as RSRadar;

                float pd = radar.DistanceToPlayer();

                Vector3 playerDirection = radar.FindPlayer();

                List<RSUnit> caterpillarUnits = connectedUnits.FindAll(unit => unit.TypeName == "caterpillar");

                Vector3 center = Vector3.zero;
                foreach (RSUnit unit in caterpillarUnits)
                {
                    center += unit.transform.position;
                }
                center /= caterpillarUnits.Count;

                foreach (RSUnit caterpillarUnit in caterpillarUnits)
                {
                    float d = Vector3.Dot(playerDirection, caterpillarUnit.transform.position - center); // farther or nearer to player
                    Vector3 caterpillarDirection = caterpillarUnit.transform.forward;
                    float dd = Vector3.Dot(playerDirection, caterpillarDirection); // to player or from player

                    RSCaterpillar caterpillar = caterpillarUnit as RSCaterpillar;

                    if (pd > 20.0F)
                    {
                        if (d > 0) // nearer
                        {
                            if (dd > 0)
                            { // to player
                                caterpillar.EngineForce = 2.0F * (dd - 0.5F);
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
        } // public void Update()
    }
    
}