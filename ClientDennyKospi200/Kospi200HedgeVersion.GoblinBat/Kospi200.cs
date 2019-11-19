﻿using System;
using System.Drawing;
using System.Windows.Forms;
using ShareInvest.Analysize;
using ShareInvest.Const;
using ShareInvest.Controls;
using ShareInvest.EventHandler;
using ShareInvest.FindByName;
using ShareInvest.OpenAPI;
using ShareInvest.SecondaryForms;

namespace ShareInvest.Kospi200HedgeVersion
{
    public partial class Kospi200 : Form
    {
        public Kospi200()
        {
            InitializeComponent();
            ChooseStrategy(new ChooseAnalysis(), new SelectStrategies());
            Dispose();
            Environment.Exit(0);
        }
        private void ChooseStrategy(ChooseAnalysis analysis, SelectStrategies strategy)
        {
            analysis.SendClose += strategy.OnReceiveClose;
            strategy.OnReceiveClose(analysis.Key.Split('^'));
            splitContainerStrategy.Panel1.Controls.Add(analysis);
            splitContainerStrategy.Panel2.Controls.Add(strategy);
            analysis.Dock = DockStyle.Fill;
            strategy.Dock = DockStyle.Fill;
            Size = new Size(1650, 920);
            splitContainerStrategy.SplitterDistance = 287;
            splitContainerStrategy.BackColor = Color.FromArgb(121, 133, 130);
            strategy.SendClose += OnReceiveClose;
            ShowDialog();
        }
        private void StartTrading(Balance bal, ConfirmOrder order, AccountSelection account, ConnectKHOpenAPI api)
        {
            Controls.Add(api);
            splitContainerBalance.Panel1.Controls.Add(order);
            splitContainerBalance.Panel2.Controls.Add(bal);
            api.Dock = DockStyle.Fill;
            order.Dock = DockStyle.Fill;
            bal.Dock = DockStyle.Fill;
            bal.BackColor = Color.FromArgb(203, 212, 206);
            order.BackColor = Color.FromArgb(121, 133, 130);
            api.Hide();
            account.SendSelection += OnReceiveAccount;
            splitContainerBalance.Panel2MinSize = 3;
            splitContainerBalance.Panel1MinSize = 96;
            order.SendTab += OnReceiveTabControl;
            bal.SendReSize += OnReceiveSize;
            ResumeLayout();
        }
        private void OnReceiveClose(object sender, DialogClose e)
        {
            SuspendLayout();
            StartTrading(Balance.Get(), ConfirmOrder.Get(), new AccountSelection(), new ConnectKHOpenAPI());
            strategy = new Strategy(new Specify
            {
                Reaction = e.Reaction,
                ShortDayPeriod = e.ShortDay,
                ShortTickPeriod = e.ShortTick,
                LongDayPeriod = e.LongDay,
                LongTickPeriod = e.LongTick,
                HedgeType = e.Hedge
            });
        }
        private void OnReceiveAccount(object sender, Account e)
        {
            account.Text = e.AccNo;
            id.Text = e.ID;
            ConnectAPI api = ConnectAPI.Get();
            api.SendDeposit += OnReceiveDeposit;
            api.LookUpTheDeposit(e.AccNo, true);
        }
        private void OnReceiveDeposit(object sender, Deposit e)
        {
            for (int i = 0; i < e.ArrayDeposit.Length; i++)
                if (e.ArrayDeposit[i].Length > 0)
                    string.Concat("balance", i).FindByName<Label>(this).Text = long.Parse(e.ArrayDeposit[i]).ToString("N0");

            splitContainerAccount.BackColor = Color.FromArgb(121, 133, 130);
            tabControl.SelectedIndex = 1;
            strategy.SetAccount(new InQuiry { AccNo = account.Text, BasicAssets = long.Parse(e.ArrayDeposit[20]) });
        }
        private void OnReceiveSize(object sender, GridReSize e)
        {
            splitContainerBalance.SplitterDistance = splitContainerBalance.Height - e.ReSize - splitContainerBalance.SplitterWidth;
            timer.Stop();
            CenterToScreen();

            if (e.Count < 8)
                FormSizes[2, 1] = 315;

            else if (e.Count > 7)
                FormSizes[2, 1] = 328 + (e.Count - 7) * 21;
        }
        private void OnReceiveTabControl(object sender, Mining e)
        {
            tabControl.SelectedIndex = e.Tab;
        }
        private void TabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            Size = new Size(FormSizes[tabControl.SelectedIndex, 0], FormSizes[tabControl.SelectedIndex, 1]);
            splitContainerBalance.AutoScaleMode = AutoScaleMode.Font;
            CenterToScreen();
        }
        private void ServerCheckedChanged(object sender, EventArgs e)
        {
            if (CheckCurrent)
            {
                server.Text = "During Auto Renew";
                server.ForeColor = Color.Ivory;
                account.ForeColor = Color.Ivory;
                id.ForeColor = Color.Ivory;
                timer.Start();

                return;
            }
            timer.Stop();
            server.Text = "Stop Renewal";
            server.ForeColor = Color.Maroon;
        }
        private void TimerTick(object sender, EventArgs e)
        {
            ConnectAPI api = ConnectAPI.Get();
            api.LookUpTheDeposit(account.Text, api.OnReceiveBalance);
        }
        private bool CheckCurrent
        {
            get
            {
                return server.Checked;
            }
        }
        private int[,] FormSizes
        {
            get; set;
        } =
        {
            { 1650, 920 },
            { 750, 370 },
            { 594, 315 }
        };
        private Strategy strategy;
    }
}