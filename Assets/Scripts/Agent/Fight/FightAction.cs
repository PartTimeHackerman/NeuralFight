using System;
using System.Collections.Generic;
using UnityEngine;

public class FightAction : MonoBehaviour
    {
        public int decFreq = 30;
        public int step = 0;
        private InternalModel model;
        public bool run = true;
        public FightPlayerObservations obsSelf;
        public FightObservationsForEnemy obsForEnemy;
        public FightPlayerActions act;
        public bool playerOne;
        private List<float> observations;
    
        protected virtual void Start()
        {
            SetUpModel();
        }

        public void SetUp(Fighter fighter, Fighter enemyFighter)
        {
            obsSelf.SetUp(fighter, enemyFighter);
            obsSelf.EnemyBodyParts = enemyFighter.BodyParts;
            obsSelf.enemyObservations = enemyFighter.FightAction.obsForEnemy;
            act.EnemyBodyParts = enemyFighter.BodyParts;
        }

        protected virtual void FixedUpdate()
        {
            if (run)
            {
                if (step >= decFreq)
                {
                    List<float> obs = obsSelf.GetObservations(playerOne);
                    observations = obs;
                    RunModel();
                    step = 0;
                }
                step++;
            }
        }

        protected virtual  void RunModel()
        {
            //List<float> observations = Observations.getObservations();
            List<float> actions = model.getActions(observations);
            act.setActions(actions);

        }

        protected virtual void SetUpModel()
        {
            model = new InternalModel("Models/Fight");
        }
    }