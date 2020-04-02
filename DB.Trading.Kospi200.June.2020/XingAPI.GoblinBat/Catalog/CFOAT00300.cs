﻿using System;
using ShareInvest.Catalog;
using ShareInvest.Catalog.XingAPI;
using ShareInvest.EventHandler;

namespace ShareInvest.XingAPI.Catalog
{
    internal class CFOAT00300 : Query, IOrders, IMessage<NotifyIconText>, IStates<State>
    {
        internal CFOAT00300() : base()
        {
            Console.WriteLine(GetType().Name);
        }
        protected override void OnReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            base.OnReceiveMessage(bIsSystemError, nMessageCode, szMessage);
            SendMessage?.Invoke(this, new NotifyIconText(szMessage));
        }
        protected override void OnReceiveData(string szTrCode)
        {
            var enumerable = GetOutBlocks();
            var temp = new string[enumerable.Count];

            while (enumerable.Count > 0)
            {
                var param = enumerable.Dequeue();

                for (int i = 0; i < GetBlockCount(param.Block); i++)
                    temp[temp.Length - enumerable.Count - 1] = GetFieldData(param.Block, param.Field, i);
            }
            SendState?.Invoke(this, new State(API.OnReceiveBalance, API.SellOrder.Count, API.Quantity, API.BuyOrder.Count, API.AvgPurchase, API.MaxAmount));
        }
        public void QueryExcute(Order order)
        {
            var secret = new Secret();
            string name = GetType().Name, block = string.Empty;

            if (LoadFromResFile(secret.GetResFileName(name)))
            {
                foreach (var param in GetInBlocks(secret.GetData(name, order)))
                {
                    SetFieldData(param.Block, param.Field, param.Occurs, param.Data);

                    if (block.Equals(string.Empty))
                        block = param.Block;
                }
                if (API.SellOrder.ContainsKey(order.OrgOrdNo) || API.BuyOrder.ContainsKey(order.OrgOrdNo))
                    SendErrorMessage(name, Request(false));

                else
                    ClearBlockdata(block);
            }
        }
        public event EventHandler<NotifyIconText> SendMessage;
        public event EventHandler<State> SendState;
    }
}