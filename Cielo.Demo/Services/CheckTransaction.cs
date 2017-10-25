﻿using System;
using System.Configuration;
using System.Diagnostics;
using Cielo.API;
using Cielo.API.Configuration;
using Cielo.API.Responses.Exceptions;

namespace Cielo.Demo.Services
{
    public class CheckTransaction : ICieloDemo
    {
        private ICieloDemoInterface _app;

        public static CieloDemoInfo Info
        {
            get
            {
                var info = new CieloDemoInfo(
                    typeof(CheckTransaction),
                    "check",
                    "Check a transaction",
                    "");
                return info;
            }
        }

        #region ICieloDemo Members

        public void Execute(ICieloDemoInterface app)
        {
            this._app = app;
            this.GetArgs();
            this.ExecuteRequest();
        }

        #endregion

        private void GetArgs()
        {
            foreach (var arg in _app.Args)
            {

            }
        }

        private void ExecuteRequest()
        {
            var watch = Stopwatch.StartNew();
            ConsoleHelper.WriteHeader("Checking a Transaction");

            CustomConfiguration configuration = new CustomConfiguration()
            {
                DefaultEndpoint = ConfigurationManager.AppSettings["cielo.endpoint.default"],
                QueryEndpoint = ConfigurationManager.AppSettings["cielo.endpoint.query"],
                MerchantId = ConfigurationManager.AppSettings["cielo.customer.id"],
                MerchantKey = ConfigurationManager.AppSettings["cielo.customer.key"],
                ReturnUrl = ConfigurationManager.AppSettings["cielo.return.url"],
            };

            CieloService cieloService = new CieloService(configuration);

            try
            {
                var response = cieloService.CheckTransaction(paymentId: Guid.Parse("1a2d178a-dc87-4627-92a2-b5d18ba076cd"));

                ConsoleHelper.WriteResult(response);
            }
            catch (ResponseException ex)
            {
                ConsoleHelper.WriteError(ex);
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteError(ex);
            }

            watch.Stop();
            ConsoleHelper.WriteFooter(watch.ElapsedMilliseconds);
        }
    }
}
