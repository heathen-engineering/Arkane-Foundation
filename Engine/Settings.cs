﻿using HeathenEngineering.Arkane.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HeathenEngineering.Arkane.Engine
{
    [CreateAssetMenu(menuName = "Arkane/Settings")]
    public class Settings : ScriptableObject
    {
        public static Settings current;

        public DomainTarget Authentication = new DomainTarget("https://login-staging.arkane.network", "https://login.arkane.network");
        public DomainTarget Business = new DomainTarget("https://business-staging.arkane.network", "https://business.arkane.network");
        public DomainTarget API = new DomainTarget("https://api-staging.arkane.network", "https://api.arkane.network");
        
        public bool UseStaging = true;
        public AppId AppId;
        public AuthenticationMode AuthenticationMode = new AuthenticationMode("<client id>", "password");

        public List<Engine.Contract> Contracts = new List<Engine.Contract>();
         
        public Engine.Contract this[int contractIndex] => Contracts?[contractIndex];

        public Engine.Contract this[ulong contractId] => Contracts?.FirstOrDefault(p => p.id == contractId);

        public Engine.Contract this[string contractName] => Contracts?.FirstOrDefault(p => p.systemName == contractName);

        /// <summary>
        /// Used to authenticate the user to the Arkane Network
        /// </summary>
        public string AuthenticationUri { get { return Authentication[UseStaging] + "/auth/realms/Arkane/protocol/openid-connect/token"; } }

        public string ConnectUri 
        { 
            get
            {
                if (UseStaging)
                    return "https://connect.arkane.network";
                else
                    return "https://connect-staging.arkane.network";
            }
        }

        /// <summary>
        /// Used to fetch the authenticated users wallets from the Arkane Network
        /// </summary>
        public string WalletUri { get { return API[UseStaging] + "/api/wallets"; } }

        /// <summary>
        /// Used to work against the business API for deploying, getting and working with Arkane contracts.
        /// </summary>
        public string ContractUri { get { return GetContractUri(AppId); } }

        /// <summary>
        /// Used to work against the business API for listing and working with Arkane applicaitons.
        /// </summary>
        public string AppsUri { get { return Business[UseStaging] + "/api/apps"; } }

        public string DefineTokenTypeUri(Engine.Contract forContract)
        {
            if (forContract == null)
                throw new NullReferenceException("A null contract was provided");
            else
                return ContractUri + "/" + forContract.id.ToString() + "/token-types";
        }

        public string DefineTokenTypeUri(AppId onApp, Engine.Contract forContract)
        {
            if (forContract == null)
                throw new NullReferenceException("A null contract was provided");
            else
                return GetContractUri(onApp) + "/" + forContract.id.ToString() + "/define-token-type";
        }

        public string CreateTokenUri(Engine.Contract forContract)
        {
            if (forContract == null)
                throw new NullReferenceException("A null contract was provided");
            else
                return ContractUri + "/" + forContract.id.ToString() + "/tokens";
        }

        public string CreateTokenUri(AppId onApp, Engine.Contract forContract)
        {
            if (forContract == null)
                throw new NullReferenceException("A null contract was provided");
            else
                return GetContractUri(onApp) + "/" + forContract.id.ToString() + "/tokens";
        }

        public string GetTokenUri(Engine.Contract forContract)
        {
            if (forContract == null)
                throw new NullReferenceException("A null contract was provided");
            else
                return ContractUri + "/" + forContract.id.ToString() + "/token-types";
        }

        public string GetTokenUri(long forContractId)
        {
            return ContractUri + "/" + forContractId.ToString() + "/token-types";
        }

        public string GetTokenUri(AppId onApp, Engine.Contract forContract)
        {
            if (forContract == null)
                throw new NullReferenceException("A null contract was provided");
            else
                return GetContractUri(onApp) + "/" + forContract.id.ToString() + "/token-types";
        }

        public string GetTokenUri(AppId onApp, long forContractId)
        {
            return GetContractUri(onApp) + "/" + forContractId.ToString() + "/token-types";
        }

        /// <summary>
        /// Get the contract uri for a specific ArkaneApp
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public string GetContractUri(AppId app)
        {
            if (app == AppId.Invalid)
                throw new System.InvalidOperationException("Unable to construct a valid contract URI for an invalid app id.");

            return AppsUri + "/" + app.id.ToString() + "/contracts";
        }
    }
}
