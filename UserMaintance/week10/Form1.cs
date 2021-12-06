﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldsHardestGame;

namespace week10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ga = gc.ActivateDisplay();
            this.Controls.Add(ga);

            gc.GameOver += Gc_GameOver;

            for (int i = 0; i < populationSize; i++)
            {
                gc.AddPlayer(nbOfSteps);
            }
            gc.Start(true);

        }

        private void Gc_GameOver(object sender)
        {
            generation++;
            label1.Text = string.Format("{0}. generacio", generation);

            var playerList = from p in gc.GetCurrentPlayers()
                             orderby p.GetFitness() descending
                             select p;

            var topPerformers = playerList.Take(populationSize / 2).ToList();

            var winners = from p in topPerformers
                          where p.IsWinner
                          select p;
            if (winners.Count() > 0)
                {

                winnerBrain = winners.FirstOrDefault().Brain.Clone();
                gc.GameOver -= Gc_GameOver;
                return;

            }


            gc.ResetCurrentLevel();
            foreach (var p in topPerformers)
            {
                var b = p.Brain.Clone();
                if (generation % 3 == 0)
                    gc.AddPlayer(b.ExpandBrain(nbOfStepsIncrement));
                else
                    gc.AddPlayer(b);

                if (generation % 3 == 0)
                    gc.AddPlayer(b.Mutate().ExpandBrain(nbOfStepsIncrement));
                else
                    gc.AddPlayer(b.Mutate());
            }

            gc.Start();
        }

        private void Gc_GameOver1(object sender)
        {
            throw new NotImplementedException();
        }

        Brain winnerBrain = null;

        GameController gc = new GameController();
        GameArea ga = new GameArea();

        int populationSize = 100;
        int nbOfSteps = 10;
        int nbOfStepsIncrement = 10;
        int generation = 1;
    }
}