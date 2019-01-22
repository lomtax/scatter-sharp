﻿using Newtonsoft.Json;
using ScatterSharp;
using ScatterSharp.Core.Storage;
using ScatterSharp.Core.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EosSharp.Core.Api.v1;
using ScatterSharp.UnitTests;

public class TestScatterScript : MonoBehaviour
{
    public async void PushTransaction()
    {
        try
        {
            var network = new ScatterSharp.Core.Api.Network()
            {
                blockchain = Scatter.Blockchains.EOSIO,
                host = "api.jungle.alohaeos.com",
                port = 443,
                protocol = "https",
                chainId = "038f4b0fc8ff18a4f0842a8f0564611f6e96e8535901dd45e43ac8691a1c4dca"
            };

            var fileStorage = new FileStorageProvider(Application.persistentDataPath + "/scatterapp.dat");

            using (var scatter = new Scatter("UNITY-SCATTER-JUNGLE", network, fileStorage))
            {
                await scatter.Connect();

                await scatter.GetIdentity(new IdentityRequiredFields()
                {
                    accounts = new List<ScatterSharp.Core.Api.Network>()
                    {
                        network
                    },
                    location = new List<LocationFields>(),
                    personal = new List<PersonalFields>()
                });

                var eos = scatter.Eos();
                
                var account = scatter.Identity.accounts.First();

                var result = await eos.CreateTransaction(new Transaction()
                {
                    actions = new List<EosSharp.Core.Api.v1.Action>()
                    {
                        new EosSharp.Core.Api.v1.Action()
                        {
                            account = "eosio.token",
                            authorization =  new List<PermissionLevel>()
                            {
                                new PermissionLevel() {actor = account.name, permission = account.authority }
                            },
                            name = "transfer",
                            data = new { from = account.name, to = "eosio", quantity = "0.0001 EOS", memo = "Unity 3D hello crypto world!" }
                        }
                    }
                });

                print(result);
            }
        }
        catch (Exception ex)
        {
            print(JsonConvert.SerializeObject(ex));
        }
    }

    public async void TestScatterConnect()
    {
        print("test connect");

        var network = new ScatterSharp.Core.Api.Network()
        {
            blockchain = Scatter.Blockchains.EOSIO,
            host = "api.jungle.alohaeos.com",
            port = 443,
            protocol = "https",
            chainId = "038f4b0fc8ff18a4f0842a8f0564611f6e96e8535901dd45e43ac8691a1c4dca"
        };

        var fileStorage = new FileStorageProvider(Application.persistentDataPath + "/scatterapp.dat");

        using (var scatter = new Scatter("UNITY-SCATTER-JUNGLE", network, fileStorage))
        {
            print("connecting");

            try
            {
                await scatter.Connect();
            }
            catch (Exception ex)
            {
                print(JsonConvert.SerializeObject(ex));
            }

            print("connected");
        }
    }
}
