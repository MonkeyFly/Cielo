﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Cielo.API.Enums;
using Cielo.API.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cielo.API.Responses
{
    public class NewTransactionResponse
    {
        #region private vars

        [JsonExtensionData]
        private IDictionary<string, object> _response;

        #endregion

        #region ctor

        public NewTransactionResponse()
        {
            _response = new Dictionary<string, object>();
        }

        #endregion
                
        #region methods

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            JObject payment = (JObject)_response["Payment"];
            MerchantOrderId = _response["MerchantOrderId"]?.ToString();
            ProofOfSale = payment["ProofOfSale"]?.ToString();
            Tid = payment["Tid"]?.ToString();
            AuthorizationCode = payment["AuthorizationCode"]?.ToString();
            PaymentId = Guid.Parse(payment["PaymentId"]?.ToString());
            Status = EnumExtension.ToEnum<Status>(payment["Status"]?.ToString());
            ReturnCode = EnumExtension.ToEnum<ReturnCode>(payment["ReturnCode"]?.ToString());
            ReturnMessage = payment["ReturnMessage"]?.ToString();
            AuthenticationUrl = payment["AuthenticationUrl"]?.ToString();
            JObject creditCard = (JObject)payment["CreditCard"];
            Cardtoken = creditCard["CardToken"]?.ToString();
        }

        #endregion

        #region properties

        public string MerchantOrderId { get; private set; }
        public string ProofOfSale { get; private set; }
        public string Tid { get; private set; }
        public string AuthorizationCode { get; private set; }
        public string AuthenticationUrl { get; private set; }
        public Guid PaymentId { get; private set; }
        public Status Status { get; private set; }
        public ReturnCode ReturnCode { get; private set; }
        public string ReturnMessage { get; private set; }
        public string Cardtoken { get; private set; }

        #endregion
    }
}
